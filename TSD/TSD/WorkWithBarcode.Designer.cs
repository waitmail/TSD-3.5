namespace TSD
{
    partial class WorkWithBarcode
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                // Отписка от событий
                this.txtB_input_barcode.KeyPress -= txtB_input_barcode_KeyPress;
                this.txtB_quantity.KeyPress -= txtB_quantity_KeyPress;
                this.Load -= WorkWithBarcode_Load;
                this.Paint -= WorkWithBarcode_Paint;

                // Остановка таймера
                if (timer != null)
                {
                    timer.Enabled=false;
                    timer.Dispose();
                }
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
            this.label_decription_document = new System.Windows.Forms.Label();
            this.txtB_input_barcode = new System.Windows.Forms.TextBox();
            this.txtB_tovar = new System.Windows.Forms.TextBox();
            this.txtB_quantity = new System.Windows.Forms.TextBox();
            this.label_сканировано = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_price_value = new System.Windows.Forms.Label();
            this.label_количество_в_1с = new System.Windows.Forms.Label();
            this.label_код_шк = new System.Windows.Forms.Label();
            this.panel_tovar_not_found = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_количество_в_магазине = new System.Windows.Forms.Label();
            this.label_powerstatus = new System.Windows.Forms.Label();
            this.panel_tovar_not_found.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_decription_document
            // 
            this.label_decription_document.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label_decription_document.BackColor = System.Drawing.Color.Silver;
            this.label_decription_document.Enabled = false;
            this.label_decription_document.Location = new System.Drawing.Point(4, 4);
            this.label_decription_document.Name = "label_decription_document";
            this.label_decription_document.Size = new System.Drawing.Size(311, 38);
            this.label_decription_document.Text = "ФАКТ = ПЛАН  ЗЕЛЕНАЯ КНОПКА";
            this.label_decription_document.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtB_input_barcode
            // 
            this.txtB_input_barcode.Location = new System.Drawing.Point(78, 268);
            this.txtB_input_barcode.MaxLength = 13;
            this.txtB_input_barcode.Name = "txtB_input_barcode";
            this.txtB_input_barcode.Size = new System.Drawing.Size(116, 23);
            this.txtB_input_barcode.TabIndex = 5;
            // 
            // txtB_tovar
            // 
            this.txtB_tovar.Enabled = false;
            this.txtB_tovar.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular);
            this.txtB_tovar.Location = new System.Drawing.Point(3, 45);
            this.txtB_tovar.MaxLength = 100;
            this.txtB_tovar.Multiline = true;
            this.txtB_tovar.Name = "txtB_tovar";
            this.txtB_tovar.Size = new System.Drawing.Size(310, 161);
            this.txtB_tovar.TabIndex = 6;
            // 
            // txtB_quantity
            // 
            this.txtB_quantity.Location = new System.Drawing.Point(42, 239);
            this.txtB_quantity.MaxLength = 5;
            this.txtB_quantity.Name = "txtB_quantity";
            this.txtB_quantity.Size = new System.Drawing.Size(42, 23);
            this.txtB_quantity.TabIndex = 7;
            // 
            // label_сканировано
            // 
            this.label_сканировано.Location = new System.Drawing.Point(6, 242);
            this.label_сканировано.Name = "label_сканировано";
            this.label_сканировано.Size = new System.Drawing.Size(30, 20);
            this.label_сканировано.Text = "Кол";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(89, 239);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 20);
            this.label1.Text = "из";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.label2.Location = new System.Drawing.Point(6, 210);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 20);
            this.label2.Text = "Цена";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.label4.Location = new System.Drawing.Point(133, 210);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 20);
            this.label4.Text = "р.";
            // 
            // label_price_value
            // 
            this.label_price_value.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.label_price_value.Location = new System.Drawing.Point(53, 212);
            this.label_price_value.Name = "label_price_value";
            this.label_price_value.Size = new System.Drawing.Size(74, 24);
            this.label_price_value.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label_количество_в_1с
            // 
            this.label_количество_в_1с.Location = new System.Drawing.Point(115, 239);
            this.label_количество_в_1с.Name = "label_количество_в_1с";
            this.label_количество_в_1с.Size = new System.Drawing.Size(42, 20);
            // 
            // label_код_шк
            // 
            this.label_код_шк.Location = new System.Drawing.Point(6, 268);
            this.label_код_шк.Name = "label_код_шк";
            this.label_код_шк.Size = new System.Drawing.Size(67, 20);
            this.label_код_шк.Text = "Код / ШК";
            // 
            // panel_tovar_not_found
            // 
            this.panel_tovar_not_found.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel_tovar_not_found.Controls.Add(this.label5);
            this.panel_tovar_not_found.Controls.Add(this.label3);
            this.panel_tovar_not_found.Location = new System.Drawing.Point(3, 45);
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
            // label_количество_в_магазине
            // 
            this.label_количество_в_магазине.Location = new System.Drawing.Point(163, 236);
            this.label_количество_в_магазине.Name = "label_количество_в_магазине";
            this.label_количество_в_магазине.Size = new System.Drawing.Size(145, 22);
            // 
            // label_powerstatus
            // 
            this.label_powerstatus.ForeColor = System.Drawing.Color.Red;
            this.label_powerstatus.Location = new System.Drawing.Point(198, 269);
            this.label_powerstatus.Name = "label_powerstatus";
            this.label_powerstatus.Size = new System.Drawing.Size(112, 20);
            // 
            // WorkWithBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(318, 295);
            this.ControlBox = false;
            this.Controls.Add(this.label_powerstatus);
            this.Controls.Add(this.label_количество_в_магазине);
            this.Controls.Add(this.label_количество_в_1с);
            this.Controls.Add(this.label_код_шк);
            this.Controls.Add(this.label_price_value);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_сканировано);
            this.Controls.Add(this.txtB_quantity);
            this.Controls.Add(this.txtB_tovar);
            this.Controls.Add(this.txtB_input_barcode);
            this.Controls.Add(this.label_decription_document);
            this.Controls.Add(this.panel_tovar_not_found);
            this.Name = "WorkWithBarcode";
            this.Text = "Сканирование";
            this.TopMost = true;
            this.panel_tovar_not_found.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_сканировано;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label_decription_document;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_price_value;
        private System.Windows.Forms.Label label_код_шк;
        private System.Windows.Forms.Panel panel_tovar_not_found;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txtB_input_barcode;
        public System.Windows.Forms.TextBox txtB_tovar;
        public System.Windows.Forms.TextBox txtB_quantity;
        public System.Windows.Forms.Label label_количество_в_1с;
        public System.Windows.Forms.Label label_количество_в_магазине;
        private System.Windows.Forms.Label label_powerstatus;
    }
}