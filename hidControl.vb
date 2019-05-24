Option Explicit On

Imports Microsoft.Win32.SafeHandles
Imports SkylanderEditor.Hid
Imports SkylanderEditor.FileIO
Imports SkylanderEditor.DeviceManagement
Imports System.Runtime.InteropServices
Imports System.IO

Module hidControl

    Dim deviceStream As FileStream
    Dim MyHid As New Hid()
    Dim MyDeviceManagement As New DeviceManagement()
    Dim deviceNotificationHandle As IntPtr
    Dim myDevicePathName As String

    Const reportSize = 33


    'gets the handle of Portal of Power connected to the computer
    Public Function FindThePortal() As SafeFileHandle

        Dim deviceFound As Boolean
        Dim devicePathName(127) As String
        Dim hidGuid As System.Guid
        Dim memberIndex As Int32
        Dim myProductID As Int32
        Dim myVendorID As Int32
        Dim success As Boolean
        Dim preparsedData As IntPtr
        Dim myDeviceDetected As Boolean
        Dim hidHandle As SafeFileHandle

        myDeviceDetected = False

        ' Get the device's Vendor ID and Product ID from the form's text boxes.

        myVendorID = 5168
        myProductID = 336

        ' ***
        ' API function: 'HidD_GetHidGuid

        ' Purpose: Retrieves the interface class GUID for the HID class.

        ' Accepts: 'A System.Guid object for storing the GUID.
        ' ***

        HidD_GetHidGuid(hidGuid)

        ' Fill an array with the device path names of all attached HIDs.

        deviceFound = MyDeviceManagement.FindDeviceFromGuid _
         (hidGuid, _
         devicePathName)

        ' If there is at least one HID, attempt to read the Vendor ID and Product ID
        ' of each device until there is a match or all devices have been examined.

        If deviceFound Then

            memberIndex = 0

            Do
                ' ***
                ' API function:
                ' CreateFile

                ' Purpose:
                ' Retrieves a handle to a device.

                ' Accepts:
                ' A device path name returned by SetupDiGetDeviceInterfaceDetail
                ' The type of access requested (read/write).
                ' FILE_SHARE attributes to allow other processes to access the device while this handle is open.
                ' A Security structure or IntPtr.Zero. 
                ' A creation disposition value. Use OPEN_EXISTING for devices.
                ' Flags and attributes for files. Not used for devices.
                ' Handle to a template file. Not used.

                ' Returns: a handle without read or write access.
                ' This enables obtaining information about all HIDs, even system
                ' keyboards and mice. 
                ' Separate handles are used for reading and writing.
                ' ***

                ' Open the handle without read/write access to enable getting information about any HID, even system keyboards and mice.

                hidHandle = CreateFile _
                 (devicePathName(memberIndex), _
                 0, _
                 FILE_SHARE_READ Or FILE_SHARE_WRITE, _
                 IntPtr.Zero, _
                 OPEN_EXISTING, _
                 0, _
                 0)


                If Not (hidHandle.IsInvalid) Then

                    ' The returned handle is valid, 
                    ' so find out if this is the device we're looking for.

                    ' Set the Size property of DeviceAttributes to the number of bytes in the structure.

                    MyHid.DeviceAttributes.Size = Marshal.SizeOf(MyHid.DeviceAttributes)

                    ' ***
                    ' API function:
                    ' HidD_GetAttributes

                    ' Purpose:
                    ' Retrieves a HIDD_ATTRIBUTES structure containing the Vendor ID, 
                    ' Product ID, and Product Version Number for a device.

                    ' Accepts:
                    ' A handle returned by CreateFile.
                    ' A pointer to receive a HIDD_ATTRIBUTES structure.

                    ' Returns:
                    ' True on success, False on failure.
                    ' ***

                    success = HidD_GetAttributes(hidHandle, MyHid.DeviceAttributes)

                    If success Then

                        ' Find out if the device matches the one we're looking for.

                        If (MyHid.DeviceAttributes.VendorID = myVendorID) And _
                         (MyHid.DeviceAttributes.ProductID = myProductID) Then


                            ' Display the information in form's list box.

                            myDeviceDetected = True

                            ' Save the DevicePathName for OnDeviceChange().

                            myDevicePathName = devicePathName(memberIndex)
                        Else

                            ' It's not a match, so close the handle.

                            myDeviceDetected = False

                            hidHandle.Close()

                        End If

                    Else
                        ' There was a problem in retrieving the information.

                        myDeviceDetected = False
                        hidHandle.Close()
                    End If

                End If

                ' Keep looking until we find the device or there are no devices left to examine.

                memberIndex = memberIndex + 1

            Loop Until (myDeviceDetected Or (memberIndex = devicePathName.Length))

        End If

        If myDeviceDetected Then

            ' The device was detected.
            ' Register to receive notifications if the device is removed or attached.

            MyDeviceManagement.RegisterForDeviceNotifications _
             (myDevicePathName, _
             Form1.Handle, _
             hidGuid, _
             deviceNotificationHandle)


            ' Learn the capabilities of the device.

            HidD_GetPreparsedData(hidHandle, preparsedData)
            HidP_GetCaps(preparsedData, MyHid.Capabilities)
            If Not (preparsedData = IntPtr.Zero) Then
                HidD_FreePreparsedData(preparsedData)
            End If


            ' Get the Input report buffer size.

            'GetInputReportBufferSize()

            'Close the handle and reopen it with read/write access.

            hidHandle.Close()

            hidHandle = CreateFile _
             (myDevicePathName, _
             GENERIC_READ Or GENERIC_WRITE, _
             FILE_SHARE_READ Or FILE_SHARE_WRITE, _
             IntPtr.Zero, _
             OPEN_EXISTING, _
             0, _
             0)

            deviceStream = New FileStream(hidHandle, FileAccess.Read Or FileAccess.Write, reportSize, False)


            ' Flush any waiting reports in the input buffer. (optional)

            HidD_FlushQueue(hidHandle)
            Form1.unlockPortalControls()
            Form1.ToolStripStatusLabel1.Text = "Portal Connected!"

        Else
            ' The device wasn't detected.
            Form1.lockPortalControls()
            Form1.ToolStripStatusLabel1.Text = "Portal Not Found!"

        End If

        Return hidHandle

    End Function


    'send a report to the portal, they are 33 bytes long
    Public Sub outputReport(ByVal hidHandle As SafeFileHandle, ByRef outReport As Byte())
        HidD_SetOutputReport(hidHandle, outReport(0), reportSize)
    End Sub

    'receive a report from the portal into the specified array, reports are 33 bytes long
    Public Sub inputReport(ByVal hidHandle As SafeFileHandle, ByRef inReport As Byte())
        If (deviceStream.CanRead) Then
            deviceStream.Read(inReport, 0, reportSize)
        End If

    End Sub

    'this flushes the input reports from the portal, we need to clear it before reading
    Public Sub flushHid(ByVal hidHandle As SafeFileHandle)
        HidD_FlushQueue(hidHandle)
    End Sub

    'cleanup function to close the handles to the Portal
    Public Sub CloseCommunications(ByRef hidHandle As SafeFileHandle)

        If (Not (deviceStream Is Nothing)) Then

            deviceStream.Close()
        End If

        If (Not (hidHandle Is Nothing)) Then
            If (Not hidHandle.IsInvalid) Then
                hidHandle.Close()
            End If
        End If

        MyDeviceManagement.StopReceivingDeviceNotifications(deviceNotificationHandle)

    End Sub

    'function to check if the device removed is the previously openned portal
    Public Function checkDevice(ByRef m As Message) As Boolean
        checkDevice = MyDeviceManagement.DeviceNameMatch(m, myDevicePathName)
    End Function
End Module
