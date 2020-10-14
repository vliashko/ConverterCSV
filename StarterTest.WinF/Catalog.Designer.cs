namespace StarterTest.WinF
{
    partial class Catalog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WorkWithTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChangeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowDataMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearAllDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sqlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(12, 86);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.ShowEditingIcon = false;
            this.dataGridView.Size = new System.Drawing.Size(846, 424);
            this.dataGridView.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ImportToolStripMenuItem,
            this.WorkWithTableToolStripMenuItem,
            this.ShowDataMenuItem,
            this.ClearAllDatabaseToolStripMenuItem,
            this.sqlToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(870, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ImportToolStripMenuItem
            // 
            this.ImportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ImportMenuItem,
            this.ExportMenuItem});
            this.ImportToolStripMenuItem.Name = "ImportToolStripMenuItem";
            this.ImportToolStripMenuItem.Size = new System.Drawing.Size(120, 20);
            this.ImportToolStripMenuItem.Text = "Работа с данными";
            // 
            // ImportMenuItem
            // 
            this.ImportMenuItem.Name = "ImportMenuItem";
            this.ImportMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ImportMenuItem.Text = "Импорт (.csv)";
            this.ImportMenuItem.Click += new System.EventHandler(this.ImportMenuItem_Click);
            // 
            // ExportMenuItem
            // 
            this.ExportMenuItem.Name = "ExportMenuItem";
            this.ExportMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ExportMenuItem.Text = "Экспорт";
            this.ExportMenuItem.Click += new System.EventHandler(this.ExportMenuItem_Click);
            // 
            // WorkWithTableToolStripMenuItem
            // 
            this.WorkWithTableToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddMenuItem,
            this.ChangeMenuItem,
            this.DeleteMenuItem});
            this.WorkWithTableToolStripMenuItem.Name = "WorkWithTableToolStripMenuItem";
            this.WorkWithTableToolStripMenuItem.Size = new System.Drawing.Size(121, 20);
            this.WorkWithTableToolStripMenuItem.Text = "Работа с таблицей";
            // 
            // AddMenuItem
            // 
            this.AddMenuItem.Name = "AddMenuItem";
            this.AddMenuItem.Size = new System.Drawing.Size(168, 22);
            this.AddMenuItem.Text = "Добавить запись";
            this.AddMenuItem.Click += new System.EventHandler(this.AddMenuItem_Click);
            // 
            // ChangeMenuItem
            // 
            this.ChangeMenuItem.Enabled = false;
            this.ChangeMenuItem.Name = "ChangeMenuItem";
            this.ChangeMenuItem.Size = new System.Drawing.Size(168, 22);
            this.ChangeMenuItem.Text = "Изменить запись";
            this.ChangeMenuItem.Click += new System.EventHandler(this.ChangeMenuItem_Click);
            // 
            // DeleteMenuItem
            // 
            this.DeleteMenuItem.Enabled = false;
            this.DeleteMenuItem.Name = "DeleteMenuItem";
            this.DeleteMenuItem.Size = new System.Drawing.Size(168, 22);
            this.DeleteMenuItem.Text = "Удалить запись";
            this.DeleteMenuItem.Click += new System.EventHandler(this.DeleteMenuItem_Click);
            // 
            // ShowDataMenuItem
            // 
            this.ShowDataMenuItem.Name = "ShowDataMenuItem";
            this.ShowDataMenuItem.Size = new System.Drawing.Size(127, 20);
            this.ShowDataMenuItem.Text = "Отобразить данные";
            this.ShowDataMenuItem.Click += new System.EventHandler(this.ShowDataMenuItem_Click);
            // 
            // ClearAllDatabaseToolStripMenuItem
            // 
            this.ClearAllDatabaseToolStripMenuItem.Name = "ClearAllDatabaseToolStripMenuItem";
            this.ClearAllDatabaseToolStripMenuItem.Size = new System.Drawing.Size(142, 20);
            this.ClearAllDatabaseToolStripMenuItem.Text = "Очистить базу данных";
            this.ClearAllDatabaseToolStripMenuItem.Click += new System.EventHandler(this.ClearAllDatabaseToolStripMenuItem_Click);
            // 
            // sqlToolStripMenuItem
            // 
            this.sqlToolStripMenuItem.Name = "sqlToolStripMenuItem";
            this.sqlToolStripMenuItem.Size = new System.Drawing.Size(12, 20);
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(12, 33);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(51, 47);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // Catalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 522);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Catalog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Основная форма";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ImportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImportMenuItem;
        private System.Windows.Forms.ToolStripMenuItem WorkWithTableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AddMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ChangeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowDataMenuItem;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem ClearAllDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sqlToolStripMenuItem;
    }
}