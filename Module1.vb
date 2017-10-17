Module Module1
    Public Nodes As New ArrayList
    Public Starting As String = "C"
    Public Ending As String = "E"
    Public Trackers As New ArrayList
    Sub load()
        Using r As New IO.StreamReader("IN.txt")
            Do Until r.Peek = -1
                Dim s() As String = r.ReadLine.Split(",")
                Dim nn As Node = getN(s(0))
                If nn.Value = "" Then
                    Nodes.Add(New Node() With {.Value = s(0)})
                    getN(s(0)).Children.Add(s(1))
                Else
                    nn.Children.Add(s(1))
                End If
            Loop
        End Using
    End Sub
    Public Function getN(Name As String) As Node
        For Each n As Node In Nodes
            If n.Value = Name Then Return n
        Next
        Return New Node
    End Function
    Sub Main()
        load()
        Dim n As Node = getN(Starting)
        For Each c As String In getN(Starting).Children
            Dim path As New ArrayList
            Console.WriteLine(c)
            path.Add(c)
            path.Add(Starting)
            Trackers.Add(New Tracker(c, Starting, path))
        Next
        Dim id As Integer
        Dim size As Integer = 10000000
        Dim i As Integer
        For Each t As Tracker In Trackers
            If t.Path.Count < size And t.Done Then
                size = t.Path.Count
                id = i
            End If
            i += 1
        Next
        Trackers(id).show
        Console.ReadLine()
    End Sub

End Module
Public Class Node
    Public Value As String = ""
    Public Parent As String = ""
    Public Children As New ArrayList
End Class
Public Class Tracker
    Public Parent As String
    Public Path As ArrayList
    Public Value As String
    Public Done As Boolean
    Public Sub New(value As String, parent As String, path As ArrayList)
        Me.Value = value
        Me.Parent = parent
        Me.Path = path
        If parent = Ending Or path.Contains(Ending) And Not Done Then
            Done = True ' Show()
        Else
            Run()
        End If
    End Sub
    Public Sub Run()
        Walk()
    End Sub
    Public Sub Walk()
        For Each p As String In getN(Value).Children
            If Not AlreadyWalked(p) Then
                Dim np As New ArrayList
                np.Add(p)
                For Each swag As String In Me.Path
                    np.Add(swag)
                Next
                Trackers.Add(New Tracker(p, Value, np))
            End If
        Next
    End Sub
    Public Function AlreadyWalked(n As String) As Boolean
        For Each p As String In Me.Path
            If n = p Then Return True
        Next
        Return False
    End Function
    Public Sub Show()
        Dim o As String
        For Each p As String In Path
            o = p & o
        Next
        Console.WriteLine(String.Format("Parent = {0}\nValue = {1}\nPath = {2}\n".Replace("\n", "{3}"), Parent, Value, o, vbNewLine))
    End Sub
End Class