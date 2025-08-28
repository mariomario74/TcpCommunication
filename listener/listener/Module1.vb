Imports System.Net
Imports System.Net.Sockets
Imports System.IO
Imports System.Text
Imports System
Imports System.Data.SqlTypes
Imports System.Threading.Tasks
Imports System.Xml.XPath
Imports System.Threading
Imports System.Runtime.CompilerServices
Imports System.Net.NetworkInformation


Module Module1
    Private connectionWaitHandle As New AutoResetEvent(False)
    Dim port As Integer = 65000                         'Chosen port
    Dim localAddress = IPAddress.Parse("127.0.0.1")     'Chosen address
    Private server As TcpListener = New TcpListener(IPAddress.Any, port)  'Create TcpListener with our desired 
    Dim buffer(1024) As Byte    'Buffer to read data
    Dim data() As Byte
    Dim msg As String
    Dim msgCount As Integer = 0
    Dim storage(512) As String
    Dim firstConnection As Boolean = True
    Sub Main()
        server.Start()
        While True

            Dim result As IAsyncResult = server.BeginAcceptTcpClient(AddressOf handleAsyncConnection, server)
            connectionWaitHandle.WaitOne()
            connectionWaitHandle.Reset()

        End While

        server.Stop()

        Console.WriteLine("Hit enter to continue...")
        Console.Read()

    End Sub

    Sub handleAsyncConnection(result As IAsyncResult)

        Dim server As TcpListener = CType(result.AsyncState, TcpListener)
        Dim client As TcpClient = server.EndAcceptTcpClient(result)
        connectionWaitHandle.Set()

        Dim stream As NetworkStream = client.GetStream      'Establish Network Stream connection.
        msg = stream.Read(Buffer, 0, Buffer.Length)         'Read message from the TCP Stream
        Data = System.Text.Encoding.ASCII.GetBytes(msg)     'Convert in bytes to write to the TCP Stream
        Dim finalMessage As String = System.Text.Encoding.ASCII.GetString(Buffer, 0, msg)       'Convert byte message back to String
        stream.Write(data, 0, data.Length)      'Write to stream
        If firstConnection = True Then
            Console.WriteLine("First connected user says: {0}", finalMessage)       'Write message to our console
            firstConnection = False
        Else
            Console.WriteLine("Second connected user says: {0}", finalMessage)
            firstConnection = True
        End If
        If finalMessage <> "show -all" Then
            storage(msgCount) = finalMessage
        End If

        msgCount = msgCount + 1

        If finalMessage = "show -all" Then
            For k = 0 To msgCount - 1
                Console.WriteLine(storage(k))
            Next
        End If

        If finalMessage = "clear" Or finalMessage = "cl" Then

            Console.Clear()

        End If

        If finalMessage = "clear storage" Or finalMessage = "cl storage" Then
            Array.Clear(storage, 0, msgCount)
            msgCount = 0
        End If

        Dim properties As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties()
        Dim connections As TcpConnectionInformation() = properties.GetActiveTcpConnections()

        For Each connection As TcpConnectionInformation In connections
            Console.Write("Local endpoint: {0} ", connection.LocalEndPoint.Address)
            Console.Write("Remote endpoint: {0} ", connection.RemoteEndPoint.Address)
            Console.WriteLine("{0}", connection.State)
        Next
        Console.WriteLine()

        client.Close()

    End Sub

End Module