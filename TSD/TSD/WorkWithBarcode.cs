using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Threading;

namespace TSD
{
    public partial class WorkWithBarcode : Form
    {
        public string guid = "";
        public string tovar_code = "";//Отсканированная последняя позиция
        public string characteristic_guid = "";//Отсканированная последняя позиция
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        public bool open_from_document_list = false;
        private int display_quantity = 0;
        private PowerStatus ps = new PowerStatus();
        public bool close_this_form = false;
        public string typ_doc = "";
        public int its_new = 0;
        public string num_box = "";
        //public string status = "";


        public WorkWithBarcode()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.txtB_input_barcode.KeyPress += new KeyPressEventHandler(txtB_input_barcode_KeyPress);            
            txtB_quantity.KeyPress += new KeyPressEventHandler(txtB_quantity_KeyPress);
            this.Load += new EventHandler(WorkWithBarcode_Load);
            timer.Interval = 3000;
            timer.Tick += new EventHandler(timer_Tick);
            this.Paint += new PaintEventHandler(WorkWithBarcode_Paint);
            this.its_new = Program.its_new_document(guid);
        }

        private void WorkWithBarcode_Paint(object sender, PaintEventArgs e)
        {
            Program.write_log("WorkWithBarcode_Paint1");
            label_powerstatus.Text = ps.ReportPowerStatus("main") + " | " + ps.ReportPowerStatus("");
            Program.write_log("WorkWithBarcode_Paint2");
        }

