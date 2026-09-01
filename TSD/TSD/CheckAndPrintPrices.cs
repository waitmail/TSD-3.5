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
    public partial class CheckAndPrintPrices : Form
    {

        public string guid = "";
        public string tovar_code = "";//Отсканированная последняя позиция
        public string characteristic_guid = "";//Отсканированная последняя позиция
        public bool close_this_form = false;
        public string typ_doc = "";
        public int its_new = 0;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private bool verification = true;
        //TextBox dayTextBox = new TextBox { Width = 30, Location = new Point(10, 250)};
        //TextBox monthTextBox = new TextBox { Width = 30, Location = new Point(50, 250)};
        //TextBox yearTextBox = new TextBox { Width = 50, Location = new Point(90, 250)};
        
        public CheckAndPrintPrices()
        {
            InitializeComponent();
            this.txtB_input_barcode.KeyPress += new KeyPressEventHandler(txtB_input_barcode_KeyPress);
            this.txtB_quantity.KeyPress += new KeyPressEventHandler(txtB_quantity_KeyPress);
            this.KeyPreview = true;
            timer.Interval = 3000;
            timer.Tick += new EventHandler(timer_Tick);
            set_label_product_expiration_date();            
            this.Load += new EventHandler(CheckAndPrintPrices_Load);           
        }

        private void CheckAndPrintPrices_Load(object sender, EventArgs e)
        {
            if (typ_doc == "5")
            {
                btn_enter_date.Visible = true;
                label_product_expiration_date.Visible = true;
                if (typ_doc == "5")
                {
                    label_ценников.Text = "Товаров";
                    this.Text = "Сбор сроков номенклатуры";
                }
            }
        }

        private void set_label_product_expiration_date()
        {
            label_product_expiration_date.Text = "1900-01-01";
        }

        //private void InitializeDatePicker()
        //{
        //    //TextBox dayTextBox = new TextBox { Width = 30, Location = new Point(10, 10) };
        //    //TextBox monthTextBox = new TextBox { Width = 30, Location = new Point(50, 10) };
        //    //TextBox yearTextBox = new TextBox { Width = 50, Location = new Point(90, 10) };

        //    this.Controls.Add(dayTextBox);
        //    this.Controls.Add(monthTextBox);
        //    this.Controls.Add(yearTextBox);

        //    Button submitButton = new Button { Text = "OK", Location = new Point(150, 250) };
        //    submitButton.Click += (sender, e) =>
        //    {
        //        int day = int.Parse(dayTextBox.Text);
        //        int month = int.Parse(monthTextBox.Text);
        //        int year = int.Parse(yearTextBox.Text);
        //        DateTime selectedDate = new DateTime(year, month, day);
        //        MessageBox.Show("Выбрана дата: " + selectedDate.ToShortDateString());
        //    };

        //    this.Controls.Add(submitButton);
        //}

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Enabled = false;
            panel_tovar_not_found.Visible = false;
            this.Focus();
            this.txtB_input_barcode.Focus();            
        }

        private void txtB_quantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (tovar_code == "")
            {
                //MessageBox.Show(" Товар не определен ");
                //tovar_not_found();
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
                        create_write_or_update_record(true);
                        //write_record();
                        //txtB_total_price_tags.Text = get_quantity_shop();
                        //this.txtB_quantity.Text = "";
                        //this.txtB_input_barcode.Text = "";
                        //tovar_code = "";
                        txtB_input_barcode.Focus();
                    }
                }
            }
            else if  ((e.KeyChar == 'Z' || e.KeyChar == 'z'))
            {
                if (!verification)
                {
                    //create_write_or_update_record(true);
                } 
            }
        }

        private void txtB_input_barcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            //            label_date_scaning.Text = "Сканировано "+DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            if (e.KeyChar == 13)
            {
                if (this.txtB_input_barcode.Text.Length == 0)//||(this.inputbarcode.Text=="\r\n"))//тут еще проверка на минимальность символов
                {
                    //tovar_not_found();                    
                    return;
                }

                if (typ_doc == "5")
                {
                    if (label_product_expiration_date.Text.Trim() == "1900-01-01")
                    {
                        MessageBox.Show(" Вы не указали дату окончания срока годности ");
                        return;
                    }
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

        }

        public void find_barcode_or_code_in_tovar(string barcode)
        {

            //label_количество_в_магазине.Text = " в документе  ";
            //label_количество_в_1с.Text = "";


            txtB_quantity.Focus();
            SQLiteConnection conn = Program.ConnectForDataBase();
            try
            {
                conn.Open();
                string query = "";
                if (barcode.Trim().Length < 8)
                {
                    query = "SELECT code AS tovar_code FROM tovar where code=@barcode"; //LEFT JOIN characteristic 
                }
                else
                {
                    query = "SELECT barcodes.tovar_code FROM barcodes where barcodes.barcode_code=@barcode"; //LEFT JOIN characteristic                     
                }

                //SQLiteParameter _barcode = new SQLiteParameter("barcode", SqlDbType.NVarChar);
                //_barcode.Value = barcode;

                SQLiteParameter _barcode = new SQLiteParameter("barcode", barcode);
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.Add(_barcode);
                object result_query = command.ExecuteScalar();
                command.Dispose();

                if (result_query != null)
                {
                    tovar_code = result_query.ToString();//присвоили локальной переменной код отсканированной позиции


                    //Проверка на наличие характеристики
                    //SelectCharacteristic sc = new SelectCharacteristic();
                    //sc.Visible = false;
                    //sc.tovar_code = tovar_code;
                    //sc.ShowDialog();

                    //characteristic_guid = Program.CharacteristicGuid;

                    //if (txtB_quantity.Text.Trim().Length == 0)
                    //{
                    txtB_quantity.Text = "1";
                    txtB_quantity.Select(0, txtB_quantity.Text.Length);
                    //}

                    query = "SELECT tovar.name AS tovar_name ,characteristic.name AS characteristic_name ," +
                        " characteristic.guid ,tovar.retail_price," +
                        " retail_price_characteristic FROM tovar " +
                        " LEFT JOIN characteristic ON tovar.code = characteristic.tovar_code " +
                         " WHERE tovar.code = " + tovar_code +
                         (characteristic_guid == "" ? "" : " AND characteristic.guid='" + characteristic_guid + "'")
                          ;
                    command = new SQLiteCommand(query, conn);

                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["guid"].ToString().Trim() == "") //Характеристик  значит нет
                        {
                            if (reader["retail_price"] != null)
                            {
                                txtB_price_value.Text = reader["retail_price"].ToString().Replace(",", ".");
                            }
                            else
                            {
                                txtB_price_value.Text = "";// reader["retail_price"].ToString().Replace(",", ".");
                            }
                        }
                        else
                        {
                            if (reader["retail_price_characteristic"] != null)
                            {
                                txtB_price_value.Text = reader["retail_price_characteristic"].ToString().Replace(",", ".");
                            }
                            else
                            {
                                txtB_price_value.Text = "";
                            }

                        }

                        //получить количество 
                        if (characteristic_guid != "")
                        {
                            query = "SELECT quantity_shop FROM dt WHERE guid='" +
                                guid + "' AND tovar_code=" + tovar_code + " AND characteristic = '" + characteristic_guid + "'";
                        }
                        else
                        {
                            query = "SELECT quantity_shop FROM dt WHERE guid='" + guid + "' AND tovar_code=" + tovar_code;
                        }
                        command = new SQLiteCommand(query, conn);
                        result_query = command.ExecuteScalar();
                        //if (Convert.ToInt32(result_query) != 0)

                        if (result_query != null)
                        {
                            txtB_tovar.Text = tovar_code + "," + reader["tovar_name"].ToString() + ";" +
                                reader["characteristic_name"].ToString();
                            txtB_total_price_tags.Text = get_quantity_shop();
                            //label_количество_в_магазине.Text = " в документе  " + get_quantity_shop();
                            //label_количество_в_1с.Text = get_quantity_1c();

                        }
                        else
                        {
                            if (its_new == 0)
                            {
                                txtB_tovar.Text = tovar_code + ", " + reader["tovar_name"].ToString() + ";" +
                                    reader["characteristic_name"].ToString() +
                                    " \r\n ОТСУТСТВУЕТ В ЭТОМ ДОКУМЕНТЕ ";
                                PlaySound ps = new PlaySound();
                                ps.PlaySound_WAV("\\Windows\\exclam.wav");
                                if (typ_doc == "2")
                                {
                                    tovar_code = "";
                                    txtB_quantity.Text = "0";
                                    txtB_input_barcode.Focus();
                                }
                            }
                            else//Новыми могут быть только поступления
                            {
                                txtB_tovar.Text = tovar_code + "," + reader["tovar_name"].ToString() + ";" +
                              reader["characteristic_name"].ToString();
                                txtB_total_price_tags.Text = get_quantity_shop();//label_количество_в_магазине.Text = " в документе  " + get_quantity_shop();
                                //label_количество_в_1с.Text = get_quantity_1c();
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
        }

        public string get_quantity_shop()
        {
            string result = "";

            SQLiteConnection conn = Program.ConnectForDataBase();

            try
            {
                conn.Open();
                string query = "";
                if (characteristic_guid != "")
                {
                    query = " SELECT SUM(quantity_shop) FROM dt WHERE guid='" + guid + "'" +
                         " AND tovar_code=" + tovar_code +
                         " AND characteristic='" + characteristic_guid + "' GROUP BY tovar_code,characteristic ";
                }
                else
                {
                    if (typ_doc == "5")
                    {
                        query = " SELECT SUM(quantity_shop) FROM dt WHERE guid='" + guid + "'" +
                                                " AND tovar_code=" + tovar_code + " AND date_expiration='" + label_product_expiration_date.Text + "' GROUP BY tovar_code ";
                    }
                    else
                    {
                        query = " SELECT SUM(quantity_shop) FROM dt WHERE guid='" + guid + "'" +
                                                                       " AND tovar_code=" + tovar_code + " GROUP BY tovar_code ";
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

            return result;
        }


        //private string get_quantity_1c()
        //{
        //    string result = "";


        //    //if (display_quantity == 0)
        //    //{
        //    //    return result;
        //    //}

        //    SQLiteConnection conn = Program.ConnectForDataBase();

        //    try
        //    {
        //        conn.Open();
        //        string query = "";
        //        if (characteristic_guid != "")
        //        {
        //            query = " SELECT SUM(quantity) FROM dt WHERE guid='" + guid + "'" +
        //                 " AND tovar_code=" + tovar_code +
        //                 " AND characteristic='" + characteristic_guid + "' GROUP BY tovar_code,characteristic ";
        //        }
        //        else
        //        {
        //            query = " SELECT SUM(quantity) FROM dt WHERE guid='" + guid + "'" +
        //                                    " AND tovar_code=" + tovar_code + " GROUP BY tovar_code ";
        //        }

        //        SQLiteCommand command = new SQLiteCommand(query, conn);
        //        object result_query = command.ExecuteScalar();
        //        if (result_query != null)
        //        {
        //            result = result_query.ToString();
        //        }
        //        command.Dispose();
        //        conn.Close();
        //    }
        //    catch
        //    {

        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //    }

        //    return result;
        //}


        //private bool check_positive_quantity(string quantity_shop)
        //{
        //    bool result = true;

        //    string quantity = get_quantity_shop();
        //    if (quantity.Length == 0)
        //    {
        //        quantity = "0";
        //    }

        //    if (quantity == "0")
        //    {

        //    }
        //    else
        //    {
        //        if (Convert.ToInt32(quantity_shop) * -1 > Convert.ToInt32(quantity))
        //        {
        //            result = false;
        //        }
        //    }

        //    return result;
        //}

        private bool check_positive_quantity(string quantity_shop)
        {
            bool result = true;

            string quantity = get_quantity_shop();
            if (quantity.Length == 0)
            {
                quantity = "0";
            }

            int currentQuantityInDb = Convert.ToInt32(quantity);
            int inputQuantity = Convert.ToInt32(quantity_shop);

            // Если вводят отрицательное число
            if (inputQuantity < 0)
            {
                // Берем модуль вводимого числа (например, -1 превращаем в 1)
                int absInputQuantity = Math.Abs(inputQuantity);

                // Если то, что хотят отнять, больше чем есть в базе — запрещаем
                if (absInputQuantity > currentQuantityInDb)
                {
                    result = false;
                }
            }

            return result;
        }

        private void update_distribute_quantity(string tovar_code, int quantity_shop, int line_number, SQLiteConnection conn)
        {

            //    SQLiteConnection conn = Program.ConnectForDataBase();

            try
            {
                //conn.Open();
                string query = "";
                if (characteristic_guid == "")
                {
                    query = " UPDATE dt SET its_sent=1,quantity_shop=" +
                    quantity_shop.ToString() + " WHERE guid='" + guid +
                    "' AND tovar_code = " + tovar_code +
                    " AND line_number=" + line_number.ToString();
                }
                else
                {
                    query = " UPDATE dt SET its_sent=1,quantity_shop=" +
                  quantity_shop.ToString() + " WHERE guid='" + guid + "'" +
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
        }


        private void write_record()
        {
            //label_количество_в_магазине.Text = " в документе  ";
            //label_количество_в_1с.Text = "";
            // --- ЖЕСТКАЯ ПРОВЕРКА ДАТЫ ДЛЯ ТИПА 5 ---
            if (typ_doc == "5")
            {
                if (label_product_expiration_date.Text.Trim() == "1900-01-01")
                {
                    MessageBox.Show(" Сначала выберите дату окончания срока годности! ");
                    txtB_input_barcode.Focus();
                    return; // Прерываем выполнение, в базу ничего не запишется
                }
            }
            // -----------------------------------------
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
            if (Convert.ToInt32(txtB_quantity.Text.Trim()) == 0)
            {
                MessageBox.Show(" Количество должно быть больше нуля ");
                txtB_quantity.Text = "";
                txtB_input_barcode.Focus();
                return;
            }
            if (tovar_code == "")
            {
                //MessageBox.Show(" Товар не определен ");
                tovar_not_found();
                txtB_input_barcode.Focus();
                return;
            }
            if (typ_doc == "5")
            {
                if (label_product_expiration_date.Text.Trim() == "1900-01-01")
                {
                    MessageBox.Show(" Вы не указали дату окончания срока годности ");
                    return;
                }
            }


            SQLiteConnection conn = Program.ConnectForDataBase();

            try
            {
                conn.Open();
                string query = "";
                int quantity_shop = Convert.ToInt32(txtB_quantity.Text.Trim());
                quantity_shop += Convert.ToInt32(get_quantity_shop() == "" ? "0" : get_quantity_shop());

                if (typ_doc == "4")
                {
                    if (quantity_shop > 9)
                    {
                        MessageBox.Show("Количество ценников не может быть больше 9-ти");
                        return;
                    }
                }               

                //перед обновлением по количеству надо понять есть такой товар в документе или нет
                if (characteristic_guid != "")
                {
                    //query = "SELECT COUNT(*) FROM dt WHERE guid ='" + guid + "' AND tovar_code=" + tovar_code + " AND characteristic='" + characteristic_guid + "'";
                    query = "SELECT line_number FROM dt WHERE guid ='" + guid + "' AND tovar_code=" + tovar_code + " AND characteristic='" + characteristic_guid + "'";

                }
                else
                {
                    if (typ_doc == "4")
                    {
                        query = "SELECT line_number FROM dt WHERE guid ='" + guid + "' AND tovar_code=" + tovar_code;
                    }
                    else if (typ_doc == "5")
                    {
                        query = "SELECT line_number FROM dt WHERE guid ='" + guid + "' AND tovar_code=" + tovar_code + " AND date_expiration ='" + label_product_expiration_date.Text+"'";
                    }
                }

                SQLiteCommand command = new SQLiteCommand(query, conn);
                object result_query = command.ExecuteScalar();
                int line_number = -1;
                if (result_query != null)
                {
                    line_number = Convert.ToInt16(result_query);
                }
                
                command.Dispose();
                if (line_number > -1)//таких товаров больше 1-го необходимо распределить это количество по строка
                {
                    update_distribute_quantity(tovar_code, quantity_shop, line_number, conn);


                    ////if (conn.State == ConnectionState.Open)
                    ////{
                    ////    conn.Close();
                    ////}

                    //if (quantity_shop > 0)
                    //{
                    //    distribute_positve_quantity(quantity_shop, conn);//распределить положительное количество
                    //}
                    //else
                    //{
                    //    distribute_negative_quantity(quantity_shop, conn);//распределить отрицательное количество
                    //}
                }
                else //Этой позиции нет в документе надо ее добавить 
                {
                    if (characteristic_guid != "")
                    {
                        //query = " INSERT INTO dt(guid,tovar_code,characteristic,quantity,quantity_shop,price_buy,price,line_number,its_sent)VALUES(@guid,@tovar_code,@characteristic,@quantity,@quantity_shop,@price_buy,@price,@line_number,@its_sent);";
                        //query = " INSERT INTO dt(guid,tovar_code,characteristic,quantity,quantity_shop,price_buy,price)VALUES(@guid,@tovar_code,@characteristic,@quantity,@quantity_shop,@price_buy,@price);";
                        query = " INSERT INTO dt(guid,tovar_code,characteristic,quantity,quantity_shop,price_buy,price,line_number,its_sent,date)VALUES('" +
                        guid + "'," +
                        tovar_code + ",'" +
                        characteristic_guid + "'," +
                        "0" + "," +
                        quantity_shop.ToString() + "," +
                        "0" + "," +
                        "0" + "," +
                        get_new_line_number() + "," +
                        "1" + ",'"+
                        label_product_expiration_date.Text.Trim()+"')";
                        //yearTextBox.Text+"-"+monthTextBox.Text+"-"+dayTextBox.Text+"');";
                    }
                    else
                    {
                        //query = " INSERT INTO dt(guid,tovar_code,quantity,quantity_shop,price_buy,price,line_number,its_sent)VALUES(@guid,@tovar_code,@quantity,@quantity_shop,@price_buy,@price,@line_number,@its_sent);";
                        //query = " INSERT INTO dt(guid,tovar_code,quantity,quantity_shop,price_buy,price)VALUES(@guid,@tovar_code,@quantity,@quantity_shop,@price_buy,@price);";
                        query = " INSERT INTO dt(guid,tovar_code,quantity,quantity_shop,price_buy,price,line_number,its_sent,date_expiration)VALUES('" +
                        guid + "'," +
                        tovar_code + "," +
                        "0" + "," +
                        quantity_shop.ToString() + "," +
                        "0" + "," +
                        "0" + "," +
                        get_new_line_number() + "," +
                        "1" +",'"+
                        label_product_expiration_date.Text.Trim() + "')";
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
                    query = " SELECT tovar.name AS tovar_name,characteristic.name AS characteristic_name  FROM tovar LEFT JOIN characteristic " +
                    " ON tovar.code=characteristic.tovar_code WHERE code=" + tovar_code +
                    " AND characteristic.guid='" + characteristic_guid + "'";
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
                    query = " SELECT name FROM tovar WHERE code=" + tovar_code;
                    command = new SQLiteCommand(query, conn);
                    tovar_name = command.ExecuteScalar().ToString();
                }

                //НАЧАЛО Обновление записей на форме
                txtB_tovar.Text = tovar_code + ", " + tovar_name;
                txtB_total_price_tags.Text = get_quantity_shop();
                //label_количество_в_магазине.Text = " в документе  " + get_quantity_shop();
                //label_количество_в_1с.Text = get_quantity_1c();
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

                query = "UPDATE dh SET status=1 WHERE guid='" + guid + "'";
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

            label_product_expiration_date.Text = "1900-01-01";
        }

        private void tovar_not_found()
        {
            //if (this.its_new == 0)
            //{
                panel_tovar_not_found.Visible = true;
                panel_tovar_not_found.BringToFront();               
                timer.Enabled = true;
                txtB_tovar.Text = "";
                txtB_total_price_tags.Text = "";
                tovar_code = "";
                txtB_input_barcode.Text = "";
                txtB_quantity.Text = "";
                PlaySound ps = new PlaySound();
                ps.PlaySound_WAV("\\Windows\\exclam.wav");
                ps.PlaySound_WAV("\\Windows\\exclam.wav");
            //}
        }


        /// <summary>
        /// При добавлении новой строки
        /// нужно получить новый номер
        /// </summary>
        /// <returns></returns>
        private string get_new_line_number()
        {
            string result = "1";

            SQLiteConnection conn = Program.ConnectForDataBase();

            try
            {
                conn.Open();
                string query = "SELECT MAX(line_number) FROM dt WHERE guid='" + guid + "'";
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

            return result;
        }

        private void create_write_or_update_record(bool write)
        {
            txtB_input_barcode.Focus();
            if (typ_doc == "4")
            {
                if (!write)//Если истина тогда безусловная запись 
                {
                    if (verification)
                    {
                        return;
                    }
                }
            }

            write_record();
            //txtB_total_price_tags.Text = get_quantity_shop();
            txtB_total_price_tags.Text = "";
            this.txtB_quantity.Text = "";
            txtB_input_barcode.Text = "";
            txtB_price_value.Text = "";
            txtB_tovar.Text = "";
            tovar_code = "";
            txtB_input_barcode.Focus();
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            // base.OnKeyDown(e);
            if (e.KeyCode == Keys.Z)
            {
                //txtB_input_barcode.Focus();
                //if (verification)
                //{
                //    return;
                //}
                //write_record();
                //txtB_total_price_tags.Text = get_quantity_shop();
                //txtB_input_barcode.Text = "";
                //txtB_input_barcode.Focus();

                create_write_or_update_record(false);

                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.ControlKey)
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
                verification = true;
                this.Text = "Проверка и печать ценников проверка";
            }
            else if (e.KeyCode == Keys.F15)
            {
                verification = false;
                this.Text = "Проверка и печать ценников добавление";
            }
        }

        private void btn_enter_date_Click(object sender, EventArgs e)
        {           
            //create_write_or_update_record();
            //EnterExpirationDate enterExpirationDate = new EnterExpirationDate();
            //DialogResult dr = enterExpirationDate.ShowDialog();
            //if (dr == DialogResult.OK)
            //{
            //    this.label_product_expiration_date.Text = enterExpirationDate.date_expiration;
            //}
            //txtB_input_barcode.Focus();
            //GC.Collect();
            //// Ожидание завершения всех финализаторов
            //GC.WaitForPendingFinalizers();

            create_write_or_update_record(true);

            using (EnterExpirationDate enterExpirationDate = new EnterExpirationDate())
            {
                DialogResult dr = enterExpirationDate.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    this.label_product_expiration_date.Text = enterExpirationDate.date_expiration;
                }
            }

            txtB_input_barcode.Focus();
        }       
    }
}