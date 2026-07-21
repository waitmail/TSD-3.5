namespace TSD
{
    partial class CheckAndPrintPrices
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
            if (this.timer != null)
            {                
                this.timer.Tick -= timer_Tick; // Отключаем обработчик события
                this.timer.Dispose(); // Освобождаем ресурсы таймера
            }            
            this.txtB_input_barcode.KeyPress -= txtB_input_barcode_KeyPress;
            this.txtB_quantity.KeyPress -= txtB_quantity_KeyPress;                        
            this.Load -= CheckAndPrintPrices_Load;            
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtB_tovar = new System.Windows.Forms.TextBox();
            this.txtB_price_value = new System.Windows.Forms.TextBox();
            this.label_шк = new System.Windows.Forms.Label();
            this.label_ценников = new System.Windows.Forms.Label();
            this.txtB_quantity = new System.Windows.Forms.TextBox();
            this.txtB_input_barcode = new System.Windows.Forms.TextBox();
            this.label_всего = new System.Windows.Forms.Label();
            this.txtB_total_price_tags = new System.Windows.Forms.TextBox();
            this.panel_tovar_not_found = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_enter_date = new System.Windows.Forms.Button();
            this.label_product_expiration_date = new System.Windows.Forms.Label();
            this.panel_tovar_not_found.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtB_tovar
            // 
            this.txtB_tovar.Enabled = false;
            this.txtB_tovar.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular);
            this.txtB_tovar.Location = new System.Drawing.Point(4, 3);
            this.txtB_tovar.MaxLength = 80;
            this.txtB_tovar.Multiline = true;
            this.txtB_tovar.Name = "txtB_tovar";
            this.txtB_tovar.Size = new System.Drawing.Size(310, 122);
            this.txtB_tovar.TabIndex = 7;
            // 
            // txtB_price_value
            // 
            this.txtB_price_value.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtB_price_value.Enabled = false;
            this.txtB_price_value.Font = new System.Drawing.Font("Tahoma", 26F, System.Drawing.FontStyle.Regular);
            this.txtB_price_value.Location = new System.Drawing.Point(4, 129);
            this.txtB_price_value.MaxLength = 8;
            this.txtB_price_value.Multiline = true;
            this.txtB_price_value.Name = "txtB_price_value";
            this.txtB_price_value.Size = new System.Drawing.Size(310, 57);
            this.txtB_price_value.TabIndex = 8;
            this.txtB_price_value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_шк
            // 
            this.label_шк.Location = new System.Drawing.Point(4, 237);
            this.label_шк.Name = "label_шк";
            this.label_шк.Size = new System.Drawing.Size(32, 17);
            this.label_шк.Text = "ШК";
            // 
            // label_ценников
            // 
            this.label_ценников.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.label_ценников.Location = new System.Drawing.Point(4, 192);
            this.label_ценников.Name = "label_ценников";
            this.label_ценников.Size = new System.Drawing.Size(87, 23);
            this.label_ценников.Text = "Ценников";
            // 
            // txtB_quantity
            // 
            this.txtB_quantity.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.txtB_quantity.Location = new System.Drawing.Point(92, 192);
            this.txtB_quantity.MaxLength = 2;
            this.txtB_quantity.Name = "txtB_quantity";
            this.txtB_quantity.Size = new System.Drawing.Size(42, 29);
            this.txtB_quantity.TabIndex = 12;
            // 
            // txtB_input_barcode
            // 
            this.txtB_input_barcode.Location = new System.Drawing.Point(39, 231);
            this.txtB_input_barcode.MaxLength = 13;
            this.txtB_input_barcode.Name = "txtB_input_barcode";
            this.txtB_input_barcode.Size = new System.Drawing.Size(275, 23);
            this.txtB_input_barcode.TabIndex = 11;
            // 
            // label_всего
            // 
            this.label_всего.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.label_всего.Location = new System.Drawing.Point(140, 192);
            this.label_всего.Name = "label_всего";
            this.label_всего.Size = new System.Drawing.Size(56, 23);
            this.label_всего.Text = "Всего";
            // 
            // txtB_total_price_tags
            // 
            this.txtB_total_price_tags.Enabled = false;
            this.txtB_total_price_tags.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.txtB_total_price_tags.Location = new System.Drawing.Point(202, 192);
            this.txtB_total_price_tags.MaxLength = 1;
            this.txtB_total_price_tags.Name = "txtB_total_price_tags";
            this.txtB_total_price_tags.Size = new System.Drawing.Size(42, 29);
            this.txtB_total_price_tags.TabIndex = 17;
            // 
            // panel_tovar_not_found
            // 
            this.panel_tovar_not_found.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel_tovar_not_found.Controls.Add(this.label5);
            this.panel_tovar_not_found.Controls.Add(this.label3);
            this.panel_tovar_not_found.Location = new System.Drawing.Point(3, 39);
            this.panel_tovar_not_found.Name = "panel_tovar_not_found";
            this.panel_tovar_not_found.Size = new System.Drawing.Size(312, 217);
            this.panel_tovar_not_found.Visible = false;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Regular);
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(6, 89);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(300, 58);
            this.label5.Text = "НЕ НАЙДЕН";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Regular);
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(6, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(300, 58);
            this.label3.Text = "ТОВАР";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btn_enter_date
            // 
            this.btn_enter_date.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.btn_enter_date.Location = new System.Drawing.Point(4, 257);
            this.btn_enter_date.Name = "btn_enter_date";
            this.btn_enter_date.Size = new System.Drawing.Size(148, 35);
            this.btn_enter_date.TabIndex = 22;
            this.btn_enter_date.Text = "Следующий срок";
            this.btn_enter_date.Visible = false;
            this.btn_enter_date.Click += new System.EventHandler(this.btn_enter_date_Click);
            // 
            // label_product_expiration_date
            // 
            this.label_product_expiration_date.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label_product_expiration_date.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Regular);
            this.label_product_expiration_date.Location = new System.Drawing.Point(153, 257);
            this.label_product_expiration_date.Name = "label_product_expiration_date";
            this.label_product_expiration_date.Size = new System.Drawing.Size(156, 35);
            this.label_product_expiration_date.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label_product_expiration_date.Visible = false;
            // 
            // CheckAndPrintPrices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(318, 295);
            this.ControlBox = false;
            this.Controls.Add(this.label_product_expiration_date);
            this.Controls.Add(this.btn_enter_date);
            this.Controls.Add(this.txtB_total_price_tags);
            this.Controls.Add(this.label_всего);
            this.Controls.Add(this.label_шк);
            this.Controls.Add(this.label_ценников);
            this.Controls.Add(this.txtB_quantity);
            this.Controls.Add(this.txtB_input_barcode);
            this.Controls.Add(this.txtB_price_value);
            this.Controls.Add(this.txtB_tovar);
            this.Controls.Add(this.panel_tovar_not_found);
            this.Name = "CheckAndPrintPrices";
            this.Text = "Проверка и печать ценников";
            this.TopMost = true;
            this.panel_tovar_not_found.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TextBox txtB_tovar;
        private System.Windows.Forms.TextBox txtB_price_value;
        private System.Windows.Forms.Label label_шк;
        private System.Windows.Forms.Label label_ценников;
        public System.Windows.Forms.TextBox txtB_quantity;
        public System.Windows.Forms.TextBox txtB_input_barcode;
        private System.Windows.Forms.Label label_всего;
        public System.Windows.Forms.TextBox txtB_total_price_tags;
        private System.Windows.Forms.Panel panel_tovar_not_found;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_enter_date;
        public System.Windows.Forms.Label label_product_expiration_date;
    }
}