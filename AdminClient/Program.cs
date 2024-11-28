using System.Net.WebSockets;

namespace AdminClient
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static async Task ConnectWithRetry(string serverUri)
        {
            while (true)
            {
                ClientWebSocket webSocket = new ClientWebSocket(); // Новый экземпляр WebSocket при каждой попытке подключения

                try
                {
                    await webSocket.ConnectAsync(new Uri(serverUri), CancellationToken.None);
                    Console.WriteLine("Connected to the WebSocket server.");

                    // Если соединение успешно, начинаем цикл получения сообщений
                    while (webSocket.State == WebSocketState.Open)
                    {
                       // await ReceiveMessages(webSocket);
                    }
                }
                catch (WebSocketException)
                {
                    Console.WriteLine("Server not available. Retrying in 5 seconds...");
                    await Task.Delay(5000);
                }
                finally
                {
                    // Закрываем WebSocket перед следующей попыткой подключения
                    if (webSocket.State != WebSocketState.Closed && webSocket.State != WebSocketState.Aborted)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    }
                }
            }
        }
        static async Task Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Main());
            await ConnectWithRetry("ws://localhost:6000/");
        }
    }
}