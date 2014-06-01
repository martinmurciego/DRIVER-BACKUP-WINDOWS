Imports DriverBackup__2.DeviceManagement
Imports System.Runtime.InteropServices


Namespace DeviceBackupRestore

    Public Delegate Sub BRBeginEndHandler(ByVal sender As Object, ByVal e As OperationEventArgs)

    Public Delegate Sub BRDeviceProcessingHandler(ByVal sender As Object, ByVal e As DeviceEventArgs)

    Public Delegate Sub BRExceptionHandler(ByVal sender As Object, ByVal e As ExceptionEventArgs)

    Public Delegate Sub BRFileProcessingHandler(ByVal sender As Object, ByVal e As FileEventArgs)

    Public Delegate Sub BRConfirmOperationHandler(ByVal sender As Object, ByVal e As CancelEventArgs)

    Public Enum BackupRestoreErrorCodes
        BRE_Generic ' Errore generico
        BRE_InvalidDevice 'Device non processabile
        BRE_UnformattablePath ' Formato di percorso non risolvibile
        BRE_NoDevices ' Nessun device o computer
        BRE_FileOverwiting 'Sovrascrittura file disabilitata
        BRE_LackOfSpace ' Mancanza di spazio sul computer
        BRE_CantReadWriteBkInfo ' Impossibile scrivere le informazioni di backup
        BRE_FileIOError 'Errore creazione\copia file\directory
        BRE_OpCanceled 'Operazione annullata dall'utente
        BRE_OemInfExist 'Il drivers è già installato sul computer
        BRE_OemInfAlreadyUsed 'Il drivers è correntemente in uso e non può essere disinstallato.
        BRE_MissingInfFile 'Il file di installazione è mancante
        BRE_CantUpdateInfo 'Impossibile aggiornare le informazioni sul percorso
        BRE_ForceUpdate 'Chiede all'utente se l'installazione del driver debba essere forzata
        BRE_CantForceUpdating 'l'aggiornamento del driver non può essere forzato
    End Enum

    Public Class CfgMgr
        'Dll
        Public Declare Unicode Function SetupCopyOEMInf Lib "setupapi.dll" Alias "SetupCopyOEMInfW" (ByVal SourceInfFileName As String, ByVal OEMSourceMediaLocation As String, ByVal OEMSourceMediaType As IntPtr, ByVal CopyStyle As Int32, <Out()> ByVal DestinationInfFileName As StringBuilder, ByVal DestinationInfFileNameSize As Int32, ByRef RequiredSize As Int32, ByVal DestinationInfFileNameComponent As IntPtr) As IntPtr
        Public Const ERROR_FILE_EXISTS As Int32 = 80I
        Public Const SPOST_PATH As Int32 = 1
        Public Const SP_COPY_NOOVERWRITE As Int32 = &H8
        Public Const SP_COPY_REPLACEONLY As Int32 = &H2

        'Funzioni PnP devices manager

        Public Declare Function CM_Connect_Machine Lib "cfgmgr32.dll" Alias "CM_Connect_MachineA" (ByVal machine As IntPtr, ByRef hwnd As IntPtr) As IntPtr
        Public Declare Sub CM_Disconnect_Machine Lib "cfgmgr32.dll" (ByVal hwnd As IntPtr)
        Public Declare Function CM_Locate_DevNode_Ex Lib "cfgmgr32.dll" Alias "CM_Locate_DevNode_ExA" (ByRef devRoot As IntPtr, ByVal deviceId As IntPtr, ByVal flags As IntPtr, ByVal machineH As IntPtr) As IntPtr
        Public Declare Function CM_Locate_DevNode Lib "cfgmgr32.dll" Alias "CM_Locate_DevNodeA" (ByRef devRoot As UInt32, ByVal deviceId As String, ByVal flags As UInt32) As UInt32
        Public Declare Function CM_Reenumerate_DevNode_Ex Lib "cfgmgr32.dll" (ByVal devRoot As IntPtr, ByVal flags As IntPtr, ByVal machine As IntPtr) As IntPtr
        Public Declare Function CM_Get_Device_ID_List_Size Lib "cfgmgr32.dll" Alias "CM_Get_Device_ID_List_SizeA" (ByRef pulLen As UInt32, ByVal pszFilter As UInt32, ByVal uFlags As UInt32) As UInt32
        Public Declare Function CM_Get_Device_ID_List Lib "cfgmgr32.dll" Alias "CM_Get_Device_ID_ListA" (ByVal pszFilter As UInt32, ByVal Buffer As UInt32, ByVal BufferLen As UInt32, ByVal uFlags As UInt32) As UInt32
        Public Declare Function CM_Get_DevNode_Status Lib "cfgmgr32.dll" Alias "CM_Get_DevNode_Status" (ByRef status As UInt32, ByRef problemNumber As UInt32, ByVal DevInst As UInt32, ByVal flags As UInt32) As UInt32

        'Costanti PnP Device Manager

        Public Const CR_SUCCESS = 0
        Public Const CM_LOCATE_DEVNODE_NORMAL = &H0
        Public Const CM_LOCATE_DEVNODE_PHANTOM = &H1
        Public Const CM_LOCATE_DEVNODE_ALL = CM_LOCATE_DEVNODE_NORMAL Or CM_LOCATE_DEVNODE_PHANTOM

        Public Const DN_ROOT_ENUMERATED = &H1  ' Was enumerated by ROOT
        Public Const DN_DRIVER_LOADED = &H2  ' Has Register_Device_Driver
        Public Const DN_ENUM_LOADED = &H4  ' Has Register_Enumerator
        Public Const DN_STARTED = &H8  ' Is currently configured
        Public Const DN_MANUAL = &H10  ' Manually installed
        Public Const DN_NEED_TO_ENUM = &H20  ' May need reenumeration
        Public Const DN_NOT_FIRST_TIME = &H40  ' Has received a config
        Public Const DN_HARDWARE_ENUM = &H80  ' Enum generates hardware ID
        Public Const DN_LIAR = &H100  ' Lied about can reconfig once
        Public Const DN_HAS_MARK = &H200  ' Not CM_Create_DevInst lately
        Public Const DN_HAS_PROBLEM = &H400  ' Need device installer
        Public Const DN_FILTERED = &H800  ' Is filtered
        Public Const DN_MOVED = &H1000  ' Has been moved
        Public Const DN_DISABLEABLE = &H2000  ' Can be disabled
        Public Const DN_REMOVABLE = &H4000  ' Can be removed
        Public Const DN_PRIVATE_PROBLEM = &H8000  ' Has a private problem
        Public Const DN_MF_PARENT = &H10000  ' Multi function parent
        Public Const DN_MF_CHILD = &H20000  ' Multi function child
        Public Const DN_WILL_BE_REMOVED = &H40000  ' DevInst is being removed
        Public Const DN_NEEDS_LOCKING = &H2000000  ' S: Devnode need lock resume processing
        Public Const DN_ARM_WAKEUP = &H4000000  ' S: Devnode can be the wakeup device
        Public Const DN_APM_ENUMERATOR = &H8000000  ' S: APM aware enumerator
        Public Const DN_APM_DRIVER = &H10000000  ' S: APM aware driver
        Public Const DN_SILENT_INSTALL = &H20000000  ' S: Silent install
        Public Const DN_NO_SHOW_IN_DM = &H40000000  ' S: No show in device manager
        Public Const DN_BOOT_LOG_PROB = &H80000000  ' S: Had a problem during preassignment of boot log con
        Public Const DN_NEED_RESTART = DN_LIAR                 ' System needs to be restarted for this Devnode to work properly
        Public Const DN_DRIVER_BLOCKED = DN_NOT_FIRST_TIME       ' One or more drivers are blocked from loading for this Devnode
        Public Const DN_LEGACY_DRIVER = DN_MOVED                ' This device is using a legacy driver
        Public Const DN_CHILD_WITH_INVALID_ID = DN_HAS_MARK             ' One or more children have invalid ID(s)

    End Class


    Public Interface BRFileManager
        Sub Copy(ByVal source As String, ByVal dest As String, ByVal overwrite As Boolean)
        Function FileExists(ByVal fileName As String) As Boolean
        Function DirectoryExists(ByVal dirName As String) As Boolean
        Sub CreateDirectory(ByVal dirName As String)
        Function AvailableSpace() As Long
    End Interface

    Public Class BRStdFileManager
        'Gestore dei file standard
        Implements BRFileManager

        Dim m_dir As String

        Public Sub New(ByVal dir As String)
            m_dir = dir

        End Sub

        Public Sub Copy(ByVal source As String, ByVal dest As String, ByVal overwrite As Boolean) Implements BRFileManager.Copy
            File.Copy(source, dest, overwrite)
        End Sub

        Public Sub CreateDirectory(ByVal dirName As String) Implements BRFileManager.CreateDirectory
            Directory.CreateDirectory(dirName)
        End Sub

        Public Function DirectoryExists(ByVal dirName As String) As Boolean Implements BRFileManager.DirectoryExists
            Return Directory.Exists(dirName)
        End Function

        Public Function FileExists(ByVal fileName As String) As Boolean Implements BRFileManager.FileExists
            Return File.Exists(fileName)
        End Function

        Public Function AvailableSpace() As Long Implements BRFileManager.AvailableSpace
            Try
                Dim drvIn As New DriveInfo(m_dir) 'Crea il drive

                Return drvIn.AvailableFreeSpace
            Catch ex As Exception
                Return [Long].MaxValue
            End Try
        End Function
    End Class

    <Serializable()> Public Class BRPackageInfo

        Dim inf_Desc As String
        Dim inf_SysVersion As Version
        Dim inf_SysDesc As String
        Dim inf_DrvBackVersion As Version
        Dim inf_Date As Date
        Dim inf_PCName As String

        Public ReadOnly Property IsDifferentSystem() As Boolean
            Get
                Try
                    Return (Me.SystemVersion.CompareTo(New Version(My.Computer.Info.OSVersion)) <> 0)
                    'Versione differente di sistema operativo
                Catch ex As Exception
                    Return True
                End Try
            End Get
        End Property


        Public ReadOnly Property ComputerName() As String
            Get
                Return Me.inf_PCName
            End Get
        End Property

        Public ReadOnly Property Description() As String
            Get
                Return Me.inf_Desc
            End Get
        End Property

        Public ReadOnly Property SystemVersion() As Version
            Get
                Return Me.inf_SysVersion
            End Get
        End Property

        Public ReadOnly Property SystemDescription() As String
            Get
                Return Me.inf_SysDesc
            End Get
        End Property

        Public ReadOnly Property DrvVersion() As Version
            Get
                Return Me.inf_DrvBackVersion
            End Get
        End Property

        Public ReadOnly Property CreationDate() As Date
            Get
                Return Me.inf_Date
            End Get
        End Property


        Public Sub New(ByVal desc As String)
            'Ricava le informazioni necessarie
            Try
                Me.inf_Desc = desc
                Me.inf_SysDesc = My.Computer.Info.OSFullName
                Me.inf_DrvBackVersion = My.Application.Info.Version
                Me.inf_Date = [Date].Now
                Me.inf_PCName = My.Computer.Name
                Me.inf_SysVersion = New Version(My.Computer.Info.OSVersion)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub New()
            'Costruttore per la serializzazione
        End Sub


    End Class


    Public Class FileEventArgs : Inherits EventArgs
        Dim m_fileName As String
        Dim m_errors As Boolean
        Dim m_size As Long

        Public ReadOnly Property Size() As Long
            Get
                Return m_size
            End Get
        End Property

        Public ReadOnly Property FileName() As String
            Get
                Return Me.m_fileName
            End Get
        End Property

        Public ReadOnly Property Errors() As Boolean
            Get
                Return Me.m_errors
            End Get
        End Property

        Public Sub New(ByVal fName As String, ByVal errs As Boolean, Optional ByVal size As Long = 0)
            Me.m_fileName = fName
            Me.m_errors = errs
            Me.m_size = size
        End Sub

    End Class

    Public Class DeviceEventArgs : Inherits CancelEventArgs

        Dim m_device As Device
        Dim m_completed As Boolean
        Dim m_filecopied As Integer
        Dim m_haserrors As Boolean
        Dim m_data As New Dictionary(Of String, Object)

        Public Property Data() As Dictionary(Of String, Object)
            Get
                Return Me.m_data
            End Get
            Set(ByVal val As Dictionary(Of String, Object))
                Me.m_data = val
            End Set
        End Property

        Public ReadOnly Property HasErrors() As Boolean
            Get
                Return Me.m_haserrors
            End Get
        End Property

        Public ReadOnly Property FilesCopied() As Integer
            Get
                Return Me.m_filecopied
            End Get
        End Property

        Public ReadOnly Property Source() As Device
            Get
                Return Me.m_device
            End Get
        End Property

        Public ReadOnly Property Completed() As Boolean
            Get
                Return Me.m_completed
            End Get
        End Property
        Public Sub New(ByVal dev As Device, ByVal completed As Boolean, ByVal fCopied As Integer, ByVal hasErrors As Boolean)
            Me.m_device = dev
            Me.m_completed = completed
            Me.m_filecopied = fCopied
            Me.m_haserrors = hasErrors
        End Sub

    End Class

    Public Class ExceptionEventArgs : Inherits Exception

        Dim m_code As BackupRestoreErrorCodes
        Dim m_cancel As Boolean = False


        Public Property Cancel() As Boolean
            Get
                Return Me.m_cancel
            End Get
            Set(ByVal value As Boolean)
                Me.m_cancel = value
            End Set
        End Property


        Public ReadOnly Property Code() As BackupRestoreErrorCodes
            Get
                Return Me.m_code
            End Get
        End Property

        Public Sub New(ByVal code As BackupRestoreErrorCodes)
            Me.m_code = code
        End Sub

    End Class

    Public Class OperationEventArgs : Inherits EventArgs

        Dim m_completed As Boolean
        Dim m_totalDevices As Integer
        Dim m_path As String

        Public Property OperationPath() As String
            Get
                Return m_path
            End Get

            Set(ByVal val As String)
                m_path = val
            End Set
        End Property

        Public ReadOnly Property Completed() As Boolean
            Get
                Return Me.m_completed
            End Get
        End Property

        Public ReadOnly Property TotalDevices() As Integer
            Get
                Return Me.m_totalDevices
            End Get
        End Property

        Public Sub New(ByVal totalDev As Integer, ByVal completed As Boolean)
            Me.m_completed = completed
            Me.m_totalDevices = totalDev
        End Sub

        Public Sub New(ByVal totalDev As Integer, ByVal completed As Boolean, ByVal path As String)
            Me.m_completed = completed
            Me.m_totalDevices = totalDev
            Me.m_path = path
        End Sub
    End Class

    Public Class DeviceBackupOffline
        Implements IDisposable

        Private m_computerName As String
        Private m_privilegeError As Boolean = False
        Private m_pathError As Boolean = False

        Public ReadOnly Property HasPrivilegeError() As Boolean
            Get
                Return m_privilegeError
            End Get
        End Property

        Public ReadOnly Property HasPathError() As Boolean
            Get
                Return m_pathError
            End Get
        End Property

        Public ReadOnly Property ComputerName() As String
            Get
                Return Me.m_computerName
            End Get
        End Property

        Private Structure LUID
            Dim UsedPart As Integer
            Dim IgnoredForNowHigh32BitPart As Integer
        End Structure

        Private Structure TOKEN_PRIVILEGES
            Dim PrivilegeCount As Integer
            Dim TheLuid As LUID
            Dim Attributes As Integer
        End Structure

        'The API functions below are all used to give the application the proper privilege so the OS will allow the app to Shutdown Windows.
        Private Declare Function OpenProcessToken Lib "advapi32.dll" (ByVal ProcessHandle As IntPtr, ByVal DesiredAccess As Integer, ByRef TokenHandle As Integer) As Integer
        Private Declare Function LookupPrivilegeValue Lib "advapi32.dll" Alias "LookupPrivilegeValueA" (ByVal lpSystemName As String, ByVal lpName As String, ByRef lpLuid As LUID) As Integer
        Private Declare Function AdjustTokenPrivileges Lib "advapi32.dll" (ByVal TokenHandle As Integer, ByVal DisableAllPrivileges As Boolean, ByRef NewState As TOKEN_PRIVILEGES, ByVal BufferLength As Integer, ByRef PreviousState As TOKEN_PRIVILEGES, ByRef ReturnLength As Integer) As Integer

        'Funzioni per il caricamento di chiavi da file binario
        Private Declare Function RegLoadKey Lib "advapi32.dll" Alias "RegLoadKeyA" (ByVal hKey As IntPtr, ByVal lpSubKey As String, ByVal lpFile As String) As IntPtr
        Private Declare Function RegUnloadKey Lib "advapi32.dll" Alias "RegUnLoadKeyA" (ByVal hKey As IntPtr, ByVal lpSubKey As String) As IntPtr

        Private Const TOKEN_ADJUST_PRIVILEGES As Int32 = &H20
        Private Const TOKEN_QUERY As Int32 = &H8
        Private Const SE_PRIVILEGE_ENABLED As Int32 = &H2

        Public Property UseOfflinePCName() As Boolean
            Get
                Return Not (Utils.ComputerName = My.Computer.Name)
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Utils.ComputerName = Me.ComputerName
                Else
                    Utils.ComputerName = ""
                End If
            End Set
        End Property


        Private Sub New(ByVal sysDir As String)
            'Carica il file di registro SYSTEM o SYSTEM.DAT
            'in una chiave temporanea del registro di sistem

            Try
                If Not LoadRegistryFile(sysDir) Then
                    Return
                End If
                'Apre la chiave caricata
                'Legge il nome del computer offline
                Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey(My.Settings.OfflineKeyName, False)
                Dim controlSetKey As String = ""
                Dim compKey As RegistryKey = Nothing

                For Each subKey As String In key.GetSubKeyNames
                    If subKey.StartsWith("ControlSet") Then
                        controlSetKey = subKey
                        Exit For
                    End If
                Next

                If [String].IsNullOrEmpty(controlSetKey) Then Throw New Exception

                'Ricava il nome del computer (se possibile) per usi futuri
                Try
                    compKey = Registry.LocalMachine.OpenSubKey(My.Settings.OfflineKeyName & "\" & controlSetKey & "\Control\ComputerName\ComputerName")
                    Me.m_computerName = DirectCast(compKey.GetValue("ComputerName"), [String])
                Catch ex2 As Exception
                    Me.m_computerName = ""
                Finally
                    If compKey IsNot Nothing Then compKey.Close()
                End Try

                'Configura il DeviceManagement
                DeviceClassCollection.RegKey = My.Settings.OfflineKeyName & "\" & controlSetKey & "\Control\Class"
                'Configura Utils
                Utils.InitUtils(sysDir)
                key.Close()
            Catch ex As Exception
                'UnloadRegistryFile()
                Throw New Exception
            End Try

        End Sub

        Public Shared Function Create(ByVal sysDir As String) As DeviceBackupOffline

            Try

                Return New DeviceBackupOffline(sysDir)

            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Private Function AdjustPrivileges() As Boolean
            Dim retval As Int32
            Try

                Dim hdlProcessHandle As IntPtr
                Dim hdlTokenHandle As Int32
                Dim tmpLuid As LUID
                Dim tkp As TOKEN_PRIVILEGES
                Dim tkpNewButIgnored As TOKEN_PRIVILEGES
                Dim lBufferNeeded As Int32
                hdlProcessHandle = Process.GetCurrentProcess.Handle
                OpenProcessToken(hdlProcessHandle, (TOKEN_ADJUST_PRIVILEGES Or TOKEN_QUERY), hdlTokenHandle)
                'Get the LUID for shutdown privilege.
                LookupPrivilegeValue("", "SeRestorePrivilege", tmpLuid)
                tkp.PrivilegeCount = 1 'One privilege to set
                tkp.TheLuid = tmpLuid
                tkp.Attributes = SE_PRIVILEGE_ENABLED
                'Enable the shutdown privilege in the access token of this process.

                retval = AdjustTokenPrivileges(hdlTokenHandle, False, tkp, Len(tkpNewButIgnored), tkpNewButIgnored, lBufferNeeded)
                If (retval = 0) Then
                    Return False
                End If

                LookupPrivilegeValue("", "SeBackupPrivilege", tmpLuid)
                tkp.PrivilegeCount = 1 'One privilege to set
                tkp.TheLuid = tmpLuid
                tkp.Attributes = SE_PRIVILEGE_ENABLED
                'Enable the shutdown privilege in the access token of this process.

                retval = AdjustTokenPrivileges(hdlTokenHandle, False, tkp, Len(tkpNewButIgnored), tkpNewButIgnored, lBufferNeeded)
                If (retval = 0) Then
                    Return False
                End If
                LookupPrivilegeValue("", "SeBackupPrivilege", tmpLuid)

                Return True
            Catch ex As Exception
                Debug.Print(My.Settings.DebugStringFormat, "DeviceBackupOffLine::AdjustPrivileges", Marshal.GetLastWin32Error.ToString & " RetVal: " & retval)
                Return False
            End Try
        End Function


        Private Function LoadRegistryFile(ByVal sysPath As String) As Boolean
            'Trova nella cartella selezionata
            Try
                Dim fName As String = Path.Combine(sysPath, My.Settings.OfflineRegistryPath)
                Dim result As IntPtr
                'Verifica la presenza del file registro
                If Not File.Exists(fName) Then
                    fName = Path.Combine(sysPath, My.Settings.OfflineRegistryPath & ".dat")
                    If Not File.Exists(fName) Then
                        Me.m_pathError = True
                        Return False
                    End If
                End If

                'Carica temporaneamente l'hive sul registro locale
                'Imposta i privilegi per eseguire l'operazione
                If Not AdjustPrivileges() Then Return False
                'Scarica eventualmente l'hive precedentemente caricato
                UnloadRegistryFile()
                'Carica l'hive
                result = RegLoadKey(RegistryHive.LocalMachine, My.Settings.OfflineKeyName, fName)
                If result <> 0 Then
                    If result = 1314 Then
                        Me.m_privilegeError = True
                        Return result = 0
                    End If
                    Me.m_pathError = True
                End If

                Return result = 0
            Catch ex As Exception
                Debug.Print(My.Settings.DebugStringFormat, "DeviceBackupOffLine::LoadRegistryFile", Marshal.GetLastWin32Error.ToString)
                Return False
            End Try
        End Function

        Private Function UnloadRegistryFile() As Boolean
            'Scarica l'hive precedentemente caricato
            Try
                Dim result As IntPtr

                result = RegUnloadKey(RegistryHive.LocalMachine, My.Settings.OfflineKeyName & ControlChars.NullChar)
                Return Not (result = 0)

            Catch ex As Exception
                Debug.Print(My.Settings.DebugStringFormat, "DeviceBackupOffLine::UnloadRegistryFile", Marshal.GetLastWin32Error.ToString)
                Return False
            End Try
        End Function

        Private disposedValue As Boolean = False        ' Per rilevare chiamate ridondanti

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: liberare altro stato (oggetti gestiti).

                End If
                'Reimposta la chiave di registro standard
                UnloadRegistryFile()
                DeviceClassCollection.RegKey = My.Settings.MainRegKey
                Utils.InitUtils("")
                Utils.ComputerName = ""
                ' TODO: liberare lo stato personale (oggetti non gestiti).
                ' TODO: impostare campi di grandi dimensioni su null.
            End If
            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' Questo codice è aggiunto da Visual Basic per implementare in modo corretto il modello Disposable.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Non modificare questo codice. Inserire il codice di pulitura in Dispose(ByVal disposing As Boolean).
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class


    Public Class DeviceBackup
        'Implementa le funzioni necessarie al backup di una collezione di devices

        'Definisce gli eventi esposti dalla classe
        Public Event BackupStarted As BRBeginEndHandler
        Public Event BackupEnded As BRBeginEndHandler

        Public Event BackupBeginDevice As BRDeviceProcessingHandler
        Public Event BackupEndDevice As BRDeviceProcessingHandler

        Public Event BackupFile As BRFileProcessingHandler
        Public Event BackupError As BRExceptionHandler
        Public Event BackupDeviceError As BRExceptionHandler

        Private Delegate Function InvokeBackup() As Boolean

        Private bk_Path As String
        Private bk_PathFormat As String
        Private bk_DevicePathFormat As String
        Private bk_DateFormat As String
        Private bk_Desc As String

        Private bk_devCollection As DeviceCollection 'Lista di Device da processare
        Private bk_BackupInfoFile As String
        Private bk_Overwrite As Boolean = False
        Private bk_fileMan As BRFileManager = Nothing 'File manager standard

        'Variabili di threading
        Dim syncObj As New Object 'Oggetto di sincronizzazione
        Dim cancelBackup As Boolean 'Variabile di stop
        Dim currThread As Thread 'Thread secondario

        Public Property FileManager() As BRFileManager
            Get
                Return Me.bk_fileMan
            End Get
            Set(ByVal value As BRFileManager)
                Me.bk_fileMan = value
            End Set
        End Property

        'Proprietà privata thread-safe che regola l'accesso a cancelBackup
        Public Property BackupCanceled() As Boolean
            Get
                Try
                    Monitor.Enter(syncObj)
                    Return cancelBackup
                Catch ex As Exception
                Finally
                    Monitor.Exit(syncObj)
                End Try
            End Get
            Set(ByVal value As Boolean)
                Try
                    Monitor.Enter(syncObj)
                    cancelBackup = value
                Catch ex As Exception
                Finally
                    Monitor.Exit(syncObj)
                End Try
            End Set
        End Property

        Public Property Description() As String
            Get
                Return Me.bk_Desc
            End Get
            Set(ByVal value As String)
                Me.bk_Desc = value
            End Set
        End Property

        Public Property CanOverwrite() As Boolean
            Get
                Return Me.bk_Overwrite
            End Get
            Set(ByVal value As Boolean)
                Me.bk_Overwrite = value
            End Set
        End Property

        Public Property BackupInfoFile() As String
            Get
                Return Me.bk_BackupInfoFile
            End Get
            Set(ByVal value As String)
                'Formatta se necessario il nome del file
                Me.bk_BackupInfoFile = FormatBackupPath(value)
            End Set
        End Property

        Public Property DevicePathFormat() As String
            Get
                Return Me.bk_DevicePathFormat
            End Get
            Set(ByVal value As String)
                If [String].IsNullOrEmpty(value) Then
                    Me.bk_DevicePathFormat = My.Settings.StdDevicePathFormat
                Else
                    Me.bk_DevicePathFormat = value
                End If
            End Set
        End Property

        Public Property BackupPathFormat() As String
            'Inserisce il formato con cui verrà costruito il nome della directory di ciascun device
            Get
                Return Me.bk_PathFormat
            End Get
            Set(ByVal value As String)
                If [String].IsNullOrEmpty(value) Then
                    Me.bk_PathFormat = My.Settings.StdBackupPathFormat
                Else
                    Me.bk_PathFormat = value
                End If
            End Set
        End Property

        Public Property BackupPath() As String
            Get
                Return Me.bk_Path
            End Get

            Set(ByVal value As String)

                Me.bk_Path = Environment.ExpandEnvironmentVariables(value)

            End Set
        End Property

        Public Property BackupDateFormat() As String
            Get
                Return Me.bk_DateFormat
            End Get
            Set(ByVal value As String)
                Me.bk_DateFormat = value
            End Set
        End Property

        Public Sub New(ByVal devs As DeviceCollection, ByVal path As String, ByVal dateFmt As String)

            Me.bk_devCollection = devs

            Me.bk_Path = path

            Me.BackupInfoFile = My.Settings.StdBackupInfoFile

            If [String].IsNullOrEmpty(dateFmt) Then
                Me.BackupDateFormat = My.Settings.DateTimePattern
            Else
                Me.BackupDateFormat = dateFmt
            End If
        End Sub

        Private Function FormatBackupPath(ByVal sFormat As String) As String
            Try
                Dim relativePath As New StringBuilder(sFormat)

                relativePath.Replace("%COMPUTERNAME%", Utils.PathStringFilter(Utils.ComputerName))

                relativePath.Replace("%NOW%", Utils.PathStringFilter(Date.Today.ToString(Me.BackupDateFormat).Replace("/", "-")))

                Return relativePath.ToString
            Catch ex As Exception
                Debug.Print(My.Settings.DebugStringFormat, "DeviceBackup::FormatBackupPath", ex.Message)
                Return [String].Empty
            End Try
        End Function

        Private Function FormatDevicePath(ByVal sFormat As String, ByVal currDev As Device) As String
            'Sostituisce i segnaposto
            Try
                Dim relativePath As New StringBuilder(sFormat)

                'Gli argomenti passati non sono validi
                If currDev Is Nothing OrElse Not currDev.IsValid Then Return [String].Empty

                With currDev
                    relativePath.Replace("%DEVNAME%", Utils.PathStringFilter(currDev.UnivoqueDescription))

                    If .ClassInfo IsNot Nothing AndAlso .ClassInfo.IsValidClass Then
                        relativePath.Replace("%CLASSNAME%", Utils.PathStringFilter(currDev.ClassInfo.ClassDescription))
                        relativePath.Replace("%CLASSGUID%", Utils.PathStringFilter(currDev.ClassInfo.ClassGuid.ToString("B")))
                    End If

                    relativePath.Replace("%PROVIDERNAME%", Utils.PathStringFilter(currDev.ProviderName))
                End With

                Return FormatBackupPath(relativePath.ToString)
            Catch ex As Exception
                Debug.Print(My.Settings.DebugStringFormat, "DeviceBackup::FormatDevicePath", ex.Message)
                Return [String].Empty
            End Try

        End Function


        Public Function AsyncBackup() As Boolean
            'Avvia l'operazione di backup in un nuovo thread
            Try

                currThread = New Thread(AddressOf Backup)
                currThread.Name = "DRVBackup thread"
                currThread.Start()
                Debug.WriteLine("Thread started successfully. " & currThread.Name)
                Return True

            Catch ex As Exception
                Debug.Print(My.Settings.DebugStringFormat, "DeviceBackup::AsyncBackup", ex.Message)
                Return False
            End Try
        End Function

        'Backup sincrono
        Public Sub Backup()

            Dim backupedDevs As Integer
            Dim copiedFiles As Integer
            Dim rootDir As String
            Dim deviceError As Boolean

            Try
                BackupCanceled = False

                If bk_fileMan Is Nothing Then Return
                'Scatena l'evento di inizio backup
                RaiseEvent BackupStarted(Me, New OperationEventArgs(Me.bk_devCollection.Count, False, BackupPath))

                'Controlla se lo spazio su disco è sufficiente

                If bk_fileMan.AvailableSpace < Me.bk_devCollection.TotalDeviceFilesSize Then
                    RaiseEvent BackupError(Me, New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_LackOfSpace))
                    Return
                End If

                'Prepara la directory principale del backup

                rootDir = Path.Combine(BackupPath, FormatBackupPath(BackupPathFormat))

                'Processa singolarmente i devices
                For Each dc As Device In Me.bk_devCollection

                    'Controlla che un'altro thread non abbia bloccato l'operazione
                    If BackupCanceled Then
                        RaiseEvent BackupError(Me, New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_OpCanceled))
                        'L'utente ha scelto di annullare l'operazione
                        Return
                    End If

                    copiedFiles = 0 'Nessun file copiato
                    deviceError = False 'Il device non ha ancora causato errori

                    Try
                        Dim localDevPath As String
                        Dim devBackupPath As String
                        'Informazioni estese non corrette o il device non è selezionato

                        RaiseEvent BackupBeginDevice(Me, New DeviceEventArgs(dc, False, 0, False))

                        If dc IsNot Nothing AndAlso dc.IsValid Then

                            devBackupPath = FormatDevicePath(Me.DevicePathFormat, dc)
                            localDevPath = Path.Combine(rootDir, devBackupPath) 'Ricava il nome della directory di destinazione

                            If Not dc.ExtendedInfo.ContainsKey("Backuped") Then dc.ExtendedInfo.Add("Backuped", False)
                            If Not dc.ExtendedInfo.ContainsKey("BackupPath") Then dc.ExtendedInfo.Add("BackupPath", [String].Empty)

                            dc.ExtendedInfo("Backuped") = False
                            dc.ExtendedInfo("BackupPath") = [String].Empty

                            If Not [String].IsNullOrEmpty(localDevPath) Then

                                'Lancia un'eccezzione se la directory non può essere creata
                                If Not FileManager.DirectoryExists(localDevPath) Then FileManager.CreateDirectory(localDevPath)
                                'Continua solo se la directory può essere creata
                                'Processa singolarmente i files
                                For Each fl As DeviceFile In dc.DriverFiles
                                    'Lancia un'eccezzione se il file non può essere copiato

                                    If FileManager.FileExists(Path.Combine(localDevPath, fl.OriginalFileName)) = True And Not CanOverwrite Then
                                        'Sovrascrittura disabilitata
                                        Dim ex As New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_FileOverwiting)
                                        ex.Data.Add("Filename", Path.Combine(localDevPath, fl.OriginalFileName))
                                        RaiseEvent BackupDeviceError(dc, ex)
                                        deviceError = True
                                    Else
                                        FileManager.Copy(fl.FullName, Path.Combine(localDevPath, fl.OriginalFileName), CanOverwrite)

                                        If fl.OriginalFileName <> fl.Name Then
                                            'Copia il file con un'altro nome qualora sia stato rinominato dal setuo
                                            FileManager.Copy(fl.FullName, Path.Combine(localDevPath, fl.Name), CanOverwrite)
                                        End If

                                        copiedFiles += 1
                                        RaiseEvent BackupFile(dc, New FileEventArgs(fl.Name, False, fl.Size))
                                    End If

                                Next

                                'Aggiorna le informazioni estese del device
                                dc.ExtendedInfo("Backuped") = True
                                dc.ExtendedInfo("BackupPath") = Path.Combine(FormatBackupPath(BackupPathFormat), devBackupPath)
                            Else
                                RaiseEvent BackupDeviceError(Nothing, New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_UnformattablePath))
                                deviceError = True
                            End If

                        Else
                            'Segnala un device non trattabile
                            RaiseEvent BackupDeviceError(Nothing, New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_InvalidDevice))
                            deviceError = True
                        End If

                    Catch ex As Exception
                        Dim genericIO As New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_FileIOError)
                        genericIO.Data.Add("Msg", ex.Message)
                        RaiseEvent BackupDeviceError(Nothing, genericIO)
                        deviceError = True
                    Finally
                        'Conclude il trattamento del device
                        RaiseEvent BackupEndDevice(Me, New DeviceEventArgs(dc, True, copiedFiles, deviceError))
                        backupedDevs += 1
                    End Try
                Next

                'Scrive le informazioni sui devices  processati
                If Not WritePackage(Me.bk_devCollection) Then
                    RaiseEvent BackupError(Me, New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_CantReadWriteBkInfo))
                End If

            Catch ex As Exception
                'Gestione errori
                Debug.Print(My.Settings.DebugStringFormat, "DeviceBackup::Backup", ex.Message)
            Finally
                'Garantisce l'evento di fine backup anche in presenza di errori
                RaiseEvent BackupEnded(Me, New OperationEventArgs(backupedDevs, True))
            End Try
        End Sub


        Private Function WritePackage(ByVal lst As DeviceCollection) As Boolean
            Try
                Dim bn As New BinaryFormatter
                Dim tempFile As String

                Using memStream As New MemoryStream
                    'Scrive le informazioni sul sistema e sul package
                    bn.Serialize(memStream, New BRPackageInfo(Me.Description))
                    bn.Serialize(memStream, lst)
                    'Crea un file temporaneo
                    tempFile = Path.GetTempFileName

                    Using fStream As New FileStream(tempFile, FileMode.OpenOrCreate)
                        memStream.WriteTo(fStream)
                        fStream.Flush()
                    End Using
                    'Copia il file temporaneo

                    FileManager.Copy(tempFile, Path.Combine(Me.BackupPath, Me.BackupInfoFile), True)
                    'Distrugge il file temporaneo
                    File.Delete(tempFile)
                End Using
                Return True
            Catch ex As Exception
                'E' avvenuto un'errore nella serializzazione
                Debug.Print(My.Settings.DebugStringFormat, "DeviceBackup::WritePackage", ex.Message)
                Return False
            End Try
        End Function


    End Class

    Public Class DeviceRestore
        'Eventi publici della classe DeviceRestore
        Public Event RestoreBegin As BRBeginEndHandler
        Public Event RestoreBeginDevice As BRDeviceProcessingHandler
        Public Event RestoreEndDevice As BRDeviceProcessingHandler
        Public Event RestoreEnd As BRBeginEndHandler
        Public Event RestoreDeviceError As BRExceptionHandler
        Public Event RestoreError As BRExceptionHandler
        Public Event RestoreForceUpdate As BRConfirmOperationHandler

        'Classe che gestisce il ripristino dei Devices
        Dim devCollection As DeviceCollection
        Dim pckInfo As BRPackageInfo
        Dim pckPath As String
        'Dim installPath As String
        Dim forceUpdate As Boolean

        'Variabili di threading
        Dim syncObj As New Object 'Oggetto di sincronizzazione
        Dim cancelRestore As Boolean 'Variabile di stop
        Dim currThread As Thread 'Thread secondario

        'Dll
        Private Declare Unicode Function SetupCopyOEMInf Lib "setupapi.dll" Alias "SetupCopyOEMInfW" (ByVal SourceInfFileName As String, ByVal OEMSourceMediaLocation As String, ByVal OEMSourceMediaType As IntPtr, ByVal CopyStyle As Int32, <Out()> ByVal DestinationInfFileName As StringBuilder, ByVal DestinationInfFileNameSize As Int32, ByRef RequiredSize As Int32, ByVal DestinationInfFileNameComponent As IntPtr) As IntPtr
        Private Const ERROR_FILE_EXISTS As Int32 = 80I
        Private Const SPOST_PATH As Int32 = 1
        Private Const SP_COPY_NOOVERWRITE As Int32 = &H8
        Private Const SP_COPY_REPLACEONLY As Int32 = &H2

        'Funzioni PnP devices manager

        Private Declare Function CM_Connect_Machine Lib "cfgmgr32.dll" Alias "CM_Connect_MachineA" (ByVal machine As IntPtr, ByRef hwnd As IntPtr) As IntPtr
        Private Declare Sub CM_Disconnect_Machine Lib "cfgmgr32.dll" (ByVal hwnd As IntPtr)
        Private Declare Function CM_Locate_DevNode_Ex Lib "cfgmgr32.dll" Alias "CM_Locate_DevNode_ExA" (ByRef devRoot As IntPtr, ByVal deviceId As IntPtr, ByVal flags As IntPtr, ByVal machineH As IntPtr) As IntPtr
        Private Declare Function CM_Locate_DevNode Lib "cfgmgr32.dll" Alias "CM_Locate_DevNodeA" (ByRef devRoot As UInt32, ByVal deviceId As String, ByVal flags As UInt32) As UInt32
        Private Declare Function CM_Reenumerate_DevNode_Ex Lib "cfgmgr32.dll" (ByVal devRoot As IntPtr, ByVal flags As IntPtr, ByVal machine As IntPtr) As IntPtr
        Private Declare Function CM_Get_Device_ID_List_Size Lib "cfgmgr32.dll" Alias "CM_Get_Device_ID_List_SizeA" (ByRef pulLen As UInt32, ByVal pszFilter As UInt32, ByVal uFlags As UInt32) As UInt32
        Private Declare Function CM_Get_Device_ID_List Lib "cfgmgr32.dll" Alias "CM_Get_Device_ID_ListA" (ByVal pszFilter As UInt32, ByVal Buffer As UInt32, ByVal BufferLen As UInt32, ByVal uFlags As UInt32) As UInt32
        Private Declare Function CM_Get_DevNode_Status Lib "cfgmgr32.dll" Alias "CM_Get_DevNode_Status" (ByRef status As UInt32, ByRef problemNumber As UInt32, ByVal DevInst As UInt32, ByVal flags As UInt32) As UInt32

        'Costanti PnP Device Manager

        Private Const CR_SUCCESS = 0
        Private Const CM_LOCATE_DEVNODE_NORMAL = &H0
        Private Const CM_LOCATE_DEVNODE_PHANTOM = &H1
        Private Const CM_LOCATE_DEVNODE_ALL = CM_LOCATE_DEVNODE_NORMAL Or CM_LOCATE_DEVNODE_PHANTOM

        Private Const DN_ROOT_ENUMERATED = &H1  ' Was enumerated by ROOT
        Private Const DN_DRIVER_LOADED = &H2  ' Has Register_Device_Driver
        Private Const DN_ENUM_LOADED = &H4  ' Has Register_Enumerator
        Private Const DN_STARTED = &H8  ' Is currently configured
        Private Const DN_MANUAL = &H10  ' Manually installed
        Private Const DN_NEED_TO_ENUM = &H20  ' May need reenumeration
        Private Const DN_NOT_FIRST_TIME = &H40  ' Has received a config
        Private Const DN_HARDWARE_ENUM = &H80  ' Enum generates hardware ID
        Private Const DN_LIAR = &H100  ' Lied about can reconfig once
        Private Const DN_HAS_MARK = &H200  ' Not CM_Create_DevInst lately
        Private Const DN_HAS_PROBLEM = &H400  ' Need device installer
        Private Const DN_FILTERED = &H800  ' Is filtered
        Private Const DN_MOVED = &H1000  ' Has been moved
        Private Const DN_DISABLEABLE = &H2000  ' Can be disabled
        Private Const DN_REMOVABLE = &H4000  ' Can be removed
        Private Const DN_PRIVATE_PROBLEM = &H8000  ' Has a private problem
        Private Const DN_MF_PARENT = &H10000  ' Multi function parent
        Private Const DN_MF_CHILD = &H20000  ' Multi function child
        Private Const DN_WILL_BE_REMOVED = &H40000  ' DevInst is being removed
        Private Const DN_NEEDS_LOCKING = &H2000000  ' S: Devnode need lock resume processing
        Private Const DN_ARM_WAKEUP = &H4000000  ' S: Devnode can be the wakeup device
        Private Const DN_APM_ENUMERATOR = &H8000000  ' S: APM aware enumerator
        Private Const DN_APM_DRIVER = &H10000000  ' S: APM aware driver
        Private Const DN_SILENT_INSTALL = &H20000000  ' S: Silent install
        Private Const DN_NO_SHOW_IN_DM = &H40000000  ' S: No show in device manager
        Private Const DN_BOOT_LOG_PROB = &H80000000  ' S: Had a problem during preassignment of boot log con
        Private Const DN_NEED_RESTART = DN_LIAR                 ' System needs to be restarted for this Devnode to work properly
        Private Const DN_DRIVER_BLOCKED = DN_NOT_FIRST_TIME       ' One or more drivers are blocked from loading for this Devnode
        Private Const DN_LEGACY_DRIVER = DN_MOVED                ' This device is using a legacy driver
        Private Const DN_CHILD_WITH_INVALID_ID = DN_HAS_MARK             ' One or more children have invalid ID(s)


        Public Shared Function EnumPnPDevices() As Dictionary(Of String, Boolean)
            'Ottiene la lista dei devices senza driver o con problemi
            Dim buff As IntPtr
            Dim buffLen As IntPtr
            Dim strBuff As String
            Dim devices As New Dictionary(Of String, Boolean)
            Dim currDevNode As UInt32
            Dim currDevNodeStatus As UInt32
            Dim currDevNodeProblem As UInt32
            Dim currDevHasProblem As Boolean

            Try

                If CM_Get_Device_ID_List_Size(buffLen, 0, 0) = CR_SUCCESS Then

                    buff = Marshal.AllocHGlobal(buffLen) 'Alloca il buffer

                    If CM_Get_Device_ID_List(0, buff, buffLen, 0) = CR_SUCCESS Then

                        strBuff = Marshal.PtrToStringAnsi(buff, buffLen) 'Converte il buffer in stringa managed

                        Marshal.FreeHGlobal(buff) 'Libera il buffer

                        For Each str As String In strBuff.ToString.Split(vbNullChar)
                            If [String].IsNullOrEmpty(str) Then Continue For 'Errore nel nome del device

                            currDevHasProblem = False

                            'Apre il devnode corrispondente
                            If CM_Locate_DevNode(currDevNode, str, CM_LOCATE_DEVNODE_ALL) = CR_SUCCESS Then
                                'Devnode aperto con successo
                                'recupera lo status
                                If CM_Get_DevNode_Status(currDevNodeStatus, currDevNodeProblem, currDevNode, 0) = CR_SUCCESS Then
                                    If (currDevNodeStatus Or DN_HAS_PROBLEM) = currDevNodeStatus Then currDevHasProblem = True
                                    'If (currDevNodeStatus Or DN_DRIVER_LOADED) <> currDevNodeStatus Then currDevHasProblem = True
                                End If
                                'Aggiunge il device alla lista
                                devices.Add(str, currDevHasProblem)
                            End If

                        Next
                    Else
                        'Impossibile ottenere la lista
                        Return Nothing
                    End If

                Else
                    'Impossibile ottenere la dimensione della lista
                    Return Nothing
                End If

                Return devices

            Catch ex As Exception
                Debug.Print(My.Settings.DebugStringFormat, "DeviceRestore::EnumPnPDevices", ex.Message)
                Return Nothing
            End Try
        End Function


        Public Property UpdateDeviceInfo() As Boolean
            Get
                Return Me.forceUpdate
            End Get
            Set(ByVal value As Boolean)
                Me.forceUpdate = value
            End Set
        End Property


        Public Shared Function PnPConfigUpdate() As Boolean
            'Forza il sistema operativo ad effettuare il riconoscimento
            ' di nuove periferiche Plug & Play
            Dim machineHandle As IntPtr
            Dim dvRoot As IntPtr
            Dim result As Boolean

            Try
                machineHandle = 0

                If CM_Connect_Machine(0, machineHandle) <> CR_SUCCESS Then
                    Return False
                End If

                If CM_Locate_DevNode_Ex(dvRoot, 0, CM_LOCATE_DEVNODE_NORMAL, machineHandle) = CR_SUCCESS Then

                    If CM_Reenumerate_DevNode_Ex(dvRoot, 0, machineHandle) = CR_SUCCESS Then
                        result = True
                    Else
                        result = False
                    End If
                Else
                    result = False
                End If

                Return result
            Catch ex As Exception
                Return False
            Finally
                If machineHandle <> 0 Then
                    CM_Disconnect_Machine(machineHandle)
                    machineHandle = 0
                End If
            End Try
        End Function

        'Proprietà privata thread-safe che regola l'accesso a cancelBackup
        Public Property RestoreCanceled() As Boolean
            Get
                Try
                    Monitor.Enter(syncObj)
                    Return cancelRestore
                Catch ex As Exception
                Finally
                    Monitor.Exit(syncObj)
                End Try
            End Get
            Set(ByVal value As Boolean)
                Try
                    Monitor.Enter(syncObj)
                    cancelRestore = value
                Catch ex As Exception
                Finally
                    Monitor.Exit(syncObj)
                End Try
            End Set
        End Property

        Public ReadOnly Property DeviceList() As DeviceCollection
            Get
                Return Me.devCollection
            End Get
        End Property

        Public ReadOnly Property Info() As BRPackageInfo
            Get
                Return Me.pckInfo
            End Get
        End Property

        Private Function ByPass() As Integer
            Return 1
        End Function

        Private Function Bypass2() As Integer
            Return 0
        End Function

        Public Function AsyncRestoreDevices() As Boolean
            'Avvia l'operazione di backup in un nuovo thread
            Try

                currThread = New Thread(AddressOf RestoreDevices)
                currThread.Name = "DRVBackup restoration thread"
                currThread.Start()
                Debug.WriteLine("Thread started successfully. " & currThread.Name)
                Return True

            Catch ex As Exception
                Debug.Print(My.Settings.DebugStringFormat, "DeviceRestore::AsyncRestore", ex.Message)
                Return False
            End Try
        End Function

        Public Sub RestoreDevices()
            'Cerca tutti i drivers selezionati e compatibili per il ripristino
            Dim devList As DeviceCollection = DeviceCollection.Create(Me.devCollection, "Selected", True)
            Dim devsRestored As Integer = 0
            Dim hasErrors As Boolean
            Dim infFile As String
            Dim returnInfName As New StringBuilder
            Dim instPath As String
            Dim flags As Integer
            Try
                RaiseEvent RestoreBegin(Me, New OperationEventArgs(devList.Count, False))

                For Each dv As Device In devList

                    If RestoreCanceled Then
                        RaiseEvent RestoreError(Me, New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_OpCanceled))
                        'L'utente ha scelto di annullare l'operazione
                        Return
                    End If

                    Try 'Gestione errori ripristino
                        hasErrors = False

                        Dim beginEvent As New DeviceEventArgs(dv, False, 0, False)
                        beginevent.Cancel = False
                        'Invia all'utente conferma del ripristino del device
                        RaiseEvent RestoreBeginDevice(Me, beginEvent)

                        If beginEvent.Cancel Then
                            RaiseEvent RestoreDeviceError(Me, New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_OpCanceled))
                            Continue For
                        End If
                        'Lancia un'eccezzione se il device non ha tutti i files

                        If dv.PortabilityLevel = DevicePortability.DCmp_None Then
                            RaiseEvent RestoreDeviceError(Me, New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_InvalidDevice))
                            Continue For
                        End If
                        'Aggiunge le informazioni estese
                        If dv.ExtendedInfo.ContainsKey("OEMInf") = False Then dv.ExtendedInfo.Add("OEMInf", "")
                        If dv.ExtendedInfo.ContainsKey("Restored") = False Then dv.ExtendedInfo.Add("Restored", False)
                        'Prepara il nome della directory i cui verranno copiati i files
                        instPath = Path.Combine(Me.pckPath, DirectCast(dv.ExtendedInfo("BackupPath"), String)) 'Path.Combine(Me.InstallationPath, Utils.PathStringFilter(dv.UnivoqueDescription))

                        infFile = Path.Combine(instPath, dv.InstallationFile)
                        returnInfName = New StringBuilder(My.Settings.MaxPath)

                        If File.Exists(infFile) OrElse [String].Compare(Path.GetExtension(infFile), "Inf", True) <> 0 Then

                            If UpdateDeviceInfo Then
                                flags = SP_COPY_REPLACEONLY
                            Else
                                flags = SP_COPY_NOOVERWRITE
                            End If

                            If SetupCopyOEMInf(infFile, instPath, SPOST_PATH, flags, returnInfName, My.Settings.MaxPath, 0, 0) = 1 Then
                                'File installato con successo
                                dv.ExtendedInfo.Item("OEMInf") = returnInfName.ToString
                                hasErrors = False
                            Else
                                'Impossibile installare il file INF
                                If Marshal.GetLastWin32Error = ERROR_FILE_EXISTS Then
                                    If UpdateDeviceInfo Then
                                        'L'utente ha già scelto di forzare l'installazione del driver
                                        RaiseEvent RestoreDeviceError(Me, New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_CantForceUpdating))
                                        hasErrors = True
                                    Else
                                        'Chiede all'utente se forzare l'installazione
                                        Dim forceEx As New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_ForceUpdate)
                                        forceEx.Cancel = False
                                        forceEx.Data.Add("Device", dv)
                                        RaiseEvent RestoreDeviceError(Me, forceEx)
                                        If Not forceEx.Cancel Then
                                            'L'utente ha scelto di forzare
                                            flags = SP_COPY_REPLACEONLY
                                            If SetupCopyOEMInf(infFile, instPath, SPOST_PATH, flags, returnInfName, My.Settings.MaxPath, 0, 0) = 1 Then
                                                'Forzatura riuscita
                                                dv.ExtendedInfo.Item("OEMInf") = returnInfName.ToString
                                                hasErrors = False
                                            Else
                                                'Forzatura non riuscita
                                                RaiseEvent RestoreDeviceError(Me, New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_CantForceUpdating))
                                                hasErrors = True
                                            End If
                                        Else
                                            'L'utente ha annullato l'aggiornamento
                                            'Invia un messaggio di errore OemExist
                                            RaiseEvent RestoreDeviceError(Me, New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_OemInfExist))
                                            hasErrors = True
                                        End If
                                    End If
                                Else
                                    'Errore irreversibile dipendente da mancate informazioni o errore di sistema
                                    RaiseEvent RestoreDeviceError(Me, New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_InvalidDevice))
                                    hasErrors = True
                                End If
                            End If

                        Else
                            RaiseEvent RestoreDeviceError(Me, New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_MissingInfFile))
                            hasErrors = True
                            Continue For 'Il file di installazione non è stato trovato
                        End If

                    Catch ex As Exception
                        'Impossibile eseguire il restore del device
                        Dim generic As New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_Generic)
                        hasErrors = True
                        generic.Data.Add("Msg", ex.Message)
                        RaiseEvent RestoreDeviceError(Me, generic)
                    Finally
                        Dim de As New DeviceEventArgs(dv, True, 0, hasErrors)
                       
                        dv.ExtendedInfo.Item("Restored") = Not hasErrors 'Device ripristinato
                        RaiseEvent RestoreEndDevice(Me, de)
                    End Try
                Next
               
            Catch ex As Exception
                Dim generic As New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_Generic)
                generic.Data.Add("Msg", ex.Message)
                RaiseEvent RestoreError(Me, generic)
            Finally
                RaiseEvent RestoreEnd(Me, New OperationEventArgs(devsRestored, True))
            End Try

        End Sub

        'Private Function UpdateRegistry() As Boolean
        'Dim rootKey As RegistryKey = Nothing
        '   Try
        'Aggiorna sul registro di sistema la lista dei drivers
        'ripristinati da DriverBackup!
        ' rootKey = Registry.LocalMachine.CreateSubKey(My.Settings.RegistryRestoreKey)

        'If rootKey Is Nothing Then Return False

        ' For Each dv As Device In Me.devCollection
        '  If dv.ExtendedInfo.ContainsKey("Restored") AndAlso dv.ExtendedInfo.Item("Restored") = True AndAlso dv.ExtendedInfo.ContainsKey("OEMInf") AndAlso Not [String].IsNullOrEmpty(dv.ExtendedInfo.Item("OEMInf")) Then
        'Il driver è stato effettivamente ripristinato da DriverBackup!
        '      rootKey.SetValue(dv.ExtendedInfo("OEMInf"), dv.UnivoqueDescription, RegistryValueKind.String)
        '  End If
        ' Next

        ' Return True
        ' Catch ex As Exception
        '     Return False
        ' Finally
        '      If rootKey IsNot Nothing Then rootKey.Close()
        '    End Try
        ' End Function

        Private Sub CheckDevices()
            Try

                Dim pt As String
                Dim fileList As New List(Of DeviceFile)
                'Recupera informazioni sull'hardware collegato al sistema
                Dim pnpDevs As Dictionary(Of String, Boolean) = EnumPnPDevices()
                Dim pnpDevsInstalled As DeviceCollection = DeviceCollection.Create(Nothing)

                For Each dv As Device In Me.devCollection
                    'Verifica che ogni device sia compatibile con il ripristino
                    dv.ExtendedInfo.Add("RestoreHwID", [String].Empty)
                    dv.ExtendedInfo.Add("RestoreMode", 0)
                    dv.ExtendedInfo.Item("RestoreMode") = 0
                    Try
                        fileList = New List(Of DeviceFile)
                        pt = Path.Combine(Me.pckPath, DirectCast(dv.ExtendedInfo("BackupPath"), String))
                        'Crea una nuova lista di files collegati con i files reali
                        For Each dvFile As DeviceFile In dv.DriverFiles
                            Dim newFile As DeviceFile = DeviceFile.Create(Path.Combine(pt, dvFile.Name), dvFile.OriginalFileName)
                            'If newFile IsNot Nothing Then
                            fileList.Add(newFile)
                            'End If
                        Next
                        dv.DriverFiles = fileList

                        'Controlla che il driver processato possa essere utile per una periferica connessa
                        Try
                            For Each k As KeyValuePair(Of String, Boolean) In pnpDevs
                                If k.Key.StartsWith(dv.MatchingID) And k.Value = True Then
                                    dv.ExtendedInfo.Item("Selected") = True
                                    dv.ExtendedInfo.Item("RestoreMode") = 1
                                    'Specifica l'HardwareId della periferica PnP
                                    dv.ExtendedInfo.Item("RestoreHwID") = k.Key.Substring(0, k.Key.LastIndexOf("\"c) - 1)
                                End If
                            Next
                        Catch ex As Exception
                            'Operazione non riuscita
                        End Try
                        'Controlla che il driver possa essere un'aggiornamento
                        'Apre il file di installazione
                        Dim instFile As DeviceInfFile = DeviceInfFile.OpenInfFile(dv.DriverFiles.Item(0).FullName)

                        For Each pnpDev As Device In pnpDevsInstalled
                            If [String].Compare(pnpDev.MatchingID, dv.MatchingID) = 0 Then
                                If DateTime.Compare(dv.ReleaseDate, pnpDev.ReleaseDate) > 0 Or dv.ReleaseVersion > pnpDev.ReleaseVersion Then
                                    'Il driver installato sul computer è meno recente di quello contenuto nel package
                                    'di backup
                                    dv.ExtendedInfo.Item("Selected") = True
                                    dv.ExtendedInfo.Add("IsNewer", True)
                                    dv.ExtendedInfo.Item("RestoreMode") = 0
                                End If
                                Try
                                    If DateTime.Compare(instFile.ReleaseDate, pnpDev.ReleaseDate) > 0 Or instFile.FileVersion > dv.ReleaseVersion Then
                                        'Confronto con INF File
                                        dv.ExtendedInfo.Item("Selected") = True
                                        dv.ExtendedInfo.Add("IsNewer", True)
                                        dv.ExtendedInfo.Item("RestoreMode") = 0
                                    End If
                                Catch
                                End Try
                                Exit For
                            End If
                        Next

                    Catch ex As Exception
                        Debug.WriteLine([String].Format(My.Settings.DebugStringFormat, "DeviceRestore::CheckDevices", ex.Message))
                    Finally
                        dv.DriverFiles = fileList
                    End Try

                    If Not dv.ExtendedInfo.ContainsKey("Restored") Then
                        dv.ExtendedInfo.Add("Restored", False)
                    Else
                        dv.ExtendedInfo.Item("Restored") = False
                    End If

                Next

            Catch ex As Exception
                Debug.WriteLine([String].Format(My.Settings.DebugStringFormat, "DeviceRestore::CheckDevices", ex.Message))
            End Try
        End Sub


        Private Sub New(ByVal list As DeviceCollection, ByVal info As BRPackageInfo, ByVal path As String)
            'Modalità standard
            Me.pckPath = path
            Me.devCollection = list
            Me.pckInfo = info
            Me.devCollection.SetDevicesProperties(Nothing, "Selected", GetType(Boolean), False, False)
            CheckDevices() 'Verifica l'utilizzabilità dei drivers salvati
        End Sub

        Public Shared Function Create(ByVal fileName As String) As DeviceRestore
            'Carica le informazioni dal file backup
            Dim fStream As FileStream = Nothing
            Dim bnSerialize As New BinaryFormatter
            Dim info As BRPackageInfo
            Dim list As DeviceCollection
            Dim pt As String

            Try

                fStream = New FileStream(fileName, FileMode.Open)

                pt = Path.GetDirectoryName(fileName)
                info = DirectCast(bnSerialize.Deserialize(fStream), BRPackageInfo)
                list = DirectCast(bnSerialize.Deserialize(fStream), DeviceCollection)
                fStream.Close()

                Return New DeviceRestore(list, info, pt)


            Catch ex As Exception
                'RaiseEvent RestoreError(Me, New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_CantReadWriteBkInfo))
                Debug.WriteLine([String].Format(My.Settings.DebugStringFormat, "DeviceRestore::Create", ex.Message))
                Return Nothing
            Finally
                If fStream IsNot Nothing Then fStream.Close()
            End Try

        End Function

    End Class

    Public Class DeviceRemove
        Implements IDisposable

        'Contiene i metodi per la disinstallazione dei device
        Public Event DeviceRemoveError As BRExceptionHandler
        Public Event DeviceUsed As CancelEventHandler

        Private Declare Function SetupUninstallOEMInf Lib "setupapi.dll" Alias "SetupUninstallOEMInfA" (ByVal filename As String, ByVal flags As Integer, ByVal Reserved As Integer) As IntPtr
        'CM_Disable_DevNode(
        'IN DEVINST  dnDevInst,
        'IN ULONG  ulFlags);


        Private Const SUOI_FORCEDELETE As Integer = &H1

        Dim rootKey As RegistryKey = Nothing

        Public Function RemoveOEMInf(ByVal infName As String) As Boolean
            Try
                'Implementazione della funzione UninstallOEMInf
                If SetupUninstallOEMInf(infName, 0, 0) = 1 Then
                    'Il file è stato disinstallato
                    rootKey.DeleteValue(infName, False) 'Aggiorna le informazioni nel registro
                Else
                    'Invia una notifica di forzatura della disinstallazione
                    Dim e As New CancelEventArgs
                    e.Cancel = False
                    RaiseEvent DeviceUsed(Me, e)
                    If Not e.Cancel Then
                        'L'utente ha scelto di forzare la disinstallazione
                        If SetupUninstallOEMInf(infName, SUOI_FORCEDELETE, 0) = 1 Then
                            'Il file è stato disinstallato
                            rootKey.DeleteValue(infName, False) 'Aggiorna le informazioni nel registro
                        Else
                            Dim exc As New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_Generic)
                            exc.Data.Add("ErrCode", Marshal.GetLastWin32Error)
                            RaiseEvent DeviceRemoveError(Me, exc)
                            Return False
                        End If
                    Else
                        RaiseEvent DeviceRemoveError(Me, New ExceptionEventArgs(BackupRestoreErrorCodes.BRE_OpCanceled))
                        Return False
                    End If
                End If

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Sub New()

        End Sub

       
        Public Function GetInstalledDevices() As Dictionary(Of String, String)
            Dim list As New Dictionary(Of String, String)
            Dim value As String
            Try
               
                For Each vName As String In Me.rootKey.GetValueNames
                    value = DirectCast(rootKey.GetValue(vName, [String].Empty), [String])
                    If [String].IsNullOrEmpty(value) = False Then
                        list.Add(vName, value)
                    End If
                Next

                Return list
            Catch ex As Exception
                Return list
            End Try
        End Function

        Private disposedValue As Boolean = False        ' Per rilevare chiamate ridondanti

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: liberare le risorse gestite chiamate in modo esplicito
                    If rootKey IsNot Nothing Then
                        rootKey.Close()
                    End If
                End If

                ' TODO: liberare le risorse non gestite condivise
            End If
            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' Questo codice è aggiunto da Visual Basic per implementare in modo corretto il modello Disposable.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Non modificare questo codice. Inserire il codice di pulitura in Dispose(ByVal disposing As Boolean).
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

End Namespace


