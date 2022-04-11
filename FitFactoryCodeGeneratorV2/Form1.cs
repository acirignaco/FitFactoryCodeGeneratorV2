using Npgsql;
using System.ComponentModel;

namespace FitFactoryCodeGeneratorV2
{
    public partial class Form1 : Form
    {
        string tab = "    ";
        string dtab = "        ";
        string folderLocation = "";
        string txtTableNameToLowerFirstChar = "";
        List<string?> listOfDataTypes = new List<string?>();
        List<string?> listOfModelNames = new List<string?>();

        public string ToLowerFirstChar()
        {
            if (string.IsNullOrEmpty(txtTableName.Text))
                return txtTableName.Text;

            return char.ToLower(txtTableName.Text[0]) + txtTableName.Text.Substring(1);
        }


        public Form1()
        {
            InitializeComponent();
        }
        public bool FormValidation()
        {
            if (folderLocation == string.Empty)
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

        #region "BUTTON CLICKS"

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            bool successfulValidation = FormValidation();
            if (successfulValidation)
                btnGenerate_Click(sender, e, dataGridPropertyFields);
        }

        private void btnGenerate_Click(object sender, EventArgs e, DataGridView dataGridPropertyFields)
        {
            txtTableNameToLowerFirstChar = ToLowerFirstChar();
            //listOfDataTypes = GetAllDataTypes();
            //listOfModelNames = GetAllModelNames();

            string modelsPath = folderLocation + "\\Models\\" + txtTableName.Text + ".cs";
            string corePath = folderLocation + "\\Data\\" + txtTableName.Text + "Service.Core.cs";
            string servicePath = folderLocation + "\\Data\\" + txtTableName.Text + "Service.cs";
            string dataviewPath = folderLocation + "\\DataViews\\" + txtTableName.Text + "ListItem.cs";
            string appDbContextPath = folderLocation + "\\Data\\" + "AppDbContext.cs";
            string pagesPath = folderLocation + "\\Pages\\" + $"\\{txtPluralName.Text}\\" + txtPluralName.Text + "List.razor";
            string pagesAddPath = folderLocation + "\\Pages\\" + $"\\{txtPluralName.Text}\\" + txtTableName.Text + "Add.razor";
            string pagesEditPath = folderLocation + "\\Pages\\" + $"\\{txtPluralName.Text}\\" + txtTableName.Text + "Edit.razor";
            string programClass = folderLocation + "\\Program.cs";

            List<string> paths = new List<string>();
            paths.Add(modelsPath);
            paths.Add(corePath);
            if (!checkCore.Checked)
            {
                paths.Add(servicePath);
            }
            paths.Add(dataviewPath);
            paths.Add(appDbContextPath);
            paths.Add(pagesPath);
            paths.Add(pagesAddPath);
            paths.Add(pagesEditPath);
            paths.Add(programClass);    

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

            GeneratePosgresTableAndView();
            ClearFields();
        }

        #endregion

