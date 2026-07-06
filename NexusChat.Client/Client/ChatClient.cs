using System.Net.Sockets;

namespace Client
{
    internal class ChatClient : IDisposable
    {
        private const string _host = "127.0.0.1";
        private const int _port = 8888;

        protected readonly Socket tcpClient;

        public ChatClient()
        {
            tcpClient = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }        

        /// <summary>
        /// Запуск чата
        /// </summary>
        public async Task StartChat()
        {
            try
            {
                await tcpClient.ConnectAsync(_host, _port);
                await Task.Run(async () => MessageOperations.ReceiveMessageAsync(tcpClient));
                await MessageOperations.SendMessageAsync(tcpClient);
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                tcpClient.Close();
            }
        }

        public void Dispose() => tcpClient.Dispose();
    }
}
