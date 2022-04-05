namespace FitFactoryCodeGeneratorV2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnGenerate = new System.Windows.Forms.Button();
            this.dataGridPropertyFields = new System.Windows.Forms.DataGridView();
            this.PropertyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DropdownIdentifier = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Required = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsKey = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnClear = new System.Windows.Forms.Button();
            this.checkCore = new System.Windows.Forms.CheckBox();
            this.txtPluralName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTableName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPropertyFields)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            this.btnGenerate.BackColor = System.Drawing.Color.White;
            this.btnGenerate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnGenerate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerate.FlatAppearance.BorderColor = System.Drawing.Color.MediumSeaGreen;
            this.btnGenerate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnGenerate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnGenerate.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnGenerate.Location = new System.Drawing.Point(303, 378);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(145, 37);
            this.btnGenerate.TabIndex = 7;
            this.btnGenerate.Text = "Generate Code";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // dataGridPropertyFields
            // 
            this.dataGridPropertyFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridPropertyFields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PropertyName,
            this.Type,
            this.Length,
            this.DropdownIdentifier,
            this.Required,
            this.IsKey});
            this.dataGridPropertyFields.Location = new System.Drawing.Point(20, 222);
            this.dataGridPropertyFields.Name = "dataGridPropertyFields";
            this.dataGridPropertyFields.RowHeadersWidth = 51;
            this.dataGridPropertyFields.RowTemplate.Height = 25;
            this.dataGridPropertyFields.Size = new System.Drawing.Size(545, 150);
            this.dataGridPropertyFields.TabIndex = 6;
            // 
            // PropertyName
            // 
            this.PropertyName.HeaderText = "Property Name";
            this.PropertyName.MinimumWidth = 6;
            this.PropertyName.Name = "PropertyName";
            this.PropertyName.Width = 125;
            // 
            // Type
            // 
            this.Type.DataPropertyName = "DataType";
            this.Type.HeaderText = "Data Type";
            this.Type.MinimumWidth = 6;
            this.Type.Name = "Type";
            this.Type.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Type.Width = 115;
            // 
            // Length
            // 
            this.Length.HeaderText = "Length";
            this.Length.MinimumWidth = 6;
            this.Length.Name = "Length";
            this.Length.Width = 110;
            // 
            // DropdownIdentifier
            // 
            this.DropdownIdentifier.HeaderText = "Dropdown Identifier";
            this.DropdownIdentifier.Name = "DropdownIdentifier";
            // 
            // Required
            // 
            this.Required.HeaderText = "Required?";
            this.Required.MinimumWidth = 6;
            this.Required.Name = "Required";
            this.Required.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Required.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Required.Width = 110;
            // 
            // IsKey
            // 
            this.IsKey.HeaderText = "IsKey?";
            this.IsKey.MinimumWidth = 6;
            this.IsKey.Name = "IsKey";
            this.IsKey.Width = 110;
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.White;
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnClear.Location = new System.Drawing.Point(451, 378);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(115, 37);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // checkCore
            // 
            this.checkCore.AutoSize = true;
            this.checkCore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkCore.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.checkCore.Location = new System.Drawing.Point(177, 176);
            this.checkCore.Name = "checkCore";
            this.checkCore.Size = new System.Drawing.Size(15, 14);
            this.checkCore.TabIndex = 5;
            this.checkCore.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkCore.UseVisualStyleBackColor = true;
            // 
            // txtPluralName
            // 
            this.txtPluralName.Location = new System.Drawing.Point(20, 137);
            this.txtPluralName.Name = "txtPluralName";
            this.txtPluralName.Size = new System.Drawing.Size(546, 23);
            this.txtPluralName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(20, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Plural";
            // 
            // txtTableName
            // 
            this.txtTableName.Location = new System.Drawing.Point(20, 86);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size(546, 23);
            this.txtTableName.TabIndex = 3;
            this.txtTableName.TextChanged += new System.EventHandler(this.txtTableName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(20, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Table Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(20, 204);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Enter Data Properties";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(20, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Create Core Class Only?";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 426);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTableName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPluralName);
            this.Controls.Add(this.checkCore);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.dataGridPropertyFields);
            this.Controls.Add(this.btnGenerate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FitFactory Code Generator";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPropertyFields)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnGenerate;
        private DataGridView dataGridPropertyFields;
        private Button btnClear;
        private CheckBox checkCore;
        private TextBox txtPluralName;
        private Label label2;
        private TextBox txtTableName;
        private Label label3;
        private Label label4;
        private FolderBrowserDialog folderBrowserDialog1;
        private Label label5;
        private DataGridViewTextBoxColumn PropertyName;
        private DataGridViewComboBoxColumn Type;
        private DataGridViewTextBoxColumn Length;
        private DataGridViewCheckBoxColumn DropdownIdentifier;
        private DataGridViewCheckBoxColumn Required;
        private DataGridViewCheckBoxColumn IsKey;
    }
}