        private void get_display_quantity()
        {
            Program.write_log("get_display_quantity1");
             SQLiteConnection conn = Program.ConnectForDataBase();
             try
             {
                 conn.Open();
                 string query = "SELECT display_quantity FROM dh where guid=@guid";

                 SQLiteParameter _guid = new SQLiteParameter("guid", guid);
                 SQLiteCommand command = new SQLiteCommand(query, conn);
                 command.Parameters.Add(_guid);
                 SQLiteDataReader reader = command.ExecuteReader();
                 while (reader.Read())
                 {
                     this.display_quantity = Convert.ToInt16(reader["display_quantity"]);
                 }
                 reader.Close();
                 command.Dispose();
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
             Program.write_log("get_display_quantity2");
        }

        private string get_quantity_1c()
        {
            Program.write_log("get_quantity_1c1");
            string result = "";


            if (display_quantity == 0)
            {
                return result;
            }

            SQLiteConnection conn = Program.ConnectForDataBase();

            try
            {
                conn.Open();
                string query = "";
                
                if (characteristic_guid != "")
                {
                    if (num_box.Trim() == "")
                    {
                        query = " SELECT SUM(quantity) FROM dt WHERE guid='" + guid + "'" +
                             " AND tovar_code=" + tovar_code +
                             " AND characteristic='" + characteristic_guid + "' GROUP BY tovar_code,characteristic ";
                    }
                    else
                    {
                        query = " SELECT SUM(quantity) FROM dt WHERE guid='" + guid + "'" +
                          " AND tovar_code=" + tovar_code +
                          " AND characteristic='" + characteristic_guid + "' AND box='" + num_box + "' GROUP BY tovar_code,characteristic ";
                    }
                }
                else
                {
                    if (num_box.Trim() == "")
                    {
                        query = " SELECT SUM(quantity) FROM dt WHERE guid='" + guid + "'" +
                                                " AND tovar_code=" + tovar_code + " GROUP BY tovar_code ";
                    }
                    else
                    {
                        query = " SELECT SUM(quantity) FROM dt WHERE guid='" + guid + "'" +
                                                                       " AND tovar_code=" + tovar_code + " AND box='"+num_box+"' GROUP BY tovar_code ";
                    }
                }

                SQLiteCommand command = new SQLiteCommand(query, conn);
                object result_query = command.ExecuteScalar();
                if (result_query != null)
                {
                    result = result_query.ToString();
                }
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
            Program.write_log("get_quantity_1c2");
            return result;            
        }

        private string get_quantity_shop()
        {
            Program.write_log("get_quantity_shop1");
            string result = "";
                     
            SQLiteConnection conn = Program.ConnectForDataBase();

            try
            {
                conn.Open();
                string query = "";
                if (characteristic_guid != "")
                {
                    if (num_box == "")
                    {
                        query = " SELECT SUM(quantity_shop) FROM dt WHERE guid='" + guid + "'" +
                             " AND tovar_code=" + tovar_code +
                             " AND characteristic='" + characteristic_guid + "' GROUP BY tovar_code,characteristic ";
                    }
                    else
                    {
                        query = " SELECT SUM(quantity_shop) FROM dt WHERE guid='" + guid + "'" +
                                                    " AND tovar_code=" + tovar_code +
                                                    " AND characteristic='" + characteristic_guid + "' AND box='" + num_box + "'  GROUP BY tovar_code,characteristic ";
                    }
                   
                }
                else
                {
                    if (num_box == "")
                    {
                        query = " SELECT SUM(quantity_shop) FROM dt WHERE guid='" + guid + "'" +
                                                " AND tovar_code=" + tovar_code + " GROUP BY tovar_code ";
                    }
                    else
                    {
                        query = " SELECT SUM(quantity_shop) FROM dt WHERE guid='" + guid + "'" +
                                                                       " AND tovar_code=" + tovar_code +" AND box='" + num_box + "' GROUP BY tovar_code ";
                    }
                }
                SQLiteCommand command = new SQLiteCommand(query, conn);
                object result_query = command.ExecuteScalar();
                if (result_query != null)
                {
                    result = result_query.ToString();
                }
                conn.Close();
                command.Dispose();
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
            Program.write_log("get_quantity_shop2");
            return result;            
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Enabled = false;
            panel_tovar_not_found.Visible = false;            
            this.Focus();
            this.txtB_input_barcode.Focus();
        }     
    
        private void to_check_the_status_of_the_document()
        {
            Program.write_log("to_check_the_status_of_the_document1");
            SQLiteConnection conn = Program.ConnectForDataBase();
            try
            {
                conn.Open();
                string query = "Select status,display_quantity FROM dh where guid='" + guid + "'";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                object result_query = command.ExecuteScalar();
                if (result_query != null)
                {
                    if (result_query.ToString() == "2")
                    {
                        txtB_input_barcode.Enabled = false;
                        txtB_quantity.Enabled = false;
                    }                    
                }
                command.Dispose();
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
            Program.write_log("to_check_the_status_of_the_document2");
        }

        private void load_data_last_scaned()
        {
            Program.write_log("load_data_last_scaned1");
            SQLiteConnection conn = Program.ConnectForDataBase();
            try
            {
                conn.Open();
                string query = "Select description FROM last_scaned where guid='" + guid + "'";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                object result_query = command.ExecuteScalar();
                if (result_query != null)
                {
                    //string description
                    txtB_tovar.Text = result_query.ToString();
                    //char[] delimiters = new char[] { '|' };

                    //string[] s = description.ToString().Split(delimiters);
//                    label_date_scaning.Text = s[0].ToString();
                    //txtB_tovar.Text = s[1].ToString();
                }
                command.Dispose();
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
            Program.write_log("load_data_last_scaned2");
        }
        
        private void WorkWithBarcode_Load(object sender, EventArgs e)
        {
            Program.write_log("WorkWithBarcode_Load1");

            if (its_new == 1)
            {
                num_box = "0";
            }
            to_check_the_status_of_the_document();

            if (!open_from_document_list)
            {
                load_data_last_scaned();                
                txtB_input_barcode.Focus();
            }
            else
            {
                txtB_quantity.Focus();
            }
            txtB_quantity.Select(0, txtB_quantity.Text.Length);
            get_display_quantity();
            if (tovar_code != "")
            {
                label_количество_в_магазине.Text = " в коробке  " + get_quantity_shop();
                label_количество_в_1с.Text = get_quantity_1c();
            }
            Program.write_log("WorkWithBarcode_Load2");   
        }
        
        private void txtB_quantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            Program.write_log("txtB_quantity_KeyPress1");   
            if (tovar_code == "")
            {
                //MessageBox.Show(" Товар не определен ");
                tovar_not_found();
                e.Handled = true;
                txtB_input_barcode.Focus();
                return;
            }

            if (!(Char.IsDigit(e.KeyChar)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    if (e.KeyChar != (char)Keys.Enter)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        //поставить проверку на наличие штрихкода в базе 

                        write_record();
                        this.txtB_quantity.Text = "";
                        this.txtB_input_barcode.Text = "";
                        tovar_code = ""; 
                        txtB_input_barcode.Focus();
                    }
                }
            }
            Program.write_log("txtB_quantity_KeyPress2");   
        }

        private void tovar_not_found()
        {
            Program.write_log("tovar_not_found1");   
            //if (this.its_new == 0)
            //{
                panel_tovar_not_found.Visible = true;
                panel_tovar_not_found.BringToFront();
                timer.Enabled = true;
                txtB_tovar.Text = "";
                label_price_value.Text = "";
                tovar_code = "";
                txtB_input_barcode.Text = "";
                txtB_quantity.Text = "";
                PlaySound ps = new PlaySound();
                ps.PlaySound_WAV("\\Windows\\exclam.wav");
                ps.PlaySound_WAV("\\Windows\\exclam.wav");
            //}
                Program.write_log("tovar_not_found2");   
        }

        private void txtB_input_barcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            Program.write_log("txtB_input_barcode_KeyPress1");   
//            label_date_scaning.Text = "Сканировано "+DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            if (e.KeyChar == 13)
            {
                if (this.txtB_input_barcode.Text.Length == 0)//||(this.inputbarcode.Text=="\r\n"))//тут еще проверка на минимальность символов
                {
                    //tovar_not_found();                    
                    return;
                }

                find_barcode_or_code_in_tovar(this.txtB_input_barcode.Text.Trim());                
                return;
            }

            if (!(Char.IsDigit(e.KeyChar)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {

                    e.Handled = true;
                }
            }
            Program.write_log("txtB_input_barcode_KeyPress2");      
        }
        
        private bool check_positive_quantity(string quantity_shop)
        {
            Program.write_log("check_positive_quantity1");   
            bool result = true;

            string quantity = get_quantity_shop();
            if (quantity.Length == 0)
            {
                quantity = "0";
            }

            if (quantity == "0")
            {

            }
            else
            {
                if (Convert.ToInt32(quantity_shop) * -1 > Convert.ToInt32(quantity))
                {
                    result = false;
                }
            }
            Program.write_log("check_positive_quantity2");   
            return result;
        }


        /// <summary>
        /// Записать новые изменения
        /// </summary>
        private void write_record()
        {
            Program.write_log("write_record1");   

            label_количество_в_магазине.Text = " в коробке  ";
            label_количество_в_1с.Text = "";

            //проверка на отрицательное количество 
            if (txtB_quantity.Text.IndexOf("-") != -1) // вводится отрицательное количество, проверим чтобы не ушло в минус  
            {
                if (!check_positive_quantity(txtB_quantity.Text))
                {
                    PlaySound ps = new PlaySound();
                    ps.PlaySound_WAV("\\Windows\\exclam.wav");
                    ps.PlaySound_WAV("\\Windows\\exclam.wav");         
                    return;
                }
            }

            if (txtB_quantity.Text.Trim().Length == 0)
            {
                txtB_input_barcode.Focus();
                return;                
            }
            else if (Convert.ToInt32(txtB_quantity.Text.Trim()) == 0)
            {
                MessageBox.Show(" Количество должно быть больше нуля ");
                txtB_quantity.Text = "";
                txtB_input_barcode.Focus();
                return;
            }
            else if (tovar_code == "")
            {
                //MessageBox.Show(" Товар не определен ");
                tovar_not_found();
                txtB_input_barcode.Focus();
                return; 
            }

            SQLiteConnection conn = Program.ConnectForDataBase();

            try
            {
                conn.Open();
                string query = "";
                int quantity_shop = Convert.ToInt32(txtB_quantity.Text.Trim()); 

                //перед обновлением по количеству надо понять есть такой товар в документе или нет
                if (characteristic_guid != "")
                {
                    if (num_box == "")
                    {
                        query = "SELECT COUNT(*) FROM dt WHERE guid ='" + guid + "' AND tovar_code=" + tovar_code + " AND characteristic='" + characteristic_guid + "'";
                    }
                    else
                    {
                        query = "SELECT COUNT(*) FROM dt WHERE guid ='" + guid + "' AND tovar_code=" + tovar_code + " AND characteristic='" + characteristic_guid + "' AND box='" + num_box + "' ";
                    }
                }
                else
                {
                    if (num_box == "")
                    {
                        query = "SELECT COUNT(*) FROM dt WHERE guid ='" + guid + "' AND tovar_code=" + tovar_code;
                    }
                    else
                    {
                        query = "SELECT COUNT(*) FROM dt WHERE guid ='" + guid + "' AND tovar_code=" + tovar_code + " AND box='" + num_box + "' ";
                    }
                }
                SQLiteCommand  command = new SQLiteCommand(query, conn);
                int count_tovar = Convert.ToInt16(command.ExecuteScalar());
                command.Dispose();
                if (count_tovar > 0)//таких товаров больше 1-го необходимо распределить это количество по строка
                {
                    //if (conn.State == ConnectionState.Open)
                    //{
                    //    conn.Close();
                    //}

                    if (quantity_shop > 0)
                    {
                        distribute_positve_quantity(quantity_shop,conn);//распределить положительное количество
                    }
                    else
                    {
                        distribute_negative_quantity(quantity_shop,conn);//распределить отрицательное количество
                    }
                }
                else //Этой позиции нет в документе надо ее добавить 
                {
                    if (characteristic_guid != "")
                    {
                        //query = " INSERT INTO dt(guid,tovar_code,characteristic,quantity,quantity_shop,price_buy,price,line_number,its_sent)VALUES(@guid,@tovar_code,@characteristic,@quantity,@quantity_shop,@price_buy,@price,@line_number,@its_sent);";
                        //query = " INSERT INTO dt(guid,tovar_code,characteristic,quantity,quantity_shop,price_buy,price)VALUES(@guid,@tovar_code,@characteristic,@quantity,@quantity_shop,@price_buy,@price);";
                        query = " INSERT INTO dt(guid,tovar_code,characteristic,quantity,quantity_shop,price_buy,price,line_number,its_sent,box,box_status)VALUES('" +
                        guid + "'," +
                        tovar_code + ",'" +
                        characteristic_guid + "'," +
                        "0" + "," +
                        quantity_shop.ToString() + "," +
                        "0" + "," +
                        "0" + "," +
                        get_new_line_number() + "," +
                        "1" + ",'" +
                        num_box+"','"+
                        "т');";
                    }
                    else
                    {
                        //query = " INSERT INTO dt(guid,tovar_code,quantity,quantity_shop,price_buy,price,line_number,its_sent)VALUES(@guid,@tovar_code,@quantity,@quantity_shop,@price_buy,@price,@line_number,@its_sent);";
                        //query = " INSERT INTO dt(guid,tovar_code,quantity,quantity_shop,price_buy,price)VALUES(@guid,@tovar_code,@quantity,@quantity_shop,@price_buy,@price);";
                        query = " INSERT INTO dt(guid,tovar_code,quantity,quantity_shop,price_buy,price,line_number,its_sent,box,box_status)VALUES('" +
                        guid + "'," +
                        tovar_code + "," +
                        "0" + "," +
                        quantity_shop.ToString() + "," +
                        "0" + "," +
                        "0" + "," +
                        get_new_line_number() + "," +
                        "1" + ",'" +
                        num_box + "','"+
                        "т');";
                    }

                    command = new SQLiteCommand(query, conn);                   
                    command.ExecuteNonQuery();
                    command.Dispose();
                }

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                string tovar_name = "";
                if (characteristic_guid != "")
                {
                    if (num_box == "")
                    {
                        query = " SELECT tovar.name AS tovar_name,characteristic.name AS characteristic_name  FROM tovar LEFT JOIN characteristic " +
                        " ON tovar.code=characteristic.tovar_code WHERE code=" + tovar_code +
                        " AND characteristic.guid='" + characteristic_guid + "'";
                    }
                    else
                    {
                        query = " SELECT tovar.name AS tovar_name,characteristic.name AS characteristic_name  FROM tovar LEFT JOIN characteristic " +
                                               " ON tovar.code=characteristic.tovar_code WHERE code=" + tovar_code +
                                               " AND characteristic.guid='" + characteristic_guid + "' AND box='" + num_box + "' ";
                    }
                    command = new SQLiteCommand(query, conn);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        tovar_name = reader["tovar_name"].ToString() + ";" + reader["characteristic_name"].ToString();
                    }
                    reader.Close();
                    command.Dispose();
                }
                else
                {
                    //if (num_box == "")
                    //{
                    //    query = " SELECT name FROM tovar WHERE code=" + tovar_code;
                    //}
                    //else
                    //{
                        query = " SELECT name FROM tovar WHERE code=" + tovar_code ;
                    //}
                    command = new SQLiteCommand(query, conn);
                    tovar_name = command.ExecuteScalar().ToString();
                    command.Dispose();
                }

                //НАЧАЛО Обновление записей на форме
                txtB_tovar.Text = tovar_code+", "+tovar_name;
                label_количество_в_магазине.Text = " в коробке  " + get_quantity_shop();
                label_количество_в_1с.Text = get_quantity_1c();
                //КОНЕЦ Обновление записей на форме
                               
                //пометить последнюю записанную позицию
                string description = txtB_tovar.Text;

                query = "DELETE FROM last_scaned";
                command = new SQLiteCommand(query, conn);
                command.ExecuteNonQuery();
                command.Dispose();

                query = "INSERT INTO last_scaned(guid,description) VALUES ('" + guid + "','" + description + "')";                
                command = new SQLiteCommand(query, conn);
                command.ExecuteNonQuery();
                command.Dispose();

                query = "UPDATE dh SET status=1 WHERE guid='" + guid+"'";
                command = new SQLiteCommand(query, conn);
                command.ExecuteNonQuery();
                command.Dispose();

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

            if (close_this_form)
            {
                this.Close();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Program.write_log("write_record2");   
        }


        /// <summary>
        /// При добавлении новой строки
        /// нужно получить новый номер
        /// </summary>
        /// <returns></returns>
        private string get_new_line_number()
        {
            Program.write_log("get_new_line_number1");   
            string result = "1";

            SQLiteConnection conn = Program.ConnectForDataBase();

            try
            {
                conn.Open();
                string query = "SELECT MAX(line_number) FROM dt WHERE guid='" +guid+"'";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                result = (Convert.ToInt32(command.ExecuteScalar()) + 1).ToString();
                conn.Close();
                command.Dispose();
            }
            catch (Exception)
            {

            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            Program.write_log("get_new_line_number2");               
            return result;
        }

        private void update_distribute_quantity(string tovar_code, int quantity_shop, int line_number, SQLiteConnection conn)
        {

        //    SQLiteConnection conn = Program.ConnectForDataBase();
            Program.write_log("update_distribute_quantity1");   

            try
            {
                //conn.Open();
                string query = "";
                if (characteristic_guid == "")
                {
                    query = " UPDATE dt SET its_sent=1,quantity_shop=" +
                    quantity_shop.ToString() + " , box_status='т' WHERE guid='" + guid +
                    "' AND tovar_code = " + tovar_code +
                    " AND line_number=" + line_number.ToString();
                    
                }
                else
                {
                    query = " UPDATE dt SET its_sent=1,quantity_shop=" +
                        //quantity_shop.ToString() + " WHERE guid='" + guid +"'"+
                  quantity_shop.ToString() + " , box_status='т' WHERE guid='" + guid +
                  " AND tovar_code = " + tovar_code +
                  " AND characteristic = '" + characteristic_guid + "'" +
                  " AND line_number=" + line_number.ToString();
                  
                }
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.ExecuteNonQuery();
                command.Dispose();
                //conn.Close();
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
                //if (conn.State == ConnectionState.Open)
                //{
                //    Close();
                //}
            }
            Program.write_log("update_distribute_quantity2");   
        }


        /// <summary>
        /// Распределим количество по строкам
        /// количество из 1с = количеству в магазине 
        /// 
        /// 
        /// </summary>
        /// <param name="tovar_code"></param>
        /// <param name="quantity"></param>
        private void distribute_positve_quantity(int quantity, SQLiteConnection conn)
        {
            Program.write_log("distribute_positve_quantity1");   
           // SQLiteConnection conn = Program.ConnectForDataBase();            
            try
            {
                int last_line_number = 0;
                int quantity_1c_local = 0;
                int quantity_shop_local = 0;
                int quantity_local_distribute = 0;

                //conn.Open();
                string query = "";
                if (characteristic_guid == "")
                {
                    if (num_box == "")
                    {
                        query = " SELECT line_number,quantity_shop,quantity FROM dt WHERE guid ='" +
                           guid + "' AND tovar_code = " + tovar_code +
                           " order by line_number ";
                    }
                    else
                    {
                        query = " SELECT line_number,quantity_shop,quantity FROM dt WHERE guid ='" +
                                                  guid + "' AND tovar_code = " + tovar_code +" AND box='" + num_box + "' "+
                                                  " order by line_number ";
                    }
                }
                else
                {
                    if (num_box == "")
                    {
                        query = " SELECT line_number,quantity_shop,quantity FROM dt WHERE guid ='" +
                                guid + "' AND tovar_code = " + tovar_code +
                                " AND characteristic='" + characteristic_guid + "'" +
                                " order by line_number ";
                    }
                    else
                    {
                        query = " SELECT line_number,quantity_shop,quantity FROM dt WHERE guid ='" +
                              guid + "' AND tovar_code = " + tovar_code +" AND box='" + num_box + "' " +
                              " AND characteristic='" + characteristic_guid + "'" +
                              " order by line_number ";
 
                    }
                }

                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    last_line_number = Convert.ToInt32(reader["line_number"]);
                    quantity_1c_local = Convert.ToInt32(reader["quantity"]); //количество из 1с в строке документа 
                    quantity_shop_local = Convert.ToInt32(reader["quantity_shop"]);//количество в магазине уже внесенное в строке документа 

                    quantity_local_distribute = quantity_1c_local - quantity_shop_local;

                    if (quantity_1c_local == 0)//в строке документа нет количества из 1с все пишем в одну строку
                    {
                        //количество которое осталось распределить + уже внесенное в строке документа
                        update_distribute_quantity(tovar_code, quantity + quantity_shop_local, last_line_number,conn);
                        quantity = 0;
                    }
                    else
                    {
                        if (quantity <= quantity_local_distribute)
                        {

                            //количество которое осталось распределить + уже внесенное в строке документа
                            update_distribute_quantity(tovar_code, quantity + quantity_shop_local, last_line_number,conn);
                            quantity = 0;
                        }
                        else //количество которое осталось распределить больше того что мы можем поместить в строке документа
                        {
                            quantity = quantity - quantity_local_distribute;
                            //то количество которое мы можем добавить к строке документа + уже внесенное в строке документа
                            update_distribute_quantity(tovar_code, quantity_local_distribute + quantity_shop_local, last_line_number,conn);
                        }                       
                    }
                                      

                    if (quantity == 0)
                    {
                        break;
                    }

                }
                reader.Close();
                command.Dispose();
                //conn.Close();


                if (quantity != 0)
                {
                    update_distribute_quantity(tovar_code, quantity + quantity_local_distribute + quantity_shop_local, last_line_number,conn);
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
                //if (conn.State == ConnectionState.Open)
                //{
                //    Close();
                //}
            }
            Program.write_log("distribute_positve_quantity2");   
        }

        /// <summary>
        /// Распределим корректировочное отрицательное 
        /// количество по строкам
        /// 
        /// </summary>
        /// <param name="tovar_code"></param>
        /// <param name="quantity"></param>
        private void distribute_negative_quantity(int quantity, SQLiteConnection conn)
        {
            Program.write_log("distribute_negative_quantity1");   
            quantity = quantity * -1;
            //SQLiteConnection conn = Program.ConnectForDataBase();
            try
            {
                int last_line_number = 0;
                //int quantity_1c_local = 0;
                int quantity_shop_local = 0;
                //int quantity_local_distribute = 0;

                //conn.Open();
                string query = "";
                if (characteristic_guid == "")
                {
                    if (num_box == "")
                    {
                        query = " SELECT line_number,quantity_shop,quantity FROM dt WHERE guid ='" +
                           guid + "' AND tovar_code = " + tovar_code +
                           " order by line_number ";
                    }
                    else
                    {
                        query = " SELECT line_number,quantity_shop,quantity FROM dt WHERE guid ='" +
                                                  guid + "' AND tovar_code = " + tovar_code +" AND box='" + num_box + "' " +
                                                  " order by line_number ";
                    }
                }
                else
                {
                    if (num_box == "")
                    {
                        query = " SELECT line_number,quantity_shop,quantity FROM dt WHERE guid ='" +
                                guid + "' AND tovar_code = " + tovar_code +
                                " AND characteristic='" + characteristic_guid + "'" +
                                " order by line_number ";
                    }
                    else
                    {
                        query = " SELECT line_number,quantity_shop,quantity FROM dt WHERE guid ='" +
                                guid + "' AND tovar_code = " + tovar_code + " AND box='" + num_box + "' " +
                                " AND characteristic='" + characteristic_guid + "'" +
                                " order by line_number ";
 
                    }
                }

                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    last_line_number = Convert.ToInt32(reader["line_number"]);
                    //quantity_1c_local = Convert.ToInt32(reader["quantity"]); //количество из 1с в строке документа 
                    quantity_shop_local = Convert.ToInt32(reader["quantity_shop"]);//количество в магазине уже внесенное в строке документа 
                                       
                    if (quantity < quantity_shop_local)
                    {

                        //количество которое осталось распределить отминусовываем уже внесенное в строке документа
                        update_distribute_quantity(tovar_code, quantity_shop_local - quantity, last_line_number,conn);
                        quantity = 0;
                    }
                    else //количество которое осталось распределить больше того что мы можем поместить в строке документа
                    {                        
                        //то количество которое мы можем добавить к строке документа + уже внесенное в строке документа
                        update_distribute_quantity(tovar_code, 0 , last_line_number,conn);
                        quantity = quantity - quantity_shop_local;
                    }                   

                    if (quantity == 0)
                    {
                        break;
                    }

                }
                reader.Close();
                command.Dispose();
                //conn.Close();

                //if (quantity != 0)
                //{
                //    // quantity                  количество которое осталось рапределить
                //    //quantity_shop_local        количество добавленное в последнюю строку 
                //    //quantity_local_distribute  количество которое было в последней строке     

                //    //update_distribute_quantity(tovar_code, quantity + quantity_local_distribute + quantity_shop_local, last_line_number);
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
            finally
            {
                //if (conn.State == ConnectionState.Open)
                //{
                //    Close();
                //}
            }
            Program.write_log("distribute_negative_quantity2");   
        }

        public void find_barcode_or_code_in_tovar(string barcode)
        {
            Program.write_log("find_barcode_or_code_in_tovar1");   

            label_количество_в_магазине.Text = " в коробке  ";
            label_количество_в_1с.Text = "";


            txtB_quantity.Focus();
            SQLiteConnection conn = Program.ConnectForDataBase();
            try
            {
                conn.Open();
                string query = "";
                if (barcode.Trim().Length < 8)
                {
                    query = "SELECT code,name FROM tovar where code=@barcode"; //LEFT JOIN characteristic 
                }
                else
                {
                    query = "SELECT barcodes.tovar_code,tovar.name FROM barcodes LEFT JOIN tovar ON  barcodes.tovar_code = tovar.code where barcodes.barcode_code=@barcode"; //LEFT JOIN characteristic                     
                }

                //SQLiteParameter _barcode = new SQLiteParameter("barcode", SqlDbType.NVarChar);
                //_barcode.Value = barcode;

                SQLiteParameter _barcode = new SQLiteParameter("barcode", barcode);                                
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.Add(_barcode);
                // НОВОЕ
                SelectProducts sp = new SelectProducts();
                SQLiteDataReader reader = command.ExecuteReader();

                sp.listView_tovar.View = View.Details;

                //listView_inventory.AllowColumnReorder = false;

                // Select the item and subitems when selection is made.
                sp.listView_tovar.FullRowSelect = true;

                sp.listView_tovar.Columns.Add("Код", 60, HorizontalAlignment.Left);
                sp.listView_tovar.Columns.Add("Наименование", 200, HorizontalAlignment.Center);


                object result_query = null;
                while (reader.Read())
                {
                    result_query = reader[0].ToString();
                    ListViewItem lvi = new ListViewItem(reader[0].ToString());
                    lvi.Tag = reader[0].ToString();
                    lvi.SubItems.Add(reader[1].ToString());
                    sp.listView_tovar.Items.Add(lvi);
                }
                reader.Dispose();
                command.Dispose();

                if (sp.listView_tovar.Items.Count > 1)
                {
                    //this.SendToBack();
                    sp.TopMost = true;
                    sp.listView_tovar.Items[0].Selected = true;
                    sp.listView_tovar.Items[0].Focused = true;
                    sp.ShowDialog();
                    //this.TopMost = true;
                    result_query = Program.TovarCode;
                }

                //КОНЕЦ НОВОЕ

                if (result_query != null)
                {
                    //MessageBox.Show(result_query.ToString());
                    tovar_code = result_query.ToString();//присвоили локальной переменной код отсканированной позиции


                    //Проверка на наличие характеристики
                    SelectCharacteristic sc = new SelectCharacteristic();
                    sc.Visible = false;
                    sc.tovar_code = tovar_code;
                    sc.ShowDialog();
                    
                    characteristic_guid = Program.CharacteristicGuid;

                    //if (txtB_quantity.Text.Trim().Length == 0)
                    //{
                        txtB_quantity.Text = "1";
                        txtB_quantity.Select(0, txtB_quantity.Text.Length);
                    //}

                    query = "SELECT tovar.name AS tovar_name ,characteristic.name AS characteristic_name ," +
                        " characteristic.guid ,tovar.retail_price," +
                        " retail_price_characteristic FROM tovar " +
                        " LEFT JOIN characteristic ON tovar.code = characteristic.tovar_code " +
                         " WHERE tovar.code = " + tovar_code  +
                         (characteristic_guid == "" ? "" : " AND characteristic.guid='"+characteristic_guid+"'")
                          ;
                    command = new SQLiteCommand(query, conn);

                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["guid"].ToString().Trim() == "") //Характеристик  значит нет
                        {
                            if (reader["retail_price"] != null)
                            {
                                label_price_value.Text = reader["retail_price"].ToString().Replace(",", ".");
                            }
                            else
                            {
                                label_price_value.Text = "";// reader["retail_price"].ToString().Replace(",", ".");
                            }
                        }
                        else
                        {
                            if (reader["retail_price_characteristic"] != null)
                            {
                                label_price_value.Text = reader["retail_price_characteristic"].ToString().Replace(",", ".");
                            }
                            else
                            {
                                label_price_value.Text = "";
                            }

                        }

                        //получить количество 
                        if (characteristic_guid != "")
                        {
                            if (num_box == "")
                            {
                                query = "SELECT quantity_shop FROM dt WHERE guid='" +
                                    guid + "' AND tovar_code=" + tovar_code + " AND characteristic = '" + characteristic_guid + "'";
                            }
                            else
                            {
                                query = "SELECT quantity_shop FROM dt WHERE guid='" +
                                                                   guid + "' AND tovar_code=" + tovar_code + " AND characteristic = '" + characteristic_guid + "' AND box='" + num_box + "' ";
                            }
                        }
                        else
                        {
                            if (num_box == "")
                            {
                                query = "SELECT quantity_shop FROM dt WHERE guid='" + guid + "' AND tovar_code=" + tovar_code;
                            }
                            else
                            {
                                query = "SELECT quantity_shop FROM dt WHERE guid='" + guid + "' AND tovar_code=" + tovar_code+ " AND box='" + num_box + "' ";;
                            }
                        }
                        command = new SQLiteCommand(query, conn);
                        result_query = command.ExecuteScalar();
                        command.Dispose();
                        //if (Convert.ToInt32(result_query) != 0)
                        
                        if (result_query != null)
                        {
                            txtB_tovar.Text = tovar_code + "," + reader["tovar_name"].ToString() +";"+
                                reader["characteristic_name"].ToString();
                            label_количество_в_магазине.Text = " в коробке  " +get_quantity_shop() ;
                            label_количество_в_1с.Text =  get_quantity_1c(); 

                        }
                        else//В документе нет этой строчки
                        {
                            if (its_new == 0)
                            {
                                if (typ_doc != "5")
                                {
                                    txtB_tovar.Text = tovar_code + ", " + reader["tovar_name"].ToString() + ";" +
                                        reader["characteristic_name"].ToString() +
                                        " \r\n ОТСУТСТВУЕТ В ЭТОЙ КОРОБКЕ ";
                                    PlaySound ps = new PlaySound();
                                    ps.PlaySound_WAV("\\Windows\\exclam.wav");
                                    if (typ_doc == "2")
                                    {
                                        tovar_code = "";
                                        txtB_quantity.Text = "0";
                                        txtB_input_barcode.Focus();
                                    }
                                }
                                else//внести запись в документ
                                {

                                }
                            }
                            else//Новыми могут быть только поступления
                            {
                                txtB_tovar.Text = tovar_code + "," + reader["tovar_name"].ToString() + ";" +
                              reader["characteristic_name"].ToString();
                                label_количество_в_магазине.Text = " в коробке  " + get_quantity_shop();
                                label_количество_в_1с.Text = get_quantity_1c();  
                            }
                            //ps.PlaySound_WAV("\\Windows\\exclam.wav");
                            //ps.PlaySound_WAV("\\Windows\\freefall.wav");
                            //ps.PlaySound_WAV("\\Windows\\freefall.wav");
                            //ps.PlaySound_WAV("\\Windows\\freefall.wav");
                        }
                    }
                    reader.Close();
                    command.Dispose();
                }

                else
                {
                    tovar_not_found();
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
            Program.write_log("find_barcode_or_code_in_tovar2");   
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Program.write_log("btn_cancel_Click1");   
            this.Close();
            Program.write_log("btn_cancel_Click2");   
        }
        
        protected override void OnKeyDown(KeyEventArgs e)
        {
            Program.write_log("OnKeyDown1");   
           // base.OnKeyDown(e);
            if (e.KeyCode == Keys.Z)
            {
                txtB_input_barcode.Focus();
                write_record();
                txtB_input_barcode.Text = "";
                txtB_input_barcode.Focus();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.F15)
            {
                if (txtB_quantity.Text.IndexOf("-") == -1)
                {
                    txtB_quantity.Text = "-" + txtB_quantity.Text;
                    if (txtB_quantity.Text.Trim().Length > 1)
                    {
                        txtB_quantity.Select(1, txtB_quantity.Text.Length - 1);
                    }
                }
 
            }
            else if (e.KeyCode == Keys.F14)
            {
                txtB_quantity.Text = (Convert.ToInt64(get_quantity_1c()) - Convert.ToInt64(get_quantity_shop())).ToString();
            }
            Program.write_log("OnKeyDown2");   
        }        


        private void btn_complete_Click(object sender, EventArgs e)
        {
            Program.write_log("btn_complete_Click1");   
            SQLiteConnection conn = Program.ConnectForDataBase();

            try
            {
                conn.Open();
                string query = " UPDATE dh SET status=2 WHERE guid='"+guid+"'";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.ExecuteNonQuery();
                command.Dispose();
                conn.Close();
                this.Close();
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
            Program.write_log("btn_complete_Click2");   
        }
    }
}