        #region "CREATE/OVERRIDE"

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
                destinationFile = folderLocation + "\\BackupFiles\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss___") + txtTableName.Text + ".bak";
            }
            else if (sourceFile.Contains("Service.Core.cs"))
            {
                destinationFile = folderLocation + "\\BackupFiles\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss___") + txtTableName.Text + "Service.Core.bak";
            }
            else if (sourceFile.Contains("Service.cs"))
            {
                destinationFile = folderLocation + "\\BackupFiles\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss___") + txtTableName.Text + "Service.bak";
            }
            else if (sourceFile.Contains("DataViews"))
            {
                destinationFile = folderLocation + "\\BackupFiles\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss___") + txtTableName.Text + "ListItem.bak";
            }
            else if (sourceFile.Contains("AppDbContext.cs"))
            {
                destinationFile = folderLocation + "\\BackupFiles\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss___") + "AppDbContext.cs.bak";
            }
            else if (sourceFile.Contains(".razor") && sourceFile.Contains("List"))
            {
                destinationFile = folderLocation + "\\BackupFiles\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss___") + txtTableName.Text + "List.bak";
            }
            else if (sourceFile.Contains(".razor") && sourceFile.Contains("Add"))
            {
                destinationFile = folderLocation + "\\BackupFiles\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss___") + txtTableName.Text + "Add.bak";
            }
            else if (sourceFile.Contains(".razor") && sourceFile.Contains("Edit"))
            {
                destinationFile = folderLocation + "\\BackupFiles\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss___") + txtTableName.Text + "Edit.bak";
            }
            else if (sourceFile.Contains("Program.cs"))
            {
                destinationFile = folderLocation + "\\BackUpFiles\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss___") + "Program.cs.bak";
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
            else if (sourceFile.Contains(".razor") && sourceFile.Contains("List"))
            {
                if (!Directory.Exists(folderLocation + "\\Pages\\" + $"\\{txtPluralName.Text}\\"))
                {
                    Directory.CreateDirectory(folderLocation + "\\Pages\\" + $"\\{txtPluralName.Text}\\");
                }
                csContent = GenerateListPage();
            }
            else if (sourceFile.Contains(".razor") && sourceFile.Contains("Add"))
            {
                csContent = GenerateAddPage();
            }
            else if (sourceFile.Contains(".razor") && sourceFile.Contains("Edit"))
            {
                csContent = GenerateEditPage();
            }
            else if (sourceFile.Contains("Program.cs"))
            {
                csContent = AmmendProgramClass();
            }

            // write content to file            
            StreamWriterCreate(sourceFile, csContent);
            MessageBox.Show("Successfully created file in " + sourceFile);
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

        #endregion

        #region "GENERATED CODE METHODS BACKEND"

        public string GenerateCodeModel(string filename, DataGridView dataGridView)
        {
            string codeStructure = "";
            string imports = "using System;\nusing System.Collections.Generic;\nusing System.Linq;\nusing System.Text;\nusing System.Threading.Tasks;\nusing System.ComponentModel.DataAnnotations;\n";
            string namespaceAndClass = $"\nnamespace Fitfactory.Models\n{{\n{tab}public class {char.ToUpper(filename[0]) + filename.Substring(1)} \n{tab}{{\n\n";

            codeStructure = imports + namespaceAndClass;

            codeStructure += $"        public {txtTableName.Text}()\n";
            codeStructure += $"        {{\n";
            foreach (DataGridViewRow row in dataGridPropertyFields.Rows)
            {
                if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "")
                {
                }
                else 
                {
                    if (row.Cells["Type"].Value.Equals("DateTime?"))
                    {
                        codeStructure += $"            {row.Cells["PropertyName"].Value} = DateTime.UtcNow;\n";
                    }
                }
            }
            codeStructure += $"        }}\n\n";


            foreach (DataGridViewRow row in dataGridPropertyFields.Rows)
            {
                if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "")
                {

                }
                else
                {
                    if (!row.Cells["Type"].Value.Equals("string?") && !row.Cells["Type"].Value.Equals("int") &&
                        !row.Cells["Type"].Value.Equals("bool") && !row.Cells["Type"].Value.Equals("Decimal") && 
                        !row.Cells["Type"].Value.Equals("DateTime?"))
                    {
                        if ((row.Cells["Required"].Value != null) && (bool)row.Cells["Required"].Value == true)
                        {
                            codeStructure += dtab + "[Required]\n";
                        }
                        codeStructure += dtab + "public ";
                        codeStructure += "int ";
                        codeStructure += row.Cells["PropertyName"].Value + "Id { get; set; } \n\n";
                    }

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
            List<string>? dropdownListValues = GetDropdownListValue();
            string codeStructure = "";
            codeStructure += $"using Fitfactory.DataViews;\nusing Fitfactory.Helpers;\nusing Fitfactory.Models;\nusing Microsoft.EntityFrameworkCore;\nusing System.Linq.Dynamic.Core;\n\nnamespace Fitfactory.Data\n{{\n{tab}public partial class {txtTableName.Text}Service\n{tab}{{\n{dtab}private readonly AppDbContext _appDbContext;";


            codeStructure += $"\n\n{dtab}public {txtTableName.Text}Service(AppDbContext appDbContext)\n{dtab}{{\n{tab}{dtab}_appDbContext = appDbContext; \n{dtab}}}";
            codeStructure += "\n\n        #region \"CRUD\"";

            codeStructure += $"\n\n{dtab}public Dictionary<int, {dropdownListValues[1]}> GetDropList()\n{dtab}{{\n{tab + dtab}return _appDbContext.{txtPluralName.Text}.OrderBy(a => a.Id).ToDictionary(c => c.Id, c => c.{dropdownListValues[0]});\n{dtab}}}";
            codeStructure += $"\n\n{dtab}public {txtTableName.Text}? GetById(int Id)\n{dtab}{{\n{tab}{dtab}return _appDbContext.{txtPluralName.Text}.FirstOrDefault(c => c.Id == Id);\n{dtab}}}";
            codeStructure += $"\n\n{dtab}public List<{txtTableName.Text}ListItem> GetList(int pageIndex = 0, int pageSize = 0, string orderBy = \"\", string filterQuery = \"\")\n{dtab}{{\n{tab}{dtab}IQueryable<{txtTableName.Text}ListItem> dataList; \n{tab}{dtab}int count;\n{tab}{dtab}if (string.IsNullOrEmpty(orderBy) && string.IsNullOrEmpty(filterQuery))\n{tab}{dtab}{{\n{dtab}{dtab}dataList = _appDbContext.{txtPluralName.Text}List.AsQueryable();\n{dtab}{dtab}count = dataList.Count();\n{tab}{dtab}}}\n{tab}{dtab}else if (string.IsNullOrEmpty(orderBy) && !string.IsNullOrEmpty(filterQuery))\n{tab}{dtab}{{\n{dtab}{dtab}dataList = _appDbContext.{txtPluralName.Text}List.Where(filterQuery).AsQueryable();\n{dtab}{dtab}count = dataList.Count();\n{tab}{dtab}}}\n{tab}{dtab}else if (string.IsNullOrEmpty(filterQuery) && !string.IsNullOrEmpty(orderBy))\n{tab}{dtab}{{\n{dtab}{dtab}dataList = _appDbContext.{txtPluralName.Text}List.OrderBy(orderBy).AsQueryable();\n{dtab}{dtab}count = dataList.Count();\n{tab}{dtab}}}\n{tab}{dtab}else {{\n{dtab}{dtab}dataList = _appDbContext.{ txtPluralName.Text}List.OrderBy(orderBy).Where(filterQuery).AsQueryable();\n{dtab}{dtab}count = dataList.Count();\n{tab}{dtab}}}\n\n{tab}{dtab}if (pageSize > 0)\n{tab}{dtab}{{\n{dtab}{dtab}var pagedDataList = dataList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();\n{dtab}{dtab}return (new PaginatedList<{txtTableName.Text}ListItem> (pagedDataList, count, pageIndex, pageSize).ToList());\n{tab}{dtab}}}\n{tab}{dtab}else\n{tab}{dtab}{{\n{dtab}{dtab}return dataList.ToList();\n{dtab}{dtab}\n{tab}{dtab}}}\n{dtab}}}";
            codeStructure += $"\n\n{dtab}public {txtTableName.Text} Add({txtTableName.Text} dataObject)\n{dtab}{{\n{tab}{dtab}var addedObject = _appDbContext.{txtPluralName.Text}.Add(dataObject);\n{tab}{dtab}_appDbContext.SaveChanges();\n{tab}{dtab}return addedObject.Entity;\n{dtab}}}";
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
            codeStructure = "using System.ComponentModel.DataAnnotations;\n";
            codeStructure += $"namespace Fitfactory.DataViews\n";
            codeStructure += $"{{\n";
            codeStructure += $"    public class {txtTableName.Text}ListItem\n";
            codeStructure += $"    {{\n\n";

            foreach (DataGridViewRow row in dataGridPropertyFields.Rows)
            {
                if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "")
                {

                }
                else
                {
                    if (row.Cells["Type"].Value.Equals("string?") || row.Cells["Type"].Value.Equals("int") ||
                        row.Cells["Type"].Value.Equals("bool") || row.Cells["Type"].Value.Equals("Decimal") || 
                        row.Cells["Type"].Value.Equals("DateTime?"))
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
            }


            codeStructure += $"    }}\n";
            codeStructure += $"}}";

            return codeStructure;
        }

        public string AmmendAppDbContext()
        {
            // get current context of AppDbContext.cs 
            string originalAppDbContextContentLocation = folderLocation + "\\BackupFiles\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss___") + "AppDbContext.cs.bak";
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

        public string AmmendProgramClass()
        {
            string originalProgramClassLocation = folderLocation + "\\BackupFiles\\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss___") + "Program.cs.bak";
            string newProgramClass = File.ReadAllText(originalProgramClassLocation);

            if (!newProgramClass.Contains($"builder.Services.AddScoped<{txtTableName.Text}Service>();"))
            {
                string serviceScopeString = $"\nbuilder.Services.AddScoped<{txtTableName.Text}Service>();"; 
                newProgramClass = newProgramClass.Insert(newProgramClass.IndexOf("%") + 1, serviceScopeString);
            }
            return newProgramClass;
        }


        public List<string> GetDropdownListValue()
        {
            List<string>? dropdownListValue = new List<string>();
            foreach (DataGridViewRow row in dataGridPropertyFields.Rows)
            {
                if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "")
                { }
                else
                {
                    if ((row.Cells["DropdownIdentifier"].Value != null) && (bool)row.Cells["DropdownIdentifier"].Value == true)
                    {
                        dropdownListValue.Add(row.Cells["PropertyName"].Value.ToString());
                        dropdownListValue.Add(row.Cells["Type"].Value.ToString());
                    }
                }
            }

            return dropdownListValue;
        }

        #endregion

        #region "CODE METHODS FRONTEND"

        public string GenerateListPage()
        {
            string codeStructure = $"@page \"/{txtPluralName.Text}List\"\n\n" +
            $"<PageTitle>{txtPluralName.Text} - Fitfactory ERP</PageTitle>\n\n" +
            $"@using Fitfactory.Data\n@using Fitfactory.DataViews\n" +
            $"@using Fitfactory.Models\n@using Syncfusion.Blazor.Grids\n\n" +
            $"@inject IToastService ToastService\n" +
            $"@inject {txtTableName.Text}Service {txtTableName.Text}Service\n" +
            $"@inject NavigationManager NavigationManager\n\n" +
            $"@attribute [Authorize]\n\n" +
            $"<br/>\n" +
            $"<div class=\"card\">\n" +
            $"    <h3 class=\"card-header\">{txtPluralName.Text}</h3>\n" +
            $"    <div class=\"card-body\">\n" +
            $"        @if (Loaded == false)\n" +
            $"        {{\n" +
            $"            <div class=\"d-flex justify-content-center\">\n" +
            $"                <div class=\"spinner-border\" role=\"status\">\n" +
            $"                    <span class=\"visually-hidden\">Loading...</span>\n" +
            $"                </div>\n" +
            $"            </div>\n" +
            $"        }}\n" +
            $"        else\n" +
            $"        {{\n" +

            $"            <button @onclick=\"@(() => ShowAdd{txtTableName.Text}())\" class=\"btn btn-success\">Add {txtTableName.Text}</button>\n" +
            $"            <button @onclick=\"@(() => ShowEdit{txtTableName.Text}())\" class=\"btn btn-success\">Edit {txtTableName.Text}</button>\n" +
            $"            @*<button @onclick=\"@(() => PrintReport())\" class=\"btn btn-primary\">Print Selected</button>*@\n" +
            $"            <div class=\"container-fluid\">\n" +
            $"                <br />\n" +
            $"                <SfGrid @ref=\"grid\" DataSource=\"@gridData\" RowHeight=\"38\" AllowSorting=\"true\" AllowFiltering=\"true\" AllowPaging=\"true\" AllowGrouping=\"true\" EnableHover=\"true\" AllowSelection=\"true\" AllowResizing=\"true\" AllowExcelExport=\"true\" AllowPdfExport=\"true\" ContextMenuItems=\"@(new List<object>() {{ \"ExcelExport\", \"CsvExport\" }})\" ShowColumnChooser=\"true\" AllowReordering=\"true\" Toolbar=\"@(new List<string>() {{ \"ColumnChooser\" }})\">\n" +
            $"                <GridFilterSettings Type=\"Syncfusion.Blazor.Grids.FilterType.Menu\"></GridFilterSettings>\n" +
            $"                <GridPageSettings PageSize=\"30\"></GridPageSettings>\n" +
            $"                <GridColumns>\n";



            foreach (DataGridViewRow row in dataGridPropertyFields.Rows)
            {

                if (row.Cells["PropertyName"].Value != null)
                {
                    if (row.Cells["Type"].Value.Equals("string?") || row.Cells["Type"].Value.Equals("int") ||
                            row.Cells["Type"].Value.Equals("bool") || row.Cells["Type"].Value.Equals("Decimal") || 
                            row.Cells["Type"].Value.Equals("DateTime?"))
                    {

                        string primaryKeyString = "";
                        string booleanString = "";
                        if ((row.Cells["IsKey"].Value != null) && (bool)row.Cells["IsKey"].Value == true)
                        {
                            primaryKeyString = $"IsPrimaryKey =\"true\" TextAlign=\"TextAlign.Center\"";
                        } 
                        if (row.Cells["Type"].Value.Equals("bool"))
                        {
                            booleanString = $"DisplayAsCheckBox=\"true\" DefaultValue=\"false\" TextAlign=\"TextAlign.Center\"";
                        }
                        codeStructure += $"                    <GridColumn Field=@nameof({txtTableName.Text}ListItem.{row.Cells["PropertyName"].Value}) HeaderText=\"{row.Cells["PropertyName"].Value}\" {primaryKeyString} {booleanString} Width=\"150\"></GridColumn>\n";
                    }
                }
            }


            codeStructure += $"                </GridColumns>\n" +
            $"                </SfGrid>\n" +
            $"            </div>\n" +
            $"        }}\n" +
            $"    </div>\n" +
            $"</div>\n\n" +
            $"<{txtTableName.Text}Add @ref=\"AddDialog\"></{txtTableName.Text}Add>\n\n" +
            $"<{txtTableName.Text}Edit @ref=\"EditDialog\"></{txtTableName.Text}Edit>\n\n" +

            $"@code {{\n" +
            $"    private List<{txtTableName.Text}ListItem> gridData;\n" +
            $"    SfGrid<{txtTableName.Text}ListItem> grid {{ get; set; }}\n" +
            $"    private bool Loaded;\n" +
            $"    {txtTableName.Text}Add AddDialog;\n" +
            $"    {txtTableName.Text}Edit EditDialog;\n\n" +
            $"    protected override async Task OnInitializedAsync()\n" +
            $"    {{\n" +
            $"        await Task.Run(LoadData);\n" +
            $"    }}\n\n" +
            $"    private void LoadData()\n" +
            $"    {{\n" +
            $"        gridData = {txtTableName.Text}Service.GetList();\n" +
            $"        Loaded = true;\n" +
            $"    }}\n\n" +
            $"    void ShowEdit{txtTableName.Text}()\n" +
            $"    {{\n" +
            $"        if (grid.SelectedRecords != null && grid.SelectedRecords.Count > 0)\n" +
            $"        {{\n" +
            $"            var selectedRow = this.grid.GetSelectedRecordsAsync();\n" +
            $"            int {txtTableNameToLowerFirstChar}Id = selectedRow.Result.AsEnumerable().First().Id;\n" +
            $"            EditDialog.OpenDialog({txtTableNameToLowerFirstChar}Id);\n" +
            $"        }}\n" +
            $"        else\n" +
            $"        {{\n" +
            $"            ToastService.ShowError(\"Please select a row before editing!\");\n" +
            $"        }}\n" +
            $"    }}\n\n" +
            $"    void ShowAdd{txtTableName.Text}()\n" +
            $"    {{\n" +
            $"        AddDialog.OpenDialog();\n" +
            $"    }}\n" +
            $"}}\n";

            return codeStructure;

            //foreach (DataGridViewRow row in dataGridPropertyFields.Rows)
            //{

            //    if (row.Cells["PropertyName"].Value != null)
            //    {
            //        if (!row.Cells["Type"].Value.Equals("string?") && !row.Cells["Type"].Value.Equals("int") &&
            //                !row.Cells["Type"].Value.Equals("bool") && !row.Cells["Type"].Value.Equals("Decimal") &&
            //                !row.Cells["Type"].Value.Equals("DateTime?"))
            //        {
            //            codeStructure += $"        parameters.Add(\"{txtTableNameToLowerFirstChar + "s"}\", {row.Cells["Type"].Value.ToString().Remove(row.Cells["Type"].Value.ToString().Length - 1, 1)}Service.GetList());\n";
            //        }
            //    }
            //}


        }

        public string GenerateAddPage()
        {
            string codeStructure = $"@page \"/{txtTableName.Text}Add\"\n\n" +
            $"@using Fitfactory.Data\n@using Fitfactory.DataViews\n" +
            $"@using Fitfactory.Models\n@using Syncfusion.Blazor.Popups\n\n" +
            $"@inject {txtTableName.Text}Service {txtTableName.Text}Service\n" +
            $"@inject IToastService ToastService\n" +
            $"@inject NavigationManager NavigationManager\n\n" +
            $"@attribute [Authorize]\n\n" +

            $"<div id=\"DialogTarget\">\n" +
            $"    <SfDialog Target=\"#DialogTarget\" Width=\"335px\" AllowDragging=\"true\" IsModal=\"true\" @bind-Visible=\"@IsOpen\">\n" +
            $"        <DialogTemplates>\n" +
            $"            <Header>Add {txtTableName.Text}</Header>\n" +
            $"            <Content>\n" +
            $"                <EditForm Model=\"{txtTableNameToLowerFirstChar}\">\n" +
            $"                    <DataAnnotationsValidator />" + 
            $"                    \n\n\n\n\n";

            foreach (DataGridViewRow row in dataGridPropertyFields.Rows)
            {
                if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "")
                {
                }
                else
                {
       
                    if (row.Cells["Type"].Value.Equals("string?"))
                    {
                        codeStructure += $"                    <label for=\"{row.Cells["PropertyName"].Value}\" class=\"form-label mt-4\">{row.Cells["PropertyName"].Value}</label>\n";
                        codeStructure += $"                    <InputText type=\"int\" class=\"form-control\" @bind-Value=\"@{txtTableNameToLowerFirstChar}.{row.Cells["PropertyName"].Value}\" placeholder=\"Enter {row.Cells["PropertyName"].Value}\" ></InputText>\n\n";
                    }
                    else if (row.Cells["Type"].Value.Equals("int") && (bool)row.Cells["IsKey"].Value == false)
                    {
                        codeStructure += $"                    <label for=\"{row.Cells["PropertyName"].Value}\" class=\"form-label mt-4\">{row.Cells["PropertyName"].Value}</label>\n";
                        codeStructure += $"                    <InputNumber type=\"text\" class=\"form-control\" @bind-Value=\"@{txtTableNameToLowerFirstChar}.{row.Cells["PropertyName"].Value}\" placeholder=\"Enter {row.Cells["PropertyName"].Value}\" ></InputNumber>\n";
                    }
                    else if (row.Cells["Type"].Value.Equals("bool"))
                    {
                        codeStructure += $"                    <br />\n";
                        codeStructure += $"                    <input class=\"form-check-input\" type=\"checkbox\" value=\"true\" id=\"flexCheckDefault\" @bind=\"@{txtTableNameToLowerFirstChar}.{ row.Cells["PropertyName"].Value}\">\n";
                        codeStructure += $"                    <label class=\"form-check-label\" for=\"flexCheckDefault\">{row.Cells["PropertyName"].Value}</label>\n\n";
                    }
                    else if (row.Cells["Type"].Value.Equals("Decimal"))
                    {
                        codeStructure += $"                    <label for=\"{row.Cells["PropertyName"].Value}\" class=\"form-label mt-4\">{row.Cells["PropertyName"].Value}</label>\n";
                        codeStructure += $"                    <InputNumber type=\"text\" class=\"form-control\" @bind-Value=\"@{txtTableNameToLowerFirstChar}.{row.Cells["PropertyName"].Value}\" placeholder=\"Enter {row.Cells["PropertyName"].Value}\" ></InputNumber>\n";
                    }
                    else if (row.Cells["Type"].Value.Equals("DateTime?"))
                    {
                        codeStructure += $"                    <label for=\"{row.Cells["PropertyName"].Value}\" class=\"form-label mt-4\">{row.Cells["PropertyName"].Value}</label>\n";
                        codeStructure += $"                    <InputDate type=\"date\" class=\"form-control\" @bind-Value=\"@{txtTableNameToLowerFirstChar}.{row.Cells["PropertyName"].Value}\" placeholder=\"\" ></InputDate>\n";
                    }

                }
            }

            codeStructure += $"                </EditForm>\n" + 
            $"            </Content>\n" +
            $"        </DialogTemplates>\n" +
            $"        <DialogPositionData X=\"center\" Y=\"top\" />\n" +
            $"        <DialogButtons>\n" +
            $"            <DialogButton Content=\"Add\" IsPrimary=\"true\" OnClick=\"@Create{txtTableName.Text}\" />\n" +
            $"            <DialogButton Content=\"Cancel\" IsPrimary=\"false\" OnClick=\"@CancelClick\" />\n" +
            $"        </DialogButtons>\n" +
            $"        <DialogAnimationSettings Effect=\"@DialogEffect.Zoom\"></DialogAnimationSettings>\n" +
            $"    </SfDialog>\n" +
            $"</div>\n\n" +

            $"@code {{\n" +
            $"    {txtTableName.Text} {txtTableNameToLowerFirstChar} = new {txtTableName.Text}();\n" +
            $"    bool IsOpen {{ get; set; }} = false;\n\n" +
            $"    protected void Create{txtTableName.Text}()\n" +
            $"    {{\n" +
            $"        {txtTableName.Text}Service.Add({txtTableNameToLowerFirstChar});\n" +
            $"        ToastService.ShowSuccess($\"The new {txtTableNameToLowerFirstChar})\", \"Successfully Added\" );\n" +
            $"        IsOpen = false;\n" +
            $"        this.StateHasChanged();\n" +
            $"    }}\n\n" +
            $"    public void OpenDialog()\n" +
            $"    {{\n" +
            $"        IsOpen = true;\n" +
            $"        this.StateHasChanged();\n" +
            $"    }}\n\n" +
            $"    private void CancelClick()\n" +
            $"    {{\n" +
            $"        IsOpen = false;\n" +
            $"        this.StateHasChanged();\n" +
            $"    }}\n" +
            $"}}\n\n";

            return codeStructure;
        }

        public string GenerateEditPage()
        {
            string codeStructure = $"@page \"/{txtTableName.Text}Edit\"\n\n" +
            $"@using Fitfactory.Data\n@using Fitfactory.DataViews\n" +
            $"@using Fitfactory.Models\n@using Syncfusion.Blazor.Popups\n\n" +
            $"@inject {txtTableName.Text}Service {txtTableName.Text}Service\n" +
            $"@inject IToastService ToastService\n" +
            $"@attribute [Authorize]\n\n" +

            $"<div id=\"DialogTarget\">\n" +
            $"    <SfDialog Target=\"#DialogTarget\" Width=\"335px\" AllowDragging=\"true\" IsModal=\"true\" @bind-Visible=\"@IsOpen\">\n" +
            $"        <DialogTemplates>\n" +
            $"            <Header>Edit {txtTableName.Text}</Header>\n" +
            $"            <Content>\n" +
            $"                <EditForm Model=\"{txtTableNameToLowerFirstChar}\">\n" +
            $"                    <DataAnnotationsValidator />" +
            $"                    \n\n";

            foreach (DataGridViewRow row in dataGridPropertyFields.Rows)
            {
                if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "")
                {
                }
                else
                {
                    string primaryKeyString = "";
                    if((row.Cells["IsKey"].Value != null) && (bool)row.Cells["IsKey"].Value == true)
                    {
                        primaryKeyString = "disabled = \"disabled\"";
                    }

                    if (row.Cells["Type"].Value.Equals("string?"))
                    {
                        codeStructure += $"                    <label for=\"{row.Cells["PropertyName"].Value}\" class=\"form-label mt-4\">{row.Cells["PropertyName"].Value}</label>\n";
                        codeStructure += $"                    <InputText type=\"int\" class=\"form-control\" @bind-Value=\"@{txtTableNameToLowerFirstChar}.{row.Cells["PropertyName"].Value}\" placeholder=\"Enter {row.Cells["PropertyName"].Value}\" ></InputText>\n\n";
                    }
                    else if (row.Cells["Type"].Value.Equals("int"))
                    {
                        codeStructure += $"                    <label for=\"{row.Cells["PropertyName"].Value}\" class=\"form-label mt-4\">{row.Cells["PropertyName"].Value}</label>\n";
                        codeStructure += $"                    <InputNumber type=\"text\" class=\"form-control\" {primaryKeyString} @bind-Value=\"@{txtTableNameToLowerFirstChar}.{row.Cells["PropertyName"].Value}\" placeholder=\"Enter {row.Cells["PropertyName"].Value}\" ></InputNumber>\n";
                    }
                    else if (row.Cells["Type"].Value.Equals("bool"))
                    {
                        codeStructure += $"                    <br />\n";
                        codeStructure += $"                    <input class=\"form-check-input\" type=\"checkbox\" value=\"true\" id=\"flexCheckDefault\" @bind=\"@{txtTableNameToLowerFirstChar}.{ row.Cells["PropertyName"].Value}\">\n";
                        codeStructure += $"                    <label class=\"form-check-label\" for=\"flexCheckDefault\">Default {txtTableName.Text}</label>\n\n";
                    }
                    else if (row.Cells["Type"].Value.Equals("DateTime?"))
                    {
                        codeStructure += $"                    <label for=\"{row.Cells["PropertyName"].Value}\" class=\"form-label mt-4\">{row.Cells["PropertyName"].Value}</label>\n";
                        codeStructure += $"                    <InputDate type=\"date\" class=\"form-control\" @bind-Value=\"@{txtTableNameToLowerFirstChar}.{row.Cells["PropertyName"].Value}\" placeholder=\"\" ></InputDate>\n";
                    }

                }
            }


            codeStructure += $"                </EditForm>\n" +
            $"            </Content>\n" +
            $"        </DialogTemplates>\n" +
            $"        <DialogPositionData X=\"center\" Y=\"top\" />\n" +
            $"        <DialogButtons>\n" +
            $"            <DialogButton Content=\"Edit\" IsPrimary=\"true\" OnClick=\"@Edit{txtTableName.Text}\" />\n" +
            $"            <DialogButton Content=\"Cancel\" IsPrimary=\"false\" OnClick=\"@CancelClick\" />\n" +
            $"        </DialogButtons>\n" +
            $"        <DialogAnimationSettings Effect=\"@DialogEffect.Zoom\"></DialogAnimationSettings>\n" +
            $"    </SfDialog>\n" +
            $"</div>\n\n" +

            $"@code {{\n" +
            $"    public int {txtTableNameToLowerFirstChar}Id {{ get; set; }}\n" +
            $"    {txtTableName.Text}? {txtTableNameToLowerFirstChar} = new {txtTableName.Text}();\n" +
            $"    bool IsOpen {{ get; set; }} = false;\n\n" +
            $"    protected void Edit{txtTableName.Text}()\n" +
            $"    {{\n" +
            $"        {txtTableName.Text}Service.Update({txtTableNameToLowerFirstChar});\n" +
            $"        ToastService.ShowSuccess($\"The new {txtTableName.Text}\", \"Successfully Edited\" );\n" +
            $"        IsOpen = false;\n" +
            $"        this.StateHasChanged();\n" +
            $"    }}\n\n" +
            $"    public void OpenDialog(int {txtTableNameToLowerFirstChar}Id)\n" +
            $"    {{\n" +
            $"        if ({txtTableNameToLowerFirstChar}Id > 0)\n" +
            $"        {{\n" +
            $"            {txtTableNameToLowerFirstChar} = {txtTableName.Text}Service.GetById({txtTableNameToLowerFirstChar}Id);\n" +
            $"        }}\n" +
            $"        IsOpen = true;\n" +
            $"        this.StateHasChanged();\n" +
            $"    }}\n\n" +
            $"    private void CancelClick()\n" +
            $"    {{\n" +
            $"        IsOpen = false;\n" +
            $"        this.StateHasChanged();\n" +
            $"    }}\n" +
            $"}}\n\n";

            return codeStructure;
        }

        #endregion

        #region "GENERATE DATABASE"

        private void GeneratePosgresTableAndView()
        {
            var connectionString = "Server=109.228.39.158;Port=5432;Database=Fitfactory;Persist Security Info=False;User ID=postgres;Password=postgres;";

            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            
            cmd.CommandText = GenerateTableSQL();
            cmd.ExecuteNonQuery();
            MessageBox.Show("Database Table Created! If already existed then it didnt override!");

            try 
            {
                cmd.CommandText = GenerateViewSQL();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Database View Created!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Creating View!! Error message: {ex.Message}");
            }

        }

        public string GenerateTableSQL()
        {
            string sqlStatement;
            sqlStatement = $"CREATE TABLE IF NOT EXISTS \"{txtPluralName.Text}\"";
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
                        sqlStatement += "\"" + row.Cells["PropertyName"].Value + "\"" + " SERIAL PRIMARY KEY,";
                    }
                    else if (row.Cells["Type"].Value.Equals("string?"))
                    {
                        sqlStatement += "\"" + row.Cells["PropertyName"].Value + "\"" + $" VARCHAR({row.Cells["Length"].Value}) " + required + ",";
                    }
                    else if (row.Cells["Type"].Value.Equals("int"))
                    {
                        sqlStatement += "\"" + row.Cells["PropertyName"].Value + "\"" + $" INTEGER " + required + ",";
                    }
                    else if (row.Cells["Type"].Value.Equals("bool"))
                    {
                        sqlStatement += "\"" + row.Cells["PropertyName"].Value + "\"" + $" BOOLEAN " + required + ",";
                    }
                    else if (row.Cells["Type"].Value.Equals("Decimal"))
                    {
                        sqlStatement += "\"" + row.Cells["PropertyName"].Value + "\"" + $" NUMERIC({row.Cells["Length"].Value}) " + required + ",";
                    }
                    else if (row.Cells["Type"].Value.Equals("DateTime?"))
                    {
                        sqlStatement += "\"" + row.Cells["PropertyName"].Value + "\"" + $" TIMESTAMPTZ " + required + ",";
                    }

                    if (!row.Cells["Type"].Value.Equals("string?") && !row.Cells["Type"].Value.Equals("int") && !row.Cells["Type"].Value.Equals("DateTime?") &&
                    !row.Cells["Type"].Value.Equals("bool") && !row.Cells["Type"].Value.Equals("Decimal"))
                    {
                        sqlStatement += "\"" + row.Cells["PropertyName"].Value + "Id\"" + $" INTEGER " + required + ",";
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
            sqlStatement = $"CREATE VIEW \"{txtPluralName.Text}List\" AS ";
            sqlStatement += "SELECT ";
            
            foreach (DataGridViewRow row in dataGridPropertyFields.Rows)
            {
                if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "")
                {
                }
                else
                {
                    if (row.Cells["Type"].Value.Equals("string?") || row.Cells["Type"].Value.Equals("int") || row.Cells["Type"].Value.Equals("DateTime?") ||
                       row.Cells["Type"].Value.Equals("bool") || row.Cells["Type"].Value.Equals("Decimal"))
                    {
                        sqlStatement += $"\"{txtPluralName.Text}\"" + "." + $"\"{row.Cells[0].Value}\"" + ",";

                    }
                }
            }

            sqlStatement = sqlStatement.Remove(sqlStatement.Length - 1, 1);
            sqlStatement += $" FROM \"{txtPluralName.Text}\";";
            return sqlStatement;
        }

        #endregion

        #region "MISC"

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
            //txtSelectFolder.Clear();
            txtTableName.Clear();
            txtPluralName.Clear();
            checkCore.Checked = false;
            dataGridPropertyFields.Rows.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            folderLocation = Form2.SetValueForFolderLocation;

            //When the form is loaded, adds datatypes to the combobox in the datagrid
            DataGridViewComboBoxCell cmbbox = new DataGridViewComboBoxCell();
            cmbbox.Items.Add("string?");
            cmbbox.Items.Add("int");
            cmbbox.Items.Add("bool");
            cmbbox.Items.Add("Decimal");
            cmbbox.Items.Add("DateTime?");

            foreach (var name in GetAllModelNamesFromFolder())
            {
                cmbbox.Items.Add(name);
            }

            ((DataGridViewComboBoxColumn)dataGridPropertyFields.Columns["Type"]).DataSource = cmbbox.Items;
        }

        public List<string?> GetAllModelNamesFromFolder()
        {
            List<string?> modelNames = new List<string?>();

            //string directory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Models\\";
            DirectoryInfo di = new DirectoryInfo($"{folderLocation}\\Models\\");
            FileInfo[] files  = di.GetFiles("*");

            foreach (var file in files)
            {
                modelNames.Add(file.Name.Substring(0, file.Name.Length - 3) + "?");
            }

            return modelNames;
        }

        public List<string?> GetAllDataTypes()
        {
            List<string?> dataTypesList = new List<string?>();
            foreach (DataGridViewRow row in dataGridPropertyFields.Rows)
            {

                if (row.Cells["PropertyName"].Value != null)
                {
                    if (row.Cells["Type"].Value.Equals("string?") || row.Cells["Type"].Value.Equals("int") ||
                            row.Cells["Type"].Value.Equals("bool") || row.Cells["Type"].Value.Equals("Decimal") ||
                            row.Cells["Type"].Value.Equals("DateTime?"))
                    {
                        dataTypesList.Add(row.Cells["Type"].Value.ToString());
                    }
                }
            }

            return dataTypesList;
        }

        public List<string?> GetAllModelNames()
        {
            List<string?> modelNamesList = new List<string?>();
            foreach (DataGridViewRow row in dataGridPropertyFields.Rows)
            {

                if (row.Cells["PropertyName"].Value != null)
                {
                    if (!row.Cells["Type"].Value.Equals("string?") && !row.Cells["Type"].Value.Equals("int") &&
                            !row.Cells["Type"].Value.Equals("bool") && !row.Cells["Type"].Value.Equals("Decimal") &&
                            !row.Cells["Type"].Value.Equals("DateTime?"))
                    {
                        modelNamesList.Add(row.Cells["Type"].Value.ToString());
                    }
                }
            }

            return modelNamesList;
        }

        #endregion

    }
}