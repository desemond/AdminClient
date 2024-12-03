using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminClient
{
    public partial class Main : Form
    {
        TextBox text1 = new TextBox();
        TextBox text2 = new TextBox();
        TextBox text7 = new TextBox();
        TextBox text4 = new TextBox();
        private SplitContainer splitContainer = new SplitContainer();
        private DataGridView datagrid = new DataGridView();

        public Main()
        {
            this.Load += Main_Load;


            //InitializeComponent();
            this.Size = new System.Drawing.Size(1000, 800);
            this.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.Sizable;


            Label label1 = new Label();
            label1.Text = "введите путь в строку ниже для добавления в мониторинг";
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            label1.AutoSize = true;
            splitContainer.Panel1.Controls.Add(label1);


            text1.Location = new Point(label1.Left + 20, label1.Bottom + 10);
            text1.Size = new Size(400, 30);
            splitContainer.Panel1.Controls.Add(text1);

            Button button1 = new Button();
            button1.Location = new Point(text1.Right + 20, text1.Top);
            button1.Size = new Size(100, 30);
            button1.Text = "добавить";
            button1.Click += new EventHandler(button1_Click);
            splitContainer.Panel1.Controls.Add(button1);

            Label label2 = new Label();
            label2.Text = "введите путь в строку ниже для удаления из мониторинга";
            label2.Location = new Point(text1.Left - 20, text1.Bottom + 10);
            label2.AutoSize = true;
            splitContainer.Panel1.Controls.Add(label2);


            text2.Location = new Point(label2.Left + 20, label2.Bottom + 10);
            text2.Size = new Size(400, 30);
            splitContainer.Panel1.Controls.Add(text2);

            Button button2 = new Button();
            button2.Location = new Point(text2.Right + 20, text2.Top);
            button2.Size = new Size(100, 30);
            button2.Text = "удалить";
            button2.Click += new EventHandler(button2_Click);
            splitContainer.Panel1.Controls.Add(button2);

            //Label label3 = new Label();
            //label3.Text = "введите путь в строку ниже для удаления ключа из реестра";
            //label3.Location = new Point(text2.Left - 20, text2.Bottom + 10);
            //label3.AutoSize = true;
            //splitContainer.Panel1.Controls.Add(label3);


            //text3.Location = new Point(label3.Left + 20, label3.Bottom + 10);
            //text3.Size = new Size(400, 30);
            //splitContainer.Panel1.Controls.Add(text3);

            //Button button3 = new Button();
            //button3.Location = new Point(text3.Right + 20, text3.Top);
            //button3.Size = new Size(100, 30);
            //button3.Text = "удалить";
            //button3.Click += new EventHandler(button3_Click);
            //splitContainer.Panel1.Controls.Add(button3);

            //Label label4 = new Label();
            //label4.Text = "для добавления ключа в реестр введите путь в левую часть \n а в правую часть введите значение";
            //label4.Location = new Point(text3.Left - 20, text3.Bottom + 10);
            //label4.AutoSize = true;
            //splitContainer.Panel1.Controls.Add(label4);


            //text4.Location = new Point(label4.Left + 20, label4.Bottom + 10);
            //text4.Size = new Size(400, 30);
            //splitContainer.Panel1.Controls.Add(text4);

            //Button button4 = new Button();
            //button4.Location = new Point(text4.Right + 20, text4.Top);
            //button4.Size = new Size(100, 30);
            //button4.Text = "добавить";
            //button4.Click += new EventHandler(button4_Click);
            //splitContainer.Panel1.Controls.Add(button4);

            Label label7 = new Label();
            label7.Text = "введите количество минут для установки таймера";
            label7.Location = new Point(text2.Left, text2.Bottom + 10);
            label7.AutoSize = true;
            splitContainer.Panel1.Controls.Add(label7);

            text7.Location = new Point(label7.Left + 20, label7.Bottom + 10);
            text7.Size = new Size(400, 30);
            splitContainer.Panel1.Controls.Add(text7);

            Button button7 = new Button();
            button7.Location = new Point(text7.Left, text7.Bottom + 20);
            button7.Size = new Size(550, 30);
            button7.Text = "Установить таймер";
            button7.Click += new EventHandler(button7_Click);
            splitContainer.Panel1.Controls.Add(button7);

            Button button5 = new Button();
            button5.Location = new Point(button7.Left - 20, button7.Bottom + 20);
            button5.Size = new Size(550, 30);
            button5.Text = "Просмотреть все пути";
            button5.Click += new EventHandler(button5_Click);
            splitContainer.Panel1.Controls.Add(button5);

            Button button6 = new Button();
            button6.Location = new Point(button5.Left, button5.Bottom + 20);
            button6.Size = new Size(550, 30);
            button6.Text = "Обновить";
            button6.Click += new EventHandler(button6_Click);
            splitContainer.Panel1.Controls.Add(button6);

            splitContainer.Dock = DockStyle.Fill;
            datagrid.Dock = DockStyle.Fill;
            splitContainer.Panel2.Controls.Add(datagrid);
            splitContainer.Orientation = Orientation.Horizontal;
            this.Controls.Add(this.splitContainer);


        }
        private async Task StartWebSocketConnectionAsync()
        {
            string serverUri = "ws://localhost:6000/";

            // Запускаем WebSocket в фоновом потоке
            await Task.Run(async () =>
            {
                await SocketExtensions.ConnectWithRetry(serverUri);

            });
        }

        static bool Validate(string str)
        {
            string pattern = @"^(?:[a-zA-Z]:\\|\\\\)(?:[^\\:*?""<>|]+\\)*[^\\:*?""<>|]*$"; // Регулярное выражение для валидации пути
            string pattern2 = @"^(HKEY_CURRENT_USER|HKEY_LOCAL_MACHINE)\\([^\\]+\\)*[^\\]+$";
            if (Regex.IsMatch(str, pattern) || Regex.IsMatch(str, pattern2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            if (Validate(text2.Text))
            {
                string message = "remove from monitor " + text2.Text;
                if (Storage.currentWebSocket.State == WebSocketState.Open)
                {
                    await SocketExtensions.SendTextMessageAsync(Storage.currentWebSocket, message);
                    Storage.AllClients = await SocketExtensions.ReceiveClientLevelsAsync(Storage.currentWebSocket);
                }

            }
            else
            {
                MessageBox.Show("error", "erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void button3_Click(object sender, EventArgs e)
        {
            if (Validate(text7.Text))
            {
                string message = "remove from register" + text7.Text;
                if (Storage.currentWebSocket.State == WebSocketState.Open)
                {
                    await SocketExtensions.SendTextMessageAsync(Storage.currentWebSocket, message);
                    Storage.AllClients = await SocketExtensions.ReceiveClientLevelsAsync(Storage.currentWebSocket);
                }
            }
            else
            {
                MessageBox.Show("error", "erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void button4_Click(object sender, EventArgs e)
        {
            if (Validate(text4.Text))
            {
                string message = "add to register " + text4.Text;
                if (Storage.currentWebSocket.State == WebSocketState.Open)
                {
                    await SocketExtensions.SendTextMessageAsync(Storage.currentWebSocket, message);
                    Storage.AllClients = await SocketExtensions.ReceiveClientLevelsAsync(Storage.currentWebSocket);
                }
            }
            else
            {
                MessageBox.Show("error", "erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            if (Validate(text1.Text))
            {
                string message = "add to monitor " + text1.Text;
                if (Storage.currentWebSocket.State == WebSocketState.Open)
                {
                    await SocketExtensions.SendTextMessageAsync(Storage.currentWebSocket, message);
                    Storage.AllClients = await SocketExtensions.ReceiveClientLevelsAsync(Storage.currentWebSocket);
                    MessageBox.Show("succ", "erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("error", "erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (Storage.currentWebSocket.State == WebSocketState.Open)
            {
                Table table = new Table();
                table.Show();
            }
            else MessageBox.Show("Is not connected", "erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        private async void button6_Click(object sender, EventArgs e)
        {
           
                string message = "Update";
                if (Storage.currentWebSocket.State == WebSocketState.Open)
                {
                    await SocketExtensions.SendTextMessageAsync(Storage.currentWebSocket, message);
                    Storage.AllClients = await SocketExtensions.ReceiveClientLevelsAsync(Storage.currentWebSocket);
                }
           
        }
        private async void button7_Click(object sender, EventArgs e)
        {

            string message = "Set-Timer " + text7.Text; 
           // if ()
            if (Storage.currentWebSocket.State == WebSocketState.Open)
            {
                await SocketExtensions.SendTextMessageAsync(Storage.currentWebSocket, message);
                MessageBox.Show("Timer set" +text7.Text, "Timer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
        private async void Main_Load(object sender, EventArgs e)
        {
            await ConnectToServerAsync();
            await SocketExtensions.SendTextMessageAsync(Storage.currentWebSocket, "admin here"); // Отправляем сообщение при загрузке
            Storage.AllClients = await SocketExtensions.ReceiveClientLevelsAsync(Storage.currentWebSocket);
        }
        private async Task ConnectToServerAsync()
        {
            ClientWebSocket _webSocket = new ClientWebSocket();

            while (_webSocket.State != WebSocketState.Open)
            {
                _webSocket = new ClientWebSocket();
                Storage.currentWebSocket = _webSocket;
                try
                {
                    // Попытка подключения к серверу;
                    await _webSocket.ConnectAsync(new Uri("ws://localhost:6000"), CancellationToken.None);
                    
                }
                catch (WebSocketException ex)
                {
                    // Обработка ошибок подключения
                    Console.WriteLine($"Connection failed: {ex.Message}. Retrying...");
                    await Task.Delay(1000); // Задержка перед повторной попыткой
                }
                
            }
        }


        private async void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Storage.currentWebSocket.State != WebSocketState.Closed && Storage.currentWebSocket.State != WebSocketState.Aborted)
            {
                await Storage.currentWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            }
        }
    }
}
