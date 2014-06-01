
Public Class Utils

    Public Enum SYSDIRS
        CSIDL_ALTSTARTUP = &H1D
        CSIDL_APPDATA = &H1A
        CSIDL_BITBUCKET = &HA
        CSIDL_CONTROLS = &H3
        CSIDL_COOKIES = &H21
        CSIDL_DESKTOP = &H0
        CSIDL_DESKTOPDIRECTORY = &H10
        CSIDL_DRIVES = &H11
        CSIDL_FAVORITES = &H6
        CSIDL_FONTS = &H14
        CSIDL_HISTORY = &H22
        CSIDL_INTERNET = &H1
        CSIDL_INTERNET_CACHE = &H20
        CSIDL_LOCAL_APPDATA = &H1C
        CSIDL_MYPICTURES = &H27
        CSIDL_NETHOOD = &H13
        CSIDL_NETWORK = &H12
        CSIDL_PERSONAL = &H5
        CSIDL_PRINTERS = &H4
        CSIDL_PRINTHOOD = &H1B
        CSIDL_PROFILE = &H28
        CSIDL_PROGRAM_FILES = &H26
        CSIDL_PROGRAM_FILES_COMMON = &H2B
        CSIDL_PROGRAM_FILES_COMMONX86 = &H2C
        CSIDL_PROGRAM_FILESX86 = &H2A
        CSIDL_PROGRAMS = &H2
        CSIDL_RECENT = &H8
        CSIDL_SENDTO = &H9
        CSIDL_STARTMENU = &HB
        CSIDL_STARTUP = &H7
        CSIDL_SYSTEM = &H25
        CSIDL_TEMPLATES = &H15
        CSIDL_WINDOWS = &H24
        CSIDL_ADMINTOOLS = &H30
        CSIDL_COMMON_ADMINTOOLS = &H2F
        CSIDL_COMMON_APPDATA = &H23
        CSIDL_COMMON_ALTSTARTUP = &H1D
        CSIDL_COMMON_DESKTOPDIRECTORY = &H19
        CSIDL_COMMON_DOCUMENTS = &H2E
        CSIDL_COMMON_FAVORITES = &H1F
        CSIDL_COMMON_PROGRAMS = &H17
        CSIDL_COMMON_STARTMENU = &H16
        CSIDL_COMMON_STARTUP = &H18
        CSIDL_COMMON_TEMPLATES = &H2D
        CSIDL_SYSTEMX86 = &H29
    End Enum

    Private Declare Unicode Function SHGetFolderPath Lib "shell32.dll" Alias "SHGetFolderPathW" (ByVal hwnd As IntPtr, ByVal csidl As SYSDIRS, ByVal hToken As IntPtr, ByVal dwFlags As Integer, ByVal pszPath As StringBuilder) As IntPtr

    Public Enum EnWinVersion
        W98 = 4
        W98SE = 5
        WME = 6
        W95 = 2
        W95B = 3
        WNT = 7
        WNT_40 = 8
        W2000 = 9
        WXP = 10
        WSERV_03 = 11
        WVISTA = 12
        UNK = 0
    End Enum

    Private Enum FILE_ATTR
        FILE_ATTRIBUTE_ARCHIVE = &H20
        FILE_ATTRIBUTE_COMPRESSED = &H800
        FILE_ATTRIBUTE_DEVICE = &H40
        FILE_ATTRIBUTE_DIRECTORY = &H10
        FILE_ATTRIBUTE_ENCRYPTED = &H4000
        FILE_ATTRIBUTE_HIDDEN = &H2
        FILE_ATTRIBUTE_NORMAL = &H80
        FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = &H2000
        FILE_ATTRIBUTE_OFFLINE = &H1000
        FILE_ATTRIBUTE_READONLY = &H1
        FILE_ATTRIBUTE_REPARSE_POINT = &H400
        FILE_ATTRIBUTE_SPARSE_FILE = &H200
        FILE_ATTRIBUTE_SYSTEM = &H4
        FILE_ATTRIBUTE_TEMPORARY = &H100
    End Enum

    '<StructLayout(LayoutKind.Explicit, CharSet:=CharSet.Unicode)> Private Structure WIN32_FIND_DATA
    '<FieldOffset(0)> Public dwFileAttributes As FILE_ATTR
    '<FieldOffset(4)> Public ftCreationTime As Int64
    '<FieldOffset(12)> Public ftLastAccessTime As Int64
    '<FieldOffset(20)> Public ftLastWriteTime As Int64
    '<FieldOffset(28)> Public nFileSizeHigh As Int32
    '<FieldOffset(32)> Public nFileSizeLow As Int32
    '<FieldOffset(28)> Public nFileSize As UInt64
    '<FieldOffset(36)> Public dwReserved0 As Int32
    '<FieldOffset(40)> Public dwReserved1 As Int32
    '<FieldOffset(44), MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)> Public cFileName As String
    '<FieldOffset(305), MarshalAs(UnmanagedType.ByValTStr, SizeConst:=28)> Public cAlternate As String
    'End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> Private Structure WIN32_FIND_DATA
        Public dwFileAttributes As FILE_ATTR
        Public ftCreationTime As System.Runtime.InteropServices.ComTypes.FILETIME
        Public ftLastAccessTime As System.Runtime.InteropServices.ComTypes.FILETIME
        Public ftLastWriteTime As System.Runtime.InteropServices.ComTypes.FILETIME
        Public nFileSizeHigh As UInteger
        Public nFileSizeLow As UInteger
        Public dwReserved0 As UInteger
        Public dwReserved1 As UInteger
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)> Public cFileName As String
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=14)> Public cAlternateFileName As String
    End Structure

    Public Class HWOBJ
        Dim m_vendor As String
        Dim m_devname As String
        Dim m_subsys As String
        Dim m_hwid As String
        Dim m_vendorId As String

        Public ReadOnly Property VendorID() As String
            Get
                Return m_vendorId
            End Get
        End Property

        Public ReadOnly Property Vendor() As String
            Get
                Return m_vendor
            End Get
        End Property

        Public ReadOnly Property DeviceName() As String
            Get
                Return m_devname
            End Get
        End Property

        Public ReadOnly Property Subsystem() As String
            Get
                Return m_subsys
            End Get
        End Property

        Public ReadOnly Property HardwareId() As String
            Get
                Return m_hwid
            End Get
        End Property

        Public Sub New(ByVal vendor As String, ByVal devName As String, ByVal subSys As String, ByVal hwid As String)
            m_vendor = vendor
            m_devname = devName
            m_subsys = subSys
            m_hwid = hwid
        End Sub

    End Class

    Private Shared m_SysV As EnWinVersion = EnWinVersion.UNK 'Versione sistema
    Private Shared dt_Formatter As DateTimeFormatInfo = Nothing 'Formatter date formato americano
    Private Shared m_ResolveID As New Dictionary(Of Integer, String)  'Lista di DirID
    Private Shared m_DirCache As New Dictionary(Of String, List(Of String))  'Cache di directory
    Private Shared m_findingPaths As New List(Of String) 'Finding-Paths

    Private Declare Unicode Function LoadLibrary Lib "kernel32.dll" Alias "LoadLibraryW" (ByVal lpLibFileName As String) As IntPtr
    Private Declare Unicode Function FreeLibrary Lib "kernel32.dll" (ByVal hLibModule As IntPtr) As IntPtr
    Private Declare Unicode Function LoadString Lib "user32.dll" Alias "LoadStringW" (ByVal hInstance As IntPtr, ByVal wID As Integer, ByVal ClassName As StringBuilder, ByVal nBufferMax As Integer) As Integer
    Private Declare Unicode Function GetModuleHandle Lib "kernel32.dll" Alias "GetModuleHandleW" (ByVal lpModuleName As String) As IntPtr
    Private Declare Unicode Function GetPrintProcessorDirectory Lib "winspool.drv" Alias "GetPrintProcessorDirectoryW" (ByVal pName As String, ByVal pEnvironment As String, ByVal Level As Integer, <Out()> ByVal pPrintProcessorInfo As StringBuilder, ByVal cdBuf As Integer, ByRef pcbNeeded As Integer) As Integer
    Private Declare Unicode Function GetPrinterDriverDirectory Lib "winspool.drv" Alias "GetPrinterDriverDirectoryW" (ByVal pName As String, ByVal pEnvironment As String, ByVal Level As Integer, ByVal pDriverDirectory As StringBuilder, ByVal cdBuf As Integer, ByRef pcbNeeded As Integer) As Integer
    Private Declare Unicode Function GetColorDirectory Lib "mscms.dll" Alias "GetColorDirectoryW" (ByVal pcstr As String, ByVal pstr As StringBuilder, ByRef pdword As Integer) As Integer

    Private Declare Unicode Function FindFirstFile Lib "kernel32.dll" Alias "FindFirstFileW" (ByVal lpFileName As String, ByRef lpFindFileData As WIN32_FIND_DATA) As Integer
    Private Declare Unicode Function FindNextFile Lib "kernel32.dll" Alias "FindNextFileW" (ByVal hFindFile As IntPtr, ByRef lpFindFileData As WIN32_FIND_DATA) As Integer
    Private Declare Function FindClose Lib "kernel32.dll" (ByVal hFindFile As Integer) As Integer

    Private Declare Unicode Function ShellExecute Lib "Shell32.dll" Alias "ShellExecuteW" (ByVal hwnd As IntPtr, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Integer) As Integer

    Public Declare Function AllocConsole Lib "kernel32.dll" () As Integer
    Public Declare Function FreeConsole Lib "kernel32.dll" () As Integer

    Private Shared m_sysDir As String
    Private Shared m_localSysDir As String
    Private Shared m_pcname As String


    Public Shared Property ComputerName() As String
        Get
            Return m_pcname
        End Get
        Set(ByVal value As String)
            If [String].IsNullOrEmpty(value) Then
                m_pcname = My.Computer.Name
            Else
                m_pcname = value
            End If
        End Set
    End Property
    Public Shared ReadOnly Property LocalSysDir() As String
        Get
            Return m_localSysDir
        End Get
    End Property

    Public Shared ReadOnly Property CurrentSysDir() As String
        Get
            Return m_sysDir
        End Get
    End Property

    Public Shared Function CheckAdminMode() As Boolean
        Try
            Dim wprincipal As New WindowsPrincipal(WindowsIdentity.GetCurrent)
            Return wprincipal.IsInRole("BUILTIN\Administrators")
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Shared Function OpenShellFile(ByVal FrmHwnd As IntPtr, ByVal FileName As String) As Boolean
        Dim result As Integer
        result = ShellExecute(FrmHwnd, "open", FileName, Nothing, Nothing, 0)
        Return result > 32
    End Function

    Public Shared Function PathStringFilter(ByVal rawPath As String) As String

        If [String].IsNullOrEmpty(rawPath) Then Return ""

        Return Regex.Replace(rawPath, My.Settings.PathFilter, " ").Trim(" "c, "_"c)
    End Function

    Public Shared ReadOnly Property DefaultDirs() As Dictionary(Of Integer, String)
        Get
            Return m_ResolveID
        End Get
    End Property

    Public Shared Function GetSystemFolder(ByVal clsID As SYSDIRS, Optional ByVal defaultValue As String = "") As String
        Try
            Dim strBuff As New StringBuilder(My.Settings.MaxStrBufferSize)

            SHGetFolderPath(Nothing, clsID, Nothing, 0, strBuff)

            Return strBuff.ToString

        Catch ex As Exception
            Return defaultValue

        End Try
    End Function


    Public Shared Function ResolveDirID(ByVal ID As Integer) As String
        If m_ResolveID.ContainsKey(ID) = False Then Return [String].Empty
        Return m_ResolveID.Item(ID)
    End Function


    Private Shared Sub FillDIRIDs()
        Dim strBuff As New StringBuilder(My.Settings.MaxStrBufferSize)
        Dim reqsz As Integer

        Try
            m_ResolveID = New Dictionary(Of Integer, String)

            m_ResolveID.Add(-1, [String].Empty)
            m_ResolveID.Add(10, Environment.ExpandEnvironmentVariables("%windir%\"))
            m_ResolveID.Add(11, Environment.ExpandEnvironmentVariables("%windir%\system32\"))
            m_ResolveID.Add(12, Environment.ExpandEnvironmentVariables("%windir%\system32\drivers\"))
            m_ResolveID.Add(17, Environment.ExpandEnvironmentVariables("%windir%\Inf\"))
            m_ResolveID.Add(18, Environment.ExpandEnvironmentVariables("%windir%\Help\"))
            m_ResolveID.Add(20, Environment.ExpandEnvironmentVariables("%windir%\Fonts\"))
            m_ResolveID.Add(24, Directory.GetDirectoryRoot(Environment.ExpandEnvironmentVariables("%windir%\")))
            m_ResolveID.Add(50, Environment.ExpandEnvironmentVariables("%windir%\system\"))
            m_ResolveID.Add(51, Environment.ExpandEnvironmentVariables("%windir%\system32\spool\"))
            m_ResolveID.Add(52, Environment.ExpandEnvironmentVariables("%windir%\system32\spool\drivers\"))
            m_ResolveID.Add(16406, Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) & "\")
            m_ResolveID.Add(16408, GetSystemFolder(SYSDIRS.CSIDL_STARTUP))
            m_ResolveID.Add(16409, GetSystemFolder(SYSDIRS.CSIDL_DESKTOPDIRECTORY))
            m_ResolveID.Add(16415, GetSystemFolder(SYSDIRS.CSIDL_FAVORITES))
            m_ResolveID.Add(16419, GetSystemFolder(SYSDIRS.CSIDL_APPDATA))
            m_ResolveID.Add(16422, GetSystemFolder(SYSDIRS.CSIDL_PROGRAM_FILES))
            m_ResolveID.Add(16427, GetSystemFolder(SYSDIRS.CSIDL_PROGRAM_FILES_COMMON))
            m_ResolveID.Add(16429, GetSystemFolder(SYSDIRS.CSIDL_TEMPLATES))
            m_ResolveID.Add(16430, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))

            If GetPrinterDriverDirectory(Nothing, Nothing, 1, strBuff, strBuff.Capacity, 0) <> 0 Then
                m_ResolveID.Add(66000, strBuff.ToString)
            Else
                m_ResolveID.Add(66000, [String].Empty)
            End If

            strBuff = New StringBuilder(My.Settings.MaxStrBufferSize)

            If GetPrintProcessorDirectory(Nothing, Nothing, 1, strBuff, strBuff.Capacity, reqsz) <> 0 Then
                strBuff.Append("\"c)
                m_ResolveID.Add(66001, strBuff.ToString)
                m_ResolveID.Add(55, strBuff.ToString)
            Else
                m_ResolveID.Add(66001, [String].Empty)
                m_ResolveID.Add(55, [String].Empty)
            End If

            strBuff = New StringBuilder(My.Settings.MaxStrBufferSize)
            reqsz = strBuff.Capacity

            If GetColorDirectory(Nothing, strBuff, reqsz) Then
                strBuff.Append("\"c)
                m_ResolveID.Add(66003, strBuff.ToString)
            Else
                m_ResolveID.Add(66003, [String].Empty)
            End If

        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "Utils::FillDIRIDs", ex.Message)
        End Try
    End Sub

    Shared Sub New()
        'Inizializza il tipo Utils
        Try

            m_localSysDir = Environment.ExpandEnvironmentVariables("%windir%")
            ComputerName = ""
            InitUtils("")

        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "Utils::New", "Initialization failed")
        End Try
    End Sub

    Public Shared Function ReplaceSysDir(ByVal dir As String) As String
        Dim result As String

        Try
            result = dir.Replace(LocalSysDir, CurrentSysDir)
            result = result.Replace(Directory.GetDirectoryRoot(LocalSysDir), Directory.GetDirectoryRoot(CurrentSysDir))

            Return result
        Catch ex As Exception
            Return [String].Empty

        End Try

    End Function

    Public Shared Sub InitUtils(ByVal sysDir As String)
        Try
            m_sysDir = sysDir
            'Riempe le directory predefinite
            m_ResolveID.Clear()
            FillDIRIDs()
            m_findingPaths.Clear()
            m_DirCache.Clear()
            m_DirCache = Nothing


            If Not [String].IsNullOrEmpty(sysDir) Then

                If sysDir.EndsWith("\"c) Then sysDir = sysDir.TrimEnd(New Char() {"\"c})

                Dim newResolveID As New Dictionary(Of Integer, String)
                Dim findingPts As String
                Dim dirName As String

                'Se arriva a questo punto si tratta di un'inizializzazione a sistema offline
                'Rimpiazza i percorsi al sistema locale con quelli offline
                For Each key As KeyValuePair(Of Integer, String) In m_ResolveID
                    dirName = key.Value.Replace(LocalSysDir, sysDir)
                    If key.Key = -1 Then
                        newResolveID.Add(key.Key, key.Value)
                        Continue For
                    End If

                    If dirName.StartsWith(Directory.GetDirectoryRoot(LocalSysDir)) Then Continue For

                    If Directory.Exists(dirName) Then
                        'Dal momento che è offline, la directory potrebbe non esistere
                        newResolveID.Add(key.Key, key.Value.Replace(LocalSysDir, sysDir))
                    End If
                Next

                'Assegna le nuove directory

                m_ResolveID = newResolveID

                findingPts = My.Settings.FindingPaths.Replace("%windir%", sysDir)

                For Each fstr As String In findingPts.Split(","c)
                    If Directory.Exists(fstr) Then
                        m_findingPaths.Add(fstr) 'Aggiunge solamente se la directory esiste
                    End If
                Next


            Else
                'Modo standard, mappato al sistema locale
                'Espande le finding-paths
                For Each fStr As String In Environment.ExpandEnvironmentVariables(My.Settings.FindingPaths).Split(","c)
                    If Directory.Exists(fStr) Then
                        m_findingPaths.Add(fStr) 'Aggiunge solamente se la directory esiste
                    End If
                Next
            End If

        Catch ex As Exception

        End Try

    End Sub


    Public Shared Function FindFileInMainPaths(ByVal filename As String) As String
        Try
            'Debug.WriteLine("Invoked FindFile   " & filename)

            If [String].IsNullOrEmpty(filename) Then Return [String].Empty

            Dim fname As String
            'Dim paths As String = Environment.ExpandEnvironmentVariables(My.Settings.FindingPaths)

            'Cerca nelle cartelle principali (solo root)
            For Each strDir As String In m_findingPaths
                'Cicla sulle directory
                fname = Path.Combine(strDir, filename)

                If File.Exists(fname) Then Return fname
            Next

            'Ricerca nelle DIRID
            For Each strDir As String In m_ResolveID.Values
                'Cicla sulle directory
                fname = Path.Combine(strDir, filename)

                If File.Exists(fname) Then Return fname
            Next

            'Ricerca profonda
            For Each strDir As String In m_findingPaths
                'Cicla sulle sotto-directory
                For Each subDir As String In GetSubDirectories(strDir, True)
                    fname = Path.Combine(subDir, filename)

                    If File.Exists(fname) Then Return fname
                Next
            Next

            Return [String].Empty
        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "Utils::FindFile", ex.Message)
            Return [String].Empty
        End Try
    End Function

    Public Shared Function GetSubDirectories(ByVal dirPath As String, Optional ByVal inCache As Boolean = True) As List(Of String)
        'Invoca codice un-managed per recuperare le sottodirectory
        'Ritorna la lista presente in cache
        If m_DirCache IsNot Nothing AndAlso m_DirCache.ContainsKey(dirPath) = True Then Return m_DirCache.Item(dirPath)

        'Crea una nuova lista di directory e la aggiunge alla cache
        Dim searchHandle As IntPtr = New IntPtr(-1)
        Dim searchCont As New WIN32_FIND_DATA
        Dim dirList As New List(Of String) 'Lista di directory

        Try
            If Directory.Exists(dirPath) = False Then Return dirList

            dirList.Add(dirPath)

            searchHandle = FindFirstFile(dirPath & "*", searchCont)

            If searchHandle <> -1 Then
                'Processa le sotto cartelle
                Do While FindNextFile(searchHandle, searchCont) <> 0
                    If (searchCont.dwFileAttributes Or FILE_ATTR.FILE_ATTRIBUTE_DIRECTORY) = searchCont.dwFileAttributes AndAlso searchCont.cFileName <> "." AndAlso searchCont.cFileName <> ".." Then
                        'la aggiunge all'array
                        Dim subDir As String = dirPath & searchCont.cFileName & "\"
                        dirList.Add(subDir)
                        'processa la sotto directory
                        GetSubDirectories(subDir, dirList) 'Processa ricorsivamente le sottodirectory
                    End If
                Loop
                'Aggiunge l'albero trovato alla cache
                If inCache Then
                    If m_DirCache Is Nothing Then m_DirCache = New Dictionary(Of String, List(Of String))
                    m_DirCache.Add(dirPath, dirList)
                End If
            Else
                'Nessuna sottodirectory
                Return dirList
            End If

            Return dirList

        Catch ex As Exception
            Return dirList
        Finally
            FindClose(searchHandle)
        End Try
    End Function

    Private Shared Sub GetSubDirectories(ByVal dirPath As String, ByVal dirList As List(Of String))

        Dim searchH As IntPtr = New IntPtr(-1)
        Dim searchc As New WIN32_FIND_DATA

        Try
            searchH = FindFirstFile(dirPath & "*", searchc)

            If searchH <> -1 Then
                'Processa le sotto cartelle
                Do While FindNextFile(searchH, searchc) <> 0
                    If (searchc.dwFileAttributes Or FILE_ATTR.FILE_ATTRIBUTE_DIRECTORY) = searchc.dwFileAttributes AndAlso searchc.cFileName <> "." AndAlso searchc.cFileName <> ".." Then
                        'la aggiunge all'array
                        Dim subDir As String = dirPath & searchc.cFileName & "\"
                        dirList.Add(subDir)
                        'processa la sotto directory
                        GetSubDirectories(subDir, dirList) 'Processa ricorsivamente le sottodirectory
                    End If
                Loop
            End If

            Return

        Catch ex As Exception
        Finally
            FindClose(searchH)
        End Try

    End Sub

    Public Shared ReadOnly Property DateTimeFormatter() As DateTimeFormatInfo
        Get
            If dt_Formatter IsNot Nothing Then Return dt_Formatter
            dt_Formatter = DirectCast(DateTimeFormatInfo.CurrentInfo.Clone, DateTimeFormatInfo)
            dt_Formatter.ShortDatePattern = My.Settings.DateTimePattern
            Return dt_Formatter
        End Get
    End Property

    Public Shared ReadOnly Property WindowsVersion() As EnWinVersion
        Get
            If m_SysV <> EnWinVersion.UNK Then Return m_SysV

            'Riconosce la versione di Windows
            Dim os As OperatingSystem = Environment.OSVersion
            Select Case os.Version.Major
                Case Is = 5
                    'Serie Windows XP
                    Select Case os.Version.Minor
                        Case Is = 1
                            m_SysV = EnWinVersion.WXP
                        Case Is = 2
                            m_SysV = EnWinVersion.WSERV_03
                        Case Else
                            m_SysV = EnWinVersion.W2000
                    End Select
                Case Is = 6
                    'Windows Vista
                    m_SysV = EnWinVersion.WVISTA
                Case Else
                    m_SysV = EnWinVersion.WXP
            End Select
            Return m_SysV
        End Get
    End Property

    Public Shared Function LoadStringResource(ByVal exeFileName As String, ByVal strID As IntPtr, ByVal defaultValue As String) As String
        'Carica una risorsa stringa da DLL
        Dim dllHandle As IntPtr = New IntPtr(-1)
        Dim strRes As New StringBuilder(My.Settings.MaxStrBufferSize)

        Try
            If [String].IsNullOrEmpty(exeFileName) Then Throw New ArgumentNullException

            dllHandle = LoadLibrary(exeFileName) 'Carica la libreria

            If LoadString(dllHandle, strID, strRes, strRes.Capacity) = 0 Then
                'Funzione fallita
                Return defaultValue
            Else
                Return strRes.ToString
            End If

        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "Utils::LoadStringResource", "Can't load resource file")
            Return defaultValue
        Finally
            FreeLibrary(dllHandle)
        End Try
    End Function

    Public Shared Function GenerateAutorun(ByVal pathDest As String, ByVal cmdLine As String, ByVal appPath As String, ByVal languageFiles As Dictionary(Of String, String)) As Boolean
        Try
            If Directory.Exists(appPath) = False Or Directory.Exists(pathDest) = False Then Return False
            'Crea il file autorun
            Dim fStream As New StreamWriter(Path.Combine(pathDest, "autorun.inf"))

            fStream.WriteLine("[autorun]")
            fStream.WriteLine("open= DrvBK.exe " & cmdLine)
            fStream.WriteLine("icon= DrvBK.ico")
            fStream.Close()

            'Genera il file BAT
            fStream = New StreamWriter(Path.Combine(pathDest, "Restore.bat"))
            fStream.WriteLine("DrvBK.exe " & cmdLine)
            fStream.Close()
            'Copia i files eseguibili
            For Each fName As String In My.Settings.ExecutableFiles.Split(";")
                File.Copy(Path.Combine(appPath, fName), Path.Combine(pathDest, fName))
            Next
            'Copia i files linguaggio
            For Each k As KeyValuePair(Of String, String) In languageFiles
                Try
                    File.Copy(Path.Combine(appPath, k.Key), Path.Combine(pathDest, k.Key))
                    'Copia la directory Help
                    My.Computer.FileSystem.CopyDirectory(Path.Combine(appPath, Path.GetFileNameWithoutExtension(k.Key)), Path.Combine(pathDest, Path.GetFileNameWithoutExtension(k.Key)), True)
                Catch exLang As Exception
                End Try
            Next

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function LocateUnknownDevice(ByVal hwID As String, ByVal fs As Stream) As HWOBJ
        'Ricava le informazioni sui device sconosciuti da un database interno pci.ids
        Dim st As StreamReader = Nothing

        Try
            Dim pattern As String = "_(?<ID>[0-9a-fA-F]+)\&*"
            Dim regex As New Regex(pattern)
            Dim vendor As String = ""
            Dim devname As String = ""
            Dim subsys As String = ""
            Dim matchC As MatchCollection
            Dim mat As Match

            st = New StreamReader(fs)

            If fs.CanSeek Then fs.Seek(0, SeekOrigin.Begin) 'Riporta lo stream all'inizio

            matchC = regex.Matches(hwID)

            If matchC.Count > 0 Then vendor = "^" & matchC.Item(0).Groups("ID").Value & "\s*(?<VENDOR>.+)"
            If matchC.Count > 1 Then devname = "^\t{1}?" & matchC.Item(1).Groups("ID").Value.ToLower & "\s*(?<DEVNAME>.+)"
            If matchC.Count > 2 Then subsys = matchC.Item(2).Groups("ID").Value

            If subsys.Length = 8 Then
                subsys = subsys.Insert(4, " ")
                subsys = "^\t{2}?" & subsys & "\s*(?<SUBSYS>.+)"
            Else
                subsys = [String].Empty
            End If

            Do
                mat = regex.Match(st.ReadLine, vendor)

                If mat.Success Then
                    'Vendor trovato
                    vendor = mat.Groups("VENDOR").Value
                    'Ricerca il device id
                    If Not [String].IsNullOrEmpty(devname) Then
                        Do
                            mat = regex.Match(st.ReadLine, devname)

                            If mat.Success Then
                                devname = mat.Groups("DEVNAME").Value 'Devname trovato
                                If Not [String].IsNullOrEmpty(subsys) Then

                                    Do
                                        mat = regex.Match(st.ReadLine, subsys)
                                        If mat.Success Then
                                            'Legge il valore ed esce dal ciclo
                                            subsys = mat.Groups("SUBSYS").Value
                                            Exit Do
                                        End If
                                        If st.EndOfStream Then Exit Do
                                    Loop
                                End If

                                Exit Do 'Esce dopo aver trovato devname ed eventualmente subvendor
                            End If

                            If st.EndOfStream Then Exit Do
                        Loop
                    End If
                    'Dopo che ha trovato il vendor esce comunque dal ciclo
                    Exit Do
                End If

                If st.EndOfStream Then Exit Do 'Fine dello stream raggiunta
            Loop

            If fs.CanSeek Then fs.Seek(0, SeekOrigin.Begin)

            Return New HWOBJ(vendor, devname, subsys, hwID)

        Catch ex As Exception
            Return Nothing 'Ritorna l'id hardware in caso di errore
        End Try

    End Function
End Class
