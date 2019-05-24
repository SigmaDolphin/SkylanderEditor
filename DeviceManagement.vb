Option Strict On
Option Explicit On 
Imports System.Runtime.InteropServices

''' <summary>
''' For detecting devices and receiving device notifications.
''' </summary>

Partial Friend NotInheritable Class DeviceManagement

    ' Used in error messages:

    Const MODULE_NAME As String = "DeviceManagement"

    ' For viewing results of API calls in debug.write statements:

	''' <summary>
	''' Compares two device path names. Used to find out if the device name 
	''' of a recently attached or removed device matches the name of a 
	''' device the application is communicating with.
	''' </summary>
	''' 
	''' <param name="m"> a WM_DEVICECHANGE message. A call to RegisterDeviceNotification
	''' causes WM_DEVICECHANGE messages to be passed to an OnDeviceChange routine.. </param>
	''' <param name="mydevicePathName"> a device pathname returned by 
	''' SetupDiGetDeviceInterfaceDetail in an SP_DEVICE_INTERFACE_DETAIL_DATA structure. </param>
	''' 
	''' <returns>
	''' True if the names match, False if not.
	''' </returns>
	''' 
	Friend Function DeviceNameMatch _
	 (ByVal m As Message, _
	 ByVal mydevicePathName As String) _
	 As Boolean

		Dim deviceNameString As String = ""
		Dim stringSize As Int32

		Try
			Dim devBroadcastDeviceInterface As New DEV_BROADCAST_DEVICEINTERFACE_1()
			Dim devBroadcastHeader As New DEV_BROADCAST_HDR()

			'The LParam parameter of Message is a pointer to a DEV_BROADCAST_HDR structure.

			Marshal.PtrToStructure(m.LParam, devBroadcastHeader)

			If (devBroadcastHeader.dbch_devicetype = DBT_DEVTYP_DEVICEINTERFACE) Then

				'The dbch_devicetype parameter indicates that the event applies to a device interface.
				'So the structure in LParam is actually a DEV_BROADCAST_DEVICEINTERFACE structure, 
				'which begins with a DEV_BROADCAST_HDR.

				'Obtain the number of characters in dbch_name by subtracting the 32 bytes
				'in the strucutre that are not part of dbch_name and dividing by 2 because there are 
				'2 bytes per character.

				stringSize = Convert.ToInt32((devBroadcastHeader.dbch_size - 32) / 2)

				'The dbcc_name parameter of DevBroadcastDeviceInterface contains the device name. 
				'Trim dbcc_name to match the size of the string.

				'ReDim devBroadcastDeviceInterface.dbcc_name(stringSize)
				Array.Resize(devBroadcastDeviceInterface.dbcc_name, stringSize)
				'Marshal data from the unmanaged block pointed to by m.LParam 
				'to the managed object DevBroadcastDeviceInterface.

				Marshal.PtrToStructure(m.LParam, devBroadcastDeviceInterface)

				'Store the device name in a String.

				deviceNameString = New String(devBroadcastDeviceInterface.dbcc_name, 0, stringSize)

				'Compare the name of the newly attached device with the name of the device 
				'the application is accessing (mydevicePathName).
				'Set ignorecase True.

				If (String.Compare(deviceNameString, mydevicePathName, True) = 0) Then
					Return True
				Else
					Return False
				End If
			End If

		Catch ex As Exception
			Throw
		End Try

	End Function

	''' <summary>
	''' Use SetupDi API functions to retrieve the device path name of an
	''' attached device that belongs to a device interface class.
	''' </summary>
	''' 
	''' <param name="myGuid"> an interface class GUID. </param>
	''' <param name="devicePathName"> a pointer to the device path name 
	''' of an attached device. </param>
	''' 
	''' <returns>
	'''  True if a device is found, False if not. 
	''' </returns>

	Friend Function FindDeviceFromGuid _
	 (ByVal myGuid As System.Guid, _
	 ByRef devicePathName() As String) _
	 As Boolean

		Dim bufferSize As Int32
		Dim detailDataBuffer As IntPtr
		Dim deviceFound As Boolean
		Dim deviceInfoSet As IntPtr
		Dim lastDevice As Boolean
		Dim memberIndex As Int32
		Dim myDeviceInterfaceData As SP_DEVICE_INTERFACE_DATA
        Dim pdevicePathName As IntPtr
		Dim success As Boolean

		Try
			'***
			' API function

			' summary 
			' Retrieves a device information set for a specified group of devices.
			' SetupDiEnumDeviceInterfaces uses the device information set.

			' parameters 
			' Interface class GUID.
			' Null to retrieve information for all device instances.
			' Optional handle to a top-level window (unused here).
			' Flags to limit the returned information to currently present devices 
			' and devices that expose interfaces in the class specified by the GUID.

			' Returns
			' Handle to a device information set for the devices.
            '*

            deviceInfoSet = SetupDiGetClassDevs _
             (myGuid, _
             IntPtr.Zero, _
             IntPtr.Zero, _
             DIGCF_PRESENT Or DIGCF_DEVICEINTERFACE)

			deviceFound = False
			memberIndex = 0

			'The cbSize element of the MyDeviceInterfaceData structure must be set to
			'the structure's size in bytes. 
			' The size is 28 bytes for 32-bit code and 32 bits for 64-bit code.

			myDeviceInterfaceData.cbSize = Marshal.SizeOf(myDeviceInterfaceData)

			Do

				'Begin with 0 and increment through the device information set until
				'no more devices are available.		

				'***
				' API function

				' summary
				' Retrieves a handle to a SP_DEVICE_INTERFACE_DATA structure for a device.
				' On return, MyDeviceInterfaceData contains the handle to a
				' SP_DEVICE_INTERFACE_DATA structure for a detected device.

				' parameters
				' DeviceInfoSet returned by SetupDiGetClassDevs.
				' Optional SP_DEVINFO_DATA structure that defines a device instance 
				' that is a member of a device information set.
				' Device interface GUID.
				' Index to specify a device in a device information set.
				' Pointer to a handle to a SP_DEVICE_INTERFACE_DATA structure for a device.

				' Returns
				' True on success.
				'***

				success = SetupDiEnumDeviceInterfaces _
				 (deviceInfoSet, _
				 IntPtr.Zero, _
				 myGuid, _
				 memberIndex, _
				 myDeviceInterfaceData)

				'Find out if a device information set was retrieved.

				If Not (success) Then
					lastDevice = True

				Else
					'A device is present.

					'***
					' API function: 

					' summary:
					' Retrieves an SP_DEVICE_INTERFACE_DETAIL_DATA structure
					' containing information about a device.
					' To retrieve the information, call this function twice.
					' The first time returns the size of the structure.
					' The second time returns a pointer to the data.

					' parameters
					' DeviceInfoSet returned by SetupDiGetClassDevs
					' SP_DEVICE_INTERFACE_DATA structure returned by SetupDiEnumDeviceInterfaces
					' A returned pointer to an SP_DEVICE_INTERFACE_DETAIL_DATA 
					' Structure to receive information about the specified interface.
					' The size of the SP_DEVICE_INTERFACE_DETAIL_DATA structure.
					' Pointer to a variable that will receive the returned required size of the 
					' SP_DEVICE_INTERFACE_DETAIL_DATA structure.
					' Returned pointer to an SP_DEVINFO_DATA structure to receive information about the device.

					' Returns
					' True on success.
					'***

					SetupDiGetDeviceInterfaceDetail _
					 (deviceInfoSet, _
					 myDeviceInterfaceData, _
					 IntPtr.Zero, _
					 0, _
					 bufferSize, _
					 IntPtr.Zero)

					'Allocate memory for the SP_DEVICE_INTERFACE_DETAIL_DATA structure using the returned buffer size.

					detailDataBuffer = Marshal.AllocHGlobal(bufferSize)

					'Store cbSize in the first bytes of the array. Use If Ternary operator to handle 32- and 64-bit systems.

					Marshal.WriteInt32(detailDataBuffer, Convert.ToInt32(IIf((IntPtr.Size = 4), 4 + Marshal.SystemDefaultCharSize, 8)))

					'Call SetupDiGetDeviceInterfaceDetail again.
					'This time, pass a pointer to DetailDataBuffer
					'and the returned required buffer size.

					success = SetupDiGetDeviceInterfaceDetail _
					 (deviceInfoSet, _
					 myDeviceInterfaceData, _
					 detailDataBuffer, _
					 bufferSize, _
					 bufferSize, _
					 IntPtr.Zero)

					'Skip over cbsize (4 bytes) to get the address of the devicePathName.

					pdevicePathName = New IntPtr(detailDataBuffer.ToInt32 + 4)

					'Get the String containing the devicePathName.

					devicePathName(memberIndex) = Marshal.PtrToStringAuto(pdevicePathName)
					
					deviceFound = True

				End If

				memberIndex = memberIndex + 1

			Loop Until (lastDevice = True)

			Return deviceFound

		Catch ex As Exception
			Throw

		Finally

			'Free the memory allocated previously by AllocHGlobal.

			If Not (detailDataBuffer = IntPtr.Zero) Then
				Marshal.FreeHGlobal(detailDataBuffer)
			End If

			'***
			' API function

			' summary
			' Frees the memory reserved for the DeviceInfoSet returned by SetupDiGetClassDevs.

			' parameters
			' DeviceInfoSet returned by SetupDiGetClassDevs.

			' returns
			' True on success.
			'***
			If Not (deviceInfoSet = IntPtr.Zero) Then
				SetupDiDestroyDeviceInfoList(deviceInfoSet)
			End If

		End Try

	End Function

	''' <summary>
	''' Requests to receive a notification when a device is attached or removed.
	''' </summary>
	''' 
	''' <param name="devicePathName"> handle to a device. </param>
	''' <param name="formHandle"> handle to the window that will receive device events. </param>
	''' <param name="classGuid"> device interface GUID. </param>
	''' <param name="deviceNotificationHandle"> returned device notification handle. </param>
	''' 
	''' <returns>
	''' True on success.
	''' </returns>
	''' 
	Friend Function RegisterForDeviceNotifications _
	 (ByVal devicePathName As String, _
	 ByVal formHandle As IntPtr, _
	 ByVal classGuid As Guid, _
	 ByRef deviceNotificationHandle As IntPtr) _
	 As Boolean

		'A DEV_BROADCAST_DEVICEINTERFACE header holds information about the request.

		Dim devBroadcastDeviceInterface As DEV_BROADCAST_DEVICEINTERFACE = _
		 New DEV_BROADCAST_DEVICEINTERFACE()
		Dim devBroadcastDeviceInterfaceBuffer As IntPtr
		Dim size As Int32

		Try
			'Set the parameters in the DEV_BROADCAST_DEVICEINTERFACE structure.

			'Set the size.

			size = Marshal.SizeOf(devBroadcastDeviceInterface)
			devBroadcastDeviceInterface.dbcc_size = size

			'Request to receive notifications about a class of devices.

			devBroadcastDeviceInterface.dbcc_devicetype = DBT_DEVTYP_DEVICEINTERFACE

			devBroadcastDeviceInterface.dbcc_reserved = 0

			'Specify the interface class to receive notifications about.

			devBroadcastDeviceInterface.dbcc_classguid = classGuid

			'Allocate memory for the buffer that holds the DEV_BROADCAST_DEVICEINTERFACE structure.

			devBroadcastDeviceInterfaceBuffer = Marshal.AllocHGlobal(size)

			'Copy the DEV_BROADCAST_DEVICEINTERFACE structure to the buffer.
			'Set fDeleteOld True to prevent memory leaks.

			Marshal.StructureToPtr _
			 (devBroadcastDeviceInterface, devBroadcastDeviceInterfaceBuffer, True)

			'***
			' API function

			' summary
			' Request to receive notification messages when a device in an interface class
			' is attached or removed.

			' parameters 
			' Handle to the window that will receive device events.
			' Pointer to a DEV_BROADCAST_DEVICEINTERFACE to specify the type of 
			' device to send notifications for.
			' DEVICE_NOTIFY_WINDOW_HANDLE indicates the handle is a window handle.

			' Returns
			' Device notification handle or NULL on failure.
			'***

			deviceNotificationHandle = RegisterDeviceNotification _
			 (formHandle, _
			 devBroadcastDeviceInterfaceBuffer, _
			 DEVICE_NOTIFY_WINDOW_HANDLE)

			'Marshal data from the unmanaged block DevBroadcastDeviceInterfaceBuffer to
			'the managed object DevBroadcastDeviceInterface

			Marshal.PtrToStructure(devBroadcastDeviceInterfaceBuffer, devBroadcastDeviceInterface)

			

			If (deviceNotificationHandle.ToInt32 = IntPtr.Zero.ToInt32) Then
				Return False
			Else
				Return True
			End If

		Catch ex As Exception
			Throw

		Finally
			'Free the memory allocated previously by AllocHGlobal.

			If Not (devBroadcastDeviceInterfaceBuffer = IntPtr.Zero) Then
				Marshal.FreeHGlobal(devBroadcastDeviceInterfaceBuffer)
			End If

		End Try
	End Function

	''' <summary>
	''' Requests to stop receiving notification messages when a device in an
	''' interface class is attached or removed.
	''' </summary>
	''' 
	''' <param name="deviceNotificationHandle"> handle returned previously by
	''' RegisterDeviceNotification. </param>

	Friend Sub StopReceivingDeviceNotifications _
	 (ByVal deviceNotificationHandle As IntPtr)

		Try
			'***
			' API function

			' summary
			' Stop receiving notification messages.

			' parameters
			' Handle returned previously by RegisterDeviceNotification.  

			' returns
			' True on success.
			'***

			' Ignore failures.

			UnregisterDeviceNotification(deviceNotificationHandle)

		Catch ex As Exception
			Throw
		End Try

	End Sub

End Class

