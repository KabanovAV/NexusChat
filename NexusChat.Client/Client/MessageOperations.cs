using Shared;
using System.Net.Sockets;

namespace Client
{
    public static class MessageOperations
    {
        /// <summary>
        /// Получение сообщений от пользователей в чате
        /// </summary>
        /// <param name="tcpClient">Сокет</param>
        public static async Task ReceiveMessageAsync(Socket tcpClient)
        {
            while (true)
            {
                try
                {
                    var response = await Message.ReceiveMessage(tcpClient);

                    if (OperatingSystem.IsWindows())
                    {
                        var (Left, Top) = Console.GetCursorPosition(); // получаем текущую позицию курсора
                        int left = Left; // смещение в символах относительно левого края
                        int top = Top; // смещение в строках относительно верха
                        // копируем ранее введенные символы в строке на следующую строку
                        Console.MoveBufferArea(0, top, left, 1, 0, top + 1);
                        // устанавливаем курсор в начало текущей строки
                        Console.SetCursorPosition(0, top);
                        // в текущей строке выводит полученное сообщение
                        Console.WriteLine(response);
                        // переносим курсор на следующую строку и пользователь продолжает ввод уже на следующей строке
                        Console.SetCursorPosition(left, top + 1);
                    }
                    else Console.WriteLine(response);
                }
                catch
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Отпарвить сообщение пользователям
        /// </summary>
        /// <param name="tcpClient">Сокет</param>
        public static async Task SendMessageAsync(Socket tcpClient)
        {
            Console.Write("Введите имя пользователя: ");
            string? message = Console.ReadLine();
            await Message.SendMessage(tcpClient, message!);

            Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");

            while (true)
            {
                message = Console.ReadLine();
                await Message.SendMessage(tcpClient, message!);
            }
        }
    }
}
