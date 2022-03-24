﻿namespace FitFactoryCodeGeneratorV2
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
            this.btnGenerate = new System.Windows.Forms.Button();
            this.dataGridPropertyFields = new System.Windows.Forms.DataGridView();
            this.PropertyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Required = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsKey = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnClear = new System.Windows.Forms.Button();
            this.checkCore = new System.Windows.Forms.CheckBox();
            this.txtSelectFolder = new System.Windows.Forms.TextBox();
            this.txtPluralName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTableName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPropertyFields)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            this.btnGenerate.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnGenerate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnGenerate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerate.FlatAppearance.BorderColor = System.Drawing.Color.MediumSeaGreen;
            this.btnGenerate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnGenerate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnGenerate.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnGenerate.Location = new System.Drawing.Point(301, 416);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(145, 46);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Generate Code";
            this.btnGenerate.UseVisualStyleBackColor = false;
            // 
            // dataGridPropertyFields
            // 
            this.dataGridPropertyFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridPropertyFields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PropertyName,
            this.Required,
            this.Length,
            this.Type,
            this.IsKey});
            this.dataGridPropertyFields.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridPropertyFields.Location = new System.Drawing.Point(22, 260);
            this.dataGridPropertyFields.Name = "dataGridPropertyFields";
            this.dataGridPropertyFields.RowTemplate.Height = 25;
            this.dataGridPropertyFields.Size = new System.Drawing.Size(545, 150);
            this.dataGridPropertyFields.TabIndex = 1;
            this.dataGridPropertyFields.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridPropertyFields_CellContentClick);
            // 
            // PropertyName
            // 
            this.PropertyName.HeaderText = "Property Name";
            this.PropertyName.Name = "PropertyName";
            // 
            // Required
            // 
            this.Required.HeaderText = "Required?";
            this.Required.Name = "Required";
            this.Required.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Required.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Length
            // 
            this.Length.HeaderText = "Length";
            this.Length.Name = "Length";
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            // 
            // IsKey
            // 
            this.IsKey.HeaderText = "IsKey?";
            this.IsKey.Name = "IsKey";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.IndianRed;
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnClear.Location = new System.Drawing.Point(452, 416);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(115, 46);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // checkCore
            // 
            this.checkCore.AutoSize = true;
            this.checkCore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkCore.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.checkCore.Location = new System.Drawing.Point(105, 214);
            this.checkCore.Name = "checkCore";
            this.checkCore.Size = new System.Drawing.Size(15, 14);
            this.checkCore.TabIndex = 3;
            this.checkCore.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkCore.UseVisualStyleBackColor = true;
            // 
            // txtSelectFolder
            // 
            this.txtSelectFolder.Location = new System.Drawing.Point(22, 40);
            this.txtSelectFolder.Name = "txtSelectFolder";
            this.txtSelectFolder.Size = new System.Drawing.Size(443, 23);
            this.txtSelectFolder.TabIndex = 4;
            // 
            // txtPluralName
            // 
            this.txtPluralName.Location = new System.Drawing.Point(22, 166);
            this.txtPluralName.Name = "txtPluralName";
            this.txtPluralName.Size = new System.Drawing.Size(541, 23);
            this.txtPluralName.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(22, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Select Project Destination...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(22, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Plural Name...";
            // 
            // txtTableName
            // 
            this.txtTableName.Location = new System.Drawing.Point(22, 101);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size(539, 23);
            this.txtTableName.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(22, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "Table Name...";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(22, 242);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Enter Fields...";
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnSelectFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectFolder.FlatAppearance.BorderColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnSelectFolder.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSelectFolder.Location = new System.Drawing.Point(471, 40);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(94, 23);
            this.btnSelectFolder.TabIndex = 11;
            this.btnSelectFolder.Text = "Select...";
            this.btnSelectFolder.UseVisualStyleBackColor = false;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(22, 213);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "Is Core Class?";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 476);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTableName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPluralName);
            this.Controls.Add(this.txtSelectFolder);
            this.Controls.Add(this.checkCore);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.dataGridPropertyFields);
            this.Controls.Add(this.btnGenerate);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "Form1";
            this.Text = "FitFactory Code Generator";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPropertyFields)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnGenerate;
        private DataGridView dataGridPropertyFields;
        private Button btnClear;
        private CheckBox checkCore;
        private TextBox txtSelectFolder;
        private TextBox txtPluralName;
        private DataGridViewTextBoxColumn PropertyName;
        private DataGridViewCheckBoxColumn Required;
        private DataGridViewTextBoxColumn Length;
        private DataGridViewTextBoxColumn Type;
        private DataGridViewCheckBoxColumn IsKey;
        private Label label1;
        private Label label2;
        private TextBox txtTableName;
        private Label label3;
        private Label label4;
        private FolderBrowserDialog folderBrowserDialog1;
        private Button btnSelectFolder;
        private Label label5;
    }
}