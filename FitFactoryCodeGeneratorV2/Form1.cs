namespace FitFactoryCodeGeneratorV2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            string folderName = folderBrowserDialog1.SelectedPath;
            txtSelectFolder.Text = folderName;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void dataGridPropertyFields_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        private void txtTableName_TextChanged(object sender, EventArgs e)
        {
            if (txtTableName.Text.Any() && !System.Text.RegularExpressions.Regex.IsMatch(txtTableName.Text, "^[a-zA-Z]+$"))
            {
                //MessageBox.Show("This textbox accepts only alphabetical characters");
                txtTableName.Text = txtTableName.Text.Remove(txtTableName.Text.Length - 1, 1);
            }
            txtPluralName.Text = txtTableName.Text + "s";
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string path = txtSelectFolder.Text + "\\" +txtTableName.Text + ".txt";
            File.Create(path);
            MessageBox.Show("Successfully created text file to " + txtSelectFolder.Text);
            ClearFields();
        }


        public void ClearFields()
        {
            txtSelectFolder.Clear();
            txtTableName.Clear();
            txtPluralName.Clear();
            checkCore.Checked = false;
            dataGridPropertyFields.Rows.Clear();
        }
    }
}