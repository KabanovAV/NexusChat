using System.Net.Sockets;
using System.Text;

namespace Shared
{
    public static class Message
    {
        public static async Task<int> SendMessage(Socket socket, string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var lengthHeader = BitConverter.GetBytes(messageBytes.Length); // Получаем 4 байта длины сообщения
            int bytesSend = await socket.SendAsync(lengthHeader);
            await socket.SendAsync(messageBytes);

            return bytesSend;
        }

        public static async Task<string> ReceiveMessage(Socket socket)
        {
            var lengthBuffer = new byte[4];
            int byteRead = 0;

            while (byteRead < 4)
            {
                int read = await socket.ReceiveAsync(lengthBuffer.AsMemory(byteRead, 4 - byteRead));
                if (read == 0) throw new SocketException((int)SocketError.ConnectionReset);
                byteRead += read;
            }

            int messageLength = BitConverter.ToInt32(lengthBuffer, 0);
            var responseBytes = new byte[messageLength];
            int totalBytes = 0;

            while (totalBytes < messageLength)
            {
                int read = await socket.ReceiveAsync(responseBytes.AsMemory(totalBytes, messageLength - totalBytes));
                if (read == 0) throw new SocketException((int)SocketError.ConnectionReset);
                totalBytes += read;
            }

            return Encoding.UTF8.GetString(responseBytes, 0, totalBytes);
        }
    }
}
