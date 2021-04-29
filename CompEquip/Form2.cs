using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompEquip
{
    public partial class Form2 : Form
    {
        Form1 form;
        public Form2(DataTable table,Form1 f)
        {
            InitializeComponent();
            this.form = f;
            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            label1.Text = dataGridView1.Rows[0].Cells[4].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (dataGridView1.CurrentRow != null)
            {
                UpdateEquip update = new UpdateEquip(dataGridView1.CurrentRow);
                update.ShowDialog();
            }

            ResetData();
        }

        private void ResetData()
        {
            dataGridView1.DataSource = form.FindEquipment(new string[] { "", "", label1.Text, "", "" });
            if (dataGridView1.DataSource != null)
            {
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[8].Visible = false;
            }
        }
    }
}
