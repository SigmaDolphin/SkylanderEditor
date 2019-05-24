Option Strict On
Option Explicit On 
Imports System.Runtime.InteropServices

Partial Friend NotInheritable Class DeviceManagement

	'''<remarks>
	''' API declarations relating to device management (SetupDixxx and 
	''' RegisterDeviceNotification functions).
	''' Constants are from dbt.h and setupapi.h.
	'''</remarks>

	Friend Const DBT_DEVICEARRIVAL As Int32 = &H8000
	Friend Const DBT_DEVICEREMOVECOMPLETE As Int32 = &H8004
	Friend Const DBT_DEVTYP_DEVICEINTERFACE As Int32 = 5
	Friend Const DBT_DEVTYP_HANDLE As Int32 = 6
	Friend Const DEVICE_NOTIFY_ALL_INTERFACE_CLASSES As Int32 = 4
	Friend Const DEVICE_NOTIFY_SERVICE_HANDLE As Int32 = 1
	Friend Const DEVICE_NOTIFY_WINDOW_HANDLE As Int32 = 0
	Friend Const WM_DEVICECHANGE As Int32 = &H219

	Friend Const DIGCF_PRESENT As Int32 = 2
	Friend Const DIGCF_DEVICEINTERFACE As Int32 = &H10

	'Two declarations for the DEV_BROADCAST_DEVICEINTERFACE structure.

	'Use this one in the call to RegisterDeviceNotification() and
	'in checking dbch_devicetype in a DEV_BROADCAST_HDR structure:

	<StructLayout(LayoutKind.Sequential)> _
	Friend Class DEV_BROADCAST_DEVICEINTERFACE
		Friend dbcc_size As Int32
		Friend dbcc_devicetype As Int32
		Friend dbcc_reserved As Int32
		Friend dbcc_classguid As Guid
		Friend dbcc_name As Int16
	End Class

	'Use this to read the dbcc_name string and classguid:

	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	   Friend Class DEV_BROADCAST_DEVICEINTERFACE_1
		Friend dbcc_size As Int32
		Friend dbcc_devicetype As Int32
		Friend dbcc_reserved As Int32
		<MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U1, SizeConst:=16)> _
		Friend dbcc_classguid() As Byte
		<MarshalAs(UnmanagedType.ByValArray, sizeconst:=255)> _
		Friend dbcc_name() As Char
	End Class

	<StructLayout(LayoutKind.Sequential)> _
	Friend Class DEV_BROADCAST_HDR
		Friend dbch_size As Int32
		Friend dbch_devicetype As Int32
		Friend dbch_reserved As Int32
	End Class

	Friend Structure SP_DEVICE_INTERFACE_DATA
		Friend cbSize As Int32
		Friend InterfaceClassGuid As System.Guid
		Friend Flags As Int32
		Friend Reserved As IntPtr
	End Structure

	Friend Structure SP_DEVICE_INTERFACE_DETAIL_DATA
		Friend cbSize As Int32
		Friend DevicePath As String
	End Structure

	Friend Structure SP_DEVINFO_DATA
		Friend cbSize As Int32
		Friend ClassGuid As System.Guid
		Friend DevInst As Int32
		Friend Reserved As Int32
	End Structure

	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> Shared Function RegisterDeviceNotification _
	 (ByVal hRecipient As IntPtr, _
	 ByVal NotificationFilter As IntPtr, _
	 ByVal Flags As Int32) _
	 As IntPtr
	End Function

	<DllImport("setupapi.dll", SetLastError:=True)> Shared Function SetupDiCreateDeviceInfoList _
	 (ByRef ClassGuid As System.Guid, _
	 ByVal hwndParent As Int32) _
	 As Int32
	End Function

	<DllImport("setupapi.dll", SetLastError:=True)> Shared Function SetupDiDestroyDeviceInfoList _
	 (ByVal DeviceInfoSet As IntPtr) _
	 As Int32
	End Function

	<DllImport("setupapi.dll", SetLastError:=True)> Shared Function SetupDiEnumDeviceInterfaces _
	 (ByVal DeviceInfoSet As IntPtr, _
	 ByVal DeviceInfoData As IntPtr, _
	 ByRef InterfaceClassGuid As System.Guid, _
	 ByVal MemberIndex As Int32, _
	 ByRef DeviceInterfaceData As SP_DEVICE_INTERFACE_DATA) _
	 As Boolean
	End Function

	<DllImport("setupapi.dll", CharSet:=CharSet.Auto, SetLastError:=True)> Shared Function SetupDiGetClassDevs _
	  (ByRef ClassGuid As System.Guid, _
	  ByVal Enumerator As IntPtr, _
	  ByVal hwndParent As IntPtr, _
	  ByVal Flags As Int32) _
	 As IntPtr
	End Function

	<DllImport("setupapi.dll", CharSet:=CharSet.Auto, SetLastError:=True)> Shared Function SetupDiGetDeviceInterfaceDetail _
	 (ByVal DeviceInfoSet As IntPtr, _
	 ByRef DeviceInterfaceData As SP_DEVICE_INTERFACE_DATA, _
	 ByVal DeviceInterfaceDetailData As IntPtr, _
	 ByVal DeviceInterfaceDetailDataSize As Int32, _
	 ByRef RequiredSize As Int32, _
	 ByVal DeviceInfoData As IntPtr) _
	 As Boolean
	End Function

	<DllImport("user32.dll", SetLastError:=True)> Shared Function UnregisterDeviceNotification _
	 (ByVal Handle As IntPtr) _
	As Boolean
	End Function

End Class

