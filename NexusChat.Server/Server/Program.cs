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
    await Message.SendMessage(tcpClient, "Добро пожаловат в NexusChat");

    while (true)
    {
        // Получение сообщения от клиента
        var response = await Message.ReceiveMessage(tcpClient);

        if (response == "close") break;
        Console.WriteLine($"Сообщение пользователя: {response}");

        await Message.SendMessage(tcpClient, $"{DateTime.Now.ToShortTimeString()} Сообщение доставлено!");
    }
}
catch (SocketException ex)
{
    Console.WriteLine(ex.Message);
}
finally
{
    tcpListener.Close();
}