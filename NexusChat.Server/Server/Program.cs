using System.Net;
using System.Net.Sockets;
using System.Text;

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
    var message = "Добро пожаловат в NexusChat";
    var messageBytes = Encoding.UTF8.GetBytes(message);
    var lengthHeader = BitConverter.GetBytes(messageBytes.Length); // Получаем 4 байта длины сообщения

    await tcpClient.SendAsync(lengthHeader);
    int bytesSend = await tcpClient.SendAsync(messageBytes);
    Console.WriteLine($"На адрес {tcpClient.RemoteEndPoint} отправлено {bytesSend} байт(а)");

    // Получение сообщения от клиента
    var lengthBuffer = new byte[4];
    int byteRead = 0;

    while (byteRead < 4)
    {
        int read = await tcpClient.ReceiveAsync(lengthBuffer.AsMemory(byteRead, 4 - byteRead));
        if (read == 0) throw new SocketException((int)SocketError.ConnectionReset);
        byteRead += read;
    }

    int messageLength = BitConverter.ToInt32(lengthBuffer, 0);
    var responseBytes = new byte[messageLength];
    int totalBytes = 0;

    while (totalBytes < messageLength)
    {
        int read = await tcpClient.ReceiveAsync(responseBytes.AsMemory(totalBytes, messageLength - totalBytes));
        if (read == 0) throw new SocketException((int)SocketError.ConnectionReset);
        totalBytes += read;
    }

    var response = Encoding.UTF8.GetString(responseBytes, 0, totalBytes);
    Console.WriteLine(response);
}
catch (SocketException ex)
{
    Console.WriteLine(ex.Message);
}