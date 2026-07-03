using Shared;
using System.Net.Sockets;

using Socket tcpClient = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

try
{
    await tcpClient.ConnectAsync("127.0.0.1", 8888);
    Console.WriteLine($"Подключение к {tcpClient.RemoteEndPoint} установлено");

    // Получение сообщения от сервера
    var response = await Message.ReceiveMessage(tcpClient);
    Console.WriteLine(response);

    // Отправка сообщения на сервера
    await Message.SendMessage(tcpClient, $"Пользователь {tcpClient.LocalEndPoint} отключился");
}
catch (SocketException ex)
{
    Console.WriteLine(ex.Message);
}