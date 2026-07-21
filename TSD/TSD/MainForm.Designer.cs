namespace TSD
{
    partial class MainForm
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
            this.btn_inventory = new System.Windows.Forms.Button();
            this.btn_change_data = new System.Windows.Forms.Button();
            this.btn_view_tovar = new System.Windows.Forms.Button();
            this.btn_check_price = new System.Windows.Forms.Button();
            this.btn_goods_receipt = new System.Windows.Forms.Button();
            this.btn_customer_order = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label_powerstatus = new System.Windows.Forms.Label();
            this.btn_setting = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_inventory
            // 
            this.btn_inventory.Location = new System.Drawing.Point(3, 167);
            this.btn_inventory.Name = "btn_inventory";
            this.btn_inventory.Size = new System.Drawing.Size(310, 30);
            this.btn_inventory.TabIndex = 0;
            this.btn_inventory.Text = "(5) Инвентаризация";
            this.btn_inventory.Click += new System.EventHandler(this.btn_inventory_Click);
            // 
            // btn_change_data
            // 
            this.btn_change_data.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_change_data.Location = new System.Drawing.Point(98, 249);
            this.btn_change_data.Name = "btn_change_data";
            this.btn_change_data.Size = new System.Drawing.Size(128, 30);
            this.btn_change_data.TabIndex = 1;
            this.btn_change_data.Text = "(0) Синхронизация";
            this.btn_change_data.Click += new System.EventHandler(this.btn_change_data_Click);
            // 
            // btn_view_tovar
            // 
            this.btn_view_tovar.Location = new System.Drawing.Point(3, 3);
            this.btn_view_tovar.Name = "btn_view_tovar";
            this.btn_view_tovar.Size = new System.Drawing.Size(310, 30);
            this.btn_view_tovar.TabIndex = 4;
            this.btn_view_tovar.Text = "(1) Сбор сроков хранения номенклатуры";
            this.btn_view_tovar.Click += new System.EventHandler(this.btn_view_tovar_Click);
            // 
            // btn_check_price
            // 
            this.btn_check_price.Location = new System.Drawing.Point(3, 44);
            this.btn_check_price.Name = "btn_check_price";
            this.btn_check_price.Size = new System.Drawing.Size(310, 30);
            this.btn_check_price.TabIndex = 5;
            this.btn_check_price.Text = "(2) Проверка/печать ценников";
            this.btn_check_price.Click += new System.EventHandler(this.btn_check_price_Click);
            // 
            // btn_goods_receipt
            // 
            this.btn_goods_receipt.Location = new System.Drawing.Point(3, 85);
            this.btn_goods_receipt.Name = "btn_goods_receipt";
            this.btn_goods_receipt.Size = new System.Drawing.Size(310, 30);
            this.btn_goods_receipt.TabIndex = 6;
            this.btn_goods_receipt.Text = "(3) Поступление товаров";
            this.btn_goods_receipt.Click += new System.EventHandler(this.btn_goods_receipt_Click);
            // 
            // btn_customer_order
            // 
            this.btn_customer_order.Location = new System.Drawing.Point(3, 126);
            this.btn_customer_order.Name = "btn_customer_order";
            this.btn_customer_order.Size = new System.Drawing.Size(310, 30);
            this.btn_customer_order.TabIndex = 7;
            this.btn_customer_order.Text = "(4) Заказ клиента";
            this.btn_customer_order.Click += new System.EventHandler(this.btn_customer_order_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(3, 208);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(310, 30);
            this.button2.TabIndex = 8;
            this.button2.Text = "(6) Собрать по документу";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label_powerstatus
            // 
            this.label_powerstatus.ForeColor = System.Drawing.Color.Red;
            this.label_powerstatus.Location = new System.Drawing.Point(231, 249);
            this.label_powerstatus.Name = "label_powerstatus";
            this.label_powerstatus.Size = new System.Drawing.Size(76, 30);
            // 
            // btn_setting
            // 
            this.btn_setting.Location = new System.Drawing.Point(4, 249);
            this.btn_setting.Name = "btn_setting";
            this.btn_setting.Size = new System.Drawing.Size(92, 29);
            this.btn_setting.TabIndex = 9;
            this.btn_setting.Text = "(7)Настройки";
            this.btn_setting.Click += new System.EventHandler(this.btn_setting_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(318, 295);
            this.ControlBox = false;
            this.Controls.Add(this.btn_setting);
            this.Controls.Add(this.label_powerstatus);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_customer_order);
            this.Controls.Add(this.btn_goods_receipt);
            this.Controls.Add(this.btn_check_price);
            this.Controls.Add(this.btn_view_tovar);
            this.Controls.Add(this.btn_change_data);
            this.Controls.Add(this.btn_inventory);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "ТСД";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_inventory;
        private System.Windows.Forms.Button btn_change_data;
        private System.Windows.Forms.Button btn_view_tovar;
        private System.Windows.Forms.Button btn_check_price;
        private System.Windows.Forms.Button btn_goods_receipt;
        private System.Windows.Forms.Button btn_customer_order;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label_powerstatus;
        private System.Windows.Forms.Button btn_setting;
    }
}

