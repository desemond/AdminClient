using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminClient
{
    public partial class Main : Form
    {
        private SplitContainer splitContainer = new SplitContainer();
        private DataGridView datagrid = new DataGridView();
        public Main()
        {
            //InitializeComponent();
            this.Size = new System.Drawing.Size(1000, 800);
            this.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            
            Label label1 = new Label();
            label1.Text = "введите путь в строку ниже для добавления в мониторинг";
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            label1.AutoSize = true;
            splitContainer.Panel1.Controls.Add(label1);

            TextBox text1 = new TextBox();
            text1.Location = new Point(label1.Left + 20, label1.Bottom + 10);
            text1.Size = new Size(400, 30);
            splitContainer.Panel1.Controls.Add(text1);

            Button button1 = new Button();
            button1.Location = new Point(text1.Right + 20, text1.Top);
            button1.Size = new Size(100, 30);
            button1.Text = "добавить";
            splitContainer.Panel1.Controls.Add(button1);
         
            Label label2 = new Label();
            label2.Text = "введите путь в строку ниже для удаления из мониторинга";
            label2.Location = new Point(text1.Left - 20, text1.Bottom + 10);
            label2.AutoSize = true;
            splitContainer.Panel1.Controls.Add(label2);

            TextBox text2 = new TextBox();
            text2.Location = new Point(label2.Left + 20, label2.Bottom + 10);
            text2.Size = new Size(400, 30);
            splitContainer.Panel1.Controls.Add(text2);

            Button button2 = new Button();
            button2.Location = new Point(text2.Right + 20, text2.Top);
            button2.Size = new Size(100, 30);
            button2.Text = "удалить";
            splitContainer.Panel1.Controls.Add(button2);

            Label label3 = new Label();
            label3.Text = "введите путь в строку ниже для удаления ключа из реестра";
            label3.Location = new Point(text2.Left - 20, text2.Bottom + 10);
            label3.AutoSize = true;
            splitContainer.Panel1.Controls.Add(label3);

            TextBox text3 = new TextBox();
            text3.Location = new Point(label3.Left + 20, label3.Bottom + 10);
            text3.Size = new Size(400, 30);
            splitContainer.Panel1.Controls.Add(text3);

            Button button3 = new Button();
            button3.Location = new Point(text3.Right + 20, text3.Top);
            button3.Size = new Size(100, 30);
            button3.Text = "удалить";
            splitContainer.Panel1.Controls.Add(button3);

            Label label4 = new Label();
            label4.Text = "введите путь  в строку ниже для добавления ключа в реестр";
            label4.Location = new Point(text3.Left - 20, text3.Bottom + 10);
            label4.AutoSize = true;
            splitContainer.Panel1.Controls.Add(label4);

            TextBox text4 = new TextBox();
            text4.Location = new Point(label4.Left + 20, label4.Bottom + 10);
            text4.Size = new Size(400, 30);
            splitContainer.Panel1.Controls.Add(text4);

            Button button4 = new Button();
            button4.Location = new Point(text4.Right + 20, text4.Top);
            button4.Size = new Size(100, 30);
            button4.Text = "добавить";
            splitContainer.Panel1.Controls.Add(button4);

            splitContainer.Dock = DockStyle.Fill;
            datagrid.Dock = DockStyle.Fill;
            splitContainer.Panel2.Controls.Add(datagrid);
            splitContainer.Orientation=Orientation.Horizontal;
            this.Controls.Add(this.splitContainer);


        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}
