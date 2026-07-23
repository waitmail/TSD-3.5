using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace TSD
{
    public partial class JoutnalInventory : Form
    {
        public int typ_doc = 0;


        public JoutnalInventory()
        {
            InitializeComponent();
            this.Load += new EventHandler(JoutnalInventory_Load);
            this.KeyPreview = true;
            this.listView_inventory.SelectedIndexChanged += new EventHandler(listView_inventory_SelectedIndexChanged);
        }

        private void listView_inventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_inventory.Items.Count > 0)
            {
                if (listView_inventory.SelectedIndices.Count > 0)
                {
                    label_comment.Text = listView_inventory.Items[listView_inventory.SelectedIndices[0]].SubItems[4].Text;
                }
            }
        }
        
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (listView_inventory.Items.Count > 0)
                {
                    btn_select_Click(null, null);
                }
            }
            //else if (e.KeyCode == Keys.D0)
            //{
            //    //btn_new_document_Click(null, null);
            //}
            //else if (e.KeyCode == Keys.D1)
            //{
            //    load_documents();
            //}
            else if (e.KeyCode == Keys.F14)
            {
                txtB_selection_text.Focus();
                txtB_selection_text.SelectAll();
            }
            else if (e.KeyCode == Keys.F15)
            {
                load_documents();
                if (listView_inventory.Items.Count>0)
                {
                    listView_inventory.Focus();
                    listView_inventory.Items[0].Selected = true;
                    listView_inventory.Items[0].Focused = true;
                }

            }
        }

        private void JoutnalInventory_Load(object sender, EventArgs e)
        {
            // Set the view to show details.
            listView_inventory.View = View.Details;

            //listView_inventory.AllowColumnReorder = false;

            // Select the item and subitems when selection is made.
            listView_inventory.FullRowSelect = true;

            // Display grid lines.
            //listView_inventory.GridLines = true;

            // Sort the items in the list in ascending order.
            //listView1.Sorting = SortOrder.Ascending;            
            listView_inventory.Columns.Add("Статус", 20, HorizontalAlignment.Left);
            //if (typ_doc != 4)
            //{
            listView_inventory.Columns.Add("Дата", 70, HorizontalAlignment.Left);
            listView_inventory.Columns.Add("П", 30, HorizontalAlignment.Left);
            listView_inventory.Columns.Add("Ф", 30, HorizontalAlignment.Left);
            //}
            listView_inventory.Columns.Add("Комментарий", 200, HorizontalAlignment.Left);
            load_documents();
            if (typ_doc == 1)
            {
                this.Text = " Инвентаризации ";
            }
            else if (typ_doc == 2)
            {
                this.Text = " Приходные накладные ";
                btn_new_document.Visible = true;
            }
            else if (typ_doc == 3)
            {
                this.Text = " Заказы клиентов ";
                btn_new_document.Visible = true;
            }
            else if (typ_doc == 4)
            {
                this.Text = " Проверка и печать ценников ";
                btn_new_document.Visible = true;
                this.Controls.Remove(this.txtB_selection_text);
                this.Controls.Remove(this.btn_selection);
                this.Controls.Remove(this.label_comment);
                this.listView_inventory.Location = new System.Drawing.Point(3, 3);                
                this.listView_inventory.Size = new System.Drawing.Size(312, 265);                
            }
            else if (typ_doc == 5)
            {
                this.Text = "  Сбор сроков хранения номенклатуры ";
                btn_new_document.Visible = true;
                this.Controls.Remove(this.txtB_selection_text);
                this.Controls.Remove(this.btn_selection);
                this.Controls.Remove(this.label_comment);
                this.listView_inventory.Location = new System.Drawing.Point(3, 3);                
                this.listView_inventory.Size = new System.Drawing.Size(312, 265);
                btn_reload.Visible = true;
            }

        }

        private void get_zagolovok()
        {
            if (typ_doc == 1)
            {
                if (txtB_selection_text.Text.Trim().Length > 0)
                {
                    this.Text = "ОТБОР Инвентаризации ";
                }
                else
                {
                    this.Text = " Инвентаризации ";
                }
            }
            else if (typ_doc == 2)
            {
                if (txtB_selection_text.Text.Trim().Length > 0)
                {
                    this.Text = " ОТБОР Приходные накладные ";
                }
                else
                {
                    this.Text = " Приходные накладные ";
                }
            }
            else if (typ_doc == 3)
            {
                if (txtB_selection_text.Text.Trim().Length > 0)
                {
                    this.Text = " ОТБОР Заказы клиентов ";
                }
                else
                {
                    this.Text = " Заказы клиентов ";
                }
            }
            else if (typ_doc == 4)
            {
                this.Text = " Проверка и печать ценников "; 
            }
            else if (typ_doc == 5)
            {
                this.Text = "  Сбор сроков хранения номенклатуры ";
            }
        }
        
        private void load_documents()
        {
            using (SQLiteConnection conn = Program.ConnectForDataBase())
            {
                listView_inventory.Items.Clear();

                try
                {
                    conn.Open();
                    string query = "";//
                    if (txtB_selection_text.Text.Trim().Length > 0)
                    {
                        query = "SELECT  status ,date,guid,info_1s FROM dh where status<3 AND type=" + typ_doc.ToString() + " AND info_1s LIKE '%" + txtB_selection_text.Text.Trim() + "%' order by date ";//
                    }
                    else
                    {
                        query = "SELECT  status ,date,guid,info_1s FROM dh where status<3 AND type=" + typ_doc.ToString() + " order by date ";//
                    }

                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string _status = reader["status"].ToString();
                                if (_status == "0")
                                {
                                    _status = "Н";
                                }
                                else if (_status == "1")
                                {
                                    _status = "Р";
                                }
                                else if (_status == "2")
                                {
                                    _status = "З";
                                }
                                else if (_status == "3")
                                {
                                    _status = "ПЦ";
                                }
                                ListViewItem item = new ListViewItem(_status);
                                item.SubItems.Add(reader.GetDateTime(1).ToString("dd.MM.yyyy"));

                                int[] boxes_info = calculate_boxes(reader["guid"].ToString());
                                item.SubItems.Add(boxes_info[0].ToString());
                                item.SubItems.Add(boxes_info[1].ToString());

                                item.Tag = reader["guid"].ToString();
                                item.SubItems.Add(reader["info_1s"].ToString());
                                listView_inventory.Items.Add(item);
                            }
                        }
                    }

                    if (listView_inventory.Items.Count > 0)
                    {
                        listView_inventory.Items[0].Selected = true;
                        listView_inventory.Items[0].Focused = true;
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            //finally
            //{
            //    if (conn.State == ConnectionState.Open)
            //    {
            //        conn.Close();
            //        conn.Dispose();
            //    }
            //} 
        }

        /// <summary>
        /// первый элемент массива всего коробок
        /// второй элемент массива обработкно
        /// или в обработке коробок
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        private int[] calculate_boxes(string guid)
        {
            int[] result = new int[2];
            using (SQLiteConnection conn = Program.ConnectForDataBase())
            {

                int boxes_total = 0;
                int boxes_in_processing = 0;
                try
                {
                    conn.Open();
                    string query = "SELECT box,MAX(box_status) AS box_status FROM dt where guid='" + guid + "' GROUP BY box ";
                    SQLiteCommand command = new SQLiteCommand(query, conn);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            boxes_total++;
                            if (reader["box_status"].ToString() != "")
                            {
                                boxes_in_processing++;
                            }
                        }
                        result[0] = boxes_total;
                        result[1] = boxes_in_processing;
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            //finally
            //{
            //    if (conn.State == ConnectionState.Open)
            //    {
            //        conn.Close();
            //    }
            //} 

            return result; 
        }


        private void btn_Esc_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_open_document_Click(object sender, EventArgs e)
        {
            if (listView_inventory.Items.Count == 0)
            {
                return;
            }
            int index = listView_inventory.SelectedIndices[0];
            string guid = listView_inventory.Items[listView_inventory.SelectedIndices[0]].Tag.ToString();
            if (guid != "")
            {
                using (DocumentList doc = new DocumentList())
                {
                    doc.guid = guid;
                    doc.typ_doc = typ_doc.ToString();// listView_inventory.FocusedItem.SubItems[1].Text;
                    doc.its_new = false;
                    doc.ShowDialog();
                }
                listView_inventory.Items.Clear();
                load_documents();
                //Позиционирование на ранее выделеном/активном документе
                listView_inventory.Items[index].Selected = true;
                listView_inventory.Items[index].Focused = true;
                listView_inventory.EnsureVisible(index);
            }
        }


        private void btn_select_Click(object sender, EventArgs e)
        {
            if (listView_inventory.Items.Count == 0)
            {
                return;
            }
            int index = listView_inventory.SelectedIndices[0];
            string guid = listView_inventory.Items[listView_inventory.SelectedIndices[0]].Tag.ToString();
            if (guid != "")
            {
                using (DocumentInventory1 d1 = new DocumentInventory1())
                {
                    d1.guid = guid;
                    d1.typ_doc = typ_doc.ToString();// listView_inventory.FocusedItem.SubItems[1].Text;
                    //d1.its_new = false;
                    d1.status = listView_inventory.Items[listView_inventory.SelectedIndices[0]].SubItems[0].Text;
                    d1.info1c = label_comment.Text;
                    d1.ShowDialog();
                }
                listView_inventory.Items.Clear();
                load_documents();
                if ((listView_inventory.Items.Count >= index)&&(listView_inventory.Items.Count>0))
                {
                    listView_inventory.Items[index].Selected = true;
                    listView_inventory.Items[index].Focused = true;
                    listView_inventory.EnsureVisible(index);
                }
            }
        }


        private void btn_new_document_Click(object sender, EventArgs e)
        {
            //using (InputCommentNewDocument inptd = new InputCommentNewDocument())
            //{
            //    if (inptd.ShowDialog() == DialogResult.Cancel)
            //    {
            //        return;
            //    }
            //}

            string new_guid = "";
            using (DocumentInventory1 doc = new DocumentInventory1())
            {
                string inptd_txtb_description_Text = "";

                using (InputCommentNewDocument inptd = new InputCommentNewDocument())
                {
                    if (inptd.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }
                    inptd_txtb_description_Text = inptd.txtb_description.Text; 
                }
                new_guid = Guid.NewGuid().ToString();
                doc.typ_doc = this.typ_doc.ToString();
                doc.guid = new_guid;
                doc.its_new = 1;
                if (!doc.write_new_document(inptd_txtb_description_Text))
                {
                    MessageBox.Show(" Не удалось записать новый документ ");
                    return;
                }

                doc.ShowDialog();
            }
            
            listView_inventory.Items.Clear();
            load_documents();
            //позиционирование на новой позиции
            int index = 0; bool finded = false;
            for (int i = 0; i < listView_inventory.Items.Count; i++)
            {
                if (listView_inventory.Items[i].Tag.ToString() == new_guid)
                {
                    index = i;
                    finded = true;
                    break;
                }
            }
            if (finded)
            {
                listView_inventory.Items[index].Selected = true;
                listView_inventory.Items[index].Focused = true;
                listView_inventory.EnsureVisible(index);
            }
            //btn_select_Click(null, null);
        }

        private void btn_selection_Click(object sender, EventArgs e)
        {
            load_documents();
        }

        private void btn_shelf_life_Click(object sender, EventArgs e)
        {

        }

        private void btn_reload_Click(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection conn = Program.ConnectForDataBase())
                {
                    conn.Open();
                    // Меняем статус 3 (отправлен) на 2 (готов к отправке) для документов сбора сроков (type=5) с 1 июля
                    string query = "UPDATE dh SET status=2 WHERE type=5 AND status=3 AND date >= '2024-07-01'";
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        int rows = command.ExecuteNonQuery();
                        MessageBox.Show("Готово! Обновлено документов: " + rows.ToString());
                    }
                }
                load_documents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }       
    }
}