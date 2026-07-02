using System.Net;
using System.Net.Sockets;

IPEndPoint ipPoint = new(IPAddress.Any, 8888);
using Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
socket.Bind(ipPoint);

socket.Listen(1000);

Console.WriteLine(socket.LocalEndPoint);