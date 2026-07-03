using System.Net.Sockets;
using System.Text;

using Socket tcpClient = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

try
{
    await tcpClient.ConnectAsync("127.0.0.1", 8888);
    Console.WriteLine($"Подключение к {tcpClient.RemoteEndPoint} установлено");

    // Получение сообщения от сервера
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
        int bytes = await tcpClient.ReceiveAsync(responseBytes.AsMemory(totalBytes, messageLength - totalBytes));
        if (bytes == 0) throw new SocketException((int)SocketError.ConnectionReset);
        totalBytes += bytes;
    }

    var response = Encoding.UTF8.GetString(responseBytes, 0, totalBytes);
    Console.WriteLine(response);

    // Отправка сообщения на сервера
    var message = $"Пользователь {tcpClient.LocalEndPoint} отключился";
    var messageBytes = Encoding.UTF8.GetBytes(message);
    var lengthHeader = BitConverter.GetBytes(messageBytes.Length);

    await tcpClient.SendAsync(lengthHeader);
    await tcpClient.SendAsync(messageBytes);
    await tcpClient.DisconnectAsync(false);
}
catch (SocketException ex)
{
    Console.WriteLine(ex.Message);
}