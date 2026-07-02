using System.Net;
using System.Net.Sockets;

IPEndPoint ipPoint = new(IPAddress.Any, 8888);
using Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
socket.Bind(ipPoint);

Console.WriteLine(socket.LocalEndPoint);
socket.Listen(1000);

Console.WriteLine("Сервер запущен. Ожидание подключений...");
using Socket client = await socket.AcceptAsync();
Console.WriteLine($"Адрес подключенного клиента: {client.RemoteEndPoint}");