using Shared;
using System.Net.Sockets;

namespace Server
{
    internal class ClientSession
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Socket TcpClient { get; init; }

        ChatServer _tcpServer;

        public ClientSession(Socket tcpClient, ChatServer tcpServer)
        {
            TcpClient = tcpClient;
            _tcpServer = tcpServer;
        }

        public async Task ProcessClientAsync()
        {
            try
            {
                string userName = await Message.ReceiveMessage(TcpClient);
                string message = $"{userName} вошел в чат";
                await _tcpServer.BroadcastMessageAsync(Id, message);
                Console.WriteLine(message);

                while (true)
                {
                    try
                    {
                        // Получение сообщения от клиента
                        message = await Message.ReceiveMessage(TcpClient);
                        message = $"{userName}: {message}";
                        Console.WriteLine(message);
                        await _tcpServer.BroadcastMessageAsync(Id, message);
                    }
                    catch
                    {
                        message = $"{userName} покинул чат";
                        Console.WriteLine(message);
                        await _tcpServer.BroadcastMessageAsync(Id, message);
                        break;
                    }

                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _tcpServer.RemoveConnection(Id);
            }
        }

        public void Close() => TcpClient.Close();
    }
}
