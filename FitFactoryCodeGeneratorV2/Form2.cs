using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FitFactoryCodeGeneratorV2
{
    public partial class Form2 : Form
    {
        public static string SetValueForFolderLocation = "";

        public Form2()
        {
            InitializeComponent();
        }


        private void go_Click(object sender, EventArgs e)
        {

            bool successfulValidation = FormValidation();
            if (successfulValidation)
            {
                SetValueForFolderLocation = txtSelectFolder.Text;

                Form1 form1 = new Form1();
                form1.ShowDialog();
            }
        

        }
        public bool FormValidation()
        {
            if (txtSelectFolder.Text == string.Empty)
            {
                MessageBox.Show("Please enter a folder location!");
                return false;
            }
          
            return true;
        }

        //once clicked allows user to browse to select their project directory
        private void btnFolderBrowserDialog1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            string folderName = folderBrowserDialog1.SelectedPath;
            txtSelectFolder.Text = folderName;
        }
    }
}
