Public Class ThreadingUtilities
    Public Delegate Function GetSetPropertyHandler(ByRef TargetObj As Object, ByVal PropName As String, ByVal SetValue As Object, ByVal Index As Object) As Object
    Public Delegate Function InvokeMethodHandler(ByRef Ctrl As Object, ByVal MethodName As String, ByVal Args() As Object) As Object

    Public Shared PropertyInvoke As New GetSetPropertyHandler(AddressOf GetSetProperty)
    Public Shared MethodInvoke As New InvokeMethodHandler(AddressOf InvokeMethod)

    Public Shared Function GetSetProperty(ByRef TargetObj As Object, ByVal PropName As String, ByVal SetValue As Object, ByVal Index As Object) As Object
        Try
            Dim prpInfo As PropertyInfo
            prpInfo = TargetObj.GetType.GetProperty(PropName)
            If SetValue IsNot Nothing Then
                'Imposta prima il nuovo valore
                prpInfo.SetValue(TargetObj, SetValue, Index)
            End If

            Return prpInfo.GetValue(TargetObj, Index)
        Catch ex As Exception
            Throw New Reflection.TargetException("Property not found")
        End Try
    End Function

    Public Shared Function GetSetCtrlProperty(ByRef TargetObj As Control, ByVal PropName As String, ByVal SetValue As Object, ByVal Index As Object) As Object
        Return TargetObj.Invoke(PropertyInvoke, TargetObj, PropName, SetValue, Index)
    End Function


    Public Shared Function InvokeCtrlMethod(ByRef Ctrl As Control, ByVal MethodName As String, ByVal ParamArray Args() As Object) As Object
        Return Ctrl.Invoke(MethodInvoke, Ctrl, MethodName, Args)
    End Function

    Public Shared Function InvokeMethod(ByRef Ctrl As Object, ByVal MethodName As String, ByVal Args() As Object) As Object
        Try
            Dim mtdInfo As MethodInfo
            Dim tp() As Type

            If Args Is Nothing Then
                mtdInfo = Ctrl.GetType.GetMethod(MethodName)
            Else
                'riempe l'array dei tipi di dato degli argomenti
                ReDim tp(Args.Length - 1)
                For I As Integer = 0 To Args.Length - 1
                    tp(I) = Args(I).GetType
                Next
                mtdInfo = Ctrl.GetType.GetMethod(MethodName, tp)
            End If

            If mtdInfo Is Nothing Then Return Nothing
            'Prova a cercare una proprietà omonima
            Return mtdInfo.Invoke(Ctrl, BindingFlags.InvokeMethod, Nothing, Args, Nothing)
        Catch ex As Exception
            Throw New Reflection.TargetException("Method not found")
        End Try
    End Function
End Class
