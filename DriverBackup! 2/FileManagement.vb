Imports DriverBackup__2.DeviceManagement

Namespace FileManagement

    'Interfaccia pubblica che espone le funzioni comuni ad un computer
    Public Delegate Sub ComputerErrorHandler(ByVal sender As IComputer, ByVal e As FileOperationException)

    Public Interface IComputer
        'Ritorna il nome del computer
        ReadOnly Property Name() As String
        'Ritorna TRUE se il computer virtuale è mappato al computer locale
        ReadOnly Property IsLocal() As Boolean
        'Ritorna la lista dei device installati nel computer
        ReadOnly Property Devices() As DeviceCollection
        'Riceva un file dal computer virtuale e lo memorizza nel percorso specificato
        Function GetFile(ByVal localPath As String, ByVal remotePath As String) As Boolean
        'Invia un file al computer virtuale memorizzandolo nel percorso remoto specificato
        Function SendFile(ByVal localPath As String, ByVal remotePath As String) As Boolean
        'Evento di notifica errori
        Event ComputerError As ComputerErrorHandler
    End Interface

    Public Enum FileOperationsEnum
        FOP_CreateDirectory
        FOP_CopyFile
        FOP_DeleteDirectory
        FOP_DeleteFile
        FOP_FileDirExist
        FOP_GetFreeSpace
    End Enum

    Public Enum FileOperationsErrorsEnum
        FOE_Generic
    End Enum

    Public Class FileOperationException : Inherits Exception
        Dim fe_Code As FileOperationsEnum
        Dim fe_ErrCode As FileOperationsErrorsEnum
        Dim fe_Success As Boolean

        Public ReadOnly Property Success() As Boolean
            Get
                Return Me.fe_Success
            End Get
        End Property

        Public ReadOnly Property ErrorCode() As FileOperationsErrorsEnum
            Get
                Return Me.fe_ErrCode
            End Get
        End Property

        Public ReadOnly Property OperationCode() As FileOperationsEnum
            Get
                Return Me.fe_Code
            End Get
        End Property

        Public Sub New(ByVal opCode As FileOperationsEnum, ByVal success As Boolean)
            Me.New(opCode, success, FileOperationsErrorsEnum.FOE_Generic)
        End Sub

        Public Sub New(ByVal opCode As FileOperationsEnum, ByVal success As Boolean, ByVal errCode As FileOperationsErrorsEnum)
            Me.fe_Code = opCode
            Me.fe_ErrCode = errCode
            Me.fe_Success = success
        End Sub

    End Class

    Public Class PC
        'Rappresenta una macchina
        Dim WithEvents pc_FileSystem As IFileSystem
        Dim pc_DeviceCollection As DeviceCollection
        Dim pc_Name As String
        Dim pc_EndPoint As Object

        Public ReadOnly Property Name() As String
            Get
                Return Me.pc_Name
            End Get
        End Property

        Public Property FileSystemManager() As IFileSystem
            Get
                Return Me.pc_FileSystem
            End Get
            Set(ByVal value As IFileSystem)
                Me.pc_FileSystem = value
            End Set
        End Property

        Public Property Devices() As DeviceCollection
            Get
                Return Me.pc_DeviceCollection
            End Get
            Set(ByVal value As DeviceCollection)
                Me.pc_DeviceCollection = value
            End Set
        End Property

        Public Sub New()
            'Istanziamento semplice da computer locale
            Me.New(DeviceCollection.Create(Nothing))
        End Sub

        Public Sub New(ByVal dvc As DeviceCollection)
            'Istanziamento semplice da computer locale
            Me.pc_FileSystem = New LocalFileSystem
            Me.pc_DeviceCollection = dvc
            Me.pc_Name = My.Computer.Name
        End Sub

        Private Sub New(ByVal ip As Object)
            'Mappa l'oggetto ad un computer della rete
            Return
        End Sub

        Public Shared Function CreateFromIP(ByVal ip As Object) As PC

            Return Nothing
        End Function

    End Class

    Public Delegate Sub FileOperationHandler(ByVal sender As Object, ByVal e As FileOperationException)
    
    Public Interface IFileSystem

        Event FileOperationEvent As FileOperationHandler

        Function CreateDirectory(ByVal dirName As String) As Boolean

        Function CopyFile(ByVal source As String, ByVal destination As String) As Boolean

        Function DeleteDirectory(ByVal dirName As String) As Boolean

        Function DeleteFile(ByVal fileName As String) As Boolean

        Function DirExist(ByVal dirName As String) As Boolean

        Function FileExist(ByVal filename As String) As Boolean

        Function GetFreeSpace(ByVal path As String) As Long

    End Interface

    Public Class LocalFileSystem
        'Oggetto mappato al file-system locale
        Implements IFileSystem

        Public Event FileOperationEvent(ByVal sender As Object, ByVal e As FileOperationException) Implements IFileSystem.FileOperationEvent

        Public Function CopyFile(ByVal source As String, ByVal destination As String) As Boolean Implements IFileSystem.CopyFile

            Try

                File.Copy(source, destination, False)

                Dim e As New FileOperationException(FileOperationsEnum.FOP_CopyFile, True)
                e.Data.Add("SourceFile", source)
                e.Data.Add("DestFile", destination)
                RaiseEvent FileOperationEvent(Me, e)

                Return True
            Catch ex As Exception
                Dim e As New FileOperationException(FileOperationsEnum.FOP_CopyFile, False, FileOperationsErrorsEnum.FOE_Generic)
                e.Data.Add("Exception", ex)
                RaiseEvent FileOperationEvent(Me, e)

                Return False
            End Try
        End Function

        Public Function CreateDirectory(ByVal dirName As String) As Boolean Implements IFileSystem.CreateDirectory
            Try
                If Not Directory.Exists(dirName) Then
                    Directory.CreateDirectory(dirName)
                End If

                Dim e As New FileOperationException(FileOperationsEnum.FOP_CreateDirectory, True)
                e.Data.Add("DirName", dirName)
                RaiseEvent FileOperationEvent(Me, e)

                Return True
            Catch ex As Exception
                Dim e As New FileOperationException(FileOperationsEnum.FOP_CreateDirectory, False, FileOperationsErrorsEnum.FOE_Generic)
                e.Data.Add("Exception", ex)
                RaiseEvent FileOperationEvent(Me, e)

                Return False
            End Try
        End Function

        Public Function DeleteDirectory(ByVal dirName As String) As Boolean Implements IFileSystem.DeleteDirectory
            Try

                Directory.Delete(dirName, False)

                Dim e As New FileOperationException(FileOperationsEnum.FOP_DeleteDirectory, True)
                e.Data.Add("DirName", dirName)
                RaiseEvent FileOperationEvent(Me, e)
                Return True

            Catch ex As Exception

                Dim e As New FileOperationException(FileOperationsEnum.FOP_DeleteDirectory, False, FileOperationsErrorsEnum.FOE_Generic)
                e.Data.Add("Exception", ex)
                RaiseEvent FileOperationEvent(Me, e)

                Return False
            End Try

        End Function

        Public Function DeleteFile(ByVal fileName As String) As Boolean Implements IFileSystem.DeleteFile
            Try

                File.Delete(fileName)

                Dim e As New FileOperationException(FileOperationsEnum.FOP_DeleteFile, True)
                e.Data.Add("FileName", fileName)
                RaiseEvent FileOperationEvent(Me, e)
                Return True

            Catch ex As Exception
                Dim e As New FileOperationException(FileOperationsEnum.FOP_DeleteFile, False)
                e.Data.Add("FileName", fileName)
                RaiseEvent FileOperationEvent(Me, e)
                Return False
            End Try
        End Function

        Public Function DirExist(ByVal dirName As String) As Boolean Implements IFileSystem.DirExist
            Return Directory.Exists(dirName)
        End Function

        Public Function FileExist(ByVal filename As String) As Boolean Implements IFileSystem.FileExist
            Return File.Exists(filename)
        End Function

        Public Function GetFreeSpace(ByVal path As String) As Long Implements IFileSystem.GetFreeSpace
            Try
                Dim d As New DriveInfo(path)

                If d IsNot Nothing Then
                    Return d.AvailableFreeSpace
                Else
                    Return 0
                End If

            Catch ex As Exception
                Return 0
            End Try
        End Function

    End Class








End Namespace


