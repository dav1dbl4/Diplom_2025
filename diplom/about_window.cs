using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace diplom
{
    public partial class about_window : Form
    {
        public about_window()
        {
            InitializeComponent();

            string basePath = Environment.CurrentDirectory;

            DirectoryInfo parentDirectory = Directory.GetParent(basePath);
            DirectoryInfo parentDirectory2 = Directory.GetParent(parentDirectory.FullName);
            DirectoryInfo parentDirectory3 = Directory.GetParent(parentDirectory2.FullName);

            string FinalPath = parentDirectory3.FullName;

            about_window_path2.Text = FinalPath;
        }

        private void About_button_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
