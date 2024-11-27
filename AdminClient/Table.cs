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
    public partial class Table : Form
    {
        private SplitContainer splitContainer;
        private ListBox listBox;
        private TabControl tabControl;
        private List<string> items = new List<string> { "Item 1", "Item 2", "Item 3" };
        public Table()
        {
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
            if (selectedItem == "Item 1")
            {
                AddTabPage("Page 1", "Content for Item 1 - Page 1");
                AddTabPage("Page 2", "Content for Item 1 - Page 2");
            }
            else if (selectedItem == "Item 2")
            {
                AddTabPage("Page A", "Content for Item 2 - Page A");
                AddTabPage("Page B", "Content for Item 2 - Page B");
            }
            else if (selectedItem == "Item 3")
            {
                AddTabPage("Page X", "Content for Item 3 - Page X");
            }
        }
        private void AddTabPage(string title, string content)
        {
            var tabPage = new TabPage(title);
            var label = new Label
            {
                Text = content,
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            tabPage.Controls.Add(label);
            tabControl.TabPages.Add(tabPage);
        }
        private void LoadListBoxItems()
        {
            // Добавляем элементы в ListBox из списка
            foreach (var item in items)
            {
                listBox.Items.Add(item);
            }
        }
        void Table_Load(object sender, EventArgs e)
        {

        }
    }
}
