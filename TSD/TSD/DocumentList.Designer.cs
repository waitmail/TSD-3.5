namespace TSD
{
    partial class DocumentList
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
                this.Load -= Document_Load;
                this.txtB_inputbarcode.KeyPress -= txtB_inputbarcode_KeyPress;
                this.txtB_status.KeyDown -= txtB_status_KeyDown;
                this.listView_stroki.SelectedIndexChanged -= listView_stroki_SelectedIndexChanged;
                this.listView_stroki.KeyDown -= listView_stroki_KeyDown;
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
            this.listView_stroki = new System.Windows.Forms.ListView();
            this.date = new System.Windows.Forms.DateTimePicker();
            this.txtB_info_1s = new System.Windows.Forms.TextBox();
            this.txtB_inputbarcode = new System.Windows.Forms.TextBox();
            this.txtB_status = new System.Windows.Forms.TextBox();
            this.label_price = new System.Windows.Forms.Label();
            this.label_selected_quantity_1c = new System.Windows.Forms.Label();
            this.label_description_tovar = new System.Windows.Forms.Label();
            this.label_selected_quantity_shop = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listView_stroki
            // 
            this.listView_stroki.FullRowSelect = true;
            this.listView_stroki.Location = new System.Drawing.Point(4, 34);
            this.listView_stroki.Name = "listView_stroki";
            this.listView_stroki.Size = new System.Drawing.Size(311, 138);
            this.listView_stroki.TabIndex = 7;
            // 
            // date
            // 
            this.date.Enabled = false;
            this.date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.date.Location = new System.Drawing.Point(5, 5);
            this.date.Name = "date";
            this.date.Size = new System.Drawing.Size(123, 24);
            this.date.TabIndex = 8;
            // 
            // txtB_info_1s
            // 
            this.txtB_info_1s.Enabled = false;
            this.txtB_info_1s.Location = new System.Drawing.Point(4, 269);
            this.txtB_info_1s.MaxLength = 200;
            this.txtB_info_1s.Name = "txtB_info_1s";
            this.txtB_info_1s.Size = new System.Drawing.Size(311, 23);
            this.txtB_info_1s.TabIndex = 9;
            // 
            // txtB_inputbarcode
            // 
            this.txtB_inputbarcode.Location = new System.Drawing.Point(134, 5);
            this.txtB_inputbarcode.MaxLength = 13;
            this.txtB_inputbarcode.Name = "txtB_inputbarcode";
            this.txtB_inputbarcode.Size = new System.Drawing.Size(147, 23);
            this.txtB_inputbarcode.TabIndex = 10;
            // 
            // txtB_status
            // 
            this.txtB_status.Enabled = false;
            this.txtB_status.Location = new System.Drawing.Point(285, 5);
            this.txtB_status.MaxLength = 1;
            this.txtB_status.Name = "txtB_status";
            this.txtB_status.Size = new System.Drawing.Size(26, 23);
            this.txtB_status.TabIndex = 12;
            // 
            // label_price
            // 
            this.label_price.ForeColor = System.Drawing.Color.Blue;
            this.label_price.Location = new System.Drawing.Point(5, 179);
            this.label_price.Name = "label_price";
            this.label_price.Size = new System.Drawing.Size(100, 20);
            // 
            // label_selected_quantity_1c
            // 
            this.label_selected_quantity_1c.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.label_selected_quantity_1c.Location = new System.Drawing.Point(261, 175);
            this.label_selected_quantity_1c.Name = "label_selected_quantity_1c";
            this.label_selected_quantity_1c.Size = new System.Drawing.Size(50, 20);
            this.label_selected_quantity_1c.Text = "из";
            // 
            // label_description_tovar
            // 
            this.label_description_tovar.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.label_description_tovar.Location = new System.Drawing.Point(5, 200);
            this.label_description_tovar.Name = "label_description_tovar";
            this.label_description_tovar.Size = new System.Drawing.Size(306, 66);
            // 
            // label_selected_quantity_shop
            // 
            this.label_selected_quantity_shop.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.label_selected_quantity_shop.Location = new System.Drawing.Point(176, 175);
            this.label_selected_quantity_shop.Name = "label_selected_quantity_shop";
            this.label_selected_quantity_shop.Size = new System.Drawing.Size(63, 20);
            this.label_selected_quantity_shop.Text = "в магазине";
            this.label_selected_quantity_shop.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // DocumentList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(318, 295);
            this.ControlBox = false;
            this.Controls.Add(this.label_selected_quantity_shop);
            this.Controls.Add(this.label_description_tovar);
            this.Controls.Add(this.label_selected_quantity_1c);
            this.Controls.Add(this.label_price);
            this.Controls.Add(this.txtB_status);
            this.Controls.Add(this.txtB_inputbarcode);
            this.Controls.Add(this.txtB_info_1s);
            this.Controls.Add(this.date);
            this.Controls.Add(this.listView_stroki);
            this.Name = "DocumentList";
            this.Text = "Document";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView_stroki;
        private System.Windows.Forms.DateTimePicker date;
        private System.Windows.Forms.TextBox txtB_info_1s;
        private System.Windows.Forms.TextBox txtB_inputbarcode;
        private System.Windows.Forms.TextBox txtB_status;
        private System.Windows.Forms.Label label_price;
        private System.Windows.Forms.Label label_selected_quantity_1c;
        private System.Windows.Forms.Label label_description_tovar;
        private System.Windows.Forms.Label label_selected_quantity_shop;
    }
}