namespace TSD
{
    partial class JoutnalInventory
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
            this.listView_inventory = new System.Windows.Forms.ListView();
            this.label_comment = new System.Windows.Forms.Label();
            this.btn_new_document = new System.Windows.Forms.Button();
            this.txtB_selection_text = new System.Windows.Forms.TextBox();
            this.btn_selection = new System.Windows.Forms.Button();
            this.btn_reload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView_inventory
            // 
            this.listView_inventory.Location = new System.Drawing.Point(3, 29);
            this.listView_inventory.Name = "listView_inventory";
            this.listView_inventory.Size = new System.Drawing.Size(312, 180);
            this.listView_inventory.TabIndex = 0;
            // 
            // label_comment
            // 
            this.label_comment.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.label_comment.Location = new System.Drawing.Point(4, 212);
            this.label_comment.Name = "label_comment";
            this.label_comment.Size = new System.Drawing.Size(311, 55);
            // 
            // btn_new_document
            // 
            this.btn_new_document.Location = new System.Drawing.Point(4, 270);
            this.btn_new_document.Name = "btn_new_document";
            this.btn_new_document.Size = new System.Drawing.Size(74, 22);
            this.btn_new_document.TabIndex = 1;
            this.btn_new_document.Text = "Новый";
            this.btn_new_document.Visible = false;
            this.btn_new_document.Click += new System.EventHandler(this.btn_new_document_Click);
            // 
            // txtB_selection_text
            // 
            this.txtB_selection_text.Location = new System.Drawing.Point(3, 4);
            this.txtB_selection_text.MaxLength = 100;
            this.txtB_selection_text.Name = "txtB_selection_text";
            this.txtB_selection_text.Size = new System.Drawing.Size(276, 23);
            this.txtB_selection_text.TabIndex = 3;
            // 
            // btn_selection
            // 
            this.btn_selection.Location = new System.Drawing.Point(285, 4);
            this.btn_selection.Name = "btn_selection";
            this.btn_selection.Size = new System.Drawing.Size(29, 24);
            this.btn_selection.TabIndex = 4;
            this.btn_selection.Click += new System.EventHandler(this.btn_selection_Click);
            // 
            // btn_reload
            // 
            this.btn_reload.Location = new System.Drawing.Point(85, 270);
            this.btn_reload.Name = "btn_reload";
            this.btn_reload.Size = new System.Drawing.Size(229, 25);
            this.btn_reload.TabIndex = 6;
            this.btn_reload.Text = "Выг. повт. док. сроки с 1 июля";
            this.btn_reload.Visible = false;
            this.btn_reload.Click += new System.EventHandler(this.btn_reload_Click);
            // 
            // JoutnalInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(318, 295);
            this.ControlBox = false;
            this.Controls.Add(this.btn_reload);
            this.Controls.Add(this.btn_selection);
            this.Controls.Add(this.txtB_selection_text);
            this.Controls.Add(this.btn_new_document);
            this.Controls.Add(this.label_comment);
            this.Controls.Add(this.listView_inventory);
            this.MinimizeBox = false;
            this.Name = "JoutnalInventory";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView_inventory;
        private System.Windows.Forms.Label label_comment;
        private System.Windows.Forms.Button btn_new_document;
        private System.Windows.Forms.TextBox txtB_selection_text;
        private System.Windows.Forms.Button btn_selection;
        private System.Windows.Forms.Button btn_reload;
    }
}