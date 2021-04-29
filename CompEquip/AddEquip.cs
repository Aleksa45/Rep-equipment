using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompEquip
{
    public partial class AddEquip : Form
    {
        
        public AddEquip()
        {
            InitializeComponent();

            try
            {
                //получение данных сервером
                DataTable table = Program.ConnectionManager.Select("select * from Empl_View");
                comboBox2.DataSource = table;
                comboBox2.DisplayMember = table.Columns[1].ColumnName;
                comboBox2.ValueMember = table.Columns[0].ColumnName;
                comboBox2.SelectedIndex = 0;

                DataTable table1 = Program.ConnectionManager.Select("select * from TypeEquip");
                comboBox1.DataSource = table1;
                comboBox1.DisplayMember = table1.Columns[1].ColumnName;
                comboBox1.ValueMember = table1.Columns[0].ColumnName;
                comboBox1.SelectedIndex = 0;

                DataTable table2 = Program.ConnectionManager.Select("select * from LifeTime");
                comboBox3.DataSource = table2;
                comboBox3.DisplayMember = table2.Columns[1].ColumnName;
                comboBox3.ValueMember = table2.Columns[0].ColumnName;
                comboBox3.SelectedIndex = 0;

                DataTable table3 = Program.ConnectionManager.Select("select * from Status");
                comboBox4.DataSource = table3;
                comboBox4.DisplayMember = table3.Columns[1].ColumnName;
                comboBox4.ValueMember = table3.Columns[0].ColumnName;
                comboBox4.SelectedIndex = 0;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox3.Text))
            {
                try
                {
                    int idEquip = int.Parse(comboBox1.SelectedValue.ToString());
                    int idEmp = int.Parse(comboBox2.SelectedValue.ToString());
                    int idLife = int.Parse(comboBox3.SelectedValue.ToString());
                    int idStat = int.Parse(comboBox4.SelectedValue.ToString());

                    string mark = textBox1.Text;
                    string model = textBox2.Text;
                    string invent = textBox3.Text;

                    DateTime date = dateTimePicker1.Value;

                    string query = $"insert into Equipment (CodeTypeEquip, Mark, Model, InventoryNumber, CodeLifeTime, CodeStatus, DateOfPurch) Values({idEquip}, '{mark}', '{model}', '{invent}', {idLife}, {idStat}, '{date.ToString("dd-MM-yyyy")}')";

                    ////добавление данных в таблицу Equipment бд через SQL-запрос
                    Program.ConnectionManager.Add(query);
   
                    //нахождение через SQL-запрос значение идентификатора последней добавленной строки в таблице Equipment 
                    DataTable table = Program.ConnectionManager.Select("select MAX(CodeEquipment)from Equipment");
                    
                    //добавление найденного значения идентификатора и идентификатора сотрудника в таблицу EquipEmp бд через SQL-запрос
                    int idEq = int.Parse(table.Rows[0][0].ToString());

                    //отправка запроса на сервер
                    query = $"insert into EquipEmp (CodeEquipment, CodeEmployees) Values({idEq}, {idEmp})";
                    Program.ConnectionManager.Add(query);

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                } 
            }
            else 
            { 
                MessageBox.Show("Error 0x00000006: Есть пустые поля.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
