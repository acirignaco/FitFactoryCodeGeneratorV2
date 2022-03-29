namespace FitFactoryCodeGeneratorV2
{
    public partial class Form1 : Form
    {
        string tab = "    ";
        string dtab = "        ";


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
            bool successfulValidation = FormValidation();
            if (successfulValidation)
                btnGenerate_Click(sender, e, dataGridPropertyFields);
        }

        public bool FormValidation()
        {
            if (txtSelectFolder.Text == string.Empty)
            {
                MessageBox.Show("Please enter a folder location for Model!");
                return false;
            }
            else if (txtTableName.Text == string.Empty)
            {
                MessageBox.Show("Please enter a Table Name!");
                return false;
            }
            else if (txtPluralName.Text == string.Empty)
            {
                MessageBox.Show("Please enter a Plural Name!");
                return false;
            }
            return true;
        }
        private void btnGenerate_Click(object sender, EventArgs e, DataGridView dataGridPropertyFields)
        {
            string path = txtSelectFolder.Text + "\\" + txtTableName.Text + ".cs";

            if (File.Exists(path))
            {
                DialogResult dialogResult = MessageBox.Show("Do you wish to overwrite this file?", "Overwrite", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    CreateFiles();
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("Didn't update " + txtTableName.Text + ".cs");
                    ClearFields();
                }
            }
            else
            {
                CreateFiles();
            } 
        }

        public void CreateFiles()
        {
            string path = txtSelectFolder.Text + "\\" + txtTableName.Text + ".cs";
            string csContent = "";

            // backup file
            // overwrite file
            // generate basic content for .cs file
            csContent = GenerateCodeStructureCS(txtTableName.Text, dataGridPropertyFields);
            // do somehting that inputs in above file           
            StreamWriterCreate(path, csContent);
            MessageBox.Show("Successfully created " + txtTableName.Text + ".cs to " + txtSelectFolder.Text);

            //CREATE CLASS
            //string path = txtSelectFolder.Text + "\\" + txtTableName.Text + ".cs";
            //// generate basic content for .cs file
            //string csContent = GenerateCodeStructureCS(txtTableName.Text, dataGridPropertyFields);
            //// do somehting that inputs in above file           
            //StreamWriterCreate(path, csContent);


            // CREATE SERVICE CLASS
            string fileLocationCore = "C:\\Users\\William\\Desktop\\test\\Data\\" + txtTableName.Text + "Service.Core.cs";
            // generate basic content for .cs file
            csContent = GenerateCodeStructureServiceClass();
            // do somehting that inputs in above file           
            StreamWriterCreate(fileLocationCore, csContent);

            if (!checkCore.Checked)
            {
                string fileLocation = "C:\\Users\\William\\Desktop\\test\\Data\\" + txtTableName.Text + "Service.cs";
                csContent = "";
                StreamWriterCreate(fileLocation, csContent);

            }

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
                    sw.Close();
                }
            }
        }


        public string GenerateCodeStructureCS(string filename, DataGridView dataGridView)
        {
            string codeStructure = "";
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


        public string GenerateCodeStructureServiceClass()
        {
            string codeStructure = "";
            codeStructure += $"using Fitfactory.DataViews;\nusing Fitfactory.Helpers;\nusing Fitfactory.Models;\nusing Microsoft.EntityFrameworkCore;\nusing System.Linq.Dynamic.Core;\n\nnamespace Fitfactory.Data\n{{\n{tab}public class {txtTableName.Text}Service\n{tab}{{\n{dtab}private readonly AppDbContext _appDbContext;";


            codeStructure += $"\n\n{dtab}public {txtTableName.Text}Service(AppDbContext appDbContext)\n{dtab}{{\n{tab}{dtab}_appDbContext = appDbContext; \n{dtab}}}";
            codeStructure += $"\n\n{dtab}public {txtTableName.Text}? GetById(int Id)\n{dtab}{{\n{tab}{dtab}return _appDbContext.{txtPluralName.Text}.FirstOrDefault(c => c.Id == Id);\n{dtab}}}";
            codeStructure += $"\n\n{dtab}public List<{txtTableName.Text}ListItem> GetList(int pageIndex = 0, int pageSize = 0, string orderBy = \"\", string filterQuery = \"\")\n{dtab}{{\n{tab}{dtab}IQueryable<{txtTableName.Text}ListItem> dataList; \n{tab}{dtab}int count;\n{tab}{dtab}if (string.IsNullOrEmpty(orderBy) && string.IsNullOrEmpty(filterQuery))\n{tab}{dtab}{{\n{dtab}{dtab}dataList = _appDbContext.{txtPluralName.Text}List.AsQueryable();\n{dtab}{dtab}count = dataList.Count();\n{tab}{dtab}}}\n{tab}{dtab}else if (string.IsNullOrEmpty(orderBy) && !string.IsNullOrEmpty(filterQuery))\n{tab}{dtab}{{\n{dtab}{dtab}dataList = _appDbContext.{txtPluralName.Text}List.Where(filterQuery).AsQueryable();\n{dtab}{dtab}count = dataList.Count();\n{tab}{dtab}}}\n{tab}{dtab}else if (string.IsNullOrEmpty(filterQuery) && !string.IsNullOrEmpty(orderBy))\n{tab}{dtab}{{\n{dtab}{dtab}dataList = _appDbContext.{txtPluralName.Text}List.OrderBy(orderBy).AsQueryable();\n{dtab}{dtab}count = dataList.Count();\n{tab}{dtab}}}\n{tab}{dtab}else {{\n{dtab}{dtab}dataList = _appDbContext.{ txtPluralName.Text}List.OrderBy(orderBy).Where(filterQuery).AsQueryable();\n{dtab}{dtab}count = dataList.Count();\n{tab}{dtab}}}\n\n{tab}{dtab}if (pageSize > 0)\n{tab}{dtab}{{\n{dtab}{dtab}var pagedDataList = dataList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();\n{dtab}{dtab}return (new PaginatedList<{txtTableName.Text}ListItem> (pagedDataList, count, pageIndex, pageSize).ToList());\n{tab}{dtab}}}\n{tab}{dtab}else\n{tab}{dtab}{{\n{dtab}{dtab}return dataList.ToList();\n{dtab}{dtab}\n{tab}{dtab}}}\n{dtab}}}";
            codeStructure += $"\n\n{dtab}public Task<{txtTableName.Text}> Add({txtTableName.Text} dataObject)\n{dtab}{{\n{tab}{dtab}var addedObject = _appDbContext.{txtPluralName.Text}.Add(dataObject);\n{tab}{dtab}_appDbContext.SaveChanges();\n{tab}{dtab}return Task.FromResult(addedObject.Entity);\n{dtab}}}";
            codeStructure += $"\n\n{dtab}public bool Delete(int Id)\n{dtab}{{\n{tab}{dtab}var dataObject = _appDbContext.{txtPluralName.Text}.FirstOrDefault(e => e.Id == Id);\n{tab}{dtab}if (dataObject == null) return false;\n\n{tab}{dtab}_appDbContext.{ txtPluralName.Text}.Remove(dataObject);\n{tab}{dtab}_appDbContext.SaveChanges();\n{tab}{dtab}return true;\n{dtab}}}";


            codeStructure += $"\n\n{dtab}public {txtTableName.Text}? Update({txtTableName.Text} obj)\n{dtab}{{\n{tab}{dtab}var dataObject = _appDbContext.{txtPluralName.Text}.FirstOrDefault(e => e.Id == obj.Id);\n{tab}{dtab}if (dataObject != null)\n{tab}{dtab}{{";
            foreach (DataGridViewRow row in dataGridPropertyFields.Rows)
            {
                if (row.Cells[0].Value != null )
                {
                
                    codeStructure += $"\n{dtab}{dtab}dataObject.{row.Cells[0].Value} = obj.{row.Cells[0].Value};";                    
                }
            }

            codeStructure += $"\n\n{dtab}{dtab}_appDbContext.SaveChanges();\n{dtab}{dtab}return dataObject;\n{tab}{dtab}}}\n{tab}{dtab}return null;\n{dtab}}}";

            codeStructure += "\n" + tab + "}" + "\n}";
            return codeStructure;
        }

        private void txtPluralName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPluralName_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}