Namespace DeviceManagement
    'Spazio nomi Device Management
    'Contiene gli oggetti per la manipolazione dei devices

    Public Delegate Sub DeviceScanningHandler(ByVal sender As Object, ByVal currClass As DeviceClass, ByVal currClassN As Integer, ByVal totalClasses As Integer)

    <Serializable()> Public Class DeviceClassCollection 'Collezione di oggetti device class
        'Implementa una collezione di oggetti device class
        Inherits Dictionary(Of String, DeviceClass)
        Implements IDisposable

        <NonSerialized()> Private Shared mnRegKey As String

        Public Shared Property RegKey() As String
            Get
                Return DeviceClassCollection.mnRegKey
            End Get
            Set(ByVal value As String)
                DeviceClassCollection.mnRegKey = value
            End Set
        End Property

        Public Shared Function Create() As DeviceClassCollection
            'Ritorna una collezione di classi di periferiche da registro di sistema
            Dim dcCollection As New DeviceClassCollection
            Dim mainKey As RegistryKey = Nothing
            Dim rgx As Regex = New Regex("([0-9A-Fa-f]{32}|[0-9A-Fa-f]{8}-([0-9A-Fa-f]{4}-){3}[0-9A-Fa-f]{12})")

            Try
                'Enumera le classi e le agguinge
                mainKey = Registry.LocalMachine.OpenSubKey(RegKey, False)

                For Each keyName As String In mainKey.GetSubKeyNames
                    'Processa singolarmente le chiavi che contengono le info sulle classi

                    'Salta la classe se il guid non è in un formato valido
                    If Not rgx.IsMatch(keyName) Then Continue For
                   
                    Dim newclass As DeviceClass = DeviceClass.Create(New Guid(keyName))

                    If newclass.IsValidClass AndAlso dcCollection.ContainsKey(newclass.ClassName) = False Then 'Aggiunge solamente classi valide
                        dcCollection.Add(newclass.ClassName, newclass)
                    End If
                Next

                Return dcCollection
            Catch ex As Exception
                'Errore grave
                Debug.Print(My.Settings.DebugStringFormat, ex.Source, ex.Message)
                Return dcCollection
            Finally
                If mainKey IsNot Nothing Then mainKey.Close()
            End Try
        End Function


        Protected Overrides Sub Finalize()
            'Device class collection
            Close()
        End Sub

        Public Sub Close()

            For Each dvc As DeviceClass In Me.Values
                dvc.Dispose() 'rilascia le risorse delle classi caricate
            Next

        End Sub

#Region " IDisposable Support "
        ' Questo codice è aggiunto da Visual Basic per implementare in modo corretto il modello Disposable.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Non modificare questo codice. Inserire il codice di pulitura in Dispose(ByVal disposing As Boolean).
            Close()
            GC.SuppressFinalize(Me)
        End Sub
