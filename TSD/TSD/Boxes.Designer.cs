namespace TSD
{
    partial class Boxes
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
                // Освобождение ресурсов, созданных дизайнером
                if (components != null)
                {
                    components.Dispose();
                }

                // Отписка от событий
                this.Load -= Boxes_Load;
                this.txtB_input_barcode.KeyDown -= txtB_input_barcode_KeyDown;

                // Освобождение других ресурсов
                if (index_items != null)
                {
                    index_items.Clear();
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
            this.listView_boxes = new System.Windows.Forms.ListView();
            this.txtB_input_barcode = new System.Windows.Forms.TextBox();
            this.txtB_info_on_doc = new System.Windows.Forms.TextBox();
            this.label_код_шк = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listView_boxes
            // 
            this.listView_boxes.Location = new System.Drawing.Point(0, 36);
            this.listView_boxes.Name = "listView_boxes";
            this.listView_boxes.Size = new System.Drawing.Size(318, 214);
            this.listView_boxes.TabIndex = 0;
            // 
            // txtB_input_barcode
            // 
            this.txtB_input_barcode.Location = new System.Drawing.Point(77, 269);
            this.txtB_input_barcode.MaxLength = 13;
            this.txtB_input_barcode.Name = "txtB_input_barcode";
            this.txtB_input_barcode.Size = new System.Drawing.Size(100, 23);
            this.txtB_input_barcode.TabIndex = 1;
            // 
            // txtB_info_on_doc
            // 
            this.txtB_info_on_doc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtB_info_on_doc.Enabled = false;
            this.txtB_info_on_doc.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.txtB_info_on_doc.Location = new System.Drawing.Point(4, 4);
            this.txtB_info_on_doc.Name = "txtB_info_on_doc";
            this.txtB_info_on_doc.Size = new System.Drawing.Size(311, 23);
            this.txtB_info_on_doc.TabIndex = 2;
            // 
            // label_код_шк
            // 
            this.label_код_шк.Location = new System.Drawing.Point(5, 270);
            this.label_код_шк.Name = "label_код_шк";
            this.label_код_шк.Size = new System.Drawing.Size(67, 20);
            this.label_код_шк.Text = "Код / ШК";
            // 
            // Boxes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(318, 295);
            this.ControlBox = false;
            this.Controls.Add(this.label_код_шк);
            this.Controls.Add(this.txtB_info_on_doc);
            this.Controls.Add(this.txtB_input_barcode);
            this.Controls.Add(this.listView_boxes);
            this.Name = "Boxes";
            this.Text = "Коробки";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView_boxes;
        private System.Windows.Forms.TextBox txtB_input_barcode;
        private System.Windows.Forms.TextBox txtB_info_on_doc;
        private System.Windows.Forms.Label label_код_шк;
    }
}