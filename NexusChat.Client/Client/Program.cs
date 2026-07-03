using Shared;
using System.Net.Sockets;

using Socket tcpClient = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

try
{
    await tcpClient.ConnectAsync("127.0.0.1", 8888);
    Console.WriteLine($"Подключение к {tcpClient.RemoteEndPoint} установлено");

    while (true)
    {
        // Получение сообщения от сервера
        var response = await Message.ReceiveMessage(tcpClient);
        Console.WriteLine(response);

        // Отправка сообщения на сервера
        Console.Write("Введите сообщение (закрыть сессию введите \'close\'): ");
        string? message = Console.ReadLine();
        await Message.SendMessage(tcpClient, message!);
        if (message == "close") break;
    }    
}
catch (SocketException ex)
{
    Console.WriteLine(ex.Message);
}
finally
{
    tcpClient.Close();
}