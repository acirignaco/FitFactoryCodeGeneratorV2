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
                //txtTableName.Text = txtTableName.Text.Remove(txtTableName.Text.Length - 1);
                txtTableName.Text = txtTableName.Text.Substring(0, txtTableName.Text.Length - 1);
                txtTableName.SelectionStart = txtTableName.Text.Length;

            }
            txtPluralName.Text = txtTableName.Text + "s";
        }


        public void ClearFields()
        {
            txtSelectFolder.Clear();
            txtTableName.Clear();
            txtPluralName.Clear();
            checkCore.Checked = false;
            dataGridPropertyFields.Rows.Clear();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            btnGenerate_Click(sender, e, dataGridPropertyFields);
        }

        private void btnGenerate_Click(object sender, EventArgs e, DataGridView dataGridPropertyFields)
        {
            string path = txtSelectFolder.Text + "\\" + txtTableName.Text + ".cs";
            // generate basic content for .cs file
            string csContent = GenerateCodeStructureCS(txtTableName.Text, dataGridPropertyFields);
            // do somehting that inputs in above file
            var currQty = "";
           

            StreamWriterCreate(path, csContent);

            
            //File.Create(path);
            MessageBox.Show("Successfully created text file to " + txtSelectFolder.Text);
            ClearFields();
            

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Path of file location with file appended</param>
        /// <param name="content"></param>
        public void StreamWriterCreate(string path, string content)
        {

            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(content);
                }
            }

        }


        public string GenerateCodeStructureCS(string filename, DataGridView dataGridView)
        {
            string codeStructure = "";
            string tab = "    ";
            string dtab = "        ";
            string imports = "using System;\nusing System.Collections.Generic;\nusing System.Linq;\nusing System.Text;\nusing System.Threading.Tasks;\nusing System.ComponentModel.DataAnnotations;\n";
            string namespaceAndClass = $"\nnamespace FitFactoryCodeGeneratorV2\n{{\n{tab}public class {char.ToUpper(filename[0]) + filename.Substring(1)} \n{tab}{{\n";

            codeStructure = imports + namespaceAndClass;

            foreach (DataGridViewRow row in dataGridPropertyFields.Rows)
            {
                if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "")
                {

                }
                else
                {
                    if ((row.Cells["IsKey"].Value != null) && (bool)row.Cells["IsKey"].Value == true)
                    {
                        codeStructure += dtab + "[Key]\n";
                    }
                    if ((row.Cells["Required"].Value != null) && (bool)row.Cells["Required"].Value == true)
                    {
                        codeStructure += dtab + "[Required]\n";
                    }
                    if ((row.Cells["Length"].Value != null) && !row.Cells["Length"].Value.Equals(""))
                    {
                        codeStructure += dtab + $"[MaxLength({row.Cells["Length"].Value})]\n";
                    }
                    codeStructure += dtab + "public ";
                    codeStructure += row.Cells["Type"].Value + " ";
                    codeStructure += row.Cells["PropertyName"].Value + " { get; set; } \n\n";
                }
            }
            codeStructure += "\n" + tab + "}" + "\n}";
            return codeStructure;
        }
    }
}