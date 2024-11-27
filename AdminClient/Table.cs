using Newtonsoft.Json;
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
            int size = jsonString.Length;
            items = JsonConvert.DeserializeObject <List<ClientLevel>>(jsonString);
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
                        AddTabPage(day.Day.ToString(), day );
                    }
                }
            }
        }
        private (DataTable RegistryTable, DataTable FileTable) GetData(DayLevel day)
        {
            // Создаем таблицы для реестра и файлов
            DataTable registryTable = new DataTable();
            DataTable fileTable = new DataTable();

            // Устанавливаем колонки для таблицы реестра
            registryTable.Columns.Add("Registry Key");
            registryTable.Columns.Add("Value");

            // Устанавливаем колонки для таблицы файлов
            fileTable.Columns.Add("String Path");
            fileTable.Columns.Add("Size");
            fileTable.Columns.Add("Type");
            fileTable.Columns.Add("Quantity");
            fileTable.Columns.Add("LastWriteTime");

            // Добавляем колонки для CheckTime
            foreach (var checkTime in day.Data[0].CheckTime)
            {
                fileTable.Columns.Add(checkTime.ToString());
            }

            // Заполняем таблицы
            foreach (var data in day.Data)
            {
                // Обработка данных реестра
                if (data.RegistryValues != null)
                {
                    foreach (var kvp in data.RegistryValues)
                    {
                        DataRow registryRow = registryTable.NewRow();
                        registryRow["Registry Key"] = kvp.Key;
                        registryRow["Value"] = kvp.Value;
                        registryTable.Rows.Add(registryRow);
                    }
                }

                // Обработка файлов
                DataRow fileRow = fileTable.NewRow();
                fileRow["String Path"] = data.Path;
                fileRow["Size"] = string.Join("; \n\r", data.Size);
                fileRow["Type"] = data.Type;
                fileRow["Quantity"] = data.Quantity;
                fileRow["LastWriteTime"] = string.Join("; \n\r", data.LastWriteTime);

                // Установка значений статуса для каждой CheckTime
                for (int j = 0; j < data.CheckTime.Count; j++)
                {
                    if (data.CheckTime.Count == data.Status.Count)
                    {
                        fileRow[data.CheckTime[j].ToString()] = data.Status[j];
                    }
                }

                fileTable.Rows.Add(fileRow);
            }

            return (registryTable, fileTable); // Возвращаем обе таблицы в виде кортежа
        }

        private void AddTabPage(string title, DayLevel day)
        {
            var tabPage = new TabPage(title);
            DataGridView grid = new DataGridView();
            grid.Dock = DockStyle.Fill;
            //функция для пополнения data grid
            var (registryData, fileData) = GetData(day);
            TabControl tabs = new TabControl();
            tabs.Dock = DockStyle.Fill;

            DataGridView regs = new DataGridView();
            regs.Dock = DockStyle.Fill;
            regs.DataSource = registryData;

            var tabAge = new TabPage("Registry");
            tabAge.Dock = DockStyle.Fill;
            tabAge.Controls.Add(regs);
            tabs.Controls.Add(tabAge);

            DataGridView files = new DataGridView();
            files.Dock = DockStyle.Fill;
            files.DataSource = fileData;

            var tabAge2 = new TabPage("Folders and files");
            tabAge2.Dock = DockStyle.Fill;
            tabAge2.Controls.Add(files);
            tabs.Controls.Add(tabAge2);


            tabPage.Controls.Add(tabs);
            

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
