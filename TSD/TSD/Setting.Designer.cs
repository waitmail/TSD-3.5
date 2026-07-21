namespace TSD
{
    partial class Setting
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
            this.cmb_bases = new System.Windows.Forms.ComboBox();
            this.lbl_num_base = new System.Windows.Forms.Label();
            this.btn_write_setting = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.lbl_version = new System.Windows.Forms.Label();
            this.lbl_guid = new System.Windows.Forms.Label();
            this.btn_get_new_program = new System.Windows.Forms.Button();
            this.lbl_have_new_version = new System.Windows.Forms.Label();
            this.btn_get_dll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmb_bases
            // 
            this.cmb_bases.Location = new System.Drawing.Point(126, 22);
            this.cmb_bases.Name = "cmb_bases";
            this.cmb_bases.Size = new System.Drawing.Size(188, 23);
            this.cmb_bases.TabIndex = 0;
            // 
            // lbl_num_base
            // 
            this.lbl_num_base.Location = new System.Drawing.Point(5, 22);
            this.lbl_num_base.Name = "lbl_num_base";
            this.lbl_num_base.Size = new System.Drawing.Size(114, 20);
            this.lbl_num_base.Text = "База";
            this.lbl_num_base.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btn_write_setting
            // 
            this.btn_write_setting.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.btn_write_setting.Location = new System.Drawing.Point(3, 258);
            this.btn_write_setting.Name = "btn_write_setting";
            this.btn_write_setting.Size = new System.Drawing.Size(155, 34);
            this.btn_write_setting.TabIndex = 2;
            this.btn_write_setting.Text = "(0) Записать";
            this.btn_write_setting.Click += new System.EventHandler(this.btn_write_setting_Click);
            // 
            // btn_close
            // 
            this.btn_close.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.btn_close.Location = new System.Drawing.Point(164, 258);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(150, 34);
            this.btn_close.TabIndex = 3;
            this.btn_close.Text = "(1) Закрыть";
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // lbl_version
            // 
            this.lbl_version.BackColor = System.Drawing.Color.White;
            this.lbl_version.Location = new System.Drawing.Point(5, 81);
            this.lbl_version.Name = "lbl_version";
            this.lbl_version.Size = new System.Drawing.Size(309, 20);
            this.lbl_version.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbl_guid
            // 
            this.lbl_guid.BackColor = System.Drawing.Color.White;
            this.lbl_guid.Location = new System.Drawing.Point(5, 54);
            this.lbl_guid.Name = "lbl_guid";
            this.lbl_guid.Size = new System.Drawing.Size(310, 20);
            this.lbl_guid.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btn_get_new_program
            // 
            this.btn_get_new_program.Enabled = false;
            this.btn_get_new_program.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.btn_get_new_program.Location = new System.Drawing.Point(3, 171);
            this.btn_get_new_program.Name = "btn_get_new_program";
            this.btn_get_new_program.Size = new System.Drawing.Size(311, 44);
            this.btn_get_new_program.TabIndex = 5;
            this.btn_get_new_program.Text = "(2) Загрузить обновление программы";
            this.btn_get_new_program.Click += new System.EventHandler(this.btn_get_new_program_Click);
            // 
            // lbl_have_new_version
            // 
            this.lbl_have_new_version.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_have_new_version.BackColor = System.Drawing.Color.White;
            this.lbl_have_new_version.Location = new System.Drawing.Point(5, 107);
            this.lbl_have_new_version.Name = "lbl_have_new_version";
            this.lbl_have_new_version.Size = new System.Drawing.Size(309, 59);
            this.lbl_have_new_version.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btn_get_dll
            // 
            this.btn_get_dll.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.btn_get_dll.Location = new System.Drawing.Point(5, 219);
            this.btn_get_dll.Name = "btn_get_dll";
            this.btn_get_dll.Size = new System.Drawing.Size(308, 34);
            this.btn_get_dll.TabIndex = 9;
            this.btn_get_dll.Text = "(3) Загрузить Dll";
            this.btn_get_dll.Click += new System.EventHandler(this.btn_get_dll_Click);
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(318, 295);
            this.ControlBox = false;
            this.Controls.Add(this.btn_get_dll);
            this.Controls.Add(this.lbl_have_new_version);
            this.Controls.Add(this.btn_get_new_program);
            this.Controls.Add(this.lbl_guid);
            this.Controls.Add(this.lbl_version);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_write_setting);
            this.Controls.Add(this.lbl_num_base);
            this.Controls.Add(this.cmb_bases);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Setting";
            this.Text = "Настройки";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmb_bases;
        private System.Windows.Forms.Label lbl_num_base;
        private System.Windows.Forms.Button btn_write_setting;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Label lbl_version;
        private System.Windows.Forms.Label lbl_guid;
        private System.Windows.Forms.Button btn_get_new_program;
        private System.Windows.Forms.Label lbl_have_new_version;
        private System.Windows.Forms.Button btn_get_dll;
    }
}