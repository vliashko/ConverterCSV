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
            this.импортToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.импортToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.экспортToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.работаСТаблицейToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьЗаписьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изменитьЗаписьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьЗаписьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отобразитьДанныеможетЗанятьНекотороеВремяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.menuStrip1.SuspendLayout();
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
            this.dataGridView.Location = new System.Drawing.Point(12, 43);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.ShowEditingIcon = false;
            this.dataGridView.Size = new System.Drawing.Size(776, 395);
            this.dataGridView.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.импортToolStripMenuItem,
            this.работаСТаблицейToolStripMenuItem,
            this.отобразитьДанныеможетЗанятьНекотороеВремяToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // импортToolStripMenuItem
            // 
            this.импортToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.импортToolStripMenuItem1,
            this.экспортToolStripMenuItem});
            this.импортToolStripMenuItem.Name = "импортToolStripMenuItem";
            this.импортToolStripMenuItem.Size = new System.Drawing.Size(120, 20);
            this.импортToolStripMenuItem.Text = "Работа с данными";
            // 
            // импортToolStripMenuItem1
            // 
            this.импортToolStripMenuItem1.Name = "импортToolStripMenuItem1";
            this.импортToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.импортToolStripMenuItem1.Text = "Импорт (.csv)";
            this.импортToolStripMenuItem1.Click += new System.EventHandler(this.импортToolStripMenuItem1_Click);
            // 
            // экспортToolStripMenuItem
            // 
            this.экспортToolStripMenuItem.Name = "экспортToolStripMenuItem";
            this.экспортToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.экспортToolStripMenuItem.Text = "Экспорт";
            this.экспортToolStripMenuItem.Click += new System.EventHandler(this.экспортToolStripMenuItem_Click);
            // 
            // работаСТаблицейToolStripMenuItem
            // 
            this.работаСТаблицейToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьЗаписьToolStripMenuItem,
            this.изменитьЗаписьToolStripMenuItem,
            this.удалитьЗаписьToolStripMenuItem});
            this.работаСТаблицейToolStripMenuItem.Enabled = false;
            this.работаСТаблицейToolStripMenuItem.Name = "работаСТаблицейToolStripMenuItem";
            this.работаСТаблицейToolStripMenuItem.Size = new System.Drawing.Size(121, 20);
            this.работаСТаблицейToolStripMenuItem.Text = "Работа с таблицей";
            // 
            // добавитьЗаписьToolStripMenuItem
            // 
            this.добавитьЗаписьToolStripMenuItem.Name = "добавитьЗаписьToolStripMenuItem";
            this.добавитьЗаписьToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.добавитьЗаписьToolStripMenuItem.Text = "Добавить запись";
            this.добавитьЗаписьToolStripMenuItem.Click += new System.EventHandler(this.добавитьЗаписьToolStripMenuItem_Click);
            // 
            // изменитьЗаписьToolStripMenuItem
            // 
            this.изменитьЗаписьToolStripMenuItem.Name = "изменитьЗаписьToolStripMenuItem";
            this.изменитьЗаписьToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.изменитьЗаписьToolStripMenuItem.Text = "Изменить запись";
            this.изменитьЗаписьToolStripMenuItem.Click += new System.EventHandler(this.изменитьЗаписьToolStripMenuItem_Click);
            // 
            // удалитьЗаписьToolStripMenuItem
            // 
            this.удалитьЗаписьToolStripMenuItem.Name = "удалитьЗаписьToolStripMenuItem";
            this.удалитьЗаписьToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.удалитьЗаписьToolStripMenuItem.Text = "Удалить запись";
            this.удалитьЗаписьToolStripMenuItem.Click += new System.EventHandler(this.удалитьЗаписьToolStripMenuItem_Click);
            // 
            // отобразитьДанныеможетЗанятьНекотороеВремяToolStripMenuItem
            // 
            this.отобразитьДанныеможетЗанятьНекотороеВремяToolStripMenuItem.Name = "отобразитьДанныеможетЗанятьНекотороеВремяToolStripMenuItem";
            this.отобразитьДанныеможетЗанятьНекотороеВремяToolStripMenuItem.Size = new System.Drawing.Size(310, 20);
            this.отобразитьДанныеможетЗанятьНекотороеВремяToolStripMenuItem.Text = "Отобразить данные (может занять некоторое время)";
            this.отобразитьДанныеможетЗанятьНекотороеВремяToolStripMenuItem.Click += new System.EventHandler(this.отобразитьДанныеможетЗанятьНекотороеВремяToolStripMenuItem_Click);
            // 
            // Catalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Catalog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Основная форма";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem импортToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem импортToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem работаСТаблицейToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьЗаписьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изменитьЗаписьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьЗаписьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem экспортToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отобразитьДанныеможетЗанятьНекотороеВремяToolStripMenuItem;
    }
}