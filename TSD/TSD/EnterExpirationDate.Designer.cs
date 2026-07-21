namespace TSD
{
    partial class EnterExpirationDate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_year0 = new System.Windows.Forms.Button();
            this.btn_year1 = new System.Windows.Forms.Button();
            this.btn_year2 = new System.Windows.Forms.Button();
            this.ttB_Month = new System.Windows.Forms.TextBox();
            this.btn_year3 = new System.Windows.Forms.Button();
            this.btn_year4 = new System.Windows.Forms.Button();
            this.btn_year5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Regular);
            this.label1.Location = new System.Drawing.Point(20, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 39);
            this.label1.Text = "Месяц :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btn_year0
            // 
            this.btn_year0.Font = new System.Drawing.Font("Tahoma", 28F, System.Drawing.FontStyle.Regular);
            this.btn_year0.Location = new System.Drawing.Point(7, 92);
            this.btn_year0.Name = "btn_year0";
            this.btn_year0.Size = new System.Drawing.Size(100, 80);
            this.btn_year0.TabIndex = 9;
            this.btn_year0.Text = "1";
            this.btn_year0.Click += new System.EventHandler(this.btn_year0_Click);
            // 
            // btn_year1
            // 
            this.btn_year1.Font = new System.Drawing.Font("Tahoma", 28F, System.Drawing.FontStyle.Regular);
            this.btn_year1.Location = new System.Drawing.Point(111, 92);
            this.btn_year1.Name = "btn_year1";
            this.btn_year1.Size = new System.Drawing.Size(100, 80);
            this.btn_year1.TabIndex = 10;
            this.btn_year1.Text = "2";
            this.btn_year1.Click += new System.EventHandler(this.btn_year1_Click);
            // 
            // btn_year2
            // 
            this.btn_year2.Font = new System.Drawing.Font("Tahoma", 28F, System.Drawing.FontStyle.Regular);
            this.btn_year2.Location = new System.Drawing.Point(215, 92);
            this.btn_year2.Name = "btn_year2";
            this.btn_year2.Size = new System.Drawing.Size(100, 80);
            this.btn_year2.TabIndex = 11;
            this.btn_year2.Text = "3";
            this.btn_year2.Click += new System.EventHandler(this.btn_year2_Click);
            // 
            // ttB_Month
            // 
            this.ttB_Month.Font = new System.Drawing.Font("Tahoma", 28F, System.Drawing.FontStyle.Regular);
            this.ttB_Month.Location = new System.Drawing.Point(185, 20);
            this.ttB_Month.MaxLength = 2;
            this.ttB_Month.Name = "ttB_Month";
            this.ttB_Month.Size = new System.Drawing.Size(85, 52);
            this.ttB_Month.TabIndex = 12;
            // 
            // btn_year3
            // 
            this.btn_year3.Font = new System.Drawing.Font("Tahoma", 28F, System.Drawing.FontStyle.Regular);
            this.btn_year3.Location = new System.Drawing.Point(7, 189);
            this.btn_year3.Name = "btn_year3";
            this.btn_year3.Size = new System.Drawing.Size(100, 80);
            this.btn_year3.TabIndex = 13;
            this.btn_year3.Text = "4";
            this.btn_year3.Click += new System.EventHandler(this.btn_year3_Click);
            // 
            // btn_year4
            // 
            this.btn_year4.Font = new System.Drawing.Font("Tahoma", 28F, System.Drawing.FontStyle.Regular);
            this.btn_year4.Location = new System.Drawing.Point(111, 189);
            this.btn_year4.Name = "btn_year4";
            this.btn_year4.Size = new System.Drawing.Size(100, 80);
            this.btn_year4.TabIndex = 14;
            this.btn_year4.Text = "5";
            this.btn_year4.Click += new System.EventHandler(this.btn_year4_Click);
            // 
            // btn_year5
            // 
            this.btn_year5.Font = new System.Drawing.Font("Tahoma", 28F, System.Drawing.FontStyle.Regular);
            this.btn_year5.Location = new System.Drawing.Point(216, 189);
            this.btn_year5.Name = "btn_year5";
            this.btn_year5.Size = new System.Drawing.Size(100, 80);
            this.btn_year5.TabIndex = 15;
            this.btn_year5.Text = "6";
            this.btn_year5.Click += new System.EventHandler(this.btn_year5_Click);
            // 
            // EnterExpirationDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(318, 295);
            this.ControlBox = false;
            this.Controls.Add(this.btn_year5);
            this.Controls.Add(this.btn_year4);
            this.Controls.Add(this.btn_year3);
            this.Controls.Add(this.ttB_Month);
            this.Controls.Add(this.btn_year2);
            this.Controls.Add(this.btn_year1);
            this.Controls.Add(this.btn_year0);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.Name = "EnterExpirationDate";
            this.Text = "Ввод срока годности";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_year0;
        private System.Windows.Forms.Button btn_year1;
        private System.Windows.Forms.Button btn_year2;
        private System.Windows.Forms.TextBox ttB_Month;
        private System.Windows.Forms.Button btn_year3;
        private System.Windows.Forms.Button btn_year4;
        private System.Windows.Forms.Button btn_year5;
    }
}