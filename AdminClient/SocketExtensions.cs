using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text;

namespace AdminClient
{
    public static class SocketExtensions
    {

        public async static Task SendTextMessageAsync(this WebSocket socket, string message)
        {
            await socket.SendAsync(
                Encoding.UTF8.GetBytes(message),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }
        public async static Task<string> ReceiveTextMessageAsync(this WebSocket socket)
        {
            var buffer = new byte[1024];
            WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            return Encoding.UTF8.GetString(buffer, 0, result.Count);
        }
        public async static Task SendPaths(this WebSocket socket, List<string> paths)
        {
            if (socket == null) throw new ArgumentNullException(nameof(socket));
            if (paths == null) throw new ArgumentNullException(nameof(paths));

            // Сериализация списка путей в JSON
            string json = JsonSerializer.Serialize(paths);
            byte[] buffer = Encoding.UTF8.GetBytes(json);

            // Создаем сегмент массива байт для отправки
            var segment = new ArraySegment<byte>(buffer);

            // Отправляем сообщение через WebSocket
            await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        public static async Task<List<string>> ReceivePaths(this WebSocket socket)
        {
            if (socket == null) throw new ArgumentNullException(nameof(socket));

            var buffer = new byte[1024 * 4]; // Буквально 4 Кб
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                return null; // Соединение закрыто
            }

            // Декодирование полученных данных
            var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var paths = JsonSerializer.Deserialize<List<string>>(json);

            return paths;
        }
        public async static Task SendClientLevelAsync(WebSocket socket, ClientLevel clientLevel)
        {
            if (socket == null || socket.State != WebSocketState.Open)
                throw new InvalidOperationException("Socket is not connected.");

            var json = JsonSerializer.Serialize(clientLevel);
            var buffer = Encoding.UTF8.GetBytes(json);

            await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        public async static Task<ClientLevel> ReceiveClientLevelAsync(WebSocket socket)
        {
            if (socket == null || socket.State != WebSocketState.Open)
                throw new InvalidOperationException("Socket is not connected.");

            var buffer = new byte[1024 * 64];
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                return JsonSerializer.Deserialize<ClientLevel>(json) ?? throw new JsonException("Deserialization failed.");
            }

            throw new InvalidOperationException("Invalid message type received.");
        }
        public async static Task SendClientLevelsAsync(WebSocket socket, List<ClientLevel> clientLevels)
        {
            if (socket == null || socket.State != WebSocketState.Open)
                throw new InvalidOperationException("Socket is not connected.");

            var json = JsonSerializer.Serialize(clientLevels);
            var buffer = Encoding.UTF8.GetBytes(json);

            await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        public async static Task<List<ClientLevel>> ReceiveClientLevelsAsync(WebSocket socket)
        {
            if (socket == null || socket.State != WebSocketState.Open)
                throw new InvalidOperationException("Socket is not connected.");

            var buffer = new byte[1024 * 1024];
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                return JsonSerializer.Deserialize<List<ClientLevel>>(json) ?? throw new JsonException("Deserialization failed.");
            }

            throw new InvalidOperationException("Invalid message type received.");
        }
        public static async Task ConnectWithRetry(string serverUri)
        {
            bool firstConn = true;
            while (true)
            {
                ClientWebSocket webSocket = new ClientWebSocket(); // Новый экземпляр WebSocket при каждой попытке подключения
                Storage.currentWebSocket = webSocket;
                
                try
                {
                    await webSocket.ConnectAsync(new Uri(serverUri), CancellationToken.None);
                    
                    // Если соединение успешно, начинаем цикл получения сообщений
                    while (webSocket.State == WebSocketState.Open)
                    {
                        if (firstConn)
                        {
                            await SocketExtensions.SendTextMessageAsync(Storage.currentWebSocket, "admin here");
                            Storage.AllClients = await SocketExtensions.ReceiveClientLevelsAsync(Storage.currentWebSocket);
                            firstConn = false;
                        }
                        
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
    }
}
