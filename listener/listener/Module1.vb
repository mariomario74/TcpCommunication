Imports System.Net
Imports System.Net.Sockets
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Runtime.InteropServices.Marshalling
Imports System.Text
Imports System
Imports System.Data.SqlTypes


Module Module1

    Sub Main()
        Dim i As Integer
        Dim listener As TcpListener
        Dim port As Integer = 65000
        Dim localAddress = IPAddress.Parse("127.0.0.1")
        Dim server As TcpListener = New TcpListener(port)
        server.Start()
        Dim client As New TcpClient
        Dim buffer(1024) As Byte
        Dim data() As Byte
        Dim msg As String
        While True

            client = server.AcceptTcpClient

            Dim stream As NetworkStream = client.GetStream
            msg = stream.Read(buffer, 0, buffer.Length)
            data = System.Text.Encoding.ASCII.GetBytes(msg)
            Dim finalMessage As String = System.Text.Encoding.ASCII.GetString(buffer, 0, msg)
            stream.Write(data, 0, data.Length)
            Console.WriteLine("Received: {0}", finalMessage)

            client.Close()

        End While

        server.Stop()

        Console.WriteLine("Hit enter to continue...")
        Console.Read()

    End Sub

End Module