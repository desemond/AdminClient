using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminClient
{
    public partial class Table : Form
    {
        private SplitContainer splitContainer;
        private ListBox listBox;
        private TabControl tabControl;

        private List<ClientLevel> items = new List<ClientLevel>();
       
        public Table()
        {
            string jsonString = File.ReadAllText("Clients.json");
            items = JsonSerializer.Deserialize<List<ClientLevel>>(jsonString);
            this.splitContainer = new SplitContainer();
            this.listBox = new ListBox();
            this.tabControl = new TabControl();

            // Настройка SplitContainer
            this.splitContainer.Dock = DockStyle.Fill;
            this.splitContainer.Orientation = Orientation.Vertical;

            // Настройка ListBox (в Panel1)
            this.listBox.Dock = DockStyle.Fill;
            this.listBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;
            this.splitContainer.Panel1.Controls.Add(this.listBox);

            // Настройка TabControl (в Panel2)
            this.tabControl.Dock = DockStyle.Fill;
            this.splitContainer.Panel2.Controls.Add(this.tabControl);

            // Настройка формы
            this.Controls.Add(this.splitContainer);
            this.Text = "Dynamic TabPages Example";
            this.Size = new System.Drawing.Size(800, 600);
            this.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            LoadListBoxItems();
        }
        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                string selectedItem = listBox.SelectedItem.ToString();
                UpdateTabPages(selectedItem);
            }
        }
        private void UpdateTabPages(string selectedItem)
        {
            // Очищаем существующие вкладки
            tabControl.TabPages.Clear();

            // Создаём новые вкладки в зависимости от выбранного элемента
            foreach (var item in items)
            {
                if (selectedItem == item.ClientName)
                {
                    foreach (var day in item.Days) 
                    {
                        AddTabPage(day.Day.ToString(), day.Data );
                    }
                }
            }
        }
        private void AddTabPage(string title, List<DataLevel> data)
        {
            var tabPage = new TabPage(title);
            DataGridView grid = new DataGridView();
            grid.Dock = DockStyle.Fill;
            //функция для пополнения data grid
            tabPage.Controls.Add(grid);
            tabControl.TabPages.Add(tabPage);
        }
        private void LoadListBoxItems()
        {
            // Добавляем элементы в ListBox из списка
            foreach (var item in items)
            {
                listBox.Items.Add(item.ClientName);
            }
        }
        void Table_Load(object sender, EventArgs e)
        {

        }
    }
}
