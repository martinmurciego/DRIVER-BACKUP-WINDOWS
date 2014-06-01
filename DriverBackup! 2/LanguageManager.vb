
Public Class LanguageManager


    Public Const StdBindingFlags As Integer = BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.Static Or BindingFlags.GetField

    Public Const StdControlPropsFlags As Integer = BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.Static


    <AttributeUsage(AttributeTargets.Class Or AttributeTargets.Field Or AttributeTargets.Property)> Public Class LanguageAttribute
        'Segnala al manager come trattare il tipo che lo espone
        Inherits Attribute

        Dim m_strArr As Boolean = False
        Dim m_exclude As Boolean = False

        Public Property Exclude() As Boolean
            Get
                Return Me.m_exclude
            End Get
            Set(ByVal value As Boolean)
                Me.m_exclude = value
            End Set
        End Property

        Public Property DictionaryOfStrings() As Boolean
            Get
                Return Me.m_strArr
            End Get
            Set(ByVal value As Boolean)
                Me.m_strArr = value
            End Set
        End Property

        Public Sub New()
            'Costruttore senza parametri
        End Sub
    End Class


    Public Shared Sub GetControlTree(ByVal rootCtrl As Control, ByRef lst As List(Of Control))
        If rootCtrl Is Nothing OrElse lst Is Nothing OrElse lst.Contains(rootCtrl) Then Return

        For Each ctrl As Control In rootCtrl.Controls
            lst.Add(ctrl)
            GetControlTree(ctrl, lst)
        Next
    End Sub


    Public Shared Function GetObjectProps(ByVal obj As Object, ByVal propertyFilter As String, ByVal flags As BindingFlags) As List(Of PropertyInfo)

        Dim lst As New List(Of PropertyInfo)

        If flags <= 0 Then flags = StdBindingFlags

        For Each prpInfo As PropertyInfo In obj.GetType.GetProperties(flags)
            If Regex.IsMatch(prpInfo.Name, propertyFilter) AndAlso prpInfo.PropertyType Is GetType([String]) AndAlso prpInfo.CanRead AndAlso prpInfo.CanWrite AndAlso prpInfo.GetIndexParameters.GetLength(0) <= 0 Then
                'La proprietà ha tutte le caratteristiche richieste
                lst.Add(prpInfo)
            End If
        Next

        Return lst

    End Function

    Public Class LanguageFileWriter

        Implements IDisposable

        Private m_xmlWriter As XmlTextWriter
        Private m_xmlStream As MemoryStream 'Buffer temporaneo
        Private m_Filename As String
        Private m_Objects As Dictionary(Of String, ObjectContainer)
        Private m_BindingFlags As BindingFlags
        Private m_languageName As String
        Private langAuthor As String

        Public Property LanguageName() As String
            Get
                Return m_languageName
            End Get
            Set(ByVal value As String)
                Me.m_languageName = value
            End Set
        End Property


        Public Property Author() As String
            Get
                Return langAuthor
            End Get
            Set(ByVal value As String)
                Me.langAuthor = value
            End Set
        End Property

        Public Property TypeBindingFlags() As BindingFlags
            Get
                Return Me.m_BindingFlags
            End Get
            Set(ByVal value As BindingFlags)
                Me.m_BindingFlags = value
            End Set
        End Property

        Public Sub RemoveObject(ByVal objName As String)
            If Me.m_Objects.ContainsKey(objName) Then Me.m_Objects.Remove(objName)
        End Sub

        Public Function AddCustomArr(ByVal containerName As String, ByVal arrName As String, ByVal arr As Dictionary(Of String, String)) As Boolean
            'Crea un container virtuale e assegna l'array custom
            Dim objCont As New ObjectContainer(containerName)

            If Me.m_Objects.ContainsKey(containerName) Then Return False
            'Aggiunge l'array di stringhe
            objCont.StringArrays.Add(arrName, arr)
            'Aggiunge l'oggetto
            Me.m_Objects.Add(containerName, objCont)

            Return True

        End Function


        Public Function AddObject(ByVal frm As Object, ByVal bFlags As BindingFlags, ByVal memberFilterType() As Type, ByVal memberFilter As String, ByVal propertyFilter As String, ByVal findCustomArrs As Boolean)
            Dim ctrlTree As New List(Of Control)
            Dim objCont As New ObjectContainer(frm.GetType.Name)

            Try

                If bFlags <= 0 Then bFlags = StdBindingFlags

                If Me.m_Objects.ContainsKey(frm.GetType.Name) Then Return True

                'Aggiunge le proprietà del form stesso
                For Each prpInfo As PropertyInfo In GetObjectProps(frm, propertyFilter, bFlags)
                    objCont.Properties.Add(prpInfo.Name, prpInfo.GetValue(frm, Nothing))
                Next

                For Each mbmInfo As MemberInfo In frm.GetType.GetMembers(bFlags)
                    Try
                        Dim objValue As Object
                        Dim objProps As New Dictionary(Of String, String)

                        Dim attr As LanguageAttribute = DirectCast(Attribute.GetCustomAttribute(mbmInfo, GetType(LanguageAttribute)), LanguageAttribute)

                        If attr IsNot Nothing AndAlso attr.Exclude Then Continue For

                        If mbmInfo.MemberType <> MemberTypes.Field Then Continue For

                        objValue = DirectCast(mbmInfo, FieldInfo).GetValue(frm)
                        'Verifica se il tipo del membro è un controllo o se è un'array di stringhe custom
                        If attr IsNot Nothing AndAlso attr.DictionaryOfStrings AndAlso findCustomArrs AndAlso objValue.GetType Is GetType(Dictionary(Of String, String)) Then
                            'Processa l'array custom
                            objCont.StringArrays.Add(mbmInfo.Name, objValue)
                        Else

                            If objValue IsNot Nothing AndAlso Array.Exists(Of Type)(memberFilterType, AddressOf objValue.GetType.IsSubclassOf) AndAlso Regex.IsMatch(mbmInfo.Name, memberFilter) Then
                                For Each prpInfo As PropertyInfo In GetObjectProps(objValue, propertyFilter, StdControlPropsFlags)
                                    objProps.Add(prpInfo.Name, prpInfo.GetValue(objValue, Nothing))
                                Next
                                If Not objCont.SubControls.ContainsKey(mbmInfo.Name) Then objCont.SubControls.Add(mbmInfo.Name, objProps)
                            End If
                        End If
                    Catch mbmEx As Exception
                        'Eccezzione durante l'eleborazione del membro
                        Continue For
                    End Try

                Next

                Me.m_Objects.Add(frm.GetType.Name, objCont)

                Return True
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
                Return False
            End Try
        End Function

        Public Sub New(ByVal fileName As String)
            Me.m_Filename = fileName
            Me.m_xmlStream = New MemoryStream()
            Me.m_xmlWriter = New XmlTextWriter(Me.m_xmlStream, Encoding.Unicode)
            Me.m_Objects = New Dictionary(Of String, ObjectContainer)
        End Sub

        Private Sub WriteStrArray(ByVal name As String, ByVal value As Dictionary(Of String, String), ByVal w As XmlTextWriter)
          
            If value Is Nothing OrElse [String].IsNullOrEmpty(name) Then Return

            w.WriteStartElement(name)

            For Each strV As KeyValuePair(Of String, String) In value
                w.WriteElementString(strV.Key, strV.Value)
            Next

            w.WriteEndElement()

        End Sub

        Private Function GenerateXMLCode() As Boolean
            'Genera il codice XML che verrà memorizzato sul file
            Me.m_xmlStream = New MemoryStream
            Me.m_xmlWriter = New XmlTextWriter(Me.m_xmlStream, Encoding.Unicode)

            With Me.m_xmlWriter
                Try
                    .Formatting = Formatting.Indented 'Indenta
                    .WriteStartDocument() 'Scrive il tag iniziale
                    'Scrive l'identificatore

                    .WriteStartElement("OBJECTSCOLLECTION") 'Tag inizio collezione

                    .WriteStartElement("IDENTIFIER")
                    .WriteElementString("IDDRVBK", "DriverBackup Language File")
                    .WriteElementString("AUTHOR", Author)
                    .WriteElementString("LANGUAGE", LanguageName)
                    .WriteEndElement()

                    For Each keyValue As KeyValuePair(Of String, ObjectContainer) In Me.m_Objects
                        Dim frmCont As ObjectContainer = keyValue.Value
                        .WriteStartElement(keyValue.Key) 'Tag inizio form

                        WriteStrArray("PROPERTIES", frmCont.Properties, Me.m_xmlWriter)

                        'Processa SubControl
                        .WriteStartElement("CONTROLS")
                        For Each ctrlV As KeyValuePair(Of String, Object) In frmCont.SubControls
                            WriteStrArray(ctrlV.Key, DirectCast(ctrlV.Value, Dictionary(Of String, String)), Me.m_xmlWriter)
                        Next
                        .WriteEndElement()

                        'Processa StringArrs
                        .WriteStartElement("CUSTOMARRS")
                        For Each ctrlV As KeyValuePair(Of String, Object) In frmCont.StringArrays
                            WriteStrArray(ctrlV.Key, DirectCast(ctrlV.Value, Dictionary(Of String, String)), Me.m_xmlWriter)
                        Next
                        .WriteEndElement()

                        .WriteEndElement() 'Tag fine form
                    Next


                    .WriteEndElement() 'Tag fine collezione
                    Return True
                Catch ex As Exception
                Finally
                    .WriteEndDocument()
                    .Flush()
                End Try
            End With

        End Function


        Public Function WriteToFile(ByVal filename As String) As Boolean
            Try
                If File.Exists(Me.m_Filename) OrElse Not GenerateXMLCode() Then Return False

                Using fs As New FileStream(Me.m_Filename, FileMode.OpenOrCreate)
                    Me.m_xmlStream.WriteTo(fs)
                    fs.Flush()
                End Using

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function WriteToFile() As Boolean
            Return WriteToFile(Me.m_Filename)
        End Function

        Private disposedValue As Boolean = False        ' Per rilevare chiamate ridondanti

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: liberare le risorse gestite chiamate in modo esplicito
                    Me.m_xmlWriter.Close()
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


    Public Class ObjectContainer

        Dim tp_Name As String
        Dim tp_Properties As Dictionary(Of String, String)
        Dim tp_StrArrs As Dictionary(Of String, Object)
        Dim tp_SubControls As Dictionary(Of String, Object)

        Public Sub New(ByVal tpName As String)
            Me.tp_Name = tpName
            Me.tp_Properties = New Dictionary(Of String, String)
            Me.tp_StrArrs = New Dictionary(Of String, Object)
            Me.tp_SubControls = New Dictionary(Of String, Object)
        End Sub

        Public ReadOnly Property Name() As String
            Get
                Return Me.tp_Name
            End Get
        End Property

        Public ReadOnly Property SubControls() As Dictionary(Of String, Object)
            Get
                Return Me.tp_SubControls
            End Get
        End Property

        Public Property Properties() As Dictionary(Of String, String)
            Get
                Return Me.tp_Properties
            End Get
            Set(ByVal val As Dictionary(Of String, String))
                Me.tp_Properties = val
            End Set
        End Property

        Public ReadOnly Property StringArrays() As Dictionary(Of String, Object)
            Get
                Return Me.tp_StrArrs
            End Get
        End Property

    End Class

    Public Class LanguageFileReader
        Dim xmldoc As XmlDocument

        Dim m_isValid As Boolean
        Dim m_langName As String
        Dim m_Author As String

        Public ReadOnly Property IsValid() As Boolean
            Get
                Return Me.m_isValid
            End Get
        End Property

        Public ReadOnly Property LanguageName() As String
            Get
                Return Me.m_langName
            End Get
        End Property

        Public ReadOnly Property Author() As String
            Get
                Return Me.m_Author
            End Get
        End Property


        Private Function ReadIdentifier() As Boolean
            Try
                Dim ndId As XmlNode = xmldoc.Item("OBJECTSCOLLECTION").Item("IDENTIFIER")
                Dim nd As XmlNode

                If ndId Is Nothing Then Return False

                nd = ndId.Item("IDDRVBK")
                If nd IsNot Nothing Then Me.m_isValid = True

                nd = ndId.Item("AUTHOR")
                If nd IsNot Nothing Then Me.m_Author = nd.InnerText

                nd = ndId.Item("LANGUAGE")
                If nd IsNot Nothing Then Me.m_langName = nd.InnerText
                Return True
            Catch ex As Exception
                Return False
            End Try

        End Function

        Private Sub New(ByVal filename As String)
            Me.xmldoc = New XmlDocument
            Me.xmldoc.Load(filename)
            Me.m_isValid = ReadIdentifier()
        End Sub

        Public Shared Function LoadLanguageFile(ByVal filename As String) As LanguageFileReader
            Try
                Return New LanguageFileReader(filename)
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Public Function LoadLanguageOnForm(ByVal frm As Form, ByVal bFlags As BindingFlags, ByVal memberFilterType() As Type, ByVal memberFilter As String, ByVal propertyFilter As String, ByVal loadCustomArrs As Boolean) As Boolean

            Dim ctrlTree As New List(Of Control)
            Dim objCont As ObjectContainer

            Try

                If bFlags <= 0 Then bFlags = StdBindingFlags
                'Carica le risorse dal file e dopo processa le proprietà, i controlli e gli array custom
                objCont = ReadContainer(frm.Name)

                If objCont Is Nothing Then Return False

                'Carica le proprietà del form stesso
                For Each prpInfo As PropertyInfo In GetObjectProps(frm, propertyFilter, bFlags)
                    'Cerca la proprietà nella lista del formcontainer
                    If prpInfo.CanWrite Then
                        If objCont.Properties.ContainsKey(prpInfo.Name) Then
                            prpInfo.SetValue(frm, objCont.Properties.Item(prpInfo.Name), Nothing)
                        End If
                    End If
                Next

                For Each mbmInfo As MemberInfo In frm.GetType.GetMembers(bFlags)
                    Try
                        Dim objValue As Object
                        Dim attr As LanguageAttribute = DirectCast(Attribute.GetCustomAttribute(mbmInfo, GetType(LanguageAttribute)), LanguageAttribute)
                        'Il membro deve essere escluso
                        If attr IsNot Nothing AndAlso attr.Exclude Then Continue For
                        'Il tipo del membro non è trattabile
                        If mbmInfo.MemberType <> MemberTypes.Field Then Continue For

                        objValue = DirectCast(mbmInfo, FieldInfo).GetValue(frm)
                        'Il membro non contiene un riferimento ad oggetto
                        If objValue Is Nothing Then Continue For
                        'Verifica se il tipo del membro deve essere processato o se è un'array di stringhe custom
                        If attr IsNot Nothing AndAlso attr.DictionaryOfStrings AndAlso loadCustomArrs AndAlso objValue.GetType Is GetType(Dictionary(Of String, String)) Then
                            If objCont.StringArrays.ContainsKey(mbmInfo.Name) Then
                                objValue = objCont.StringArrays.Item(mbmInfo.Name)
                            End If
                        Else
                            If Array.Exists(Of Type)(memberFilterType, AddressOf objValue.GetType.IsSubclassOf) Then 'AndAlso Regex.IsMatch(mbmInfo.Name, memberFilter) Then
                                If objCont.SubControls.ContainsKey(mbmInfo.Name) Then
                                    'Carica le proprietà dell'oggetto
                                    Dim objProps As Dictionary(Of String, String) = DirectCast(objCont.SubControls.Item(mbmInfo.Name), Dictionary(Of String, String))

                                    For Each objPrp As PropertyInfo In GetObjectProps(objValue, propertyFilter, StdControlPropsFlags)
                                        If objProps.ContainsKey(objPrp.Name) Then
                                            objPrp.SetValue(objValue, objProps.Item(objPrp.Name), Nothing)
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    Catch mbmEx As Exception
                        'Eccezzione durante l'eleborazione del membro
                        Continue For
                    End Try

                Next

                Return True
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
                Return False
            End Try

        End Function

        Public Function ReadContainer(ByVal containerName As String) As ObjectContainer
            Try
                If [String].IsNullOrEmpty(containerName) Then Return Nothing

                Dim frmCont As New ObjectContainer(containerName)
                Dim ndObjCollection As XmlNode = xmldoc.Item("OBJECTSCOLLECTION")

                If ndObjCollection Is Nothing Then Return Nothing

                Dim frmNode As XmlNode = ndObjCollection.Item(containerName)

                'Legge le proprietà principali

                frmCont.Properties = ReadStrArray(frmNode.Item("PROPERTIES"))

                'Legge i controlli
                If frmNode.Item("CONTROLS") IsNot Nothing Then
                    For Each ctrlNode As XmlNode In frmNode.Item("CONTROLS")
                        If frmCont.SubControls.ContainsKey(ctrlNode.Name) Then Continue For
                        frmCont.SubControls.Add(ctrlNode.Name, ReadStrArray(ctrlNode))
                    Next
                End If

                'Legge gli array di stringhe
                If frmNode.Item("CUSTOMARRS") IsNot Nothing Then
                    For Each strNode As XmlNode In frmNode.Item("CUSTOMARRS")
                        frmCont.StringArrays.Add(strNode.Name, ReadStrArray(strNode))
                    Next
                End If

                Return frmCont
            Catch ex As Exception
                Debug.WriteLine("LanguageFileReader::ReadContainer" & ex.Message)
                Return Nothing
            End Try
        End Function

        Private Function ReadStrArray(ByVal arrNode As XmlNode) As Dictionary(Of String, String)
            'Legge un'array di stringhe dalla gerarchia di nodi specificata
            Dim props As New Dictionary(Of String, String)

            Try

                If arrNode Is Nothing Then Return props

                For Each xN As XmlNode In arrNode.ChildNodes
                    props.Add(xN.Name, xN.InnerText)
                Next

                Return props
            Catch ex As Exception
                Return props
            End Try
        End Function


    End Class



End Class
