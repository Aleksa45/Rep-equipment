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
    public partial class ConnectionForm : Form
    {
        public ConnectionForm()
        {
            InitializeComponent();
            textBox1.Text = CompEquip.Properties.Settings.Default.IPAddress;
            textBox2.Text = CompEquip.Properties.Settings.Default.Port.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ipAddress = textBox1.Text.Replace(',', '.');
            int port = 8888;

            try
            {
                if (int.TryParse(textBox2.Text, out port))
                {
                    Program.ConnectionManager.Connect(ipAddress, port);
                    this.Hide();
                    new Form1().ShowDialog();
                }
                else
                {
                    MessageBox.Show("Некоректный порт");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
