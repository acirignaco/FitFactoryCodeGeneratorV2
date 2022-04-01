using Npgsql;
using System.ComponentModel;

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

        //once clicked allows user to browse to select their project directory
        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            string folderName = folderBrowserDialog1.SelectedPath;
            txtSelectFolder.Text = folderName;
        }

        //once clicked, calls the clearfields method and clears all the fields
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void txtTableName_TextChanged(object sender, EventArgs e)
        {
            if (txtTableName.Text.Any() && !System.Text.RegularExpressions.Regex.IsMatch(txtTableName.Text, "^[a-zA-Z]+$"))
            {
                txtTableName.Text = txtTableName.Text.Substring(0, txtTableName.Text.Length - 1);
                txtTableName.SelectionStart = txtTableName.Text.Length;
            }
            txtPluralName.Text = txtTableName.Text + "s";
        }

        //method to clear fields in the form when called 
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
            string modelsPath = txtSelectFolder.Text + "\\Models\\" + txtTableName.Text + ".cs";
            string corePath = txtSelectFolder.Text + "\\Data\\" + txtTableName.Text + "Service.Core.cs";
            string servicePath = txtSelectFolder.Text + "\\Data\\" + txtTableName.Text + "Service.cs";
            string dataviewPath = txtSelectFolder.Text + "\\DataViews\\" + txtTableName.Text + "ListItem.cs";
            string appDbContextPath = txtSelectFolder.Text + "\\Data\\" + "AppDbContext.cs";

            List<string> paths = new List<string>();
            paths.Add(modelsPath);
            paths.Add(corePath);
            if (!checkCore.Checked)
            {
                paths.Add(servicePath);
            }
            paths.Add(dataviewPath);
            paths.Add(appDbContextPath);

            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    DialogResult dialogResult = MessageBox.Show($"WARNING!!! Do you wish to overwrite this file: {path}?", "Overwrite", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        OverrideFile(path);
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        MessageBox.Show("Didn't overwrite: " + path);
                    }
                }
                else
                {
                    CreateFile(path);
                }
            }

            GeneratePosgresTable();
            ClearFields();
        }

        /// <summary>
        /// Create a backup and then create new file 
        /// </summary>
        /// <param name="sourceFile"></param>
        public void OverrideFile(string sourceFile)
        {
            string destinationFile = "";

            // Move Model class file to BackupFolder
            if (sourceFile.Contains("Models"))
            {
                destinationFile = txtSelectFolder.Text + "\\BackupFiles\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss___") + txtTableName.Text + ".bak";
            }
            else if (sourceFile.Contains("Service.Core.cs"))
            {
                destinationFile = txtSelectFolder.Text + "\\BackupFiles\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss___") + txtTableName.Text + "Service.Core.bak";
            }
            else if (sourceFile.Contains("Service.cs"))
            {
                destinationFile = txtSelectFolder.Text + "\\BackupFiles\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss___") + txtTableName.Text + "Service.bak";
            }
            else if (sourceFile.Contains("DataViews"))
            {
                destinationFile = txtSelectFolder.Text + "\\BackupFiles\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss___") + txtTableName.Text + "ListItem.bak";
            }
            else if (sourceFile.Contains("AppDbContext.cs"))
            {
                destinationFile = txtSelectFolder.Text + "\\BackupFiles\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss___") + "AppDbContext.cs.bak";
            }

            // Todo - might have an issue if sourceFile doesn't exist
            // To move a file to a new location 
            System.IO.File.Move(sourceFile, destinationFile);
            CreateFile(sourceFile);
        }

        public void CreateFile(string sourceFile)
        {
            string csContent = "";
            if (sourceFile.Contains("Models"))
            {
                csContent = GenerateCodeModel(txtTableName.Text, dataGridPropertyFields);
            }
            else if (sourceFile.Contains("Service.Core.cs"))
            {
                csContent = GenerateCodeCoreClass();
            }
            else if (sourceFile.Contains("Service.cs"))
            {
                if (!checkCore.Checked)
                {
                    if (!File.Exists(sourceFile))
                    {
                        // generate basic content for .cs file
                        csContent = GenerateCodeServiceClass();
                        StreamWriterCreate(sourceFile, csContent);
                    }
                }
            }
            else if (sourceFile.Contains("DataViews"))
            {
                csContent = GenerateCodeDataViewClass();
            }
            else if (sourceFile.Contains("AppDbContext.cs"))
            {
                csContent = AmmendAppDbContext();
            }

            // write content to file            
            StreamWriterCreate(sourceFile, csContent);
            MessageBox.Show("Successfully created file in " + sourceFile);
        }

        #region "GENERATED CODE METHODS"

        public string GenerateCodeModel(string filename, DataGridView dataGridView)
        {
            string codeStructure = "";
            string imports = "using System;\nusing System.Collections.Generic;\nusing System.Linq;\nusing System.Text;\nusing System.Threading.Tasks;\nusing System.ComponentModel.DataAnnotations;\n";
            string namespaceAndClass = $"\nnamespace Fitfactory.Models\n{{\n{tab}public class {char.ToUpper(filename[0]) + filename.Substring(1)} \n{tab}{{\n";

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
                    if ((row.Cells["Length"].Value != null) && !row.Cells["Length"].Value.Equals("") && row.Cells["Type"].Value.Equals("string?"))
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

        public string GenerateCodeCoreClass()
        {
            string codeStructure = "";
            codeStructure += $"using Fitfactory.DataViews;\nusing Fitfactory.Helpers;\nusing Fitfactory.Models;\nusing Microsoft.EntityFrameworkCore;\nusing System.Linq.Dynamic.Core;\n\nnamespace Fitfactory.Data\n{{\n{tab}public partial class {txtTableName.Text}Service\n{tab}{{\n{dtab}private readonly AppDbContext _appDbContext;";


            codeStructure += $"\n\n{dtab}public {txtTableName.Text}Service(AppDbContext appDbContext)\n{dtab}{{\n{tab}{dtab}_appDbContext = appDbContext; \n{dtab}}}";
            codeStructure += "\n\n        #region \"CRUD\"";
            codeStructure += $"\n\n{dtab}public {txtTableName.Text}? GetById(int Id)\n{dtab}{{\n{tab}{dtab}return _appDbContext.{txtPluralName.Text}.FirstOrDefault(c => c.Id == Id);\n{dtab}}}";
            codeStructure += $"\n\n{dtab}public List<{txtTableName.Text}ListItem> GetList(int pageIndex = 0, int pageSize = 0, string orderBy = \"\", string filterQuery = \"\")\n{dtab}{{\n{tab}{dtab}IQueryable<{txtTableName.Text}ListItem> dataList; \n{tab}{dtab}int count;\n{tab}{dtab}if (string.IsNullOrEmpty(orderBy) && string.IsNullOrEmpty(filterQuery))\n{tab}{dtab}{{\n{dtab}{dtab}dataList = _appDbContext.{txtPluralName.Text}List.AsQueryable();\n{dtab}{dtab}count = dataList.Count();\n{tab}{dtab}}}\n{tab}{dtab}else if (string.IsNullOrEmpty(orderBy) && !string.IsNullOrEmpty(filterQuery))\n{tab}{dtab}{{\n{dtab}{dtab}dataList = _appDbContext.{txtPluralName.Text}List.Where(filterQuery).AsQueryable();\n{dtab}{dtab}count = dataList.Count();\n{tab}{dtab}}}\n{tab}{dtab}else if (string.IsNullOrEmpty(filterQuery) && !string.IsNullOrEmpty(orderBy))\n{tab}{dtab}{{\n{dtab}{dtab}dataList = _appDbContext.{txtPluralName.Text}List.OrderBy(orderBy).AsQueryable();\n{dtab}{dtab}count = dataList.Count();\n{tab}{dtab}}}\n{tab}{dtab}else {{\n{dtab}{dtab}dataList = _appDbContext.{ txtPluralName.Text}List.OrderBy(orderBy).Where(filterQuery).AsQueryable();\n{dtab}{dtab}count = dataList.Count();\n{tab}{dtab}}}\n\n{tab}{dtab}if (pageSize > 0)\n{tab}{dtab}{{\n{dtab}{dtab}var pagedDataList = dataList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();\n{dtab}{dtab}return (new PaginatedList<{txtTableName.Text}ListItem> (pagedDataList, count, pageIndex, pageSize).ToList());\n{tab}{dtab}}}\n{tab}{dtab}else\n{tab}{dtab}{{\n{dtab}{dtab}return dataList.ToList();\n{dtab}{dtab}\n{tab}{dtab}}}\n{dtab}}}";
            codeStructure += $"\n\n{dtab}public Task<{txtTableName.Text}> Add({txtTableName.Text} dataObject)\n{dtab}{{\n{tab}{dtab}var addedObject = _appDbContext.{txtPluralName.Text}.Add(dataObject);\n{tab}{dtab}_appDbContext.SaveChanges();\n{tab}{dtab}return Task.FromResult(addedObject.Entity);\n{dtab}}}";
            codeStructure += $"\n\n{dtab}public bool Delete(int Id)\n{dtab}{{\n{tab}{dtab}var dataObject = _appDbContext.{txtPluralName.Text}.FirstOrDefault(e => e.Id == Id);\n{tab}{dtab}if (dataObject == null) return false;\n\n{tab}{dtab}_appDbContext.{ txtPluralName.Text}.Remove(dataObject);\n{tab}{dtab}_appDbContext.SaveChanges();\n{tab}{dtab}return true;\n{dtab}}}";


            codeStructure += $"\n\n{dtab}public {txtTableName.Text}? Update({txtTableName.Text} obj)\n{dtab}{{\n{tab}{dtab}var dataObject = _appDbContext.{txtPluralName.Text}.FirstOrDefault(e => e.Id == obj.Id);\n{tab}{dtab}if (dataObject != null)\n{tab}{dtab}{{";
            foreach (DataGridViewRow row in dataGridPropertyFields.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells["IsKey"].Value == null)
                {
                    codeStructure += $"\n{dtab}{dtab}dataObject.{row.Cells[0].Value} = obj.{row.Cells[0].Value};";
                }
            }

            codeStructure += $"\n\n{dtab}{dtab}_appDbContext.SaveChanges();\n{dtab}{dtab}return dataObject;\n{tab}{dtab}}}\n{tab}{dtab}return null;\n{dtab}}}";

            codeStructure += "\n        #endregion";

            codeStructure += "\n" + tab + "}" + "\n}";
            return codeStructure;
        }

        public string GenerateCodeServiceClass()
        {
            string codeStructure;
            codeStructure = $"using Fitfactory.DataViews;\nusing Fitfactory.Helpers;\nusing Fitfactory.Models;\nusing System.Linq.Dynamic.Core;\n\n";
            codeStructure += $"namespace Fitfactory.Data\n";
            codeStructure += $"{{\n";
            codeStructure += $"    public partial class {txtTableName.Text}Service\n";
            codeStructure += $"    {{\n\n";
            codeStructure += $"    }}\n";
            codeStructure += $"}}";

            return codeStructure;
        }

        public string GenerateCodeDataViewClass()
        {
            string codeStructure;
            codeStructure = $"namespace Fitfactory.DataViews\n";
            codeStructure += $"{{\n";
            codeStructure += $"    public class {txtTableName.Text}ListItem\n";
            codeStructure += $"    {{\n\n";
            codeStructure += $"    }}\n";
            codeStructure += $"}}";

            return codeStructure;
        }

        /// <summary>
        /// Code to ammend the AppDbContext with 
        /// </summary>
        /// <returns></returns>
        public string AmmendAppDbContext()
        {
            // get current context of AppDbContext.cs 
            string originalAppDbContextContentLocation = txtSelectFolder.Text + "\\BackupFiles\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss___") + "AppDbContext.cs.bak";
            string originalAppDbContextContent = File.ReadAllText(originalAppDbContextContentLocation);

            if (!originalAppDbContextContent.Contains($"public DbSet<{txtTableName.Text}> {txtPluralName.Text}"))
            {
                // then add changes to a SPECIFIC location
                string dbSetString = $"\n        public DbSet<{txtTableName.Text}> {txtPluralName.Text}  {{ get; set; }}";
                originalAppDbContextContent = originalAppDbContextContent.Insert(originalAppDbContextContent.IndexOf("@") + 1, dbSetString);

            }
            if (!originalAppDbContextContent.Contains($"public virtual DbSet<{txtTableName.Text}ListItem> {txtPluralName.Text}List"))
            {
                // then add changes to a SPECIFIC location;
                string virtualDbSetString = $"\n        public virtual DbSet<{txtTableName.Text}ListItem> {txtPluralName.Text}List  {{ get; set; }}";
                originalAppDbContextContent = originalAppDbContextContent.Insert(originalAppDbContextContent.IndexOf("#") + 1, virtualDbSetString);

            }
            if (!originalAppDbContextContent.Contains($"modelBuilder.Entity<{txtTableName.Text}ListItem>().ToTable"))
            {
                string modelBuilderString = $"\n            modelBuilder.Entity<{txtTableName.Text}ListItem>().ToTable(nameof({txtTableName.Text}ListItem), t => t.ExcludeFromMigrations());\n            modelBuilder.Entity<{txtTableName.Text}ListItem>(entity => {{ entity.ToTable(\"{txtPluralName.Text}List\"); }}); \n";
                originalAppDbContextContent = originalAppDbContextContent.Insert(originalAppDbContextContent.IndexOf("^") + 1, modelBuilderString);
            }
            return originalAppDbContextContent;
        }

        #endregion

        #region "GENERATE DATABASE"

        private void GeneratePosgresTable()
        {
            var connectionString = "Server=109.228.39.158;Port=5432;Database=Fitfactory;Persist Security Info=False;User ID=postgres;Password=postgres;";

            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;

            cmd.CommandText = GenerateTableSQL();
            cmd.ExecuteNonQuery();

            cmd.CommandText = GenerateTableSQL();
            cmd.ExecuteNonQuery();

            MessageBox.Show("Database Table Created!");
        }


        public string GenerateTableSQL()
        {
            string sqlStatement;
            sqlStatement = $"CREATE TABLE {txtTableName.Text}";
            sqlStatement += "(";

            foreach (DataGridViewRow row in dataGridPropertyFields.Rows)
            {
                if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "")
                {
                }
                else
                {
                    string required = "";
                    if ((row.Cells["Required"].Value != null) && (bool)row.Cells["Required"].Value)
                    {
                        required = " NOT NULL";
                    }
                    if ((row.Cells["IsKey"].Value != null) && (bool)row.Cells["IsKey"].Value == true)
                    {
                        sqlStatement += row.Cells["PropertyName"].Value + " SERIAL PRIMARY KEY, ";
                    }
                    else if (row.Cells["Type"].Value.Equals("string?"))
                    {
                        sqlStatement += row.Cells["PropertyName"].Value + $" VARCHAR({row.Cells["Length"].Value}) " + required + ",";
                    }
                    else if (row.Cells["Type"].Value.Equals("int"))
                    {
                        sqlStatement += row.Cells["PropertyName"].Value + $" INTEGER " + required + ",";
                    }
                    else if (row.Cells["Type"].Value.Equals("bool"))
                    {
                        sqlStatement += row.Cells["PropertyName"].Value + $" BOOLEAN " + required + ",";
                    }
                    else if (row.Cells["Type"].Value.Equals("decimal"))
                    {
                        sqlStatement += row.Cells["PropertyName"].Value + $" NUMERIC({row.Cells["Length"].Value}) " + required + ",";
                    }
                }
            }
            sqlStatement = sqlStatement.Remove(sqlStatement.Length - 1, 1);
            sqlStatement += ");";

            return sqlStatement;
        }

        public string GenerateViewSQL()
        {
            string sqlStatement;
            sqlStatement = $"CREATE VIEW {txtTableName.Text} AS ";
            sqlStatement += "SELECT ";

            foreach (DataGridViewRow row in dataGridPropertyFields.Rows)
            {
                sqlStatement += $"{txtPluralName.Text}" + "." + $"{row.Cells[0].Value}" + ",";
            }

            sqlStatement = sqlStatement.Remove(sqlStatement.Length - 1, 1);
            sqlStatement += $"FROM \"{txtPluralName.Text}\"";
            sqlStatement += ";";
            return sqlStatement;
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            //When the form is loaded, adds datatypes to the combobox in the datagrid
            DataGridViewComboBoxCell cmbbox = new DataGridViewComboBoxCell();
            cmbbox.Items.Add("string?");
            cmbbox.Items.Add("int");
            cmbbox.Items.Add("bool");
            cmbbox.Items.Add("decimal");
            cmbbox.Items.Add("float");
            ((DataGridViewComboBoxColumn)dataGridPropertyFields.Columns["Type"]).DataSource = cmbbox.Items;
        }

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

    }
}