Imports System
Imports System.Net.Sockets
Imports System.IO
Imports System.Runtime.Remoting.Channels

Module Program

    Dim server As String = "127.0.0.1"
    Dim port As Integer = 65000

    Sub Main()
        While True
            Console.Write("Send: ")
            Dim text As String = Console.ReadLine
            Connect(server, text)

        End While

    End Sub

    Public Sub Connect(server As String, message As String)

        Dim client As New TcpClient(server, port)
        Dim data() As Byte = System.Text.Encoding.ASCII.GetBytes(message)
        Dim stream As NetworkStream = client.GetStream
        stream.Write(data, 0, data.Length)
        Console.WriteLine("Sent: {0}", message)
        stream.Close()
        client.Close()

    End Sub

    Sub initTcpClientSender()

        Dim client As New TcpClient(server, port)

    End Sub

End Module