#End Region


    End Class

    Public Enum DevicePortability
        DCmp_None
        DCmp_Partial
        DCmp_Full
    End Enum

    Public Class DeviceFilter

        Dim flt_MustSigned As Boolean = False
        Dim flt_Provider As DeviceFilterProviders = DeviceFilterProviders.Prov_All
        Dim flt_Compatibility As DevicePortability = -1 'Nessun filtro compatibilità
        Dim flt_DeviceClass As DeviceClass = Nothing

        Public Sub New()
            'Imposta il filtro per selezionare tutti i devices
            Me.flt_Compatibility = -1
            Me.flt_DeviceClass = Nothing
            Me.flt_MustSigned = False
            Me.flt_Provider = DeviceFilterProviders.Prov_All
        End Sub

        Public Sub New(ByVal mustSigned As Boolean, ByVal provType As DeviceFilterProviders, ByVal compLevel As DevicePortability, ByVal devClass As DeviceClass)
            Me.flt_MustSigned = mustSigned
            Me.flt_DeviceClass = devClass
            Me.flt_Provider = provType
            Me.flt_Compatibility = compLevel
        End Sub

        Public Property Portability() As DevicePortability
            Get
                Return Me.flt_Compatibility
            End Get
            Set(ByVal value As DevicePortability)
                Me.flt_Compatibility = value
            End Set
        End Property

        Public Property DeviceClassInfo() As DeviceClass
            Get
                Return Me.flt_DeviceClass
            End Get
            Set(ByVal value As DeviceClass)
                Me.flt_DeviceClass = value
            End Set
        End Property

        Public Property MustSigned() As Boolean
            Get
                Return Me.flt_MustSigned
            End Get
            Set(ByVal value As Boolean)
                Me.flt_MustSigned = value
            End Set
        End Property

        Public Property ProviderType() As DeviceFilterProviders
            Get
                Return Me.flt_Provider
            End Get
            Set(ByVal value As DeviceFilterProviders)
                Me.flt_Provider = value
            End Set
        End Property

        Public Enum DeviceFilterProviders
            Prov_All
            Prov_Oem
            Prov_Others
        End Enum

        Public Function Match(ByVal dv As Device) As Boolean
            'Ritorna true se dv soddisfa i criteri impostati
            Select Case Me.flt_Provider
                Case Is = DeviceFilterProviders.Prov_Oem
                    If Not dv.IsOemDriver Then Return False
                Case Is = DeviceFilterProviders.Prov_Others
                    If dv.IsOemDriver Then Return False
            End Select

            If Me.flt_DeviceClass IsNot Nothing AndAlso dv.ClassInfo <> Me.flt_DeviceClass Then Return False

            If Me.flt_MustSigned AndAlso dv.IsDigitalSigned = False Then Return False

            If Me.flt_Compatibility <> -1 AndAlso dv.PortabilityLevel <> Me.flt_Compatibility Then Return False

            Return True
        End Function

    End Class


    <Serializable()> Public Class DeviceCollection
        'Implementa una collezione di devices
        Inherits List(Of Device)
        Implements IDisposable

        Private dc_classCollection As DeviceClassCollection

        Public ReadOnly Property TotalDeviceFilesSize() As Long
            Get
                Dim length As Long
                For Each dv As Device In Me
                    If dv IsNot Nothing AndAlso dv.IsValid Then
                        length += dv.TotalFilesSize
                    End If
                Next
                Return length
            End Get
        End Property

        Public ReadOnly Property InternalClassCollection() As DeviceClassCollection
            Get
                Return Me.dc_classCollection
            End Get
        End Property

        Public Sub New()
            'Instanziamento semplice
            Return
        End Sub

        Public Function SetDevicesProperties(ByVal filter As DeviceFilter, ByVal propName As String, ByVal valueType As Type, ByVal trueValue As Object, ByVal falseValue As Object) As Integer
            'Ritorna il numero di device che hanno soddisfatto le condizioni impostate
            Dim count As Integer = 0
            'Controlla il tipo degli argomenti object
            If trueValue.GetType IsNot falseValue.GetType OrElse trueValue.GetType IsNot valueType Then Return count

            If filter Is Nothing Then
                For Each dv As Device In Me
                    If dv.ExtendedInfo.ContainsKey(propName) = False Then
                        dv.ExtendedInfo.Add(propName, falseValue)
                        count += 1
                        Exit For
                    End If
                    dv.ExtendedInfo(propName) = falseValue
                    count += 1
                Next
            Else
                For Each dv As Device In Me
                    If dv.ExtendedInfo.ContainsKey(propName) = False Then dv.ExtendedInfo.Add(propName, Nothing)

                    If filter.Match(dv) = True Then
                        dv.ExtendedInfo(propName) = trueValue
                        count += 1
                    Else
                        dv.ExtendedInfo(propName) = falseValue
                    End If
                Next
            End If

            Return count
        End Function

        Private Sub New(ByVal dcl As DeviceClassCollection, ByVal statusDlg As DeviceScanningHandler)
            Dim processedClass As Integer

            For Each cls As DeviceClass In dcl.Values
                If cls.IsValidClass = True Then 'Processa la classe solo se è valida
                    If statusDlg IsNot Nothing Then statusDlg.Invoke(Me, cls, processedClass, dcl.Count)
                    For Each dv As Device In cls.Devices
                        If dv.IsValid = True Then
                            Me.Add(dv)
                        End If
                    Next
                    processedClass += 1
                End If
            Next
        End Sub


        Public Shared Function Create(ByVal devList As DeviceCollection, ByVal propName As String, ByVal value As Object) As DeviceCollection

            Dim newList As New DeviceCollection

            Try

                For Each dv As Device In devList
                    If dv Is Nothing Then Continue For
                    If dv.ExtendedInfo.ContainsKey(propName) AndAlso dv.ExtendedInfo(propName).Equals(value) AndAlso dv.IsValid Then
                        newList.Add(dv)
                    End If
                Next

                Return newList
            Catch ex As Exception
                Return newList
            End Try
        End Function

        Public Shared Function Create(ByVal dcl As DeviceClassCollection, ByVal statusDlg As DeviceScanningHandler) As DeviceCollection
            If dcl Is Nothing Then Return New DeviceCollection

            Return New DeviceCollection(dcl, statusDlg)
        End Function

        Public Shared Function Create(ByVal statusDlg As DeviceScanningHandler) As DeviceCollection
            'Crea la collection di classi dal registro di sistema
            Dim clsCollection As DeviceClassCollection = DeviceClassCollection.Create

            Return DeviceCollection.Create(clsCollection, statusDlg)

        End Function

        Protected Overrides Sub Finalize()
            Close()
        End Sub

        Private Sub Close()

            If Me.dc_classCollection Is Nothing Then
                'Distrugge manualmente i devices memorizzati
                For Each dv As Device In Me
                    dv.Dispose()
                Next
            Else
                'Delega la distruzione dei device alla collezione di classi
                Me.dc_classCollection.Dispose()
            End If

        End Sub

        Private disposedValue As Boolean = False        ' Per rilevare chiamate ridondanti

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: liberare le risorse gestite chiamate in modo esplicito
                    Close()
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

    <Serializable()> Public NotInheritable Class DeviceClass 'Oggetto classe di periferiche
        Implements IDisposable
        Implements IComparable(Of DeviceClass)
        Implements IEquatable(Of DeviceClass)
        Implements IDeserializationCallback

        'Implementa una classe di periferiche

        Private Declare Function SetupDiLoadClassIcon Lib "setupapi.dll" (ByRef ClassGuid As WIN_GUID, <Out()> ByRef LargeIcon As IntPtr, ByRef MiniIconIndex As IntPtr) As IntPtr
        Private Declare Unicode Function SetupDiClassNameFromGuid Lib "setupapi.dll" Alias "SetupDiClassNameFromGuidW" (ByRef ClassGuid As WIN_GUID, ByVal ClassName As StringBuilder, ByVal ClassNameSize As IntPtr, ByRef RequiredSize As IntPtr) As IntPtr
        Private Declare Function ExtractIcon Lib "shell32.dll" Alias "ExtractIconA" (ByVal hInst As IntPtr, ByVal lpszExeFileName As String, ByVal nIconIndex As IntPtr) As IntPtr

        Private Structure WIN_GUID
            Public Data1 As UInt32
            Public Data2 As UInt16
            Public Data3 As UInt16
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=8)> Public Data4() As Byte

            Public Shared Function WinGuidFromGuid(ByVal gd As Guid) As WIN_GUID
                Dim wGD As New WIN_GUID

                Try
                    Dim st As New MemoryStream(gd.ToByteArray)
                    Dim stRead As New BinaryReader(st)
                    'Copia manualmente i campi
                    wGD.Data1 = stRead.ReadUInt32
                    wGD.Data2 = stRead.ReadUInt16
                    wGD.Data3 = stRead.ReadUInt16
                    wGD.Data4 = stRead.ReadBytes(8)
                    stRead.Close()
                    st.Close()
                    Return wGD
                Catch ex As Exception
                    Debug.Print(My.Settings.DebugStringFormat, "WIN_GUID::WinGuidFormGuid", "Bad guid format.")
                    Return wGD
                End Try

            End Function
        End Structure

        'Variabili di istanza e proprietà principali della DeviceClass
        <NonSerialized()> Private cls_deviceCollection As DeviceCollection
        <NonSerialized()> Private cls_deviceCount As Integer
        Private cls_Name As String 'Nome della classe
        Private cls_Description As String 'Descrizione
        <NonSerialized()> Private cls_Icon As Icon 'Icona
        <NonSerialized()> Private cls_IconFile As String
        <NonSerialized()> Private cls_IconN As Integer
        <NonSerialized()> Private cls_wGD As WIN_GUID 'Guid
        Private cls_Guid As Guid
        <NonSerialized()> Private cls_HasDevices As Boolean
        <NonSerialized()> Private cls_IsLocal As Boolean
        Private cls_IsValid As Boolean 'Membro speciale assegnabile solo dal costruttore

        Public ReadOnly Property Devices() As DeviceCollection
            Get
                'Ritorna una device collection

                If Me.cls_deviceCollection IsNot Nothing Then Return Me.cls_deviceCollection 'Se esiste ritorna la lista in cache

                Dim dvList As New DeviceCollection
                Dim dv As Device
                Dim prevDev As Device = Nothing

                For count As Integer = 0 To Me.cls_deviceCount 'Esplora tutti i devices
                    dv = Device.Create(Me, count)
                    If dv.IsValid = True Then
                        'dv.LoadDriverFiles()
                        If dvList.Contains(dv) = False Then
                            'Modifica se necessario l'ID del device
                            If prevDev IsNot Nothing AndAlso dv.Description = prevDev.Description Then dv.DuplicateNumber = prevDev.DuplicateNumber + 1
                            dvList.Add(dv)
                        End If
                        prevDev = dv
                    End If
                Next

                Me.cls_deviceCollection = dvList

                Return dvList
            End Get
        End Property

        Public ReadOnly Property IsLocal() As Boolean
            Get
                Return Me.cls_IsLocal
            End Get
        End Property

        Public ReadOnly Property ClassName() As String
            Get
                Return Me.cls_Name
            End Get
        End Property
        Public ReadOnly Property ClassDescription() As String
            Get
                Return Me.cls_Description
            End Get
        End Property

        Public ReadOnly Property ClassIcon() As Icon
            Get
                If Me.cls_IsLocal = False Then Return Nothing
                If Me.cls_Icon Is Nothing Then
                    Dim icn As IntPtr

                    'Carica l'icona per Windows XP e precedenti
                    If SetupDiLoadClassIcon(Me.cls_wGD, icn, Nothing) Then
                        'Crea un nuovo oggetto icona dall'handle ricevuto
                        Me.cls_Icon = Icon.FromHandle(icn)
                    Else
                        Me.cls_Icon = Nothing
                    End If

                End If

                Return Me.cls_Icon
            End Get
        End Property

        Public ReadOnly Property ClassGuid() As Guid
            Get
                Return Me.cls_Guid
            End Get
        End Property

        Public ReadOnly Property IsValidClass() As Boolean
            Get
                'Verifica che la classe sia trattabile
                Return Me.cls_IsValid
            End Get
        End Property

        Public ReadOnly Property GetRegistryKey() As String
            Get
                Return [String].Format("{0}\{1}", DeviceClassCollection.RegKey, Me.cls_Guid.ToString("B"))
            End Get
        End Property

        Private Sub New(ByVal classGuid As Guid)

            Dim classKey As RegistryKey = Nothing
            Dim winVer As New Version(My.Computer.Info.OSVersion)

            Try
                Me.cls_IsValid = False

                classKey = Registry.LocalMachine.OpenSubKey(DeviceClassCollection.RegKey & "\" & classGuid.ToString("B"), False)

                If classKey Is Nothing Then
                    Debug.Print(My.Settings.DebugStringFormat, "DeviceClass::New", "Can't open class's registry key.")
                    Return
                End If


                Me.cls_HasDevices = (classKey.SubKeyCount > 0)
                Me.cls_deviceCount = classKey.SubKeyCount
                Me.cls_wGD = WIN_GUID.WinGuidFromGuid(classGuid)
                Me.cls_Guid = classGuid
                Me.cls_Name = CStr(classKey.GetValue("Class", [String].Empty)) 'Nome della classe

                Me.cls_Description = CStr(classKey.GetValue("", [String].Empty))
                Me.cls_IsLocal = True
            
                If Utils.WindowsVersion >= Utils.EnWinVersion.WVISTA Then
                    'Windows Vista o superiori
                    'Descrizione
                    Dim mc As MatchCollection = Regex.Matches(CStr(classKey.GetValue("ClassDesc", [String].Empty)), My.Settings.RGXClassDescSplitter)

                    If mc.Count = 2 Then
                        Dim strId As Integer = 0

                        If [Integer].TryParse(mc.Item(1).Groups("arg").Value, strId) Then
                            strId = Math.Abs(strId)
                            Me.cls_Description = Utils.LoadStringResource(Environment.ExpandEnvironmentVariables(mc.Item(0).Groups("arg").Value), strId, Me.cls_Description)
                        End If

                    End If

                End If

                Me.cls_IsValid = Not [String].IsNullOrEmpty(Me.cls_Name) 'Contrassegna la classe come valida

            Catch ex As Exception
                Debug.Print(My.Settings.DebugStringFormat, "DeviceClass::New", ex.Message)
            Finally
                If classKey IsNot Nothing Then classKey.Close()
            End Try
        End Sub

        Public Shared Function Create(ByVal classGuid As Guid) As DeviceClass
            'Crea la classe dal registro locale
            Return New DeviceClass(classGuid)
        End Function

        Private Sub Close()
            If Me.cls_Icon IsNot Nothing Then
                Me.cls_Icon.Dispose()
                Me.cls_Icon = Nothing
            End If

            If Me.cls_deviceCollection IsNot Nothing Then Me.cls_deviceCollection.Dispose()
        End Sub

#Region " IDisposable Support "
        ' Questo codice è aggiunto da Visual Basic per implementare in modo corretto il modello Disposable.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Non modificare questo codice. Inserire il codice di pulitura in Dispose(ByVal disposing As Boolean).
            Close()
            GC.SuppressFinalize(Me)
        End Sub
#End Region

        Protected Overrides Sub Finalize()
            Close()
        End Sub

        Public Shared Operator =(ByVal obj1 As DeviceClass, ByVal obj2 As DeviceClass) As Boolean
            Return obj1.ClassGuid = obj2.ClassGuid
        End Operator

        Public Shared Operator <>(ByVal obj1 As DeviceClass, ByVal obj2 As DeviceClass) As Boolean
            Return Not obj1 = obj2
        End Operator

        Public Function CompareTo(ByVal other As DeviceClass) As Integer Implements System.IComparable(Of DeviceClass).CompareTo
            Return Me.ClassDescription.CompareTo(other.ClassDescription)
        End Function

        Public Function Equals1(ByVal other As DeviceClass) As Boolean Implements System.IEquatable(Of DeviceClass).Equals
            Return Me = other
        End Function

        Public Sub OnDeserialization(ByVal sender As Object) Implements System.Runtime.Serialization.IDeserializationCallback.OnDeserialization
            'Inizializza correttamente la DeviceClass serializzata
            Me.cls_deviceCollection = New DeviceCollection
            Me.cls_deviceCount = 0
            Me.cls_HasDevices = False
            Me.cls_Icon = Nothing
            Me.cls_wGD = New WIN_GUID
            Me.cls_IsLocal = False
        End Sub
    End Class

    <Serializable()> Public NotInheritable Class Device 'Oggetto periferica
        Implements IDisposable
        Implements IComparable(Of Device)
        Implements IEquatable(Of Device)
        Implements IDeserializationCallback

        'Proprietà del device
        Dim dev_DeviceClass As DeviceClass
        Dim dev_Description As String = ""
        Dim dev_ReleaseDate As DateTime = DateTime.MinValue
        Dim dev_ReleaseVersion As Version
        Dim dev_InstallSection As String = ""
        Dim dev_MultipleOSes As Boolean = False
        Dim dev_HasAllFiles As Boolean = False
        Dim dev_Provider As String = ""
        Dim dev_MatchingID As String = ""
        Dim dev_IsDigitallySigned As Boolean
        Dim dev_Platform As String = ""
        Dim dev_IsOemDriver As Boolean = False
        Dim dev_IsValid As Boolean = False
        Dim dev_FilesSize As ULong = 0
        Dim dev_ExtendedProps As New Dictionary(Of String, Object) 'Collezione di proprietà definite dal programma

        Dim dev_dupNumber As UInteger = 0
        'Collezione di files di installazione
        <NonSerialized()> Dim dev_InfFile As DeviceInfFile
        Dim dev_Files As List(Of DeviceFile)
        Dim dev_InfName As String
        <NonSerialized()> Dim dev_IsLocal As Boolean

        'Collezione di proprietà standard


        Public Shared ReadOnly Property StandardProperties() As Dictionary(Of String, Object)
            Get
                'If dev_standardProperties IsNot Nothing Then Return dev_standardProperties

                Dim dev_standardProperties As New Dictionary(Of String, Object)

                With dev_standardProperties
                    .Add("Selected", True)
                    .Add("Backuped", False)
                    .Add("BackupPath", [String].Empty)
                End With

                'Clona l'oggetto
                Return dev_standardProperties 'New Dictionary(Of String, Object)(dev_standardProperties)

            End Get
        End Property

        Public ReadOnly Property UnivoqueDescription() As String
            Get
                If Me.dev_dupNumber <> 0 Then
                    Return [String].Format("{0} ({1})", Me.Description, Me.dev_dupNumber.ToString)
                Else
                    Return Me.Description
                End If
            End Get
        End Property

        Public Property DuplicateNumber() As UInteger
            Get
                Return Me.dev_dupNumber
            End Get
            Set(ByVal value As UInteger)
                Me.dev_dupNumber = value
            End Set
        End Property

        Public ReadOnly Property PortabilityLevel() As DevicePortability
            Get
                Dim port As DevicePortability = DevicePortability.DCmp_Full

                If Not Me.CanSupportMultipleOS Then port = DevicePortability.DCmp_Partial

                If Me.IsLocal Then
                    'l'oggetto è mappato ad un device sul computer locale
                    If Not Me.CanSupportMultipleOS And Not Me.HasAllFiles Then port = DevicePortability.DCmp_None
                Else
                    'L'oggetto è mappato ad un device non locale (Serializzato)
                    If Not Me.HasAllFiles Then port = DevicePortability.DCmp_None
                End If

                Return port
            End Get
        End Property

        Public Property ExtendedInfo() As Dictionary(Of String, Object)
            Get
                Return Me.dev_ExtendedProps
            End Get
            Set(ByVal value As Dictionary(Of String, Object))
                If value IsNot Nothing Then
                    Me.dev_ExtendedProps = value
                End If
            End Set
        End Property


#Region "Readonly properties"

        Public Property InstallationFile() As String
            Get
                Return Me.dev_InfName
            End Get
            Set(ByVal value As String)
                Me.dev_InfName = value
            End Set
        End Property

        Public Property IsLocal() As Boolean
            Get
                Return Me.dev_IsLocal
            End Get
            Set(ByVal value As Boolean)
                Me.dev_IsLocal = value
            End Set
        End Property

        Public Property IsValid() As Boolean
            Get
                Return Me.dev_IsValid
            End Get
            Set(ByVal value As Boolean)
                Me.dev_IsValid = value
            End Set
        End Property


        Public ReadOnly Property TotalFilesSize() As ULong
            Get
                Return Me.dev_FilesSize
            End Get
        End Property

        Public ReadOnly Property HasAllFiles() As Boolean
            Get
                If Not Me.IsLocal AndAlso Me.DriverFiles IsNot Nothing Then
                    'Il device e stato deserializzato, quindi controlla l'esistenza del files

                    For Each dvF As DeviceFile In Me.DriverFiles
                        If dvF IsNot Nothing Then
                            If dvF.Exist = False Then
                                'Ha trovato un file che non esiste
                                Me.dev_HasAllFiles = False
                                Exit For
                            End If
                        End If
                    Next
                End If

                Return Me.dev_HasAllFiles
            End Get
        End Property

        Public Property Description() As String
            Get
                Return Me.dev_Description
            End Get
            Set(ByVal value As String)
                Me.dev_Description = value
            End Set
        End Property

        Public Property ClassInfo() As DeviceClass
            Get
                Return Me.dev_DeviceClass
            End Get
            Set(ByVal value As DeviceClass)
                Me.dev_DeviceClass = value
            End Set
        End Property

        Public Property ReleaseDate() As DateTime
            Get
                Return Me.dev_ReleaseDate
            End Get
            Set(ByVal value As DateTime)
                Me.dev_ReleaseDate = value
            End Set
        End Property

        Public Property ReleaseVersion() As Version
            Get
                Return Me.dev_ReleaseVersion
            End Get
            Set(ByVal value As Version)
                Me.dev_ReleaseVersion = value
            End Set
        End Property

        Public Property InstallSection() As String
            Get
                Return Me.dev_InstallSection
            End Get
            Set(ByVal value As String)
                Me.dev_InstallSection = value
            End Set
        End Property

        Public Property CanSupportMultipleOS() As Boolean
            Get
                Return Me.dev_MultipleOSes
            End Get
            Set(ByVal value As Boolean)
                Me.dev_MultipleOSes = value
            End Set
        End Property

        Public Property IsDigitalSigned() As Boolean
            Get
                Return Me.dev_IsDigitallySigned
            End Get
            Set(ByVal value As Boolean)
                Me.dev_IsDigitallySigned = value
            End Set
        End Property

        Public Property ProviderName() As String
            Get
                Return Me.dev_Provider
            End Get
            Set(ByVal value As String)
                Me.dev_Provider = value
            End Set
        End Property

        Public Property MatchingID() As String
            Get
                Return Me.dev_MatchingID
            End Get
            Set(ByVal value As String)
                Me.dev_MatchingID = value
            End Set
        End Property

        Public Property Platform() As String
            Get
                Return Me.dev_Platform
            End Get
            Set(ByVal value As String)
                Me.dev_Platform = value
            End Set
        End Property

        Public Property IsOemDriver() As Boolean
            Get
                Return Me.dev_IsOemDriver
            End Get
            Set(ByVal value As Boolean)
                Me.dev_IsOemDriver = value
            End Set
        End Property

#End Region

        Public Sub LoadDriverFiles()
            'Ritorna se la lista è già stata creata precedentemente
            Try

                If Me.dev_Files IsNot Nothing Then Return

                Me.dev_Files = New List(Of DeviceFile)
                Me.dev_FilesSize = 0

                Dim dvFile As DeviceFile

                If Me.dev_InfFile Is Nothing Then Return 'File INF non valido

                Me.dev_HasAllFiles = True
                'Aggiunge alla lista il file INF principale

                dvFile = DeviceFile.Create(Me.dev_InfFile.InfFile)
                If dvFile IsNot Nothing AndAlso Me.dev_Files.Contains(dvFile) = False Then
                    'Aggiunge il nuovo file solo se non è null e non è contenuto già
                    'nella lista
                    If dvFile.Exist = False Then
                        Me.dev_HasAllFiles = False
                    Else
                        Me.dev_FilesSize += dvFile.Size 'Calcola la dimensione totale dei files
                    End If
                    Me.dev_Files.Add(dvFile)
                End If

                For Each fInfo As FileInfo In Me.dev_InfFile.LayoutFiles
                    dvFile = DeviceFile.Create(fInfo)
                    If dvFile IsNot Nothing AndAlso Me.dev_Files.Contains(dvFile) = False Then
                        'Aggiunge il nuovo file solo se non è null e non è contenuto già
                        'nella lista
                        If dvFile.Exist = False Then
                            Me.dev_HasAllFiles = False
                        Else
                            Me.dev_FilesSize += dvFile.Size 'Calcola la dimensione totale dei files
                        End If
                        Me.dev_Files.Add(dvFile)
                    End If
                Next

                For Each fInfo As FileInfo In Me.dev_InfFile.CatalogFiles
                    dvFile = DeviceFile.Create(fInfo)
                    If dvFile IsNot Nothing AndAlso Me.dev_Files.Contains(dvFile) = False Then
                        'Aggiunge il nuovo file solo se non è null e non è contenuto già
                        'nella lista
                        If dvFile.Exist = False Then
                            'Me.dev_HasAllFiles = False
                        Else
                            Me.dev_FilesSize += dvFile.Size 'Calcola la dimensione totale dei files
                        End If
                        Me.dev_Files.Add(dvFile)
                    End If
                Next

                For Each finfo As DeviceFile In Me.dev_InfFile.GetFilesFromSection(Me.dev_InstallSection)
                    If finfo Is Nothing Then Continue For
                    If Me.dev_Files.Contains(finfo) = False Then
                        'Aggiunge il nuovo file solo se non è null e non è contenuto già
                        'nella lista
                        If finfo.Exist = False Then
                            Me.dev_HasAllFiles = False
                        Else
                            Me.dev_FilesSize += finfo.Size 'Calcola la dimensione totale dei files
                        End If

                        Me.dev_Files.Add(finfo)
                    End If
                Next

                For Each fInfo As DeviceFile In Me.dev_InfFile.GetFilesFromSection(Me.dev_InstallSection & ".CoInstallers")
                    If fInfo Is Nothing Then Continue For
                    If Me.dev_Files.Contains(fInfo) = False Then
                        'Aggiunge il nuovo file solo se non è null e non è contenuto già
                        'nella lista
                        If fInfo.Exist = True Then
                            'I file CoInstallers non sono molto importanti
                            'e li aggiunge quando li trova
                            Me.dev_FilesSize += fInfo.Size 'Calcola la dimensione totale dei files
                        End If
                        Me.dev_Files.Add(fInfo)
                    End If
                Next



            Catch ex As Exception
                Debug.Print(My.Settings.DebugStringFormat, "Device::LoadDriverFiles", ex.Message)
            End Try
        End Sub

        Public Property DriverFiles() As List(Of DeviceFile)
            Get
                'Compone la lista dei files utilizzati dal driver del device
                LoadDriverFiles()
                Return Me.dev_Files
            End Get
            Set(ByVal value As List(Of DeviceFile))
                Me.dev_Files = value
            End Set
        End Property

        Public Sub New()
            'Costruttore per la serializzazione
        End Sub

        Private Sub New(ByVal DevClass As DeviceClass, ByVal Id As Integer)
            'Costruttore della classe Device
            Dim devKey As RegistryKey = Nothing
            Dim strBuff As String
            Try
                'Apre la chiave principale
                Me.dev_IsValid = False

                If DevClass Is Nothing OrElse DevClass.IsValidClass = False Then Return

                Me.dev_DeviceClass = DevClass

                devKey = Registry.LocalMachine.OpenSubKey(DevClass.GetRegistryKey & "\" & [String].Format("{0:0000}", Id))

                If devKey Is Nothing Then
                    Debug.Print(My.Settings.DebugStringFormat, "Device::Create", "Can't open device's registry key.")
                    Return
                End If

                Me.dev_Description = CStr(devKey.GetValue("DriverDesc", [String].Empty))

                Try
                    Me.dev_ReleaseVersion = New Version(CStr(devKey.GetValue("DriverVersion", [String].Empty)))
                Catch ex2 As Exception
                    Me.dev_ReleaseVersion = New Version(0, 0, 0, 0)
                End Try

                'Ricava la data
                strBuff = CStr(devKey.GetValue("DriverDate", [String].Empty))

                DateTime.TryParse(strBuff, Utils.DateTimeFormatter, DateTimeStyles.AssumeLocal, Me.dev_ReleaseDate)

                Me.dev_MatchingID = CStr(devKey.GetValue("MatchingDeviceId", [String].Empty))

                If Me.dev_MatchingID = "" Then
                    Me.dev_IsValid = False
                    Return
                End If

                Me.dev_Provider = CStr(devKey.GetValue("ProviderName", [String].Empty))

                Me.dev_IsOemDriver = ([String].Compare(Me.dev_Provider, "Microsoft", True) = 0)

                Me.dev_InstallSection = CStr(devKey.GetValue("InfSection", [String].Empty) & devKey.GetValue("InfSectionExt", [String].Empty))

                Me.dev_MultipleOSes = [String].IsNullOrEmpty(CStr(devKey.GetValue("InfSectionExt", "")))

                'Apre il file di installazione
                Me.dev_InfName = CStr(devKey.GetValue("InfPath", ""))

                Me.dev_InfFile = DeviceInfFile.OpenInfFile(Utils.DefaultDirs(17) & Me.dev_InfName) '%windir%\INF

                If Me.dev_InfFile Is Nothing Then
                    Me.dev_IsValid = False
                    Return
                End If

                Me.dev_IsDigitallySigned = Me.dev_InfFile.IsWHQLSigned

                Me.dev_IsValid = True
                Me.dev_IsLocal = True
                'Imposta le proprietà predefinite
                Me.ExtendedInfo = StandardProperties

            Catch ex As Exception
                Debug.Print(My.Settings.DebugStringFormat, "Device::Create", ex.Message)
            Finally
                If devKey IsNot Nothing Then devKey.Close()
            End Try
        End Sub

        Public Shared Function Create(ByVal DevClass As DeviceClass, ByVal Id As Integer) As Device
            'Ritorna un device creato dal registro locale
            Return New Device(DevClass, Id)
        End Function

        Public Shared Operator =(ByVal obj1 As Device, ByVal obj2 As Device) As Boolean
            'Testa l'uguaglianza tra due device

            If obj1 Is Nothing OrElse obj2 Is Nothing Then Return False

            If obj1.Description <> obj2.Description Then Return False

            If obj1.InstallSection <> obj2.InstallSection Then Return False

            If obj1.MatchingID <> obj2.MatchingID Then Return False

            If obj1.ReleaseVersion <> obj2.ReleaseVersion Then Return False

            If obj1.dev_InfName <> obj2.dev_InfName Then Return False

            If obj1.ProviderName <> obj2.ProviderName Then Return False

            'Compara la lista dei files di installazione
            Dim lst1 As List(Of DeviceFile) = obj1.DriverFiles
            Dim lst2 As List(Of DeviceFile) = obj2.DriverFiles

            If lst1.Count <> lst2.Count Then Return False

            'Compara ogni singolo file contenuto nelle liste
            Return lst2.TrueForAll(AddressOf lst1.Contains)

        End Operator

        Public Shared Operator <>(ByVal obj1 As Device, ByVal obj2 As Device) As Boolean
            Return Not (obj1 = obj2) 'Utilizza l'operatore =
        End Operator

        Public Function CompareTo(ByVal other As Device) As Integer Implements System.IComparable(Of Device).CompareTo
            If Me = other Then Return 0 'Sono uguali
            'Altrimenti li ordina in base alla descrizione
            Return Me.Description.CompareTo(other.Description)
        End Function

        Public Function Equals1(ByVal other As Device) As Boolean Implements System.IEquatable(Of Device).Equals
            Return Me = other 'Utilizza l'operatore =
        End Function

#Region " IDisposable Support "

        Private disposedValue As Boolean = False        ' Per rilevare chiamate ridondanti

        ' IDisposable
        Protected Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: liberare le risorse gestite chiamate in modo esplicito
                    If Me.dev_InfFile IsNot Nothing Then Me.dev_InfFile.Dispose()
                End If

                ' TODO: liberare le risorse non gestite condivise
            End If
            Me.disposedValue = True
        End Sub


        ' Questo codice è aggiunto da Visual Basic per implementare in modo corretto il modello Disposable.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Non modificare questo codice. Inserire il codice di pulitura in Dispose(ByVal disposing As Boolean).
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

        Public Sub OnDeserialization(ByVal sender As Object) Implements System.Runtime.Serialization.IDeserializationCallback.OnDeserialization
            Me.dev_IsLocal = False

        End Sub
    End Class

   
    <Serializable()> Public NotInheritable Class DeviceFile
        Implements IEquatable(Of DeviceFile)

        Dim fl_OriginalName As String
        Dim fl_FullName As String
        Dim fl_Name As String
        Dim fl_Size As Long
        Dim fl_CreationDate As DateTime
        Dim fl_Exist As Boolean
        <NonSerialized()> Dim fl_IsLocal As Boolean

        Public ReadOnly Property IsLocal() As Boolean
            Get
                Return Me.fl_IsLocal
            End Get
        End Property

        Public ReadOnly Property Size() As Long
            Get
                Return Me.fl_Size
            End Get
        End Property

        Public ReadOnly Property CreationDate() As DateTime
            Get
                Return Me.fl_CreationDate
            End Get
        End Property

        Public ReadOnly Property FullName() As String
            Get
                Return Me.fl_FullName
            End Get
        End Property

        Public ReadOnly Property Name() As String
            Get
                Return Me.fl_Name
            End Get
        End Property

        Public Property OriginalFileName() As String
            Get
                Return fl_OriginalName
            End Get
            Private Set(ByVal value As String)
                Me.fl_OriginalName = value
            End Set
        End Property

        Public ReadOnly Property Exist() As Boolean
            Get
                Return Me.fl_Exist
            End Get
        End Property

        Private Sub New(ByVal fInfo As FileInfo)
            Me.fl_Exist = fInfo.Exists
            Me.fl_OriginalName = fInfo.Name
            Me.fl_Size = fInfo.Length
            Me.fl_CreationDate = fInfo.CreationTime
            Me.fl_FullName = fInfo.FullName
            Me.fl_IsLocal = True
            Me.fl_Name = fInfo.Name
        End Sub

        Private Sub New(ByVal filename As String, ByVal oFileName As String)

            If File.Exists(filename) = False Then
                'Crea un device file non esistente
                Me.fl_Exist = False
                Me.fl_OriginalName = oFileName
                Me.fl_Size = 0
                Me.fl_CreationDate = DateTime.Now
                Me.fl_FullName = filename
                Me.fl_IsLocal = True
                Me.fl_Name = Path.GetFileName(filename)
            Else
                Dim fInfo As New FileInfo(filename)
                Me.fl_Exist = fInfo.Exists
                Me.fl_OriginalName = fInfo.Name
                Me.fl_Size = fInfo.Length
                Me.fl_CreationDate = fInfo.CreationTime
                Me.fl_FullName = fInfo.FullName
                Me.fl_IsLocal = True
                Me.fl_Name = fInfo.Name
            End If

            Me.fl_OriginalName = oFileName
        End Sub

        Public Shared Function Create(ByVal fInfo As FileInfo) As DeviceFile
            Try
                If fInfo Is Nothing Then Throw New ArgumentNullException

                Return New DeviceFile(fInfo)

            Catch ex As Exception
                Debug.Print(My.Settings.DebugStringFormat, "DeviceFile::Create", ex.Message)
                Return Nothing
            End Try
        End Function

        Public Shared Function Create(ByVal filename As String, ByVal oFileName As String) As DeviceFile
            Try

                If [String].IsNullOrEmpty(filename) Then Return Nothing

                Return New DeviceFile(filename, oFileName)

            Catch ex As Exception
                Debug.Print(My.Settings.DebugStringFormat, "DeviceFile::Create", ex.Message)
                Return Nothing
            End Try
        End Function

        Public Shared Operator =(ByVal dev1 As DeviceFile, ByVal dev2 As FileInfo) As Boolean

            If dev1 Is Nothing OrElse dev2 Is Nothing Then Return False

            If dev1.FullName <> dev2.FullName Then Return False

            If dev1.Size <> dev2.Length Then Return False

            If dev2.Exists Then Return True

            Return False

        End Operator

        Public Shared Operator <>(ByVal dev1 As DeviceFile, ByVal dev2 As FileInfo) As Boolean
            Return Not dev1 = dev2
        End Operator

        Public Shared Operator =(ByVal dev1 As DeviceFile, ByVal dev2 As DeviceFile) As Boolean

            If dev1 Is Nothing OrElse dev2 Is Nothing Then Return False

            Return (dev1.OriginalFileName.CompareTo(dev2.OriginalFileName) = 0)

        End Operator

        Public Shared Operator <>(ByVal dev1 As DeviceFile, ByVal dev2 As DeviceFile) As Boolean
            Return Not (dev1 = dev2)
        End Operator

        Public Function Equals1(ByVal other As DeviceFile) As Boolean Implements System.IEquatable(Of DeviceFile).Equals
            Return Me = other
        End Function

    End Class


    Public NotInheritable Class DeviceInfFile
        'Implementa un file di installazione INF

        Implements IDisposable


        Private Structure PINFCONTEXT
            Public InfPtr As IntPtr 'opened INF file handle
            Public CurrInfPtr As IntPtr
            Public Section As Integer
            Public Line As Integer
        End Structure

        Public Class SupportedDevice
            Dim SP_DevName As String
            Dim SP_HwID As String
            Dim SP_Manufacturer As String

            Public Sub New(ByVal dvName As String, ByVal hwID As String, ByVal manufacturer As String)
                Me.SP_DevName = dvName
                Me.SP_HwID = hwID
                Me.SP_Manufacturer = manufacturer
            End Sub

            Public ReadOnly Property Manufacturer() As String
                Get
                    Return Me.SP_Manufacturer
                End Get
            End Property

            Public ReadOnly Property HardwareID() As String
                Get
                    Return Me.SP_HwID
                End Get
            End Property

            Public ReadOnly Property DeviceName() As String
                Get
                    Return Me.SP_DevName
                End Get
            End Property

        End Class


        'Configurazione del motore di ricerca file
        Private Const INFINFO_DEFAULT_SEARCH = 3
        Private Const INVALID_HANDLE_VALUE = -1
        Private Const INF_STYLE_WIN4 = &H2
        Private Const CATALOGFILE_EXT = "CatalogFile,CatalogFile.nt,CatalogFile.ntx86,CatalogFile.ntia64,CatalogFile.ntamd64"

        Private Declare Unicode Function SetupOpenInfFile Lib "setupapi.dll" Alias "SetupOpenInfFileW" (ByVal filename As String, ByVal fClass As String, ByVal fStyle As Integer, ByRef ErrorLine As Integer) As Integer
        Private Declare Sub SetupCloseInfFile Lib "setupapi.dll" (ByVal fHandle As IntPtr)
        Private Declare Unicode Function SetupFindFirstLine Lib "setupapi.dll" Alias "SetupFindFirstLineW" (ByVal fHandle As IntPtr, ByVal Section As String, ByVal Key As String, ByRef Context As PINFCONTEXT) As Integer
        Private Declare Unicode Function SetupFindFirstLineNull Lib "setupapi.dll" Alias "SetupFindFirstLineW" (ByVal fHandle As IntPtr, ByVal Section As String, ByVal Key As IntPtr, ByRef Context As PINFCONTEXT) As Integer

        Private Declare Unicode Function SetupFindNextLine Lib "setupapi.dll" (ByRef ContextIn As PINFCONTEXT, ByRef ContextOut As PINFCONTEXT) As Integer
        Private Declare Unicode Function SetupFindNextMatchLine Lib "setupapi.dll" Alias "SetupFindNextMatchLineW" (ByRef ContextIn As PINFCONTEXT, ByVal Key As String, ByRef ContextOut As PINFCONTEXT) As Integer
        Private Declare Unicode Function SetupGetStringField Lib "setupapi.dll" Alias "SetupGetStringFieldW" (ByRef Context As PINFCONTEXT, ByVal FieldIndex As Integer, ByVal ReturnBuffer As StringBuilder, ByVal ReturnBufferSize As Integer, ByRef RequiredSize As Integer) As Integer
        Private Declare Unicode Function SetupGetTargetPath Lib "setupapi.dll" Alias "SetupGetTargetPathW" (ByVal fHandle As IntPtr, ByVal fContext As Integer, ByVal Section As String, ByVal ReturnBuffer As StringBuilder, ByVal ReturnBufferSize As Integer, ByRef RequiredSize As Integer) As Integer
        Private Declare Unicode Function SetupGetFieldCount Lib "setupapi.dll" (ByRef Context As PINFCONTEXT) As Integer
        Private Declare Unicode Function SetupGetLineCount Lib "setupapi.dll" Alias "SetupGetLineCountW" (ByVal fHandle As IntPtr, ByVal Section As String) As Integer
        Private Declare Unicode Function SetupGetLineByIndex Lib "setupapi.dll" Alias "SetupGetLineByIndexW" (ByVal fHandle As IntPtr, ByVal Section As String, ByVal index As Integer, ByRef Context As PINFCONTEXT) As Integer


        Dim m_FileHandle As IntPtr 'Handle del file aperto
        Dim m_ErrorLine As Integer 'Numero della linea in cui si è verificato l'ultimo errore

        'Informazioni principali del file
        Dim m_Signature As String
        Dim m_Class As String
        Dim m_ClassGuid As String
        Dim m_layoutFiles As New List(Of FileInfo) 'File layout
        Dim m_catalogFiles As New List(Of FileInfo) 'File catalogo
        Dim m_Version As Version
        Dim m_ReleaseDate As DateTime
        Dim m_isValid As Boolean
        Dim m_WHQL As Boolean
        Dim m_Provider As String
        Dim m_fInfo As FileInfo
#Region "ReadOnly Properties"
        Public ReadOnly Property ProviderName() As String
            Get
                Return Me.m_Provider
            End Get
        End Property

        Public ReadOnly Property IsWHQLSigned() As String
            Get
                Return Me.m_WHQL
            End Get
        End Property

        Public ReadOnly Property FileVersion() As Version
            Get
                Return Me.m_Version
            End Get
        End Property

        Public ReadOnly Property ReleaseDate() As DateTime
            Get
                Return Me.m_ReleaseDate
            End Get
        End Property

        Public ReadOnly Property CatalogFiles() As List(Of FileInfo)
            Get
                Return Me.m_catalogFiles
            End Get
        End Property

        Public ReadOnly Property LayoutFiles() As List(Of FileInfo)
            Get
                Return Me.m_layoutFiles
            End Get
        End Property

        Public ReadOnly Property ClassGuid() As String
            Get
                Return Me.m_ClassGuid
            End Get
        End Property

        Public ReadOnly Property ClassName() As String
            Get
                Return Me.m_Class
            End Get
        End Property

        Public ReadOnly Property Signature() As String
            Get
                Return Me.m_Signature
            End Get
        End Property


        Public ReadOnly Property InfFile() As FileInfo
            Get
                Return Me.m_fInfo
            End Get
        End Property
#End Region

        Private Sub New(ByVal fHandle As Integer, ByVal fInfo As FileInfo)
            Me.m_FileHandle = fHandle
            Me.m_fInfo = fInfo
            'Carica tutte le informazioni del file INF
            LoadInfInfo()
        End Sub

        Private Sub LoadInfInfo()

            Dim cont As PINFCONTEXT
            Dim strBuff As String

            Try

                If SetupFindFirstLine(Me.m_FileHandle, "Version" & ControlChars.NullChar, "Signature" & ControlChars.NullChar, cont) <> 0 Then
                    'Legge il valore della proprietà Signature
                    strBuff = GetStrField(cont, 1, [String].Empty)
                    If [String].IsNullOrEmpty(strBuff) Then
                        'File inf senza firma. Non valido
                        Me.m_isValid = False
                        Return
                    Else
                        Me.m_Signature = strBuff
                    End If
                End If

                If SetupFindFirstLine(Me.m_FileHandle, "Version" & ControlChars.NullChar, "Class" & ControlChars.NullChar, cont) <> 0 Then
                    'Legge il valore della proprietà Class
                    Me.m_Class = GetStrField(cont, 1, [String].Empty)
                End If

                If SetupFindFirstLine(Me.m_FileHandle, "Version" & ControlChars.NullChar, "ClassGuid" & ControlChars.NullChar, cont) <> 0 Then
                    'Legge il valore della proprietà ClassGuid
                    Me.m_ClassGuid = GetStrField(cont, 1, [String].Empty)
                End If

                If SetupFindFirstLine(Me.m_FileHandle, "Version" & ControlChars.NullChar, "Provider" & ControlChars.NullChar, cont) <> 0 Then
                    'Legge il valore della proprietà Provider
                    Me.m_Provider = GetStrField(cont, 1, [String].Empty)
                End If

                If SetupFindFirstLine(Me.m_FileHandle, "Version" & ControlChars.NullChar, "DriverVer" & ControlChars.NullChar, cont) <> 0 Then
                    'Legge la versione del driver e la release date
                    Try
                        Dim dt As String = GetStrField(cont, 1, "")

                        If Not [String].IsNullOrEmpty(dt) Then
                            Dim culture As New CultureInfo("en-US")
                            Me.m_ReleaseDate = DateTime.Parse(dt, culture, DateTimeStyles.NoCurrentDateDefault)
                        End If

                        Me.m_Version = New Version(GetStrField(cont, 2, [String].Empty))

                    Catch ex As Exception
                        Me.m_Version = New Version(0, 0, 0, 0)
                    End Try
                End If


                If SetupFindFirstLine(Me.m_FileHandle, "Version" & ControlChars.NullChar, "LayoutFile" & ControlChars.NullChar, cont) <> 0 Then
                    'Elenca i files di Layout
                    Dim fCount As Integer = 0
                    Dim fNameBuff As String

                    strBuff = GetStrField(cont, fCount, [String].Empty)

                    Do Until [String].IsNullOrEmpty(strBuff)
                        'Cicla sui files
                        fNameBuff = Utils.FindFileInMainPaths(strBuff)

                        If [String].IsNullOrEmpty(fNameBuff) = False Then
                            Me.m_layoutFiles.Add(New FileInfo(fNameBuff))
                        End If

                        fCount += 1
                        strBuff = GetStrField(cont, fCount, [String].Empty)
                    Loop
                End If

                Me.m_WHQL = False

                For Each strSect As String In My.Settings.CatalogSections.Split(","c)
                    'Cicla sulle possibili sezioni di CatalogFile
                    If SetupFindFirstLine(Me.m_FileHandle, "Version" & ControlChars.NullChar, strSect & ControlChars.NullChar, cont) <> 0 Then
                        'Se la sezione è stata correttamente aperta legge il file e cerca il percorso
                        Dim fNameBuff As String = GetStrField(cont, 1, [String].Empty)

                        fNameBuff = Utils.FindFileInMainPaths(fNameBuff) 'ricava il percorso del file

                        If [String].IsNullOrEmpty(fNameBuff) = False Then
                            Me.m_catalogFiles.Add(New FileInfo(fNameBuff))
                            Me.m_WHQL = True
                        End If

                    End If
                Next

            Catch ex As Exception
            End Try
        End Sub

        Public Function GetFilesFromSection(ByVal installSectionName As String) As List(Of DeviceFile)
            'Ritorna la lista dei files appartenenti alla sezione specificata
            Dim fileList As New List(Of DeviceFile)
            Dim cont As New PINFCONTEXT
            Dim contOut As New PINFCONTEXT
            Dim argBuff As String
            Dim fNameBuff As String
            Dim fBuff As DeviceFile
            Dim index As Integer

            Try
                'Processa il TAG include
                ' Tag Include=file1.inf,[file2.inf]
                For Each extraSect As String In My.Settings.CopyFilesSections.Split(",")
                    If SetupFindFirstLine(Me.m_FileHandle, installSectionName & ControlChars.NullChar, "Include" & ControlChars.NullChar, cont) <> 0 Then
                        argBuff = GetStrField(cont)
                        index = 1
                        Do Until [String].IsNullOrEmpty(argBuff)
                            fNameBuff = Utils.FindFileInMainPaths(argBuff) 'trova il percorso
                            fBuff = DeviceFile.Create(fNameBuff, argBuff)
                            If fBuff IsNot Nothing Then fileList.Add(fBuff)
                            index += 1
                            argBuff = GetStrField(cont, index)
                        Loop
                    End If
                Next

                'Processa CopyFiles multipli

                If SetupFindFirstLine(Me.m_FileHandle, installSectionName & ControlChars.NullChar, "CopyFiles" & ControlChars.NullChar, cont) <> 0 Then
                    Do

                        'Tag CopyFiles= section-list1,[section-list2]
                        argBuff = GetStrField(cont)
                        index = 1

                        Do Until [String].IsNullOrEmpty(argBuff)

                            If argBuff.StartsWith("@") Then
                                'Si tratta di un file singolo
                                fNameBuff = argBuff.Substring(2) 'Tronca la chiocciola

                                Dim pathBuff As String = Path.Combine(GetDirFromSection(), fNameBuff)

                                If File.Exists(pathBuff) = False Then
                                    pathBuff = Utils.FindFileInMainPaths(fNameBuff)
                                End If
                                fileList.Add(DeviceFile.Create(pathBuff, fNameBuff))

                            Else
                                'Si tratta di una lista di files
                                Dim filelistCont As New PINFCONTEXT
                                Dim originalName As String

                                If SetupFindFirstLine(Me.m_FileHandle, argBuff & ControlChars.NullChar, Nothing, filelistCont) <> 0 Then
                                    'processa la lista solo se la trova nel file
                                    fNameBuff = GetStrField(filelistCont) 'Nome file destinazione
                                    Dim pathBuff As String = Path.Combine(GetDirFromSection(argBuff), fNameBuff)

                                    If File.Exists(pathBuff) = False Then
                                        pathBuff = Utils.FindFileInMainPaths(fNameBuff)
                                    End If

                                    originalName = GetStrField(filelistCont, 2)

                                    If [String].IsNullOrEmpty(originalName) = False Then fNameBuff = originalName

                                    fileList.Add(DeviceFile.Create(pathBuff, fNameBuff))
                                    'processa le altre linee

                                    Do While SetupFindNextLine(filelistCont, filelistCont) <> 0
                                        fNameBuff = GetStrField(filelistCont) 'Nome file destinazione
                                        pathBuff = Path.Combine(GetDirFromSection(argBuff), fNameBuff)

                                        If File.Exists(pathBuff) = False Then
                                            pathBuff = Utils.FindFileInMainPaths(fNameBuff)
                                        End If

                                        originalName = GetStrField(filelistCont, 2)
                                        If [String].IsNullOrEmpty(originalName) = False Then fNameBuff = originalName
                                        fileList.Add(DeviceFile.Create(pathBuff, fNameBuff))
                                    Loop

                                End If
                            End If
                            'Processa una nuova voce di CopyFiles
                            index += 1
                            argBuff = GetStrField(cont, index)
                        Loop

                        If SetupFindNextMatchLine(cont, "CopyFiles" & ControlChars.NullChar, contOut) <> 0 Then
                            cont = contOut
                        Else
                            Exit Do
                        End If

                    Loop

                End If

            Return fileList

            Catch ex As Exception
                Return fileList
            End Try
        End Function


        Private Function GetDirFromSection(Optional ByVal sectionName As String = "DefaultDestDir", Optional ByVal defaultValue As String = "") As String
            'Ricava la directory destinazione della sezione specificata
            'Prova con il metodo automatico
            Dim strBuff As New StringBuilder(My.Settings.MaxStrBufferSize)
            Dim cont As New PINFCONTEXT
            Try


                SetupGetTargetPath(Me.m_FileHandle, 0, sectionName & ControlChars.NullChar, strBuff, strBuff.Capacity, 0)

                If strBuff.Length > 0 Then
                    Dim dName As String
                    If Not [String].IsNullOrEmpty(Utils.CurrentSysDir) Then
                        dName = Utils.ReplaceSysDir(strBuff.ToString)
                    Else
                        dName = strBuff.ToString
                    End If

                    If Directory.Exists(dName) Then Return dName

                End If

                    'Procede manualmente

                    If SetupFindFirstLine(Me.m_FileHandle, "DestinationDirs" & ControlChars.NullChar, sectionName & ControlChars.NullChar, cont) <> 0 Then
                        'Ricava gli argomenti di default
                    Select Case SetupGetFieldCount(cont)

                        Case Is = 1
                            Dim dirid As String = GetStrField(cont)
                            If [String].IsNullOrEmpty(dirid) = False AndAlso Utils.DefaultDirs.ContainsKey(dirid) Then
                                Return Utils.DefaultDirs.Item(dirid)
                            End If
                        Case Is = 2

                            Dim dirid As String = GetStrField(cont)
                            Dim dirP As String

                            If [String].IsNullOrEmpty(dirid) = False AndAlso Utils.DefaultDirs.ContainsKey(dirid) Then
                                dirP = Path.Combine(Utils.DefaultDirs.Item(dirid), GetStrField(cont, 2))

                                If Directory.Exists(dirP) = True Then
                                    Return dirP
                                End If
                            End If
                        Case Else
                            'Formato errato
                    End Select
                    Else

                    End If

                    Return defaultValue

            Catch ex As Exception
                Return defaultValue

            End Try
        End Function

        Public Shared Function OpenInfFile(ByVal infName As String) As DeviceInfFile
            'Crea l'oggetto file inf da un file
            Try
                Dim fHandle As Integer
                fHandle = SetupOpenInfFile(infName, Nothing, INF_STYLE_WIN4, 0)

                If fHandle <> INVALID_HANDLE_VALUE Then
                    Return New DeviceInfFile(fHandle, New FileInfo(infName)) 'Ritorna il file aperto
                Else
                    Throw New Exception("Inf file is damaged or corrupted. " & infName)
                End If

            Catch ex As Exception
                Debug.Print(My.Settings.DebugStringFormat, "InfFile::OpenInfFile", ex.Message)
                Return Nothing
            End Try
        End Function

        Private Function GetStrField(ByRef cnt As PINFCONTEXT, Optional ByVal fieldCount As Integer = 1, Optional ByVal defaultValue As String = "") As String

            Try
                Dim buff As New StringBuilder(My.Settings.MaxStrBufferSize)

                If SetupGetStringField(cnt, fieldCount, buff, buff.Capacity, 0) <> 0 Then
                    'Stringa ritornata con successo
                    Return buff.ToString
                Else
                    'Errore
                    Return defaultValue
                End If

            Catch ex As Exception
                Return defaultValue
            End Try

        End Function

        Private disposedValue As Boolean = False        ' Per rilevare chiamate ridondanti

        ' IDisposable
        Protected Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: liberare le risorse gestite chiamate in modo esplicito

                    SetupCloseInfFile(Me.m_FileHandle) 'Chiude il file inf aperto
                    Me.m_FileHandle = New IntPtr(-1) 'Imposta un'handle non valido

                End If

                ' TODO: liberare le risorse non gestite condivise
            End If
            Me.disposedValue = True
        End Sub


        Public Function GetSupportedDevices() As List(Of SupportedDevice)
            Dim lst As New List(Of SupportedDevice)
            If GetSupportedDevices(lst) = True Then
                Return lst
            Else
                Return Nothing
            End If
        End Function

        Public Function GetSupportedDevices(ByVal lst As List(Of SupportedDevice)) As Integer
            'Ritorna al chiamante una lista con tutti i dispositivi supportati dal file installazione

            Dim manufCount As Integer
            Dim manufName As String
            Dim currLine As PINFCONTEXT
            Dim currLineFields As Integer
            Dim currDevSection As String
            Dim currDevCount As Integer
            Dim currDevCxt As PINFCONTEXT
            Dim currDevName As String
            Dim currDevHwID As String
            Dim foundDevs As Integer = 0

            If lst Is Nothing Then Return 0

            manufCount = SetupGetLineCount(Me.m_FileHandle, "Manufacturer" & ControlChars.NullChar)

            If manufCount <= 0 Then
                'Sezione vuota o non esistente
                Return 0
            End If

            For line As Integer = 0 To manufCount - 1
                'Processa le linee manufacturer
                If SetupGetLineByIndex(Me.m_FileHandle, "Manufacturer" & ControlChars.NullChar, line, currLine) <> 0 Then
                    'legge le sezioni supportate
                    currLineFields = SetupGetFieldCount(currLine)
                    If currLineFields <= 0 Then Continue For 'Linea non valida

                    manufName = GetStrField(currLine, 0) 'Nome del produttore corrente

                    For field As Integer = 1 To currLineFields
                        'Processa le sezioni di devices compatibili ad una ad una
                        'Nome della sezione corrente
                        currDevSection = GetStrField(currLine, field)
                        If [String].IsNullOrEmpty(currDevSection) Then Continue For
                        'Devices contenuti nella sezione corrente
                        currDevCount = SetupGetLineCount(Me.m_FileHandle, currDevSection & ControlChars.NullChar)
                        If currDevCount <= 0 Then Continue For 'Nessun device
                        'Processa i devices e carica la lista
                        For dev As Integer = 0 To currDevCount - 1
                            If SetupGetLineByIndex(Me.m_FileHandle, currDevSection & ControlChars.NullChar, dev, currDevCxt) <> 0 Then
                                'Linea device letta con successo
                                If SetupGetFieldCount(currDevCxt) <> 2 Then Continue For 'Skip del device

                                currDevName = GetStrField(currDevCxt, 0)
                                currDevHwID = GetStrField(currDevCxt, 2)

                                If [String].IsNullOrEmpty(currDevName) OrElse [String].IsNullOrEmpty(currDevHwID) Then Continue For 'Skip del device
                                'Aggiunge il device alla lista
                                lst.Add(New SupportedDevice(currDevName, currDevHwID, manufName))
                                foundDevs += 1
                            End If
                        Next ' Fine for device
                    Next 'Fine for sezione
                End If
            Next 'Fine for manufacturer

            Return foundDevs

        End Function

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
