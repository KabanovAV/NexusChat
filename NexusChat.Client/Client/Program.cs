using System.Net.Sockets;
using System.Text;

var port = 80;
var url = "www.google.com";
var response = await SocketSendRecieveAsync(url, port);
Console.WriteLine(response);

async Task<TcpClient?> ConnectSocketAsync(string url, int port)
{
    TcpClient tcpClient = new();
    try
    {
        await tcpClient.ConnectAsync(url, port);
        Console.WriteLine($"Подключение к {url} установлено");
        Console.WriteLine($"Адрес подключения {tcpClient.Client.RemoteEndPoint}");
        Console.WriteLine($"Адрес приложения {tcpClient.Client.LocalEndPoint}");
        return tcpClient;
    }
    catch (SocketException ex)
    {
        Console.WriteLine(ex.Message);
        tcpClient.Close();
        Console.WriteLine($"Не удалось установить подключение к {url}");
    }
    return null;
}
async Task<string> SocketSendRecieveAsync(string url, int port)
{
    using var tcpClient = await ConnectSocketAsync(url, port);
    if (tcpClient == null)
        return $"Не удалось установить подключение к {url}";

    NetworkStream stream = tcpClient.GetStream();

    var message = $"GET / HTTP/1.1\r\nHost: {url}\r\nConnection: Close\r\n\r\n";
    var messageBytes = Encoding.UTF8.GetBytes(message);
    await stream.WriteAsync(messageBytes);
    Console.WriteLine($"на адрес {url} отправлено {messageBytes} байт(а)");

    var responseBytes = new byte[512];
    var builder = new StringBuilder();
    int bytes;

    do
    {
        bytes = await stream.ReadAsync(responseBytes);
        string responsePart = Encoding.UTF8.GetString(responseBytes, 0, bytes);
        builder.Append(responsePart);
    }
    while (bytes > 0);
    return builder.ToString();
}

