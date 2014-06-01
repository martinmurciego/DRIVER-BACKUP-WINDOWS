
Imports DriverBackup__2.DeviceBackupRestore
Imports DriverBackup__2.DeviceManagement

Public Class TextFormatters



    Public Interface ITextFormatter
        Sub BeginOperation(ByVal comment As String)
        Sub EndOperation(ByVal comment As String)
        Sub AddDevice(ByVal dev As Device)
        Sub AddFile(ByVal filename As String)
        Sub AddError(ByVal code As BackupRestoreErrorCodes, ByVal isDevError As Boolean)
        Sub AddMsgError(ByVal msgString As String, ByVal isDevError As Boolean)
        Sub AddMsg(ByVal msgString As String, ByVal isDev As Boolean)
        Sub EndDevice(ByVal hasErrors As Boolean)
    End Interface

    Public Class TXTFormatter
        Implements IDisposable
        Implements ITextFormatter

        'Formattatore semplice di testi in formato TXT

        Dim outStream As MemoryStream
        Dim writer As StreamWriter
        Private disposedValue As Boolean = False        ' Per rilevare chiamate ridondanti


        Public Sub New()
            Me.outStream = New MemoryStream
            Me.writer = New StreamWriter(Me.outStream)
        End Sub

        Public Function Write(ByVal filename As String) As Boolean
            Try
                filename = Environment.ExpandEnvironmentVariables(filename)

                Dim fStream As New FileStream(filename, FileMode.Append)
                Me.outStream.Flush()
                Me.outStream.WriteTo(fStream)
                fStream.Flush()
                fStream.Close()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

#Region "DisposableSupport"
        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: liberare le risorse gestite chiamate in modo esplicito
                End If

                ' TODO: liberare le risorse non gestite condivise
                outStream.Close()

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
        Public Sub EndDevice(ByVal hasErrors As Boolean) Implements ITextFormatter.EndDevice
            Try
                writer.WriteLine()
                If hasErrors Then
                    writer.WriteLine(GetLangStr("LOG_DeviceError"))
                Else
                    writer.WriteLine(GetLangStr("LOG_DeviceOK"))
                End If
            Catch ex As Exception

            End Try
        End Sub

        Public Sub AddDevice(ByVal dev As DeviceManagement.Device) Implements ITextFormatter.AddDevice
            'Scrive sul flusso in uscita le informazioni principali sul device
            Try
                '"\n{0}\n\tClasse: {1}\n\tProduttore:  {2}\n\tVersione: {3}\n\tData rilascio: {4}\n\tFile installazione:  {5}\n\tFile totali:  {6}\n"
                writer.Write([String].Format(GetLangStr("LOG_Device"), dev.Description, dev.ClassInfo.ClassDescription, dev.ProviderName, dev.ReleaseVersion, dev.ReleaseDate, dev.InstallationFile, dev.DriverFiles.Count))
            Catch ex As Exception
            End Try
        End Sub

        Public Sub AddError(ByVal code As DeviceBackupRestore.BackupRestoreErrorCodes, ByVal isDevError As Boolean) Implements ITextFormatter.AddError

            If isDevError Then
                writer.WriteLine([String].Format(ControlChars.Tab & "ERR: {0}", GetLangStr(code.ToString)))
            Else
                writer.WriteLine([String].Format("ERR: {0}", GetLangStr(code.ToString)))
            End If
        End Sub

        Public Sub AddFile(ByVal filename As String) Implements ITextFormatter.AddFile
            Try
                writer.WriteLine([String].Format(ControlChars.Tab & "--" & GetLangStr("FRMBACK:FILECOPIED"), filename))

            Catch ex As Exception

            End Try
        End Sub

        Public Sub BeginOperation(ByVal comment As String) Implements ITextFormatter.BeginOperation
            Try
                'Scrive le informazioni sul computer in uso
                writer.WriteLine("DriverBackup! " & My.Application.Info.Version.ToString)
                writer.WriteLine([String].Format("{0} {1}", My.Computer.Name, My.Computer.Info.OSFullName)) 'Nome computer e sistema operativo
                writer.WriteLine(GetLangStr("LOG_Framework") & Assembly.GetCallingAssembly.ImageRuntimeVersion)
                writer.WriteLine([String].Format(GetLangStr("LOG_Memory"), My.Computer.Info.AvailablePhysicalMemory \ 1024, My.Computer.Info.TotalPhysicalMemory \ 1024))
                writer.WriteLine([String].Format(GetLangStr("LOG_OperationStarted"), [Date].Now))
                writer.WriteLine()
                writer.WriteLine(comment)
                writer.WriteLine()
                writer.WriteLine()
            Catch ex As Exception

            End Try
        End Sub

        Public Sub EndOperation(ByVal comment As String) Implements ITextFormatter.EndOperation
            Try
                writer.WriteLine([String].Format(GetLangStr("LOG_OperationEnded"), [Date].Now))
                writer.WriteLine()
                writer.WriteLine(comment)
                writer.Flush()
            Catch ex As Exception
            End Try
        End Sub

        Public Sub AddMsgError(ByVal msgString As String, ByVal isDevError As Boolean) Implements ITextFormatter.AddMsgError
            If isDevError Then
                writer.WriteLine([String].Format(ControlChars.Tab & "ERR: {0}", msgString))
            Else
                writer.WriteLine([String].Format("ERR: {0}", msgString))
            End If
        End Sub

        Public Sub AddMsg(ByVal msgString As String, ByVal isDev As Boolean) Implements ITextFormatter.AddMsg
            If isDev Then
                writer.WriteLine([String].Format(ControlChars.Tab & "{0}", msgString))
            Else
                writer.WriteLine(msgString)
            End If
        End Sub
    End Class



End Class
