using Shared;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    internal class ChatServer : IDisposable
    {
        private const string _host = "127.0.0.1";
        private const int _port = 8888;

        private readonly IPEndPoint ipEndPoint;
        private readonly Socket tcpListener;
        private readonly List<ClientSession> clients = [];

        public ChatServer()
        {
            ipEndPoint = new(IPAddress.Parse(_host), _port);
            tcpListener = new(SocketType.Stream, ProtocolType.Tcp);
        }

        public async Task Listener()
        {
            try
            {
                tcpListener.Bind(ipEndPoint);
                tcpListener.Listen();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket tcpClient = await tcpListener.AcceptAsync();
                    ClientSession client = new(tcpClient, this);
                    clients.Add(client);
                    _ = client.ProcessClientAsync();
                }

            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconect();
            }
        }

        public async Task BroadcastMessageAsync(Guid id, string message)
        {
            foreach (ClientSession client in clients)
            {
                if (client.Id != id)
                    await Message.SendMessage(client.TcpClient, message);
            }
        }

        public void RemoveConnection(Guid id)
        {
            ClientSession? client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                clients.Remove(client);
                client.Close();
            }
        }

        public void Disconect()
        {
            foreach (ClientSession client in clients)
                client.Close();
            Dispose();
        }

        public void Dispose() => tcpListener.Dispose();
    }
}
