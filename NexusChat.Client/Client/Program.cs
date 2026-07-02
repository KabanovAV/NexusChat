using System.Net.Sockets;
using System.Text;

var port = 80;
var url = "www.google.com";

using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
try
{
    await socket.ConnectAsync(url, port);
    Console.WriteLine($"Подключение к {url} установлено");
    Console.WriteLine($"Адрес подключения {socket.RemoteEndPoint}");
    Console.WriteLine($"Адрес приложения {socket.LocalEndPoint}");

    var message = $"GET / HTTP/1.1\r\nHost: {url}\r\nConnection: close\r\n\r\n";
    var messageBytes = Encoding.UTF8.GetBytes(message);
    int bytesSent = await socket.SendAsync(messageBytes);
    Console.WriteLine($"на адрес {url} отправлено {bytesSent} байт(а)");

    await socket.DisconnectAsync(true);
}
catch (SocketException)
{
    Console.WriteLine($"Не удалось установить подключение к {url}");
}