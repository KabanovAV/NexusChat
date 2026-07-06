using Shared;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    internal class ServerObject
    {
        private readonly IPEndPoint ipEndPoint = new(IPAddress.Parse("127.0.0.1"), 8888);
        private readonly Socket tcpListener = new(SocketType.Stream, ProtocolType.Tcp);
        private readonly List<ClientObject> clients = [];        

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
                    ClientObject client = new(tcpClient, this);
                    clients.Add(client);
                    Task.Run(() => client.ProcessClientAsync());
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
            foreach (ClientObject client in clients)
            {
                if (client.Id != id)
                    await Message.SendMessage(client.TcpClient, message);
            }
        }

        public void RemoveConnection(Guid id)
        {
            ClientObject? client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                clients.Remove(client);
                client.Close();
            }
        }

        public void Disconect()
        {
            foreach (ClientObject client in clients)
                client.Close();
            tcpListener.Close();
        }
    }
}
