using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace TSD
{
    public partial class Boxes : Form
    {
        
        public string typ_doc = "";
        public int its_new = 0;
        public string guid = "";
        public string status = "";
        public string info1c = "";
        private bool show_all_stroki = true;
        private Dictionary<string, int> index_items = new Dictionary<string, int>();

        
        public Boxes()
        {            
            InitializeComponent();
            this.KeyPreview = true;
            this.Load += new EventHandler(Boxes_Load);
            this.txtB_input_barcode.KeyDown += new KeyEventHandler(txtB_input_barcode_KeyDown);
            //Program.write_log("Start Boxes");

        }      

        private void txtB_input_barcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //if (txtB_input_barcode.Text.Length < 10)
                //{ 
                //    return;
                //}
                txtB_input_barcode.Text = transform(txtB_input_barcode.Text);
                listView_boxes.Focus();
                int index = 0;
                //int find = 0; int index = 0;
                //if (txtB_input_barcode.Text.Trim().Length != 0)
                //{
                //    foreach (ListViewItem item in listView_boxes.Items)
                //    {
                //        if (item.SubItems[3].Text.Trim() == txtB_input_barcode.Text)
                //        {
                //            find = 1;
                //            listView_boxes.Items[index].Selected = true;
                //            listView_boxes.Items[index].Focused = true;
                //            listView_boxes.EnsureVisible(index);
                //        }
                //        index++;
                //    }
                //}

                try
                {
                    index = index_items[txtB_input_barcode.Text.Trim()];
                    listView_boxes.Items[index].Selected = true;
                    listView_boxes.Items[index].Focused = true;
                    listView_boxes.EnsureVisible(index);                    
                }
                catch (KeyNotFoundException)
                {
                    MessageBox.Show("Не найдена");
                }

                //if (find == 0)
                //{
                //    MessageBox.Show("Не найдена");
                //}
                e.Handled = true;
            }
            else
            {
                txtB_input_barcode.Text = txtB_input_barcode.Text.Replace("Z", "");
            }           
        }


        private string transform(string source)
        {
            string answer = "";
            int divider = source.IndexOf("#");
            if (divider == -1)
            {
                divider = source.IndexOf("-");
            }

            if (divider > 0)
            {
                string series = source.Substring(0, divider);
                string source_string = source.Substring(divider + 1, source.Length - 1 - divider);
                double dec_number = Convert.ToInt64(source_string, 16);
                answer = series + dec_number.ToString().PadLeft(10 - series.Length, '0');
            }

            return answer;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {            
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if ((e.KeyCode == Keys.F14) || (e.KeyCode == Keys.F15))
            {
                //if (show_all_stroki)
                //{
                //    show_all_stroki = false;
                //}
                //else
                //{
                //    show_all_stroki = true;
                //}
                show_all_stroki = !show_all_stroki;
                load_boxes(0);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (its_new==0)
                {
                    if (listView_boxes.SelectedIndices.Count == 0)
                    {
                        MessageBox.Show("Выберите коробку");
                        return;
                    }
                }
                if (!listView_boxes.Focused)
                {
                    return;
                }
                SelectButtonBox sb = new SelectButtonBox();
                if (its_new == 0)
                {
                    sb.status_box = listView_boxes.Items[listView_boxes.SelectedIndices[0]].SubItems[0].Text;
                }
                else
                {
                    sb.status_box = "т";
                }
                DialogResult dr = sb.ShowDialog();
                if (dr == DialogResult.Yes)
                {
                    //int index = -1;
                    if ((its_new>0)&&(listView_boxes.Items.Count==0))
                    {
                        btn_acceptance_Click(null, null);
                        return;
                    }

                    int index = listView_boxes.SelectedIndices[0];
                    if (sb.select_choise == 1)
                    {
                        btn_acceptance_Click(null, null);
                    }
                    else if (sb.select_choise == 2)
                    {
                        btn_show_divergence_Click(null, null);
                    }
                    else if (sb.select_choise == 3)
                    {
                        index = listView_boxes.SelectedIndices[0];
                        btn_without_control_Click(null, null);
                        //listView_boxes.Focus();
                        //listView_boxes.Items[index].Selected = true;
                        //listView_boxes.Items[index].Focused = true;
                    }
                    else if (sb.select_choise == 7)
                    {
                        index = listView_boxes.SelectedIndices[0];
                        btn_cansel_box_Click(null, null);
                        //listView_boxes.Focus();
                        //listView_boxes.Items[index].Selected = true;
                        //listView_boxes.Items[index].Focused = true;
                    }
                    load_boxes(index);
                }
                sb.Dispose();
            }
            //else if (e.KeyCode == Keys.D1)
            //{                
            //    btn_acceptance_Click(null, null);
            //}
            //else if (e.KeyCode == Keys.D3)
            //{
            //    int index = listView_boxes.SelectedIndices[0];
            //    btn_without_control_Click(null, null);                
            //    listView_boxes.Items[index].Selected = true;
            //    listView_boxes.Items[index].Focused = true; 
            //}
            //else if (e.KeyCode == Keys.D7)
            //{
            //    int index = listView_boxes.SelectedIndices[0];
            //    btn_cansel_box_Click(null, null);
            //    listView_boxes.Items[index].Selected = true;
            //    listView_boxes.Items[index].Focused = true; 
            //}
            else if (e.KeyCode == Keys.Z)
            {
                listView_boxes.Focus();
                txtB_input_barcode.Focus();
                //write_record();
                txtB_input_barcode.Text = "";
                txtB_input_barcode.Focus();
                e.Handled = true;
            }
        }



        private void Boxes_Load(object sender, EventArgs e)
        {
            // Set the view to show details.
            listView_boxes.View = View.Details;

            //listView_boxes.AllowColumnReorder = false;

            // Select the item and subitems when selection is made.
            listView_boxes.FullRowSelect = true;
                        
            listView_boxes.Columns.Add("С",30, HorizontalAlignment.Left);
            listView_boxes.Columns.Add("Ф", 30, HorizontalAlignment.Right);
            listView_boxes.Columns.Add("П", 30, HorizontalAlignment.Right);
            listView_boxes.Columns.Add("ШК коробки", 150, HorizontalAlignment.Left);
            load_boxes(0);
            if ((its_new > 0) && (listView_boxes.Items.Count == 0))
            {
                btn_acceptance_Click(null, null);
                return;
            }
            load_boxes(0);
        }

        private void load_boxes(int index)
        {
            listView_boxes.Items.Clear();

            string dop_query = "";
            if (!show_all_stroki)
            {
                this.Text += "  Расхождения ";//"  расхождения ) верх. кнопки ";
                dop_query = " WHERE dt_box.quantity <> dt_box.quantity_shop ";
            }
            else
            {
                this.Text = "Коробки";
            }


            using (SQLiteConnection conn = Program.ConnectForDataBase())
            {

                try
                {
                    conn.Open();
                    //string query = " SELECT SUM(quantity) AS quantity,SUM(quantity_shop) AS quantity_shop,box,MAX(box_status) AS box_status FROM dt WHERE guid='" + guid + "'" + dop_query + " GROUP BY box";
                    string query = " SELECT * FROM (SELECT SUM(quantity) AS quantity,SUM(quantity_shop) AS quantity_shop,box,MAX(box_status) AS box_status FROM dt WHERE guid='" + guid + "' GROUP BY box) AS dt_box " + dop_query;
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            int index_listviw_item = 0; index_items.Clear();
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["box_status"].ToString());
                                item.SubItems.Add(reader["quantity_shop"].ToString());
                                item.SubItems.Add(reader["quantity"].ToString());
                                //item.SubItems.Add("");
                                item.SubItems.Add(reader["box"].ToString());
                                listView_boxes.Items.Add(item);

                                index_items.Add(reader["box"].ToString(), index_listviw_item);
                                index_listviw_item++;
                            }
                            //reader.Close();
                            //command.Dispose();
                            //conn.Close();
                            listView_boxes.Focus();
                            if (listView_boxes.Items.Count > 0)
                            {
                                listView_boxes.Items[index].Selected = true;
                                listView_boxes.Items[index].Focused = true;
                                listView_boxes.EnsureVisible(index);
                            }
                            if ((this.its_new > 0) && (listView_boxes.Items.Count == 0))
                            {
                                ListViewItem item = new ListViewItem("1");//
                                item.SubItems.Add("0");
                                item.SubItems.Add("0");
                                item.SubItems.Add("Самопал");
                                listView_boxes.Items.Add(item);
                                index_items.Add("Самопал", 0);
                            }
                            txtB_info_on_doc.Text = status + " " + listView_boxes.Items[index].SubItems[1].Text + " из " + listView_boxes.Items[index].SubItems[2].Text + ". " + info1c;
                        }
                    }

                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, "load_boxes");
                    //result = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "load_boxes");
                    //result = false;
                }            
            }
            //finally
            //{
            //    if (conn.State == ConnectionState.Open)
            //    {
            //        conn.Close();
            //    }
            //    conn.Dispose();
            //}           
        }        
        
        private void btn_acceptance_Click(object sender, EventArgs e)
        {
            using (WorkWithBarcode wb = new WorkWithBarcode())
            {
                if (its_new == 0)
                {
                    wb.num_box = listView_boxes.Items[listView_boxes.SelectedIndices[0]].SubItems[3].Text;
                }
                else
                {
                    wb.num_box = "0";
                }
                wb.guid = guid;
                wb.typ_doc = this.typ_doc;
                wb.its_new = this.its_new;
                if (listView_boxes.Items.Count > 0)
                {
                    if (listView_boxes.Items[listView_boxes.SelectedIndices[0]].SubItems[0].Text == "б")
                    {
                        delete_status();
                    }
                }               
                wb.ShowDialog();
            }

            this.TopMost = true;
            this.Activate();
            this.Refresh();

            //wb.Dispose();
            //wb = null;
            //Program.write_log("Close WorkWithBarcode");
            //GC.Collect();
            //GC.SuppressFinalize(wb);
            //GC.WaitForPendingFinalizers();
            //wb = null;
            if (listView_boxes.Items.Count == 0)
            {
                load_boxes(0);
            }
        }

        private void btn_cansel_box_Click(object sender, EventArgs e)
        {
            //DialogResult dr = MessageBox.Show("Уверены ?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);

            //if (dr != DialogResult.No)
            //{
                delete_status();
            //}           
        }

        //private void delete_status()
        //{
        //    SQLiteConnection conn = Program.ConnectForDataBase();

        //    try
        //    {
        //        conn.Open();
        //        string num_box = listView_boxes.Items[listView_boxes.SelectedIndices[0]].SubItems[3].Text;
        //        string query = " UPDATE dt SET box_status='',quantity_shop=0  WHERE guid='" + guid + "' AND box ='" + num_box.Trim() + "'";
        //        SQLiteCommand command = new SQLiteCommand(query, conn);
        //        command.ExecuteNonQuery();
        //        command.Dispose();
        //        conn.Close();
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //        conn.Dispose();
        //    }
        //}

        private void delete_status()
        {
            using (SQLiteConnection conn = Program.ConnectForDataBase())
            {
                try
                {
                    conn.Open();
                    string num_box = listView_boxes.Items[listView_boxes.SelectedIndices[0]].SubItems[3].Text;
                    string query = "UPDATE dt SET box_status='', quantity_shop=0 WHERE guid=@guid AND box=@box";
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@guid", guid);
                        command.Parameters.AddWithValue("@box", num_box.Trim());
                        command.ExecuteNonQuery();
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
        }

        private void btn_without_control_Click(object sender, EventArgs e)
        {
            //здесь проверка если идет принятие по товарно тогда нельзя принять коробкой
            if (listView_boxes.Items[listView_boxes.SelectedIndices[0]].SubItems[0].Text == "т")
            {
                MessageBox.Show("Действие невозможно. Уже начата приёмка по товарам. Продолжайте принимать по товарам или отменяйте всё принятое в коробке");
                return;
            }

            using (SQLiteConnection conn = Program.ConnectForDataBase())
            {
                try
                {
                    conn.Open();
                    string num_box = listView_boxes.Items[listView_boxes.SelectedIndices[0]].SubItems[3].Text;
                    string query = " UPDATE dt SET box_status='б',quantity_shop=quantity  WHERE guid='" + guid + "' AND box ='" + num_box.Trim() + "'";
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        command.ExecuteNonQuery();
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
        }
       
        private void btn_show_divergence_Click(object sender, EventArgs e)
        {
            using (DocumentList doc = new DocumentList())
            {
                doc.guid = guid;
                doc.num_box = listView_boxes.Items[listView_boxes.SelectedIndices[0]].SubItems[3].Text;                
                doc.its_new = false;
                doc.typ_doc = this.typ_doc;
                doc.TopMost = true;
                doc.ShowDialog();            
            }           
        }   
    }
}