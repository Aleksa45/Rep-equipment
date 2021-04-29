using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.Sockets;

namespace CompEquip
{
    public partial class Form1 : Form
    {
        //строка подключения бд
        public const string connection = @"Data Source=DESKTOP-L4A77DQ\SQLEXPRESS; Initial Catalog=BDequipment; Integrated Security=true";
        public Form1()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 0;
            dataGridView1.DataSource = FindEquipment(new string[] {"","","","","" });
            if(dataGridView1.DataSource != null)
            {
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[8].Visible = false;
            }

            comboBox2.SelectedIndex = 0;
            dataGridView2.DataSource = FindEmployees(new string[] { "", "", "", "", "" });
            if(dataGridView2.DataSource != null)
            {
                dataGridView2.Columns[0].Visible = false;
            }
        }

        public DataTable FindEquipment(string[] args)
        {
            string query = $"exec find_Equip '{args[0]}','{args[1]}','{args[2]}','{args[3]}','{args[4]}'";

            DataTable table = null;

            try
            {
                table = Program.ConnectionManager.Select(query);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return table;
        }
        //обновление строк в таблице формы
        public void ResetData()
        {
            dataGridView1.DataSource = FindEquipment(new string[] { "", "", "", "", "" });
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[8].Visible = false;
        }

        private DataTable FindEmployees(string[] args)
        {
            string query = $"exec find_Empl '{args[0]}','{args[1]}','{args[2]}','{args[3]}','{args[4]}'";
            
            DataTable table = null;

            try
            {
                table = Program.ConnectionManager.Select(query);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return table;
        }
        //отображение на вкладке сотрудников закрепленного за ними оборудования
        private void button7_Click(object sender, EventArgs e)
        {
            string nameEmpl = "";
            try
            {
                if (dataGridView2.CurrentRow != null)
                {
                    nameEmpl = dataGridView2.CurrentRow.Cells[1].Value.ToString();
                    Form2 f2 = new Form2(FindEquipment(new string[] { "", "", nameEmpl, "", "" }), this);
                    f2.ShowDialog();
                    ResetData();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("За сотрудником нет закрепленного оборудования.");
            }
        }
        //поиск на вкладке "Оборудование"
        private void button4_Click(object sender, EventArgs e)
        {
            string[] args = new string[] { "", "", "", "", "" };
            args[comboBox1.SelectedIndex] = textBox1.Text;

            dataGridView1.DataSource = FindEquipment(args);
            dataGridView1.Columns[0].Visible = false;
        }
        //сброс поиска
        private void button6_Click(object sender, EventArgs e)
        {
            string[] args = new string[] { "", "", "", "", "" };

            textBox1.Clear();
            dataGridView1.DataSource = FindEquipment(args);
            dataGridView1.Columns[0].Visible = false;
        }
        //поиск на вкладке "Сотрудники"
        private void button5_Click(object sender, EventArgs e)
        {
            string[] args = new string[] { "", "", "", "", "" };
            args[comboBox2.SelectedIndex] = textBox2.Text;

            dataGridView2.DataSource = FindEmployees(args);
            dataGridView2.Columns[0].Visible = false;
        }
        //сброс поиска
        private void button8_Click(object sender, EventArgs e)
        {
            string[] args = new string[] { "", "", "", "", "" };

            textBox2.Clear();
            dataGridView2.DataSource = FindEmployees(args);
            dataGridView2.Columns[0].Visible = false;
        }
        //добавление данных
        private void button1_Click(object sender, EventArgs e)
        {
            AddEquip add = new AddEquip();
            add.ShowDialog();

            ResetData();
        }
        //удаление данных
        private void button3_Click(object sender, EventArgs e)
        {
            int id = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            int idEq = int.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString());
           
            try
            {
                Program.ConnectionManager.Remove($"delete from EquipEmp where CodeEquipEmp = ${id} delete from Equipment where CodeEquipment= ${idEq}");
            }
             catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ResetData();
        }
        //изменение данных
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                UpdateEquip update = new UpdateEquip(dataGridView1.CurrentRow);
                update.ShowDialog();
            }


            ResetData();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
