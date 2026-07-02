using System.Net;
using System.Net.Sockets;

IPAddress localAddr = IPAddress.Parse("127.0.0.1");
TcpListener server = new(localAddr, 8888);

try
{
    server.Start();
    Console.WriteLine("Сервер запущен. Ожидание подключений...");

    while (true)
    {
        using var tcpClient = await server.AcceptTcpClientAsync();
        Console.WriteLine($"Входящее подключение: {tcpClient.Client.RemoteEndPoint}");
    }
}
finally
{
    server.Stop();
}