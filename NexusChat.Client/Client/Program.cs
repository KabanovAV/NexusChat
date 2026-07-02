using System.Net.Sockets;
using System.Text;

var port = 80;
var url = "www.google.com";

async Task<Socket?> ConnectSocketAsync(string url, int port)
{
    Socket tempSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    try
    {
        await tempSocket.ConnectAsync(url, port);
        Console.WriteLine($"Подключение к {url} установлено");
        Console.WriteLine($"Адрес подключения {tempSocket.RemoteEndPoint}");
        Console.WriteLine($"Адрес приложения {tempSocket.LocalEndPoint}");
        return tempSocket;
    }
    catch (SocketException ex)
    {
        Console.WriteLine(ex.Message);
        tempSocket.Close();
        Console.WriteLine($"Не удалось установить подключение к {url}");
    }
    return null;
}
async Task<string> SocketSendRecieveAsync(string url, int port)
{
    using var socket = await ConnectSocketAsync(url, port);
    if (socket == null)
        return $"Не удалось установить подключение к {url}";

    var message = $"GET / HTTP/1.1\r\nHost: {url}\r\nConnection: Close\r\n\r\n";
    var messageBytes = Encoding.UTF8.GetBytes(message);
    int bytesSent = await socket.SendAsync(messageBytes);
    Console.WriteLine($"на адрес {url} отправлено {bytesSent} байт(а)");

    var responseBytes = new byte[512];
    var builder = new StringBuilder();
    int bytes;

    do
    {
        bytes = await socket.ReceiveAsync(responseBytes);
        string responsePart = Encoding.UTF8.GetString(responseBytes, 0, bytes);
        builder.Append(responsePart);
    }
    while (bytes > 0);
    return builder.ToString();
}

