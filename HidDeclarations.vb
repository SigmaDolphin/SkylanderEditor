Option Strict On
Option Explicit On

Imports Microsoft.Win32.SafeHandles
Imports System.Runtime.InteropServices


Partial Friend NotInheritable Class Hid

    ' API declarations for HID communications.

    ' from hidpi.h
    ' Typedef enum defines a set of integer constants for HidP_Report_Type

	Friend Const HidP_Input As Int16 = 0
	Friend Const HidP_Output As Int16 = 1
	Friend Const HidP_Feature As Int16 = 2

	Friend Structure HIDD_ATTRIBUTES
		Friend Size As Int32
		Friend VendorID As UInt16
		Friend ProductID As UInt16
		Friend VersionNumber As UInt16
	End Structure

	Friend Structure HIDP_CAPS
		Friend Usage As Int16
		Friend UsagePage As Int16
		Friend InputReportByteLength As Int16
		Friend OutputReportByteLength As Int16
		Friend FeatureReportByteLength As Int16
		<MarshalAs(UnmanagedType.ByValArray, SizeConst:=17)> Dim Reserved() As Int16
		Friend NumberLinkCollectionNodes As Int16
		Friend NumberInputButtonCaps As Int16
		Friend NumberInputValueCaps As Int16
		Friend NumberInputDataIndices As Int16
		Friend NumberOutputButtonCaps As Int16
		Friend NumberOutputValueCaps As Int16
		Friend NumberOutputDataIndices As Int16
		Friend NumberFeatureButtonCaps As Int16
		Friend NumberFeatureValueCaps As Int16
		Friend NumberFeatureDataIndices As Int16

    End Structure

    Friend Capabilities As HIDP_CAPS
    Friend DeviceAttributes As HIDD_ATTRIBUTES

    ' If IsRange is false, UsageMin is the Usage and UsageMax is unused.
    ' If IsStringRange is false, StringMin is the string index and StringMax is unused.
    ' If IsDesignatorRange is false, DesignatorMin is the designator index and DesignatorMax is unused.

	Friend Structure HidP_Value_Caps
		Friend UsagePage As Int16
		Friend ReportID As Byte
		Friend IsAlias As Int32
		Friend BitField As Int16
		Friend LinkCollection As Int16
		Friend LinkUsage As Int16
		Friend LinkUsagePage As Int16
		Friend IsRange As Int32
		Friend IsStringRange As Int32
		Friend IsDesignatorRange As Int32
		Friend IsAbsolute As Int32
		Friend HasNull As Int32
		Friend Reserved As Byte
		Friend BitSize As Int16
		Friend ReportCount As Int16
		Friend Reserved2 As Int16
		Friend Reserved3 As Int16
		Friend Reserved4 As Int16
		Friend Reserved5 As Int16
		Friend Reserved6 As Int16
		Friend LogicalMin As Int32
		Friend LogicalMax As Int32
		Friend PhysicalMin As Int32
		Friend PhysicalMax As Int32
		Friend UsageMin As Int16
		Friend UsageMax As Int16
		Friend StringMin As Int16
		Friend StringMax As Int16
		Friend DesignatorMin As Int16
		Friend DesignatorMax As Int16
		Friend DataIndexMin As Int16
		Friend DataIndexMax As Int16
	End Structure

    <DllImport("hid.dll", SetLastError:=True)> _
    Shared Function HidD_FlushQueue _
        (ByVal HidDeviceObject As SafeFileHandle) _
        As Boolean
    End Function

	<DllImport("hid.dll", SetLastError:=True)> _
	Shared Function HidD_FreePreparsedData _
		(ByVal PreparsedData As IntPtr) _
		As Boolean
	End Function

	<DllImport("hid.dll", SetLastError:=True)> _
	Shared Function HidD_GetAttributes _
	 (ByVal HidDeviceObject As SafeFileHandle, _
	 ByRef Attributes As HIDD_ATTRIBUTES) _
	 As Boolean
	End Function

	<DllImport("hid.dll", SetLastError:=True)> _
	Shared Function HidD_GetFeature _
		(ByVal HidDeviceObject As SafeFileHandle, _
		ByVal lpReportBuffer() As Byte, _
		ByVal ReportBufferLength As Int32) _
		As Boolean
	End Function

	<DllImport("hid.dll", SetLastError:=True)> _
	Shared Function HidD_GetInputReport _
		(ByVal HidDeviceObject As SafeFileHandle, _
		ByVal lpReportBuffer() As Byte, _
		ByVal ReportBufferLength As Int32) _
		As Boolean
	End Function

    <DllImport("hid.dll", SetLastError:=True)> _
    Shared Sub HidD_GetHidGuid _
        (ByRef HidGuid As System.Guid)
    End Sub

    <DllImport("hid.dll", SetLastError:=True)> _
    Shared Function HidD_GetNumInputBuffers _
        (ByVal HidDeviceObject As SafeFileHandle, _
        ByRef NumberBuffers As Int32) _
        As Boolean
    End Function

	<DllImport("hid.dll", SetLastError:=True)> _
	Shared Function HidD_GetPreparsedData _
	 (ByVal HidDeviceObject As SafeFileHandle, _
	 ByRef PreparsedData As IntPtr) _
	 As Boolean
	End Function

	<DllImport("hid.dll", SetLastError:=True)> _
	Shared Function HidD_SetFeature _
		(ByVal HidDeviceObject As SafeFileHandle, _
		ByVal lpReportBuffer() As Byte, _
		ByVal ReportBufferLength As Int32) _
		As Boolean
	End Function

    <DllImport("hid.dll", SetLastError:=True)> _
    Shared Function HidD_SetNumInputBuffers _
        (ByVal HidDeviceObject As SafeFileHandle, _
        ByVal NumberBuffers As Int32) _
        As Boolean
    End Function

    <DllImport("hid.dll", SetLastError:=True)> _
    Shared Function HidD_SetOutputReport _
        (ByVal HidDeviceObject As SafeFileHandle, _
        ByRef lpReportBuffer As Byte, _
        ByVal ReportBufferLength As Int32) _
        As Boolean
    End Function

    <DllImport("hid.dll", SetLastError:=True)> _
    Shared Function HidP_GetCaps _
        (ByVal PreparsedData As IntPtr, _
        ByRef Capabilities As HIDP_CAPS) _
        As Int32
    End Function

	<DllImport("hid.dll", SetLastError:=True)> _
	Shared Function HidP_GetValueCaps _
	 (ByVal ReportType As Int32, _
	 ByVal ValueCaps As Byte(), _
	 ByRef ValueCapsLength As Int32, _
	 ByVal PreparsedData As IntPtr) _
	 As Int32
	End Function

End Class
