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
using System.Diagnostics;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Xml.Linq;
using System.Net.Security;
using System.Text.RegularExpressions;
using LiveCharts.Wpf;
using LiveCharts;
using System.Globalization;
using System.Drawing.Printing;
using LiveCharts.WinForms;
using System.Windows.Controls;

namespace diplom
{
    public partial class mainWindow : Form
    {
        private LiveCharts.WinForms.PieChart pieChart;
        int ConstPieChart = 0;
        NpgsqlConnection connection = new NpgsqlConnection("server=localhost;port=5432;username=postgres;password=1234;database=diplom");

        public mainWindow()
        {
            InitializeComponent();

            connection.Open();

            CurrentTable.Visible = false;
            CurrentTable_value.Visible = false;

            NumberOfRecords_label1.Visible = false;
            NumberOfRecords_combobox.Visible = false;
            NumberOfRecords_label2.Visible = false;

            MainpageDGV.Visible = false;
            DBdelete.Visible = false;
            DBupdate.Visible = false;
            DBspisat.Visible = false;

            NumberOfRecords_combobox.Items.Add("10");
            NumberOfRecords_combobox.Items.Add("25");
            NumberOfRecords_combobox.Items.Add("50");
            NumberOfRecords_combobox.Items.Add("75");
            NumberOfRecords_combobox.Items.Add("100");

            LoadDataAndCreateChart();
        }

        private void LoadDataAndCreateChart()
        {
            if (ConstPieChart == 0)
            {
                using (var command = new NpgsqlCommand("SELECT * FROM calculate_diagram()", connection))
                {
                    DataSet dataSet = new DataSet();

                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                    adapter.Fill(dataSet);

                    DataTable dataTable = dataSet.Tables[0];

                    dataGridView1.DataSource = dataTable;

                    double diagram_sales = Convert.ToDouble(dataGridView1.Rows[0].Cells[0].Value);
                    double diagram_loss = Convert.ToDouble(dataGridView1.Rows[0].Cells[1].Value);

                    double total = diagram_sales + diagram_loss;

                    if (pieChart == null)
                    {
                        pieChart = new LiveCharts.WinForms.PieChart
                        {
                            Width = 450,
                            Height = 305,
                            Dock = DockStyle.None,
                        };
                        Controls.Add(pieChart);
                    }
                    else
                    {
                        pieChart.Series.Clear();
                    }

                    PieSeries seriesSales = new PieSeries
                    {
                        Title = $"Продажи: {CalculatePercentage(diagram_sales, total):0.##}%",
                        Values = new ChartValues<double> { diagram_sales },
                        DataLabels = true
                    };

                    PieSeries seriesLoss = new PieSeries
                    {
                        Title = $"Расходы: {CalculatePercentage(diagram_loss, total):0.##}%",
                        Values = new ChartValues<double> { diagram_loss },
                        DataLabels = true
                    };

                    pieChart.Series.Add(seriesSales);
                    pieChart.Series.Add(seriesLoss);

                    pieChart.LegendLocation = LegendLocation.Right;

                    pieChart.Location = new System.Drawing.Point(260, 65);


                    pieChart.BringToFront();
                }
            }
            else
            {
                using (var command = new NpgsqlCommand("SELECT * FROM calculate_diagram_by_period(@Param1, @Param2)", connection))
                {
                    string date1 = textBox_first_period.Text;
                    string date2 = textBox_second_period.Text;

                    command.Parameters.AddWithValue("@Param1", DateTime.Parse(date1));
                    command.Parameters.AddWithValue("@Param2", DateTime.Parse(date2));

                    DataSet dataSet = new DataSet();

                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                    adapter.Fill(dataSet);

                    DataTable dataTable = dataSet.Tables[0];

                    dataGridView1.DataSource = dataTable;

                    double diagram_sales = Convert.ToDouble(dataGridView1.Rows[0].Cells[0].Value);
                    double diagram_loss = Convert.ToDouble(dataGridView1.Rows[0].Cells[1].Value);

                    double total = diagram_sales + diagram_loss;

                    if (pieChart == null)
                    {
                        pieChart = new LiveCharts.WinForms.PieChart
                        {
                            Width = 450,
                            Height = 305,
                            Dock = DockStyle.None,
                        };
                        Controls.Add(pieChart);
                    }
                    else
                    {
                        pieChart.Series.Clear();
                    }

                    PieSeries seriesSales = new PieSeries
                    {
                        Title = $"Продажи: {CalculatePercentage(diagram_sales, total):0.##}%",
                        Values = new ChartValues<double> { diagram_sales },
                        DataLabels = true
                    };

                    PieSeries seriesLoss = new PieSeries
                    {
                        Title = $"Расходы: {CalculatePercentage(diagram_loss, total):0.##}%",
                        Values = new ChartValues<double> { diagram_loss },
                        DataLabels = true
                    };

                    pieChart.Series.Add(seriesSales);
                    pieChart.Series.Add(seriesLoss);

                    pieChart.LegendLocation = LegendLocation.Right;

                    pieChart.Location = new System.Drawing.Point(260, 65);


                    pieChart.BringToFront();
                }
            }
        }

        private double CalculatePercentage(double part, double total)
        {
            if (total == 0)
                return 0;
            return (part / total) * 100;
        }

        private void textBox_first_period_Enter(object sender, EventArgs e)
        {
            if (textBox_first_period.Text == "Введите первую дату")
            {
                textBox_first_period.Text = "";
                textBox_first_period.ForeColor = Color.Black;
            }
        }

