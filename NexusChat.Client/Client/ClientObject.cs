using System.Net.Sockets;

namespace Client
{
    internal class ClientObject
    {
        protected readonly Socket tcpClient = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// Запуск чата
        /// </summary>
        public async Task StartChat()
        {
            try
            {
                await tcpClient.ConnectAsync("127.0.0.1", 8888);
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
    }
}
