namespace diplom
{
    partial class about_window
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(about_window));
            this.about_window_name = new System.Windows.Forms.Label();
            this.about_window_type = new System.Windows.Forms.Label();
            this.about_window_path = new System.Windows.Forms.Label();
            this.about_window_name1 = new System.Windows.Forms.TextBox();
            this.about_window_type2 = new System.Windows.Forms.TextBox();
            this.about_window_path2 = new System.Windows.Forms.TextBox();
            this.About_button_OK = new System.Windows.Forms.Button();
            this.about_window_admin = new System.Windows.Forms.Label();
            this.about_window_admin_number = new System.Windows.Forms.Label();
            this.about_window_admin1 = new System.Windows.Forms.TextBox();
            this.about_window_admin_number1 = new System.Windows.Forms.TextBox();
            this.about_window_admin_mail = new System.Windows.Forms.Label();
            this.about_window_admin_mail1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // about_window_name
            // 
            this.about_window_name.AutoSize = true;
            this.about_window_name.Location = new System.Drawing.Point(12, 9);
            this.about_window_name.Name = "about_window_name";
            this.about_window_name.Size = new System.Drawing.Size(86, 13);
            this.about_window_name.TabIndex = 0;
            this.about_window_name.Text = "Наименование:";
            // 
            // about_window_type
            // 
            this.about_window_type.AutoSize = true;
            this.about_window_type.Location = new System.Drawing.Point(12, 31);
            this.about_window_type.Name = "about_window_type";
            this.about_window_type.Size = new System.Drawing.Size(45, 13);
            this.about_window_type.TabIndex = 1;
            this.about_window_type.Text = "Режим:";
            // 
            // about_window_path
            // 
            this.about_window_path.AutoSize = true;
            this.about_window_path.Location = new System.Drawing.Point(12, 53);
            this.about_window_path.Name = "about_window_path";
            this.about_window_path.Size = new System.Drawing.Size(51, 13);
            this.about_window_path.TabIndex = 2;
            this.about_window_path.Text = "Каталог:";
            // 
            // about_window_name1
            // 
            this.about_window_name1.BackColor = System.Drawing.SystemColors.Control;
            this.about_window_name1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.about_window_name1.Location = new System.Drawing.Point(104, 9);
            this.about_window_name1.Name = "about_window_name1";
            this.about_window_name1.ReadOnly = true;
            this.about_window_name1.Size = new System.Drawing.Size(100, 13);
            this.about_window_name1.TabIndex = 7;
            this.about_window_name1.TabStop = false;
            this.about_window_name1.Text = "ИП \"Ким\"";
            // 
            // about_window_type2
            // 
            this.about_window_type2.BackColor = System.Drawing.SystemColors.Control;
            this.about_window_type2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.about_window_type2.Location = new System.Drawing.Point(104, 31);
            this.about_window_type2.Name = "about_window_type2";
            this.about_window_type2.ReadOnly = true;
            this.about_window_type2.Size = new System.Drawing.Size(100, 13);
            this.about_window_type2.TabIndex = 8;
            this.about_window_type2.TabStop = false;
            this.about_window_type2.Text = "Файловый";
            // 
            // about_window_path2
            // 
            this.about_window_path2.BackColor = System.Drawing.SystemColors.Control;
            this.about_window_path2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.about_window_path2.Location = new System.Drawing.Point(104, 53);
            this.about_window_path2.Multiline = true;
            this.about_window_path2.Name = "about_window_path2";
            this.about_window_path2.ReadOnly = true;
            this.about_window_path2.Size = new System.Drawing.Size(180, 39);
            this.about_window_path2.TabIndex = 10;
            this.about_window_path2.TabStop = false;
            // 
            // About_button_OK
            // 
            this.About_button_OK.Location = new System.Drawing.Point(220, 157);
            this.About_button_OK.Name = "About_button_OK";
            this.About_button_OK.Size = new System.Drawing.Size(70, 20);
            this.About_button_OK.TabIndex = 11;
            this.About_button_OK.Text = "ОК";
            this.About_button_OK.UseVisualStyleBackColor = true;
            this.About_button_OK.Click += new System.EventHandler(this.About_button_OK_Click);
            // 
            // about_window_admin
            // 
            this.about_window_admin.AutoSize = true;
            this.about_window_admin.Location = new System.Drawing.Point(12, 95);
            this.about_window_admin.Name = "about_window_admin";
            this.about_window_admin.Size = new System.Drawing.Size(75, 13);
            this.about_window_admin.TabIndex = 12;
            this.about_window_admin.Text = "Разработчик:";
            // 
            // about_window_admin_number
            // 
            this.about_window_admin_number.AutoSize = true;
            this.about_window_admin_number.Location = new System.Drawing.Point(12, 117);
            this.about_window_admin_number.Name = "about_window_admin_number";
            this.about_window_admin_number.Size = new System.Drawing.Size(55, 13);
            this.about_window_admin_number.TabIndex = 13;
            this.about_window_admin_number.Text = "Телефон:";
            // 
            // about_window_admin1
            // 
            this.about_window_admin1.BackColor = System.Drawing.SystemColors.Control;
            this.about_window_admin1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.about_window_admin1.Location = new System.Drawing.Point(104, 95);
            this.about_window_admin1.Name = "about_window_admin1";
            this.about_window_admin1.ReadOnly = true;
            this.about_window_admin1.Size = new System.Drawing.Size(100, 13);
            this.about_window_admin1.TabIndex = 14;
            this.about_window_admin1.TabStop = false;
            this.about_window_admin1.Text = "Гильзидинов Д.Р.";
            // 
            // about_window_admin_number1
            // 
            this.about_window_admin_number1.BackColor = System.Drawing.SystemColors.Control;
            this.about_window_admin_number1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.about_window_admin_number1.Location = new System.Drawing.Point(104, 117);
            this.about_window_admin_number1.Name = "about_window_admin_number1";
            this.about_window_admin_number1.ReadOnly = true;
            this.about_window_admin_number1.Size = new System.Drawing.Size(100, 13);
            this.about_window_admin_number1.TabIndex = 15;
            this.about_window_admin_number1.TabStop = false;
            this.about_window_admin_number1.Text = "8-(777)-666-55-44";
            // 
            // about_window_admin_mail
            // 
            this.about_window_admin_mail.AutoSize = true;
            this.about_window_admin_mail.Location = new System.Drawing.Point(12, 139);
            this.about_window_admin_mail.Name = "about_window_admin_mail";
            this.about_window_admin_mail.Size = new System.Drawing.Size(40, 13);
            this.about_window_admin_mail.TabIndex = 16;
            this.about_window_admin_mail.Text = "Почта:";
            // 
            // about_window_admin_mail1
            // 
            this.about_window_admin_mail1.BackColor = System.Drawing.SystemColors.Control;
            this.about_window_admin_mail1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.about_window_admin_mail1.Location = new System.Drawing.Point(104, 139);
            this.about_window_admin_mail1.Name = "about_window_admin_mail1";
            this.about_window_admin_mail1.ReadOnly = true;
            this.about_window_admin_mail1.Size = new System.Drawing.Size(130, 13);
            this.about_window_admin_mail1.TabIndex = 17;
            this.about_window_admin_mail1.TabStop = false;
            this.about_window_admin_mail1.Text = "gilzidinov.prog@gmail.com";
            // 
            // about_window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 181);
            this.Controls.Add(this.about_window_admin_mail1);
            this.Controls.Add(this.about_window_admin_mail);
            this.Controls.Add(this.about_window_admin_number1);
            this.Controls.Add(this.about_window_admin1);
            this.Controls.Add(this.about_window_admin_number);
            this.Controls.Add(this.about_window_admin);
            this.Controls.Add(this.About_button_OK);
            this.Controls.Add(this.about_window_path2);
            this.Controls.Add(this.about_window_type2);
            this.Controls.Add(this.about_window_name1);
            this.Controls.Add(this.about_window_path);
            this.Controls.Add(this.about_window_type);
            this.Controls.Add(this.about_window_name);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "about_window";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ИП \"Ким\" | О программе";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label about_window_name;
        private System.Windows.Forms.Label about_window_type;
        private System.Windows.Forms.Label about_window_path;
        private System.Windows.Forms.TextBox about_window_name1;
        private System.Windows.Forms.TextBox about_window_type2;
        private System.Windows.Forms.TextBox about_window_path2;
        private System.Windows.Forms.Button About_button_OK;
        private System.Windows.Forms.Label about_window_admin;
        private System.Windows.Forms.Label about_window_admin_number;
        private System.Windows.Forms.TextBox about_window_admin1;
        private System.Windows.Forms.TextBox about_window_admin_number1;
        private System.Windows.Forms.Label about_window_admin_mail;
        private System.Windows.Forms.TextBox about_window_admin_mail1;
    }
}