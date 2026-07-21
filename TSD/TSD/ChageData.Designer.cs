namespace TSD
{
    partial class ChageData
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn_load_documents_1c = new System.Windows.Forms.Button();
            this.btn_execute_full_sinhronization = new System.Windows.Forms.Button();
            this.label_powerstatus = new System.Windows.Forms.Label();
            this.btn_upload_logs = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(3, 50);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(311, 198);
            this.textBox1.TabIndex = 5;
            // 
            // btn_load_documents_1c
            // 
            this.btn_load_documents_1c.Location = new System.Drawing.Point(3, 4);
            this.btn_load_documents_1c.Name = "btn_load_documents_1c";
            this.btn_load_documents_1c.Size = new System.Drawing.Size(311, 40);
            this.btn_load_documents_1c.TabIndex = 6;
            this.btn_load_documents_1c.Text = "(1) Обмен документами с 1с";
            this.btn_load_documents_1c.Click += new System.EventHandler(this.btn_load_documents_1c_Click);
            // 
            // btn_execute_full_sinhronization
            // 
            this.btn_execute_full_sinhronization.Location = new System.Drawing.Point(78, 252);
            this.btn_execute_full_sinhronization.Name = "btn_execute_full_sinhronization";
            this.btn_execute_full_sinhronization.Size = new System.Drawing.Size(176, 40);
            this.btn_execute_full_sinhronization.TabIndex = 8;
            this.btn_execute_full_sinhronization.Text = "(0) Полная синхронизация";
            this.btn_execute_full_sinhronization.Click += new System.EventHandler(this.btn_execute_full_sinhronization_Click);
            // 
            // label_powerstatus
            // 
            this.label_powerstatus.ForeColor = System.Drawing.Color.Red;
            this.label_powerstatus.Location = new System.Drawing.Point(258, 262);
            this.label_powerstatus.Name = "label_powerstatus";
            this.label_powerstatus.Size = new System.Drawing.Size(56, 30);
            // 
            // btn_upload_logs
            // 
            this.btn_upload_logs.Location = new System.Drawing.Point(2, 252);
            this.btn_upload_logs.Name = "btn_upload_logs";
            this.btn_upload_logs.Size = new System.Drawing.Size(75, 40);
            this.btn_upload_logs.TabIndex = 9;
            this.btn_upload_logs.Text = "Логи";
            this.btn_upload_logs.Visible = false;           
            // 
            // ChageData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(318, 295);
            this.ControlBox = false;
            this.Controls.Add(this.btn_upload_logs);
            this.Controls.Add(this.label_powerstatus);
            this.Controls.Add(this.btn_execute_full_sinhronization);
            this.Controls.Add(this.btn_load_documents_1c);
            this.Controls.Add(this.textBox1);
            this.Name = "ChageData";
            this.Text = "Обмен данными";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_load_documents_1c;
        private System.Windows.Forms.Button btn_execute_full_sinhronization;
        private System.Windows.Forms.Label label_powerstatus;
        private System.Windows.Forms.Button btn_upload_logs;
    }
}