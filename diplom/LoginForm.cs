using System;
using System.Collections.Generic;
using Npgsql;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Data.SqlClient;
using System.Net;
using NpgsqlTypes;

namespace diplom
{
    public partial class LoginForm : Form
    {
        NpgsqlConnection connection = new NpgsqlConnection("server=localhost;port=5432;username=postgres;password=1234;database=diplom");
        long rowsCount = 0;


        public LoginForm()
        {
            InitializeComponent();


            AUTHpassword.Text = "Пароль";
            AUTHpassword.ForeColor = Color.Gray;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT COUNT(*) FROM auth WHERE password=crypt(@password, 'nothing')", connection);
            command.Parameters.Add("@password", NpgsqlDbType.Text).Value = AUTHpassword.Text;
            rowsCount = (long)command.ExecuteScalar();
            if (rowsCount > 0)
            {
                Hide();
                mainWindow mainWindow = new mainWindow();
                mainWindow.ShowDialog();
                this.Close();
            }
            else
            {
                connection.Close();
                MessageBox.Show("Неправильный пароль", "Ошибка");

                AUTHpassword.Text = "Пароль";
                AUTHpassword.ForeColor = Color.Gray;
                AUTHpassword.UseSystemPasswordChar = false;
            }
        }

        Point lastPoint;
        private void LoginPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void LoginPanel_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void AUTHpassword_Enter(object sender, EventArgs e)
        {
            if (AUTHpassword.Text == "Пароль")
            {
                AUTHpassword.Text = "";
                AUTHpassword.ForeColor = Color.Black;
                AUTHpassword.UseSystemPasswordChar = true;
            }
        }

        private void AUTHpassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AUTHpassword.Text))
            {
                AUTHpassword.UseSystemPasswordChar = false;
                AUTHpassword.Text = "Пароль";
                AUTHpassword.ForeColor = Color.Gray;
            }
        }

        private void LoginForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Close();
        }
    }
}
