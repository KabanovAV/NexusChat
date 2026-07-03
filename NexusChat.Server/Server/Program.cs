using Shared;
using System.Net;
using System.Net.Sockets;

IPEndPoint ipEndPoint = new(IPAddress.Parse("127.0.0.1"), 8888);
using Socket tcpListener = new(SocketType.Stream, ProtocolType.Tcp);

try
{
    tcpListener.Bind(ipEndPoint);
    tcpListener.Listen();
    Console.WriteLine("Сервер запущен. Ожидание подключений...");

    using Socket tcpClient = await tcpListener.AcceptAsync();
    Console.WriteLine($"Адрес подключенного клиента: {tcpClient.RemoteEndPoint}");

    // Отправка сообщения клиенту
    int bytesSend = await Message.SendMessage(tcpClient, "Добро пожаловат в NexusChat");
    Console.WriteLine($"На адрес {tcpClient.RemoteEndPoint} отправлено {bytesSend} байт(а)");

    // Получение сообщения от клиента
    var response = await Message.ReceiveMessage(tcpClient);
    Console.WriteLine(response);
}
catch (SocketException ex)
{
    Console.WriteLine(ex.Message);
}