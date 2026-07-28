using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TSD
{
    public partial class DocumentList : Form
    {

        public string guid = "";
        public bool its_new = false;
        public string typ_doc = "";
        public string label_decription_document = ""; 
        private int index_position=0;
        private DateTime datetime_select_position = DateTime.Now;
        //private bool listView_stroki_change_pos = false;
        private int display_quantity = 0;
        //private bool show_all_stroki = true;
        private bool show_all_stroki = false;
        //string comment = "";
        public string num_box = "";

        public DocumentList()
        {
            InitializeComponent();
            this.Load += new EventHandler(Document_Load);
            this.txtB_inputbarcode.KeyPress += new KeyPressEventHandler(txtB_inputbarcode_KeyPress);
            this.KeyPreview = true;
            this.txtB_status.KeyDown += new KeyEventHandler(txtB_status_KeyDown);
            this.listView_stroki.SelectedIndexChanged += new EventHandler(listView_stroki_SelectedIndexChanged);
            this.listView_stroki.KeyDown += new KeyEventHandler(listView_stroki_KeyDown); 
            
        }

        /// <summary>
        /// kolstr количество позиций на которое необходимо прокрутить экран
        /// direction направление true вперед иначе назад
        /// </summary>
        /// <param name="kolstr"></param>
        /// <param name="direction"></param>
        private void listView_stroki_repaint(int kolstr, bool direction)
        {
            int position = listView_stroki.SelectedIndices[0];
            int count    = listView_stroki.Items.Count;
            int index    = 0;

            //получить возможное количество позиций на которое можем переместится
            if (direction)
            {
                if (position + kolstr < count)
                {
                    index = position + kolstr;
                }
                else
                {
                    index = count - 1;
                }
            }
            else
            {
                if (position - kolstr >0)
                {
                    index = position - kolstr;
                }
                else
                {
                    index = 0;
                } 
            }

            listView_stroki.Focus();
            listView_stroki.Items[index].Focused = true;            
            listView_stroki.Items[index].Selected = true;            
            listView_stroki.EnsureVisible(index);
            
            index_position = index;
            datetime_select_position = DateTime.Now;
             
        }

        private string get_characteristic_guid(string tovar_code, string characteristic_name)
        {
            string result = "";
            using (SQLiteConnection conn = Program.ConnectForDataBase())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT guid FROM characteristic WHERE tovar_code=@tovar_code AND name=@name";
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@tovar_code", tovar_code);
                        command.Parameters.AddWithValue("@name", characteristic_name.Trim());
                        object result_query = command.ExecuteScalar();
                        if (result_query != null)
                        {
                            result = result_query.ToString();
                        }
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
            return result;
        }

        /// <summary>
        /// При поиске строки в листвью в режиме показа
        /// расхождений, если строки не найдены 
        /// необходимо поискать в табличной части документа
        /// и если там есть такие строки то добавить в 
        /// листвью и спозиционироваться на этой строке
        /// 
        /// </summary>
        /// <param name="barcode"></param>
        private int add_string_listview(string barcode)
        {
            int result = 0;
            SQLiteConnection conn = Program.ConnectForDataBase();
            try
            {
                conn.Open();
                string query = "";
                if (barcode.Length < 7)
                {
                    query = "SELECT code FROM tovar where code=@barcode ";
                }
                else
                {
                    query = "SELECT barcodes.tovar_code FROM barcodes where barcodes.barcode_code=@barcode ";
                }
                SQLiteParameter _barcode = new SQLiteParameter("barcode", barcode);
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.Add(_barcode);
                object result_query = command.ExecuteScalar();
                if (result_query != null)//такой товар найден
                {
                    query = "SELECT dt.tovar_code, dt.quantity,dt.quantity_shop,dt.price_buy,dt.price," +
                    " tovar.name AS tovar_name,characteristic.guid AS characteristic_guid," +
                    " characteristic.name AS characteristic_name,dt.line_number FROM dt " +
                    " LEFT JOIN tovar ON dt.tovar_code = tovar.code " +
                    " LEFT JOIN characteristic ON dt.characteristic = characteristic.guid " +
                    " where dt.guid=@guid AND dt.tovar_code=@tovar_code order by dt.line_number";                    
                    command = new SQLiteCommand(query, conn);
                    SQLiteParameter _guid = new SQLiteParameter("guid", guid);
                    SQLiteParameter _tovar_code = new SQLiteParameter("tovar_code", result_query.ToString());                    
                    command.Parameters.Add(_guid);
                    command.Parameters.Add(_tovar_code);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        result = 1;
                        ListViewItem item = new ListViewItem(reader["line_number"].ToString());                        
                        item.SubItems.Add(reader["quantity_shop"].ToString());
                        if (display_quantity == 1)
                        {
                            item.SubItems.Add(reader["quantity"].ToString());
                        }
                        item.SubItems.Add(reader["tovar_name"].ToString());
                        item.SubItems.Add(reader["characteristic_name"].ToString());                        
                        item.SubItems.Add(reader["price_buy"].ToString());                        
                        item.Tag = reader["tovar_code"].ToString();
                        listView_stroki.Items.Add(item); 
                    }
                    //спозиционируемся на последней строке
                    listView_stroki.Items[listView_stroki.Items.Count-1].Selected = true;
                    listView_stroki.Items[listView_stroki.Items.Count - 1].Focused = true;
                    listView_stroki.EnsureVisible(listView_stroki.Items.Count - 1);
                }
                conn.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return result;

        }

        private string get_barcode(string tovar_code)
        {
            string result = "";
            SQLiteConnection conn = Program.ConnectForDataBase();
            try
            {
                conn.Open();
                string query = "SELECT barcode FROM barcodes WHERE tovar_code=" + tovar_code;
                SQLiteCommand command = new SQLiteCommand(query, conn);
                object result_query = command.ExecuteScalar();
                if (result_query != null)
                {
                    result = result_query.ToString();
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
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return result;
        }

        private void listView_stroki_KeyDown(object sender, KeyEventArgs e)
        {
            if (listView_stroki.SelectedIndices.Count > 0)
            {

                if (e.KeyCode == Keys.Enter)
                {
                    if ((typ_doc != "4") && (typ_doc != "5"))
                    {
                        if (num_box == "")
                        {
                            MessageBox.Show("Доступно только из коробки");
                            return;
                        }
                    }
                    //return;//пока что отбой 
                    if ((typ_doc != "4") && (typ_doc != "5"))
                    {
                        WorkWithBarcode wb = new WorkWithBarcode();
                        ListViewItem lvi = listView_stroki.Items[listView_stroki.SelectedIndices[0]];
                        string tovar_code = lvi.Tag.ToString();
                        //find_barcode_or_code_in_tovar(tovar_code);//Непонятно зачем это здесь 
                        wb.txtB_tovar.Text = tovar_code + "," + listView_stroki.Items[lvi.Index].SubItems[3].Text + ";" +
                        listView_stroki.Items[lvi.Index].SubItems[4].Text;
                        //wb.label_decription_document.Text = this.label_decription_document;
                        wb.guid = guid;
                        //wb.txtB_input_barcode.Text = get_barcode(tovar_code);
                        wb.tovar_code = tovar_code;
                        //Получить guid характеристики
                        string characteristic_name = listView_stroki.Items[lvi.Index].SubItems[2].Text.Trim();
                        if (characteristic_name.Length != 0)
                        {
                            wb.characteristic_guid = get_characteristic_guid(tovar_code, characteristic_name);
                        }
                        wb.txtB_quantity.Text = "1";
                        wb.open_from_document_list = true;

                        if (display_quantity == 1)
                        {
                            wb.label_количество_в_1с.Text = listView_stroki.Items[lvi.Index].SubItems[2].Text.Trim();
                        }
                        wb.label_количество_в_магазине.Text = " в документе " + listView_stroki.Items[lvi.Index].SubItems[1].Text.Trim();
                        string num_str = lvi.SubItems[0].Text;
                        //wb.display_quantity = display_quantity;
                        wb.close_this_form = true;
                        //wb.status = txtB_status.Text;
                        if (num_box != "")
                        {
                            wb.num_box = num_box;
                        }
                        wb.ShowDialog();
                        //переррисовать список и спозиционироваться на старой строке 
                        load_stroki();
                        //
                        int index = 0;
                        foreach (ListViewItem item in listView_stroki.Items)
                        {
                            //if (item.Tag.ToString() == tovar_code.ToString())
                            //{
                            //    index = item.Index;
                            //    break;
                            //}
                            if (item.SubItems[0].Text.Trim() == num_str.Trim())
                            {
                                index = item.Index;
                                break;
                            }
                        }
                        //int index = listView_stroki.Items.IndexOf(lvi);
                        listView_stroki.Items[index].Selected = true;
                        listView_stroki.Items[index].Focused = true;
                        listView_stroki.EnsureVisible(index);
                    }
                    else
                    {
                        CheckAndPrintPrices chp = new CheckAndPrintPrices();
                        chp.typ_doc = this.typ_doc;
                        ListViewItem lvi = listView_stroki.Items[listView_stroki.SelectedIndices[0]];
                        string tovar_code = lvi.Tag.ToString();
                        chp.txtB_input_barcode.Text = tovar_code;
                        //find_barcode_or_code_in_tovar(tovar_code);//Непонятно зачем это здесь 
                        chp.txtB_tovar.Text = tovar_code + "," + listView_stroki.Items[lvi.Index].SubItems[3].Text + ";" +
                        listView_stroki.Items[lvi.Index].SubItems[4].Text;
                        //wb.label_decription_document.Text = this.label_decription_document;
                        chp.guid = guid;
                        //wb.txtB_input_barcode.Text = get_barcode(tovar_code);
                        chp.tovar_code = tovar_code;
                        //Получить guid характеристики
                        string characteristic_name = listView_stroki.Items[lvi.Index].SubItems[2].Text.Trim();
                        if (characteristic_name.Length != 0)
                        {
                            chp.characteristic_guid = get_characteristic_guid(tovar_code, characteristic_name);
                        }
                        chp.txtB_quantity.Text = "1";
                        //chp.txtB_quantity.SelectedText = true;
                        //chp.open_from_document_list = true;

                        //if (display_quantity == 1)
                        //{
                        //    chp.label_количество_в_1с.Text = listView_stroki.Items[lvi.Index].SubItems[2].Text.Trim();
                        
                        //}
                        //wb.label_количество_в_магазине.Text = " в документе " + listView_stroki.Items[lvi.Index].SubItems[1].Text.Trim();
                        chp.txtB_tovar.Text = tovar_code + "," + characteristic_name;//listView_stroki.Items[lvi.Index].SubItems[1].Text.Trim(); 
                        string num_str = lvi.SubItems[0].Text;
                        //wb.display_quantity = display_quantity;                        
                        chp.close_this_form = true;
                        chp.label_product_expiration_date.Text = lvi.SubItems[5].Text;
                        chp.txtB_total_price_tags.Text = chp.get_quantity_shop();
                        chp.txtB_quantity.Focus();
                        chp.ShowDialog();
                        //переррисовать список и спозиционироваться на старой строке 
                        load_stroki();
                        //
                        int index = 0;
                        foreach (ListViewItem item in listView_stroki.Items)
                        {
                            //if (item.Tag.ToString() == tovar_code.ToString())
                            //{
                            //    index = item.Index;
                            //    break;
                            //}
                            if (item.SubItems[0].Text.Trim() == num_str.Trim())
                            {
                                index = item.Index;
                                break;
                            }
                        }
                        //int index = listView_stroki.Items.IndexOf(lvi);
                        listView_stroki.Items[index].Selected = true;
                        listView_stroki.Items[index].Focused = true;
                        listView_stroki.EnsureVisible(index);
 
                    }                    
                }
                else if (e.KeyCode == Keys.D1)
                {
                    listView_stroki_repaint(5, false);
                }
                else if (e.KeyCode == Keys.D2)
                {
                    listView_stroki_repaint(50, false);
                }
                else if (e.KeyCode == Keys.D3)
                {
                    listView_stroki_repaint(250, false);
                }
                else if (e.KeyCode == Keys.D7)
                {
                    listView_stroki_repaint(5, true);
                }
                else if (e.KeyCode == Keys.D8)
                {
                    listView_stroki_repaint(50, true);
                }
                else if (e.KeyCode == Keys.D9)
                {
                    listView_stroki_repaint(250, true);
                }
            }
        }


        /// <summary>
        ///  при смене строки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_stroki_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_stroki.SelectedIndices.Count > 0)
            {
                if (listView_stroki.SelectedIndices[0] != index_position)
                {
                    if ((DateTime.Now - datetime_select_position).Seconds < 2)
                    {

                        listView_stroki.Focus();
                        listView_stroki.Items[index_position].Focused = true;
                        listView_stroki.Items[index_position].Selected = true;
                        listView_stroki.EnsureVisible(index_position);
                    }
                }

                label_selected_quantity_shop.Text = listView_stroki.Items[listView_stroki.SelectedIndices[0]].SubItems[1].Text;
                if (display_quantity == 1)
                {
                    label_selected_quantity_1c.Text = " из " + listView_stroki.Items[listView_stroki.SelectedIndices[0]].SubItems[2].Text;
                    label_price.Text = " Цена " + listView_stroki.Items[listView_stroki.SelectedIndices[0]].SubItems[5].Text;
                }
                else
                {
                    label_price.Text = " Цена " + listView_stroki.Items[listView_stroki.SelectedIndices[0]].SubItems[4].Text;
                }
                               
                //+ " из "+
                //listView_stroki.Items[listView_stroki.SelectedIndices[0]].SubItems[4].Text;

                label_description_tovar.Text = listView_stroki.Items[listView_stroki.SelectedIndices[0]].Tag.ToString() + "," +
                    listView_stroki.Items[listView_stroki.SelectedIndices[0]].SubItems[3].Text + ";" +
                     listView_stroki.Items[listView_stroki.SelectedIndices[0]].SubItems[4].Text;
                //показать 5 последних знаков штрихкода 
                //if (!show_all_stroki)
                //{
                    txtB_info_1s.Text = get_all_barcodes(listView_stroki.Items[listView_stroki.SelectedIndices[0]].Tag.ToString());
                //}
                //else
                //{
                //    txtB_info_1s.Text = comment;
                //}
            }
        }

        private string get_all_barcodes(string tovar_code)        
        {
            string result = "";
            SQLiteConnection conn = Program.ConnectForDataBase();
            try
            {
                conn.Open();
                string query = "SELECT barcode_code FROM barcodes WHERE tovar_code="+tovar_code;
                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string local_barcode = reader["barcode_code"].ToString().Trim();
                    if(local_barcode.Length>5)
                    {
                        local_barcode = local_barcode.Substring(local_barcode.Length-5,5);
                    }
                    result += local_barcode + " ";
                }
                reader.Close();
                command.Dispose();
                conn.Close();
            }
            catch
            {

            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            
            return result;
        }

        private void txtB_status_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Z)
            {
                txtB_inputbarcode.Focus();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            //else if (e.KeyCode == Keys.F15)
            else if ((e.KeyCode == Keys.F15)||(e.KeyCode == Keys.F14))
            {
                if (display_quantity == 0)
                {
                    return;
                }
                if (show_all_stroki)
                {
                    show_all_stroki = false;
                }
                else
                {
                    show_all_stroki = true;
                }
                load_stroki();
            }
        }


        private void txtB_inputbarcode_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
            {
                if (this.txtB_inputbarcode.Text.Length == 0)//||(this.inputbarcode.Text=="\r\n"))//тут еще проверка на минимальность символов
                {
                    MessageBox.Show("Штрихкод не найден");
                    return;
                }
                find_barcode_or_code_in_tovar(this.txtB_inputbarcode.Text.Trim());
                // this.listView1.Items.Count

                txtB_inputbarcode.Text = "";
                return;
            }

            if (!(Char.IsDigit(e.KeyChar)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {

                    e.Handled = true;
                }
            }
        }

        private void find_barcode_or_code_in_tovar(string barcode)
        {
            int listView_stroki_index=0;

            if (listView_stroki.SelectedIndices.Count > 0)
            {
                listView_stroki_index = listView_stroki.SelectedIndices[0];
            }            

            SQLiteConnection conn = Program.ConnectForDataBase();
            bool find = false;
            try
            {
                conn.Open();
                string query = "";
                if (barcode.Length < 7)
                {
                    query = "SELECT code FROM tovar where code=@barcode";
                }
                else
                {
                    query = "SELECT barcodes.tovar_code FROM barcodes where barcodes.barcode_code=@barcode";
                }
                SQLiteParameter _barcode = new SQLiteParameter("barcode", barcode);
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.Add(_barcode);
                object result_query = command.ExecuteScalar();
                conn.Close();
                if (result_query != null)
                {
                    string tovar_code = result_query.ToString();
                   
                    foreach (ListViewItem lvi in listView_stroki.Items)
                    {
                        if (lvi.Tag.ToString().Trim() == tovar_code)
                        {
                            listView_stroki.Focus();
                            listView_stroki.Items[lvi.Index].Selected = true;
                            listView_stroki.EnsureVisible(lvi.Index);
                            //string kolm = listView_stroki.Items[lvi.Index].SubItems[4].Text.Trim();
                            //if (kolm == "")
                            //{
                            //    listView_stroki.Items[lvi.Index].SubItems[4].Text = "1";
                            //}
                            //else
                            //{
                            //    listView_stroki.Items[lvi.Index].SubItems[4].Text = (Convert.ToInt32(kolm) + 1).ToString();
                            //}
                            //Открываем форму для редактирвоания
                            //WorkWithBarcode wb = new WorkWithBarcode();                            
                            //find_barcode_or_code_in_tovar(tovar_code);
                            //wb.txtB_tovar.Text=tovar_code+","+listView_stroki.Items[lvi.Index].SubItems[1].Text+";"+
                            //    listView_stroki.Items[lvi.Index].SubItems[2].Text;
                            //wb.label_decription_document.Text = this.label_decription_document;
                            //wb.guid = guid;
                            //wb.ShowDialog();                     
                            find = true;
                            break;
                        }
                    }
                    if (!find)
                    {
                        //Теперь пробуем найти в скрытых строках которые не попали в отбор
                        if (!show_all_stroki)//Режим показа расхождений
                        {
                            if (add_string_listview(barcode)== 1)//Если дополнительно не нашли тогда спозиционируемя по старому алгоритму
                            {
                                find = true;
                            }
                        }
                        if (!find)
                        {
                            MessageBox.Show("В строках товар с кодом " + result_query + " и штрихкодом " + barcode + " не найден");
                        }
                    }

                }

                else
                {
                    MessageBox.Show("Товар со штрихкодом " + barcode + " не найден ");
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
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();

                }
            }

            if (find)
            {
                if (typ_doc != "5")
                {
                    listView_stroki_KeyDown(null, new KeyEventArgs(Keys.Enter));
                }
            }
            else
            {
                //if (!show_all_stroki)//Режим показа расхождений
                //{
                //    if (add_string_listview(barcode) != 1)//Если дополнительно не нашли тогда спозиционируемя по старому алгоритму
                //    {
                //        listView_stroki.Focus();
                //        listView_stroki.Items[listView_stroki_index].Selected = true;
                //        listView_stroki.Items[listView_stroki_index].Focused = true;
                //    }
                //}
                //else
                //{
                    listView_stroki.Focus();
                    listView_stroki.Items[listView_stroki_index].Selected = true;
                    listView_stroki.Items[listView_stroki_index].Focused = true; 
                //}               
            }
        }


        private void  load_stroki()
        {
            int num_str = 0;

            if (show_all_stroki) 
            {
                if (listView_stroki.SelectedIndices.Count > 0)
                {
                    num_str = Convert.ToInt32(listView_stroki.Items[listView_stroki.SelectedIndices[0]].SubItems[0].Text) - 1;//запомним код позиции
                }
            }

           // listView_stroki.Enabled = false;
            if (typ_doc == "1")
            {
                this.Text = "Инвентаризация";
            }
            else if (typ_doc == "2")
            {
                this.Text = "Поступление";
            }
            else if (typ_doc == "3")
            {
                this.Text = "Заказ клиента";
            }
            else if (typ_doc == "4")
            {
                this.Text = " Проверка и печать ценников ";
            }
            else if (typ_doc == "5")
            {
                this.Text = " Сбор сроков товаров ";
            }
            
            listView_stroki.Items.Clear();

            using (SQLiteConnection conn = Program.ConnectForDataBase())
            {
                try
                {
                    SQLiteCommand command = null;
                    conn.Open();
                    string query = "SELECT type,date,info_1s,status,display_quantity FROM dh where guid=@guid";

                    SQLiteParameter _guid = new SQLiteParameter("guid", guid);
                    using (command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.Add(_guid);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                typ_doc = reader["type"].ToString();
                                date.Value = reader.GetDateTime(1);
                                txtB_info_1s.Text = reader["info_1s"].ToString();
                                //comment = txtB_info_1s.Text;
                                txtB_status.Text = reader["status"].ToString();
                                display_quantity = Convert.ToInt16(reader["display_quantity"]);
                            }
                        }
                        //reader.Close();
                        paint_column();

                        string dop_query = "";
                        if (!show_all_stroki)
                        {
                            this.Text += "  Отбор ";//"  расхождения ) верх. кнопки ";
                            dop_query = " AND dt.quantity <> dt.quantity_shop ";
                        }
                        else
                        {
                            this.Text += "";
                        }
                        if (num_box == "")
                        {
                            if (typ_doc == "4")
                            {
                                query = "SELECT dt.tovar_code, dt.quantity,dt.quantity_shop,dt.price_buy,dt.price," +
                                    " tovar.name AS tovar_name,characteristic.guid AS characteristic_guid," +
                                    " characteristic.name AS characteristic_name,dt.line_number FROM dt " +
                                    " LEFT JOIN tovar ON dt.tovar_code = tovar.code " +
                                    " LEFT JOIN characteristic ON dt.characteristic = characteristic.guid " +
                                    " where dt.guid=@guid " + dop_query +
                                    " order by dt.line_number";
                            }
                            else if (typ_doc == "5")
                            {
                                query = "SELECT dt.tovar_code, dt.quantity,dt.quantity_shop,dt.price_buy,dt.price," +
                                                           " tovar.name AS tovar_name,characteristic.guid AS characteristic_guid," +
                                                           " characteristic.name AS characteristic_name,dt.line_number,dt.date_expiration FROM dt " +
                                                           " LEFT JOIN tovar ON dt.tovar_code = tovar.code " +
                                                           " LEFT JOIN characteristic ON dt.characteristic = characteristic.guid " +
                                                           " where dt.guid=@guid " + dop_query +
                                                           " order by tovar_name,date_expiration";
                            }
                            else
                            {
                                query = "SELECT dt.tovar_code, dt.quantity,dt.quantity_shop,dt.price_buy,dt.price," +
                                                                                  " tovar.name AS tovar_name,characteristic.guid AS characteristic_guid," +
                                                                                  " characteristic.name AS characteristic_name,dt.line_number FROM dt " +
                                                                                  " LEFT JOIN tovar ON dt.tovar_code = tovar.code " +
                                                                                  " LEFT JOIN characteristic ON dt.characteristic = characteristic.guid " +
                                                                                  //" where dt.guid=@guid AND box=@box" + dop_query +
                                                                                  " where dt.guid=@guid " + dop_query +
                                                                                  " order by dt.line_number";
                            }
                        }
                        else
                        {
                            query = "SELECT dt.tovar_code, dt.quantity,dt.quantity_shop,dt.price_buy,dt.price," +
                                                   " tovar.name AS tovar_name,characteristic.guid AS characteristic_guid," +
                                                   " characteristic.name AS characteristic_name,dt.line_number FROM dt " +
                                                   " LEFT JOIN tovar ON dt.tovar_code = tovar.code " +
                                                   " LEFT JOIN characteristic ON dt.characteristic = characteristic.guid " +
                                                   " where dt.guid=@guid AND box=@box" + dop_query +
                                                   " order by dt.line_number";
                        }

                    }
                    //_guid = new SQLiteParameter("guid", guid);
                    SQLiteParameter _box = null;
                    if (num_box != "")
                    {
                        _box = new SQLiteParameter("box", num_box);
                    }
                    else
                    {
                        _box = new SQLiteParameter("box", "0");
                    }

                        using (command = new SQLiteCommand(query, conn))
                        {
                            command.Parameters.Add(_guid);
                            //command.Parameters.Add(_box);
                            if (num_box != "")
                            {
                                command.Parameters.Add(_box);
                            }
                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {

                                    ListViewItem item = new ListViewItem(reader["line_number"].ToString());
                                    item.SubItems.Add(reader["quantity_shop"].ToString());
                                    if (display_quantity == 1)
                                    {
                                        item.SubItems.Add(reader["quantity"].ToString());
                                    }
                                    item.SubItems.Add(reader["tovar_name"].ToString());
                                    item.SubItems.Add(reader["characteristic_name"].ToString());
                                    item.SubItems.Add(reader["price_buy"].ToString());
                                    item.Tag = reader["tovar_code"].ToString();
                                    if (typ_doc == "5")
                                    {
                                        item.SubItems.Add(Convert.ToDateTime(reader["date_expiration"]).ToString("yyyy-MM-dd"));
                                    }
                                    listView_stroki.Items.Add(item);

                                    //i++;
                                }
                            }

                            if (num_str != 0)
                            {
                                if (listView_stroki.Items.Count != 0)
                                {
                                    int current_index = 0;
                                    foreach (ListViewItem item in listView_stroki.Items)
                                    {
                                        if (item.SubItems[0].Text == num_str.ToString())
                                        {
                                            listView_stroki.EnsureVisible(current_index);
                                            listView_stroki.Focus();
                                            listView_stroki.Items[current_index].Selected = true;
                                            listView_stroki.Items[current_index].Focused = true;
                                        }
                                        current_index++;
                                    }
                                }
                            }

                            //удалить строки с одинаковым количеством
                            //conn.Close();
                        }
                    //}
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
            //    conn.Dispose();
            //}        
        }


        /// <summary>
        /// для отбора , для сложных случаев
        /// </summary>
        /// <param name="tovar_code"></param>
        /// <param name="characteristic_name"></param>
        /// <returns></returns>
        //private int find_tovar_in_listView_stroki(string tovar_code, string characteristic_name)
        //{
        //    int index = -1;
        //    foreach (ListViewItem item in listView_stroki.Items)
        //    {
        //        if (item.Tag.ToString() == tovar_code)
        //        {
        //            index = item.Index;
        //            if (characteristic_name.Trim().Length > 0)
        //            {
        //                if (display_quantity == 1)
        //                {
        //                    if (item.SubItems[4].Text.Trim() != characteristic_name.Trim())
        //                    {
        //                        index = -1;
        //                    }
        //                }
        //                else
        //                {
        //                    if (item.SubItems[1].Text.Trim() != characteristic_name.Trim())
        //                    {
        //                        index = -1;
        //                    }
        //                }
        //            }
        //            //if (index != -1)
        //            //{
        //            //    break;
        //            //}
        //            break;
        //        }
        //    }
        //    return index;
        //}


        private void paint_column()
        {

            if (listView_stroki.Columns.Count > 0)
            {
                listView_stroki.Columns.Clear();
            }

            if (show_all_stroki)
            {
                if (typ_doc != "5")
                {
                    listView_stroki.Columns.Add("Н.с.", 30, HorizontalAlignment.Center);
                    listView_stroki.Columns.Add("Ф", 30, HorizontalAlignment.Right);
                }
                else
                {
                    listView_stroki.Columns.Add("Н.с.", 3, HorizontalAlignment.Center);
                    listView_stroki.Columns.Add("Ф", 30, HorizontalAlignment.Right);
                }
                if (display_quantity == 1)
                {
                    if (typ_doc != "5")
                    {
                        listView_stroki.Columns.Add("П", 30, HorizontalAlignment.Right);
                    }
                    else
                    {
                        listView_stroki.Columns.Add("П", 30, HorizontalAlignment.Right);
                    }
                }
                listView_stroki.Columns.Add("Номенклатура", 200, HorizontalAlignment.Left);
                listView_stroki.Columns.Add("Характеристика", 0, HorizontalAlignment.Left);
                
                if (typ_doc == "5")
                {
                    listView_stroki.Columns.Add("Ц.пр.", 0, HorizontalAlignment.Right);
                    listView_stroki.Columns.Add("Годен до:", 80, HorizontalAlignment.Right);                    
                }
                else
                {
                    listView_stroki.Columns.Add("Ц.пр.", 100, HorizontalAlignment.Right); 
                }
            }
            else
            {
                listView_stroki.Columns.Add("Н.с.", 30, HorizontalAlignment.Center);
                listView_stroki.Columns.Add("Ф", 30, HorizontalAlignment.Right);
                if (display_quantity == 1)
                {
                    listView_stroki.Columns.Add("П", 30, HorizontalAlignment.Right);
                }
                listView_stroki.Columns.Add("Номенклатура", 225, HorizontalAlignment.Left);                
            }

        }

        private void Document_Load(object sender, EventArgs e)
        {          

            // Set the view to show details.
            listView_stroki.View = View.Details;
            
            // Select the item and subitems when selection is made.
            listView_stroki.FullRowSelect = true;          

            paint_column();
            load_stroki();

            if (listView_stroki.Items.Count != 0)
            {
                listView_stroki.Focus();
                listView_stroki.Items[0].Selected = true;
                listView_stroki.Items[0].Focused = true;
            }
        }

        private void btn_Esc_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
