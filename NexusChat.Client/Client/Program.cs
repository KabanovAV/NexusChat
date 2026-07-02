using System.Net.Sockets;

var port = 80;
var url = "www.google.com";

using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
try
{
	await socket.ConnectAsync(url, port);
    Console.WriteLine($"Подключение к {url} установлено");
    Console.WriteLine($"Адрес подключения {socket.RemoteEndPoint}");
    Console.WriteLine($"Адрес приложения {socket.LocalEndPoint}");
}
catch (SocketException)
{
    Console.WriteLine($"Не удалось установить подключение к {url}");
}