        private void textBox_first_period_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_first_period.Text))
            {
                textBox_first_period.Text = "Введите первую дату";
                textBox_first_period.ForeColor = Color.Gray;
            }
        }

        private void textBox_second_period_Enter(object sender, EventArgs e)
        {
            if (textBox_second_period.Text == "Введите вторую дату")
            {
                textBox_second_period.Text = "";
                textBox_second_period.ForeColor = Color.Black;
            }
        }

        private void textBox_second_period_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_second_period.Text))
            {
                textBox_second_period.Text = "Введите вторую дату";
                textBox_second_period.ForeColor = Color.Gray;
            }
        }

        private void stats_alltime_Click(object sender, EventArgs e)
        {
            if (ConstPieChart == 0)
            {
                return;
            }
            else
            {
                ConstPieChart = 0;

                textbox_stats_all_or_period.Text = "Статистика за весь период";
                textbox_stats_all_or_period.Location = new Point(400, 40);

                textBox_first_period.Text = "Введите первую дату";
                textBox_first_period.ForeColor = Color.Gray;

                textBox_second_period.Text = "Введите вторую дату";
                textBox_second_period.ForeColor = Color.Gray;


                using (var command = new NpgsqlCommand("SELECT * from calculate_bestseller()", connection))
                {
                    DataSet DataSet = new DataSet();

                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                    adapter.Fill(DataSet);

                    DataTable DataTable = DataSet.Tables[0];

                    dataGridView1.DataSource = DataTable;

                    var result = command.ExecuteScalar();

                    string bestseller_product = dataGridView1.Rows[0].Cells[0].Value.ToString();
                    textBox_bestseller_product.Text = bestseller_product;
                    string bestseller_quantity = dataGridView1.Rows[0].Cells[1].Value.ToString();
                    textBox_bestseller_quantity.Text = bestseller_quantity;
                }

                using (var command = new NpgsqlCommand("SELECT * from calculate_revenue5()", connection))
                {
                    DataSet DataSet = new DataSet();

                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                    adapter.Fill(DataSet);

                    DataTable DataTable = DataSet.Tables[0];

                    dataGridView1.DataSource = DataTable;

                    var result = command.ExecuteScalar();

                    string product1 = dataGridView1.Rows[0].Cells[0].Value.ToString();
                    textBox_product1.Text = product1;
                    string margin1 = dataGridView1.Rows[0].Cells[1].Value.ToString();
                    textBox_margin1.Text = margin1;

                    string product2 = dataGridView1.Rows[1].Cells[0].Value.ToString();
                    textBox_product2.Text = product2;
                    string margin2 = dataGridView1.Rows[1].Cells[1].Value.ToString();
                    textBox_margin2.Text = margin2;

                    string product3 = dataGridView1.Rows[2].Cells[0].Value.ToString();
                    textBox_product3.Text = product3;
                    string margin3 = dataGridView1.Rows[2].Cells[1].Value.ToString();
                    textBox_margin3.Text = margin3;

                    string product4 = dataGridView1.Rows[3].Cells[0].Value.ToString();
                    textBox_product4.Text = product4;
                    string margin4 = dataGridView1.Rows[3].Cells[1].Value.ToString();
                    textBox_margin4.Text = margin4;

                    string product5 = dataGridView1.Rows[4].Cells[0].Value.ToString();
                    textBox_product5.Text = product5;
                    string margin5 = dataGridView1.Rows[4].Cells[1].Value.ToString();
                    textBox_margin5.Text = margin5;
                }

                LoadDataAndCreateChart();
            }
        }

        private void stats_period_Click(object sender, EventArgs e)
        {
            ConstPieChart = 1;

            string date1 = textBox_first_period.Text;
            string date2 = textBox_second_period.Text;

            textbox_stats_all_or_period.Text = "Статистика за период: " + date1 + " - " + date2;
            textbox_stats_all_or_period.Location = new Point(360, 40);

            using (var command = new NpgsqlCommand("SELECT * from calculate_bestseller_by_period(@Param1, @Param2)", connection))
            {
                command.Parameters.AddWithValue("@Param1", DateTime.Parse(date1));
                command.Parameters.AddWithValue("@Param2", DateTime.Parse(date2));

                DataSet DataSet = new DataSet();

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                adapter.Fill(DataSet);

                DataTable DataTable = DataSet.Tables[0];

                dataGridView1.DataSource = DataTable;

                var result = command.ExecuteScalar();

                string bestseller_product = dataGridView1.Rows[0].Cells[0].Value.ToString();
                textBox_bestseller_product.Text = bestseller_product;
                string bestseller_quantity = dataGridView1.Rows[0].Cells[1].Value.ToString();
                textBox_bestseller_quantity.Text = bestseller_quantity;
            }

            using (var command = new NpgsqlCommand("SELECT * from calculate_revenue5_by_period(@Param1, @Param2)", connection))
            {
                command.Parameters.AddWithValue("@Param1", DateTime.Parse(date1));
                command.Parameters.AddWithValue("@Param2", DateTime.Parse(date2));

                DataSet DataSet = new DataSet();

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                adapter.Fill(DataSet);

                DataTable DataTable = DataSet.Tables[0];

                dataGridView1.DataSource = DataTable;

                var result = command.ExecuteScalar();

                string product1 = dataGridView1.Rows[0].Cells[0].Value.ToString();
                textBox_product1.Text = product1;
                string margin1 = dataGridView1.Rows[0].Cells[1].Value.ToString();
                textBox_margin1.Text = margin1;

                string product2 = dataGridView1.Rows[1].Cells[0].Value.ToString();
                textBox_product2.Text = product2;
                string margin2 = dataGridView1.Rows[1].Cells[1].Value.ToString();
                textBox_margin2.Text = margin2;

                string product3 = dataGridView1.Rows[2].Cells[0].Value.ToString();
                textBox_product3.Text = product3;
                string margin3 = dataGridView1.Rows[2].Cells[1].Value.ToString();
                textBox_margin3.Text = margin3;

                string product4 = dataGridView1.Rows[3].Cells[0].Value.ToString();
                textBox_product4.Text = product4;
                string margin4 = dataGridView1.Rows[3].Cells[1].Value.ToString();
                textBox_margin4.Text = margin4;

                string product5 = dataGridView1.Rows[4].Cells[0].Value.ToString();
                textBox_product5.Text = product5;
                string margin5 = dataGridView1.Rows[4].Cells[1].Value.ToString();
                textBox_margin5.Text = margin5;
            }

            LoadDataAndCreateChart();
        }

        private void mainWindow_Load(object sender, EventArgs e)
        {
            ConstPieChart = 0;

            textbox_stats_all_or_period.Text = "Статистика за весь период";
            textbox_stats_all_or_period.Location = new Point(410, 40);

            dataGridView1.Visible = false;

            textBox_first_period.Text = "Введите первую дату";
            textBox_first_period.ForeColor = Color.Gray;

            textBox_second_period.Text = "Введите вторую дату";
            textBox_second_period.ForeColor = Color.Gray;

            DateTime Now = DateTime.Now;
            string formattedDate = Now.ToString("ddMMyyyyHHmm");

            string basePath = Environment.CurrentDirectory;

            DirectoryInfo parentDirectory = Directory.GetParent(basePath);
            DirectoryInfo parentDirectory2 = Directory.GetParent(parentDirectory.FullName);
            DirectoryInfo parentDirectory3 = Directory.GetParent(parentDirectory2.FullName);

            string FinalPath = parentDirectory3.FullName;

            string batFileName = FinalPath + @"\" + Guid.NewGuid() + ".bat";
            string BackupPath = FinalPath + "\\DB_backup\\" + formattedDate + "OnOpen" + ".sql";

            using (StreamWriter batFile = new StreamWriter(batFileName))
            {
                batFile.WriteLine($"cd C:\\SQL\\bin");
                batFile.WriteLine($"SET PGPASSWORD=1234");
                batFile.WriteLine($"pg_dump -U postgres -d diplom > " + BackupPath);
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + batFileName);
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WindowStyle = ProcessWindowStyle.Normal;

            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            process.WaitForExit();

            File.Delete(batFileName);

            System.IO.FileInfo file = new System.IO.FileInfo(BackupPath);
            long Backupsize = file.Length;

            label_bestseller.ForeColor = Color.FromArgb(64, 134, 143);
            label_bestseller_sales.ForeColor = Color.FromArgb(64, 134, 143);
            textBox_bestseller_product.ForeColor = Color.FromArgb(64, 134, 143);
            textBox_bestseller_quantity.ForeColor = Color.FromArgb(64, 134, 143);
            label_down_product.ForeColor = Color.FromArgb(64, 134, 143);
            label_down_margin.ForeColor = Color.FromArgb(64, 134, 143);
            label_down_margin1.ForeColor = Color.FromArgb(64, 134, 143);


            using (var command = new NpgsqlCommand("SELECT * from calculate_bestseller()", connection))
            {
                DataSet DataSet = new DataSet();

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                adapter.Fill(DataSet);

                DataTable DataTable = DataSet.Tables[0];

                dataGridView1.DataSource = DataTable;

                var result = command.ExecuteScalar();

                string bestseller_product = dataGridView1.Rows[0].Cells[0].Value.ToString();
                textBox_bestseller_product.Text = bestseller_product;
                string bestseller_quantity = dataGridView1.Rows[0].Cells[1].Value.ToString();
                textBox_bestseller_quantity.Text = bestseller_quantity;
            }

            using (var command = new NpgsqlCommand("SELECT * from calculate_revenue5()", connection))
            {
                DataSet DataSet = new DataSet();

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                adapter.Fill(DataSet);

                DataTable DataTable = DataSet.Tables[0];

                dataGridView1.DataSource = DataTable;

                var result = command.ExecuteScalar();

                string product1 = dataGridView1.Rows[0].Cells[0].Value.ToString();
                textBox_product1.Text = product1;
                string margin1 = dataGridView1.Rows[0].Cells[1].Value.ToString();
                textBox_margin1.Text = margin1;

                string product2 = dataGridView1.Rows[1].Cells[0].Value.ToString();
                textBox_product2.Text = product2;
                string margin2 = dataGridView1.Rows[1].Cells[1].Value.ToString();
                textBox_margin2.Text = margin2;

                string product3 = dataGridView1.Rows[2].Cells[0].Value.ToString();
                textBox_product3.Text = product3;
                string margin3 = dataGridView1.Rows[2].Cells[1].Value.ToString();
                textBox_margin3.Text = margin3;

                string product4 = dataGridView1.Rows[3].Cells[0].Value.ToString();
                textBox_product4.Text = product4;
                string margin4 = dataGridView1.Rows[3].Cells[1].Value.ToString();
                textBox_margin4.Text = margin4;

                string product5 = dataGridView1.Rows[4].Cells[0].Value.ToString();
                textBox_product5.Text = product5;
                string margin5 = dataGridView1.Rows[4].Cells[1].Value.ToString();
                textBox_margin5.Text = margin5;
            }
        }

        private void Hide_main_page_items()
        {
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;


            label_bestseller.Visible = false;
            label_bestseller_sales.Visible = false;

            textBox_bestseller_product.Visible = false;
            textBox_bestseller_quantity.Visible = false;

            label_down_product.Visible = false;
            label_down_margin.Visible = false;
            label_down_margin1.Visible = false;

            label3.Visible = false;
            textBox_product1.Visible = false;
            textBox_margin1.Visible = false;
            textBox_product2.Visible = false;
            textBox_margin2.Visible = false;
            textBox_product3.Visible = false;
            textBox_margin3.Visible = false;
            textBox_product4.Visible = false;
            textBox_margin4.Visible = false;
            textBox_product5.Visible = false;
            textBox_margin5.Visible = false;
            label1.Visible = false;

            stats_alltime.Visible = false;
            textBox_first_period.Visible = false;
            textBox_second_period.Visible = false;
            stats_period.Visible = false;

            textbox_stats_all_or_period.Visible = false;

            pieChart.Visible = false;
        }

        private void Show_main_page_items()
        {
            ConstPieChart = 0;

            pictureBox1.Visible = true;
            pictureBox2.Visible = true;


            label_bestseller.Visible = true;
            label_bestseller_sales.Visible = true;

            textBox_bestseller_product.Visible = true;
            textBox_bestseller_quantity.Visible = true;

            label_down_product.Visible = true;
            label_down_margin.Visible = true;
            label_down_margin1.Visible = true;

            label3.Visible = true;
            textBox_product1.Visible = true;
            textBox_margin1.Visible = true;
            textBox_product2.Visible = true;
            textBox_margin2.Visible = true;
            textBox_product3.Visible = true;
            textBox_margin3.Visible = true;
            textBox_product4.Visible = true;
            textBox_margin4.Visible = true;
            textBox_product5.Visible = true;
            textBox_margin5.Visible = true;
            label1.Visible = true;

            stats_alltime.Visible = true;
            textBox_first_period.Visible = true;
            textBox_second_period.Visible = true;
            textBox_first_period.Text = "Введите первую дату";
            textBox_first_period.ForeColor = Color.Gray;

            textBox_second_period.Text = "Введите вторую дату";
            textBox_second_period.ForeColor = Color.Gray;
            stats_period.Visible = true;

            textbox_stats_all_or_period.Visible = true;

            pieChart.Visible = true;

            using (var command = new NpgsqlCommand("SELECT * from calculate_bestseller()", connection))
            {
                DataSet DataSet = new DataSet();

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                adapter.Fill(DataSet);

                DataTable DataTable = DataSet.Tables[0];

                dataGridView1.DataSource = DataTable;

                var result = command.ExecuteScalar();

                string bestseller_product = dataGridView1.Rows[0].Cells[0].Value.ToString();
                textBox_bestseller_product.Text = bestseller_product;
                string bestseller_quantity = dataGridView1.Rows[0].Cells[1].Value.ToString();
                textBox_bestseller_quantity.Text = bestseller_quantity;
            }

            using (var command = new NpgsqlCommand("SELECT * from calculate_revenue5()", connection))
            {
                DataSet DataSet = new DataSet();

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                adapter.Fill(DataSet);

                DataTable DataTable = DataSet.Tables[0];

                dataGridView1.DataSource = DataTable;

                var result = command.ExecuteScalar();

                string product1 = dataGridView1.Rows[0].Cells[0].Value.ToString();
                textBox_product1.Text = product1;
                string margin1 = dataGridView1.Rows[0].Cells[1].Value.ToString();
                textBox_margin1.Text = margin1;

                string product2 = dataGridView1.Rows[1].Cells[0].Value.ToString();
                textBox_product2.Text = product2;
                string margin2 = dataGridView1.Rows[1].Cells[1].Value.ToString();
                textBox_margin2.Text = margin2;

                string product3 = dataGridView1.Rows[2].Cells[0].Value.ToString();
                textBox_product3.Text = product3;
                string margin3 = dataGridView1.Rows[2].Cells[1].Value.ToString();
                textBox_margin3.Text = margin3;

                string product4 = dataGridView1.Rows[3].Cells[0].Value.ToString();
                textBox_product4.Text = product4;
                string margin4 = dataGridView1.Rows[3].Cells[1].Value.ToString();
                textBox_margin4.Text = margin4;

                string product5 = dataGridView1.Rows[4].Cells[0].Value.ToString();
                textBox_product5.Text = product5;
                string margin5 = dataGridView1.Rows[4].Cells[1].Value.ToString();
                textBox_margin5.Text = margin5;
            }

            LoadDataAndCreateChart();
        }

        Point lastPoint;
        private void MainPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void MainPanel_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void File_load_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string basePath = Environment.CurrentDirectory;

            DirectoryInfo parentDirectory = Directory.GetParent(basePath);
            DirectoryInfo parentDirectory2 = Directory.GetParent(parentDirectory.FullName);
            DirectoryInfo parentDirectory3 = Directory.GetParent(parentDirectory2.FullName);

            string FinalPath = parentDirectory3.FullName;

            OpenFileDialog OPF = new OpenFileDialog();
            OPF.InitialDirectory = FinalPath + "\\DB_backup";
            OPF.Multiselect = false;
            OPF.Filter = "Файлы sql|*.sql";
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                string batFileName = FinalPath + @"\" + Guid.NewGuid() + ".bat";

                using (StreamWriter batFile = new StreamWriter(batFileName))
                {
                    batFile.WriteLine($"cd C:\\SQL\\bin");
                    batFile.WriteLine($"SET PGPASSWORD=1234");
                    batFile.WriteLine($"psql -U postgres -d test -1 -f " + OPF.FileName);
                }

                ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + batFileName);
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;
                processStartInfo.WindowStyle = ProcessWindowStyle.Normal;

                Process process = new Process();
                process.StartInfo = processStartInfo;
                process.Start();
                process.WaitForExit();

                File.Delete(batFileName);

                MessageBox.Show("База данных обновлена", "Успех");
            }
            else
            {
                MessageBox.Show("Не удалось обновить базу данных", "Ошибка");
            }
        }

        private void File_unload_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime Now = DateTime.Now;
            string formattedDate = Now.ToString("ddMMyyyyHHmm");

            string basePath = Environment.CurrentDirectory;

            DirectoryInfo parentDirectory = Directory.GetParent(basePath);
            DirectoryInfo parentDirectory2 = Directory.GetParent(parentDirectory.FullName);
            DirectoryInfo parentDirectory3 = Directory.GetParent(parentDirectory2.FullName);

            string FinalPath = parentDirectory3.FullName;

            string batFileName = FinalPath + @"\" + Guid.NewGuid() + ".bat";
            string BackupPath = FinalPath + "\\DB_backup\\" + formattedDate + ".sql";

            using (StreamWriter batFile = new StreamWriter(batFileName))
            {
                batFile.WriteLine($"cd C:\\SQL\\bin");
                batFile.WriteLine($"SET PGPASSWORD=1234");
                batFile.WriteLine($"pg_dump -U postgres -d diplom > " + BackupPath);
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + batFileName);
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WindowStyle = ProcessWindowStyle.Normal;

            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            process.WaitForExit();

            File.Delete(batFileName);

            System.IO.FileInfo file = new System.IO.FileInfo(BackupPath);
            long Backupsize = file.Length;

            if (File.Exists(BackupPath) && Backupsize != 0)
            {
                MessageBox.Show("База данных успешно выгружена по пути:" + FinalPath + "\\DB_backup" + formattedDate, "Успех");
            }
            else
            {
                MessageBox.Show("Не удалось выгрузить базу данных", "Ошибка");
            }
        }

        private void File_exit_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime Now = DateTime.Now;
            string formattedDate = Now.ToString("ddMMyyyyHHmm");

            string basePath = Environment.CurrentDirectory;

            DirectoryInfo parentDirectory = Directory.GetParent(basePath);
            DirectoryInfo parentDirectory2 = Directory.GetParent(parentDirectory.FullName);
            DirectoryInfo parentDirectory3 = Directory.GetParent(parentDirectory2.FullName);

            string FinalPath = parentDirectory3.FullName;

            string batFileName = FinalPath + @"\" + Guid.NewGuid() + ".bat";
            string BackupPath = FinalPath + "\\DB_backup\\" + formattedDate + "OnClose" + ".sql";

            using (StreamWriter batFile = new StreamWriter(batFileName))
            {
                batFile.WriteLine($"cd C:\\SQL\\bin");
                batFile.WriteLine($"SET PGPASSWORD=1234");
                batFile.WriteLine($"pg_dump -U postgres -d diplom > " + BackupPath);
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + batFileName);
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WindowStyle = ProcessWindowStyle.Normal;

            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            process.WaitForExit();

            File.Delete(batFileName);

            System.IO.FileInfo file = new System.IO.FileInfo(BackupPath);
            long Backupsize = file.Length;

            this.Close();
        }

        private void mainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            DateTime Now = DateTime.Now;
            string formattedDate = Now.ToString("ddMMyyyyHHmm");

            string basePath = Environment.CurrentDirectory;

            DirectoryInfo parentDirectory = Directory.GetParent(basePath);
            DirectoryInfo parentDirectory2 = Directory.GetParent(parentDirectory.FullName);
            DirectoryInfo parentDirectory3 = Directory.GetParent(parentDirectory2.FullName);

            string FinalPath = parentDirectory3.FullName;

            string batFileName = FinalPath + @"\" + Guid.NewGuid() + ".bat";
            string BackupPath = FinalPath + "\\DB_backup\\" + formattedDate + "OnClose" + ".sql";

            using (StreamWriter batFile = new StreamWriter(batFileName))
            {
                batFile.WriteLine($"cd C:\\SQL\\bin");
                batFile.WriteLine($"SET PGPASSWORD=1234");
                batFile.WriteLine($"pg_dump -U postgres -d diplom > " + BackupPath);
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + batFileName);
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WindowStyle = ProcessWindowStyle.Normal;

            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            process.WaitForExit();

            File.Delete(batFileName);

            System.IO.FileInfo file = new System.IO.FileInfo(BackupPath);
            long Backupsize = file.Length;
        }

        private void View_mainpage_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show_main_page_items();

            textbox_stats_all_or_period.Text = "Статистика за весь период";
            textbox_stats_all_or_period.Location = new Point(410, 40);

            CurrentTable.Visible = false;
            CurrentTable_value.Visible = false;

            NumberOfRecords_label1.Visible = false;
            NumberOfRecords_combobox.Visible = false;
            NumberOfRecords_label2.Visible = false;

            MainpageDGV.Visible = false;
            DBdelete.Visible = false;
            DBupdate.Visible = false;
            DBspisat.Visible = false;
        }

        string selectedTable;
        private void View_db_directory_stock_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide_main_page_items();

            CurrentTable.Visible = true;
            CurrentTable_value.Visible = true;

            NumberOfRecords_label1.Visible = true;
            NumberOfRecords_combobox.Visible = true;
            NumberOfRecords_label2.Visible = true;

            MainpageDGV.Visible = true;
            DBdelete.Visible = true;
            DBupdate.Visible = true;
            DBspisat.Visible = false;

            DataSet DBstock = new DataSet();

            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM stock", connection);

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
            adapter.Fill(DBstock);

            DataTable stock = DBstock.Tables[0];

            MainpageDGV.DataSource = null;
            MainpageDGV.DataSource = stock;

            selectedTable = "stock";

            CurrentTable_value.Text = "Товары";
        }

        private void View_db_directory_partners_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide_main_page_items();

            CurrentTable.Visible = true;
            CurrentTable_value.Visible = true;

            NumberOfRecords_label1.Visible = true;
            NumberOfRecords_combobox.Visible = true;
            NumberOfRecords_label2.Visible = true;

            MainpageDGV.Visible = true;
            DBdelete.Visible = true;
            DBupdate.Visible = true;
            DBspisat.Visible = false;

            DataSet DBbuyers = new DataSet();

            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM buyers", connection);

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
            adapter.Fill(DBbuyers);

            DataTable buyers = DBbuyers.Tables[0];

            MainpageDGV.DataSource = null;
            MainpageDGV.DataSource = buyers;

            selectedTable = "buyers";

            CurrentTable_value.Text = "Покупатели";
        }

        private void View_db_directory_remains_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide_main_page_items();

            CurrentTable.Visible = true;
            CurrentTable_value.Visible = true;

            NumberOfRecords_label1.Visible = true;
            NumberOfRecords_combobox.Visible = true;
            NumberOfRecords_label2.Visible = true;

            MainpageDGV.Visible = true;
            DBdelete.Visible = false;
            DBupdate.Visible = false;
            DBspisat.Visible = false;

            DataSet DBremains = new DataSet();

            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM remains", connection);

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
            adapter.Fill(DBremains);

            DataTable remains = DBremains.Tables[0];

            MainpageDGV.DataSource = null;
            MainpageDGV.DataSource = remains;

            selectedTable = "remains";

            CurrentTable_value.Text = "Остатки";
        }

        private void View_db_directory_bad_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide_main_page_items();

            CurrentTable.Visible = true;
            CurrentTable_value.Visible = true;

            NumberOfRecords_label1.Visible = true;
            NumberOfRecords_combobox.Visible = true;
            NumberOfRecords_label2.Visible = true;

            MainpageDGV.Visible = true;
            DBdelete.Visible = false;
            DBupdate.Visible = false;
            DBspisat.Visible = false;

            DataSet DBbad = new DataSet();

            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM bad", connection);

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
            adapter.Fill(DBbad);

            DataTable bad = DBbad.Tables[0];

            MainpageDGV.DataSource = null;
            MainpageDGV.DataSource = bad;

            selectedTable = "bad";

            CurrentTable_value.Text = "Списано";
        }

        private void View_db_operations_sell_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide_main_page_items();

            CurrentTable.Visible = true;
            CurrentTable_value.Visible = true;

            NumberOfRecords_label1.Visible = true;
            NumberOfRecords_combobox.Visible = true;
            NumberOfRecords_label2.Visible = true;

            MainpageDGV.Visible = true;
            DBdelete.Visible = true;
            DBupdate.Visible = true;
            DBspisat.Visible = false;

            DataSet DBsales = new DataSet();

            NpgsqlCommand command = new NpgsqlCommand("select \"Код продажи\", \"Код покупателя\", \"Код товара\", \"Покупатель\", \"Товар\", \"Дата продажи\", \"Количество\", \"Сумма\" from sales;", connection);

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
            adapter.Fill(DBsales);

            DataTable sales = DBsales.Tables[0];

            MainpageDGV.DataSource = null;
            MainpageDGV.DataSource = sales;

            selectedTable = "sales";

            CurrentTable_value.Text = "Реализация товаров";
        }

        private void View_db_operations_make_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide_main_page_items();

            CurrentTable.Visible = true;
            CurrentTable_value.Visible = true;

            NumberOfRecords_label1.Visible = true;
            NumberOfRecords_combobox.Visible = true;
            NumberOfRecords_label2.Visible = true;

            MainpageDGV.Visible = true;
            DBdelete.Visible = false;
            DBupdate.Visible = true;
            DBspisat.Visible = true;

            DataSet DBmakings = new DataSet();

            NpgsqlCommand command = new NpgsqlCommand("select \"Код изготовления\", \"Код товара\", \"Товар\", \"Дата изготовления\", \"Количество\", \"Распродано/Списано\" from makings;", connection);

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
            adapter.Fill(DBmakings);

            DataTable makings = DBmakings.Tables[0];

            MainpageDGV.DataSource = null;
            MainpageDGV.DataSource = makings;

            int R = makings.Rows.Count;
            int k = 0;

            while (R > 0)
            {
                R -= 1;
                if ((int)makings.Rows[k]["Количество"] != (int)makings.Rows[k]["Распродано/Списано"])
                {
                    int code = (int)makings.Rows[k]["Код изготовления"];

                    var selectQuery = $"SELECT is_expired(@Param1)";

                    var cmd = new NpgsqlCommand(selectQuery, connection);

                    cmd.Parameters.AddWithValue("@Param1", code);

                    bool result = (bool)cmd.ExecuteScalar();

                    if (result)
                    {
                        var currentRow = MainpageDGV.Rows[k];

                        for (int i = 0; i < MainpageDGV.Columns.Count; i++)
                        {
                            currentRow.Cells[i].Style.BackColor = Color.LightCoral;
                            currentRow.Cells[4].ErrorText = "Срок годности вышел";
                        }
                    }
                }
                k += 1;
            }

            selectedTable = "makings";

            CurrentTable_value.Text = "Изготовление товаров";
        }

        private void About_about_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about_window about_window = new about_window();
            about_window.ShowDialog();
        }

        private void NumberOfRecords_combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string rowSelectedNumber = NumberOfRecords_combobox.Text.ToString();
            int intSelectedNumber;
            if (rowSelectedNumber == "")
            {
                intSelectedNumber = 1000;
            }
            else
            {
                intSelectedNumber = Convert.ToInt32(rowSelectedNumber);
            }

            if (selectedTable == "stock")
            {
                DataSet DBstock_byNumber = new DataSet();

                string NumberOfOutputQuery = "SELECT * FROM stock LIMIT (@Param1)";

                var cmd = new NpgsqlCommand(NumberOfOutputQuery, connection);

                cmd.Parameters.AddWithValue("@Param1", intSelectedNumber);

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(DBstock_byNumber);

                DataTable stock_by_number = DBstock_byNumber.Tables[0];

                MainpageDGV.DataSource = stock_by_number;
            }
            else if (selectedTable == "buyers")
            {
                DataSet DBbuyers_byNumber = new DataSet();

                string NumberOfOutputQuery = "SELECT * FROM buyers LIMIT (@Param1)";

                var cmd = new NpgsqlCommand(NumberOfOutputQuery, connection);

                cmd.Parameters.AddWithValue("@Param1", intSelectedNumber);

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(DBbuyers_byNumber);

                DataTable buyers_by_number = DBbuyers_byNumber.Tables[0];

                MainpageDGV.DataSource = buyers_by_number;
            }
            else if (selectedTable == "remains")
            {
                DataSet DBremains_byNumber = new DataSet();

                string NumberOfOutputQuery = "SELECT * FROM remains LIMIT (@Param1)";

                var cmd = new NpgsqlCommand(NumberOfOutputQuery, connection);

                cmd.Parameters.AddWithValue("@Param1", intSelectedNumber);

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(DBremains_byNumber);

                DataTable remains_by_number = DBremains_byNumber.Tables[0];

                MainpageDGV.DataSource = remains_by_number;
            }
            else if (selectedTable == "bad")
            {
                DataSet DBbad_byNumber = new DataSet();

                string NumberOfOutputQuery = "SELECT * FROM bad LIMIT (@Param1)";

                var cmd = new NpgsqlCommand(NumberOfOutputQuery, connection);

                cmd.Parameters.AddWithValue("@Param1", intSelectedNumber);

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(DBbad_byNumber);

                DataTable bad_by_number = DBbad_byNumber.Tables[0];

                MainpageDGV.DataSource = bad_by_number;
            }
            else if (selectedTable == "sales")
            {
                DataSet DBsales_byNumber = new DataSet();

                string NumberOfOutputQuery = "select \"Код продажи\", \"Код покупателя\", \"Код товара\", \"Покупатель\", \"Товар\", \"Дата продажи\", \"Количество\", \"Сумма\" from sales; LIMIT (@Param1)";

                var cmd = new NpgsqlCommand(NumberOfOutputQuery, connection);

                cmd.Parameters.AddWithValue("@Param1", intSelectedNumber);

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(DBsales_byNumber);

                DataTable sales_by_number = DBsales_byNumber.Tables[0];

                MainpageDGV.DataSource = sales_by_number;
            }
            else if (selectedTable == "makings")
            {
                DataSet DBmakings_byNumber = new DataSet();

                string NumberOfOutputQuery = "select \"Код изготовления\", \"Код товара\", \"Товар\", \"Дата изготовления\", \"Количество\", \"Распродано/Списано\" from makings; LIMIT (@Param1)";

                var cmd = new NpgsqlCommand(NumberOfOutputQuery, connection);

                cmd.Parameters.AddWithValue("@Param1", intSelectedNumber);

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(DBmakings_byNumber);

                DataTable makings_by_number = DBmakings_byNumber.Tables[0];

                MainpageDGV.DataSource = makings_by_number;
            }
            else
            {
                MessageBox.Show("Не выбрана таблица", "Ошибка");
            }
        }

        private void NumberOfRecords_combobox_TextUpdate(object sender, EventArgs e)
        {
            string rowSelectedNumber = NumberOfRecords_combobox.Text.ToString();
            int intSelectedNumber;
            if (rowSelectedNumber == "")
            {
                intSelectedNumber = 1000;
            }
            else if (int.TryParse(rowSelectedNumber, out int number))
            {
                intSelectedNumber = Convert.ToInt32(rowSelectedNumber);
            }
            else
            {
                MessageBox.Show("Ошибка ввода", "Ошибка");
                intSelectedNumber = 1000;
                NumberOfRecords_combobox.Text = "";
            }

            if (selectedTable == "stock")
            {
                DataSet DBstock_byNumber = new DataSet();

                string NumberOfOutputQuery = "SELECT * FROM stock LIMIT (@Param1)";

                var cmd = new NpgsqlCommand(NumberOfOutputQuery, connection);

                cmd.Parameters.AddWithValue("@Param1", intSelectedNumber);

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(DBstock_byNumber);

                DataTable stock_by_number = DBstock_byNumber.Tables[0];

                MainpageDGV.DataSource = stock_by_number;
            }
            else if (selectedTable == "buyers")
            {
                DataSet DBbuyers_byNumber = new DataSet();

                string NumberOfOutputQuery = "SELECT * FROM buyers LIMIT (@Param1)";

                var cmd = new NpgsqlCommand(NumberOfOutputQuery, connection);

                cmd.Parameters.AddWithValue("@Param1", intSelectedNumber);

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(DBbuyers_byNumber);

                DataTable buyers_by_number = DBbuyers_byNumber.Tables[0];

                MainpageDGV.DataSource = buyers_by_number;
            }
            else if (selectedTable == "remains")
            {
                DataSet DBremains_byNumber = new DataSet();

                string NumberOfOutputQuery = "SELECT * FROM remains LIMIT (@Param1)";

                var cmd = new NpgsqlCommand(NumberOfOutputQuery, connection);

                cmd.Parameters.AddWithValue("@Param1", intSelectedNumber);

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(DBremains_byNumber);

                DataTable remains_by_number = DBremains_byNumber.Tables[0];

                MainpageDGV.DataSource = remains_by_number;
            }
            else if (selectedTable == "bad")
            {
                DataSet DBbad_byNumber = new DataSet();

                string NumberOfOutputQuery = "SELECT * FROM bad LIMIT (@Param1)";

                var cmd = new NpgsqlCommand(NumberOfOutputQuery, connection);

                cmd.Parameters.AddWithValue("@Param1", intSelectedNumber);

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(DBbad_byNumber);

                DataTable bad_by_number = DBbad_byNumber.Tables[0];

                MainpageDGV.DataSource = bad_by_number;
            }
            else if (selectedTable == "sales")
            {
                DataSet DBsales_byNumber = new DataSet();

                string NumberOfOutputQuery = "select \"Код продажи\", \"Код покупателя\", \"Код товара\", \"Покупатель\", \"Товар\", \"Дата продажи\", \"Количество\", \"Сумма\" from sales; LIMIT (@Param1)";

                var cmd = new NpgsqlCommand(NumberOfOutputQuery, connection);

                cmd.Parameters.AddWithValue("@Param1", intSelectedNumber);

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(DBsales_byNumber);

                DataTable sales_by_number = DBsales_byNumber.Tables[0];

                MainpageDGV.DataSource = sales_by_number;
            }
            else if (selectedTable == "makings")
            {
                DataSet DBmakings_byNumber = new DataSet();

                string NumberOfOutputQuery = "select \"Код изготовления\", \"Код товара\", \"Товар\", \"Дата изготовления\", \"Количество\", \"Распродано/Списано\" from makings; LIMIT (@Param1)";

                var cmd = new NpgsqlCommand(NumberOfOutputQuery, connection);

                cmd.Parameters.AddWithValue("@Param1", intSelectedNumber);

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(DBmakings_byNumber);

                DataTable makings_by_number = DBmakings_byNumber.Tables[0];

                MainpageDGV.DataSource = makings_by_number;
            }
        }

        private void MainpageDGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var currentRow = MainpageDGV.Rows[e.RowIndex];

            for (int i = 0; i < MainpageDGV.Columns.Count; i++)
            {
                currentRow.Cells[i].Style.BackColor = Color.LightGreen;
            }
        }

        private void DBupdate_Click(object sender, EventArgs e)
        {
            if (selectedTable == "stock")
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    var sourceTable = (DataTable)MainpageDGV.DataSource;

                    if (sourceTable == null)
                    {
                        MessageBox.Show("Таблица не загружена.");
                        return;
                    }

                    var tableChanges = sourceTable.GetChanges();
                    if (tableChanges == null)
                    {
                        MessageBox.Show("Нет изменений в таблице.");
                        return;
                    }

                    string selectQuery;
                    selectQuery = $"SELECT * FROM stock";

                    var adapter = new NpgsqlDataAdapter(selectQuery, connection);
                    var commandBuilder = new NpgsqlCommandBuilder(adapter);
                    adapter.Update(tableChanges);

                    View_db_directory_stock_ToolStripMenuItem_Click(null, null);
                }
            }
            else if (selectedTable == "buyers")
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    var sourceTable = (DataTable)MainpageDGV.DataSource;

                    if (sourceTable == null)
                    {
                        MessageBox.Show("Таблица не загружена.");
                        return;
                    }

                    var tableChanges = sourceTable.GetChanges();
                    if (tableChanges == null)
                    {
                        MessageBox.Show("Нет изменений в таблице.");
                        return;
                    }

                    string selectQuery;
                    selectQuery = $"SELECT * FROM buyers";

                    var adapter = new NpgsqlDataAdapter(selectQuery, connection);
                    var commandBuilder = new NpgsqlCommandBuilder(adapter);
                    adapter.Update(tableChanges);

                    View_db_directory_partners_ToolStripMenuItem_Click(null, null);
                }
            }
            else if (selectedTable == "sales")
            {
                var sourceTable = (DataTable)MainpageDGV.DataSource;

                var find_expired_Query = $"SELECT find_expired()";

                var find_expired_cmd = new NpgsqlCommand(find_expired_Query, connection);

                int result = (int)find_expired_cmd.ExecuteScalar();

                if (result != 0 )
                {
                    MessageBox.Show("Есть товары с истекшим сроком годности, необходимо их списать!");
                    return;
                }


                if (sourceTable == null)
                {
                    MessageBox.Show("Таблица не загружена.");
                    return;
                }

                var tableChanges = sourceTable.GetChanges();
                if (tableChanges == null)
                {
                    MessageBox.Show("Нет изменений в таблице.");
                    return;
                }

                string selectQuery;
                selectQuery = $"select \"Код продажи\", \"Код покупателя\", \"Код товара\", \"Покупатель\", \"Товар\", \"Дата продажи\", \"Количество\", \"Сумма\" from sales;";

                var adapter = new NpgsqlDataAdapter(selectQuery, connection);
                var commandBuilder = new NpgsqlCommandBuilder(adapter);
                adapter.Update(tableChanges);

                View_db_operations_sell_ToolStripMenuItem_Click(null, null);

                int R = tableChanges.Rows.Count;
                int k = 0;

                while (R > 0)
                {
                    R -= 1;

                    int code = (int)tableChanges.Rows[k]["Код товара"];
                    int count = (int)tableChanges.Rows[k]["Количество"];

                    selectQuery = $"SELECT decrease_makings(@Param1, @Param2)";

                    var cmd = new NpgsqlCommand(selectQuery, connection);

                    cmd.Parameters.AddWithValue("@Param1", code);
                    cmd.Parameters.AddWithValue("@Param2", count);

                    cmd.ExecuteNonQuery();

                    k += 1;
                }

                string calculate_remains = "SELECT calculate_remains()";

                var cmd1 = new NpgsqlCommand(calculate_remains, connection);
                    
                cmd1.ExecuteNonQuery();

                View_db_operations_sell_ToolStripMenuItem_Click(null, null);
                
            }
            else if (selectedTable == "makings")
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    var sourceTable = (DataTable)MainpageDGV.DataSource;

                    if (sourceTable == null)
                    {
                        MessageBox.Show("Таблица не загружена.");
                        return;
                    }

                    var tableChanges = sourceTable.GetChanges();
                    if (tableChanges == null)
                    {
                        MessageBox.Show("Нет изменений в таблице.");
                        return;
                    }

                    string selectQuery;
                    selectQuery = $"select \"Код изготовления\", \"Код товара\", \"Товар\", \"Дата изготовления\", \"Количество\", \"Распродано/Списано\" from makings;";

                    var adapter = new NpgsqlDataAdapter(selectQuery, connection);
                    var commandBuilder = new NpgsqlCommandBuilder(adapter);
                    adapter.Update(tableChanges);

                    string calculate_remains = "SELECT calculate_remains()";

                    var cmd1 = new NpgsqlCommand(calculate_remains, connection);

                    cmd1.ExecuteNonQuery();

                    View_db_operations_make_ToolStripMenuItem_Click(null, null);
                }
            }
        }

        private void DBspisat_Click(object sender, EventArgs e)
        {
            string rowIndex = MainpageDGV.CurrentRow.Cells["Код изготовления"].Value.ToString();
            int id = Convert.ToInt32(rowIndex);

            string spisat = "SELECT spisat(@Param1)";

            var cmd = new NpgsqlCommand(spisat, connection);

            cmd.Parameters.AddWithValue("@Param1", id);

            cmd.ExecuteNonQuery();

            View_db_operations_make_ToolStripMenuItem_Click(null, null);
        }

        private void DBdelete_Click(object sender, EventArgs e)
        {
            if (selectedTable == "stock")
            {
                if (MainpageDGV.CurrentCell != null && MainpageDGV.CurrentCell.Selected)
                {
                    string rowIndex = MainpageDGV.CurrentRow.Cells["Код товара"].Value.ToString();
                    int id = Convert.ToInt32(rowIndex);

                    string deleteQuery = "SELECT delete_stock(@Param1)";

                    var cmd = new NpgsqlCommand(deleteQuery, connection);

                    cmd.Parameters.AddWithValue("@Param1", id);

                    cmd.ExecuteNonQuery();

                    View_db_directory_stock_ToolStripMenuItem_Click(null, null);
                }
                else
                {
                    MessageBox.Show("Не выбрана строка для удаления", "Ошибка");
                }
            }
            else if (selectedTable == "buyers")
            {
                if (MainpageDGV.CurrentCell != null && MainpageDGV.CurrentCell.Selected)
                {
                    string rowIndex = MainpageDGV.CurrentRow.Cells["Код покупателя"].Value.ToString();
                    int id = Convert.ToInt32(rowIndex);

                    string deleteQuery = "SELECT delete_buyer(@Param1)";

                    var cmd = new NpgsqlCommand(deleteQuery, connection);

                    cmd.Parameters.AddWithValue("@Param1", id);

                    cmd.ExecuteNonQuery();

                    View_db_directory_partners_ToolStripMenuItem_Click(null, null);
                }
                else
                {
                    MessageBox.Show("Не выбрана строка для удаления", "Ошибка");
                }
            }
            else if (selectedTable == "sales")
            {
                if (MainpageDGV.CurrentCell != null && MainpageDGV.CurrentCell.Selected)
                {
                    string rowIndex = MainpageDGV.CurrentRow.Cells["Код продажи"].Value.ToString();
                    int id = Convert.ToInt32(rowIndex);

                    string deleteQuery = "SELECT delete_sale(@Param1)";

                    var cmd = new NpgsqlCommand(deleteQuery, connection);

                    cmd.Parameters.AddWithValue("@Param1", id);

                    cmd.ExecuteNonQuery();

                    View_db_operations_sell_ToolStripMenuItem_Click(null, null);
                }
                else
                {
                    MessageBox.Show("Не выбрана строка для удаления", "Ошибка");
                }
            }
        }

        private void MainpageDGV_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string newValue = e.FormattedValue.ToString();

            char[] forbiddenChars = new char[] { '-', '@', '#', '$', '%', '^', '&', '*', '(', ')', '=', '+', '[', ']', '{', '}', '|', '\\', ';', ':', '\'', '\"', '<', '>', '/', '?', '`', '~' };

            foreach (char c in forbiddenChars)
            {
                if (newValue.Contains(c))
                {
                    e.Cancel = true;
                    MessageBox.Show($"Ввод символа '{c}' запрещен.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    ((DataGridView)sender).Rows[e.RowIndex].ErrorText = $"Ввод символа '{c}' запрещен.";

                    FileToolStripMenuItem.Enabled = false;
                    ViewToolStripMenuItem.Enabled = false;
                    AboutToolStripMenuItem1.Enabled = false;

                    return;
                }
                else
                {
                    FileToolStripMenuItem.Enabled = true;
                    ViewToolStripMenuItem.Enabled = true;
                    AboutToolStripMenuItem1.Enabled = true;

                    ((DataGridView)sender).Rows[e.RowIndex].ErrorText = string.Empty;
                }
            }

            if (selectedTable == "stock")
            {
                if (e.ColumnIndex == 2)
                {
                    string inputValue = e.FormattedValue.ToString();

                    if (inputValue != "кг" && inputValue != "шт" && inputValue != "")
                    {
                        e.Cancel = true;
                        MessageBox.Show($"Допустимые значения: 'кг' или 'шт'", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        ((DataGridView)sender).Rows[e.RowIndex].ErrorText = $"Допустимые значения: 'кг' или 'шт'";

                        FileToolStripMenuItem.Enabled = false;
                        ViewToolStripMenuItem.Enabled = false;
                        AboutToolStripMenuItem1.Enabled = false;

                        return;
                    }
                    else
                    {
                        FileToolStripMenuItem.Enabled = true;
                        ViewToolStripMenuItem.Enabled = true;
                        AboutToolStripMenuItem1.Enabled = true;

                        ((DataGridView)sender).Rows[e.RowIndex].ErrorText = string.Empty;
                    }
                }
            }
            else if (selectedTable == "buyers")
            {
                if (e.ColumnIndex == 2)
                {
                    string inputValue = e.FormattedValue.ToString();

                    if (inputValue.Length >= 10 && inputValue.Length <= 12 || inputValue == "")
                    {
                        FileToolStripMenuItem.Enabled = true;
                        ViewToolStripMenuItem.Enabled = true;
                        AboutToolStripMenuItem1.Enabled = true;

                        ((DataGridView)sender).Rows[e.RowIndex].ErrorText = string.Empty;
                    }
                    else
                    {
                        e.Cancel = true;
                        MessageBox.Show($"Неверный ИНН", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        ((DataGridView)sender).Rows[e.RowIndex].ErrorText = $"Неверный ИНН";

                        FileToolStripMenuItem.Enabled = false;
                        ViewToolStripMenuItem.Enabled = false;
                        AboutToolStripMenuItem1.Enabled = false;

                        return;
                    }
                }
            }
        }
    }
}