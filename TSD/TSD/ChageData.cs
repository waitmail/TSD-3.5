using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.IO.Compression;

namespace TSD
{
    public partial class ChageData : Form
    {
        private PowerStatus ps = new PowerStatus();

        public ChageData()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.Paint += new PaintEventHandler(ChageData_Paint);
        }


        void ChageData_Paint(object sender, PaintEventArgs e)
        {
            label_powerstatus.Text = ps.ReportPowerStatus("main") + " | " + ps.ReportPowerStatus("");
        }


        private bool insert_value_shop_in_databse(string shop)
        {
            bool result = true;

            SQLiteConnection conn = null;
            try
            {
                conn = TSD.Program.ConnectForDataBase();
                conn.Open();
                SQLiteCommand command = null;

                string query = " DELETE FROM shop; ";
                command = new SQLiteCommand(query, conn);
                command.ExecuteNonQuery();

                query = " INSERT INTO shop(shop) VALUES('" + shop + "');";
                command = new SQLiteCommand(query, conn);
                command.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(" Ошибка при установке значения магазина " + ex.Message);
                result = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Ошибка при установке значения магазина " + ex.Message);
                result = false;
            }


            return result;
        }


        private void btn_update_tmc_Click(object sender, EventArgs e)
        {

            //try
            //{
            //    TSD.DS.DS ds = new TSD.DS.DS();
            //    string device_id = Program.get_device_id();
            //    string received = ds.GetTMCForTSD(device_id);

            //    if (received.Trim() == "0")
            //    {
            //        MessageBox.Show(" Этот ТСД еще не зарегистрирован " + device_id, "Результат запроса");
            //        return;
            //    }
            //    string key = device_id+CryptorEngine.get_count_day_tsd();
            //    //MessageBox.Show(key);
            //    string decrypt_data = CryptorEngine.Decrypt(received, true, key);
            //    received = "";
            //    string shop = decrypt_data.Substring(0, 3);
            //    if (!insert_value_shop_in_databse(shop))
            //    {
            //        MessageBox.Show("Произошли ошибки при загрузке данных, загрузка данных прервана");
            //        return;
            //    }

            //    string tovar = "";//result.Split(


            //    int start_pos = decrypt_data.IndexOf("TOVAR");
            //    int finish_pos = decrypt_data.Substring(start_pos + 5, decrypt_data.Length - start_pos - 5).IndexOf("TOVAR");
            //    if (finish_pos == 0)
            //    {
            //        MessageBox.Show("Получены неполные данные, загрука невозможна");
            //        return;
            //    }
            //    //else
            //    //{
            //    //    MessageBox.Show("Данные получены");
            //    //}

            //    tovar = decrypt_data.Substring(start_pos + 5, finish_pos);

            //    start_pos = decrypt_data.IndexOf("BARCODE");
            //    finish_pos = decrypt_data.Substring(start_pos + 7, decrypt_data.Length - start_pos - 7).IndexOf("BARCODE");
            //    if (finish_pos == 0)
            //    {
            //        MessageBox.Show("Получены неполные данные, загрука невозможна");
            //        return;
            //    }
            //    string barcode = decrypt_data.Substring(start_pos + 7, finish_pos - 1);
            //    string characteristic = "";

            //    start_pos = decrypt_data.IndexOf("CHARACTERISTIC");
            //    finish_pos = decrypt_data.Substring(start_pos + 14, decrypt_data.Length - start_pos - 14).IndexOf("CHARACTERISTIC");
            //    if ((finish_pos != 0) && (finish_pos != -1))
            //    {
            //        characteristic = decrypt_data.Substring(start_pos + 14, finish_pos - 1);
            //    }

            //    //первые 3 символа это код магазина сразу обновляем его в константах

            //    StringBuilder sb = new StringBuilder();
            //    char[] delimiters = new char[] { '|' };
            //    string[] t = tovar.Split(delimiters);
            //    tovar = "";
            //    //Освобождаем память 
            //    decrypt_data = "";


            //    //SQL
            //    SQLiteConnection conn = null;
            //    SQLiteTransaction trans = null;
            //    conn = TSD.Program.ConnectForDataBase();
            //    conn.Open();
            //    trans = conn.BeginTransaction();
            //    SQLiteCommand command = null;

            //    string query = "";
            //    query = "DELETE FROM TOVAR";
            //    command = new SQLiteCommand(query, conn);
            //    command.Transaction = trans;                
            //    command.ExecuteNonQuery();
            //    textBox1.Text = "Загружаются товары ";
            //    for (int i = 0; i < t.Length - 1; i++)
            //    {

            //        if (i % 100 == 0)
            //        {
            //            textBox1.Text = "Обрабатывается товар " + i.ToString() + " из " + t.Length.ToString();
            //        }
            //        query = "INSERT INTO tovar(code,name,retail_price,purchase_price,its_deleted,nds) VALUES(" + t[i] + ")";
            //        command = new SQLiteCommand(query, conn);
            //        command.Transaction = trans;
            //        command.ExecuteNonQuery();
            //    }
            //    textBox1.Text += "Товары загрузились \r\n";
            //    //Освобождаем память
            //    t = null;
            //    string[] b = barcode.Split(delimiters);

            //    query = "DELETE FROM barcode";
            //    command = new SQLiteCommand(query, conn);
            //    command.Transaction = trans;
            //    command.ExecuteNonQuery();

            //    textBox1.Text = "Загружаются штрихкоды \r\n";

            //    for (int i = 0; i < b.Length - 1; i++)
            //    {
            //        if (i % 100 == 0)
            //        {
            //            textBox1.Text = "Загружаются штрихкоды " + i.ToString() + " из " + b.Length.ToString();
            //        }

            //        //b[i]
            //        query = "INSERT INTO barcode(tovar_code,barcode) VALUES(" + b[i] + ")";
            //        command = new SQLiteCommand(query, conn);
            //        command.Transaction = trans;
            //        command.ExecuteNonQuery();
            //    }

            //    textBox1.Text = "Штрихкод загрузился \r\n";
            //    textBox1.Text = "Загружаются характеристики \r\n";
            //    if (characteristic != "")
            //    {
            //        string[] c = characteristic.Split(delimiters);
            //        query = "DELETE FROM characteristic";
            //        command = new SQLiteCommand(query, conn);
            //        command.ExecuteNonQuery();
            //        for (int i = 0; i < c.Length - 1; i++)
            //        {
            //            if (i % 100 == 0)
            //            {
            //                textBox1.Text = "Загружаются характеристики " + i.ToString() + " из " + c.Length.ToString();
            //            }
            //            query = "INSERT INTO characteristic(tovar_code, guid, name, retail_price_characteristic) VALUES(" + c[i] + ")";
            //            command = new SQLiteCommand(query, conn);
            //            command.Transaction = trans;
            //            command.ExecuteNonQuery();
            //        }
            //    }
            //    textBox1.Text += "Характеристики обработались \r\n";
            //    MessageBox.Show("Данные спешно загружены !!!");
            //    trans.Commit();
            //    conn.Close();
            //}
            //catch (SQLiteException ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //catch (Exception ex)
            //{

            //}
            //finally
            //{
            //    if()

            //}

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.D0)
            {
                btn_execute_full_sinhronization_Click(null, null);
            }
            if (e.KeyCode == Keys.D1)
            {
                btn_load_documents_1c_Click(null, null);
                //btn_unload_documents_Click(null, null);
                //upload_documents();
            }
            //if (e.KeyCode == Keys.D2)
            //{
            //    btn_load_documents_1c_Click(null, null);
            //    //download_documents();
            //}
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns>1 такой документ есть(был удален при проверке), 0 такого документа нет , -1 ошибка , 1000 TSD не зарегистрирован</returns>
        //private string exists_document_in_central_base()
        //{
        //    this.textBox1.Text = " \r\n Проверка наличия документа в центральной базе ";
        //    string result = "1";
        //    bool errors = false;
        //    string device_id = Program.get_device_id();

        //    SQLiteConnection conn = Program.ConnectForDataBase();

        //    try
        //    {
        //        conn.Open();
        //        string query = " SELECT guid FROM dh WHERE status=2";
        //        SQLiteCommand command = new SQLiteCommand(query, conn);
        //        SQLiteDataReader reader = command.ExecuteReader();
        //        DS.DS ds = new TSD.DS.DS();
        //        string exists_document = "";
        //        while (reader.Read())
        //        {
        //            //string device_id = Program.get_device_id();
        //            //textBox1.Text = "Загрузка справочников, запрос данных";
        //            string key = device_id + CryptorEngine.get_count_day_tsd();

        //            string data = CryptorEngine.Encrypt((device_id + reader["guid"].ToString() + device_id),true,key);
        //            //StreamWriter sw = new StreamWriter("\\Storage Card\\Test.txt");
        //            //sw.WriteLine(data);                    
        //            //sw.Close();
        //            exists_document = ds.GetExistDocumentTSD(device_id, data);                    
        //            //this.textBox1.Text = " \r\n Ответ сервера " + exists_document;
        //            if (exists_document == "-1")
        //            {
        //                textBox1.Text = " \r\n Произошли ошибки при проверка документов в центральной базе, выгрузка прервана ";
        //                errors = true;
        //                break;
        //            }
        //            else if (exists_document == "1000")
        //            {
        //                textBox1.Text = " \r\n ТСД не зарегистрирован, выгрузка прервана ";
        //                errors = true;
        //                break;
        //            }
        //            else if (exists_document == "1")
        //            {
        //                textBox1.Text = " \r\n Обнаружен документ уже имеющийся в центральной базе, меняем статус на тсд , выгрузка продолжается ";
        //                delete_document_on_status("2", reader["guid"].ToString());
        //            }
        //        }
        //        reader.Close();
        //        conn.Close();                
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        errors = true;
        //        MessageBox.Show(ex.Message);                
        //    }
        //    catch (Exception ex)
        //    {
        //        errors = true;
        //        MessageBox.Show(ex.Message);                
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //    }
        //    if (errors)
        //    {             
        //        result = "-1";
        //    }
        //    else
        //    {            
        //        result = "1";
        //    }            

        //    return result; 
        //}




        private void btn_Esc_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_load_documents_1c_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != MessageBox.Show(" Загрузить документы ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
            {
                return;
            }
            //Program.UploadLogs();
            //download_documents();
            //DownloadDocumentsJson();
            download_documents_json();
        }

        /// <summary>
        /// в первой версии документы удалялись, тепер у них сначала меняется статус
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private bool delete_document_on_status(string status)
        {
            //this.textBox1.Text = " Документы пока не удаляются ";
            //return false;
            bool result = true;

            SQLiteConnection conn = Program.ConnectForDataBase();

            SQLiteTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                //string query = "UPDATE dt SET status = 3 WHERE guid in (SELECT guid FROM dh WHERE status=2)";
                //SQLiteCommand command = new SQLiteCommand(query, conn);
                //command.Transaction = trans;
                //command.ExecuteNonQuery();
                string query = "UPDATE dh SET status=3 WHERE status=" + status;
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Transaction = trans;
                command.ExecuteNonQuery();
                trans.Commit();
                conn.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(" Ошибка при удалении отправленных документов " + ex.Message);
                result = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Ошибка при удалении отправленных документов " + ex.Message);
                result = false;
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

        /// <summary>
        /// в первой версии документы удалялись, тепер у них сначала меняется статус
        /// </summary>
        /// <param name="status"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        private bool delete_document_on_status(string status, string guid)
        {

            //this.textBox1.Text = "Документы пока не удалюятся";
            //return false;

            bool result = true;

            SQLiteConnection conn = Program.ConnectForDataBase();
            SQLiteTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                //string query = "UPDATE dt SET status=3 WHERE guid ='"+guid+"'";
                //SQLiteCommand command = new SQLiteCommand(query, conn);
                //command.Transaction = trans;
                //command.ExecuteNonQuery();
                string query = "UPDATE dh SET status=3 WHERE  guid='" + guid + "'";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Transaction = trans;
                command.ExecuteNonQuery();
                trans.Commit();
                conn.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(" Ошибка при удалении отправленных документов " + ex.Message);
                result = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Ошибка при удалении отправленных документов " + ex.Message);
                result = false;
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

        private void btn_unload_documents_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != MessageBox.Show(" Выгрузить документы ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
            {
                return;
            }
            //            upload_documents();
            upload_documents_json();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>1 есть документы на отправку , 0 нет документов на отправку -1 произошли ошибки при определении если документы на отправку</returns>
        private string check_upload_documents()
        {
            string result = "1";

            SQLiteConnection conn = Program.ConnectForDataBase();

            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM dh WHERE status=2";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                if (Convert.ToInt64(command.ExecuteScalar()) == 0)
                {
                    result = "0";
                }
                conn.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
                result = "-1";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                result = "-1";
            }

            return result;
        }

        public class DocumentHeader
        {
            public string Type { get; set; }
            public string Date { get; set; }
            public string Guid { get; set; }
            public string Info_1s { get; set; }
            //public string Comment { get; set; }            
        }

        public class DocumentTable
        {
            public string Guid { get; set; }
            public string LineNumber { get; set; }
            public string TovarCode { get; set; }
            public string QuantityShop { get; set; }
            public string Quantity { get; set; }
            public string PriceBuy { get; set; }
            public string Price { get; set; }
            public string DateExpiration { get; set; }
        }

        public class UploadDocuments
        {
            public string Shop { get; set; }
            public List<DocumentHeader> ListHeaders { get; set; }
            public List<DocumentTable> ListTables { get; set; }
        }

        private bool upload_documents_json()
        {
            //MessageBox.Show("Выгружаем документы");
            this.textBox1.Text += " Попытка выгрузить документы ";

            bool result = true;

            string result_check_upload_documents = check_upload_documents();
            //MessageBox.Show("Проверили документ "+result_check_upload_documents);
            //textBox1.Text += " Проверили документ " + result_check_upload_documents + " \r\n";
            if (result_check_upload_documents == "-1")
            {
                result = false;
                this.textBox1.Text += "\r\n Ошибки при проверке документа в центральной базе, выгрузка прервана";
                return result;
            }
            else if (result_check_upload_documents == "0")
            {
                this.textBox1.Text += "\r\n Нет документов для выгрузки ";
                result = true;
                return result;
            }

            this.textBox1.Text += "\r\n Есть документы для выгрузки ";
            //получить guid устройства
            string device_id = Program.get_device_id();

            //получить код магазина
            string shop = Program.get_code_shop();
            if (shop == "")
            {
                this.textBox1.Text += "\r\n Код магазина не найден, выгрузка прервана ";
                result = false;
                return result;
            }

            UploadDocuments upload_documents = new UploadDocuments();
            upload_documents.Shop = shop;
            upload_documents.ListHeaders = new List<DocumentHeader>();
            upload_documents.ListTables = new List<DocumentTable>();

            SQLiteConnection conn = Program.ConnectForDataBase();

            try
            {
                conn.Open();
                string query = "SELECT type,date,guid,info_1s FROM dh WHERE status=2";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DocumentHeader documentHeader = new DocumentHeader();
                    documentHeader.Guid = reader["guid"].ToString();
                    //documentHeader.Date = Convert.ToDateTime(reader["date"]).ToString("dd-MM-yyyy");
                    object dateObj = reader["date"];
                    string dateStr = (dateObj == DBNull.Value) ? string.Empty : dateObj.ToString();

                    if (string.IsNullOrEmpty(dateStr) || dateStr.Trim().Length == 0)
                    {
                        documentHeader.Date = DateTime.Now.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        try
                        {
                            documentHeader.Date = DateTime.Parse(dateStr).ToString("dd-MM-yyyy");
                        }
                        catch
                        {
                            documentHeader.Date = DateTime.Now.ToString("dd-MM-yyyy");
                        }
                    }
                    documentHeader.Info_1s = reader["info_1s"].ToString();
                    documentHeader.Type = reader["type"].ToString();
                    upload_documents.ListHeaders.Add(documentHeader);
                }
                reader.Close();

                //query = " SELECT guid,line_number,tovar_code,characteristic,quantity_shop,quantity,price_buy,price,date_expiration from dt WHERE guid in (SELECT guid FROM dh WHERE status=2) ";// 
                query = " SELECT guid,line_number,tovar_code,characteristic,quantity_shop,quantity,price_buy,price,CAST(date_expiration AS TEXT) as date_expiration from dt WHERE guid in (SELECT guid FROM dh WHERE status=2) ";// 
                command = new SQLiteCommand(query, conn);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DocumentTable documentTable = new DocumentTable();
                    documentTable.Guid = reader["guid"].ToString();
                    documentTable.LineNumber = (reader["line_number"].ToString() == "" ? "0" : reader["line_number"].ToString());
                    documentTable.TovarCode = reader["tovar_code"].ToString();
                    documentTable.QuantityShop = reader["quantity_shop"].ToString();
                    documentTable.Quantity = reader["quantity"].ToString();
                    documentTable.PriceBuy = reader["price_buy"].ToString().Replace(",", ".");
                    documentTable.Price = reader["price"].ToString().Replace(",", ".");
                    //documentTable.DateExpiration = Convert.ToDateTime(reader["date_expiration"]).ToString("dd-MM-yyyy");
                    object dateExpObj = reader["date_expiration"];
                    string dateExpStr = (dateExpObj == DBNull.Value) ? string.Empty : dateExpObj.ToString();

                    // Проверка на пустоту или пробелы (аналог IsNullOrWhiteSpace для CF)
                    if (string.IsNullOrEmpty(dateExpStr) || dateExpStr.Trim().Length == 0)
                    {
                        documentTable.DateExpiration = "1900-01-01"; // Дефолтная дата
                    }
                    else
                    {
                        try
                        {
                            // Аналог TryParse для CF
                            DateTime expDate = DateTime.Parse(dateExpStr);
                            documentTable.DateExpiration = expDate.ToString("dd-MM-yyyy");
                        }
                        catch
                        {
                            // Если в базе лежит мусор, который не может распарситься как дата
                            documentTable.DateExpiration = "1900-01-01";
                        }
                    }
                    upload_documents.ListTables.Add(documentTable);
                }
                reader.Close();
                conn.Close();

                WS.WS ds = new TSD.WS.WS();
                ds.Timeout = 12000000;
                string key = device_id + CryptorEngine.get_count_day_tsd();
                string data = JsonConvert.SerializeObject(upload_documents, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                string encrypt_data = CryptorEngine.Encrypt(data, true, key);

                int num_base = Program.GetDbId();
                if (num_base == -1)
                {
                    return false;
                }
                string result_upload = "";
                try
                {
                    result_upload = ds.UploadJsonDocumentTSD(device_id, encrypt_data, num_base);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                if (result_upload == "1")//Удалить документы со статусом 2
                {
                    textBox1.Text += " \r\n Документы успешно переданы ";
                    delete_document_on_status("2");
                }
                else
                {
                    textBox1.Text += " \r\n Не удалось передать документы " + result_upload;
                }
            }
            catch (SQLiteException ex)
            {
                //MessageBox.Show(ex.Message, "SQLiteException");
                //MessageBox.Show(ex.ToString(), "SQLiteException");
                string err = ex.ToString();
                MessageBox.Show(err, "SQLiteException");
                textBox1.Text += "\r\nОШИБКА SQLite:\r\n" + err + "\r\n";
                result = false;

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Exception");
                //MessageBox.Show(ex.ToString(), "Exception");
                string err = ex.ToString();
                MessageBox.Show(err, "Exception");
                textBox1.Text += "\r\nОШИБКА:\r\n" + err + "\r\n";
                result = false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            this.textBox1.Text += " \r\n Завершение попытки выгрузить документы ";

            return result;
        }


        /// <summary>
        /// выгрузка документов со статусом 2 в центральный офис
        /// </summary>
        /// <returns></returns>
        private bool upload_documents1()
        {
            //MessageBox.Show("Выгружаем документы");
            this.textBox1.Text += " Попытка выгрузить документы ";

            bool result = true;

            //if (exists_document_in_central_base() != "1") //проверка
            //{
            //    this.textBox1.Text += " \r\n Произошли ошибки при проверке наличия документов в центральной базе, выгрузка документов прервана  ";
            //    return false;
            //}

            //try
            //{

            string result_check_upload_documents = check_upload_documents();
            //MessageBox.Show("Проверили документ "+result_check_upload_documents);
            //textBox1.Text += " Проверили документ " + result_check_upload_documents + " \r\n";
            if (result_check_upload_documents == "-1")
            {
                result = false;
                this.textBox1.Text += "\r\n Ошибки при проверке документа в центральной базе, выгрузка прервана";
                return result;
            }
            else if (result_check_upload_documents == "0")
            {
                this.textBox1.Text += "\r\n Нет документов для выгрузки ";
                result = true;
                return result;
            }


            this.textBox1.Text += "\r\n Есть документы для выгрузки ";
            //получить guid устройства
            string device_id = Program.get_device_id();

            //получить код магазина
            string shop = Program.get_code_shop();
            if (shop == "")
            {
                this.textBox1.Text += "\r\n Код магазина не найден, выгрузка прервана ";
                result = false;
                return result;
            }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(" выгрузка документов "+ ex.Message);
            //}

            SQLiteConnection conn = Program.ConnectForDataBase();

            try
            {
                //MessageBox.Show("1");
                conn.Open();
                string query = "SELECT type,date,guid,info_1s FROM dh WHERE status=2";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataReader reader = command.ExecuteReader();
                StringBuilder sb = new StringBuilder();
                sb.Append("SHAPKA");
                //поля на сервере 
                //shop,type,tsd_ident,date_tsd,guid_1s,comment,datetime_unloading
                while (reader.Read())
                {
                    string s = "'" + shop + "'," +
                        reader["type"].ToString() + ",'" +
                        device_id + "','" +
                        reader.GetDateTime(1).ToString("dd-MM-yyyy HH:mm:ss") + "','" +
                        reader["guid"].ToString() + "','" +
                        //(reader["info_1s"].ToString().Trim().Length > 100 ? reader["info_1s"].ToString().Substring(0, 100) : reader["info_1s"].ToString()) + "'|";
                        reader["info_1s"].ToString() + "'|";
                    sb.Append(s);
                    //break;
                }
                reader.Close();
                sb.Append("SHAPKA");
                //MessageBox.Show("2");

                sb.Append("STROKI");

                //SELECT [guid_1s]      ,[line_number]      ,[tovar_code]      ,[characteristic]      ,[quantity]      ,[quantity_1s]      ,[price_buy]      ,[price]  FROM [cash_8].[dbo].[tsd_docs_table]

                query = " SELECT guid,line_number,tovar_code,characteristic,quantity_shop,quantity,price_buy,price from dt WHERE guid in (SELECT guid FROM dh WHERE status=2) ";// 
                command = new SQLiteCommand(query, conn);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string s = "'" + reader["guid"] + "'," +

                        (reader["line_number"].ToString() == "" ? "0" : reader["line_number"].ToString()) + "," +
                        //reader["line_number"].ToString() + "," +
                        reader["tovar_code"].ToString() + ",'" +
                        reader["characteristic"].ToString() + "'," +
                        reader["quantity_shop"].ToString() + "," + //quantity
                        reader["quantity"].ToString() + "," +      //quantity_1s                        
                        reader["price_buy"].ToString().Replace(",", ".") + "," +
                        reader["price"].ToString().Replace(",", ".") + "|";
                    sb.Append(s);
                    //break; 
                }
                reader.Close();
                conn.Close();
                sb.Append("STROKI");
                //MessageBox.Show("3");

                //System.IO.StreamWriter sw=new System.IO.StreamWriter("\\query.txt");
                //sw.WriteLine(sb.ToString());
                //sw.Close();


                WS.WS ds = new TSD.WS.WS();
                ds.Timeout = 12000000;
                string key = device_id + CryptorEngine.get_count_day_tsd();
                string encrypt_data = CryptorEngine.Encrypt(device_id + sb.ToString() + device_id, true, key);
                //System.IO.StreamWriter sw=new System.IO.StreamWriter("\\query.txt");
                //sw.WriteLine(encrypt_data);
                //sw.Close();

                int num_base = Program.GetDbId();
                if (num_base == -1)
                {
                    return false;
                }
                string result_upload = "";
                try
                {
                    //Временно сложим в файл
                    //string CryptorEngine.Decrypt(device_id,true,key)
                    //System.IO.StreamWriter sw = new System.IO.StreamWriter("\\query.txt");
                    //sw.WriteLine(key);
                    //sw.WriteLine(device_id);
                    //sw.WriteLine(num_base);                                        
                    //sw.WriteLine(CryptorEngine.Encrypt(device_id, true, key));
                    //sw.Close();
                    result_upload = ds.UploadDocumentTSD(device_id, encrypt_data, num_base);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                //this.textBox1.Text = " Ответ сервера по передаче документа " + result_upload;
                if (result_upload == "1")//Удалить документы со статусом 2
                {
                    textBox1.Text += " \r\n Документы успешно переданы ";
                    delete_document_on_status("2");
                }
                else
                {
                    textBox1.Text += " \r\n Не удалось передать документы " + result_upload;
                }
                //MessageBox.Show("Выгрузили документы");
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
                result = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                result = false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            this.textBox1.Text += " \r\n Завершение попытки выгрузить документы ";

            return result;
        }


        public class TMCForTSD
        {
            public string NickShop { get; set; }
            public List<Nomenklatura> ListNomenklatura { get; set; }
            public List<Barcode> ListBarcode { get; set; }
            public List<Characteristic> ListCharacteristic { get; set; }
        }

        public class Nomenklatura
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string RetailPrice { get; set; }
            public string PurchasePrice { get; set; }
            public string ItsDeleted { get; set; }
            public string Nds { get; set; }
        }

        public class Barcode
        {
            public string TovarCode { get; set; }
            public string BarCode { get; set; }
        }

        public class Characteristic
        {
            public string TovarCode { get; set; }
            public string GuidCharacteristic { get; set; }
            public string Name { get; set; }
            public string Price { get; set; }
        }

        public class ParameterDounloadTMC
        {
            public string GuidTSD { get; set; }
            public string DateLoadData { get; set; }
        }


        private string get_date_loading()
        {
            string result = "";

            SQLiteConnection conn = Program.ConnectForDataBase();

            try
            {
                conn.Open();
                string query = " SELECT * FROM constants ";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = reader["date"].ToString();
                }
                reader.Close();
                command.Dispose();
                conn.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ошибка при чтении даты последней загрузки " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при чтении даты последней загрузки " + ex.Message);
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


        private string DecompressString(Byte[] value)
        {
            string resultString = string.Empty;
            if (value != null && value.Length > 0)
            {
                using (MemoryStream stream = new MemoryStream(value))
                using (GZipStream zip = new GZipStream(stream, CompressionMode.Decompress))
                using (StreamReader reader = new StreamReader(zip))
                {
                    resultString = reader.ReadToEnd();
                }
            }
            return resultString;
        }
              

        //Мой большой метод
        private bool download_tmc()
        {            
            bool result = true;

            //SQL
            //SQLiteConnection conn = null;
            SQLiteTransaction trans = null;
            string query = "";
            string error_query = "";
            //int id_db = 0;

            using (SQLiteConnection conn = TSD.Program.ConnectForDataBase())
            {
                try
                {
                    string received = "";
                    string device_id = Program.get_device_id();
                    string key = device_id + CryptorEngine.get_count_day_tsd();                       

                    using (WS.WS ds = new TSD.WS.WS())
                    {
                        ds.Timeout = 200 * 1000;
                        
                        textBox1.Text = "Загрузка справочников, запрос данных";
                        

                        //string CryptorEngine.Decrypt(device_id,true,key)
                        //System.IO.StreamWriter sw = new System.IO.StreamWriter("\\query.txt");
                        //sw.WriteLine(key);
                        //sw.WriteLine(CryptorEngine.Encrypt(device_id, true, key));
                        //sw.Close();

                        int num_base = Program.GetDbId();
                        if (num_base == -1)
                        {
                            return false;
                        }

                        received = ds.GetTMCForTSD(device_id, CryptorEngine.Encrypt(device_id, true, key), num_base);//gaa

                        //string received ="";
                        //string fullAppName = System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase;
                        //string fullAppPath = Path.GetDirectoryName(fullAppName);

                        //UriBuilder uri = new UriBuilder(codeBase);
                        //string path = Uri.UnescapeDataString(uri.Path);
                        //string directoryPath = Path.GetDirectoryName(path);
                        //string[] files = Directory.GetFiles(fullAppPath);
                        //MessageBox.Show(File.Exists(fullAppPath + "\\TMC.txt").ToString());

                        //using (FileStream stream = File.Open(fullAppPath+"\\TMC.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        //{
                        //    using (StreamReader reader = new StreamReader(stream))
                        //    {
                        //        // прочитаем весь текст из файла
                        //        received = reader.ReadToEnd();
                        //    }
                        //}               
                        //"\\Program Files\\TSD\\TMC.txt"

                        //string received=File.OpenRead
                        //ds.Dispose();
                    }
                    textBox1.Text = "Загрузка справочников, запрос данных успешно";

                    if (received.Trim() == "1000")
                    {
                        MessageBox.Show(" Этот ТСД еще не зарегистрирован " + device_id, "Результат запроса");
                        result = false;
                        return result;
                    }
                    else if (received.Trim() == "-2")
                    {
                        MessageBox.Show(" Идет выгрузка данных из 1с, попробуйте синхронизироваться позже.");
                        result = false;
                        return result;
                    }

                    //string key = device_id + CryptorEngine.get_count_day_tsd();
                    //MessageBox.Show(key);
                    textBox1.Text = "Попытка расшифровать данные";
                    //MessageBox.Show(received.Length.ToString());
                    string decrypt_data = CryptorEngine.Decrypt(received, true, key);
                    received = "";
                    string shop = decrypt_data.Substring(device_id.Length, 3);
                    textBox1.Text = shop;
                    if (!insert_value_shop_in_databse(shop))
                    {
                        MessageBox.Show("Произошли ошибки при загрузке данных, загрузка данных прервана");
                        result = false;
                        return result;
                    }

                    textBox1.Text = shop;


                    string tovar = "";//result.Split(


                    int start_pos = decrypt_data.IndexOf("TOVAR");
                    int finish_pos = decrypt_data.Substring(start_pos + 5, decrypt_data.Length - start_pos - 5).IndexOf("TOVAR");
                    if (finish_pos == 0)
                    {
                        MessageBox.Show("Получены неполные данные, загрука невозможна");
                        result = false;
                        return result;
                    }
                    //else
                    //{
                    //    MessageBox.Show("Данные получены");
                    //}

                    tovar = decrypt_data.Substring(start_pos + 5, finish_pos);

                    start_pos = decrypt_data.IndexOf("BARCODE");
                    finish_pos = decrypt_data.Substring(start_pos + 7, decrypt_data.Length - start_pos - 7).IndexOf("BARCODE");
                    if (finish_pos == 0)
                    {
                        MessageBox.Show("Получены неполные данные, загрука невозможна");
                        result = false;
                        return result;
                    }
                    string barcode = decrypt_data.Substring(start_pos + 7, finish_pos - 1);
                    string characteristic = "";

                    start_pos = decrypt_data.IndexOf("CHARACTERISTIC");
                    finish_pos = decrypt_data.Substring(start_pos + 14, decrypt_data.Length - start_pos - 14).IndexOf("CHARACTERISTIC");
                    if ((finish_pos != 0) && (finish_pos != -1))
                    {
                        characteristic = decrypt_data.Substring(start_pos + 14, finish_pos - 1);
                    }

                    ///первые 3 символа это код магазина сразу обновляем его в константах

                    StringBuilder sb = new StringBuilder();
                    char[] delimiters = new char[] { '|' };
                    string[] t = tovar.Split(delimiters);
                    tovar = "";
                    //Освобождаем память 
                    decrypt_data = "";
                    //conn = TSD.Program.ConnectForDataBase();
                    conn.Open();
                    trans = conn.BeginTransaction();
                    SQLiteCommand command = null;
                    textBox1.Text = "Удаляем товары";
                    query = "DELETE FROM TOVAR";
                    command = new SQLiteCommand(query, conn);
                    command.Transaction = trans;
                    command.ExecuteNonQuery();
                    command.Dispose();
                    textBox1.Text = "Загружаются товары ";
                    delimiters = new char[] { '^' };
                    for (int i = 0; i < t.Length - 1; i++)
                    {

                        if (i % 1000 == 0)
                        {
                            textBox1.Text = "Обрабатывается товар " + i.ToString() + " из " + t.Length.ToString();
                        }
                        //query = "INSERT INTO tovar(code,name,retail_price,purchase_price,its_deleted,nds) VALUES(" + t[i] + ")";
                        string[] param = t[i].Replace("'", "").Split(delimiters);
                        //textBox1.Text = t[i];
                        if (i == 0)
                        {
                            query = "INSERT INTO tovar(code,name,retail_price,purchase_price,its_deleted,nds) VALUES(@code,@name,@retail_price,@purchase_price,@its_deleted,@nds)";

                            //textBox1.Text += "i =  " + i.ToString() + "\r\n";
                            //textBox1.Text += "all " +  t[i]+ "\r\n";
                            //textBox1.Text += " code " + param[0] + "\r\n";
                            //textBox1.Text += "name " + param[1] + "\r\n";
                            //textBox1.Text += "retail_price " + param[2] + "\r\n";
                            //textBox1.Text += "purchase_price " + param[3] + "\r\n";
                            //textBox1.Text += "its_deleted " + param[4] + "\r\n";
                            //textBox1.Text += "nds " + param[5] + "\r\n";

                            SQLiteParameter _code = new SQLiteParameter("code", SqlDbType.Int);
                            _code.Value = Convert.ToInt32(param[0]);
                            SQLiteParameter _name = new SQLiteParameter("name", param[1].Replace("'", ""));

                            SQLiteParameter _retail_price = new SQLiteParameter("retail_price", Convert.ToDecimal(param[2]));

                            SQLiteParameter _purchase_price = new SQLiteParameter("purchase_price", Convert.ToDecimal(param[3]));

                            SQLiteParameter _its_deleted = new SQLiteParameter("its_deleted", SqlDbType.SmallInt);
                            _its_deleted.Value = Convert.ToInt16(param[4]);
                            SQLiteParameter _nds = new SQLiteParameter("nds", Convert.ToInt32(param[5]));


                            command = new SQLiteCommand(query, conn);
                            command.Parameters.Add(_code);
                            command.Parameters.Add(_name);
                            command.Parameters.Add(_retail_price);
                            command.Parameters.Add(_purchase_price);
                            command.Parameters.Add(_its_deleted);
                            command.Parameters.Add(_nds);
                            command.Prepare();
                            //textBox1.Text = "успех";
                        }
                        else
                        {
                            //textBox1.Text += "i =  " + i.ToString() + "\r\n";
                            //textBox1.Text += "all " + t[i] + "\r\n";
                            //textBox1.Text += " code " + param[0] + "\r\n";
                            //textBox1.Text += "name " + param[1] + "\r\n";
                            //textBox1.Text += "retail_price " + param[2] + "\r\n";
                            //textBox1.Text += "purchase_price " + param[3] + "\r\n";
                            //textBox1.Text += "its_deleted " + param[4] + "\r\n";
                            //textBox1.Text += "nds " + param[5] + "\r\n";

                            command.Parameters[0].Value = Convert.ToInt32(param[0]);
                            command.Parameters[1].Value = param[1].Replace("'", "");
                            command.Parameters[2].Value = Convert.ToDecimal(param[2]);
                            command.Parameters[3].Value = Convert.ToDecimal(param[3]);
                            command.Parameters[4].Value = Convert.ToInt16(param[4]);
                            command.Parameters[5].Value = Convert.ToInt32(param[5]);
                            error_query = command.Parameters[0].Value.ToString() + " | " + command.Parameters[1].Value.ToString() + " | " +
                                command.Parameters[2].Value.ToString() + " | " + command.Parameters[3].Value.ToString() + " | " +
                                command.Parameters[4].Value.ToString() + " | " + command.Parameters[5].Value.ToString();
                        }
                        command.Transaction = trans;
                        command.ExecuteNonQuery();
                    }
                    //trans.Commit();
                    //command.Dispose();
                    //conn.Close();
                    //return true;
                    //conn = TSD.Program.ConnectForDataBase();
                    //conn.Open();
                    //trans = conn.BeginTransaction();
                    textBox1.Text += " \r\n Товары загрузились \r\n";
                    //Освобождаем память
                    t = null;
                    delimiters = new char[] { '|' };
                    string[] b = barcode.Split(delimiters);

                    textBox1.Text = " Удаляем штрихкоды \r\n";
                    query = "DELETE FROM barcodes";
                    command = new SQLiteCommand(query, conn);
                    command.Transaction = trans;
                    command.ExecuteNonQuery();
                    command.Dispose();

                    textBox1.Text = "Загружаются штрихкоды \r\n";

                    delimiters = new char[] { ',' };


                    for (int i = 0; i < b.Length - 1; i++)
                    {
                        if (i % 1000 == 0)
                        {
                            textBox1.Text = "Загружаются штрихкоды " + i.ToString() + " из " + b.Length.ToString();
                        }
                        string[] param = b[i].Split(delimiters);
                        //SQLiteParameter _tovar_code = new SQLiteParameter("tovar_code", SqlDbType.Int);
                        //_tovar_code.Value = Convert.ToInt32(param[0]);
                        //SQLiteParameter _barcode = new SQLiteParameter("barcode", SqlDbType.NVarChar);
                        //_barcode.Value = param[1].Replace("'", "");

                        if (i == 0)
                        {
                            query = "INSERT INTO barcodes(tovar_code,barcode_code) VALUES(@tovar_code,@barcode)";
                            SQLiteParameter _tovar_code = new SQLiteParameter("tovar_code", Convert.ToInt32(param[0]));
                            SQLiteParameter _barcode = new SQLiteParameter("barcode", param[1].Replace("'", ""));
                            command = new SQLiteCommand(query, conn);
                            command.Parameters.Add(_tovar_code);
                            command.Parameters.Add(_barcode);
                            command.Prepare();
                        }
                        else
                        {
                            command.Parameters[0].Value = Convert.ToInt32(param[0]);
                            command.Parameters[1].Value = param[1].Replace("'", "");
                        }
                        command.Transaction = trans;
                        error_query = command.Parameters[0].Value.ToString() + " | " + command.Parameters[1].Value.ToString();
                        command.ExecuteNonQuery();
                    }

                    textBox1.Text = "Штрихкод загрузился \r\n";
                    textBox1.Text = "Загружаются характеристики \r\n";
                    delimiters = new char[] { '|' };
                    if (characteristic != "")
                    {
                        string[] c = characteristic.Split(delimiters);
                        query = "DELETE FROM characteristic";
                        command = new SQLiteCommand(query, conn, trans);

                        command.ExecuteNonQuery();
                        command.Dispose();
                        for (int i = 0; i < c.Length - 1; i++)
                        {
                            if (i % 1000 == 0)
                            {
                                textBox1.Text = "Загружаются характеристики " + i.ToString() + " из " + c.Length.ToString();
                            }
                            query = "INSERT INTO characteristic(tovar_code, guid, name, retail_price_characteristic) VALUES(" + c[i] + ")";
                            command = new SQLiteCommand(query, conn, trans);
                            command.ExecuteNonQuery();
                            command.Dispose();
                        }
                    }
                    textBox1.Text += "Характеристики обработались \r\n";
                    //MessageBox.Show("Данные спешно загружены !!!");
                    trans.Commit();
                    //conn.Close();
                    command.Dispose();
                    trans.Dispose();
                }
                catch (SQLiteException ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    MessageBox.Show(error_query);
                    MessageBox.Show(ex.Message);
                    result = false;
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    MessageBox.Show(error_query);
                    MessageBox.Show(ex.Message);
                    result = false;
                }
            }
            //finally
            //{
            //    if (conn != null)
            //    {
            //        if (conn.State == ConnectionState.Open)
            //        {
            //            conn.Close();
            //        }
            //    }
            //}

            return result;
        }

        private int check_doc_1_status()
        {
            int result = 1;//есть такие документы

            SQLiteConnection conn = Program.ConnectForDataBase();
            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM dh WHERE status = 1";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                int result_query = Convert.ToInt32(command.ExecuteScalar());
                if (result_query == 0)
                {
                    result = 0;
                }
                command.Dispose();
                conn.Close();
            }
            catch (SQLiteException ex)
            {
                result = -1;
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                result = -1;
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


        //guid_1c

        /// <summary>
        /// получить гуиды всех не новых документов
        /// чтобы их не загружать по новой
        /// </summary>
        /// <returns></returns>
        private List<string> GetGuidStatus()
        {
            //string result = string.Empty; // есть такие документы
            List<string> list_guid = new List<string>();
            using (SQLiteConnection conn = Program.ConnectForDataBase())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT guid FROM dh WHERE status > 0";
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //result += "'" + reader["guid"].ToString() + "',";
                            list_guid.Add("'" + reader["guid"].ToString() + "'");
                        }
                    }
                }
                catch (SQLiteException ex)
                {
                    //result = "-1";
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    //result = "-1";
                }
            }

            return list_guid;
        }


        /// <summary>
        /// получить гуиды всех не новых документов
        /// чтобы их не загружать по новой
        /// </summary>
        /// <returns></returns>
        private string get_guid_1_status()
        {
            string result = "";//есть такие документы


            SQLiteConnection conn = Program.ConnectForDataBase();
            try
            {
                conn.Open();
                string query = "SELECT guid FROM dh WHERE status > 0";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result += "'" + reader["guid"].ToString() + "',";
                }
                reader.Close();
                conn.Close();
                if (result.Length > 0)
                {
                    result = result.Substring(0, result.Length - 1);
                }
            }
            catch (SQLiteException ex)
            {
                result = "-1";
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                result = "-1";
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

        public class DH : IDisposable
        {
            public string type { get; set; }
            public string date_1s { get; set; }
            //public string datetime_unloading { get; set; }
            public string guid_1s { get; set; }
            public string info_1s { get; set; }
            public string display_quantity { get; set; }
            public string allow_surplus { get; set; }

            void IDisposable.Dispose()
            {
            }
        }

        public class DT : IDisposable
        {
            public string guid_1s { get; set; }
            public string tovar_code { get; set; }
            public string quantity_1s { get; set; }
            public string price_buy { get; set; }
            public string price { get; set; }
            public string line_number { get; set; }
            public string characteristic { get; set; }
            public string box { get; set; }

            void IDisposable.Dispose()
            {
            }
        }

        public class Documents : IDisposable
        {
            public string NickShop { get; set; }
            public List<DH> ListDH { get; set; }
            public List<DT> ListDT { get; set; }

            void IDisposable.Dispose()
            {
            }
        }


        private const string ERROR_NOT_REGISTERED = "1000";
        private const string ERROR_FETCH_DOCUMENTS = "-1";

        private bool download_documents_json()
        {
            try
            {
                if (!upload_documents_json())
                {
                    AppendToTextBox("Синхронизация прервана");
                    return false;
                }

                string deviceId = Program.get_device_id();
                string key = deviceId + CryptorEngine.get_count_day_tsd();
                int numBase = Program.GetDbId();

                if (numBase == -1)
                {
                    AppendToTextBox("Не удалось получить номер базы\nПолучение документов прервано");
                    return false;
                }

                int existsDoc1Status = check_doc_1_status();
                if (existsDoc1Status == -1)
                {
                    AppendToTextBox("Произошли ошибки при получении статусов документов\nПолучение документов прервано");
                    return false;
                }

                string shopLocale = Program.get_code_shop();
                string shopRemote = GetShopOnGuid(deviceId, key, numBase);

                if (!ValidateShop(shopRemote, deviceId, shopLocale))
                {
                    return false;
                }

                string guidString = existsDoc1Status == 1 ? get_guid_1_status() : deviceId;
                string encryptGuidString = CryptorEngine.Encrypt(deviceId + guidString + deviceId, true, key);
                string decryptData = GetDocument1cBoxJson(deviceId, encryptGuidString, numBase);

                if (!ValidateDecryptData(decryptData, deviceId))
                {
                    return false;
                }

                decryptData = CryptorEngine.Decrypt(decryptData, true, key);
                Documents documents = JsonConvert.DeserializeObject<Documents>(decryptData);

                if (!ValidateAndInsertShop(documents.NickShop))
                {
                    return false;
                }

                if (documents.ListDH.Count == 0)
                {
                    AppendToTextBox("Нет документов для загрузки");
                    return true;
                }

                InsertDocumentsIntoDatabase(documents, shopLocale != shopRemote, numBase);

                return true;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
                return false;
            }
        }

        private bool ValidateShop(string shopRemote, string deviceId, string shopLocale)
        {
            if (shopRemote == ERROR_NOT_REGISTERED)
            {
                AppendToTextBox("Этот ТСД не зарегистрирован");
                return false;
            }

            if (string.IsNullOrEmpty(shopRemote))
            {
                AppendToTextBox("У ТСД нет привязки к магазину");
                return false;
            }

            if (shopLocale != shopRemote)
            {
                AppendToTextBox("В программе есть не завершенные документы, а у ТСД изменилась принадлежность к магазину, необходимо завершить все незавершенные документы");
                return false;
            }

            return true;
        }

        private bool ValidateDecryptData(string decryptData, string deviceId)
        {
            if (decryptData.Trim() == ERROR_FETCH_DOCUMENTS)
            {
                ShowErrorMessage("Ошибка при попытке получения документов");
                return false;
            }

            if (decryptData.Trim() == ERROR_NOT_REGISTERED)
            {
                ShowErrorMessage("Этот ТСД еще не зарегистрирован " + deviceId);
                return false;
            }

            return true;
        }

        private bool ValidateAndInsertShop(string nickShop)
        {
            if (Program.get_code_shop() != nickShop)
            {
                ShowErrorMessage("Попытка получения документов для другого магазина, загрузка документов отклонена");
                return false;
            }

            if (!insert_value_shop_in_databse(nickShop))
            {
                ShowErrorMessage("Произошли ошибки при загрузке данных, загрузка данных прервана");
                return false;
            }

            return true;
        }

        private void InsertDocumentsIntoDatabase(Documents documents, bool shopIsChanged, int numBase)
        {
            using (SQLiteConnection conn = Program.ConnectForDataBase())
            {
                conn.Open();
                using (SQLiteTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DeleteOldDocuments(conn, trans, shopIsChanged);
                        InsertDocuments(conn, trans, documents, numBase);
                        trans.Commit();
                        AppendToTextBox("Документы загружены");
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        ShowErrorMessage(ex.Message);
                        throw; // Проброс исключения для обработки на верхнем уровне
                    }
                }
            }
        }

        private void AppendToTextBox(string message)
        {
            textBox1.Text += message + "\r\n";
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Ошибка");
        }


        //Мой метод
        //private bool download_documents_json()
        //{
        //    try
        //    {
        //        if (!upload_documents_json())
        //        {
        //            AppendToTextBox("Синхронизация прервана");
        //            return false;
        //        }

        //        string deviceId = Program.get_device_id();
        //        string key = deviceId + CryptorEngine.get_count_day_tsd();
        //        int numBase = Program.GetDbId();

        //        if (numBase == -1)
        //        {
        //            AppendToTextBox("Не удалось получить номер базы\nПолучение документов прервано");
        //            return false;
        //        }

        //        int existsDoc1Status = check_doc_1_status();
        //        if (existsDoc1Status == -1)
        //        {
        //            AppendToTextBox("Произошли ошибки при получении статусов документов\nПолучение документов прервано");
        //            return false;
        //        }

        //        string shopLocale = Program.get_code_shop();
        //        string shopRemote = GetShopOnGuid(deviceId, key, numBase);

        //        if (shopRemote == "1000")
        //        {
        //            AppendToTextBox("Этот ТСД не зарегистрирован");
        //            return false;
        //        }

        //        if (string.IsNullOrEmpty(shopRemote))
        //        {
        //            AppendToTextBox("У ТСД нет привязки к магазину");
        //            return false;
        //        }

        //        if (shopLocale != shopRemote)
        //        {
        //            AppendToTextBox("В программе есть не завершенные документы, а у ТСД изменилась принадлежность к магазину, необходимо завершить все незавершенные документы");
        //            return false;
        //        }

        //        string guidString = existsDoc1Status == 1 ? get_guid_1_status() : deviceId;
        //        string encryptGuidString = CryptorEngine.Encrypt(deviceId + guidString + deviceId, true, key);
        //        string decryptData = GetDocument1cBoxJson(deviceId, encryptGuidString, numBase);

        //        if (decryptData.Trim() == "-1")
        //        {
        //            MessageBox.Show(" Ошибка при поптыке получения документов ", "Результат запроса");
        //            return false;
        //        }

        //        if (decryptData.Trim() == "1000")
        //        {
        //            MessageBox.Show("Этот ТСД еще не зарегистрирован " + deviceId, "Результат запроса");
        //            return false;
        //        }

        //        decryptData = CryptorEngine.Decrypt(decryptData, true, key);
        //        Documents documents = JsonConvert.DeserializeObject<Documents>(decryptData);

        //        if (Program.get_code_shop() != documents.NickShop)
        //        {
        //            MessageBox.Show("Попытка получения документов для другого магазина, загрузка документов отклонена");
        //            return false;
        //        }

        //        if (!insert_value_shop_in_databse(documents.NickShop))
        //        {
        //            MessageBox.Show("Произошли ошибки при загрузке данных, загрузка данных прервана");
        //            return false;
        //        }

        //        if (documents.ListDH.Count == 0)
        //        {
        //            AppendToTextBox("Нет документов для загрузки");
        //            return true;
        //        }

        //        using (SQLiteConnection conn = Program.ConnectForDataBase())
        //        {
        //            conn.Open();
        //            using (SQLiteTransaction trans = conn.BeginTransaction())
        //            {
        //                try
        //                {
        //                    DeleteOldDocuments(conn, trans, shopLocale != shopRemote);
        //                    InsertDocuments(conn, trans, documents, numBase);
        //                    trans.Commit();
        //                    AppendToTextBox("Документы загружены");
        //                }
        //                catch (Exception ex)
        //                {
        //                    trans.Rollback();
        //                    MessageBox.Show(ex.Message);
        //                    return false;
        //                }
        //            }
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        return false;
        //    }
        //}

        //private void AppendToTextBox(string message)
        //{
        //    textBox1.Text += message + "\r\n";
        //}

        private string GetShopOnGuid(string deviceId, string key, int numBase)
        {
            using (WS.WS ds = new TSD.WS.WS())
            {
                ds.Timeout = 200 * 1000;
                return ds.Get_Shop_On_Guid(deviceId, CryptorEngine.Encrypt(deviceId + deviceId, true, key), numBase);
            }
        }

        private string GetDocument1cBoxJson(string deviceId, string encryptGuidString, int numBase)
        {
            using (WS.WS ds = new TSD.WS.WS())
            {
                ds.Timeout = 200 * 1000;
                return ds.GetDocument1cBoxJson(deviceId, encryptGuidString, numBase);
            }
        }

        private void DeleteOldDocuments(SQLiteConnection conn, SQLiteTransaction trans, bool shopIsChanged)
        {
            string query = shopIsChanged
                ? "DELETE FROM dt WHERE guid NOT IN (SELECT guid FROM dh where status=3)"
                : "DELETE FROM dt WHERE guid IN (SELECT guid FROM dh where status=0) AND guid NOT IN (SELECT guid FROM dh where status=3)";

            ExecuteNonQuery(conn, trans, query);

            query = shopIsChanged
                ? "DELETE FROM dh WHERE guid NOT IN (SELECT guid FROM dh where status=3)"
                : "DELETE FROM dh where status=0";

            ExecuteNonQuery(conn, trans, query);
        }

        private void InsertDocuments(SQLiteConnection conn, SQLiteTransaction trans, Documents documents, int numBase)
        {
            foreach (DH dh in documents.ListDH)
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("INSERT INTO dh(type, date, guid, info_1s, display_quantity, status, its_new, db_id, allow_surplus) ");
                queryBuilder.Append("VALUES(");
                queryBuilder.Append(dh.type);
                queryBuilder.Append(", '");
                queryBuilder.Append(dh.date_1s);
                queryBuilder.Append("', '");
                queryBuilder.Append(dh.guid_1s);
                queryBuilder.Append("', '");
                queryBuilder.Append(dh.info_1s);
                queryBuilder.Append("', ");
                queryBuilder.Append(Convert.ToInt16(Convert.ToBoolean(dh.display_quantity)));
                queryBuilder.Append(", 0, 0, ");
                queryBuilder.Append(numBase);
                queryBuilder.Append(", ");
                queryBuilder.Append(Convert.ToInt16(Convert.ToBoolean(dh.allow_surplus)));
                queryBuilder.Append(")");

                ExecuteNonQuery(conn, trans, queryBuilder.ToString());
            }

            int i = 0;
            foreach (DT dt in documents.ListDT)
            {
                if (i % 1000 == 0)
                {
                    AppendToTextBox(string.Format("Загружаются строки документов {0} из {1}", i, documents.ListDT.Count));
                }

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("INSERT INTO dt(guid, tovar_code, quantity, price_buy, price, line_number, characteristic, box, quantity_shop) ");
                queryBuilder.Append("VALUES('");
                queryBuilder.Append(dt.guid_1s);
                queryBuilder.Append("', ");
                queryBuilder.Append(dt.tovar_code);
                queryBuilder.Append(", ");
                queryBuilder.Append(dt.quantity_1s);
                queryBuilder.Append(", ");
                queryBuilder.Append(dt.price_buy);
                queryBuilder.Append(", ");
                queryBuilder.Append(dt.price);
                queryBuilder.Append(", ");
                queryBuilder.Append(dt.line_number);
                queryBuilder.Append(", '");
                queryBuilder.Append(dt.characteristic);
                queryBuilder.Append("', '");
                queryBuilder.Append(dt.box);
                queryBuilder.Append("', 0)");

                ExecuteNonQuery(conn, trans, queryBuilder.ToString());
                i++;
            }
        }

        private void ExecuteNonQuery(SQLiteConnection conn, SQLiteTransaction trans, string query)
        {
            using (SQLiteCommand command = new SQLiteCommand(query, conn, trans))
            {
                command.ExecuteNonQuery();
            }
        }


        ///// <summary>
        ///// Загрузка документов
        ///// из промежуточного сервера
        ///// </summary>
        ///// <returns></returns>
        //private bool download_documents_json()
        //{
        //    bool result = true;

        //    if (!upload_documents_json())// Неудачная отправка документов произошли какие то ошибки дальнейшая синхронизация невозможна
        //    {
        //        textBox1.Text += " Синхронизация прервана \r\n ";
        //        result = false;
        //        return result;
        //    }

        //    string guid_string = string.Empty;
        //    List<string> lisg_guid = new List<string>();
        //    string decrypt_data = "";
        //    string device_id = Program.get_device_id();
        //    string key = device_id + CryptorEngine.get_count_day_tsd();
        //    int num_base = Program.GetDbId();
        //    if (num_base == -1)
        //    {
        //        return false;
        //    }
        //    try
        //    {
        //        WS.WS ds = new TSD.WS.WS();
        //        ds.Timeout = 200 * 1000;
        //        //Передается ид магазина и guid(ы) документов с 1 статусом которые уже есть на тсд, чтобы их не гонять повторно на тсд
        //        //если код магазина остается прежний

        //        int exists_doc_1_status = check_doc_1_status();
        //        if (exists_doc_1_status == -1)//произошли ошибки при получении статусов документов
        //        {
        //            textBox1.Text += "\r\n Произошли ошибки при получении статусов документов\r\nПолучение документов прервано ";
        //            result = false;
        //            return result;
        //        }
        //        else if (exists_doc_1_status == 1)
        //        {
        //            string shop_locale = Program.get_code_shop();


        //            //System.IO.StreamWriter sw = new System.IO.StreamWriter("\\query_doc.txt");
        //            //sw.WriteLine(CryptorEngine.Encrypt(CryptorEngine.Encrypt(device_id + device_id, true, key), true, key));
        //            //sw.Close();
        //            //MessageBox.Show("1");                    
        //            string shop_remote = ds.Get_Shop_On_Guid(device_id, CryptorEngine.Encrypt(device_id + device_id, true, key), num_base);
        //            // MessageBox.Show(shop_remote);

        //            if (shop_remote == "1000")
        //            {
        //                textBox1.Text += "\r\n Этот тсд не зарегистрирован ";
        //                result = false;
        //                return result;
        //            }
        //            if (shop_remote == "")
        //            {
        //                textBox1.Text += "\r\n У ТСД нет привязки к магазину ";
        //                result = false;
        //                return result;
        //            }
        //            if (shop_locale != shop_remote)
        //            {
        //                textBox1.Text += "\r\n В программе есть не завершенные документы, а у ТСД изменилась принадлежность к магазину, необходимо завершить все незавершенные документы ";
        //                result = false;
        //                return result;
        //            }
        //            else
        //            {
        //                guid_string = get_guis_1_status();//получить строку гуидов 
        //                //lisg_guid = GetGuidStatus();                        
        //            }

        //            //string encrypt_guid_string = CryptorEngine.Encrypt(device_id + guid_string + device_id, true, key);
        //            string encrypt_guid_string = CryptorEngine.Encrypt(device_id + guid_string + device_id, true, key);

        //            //System.IO.StreamWriter sw = new System.IO.StreamWriter("\\query.txt");
        //            //sw.WriteLine(CryptorEngine.Encrypt(encrypt_guid_string, true, key));
        //            //sw.Close();                    
        //            decrypt_data = ds.GetDocument1cBoxJson(device_id, encrypt_guid_string, num_base);
        //        }
        //        else if (exists_doc_1_status == 0)
        //        {
        //            string encrypt_guid_string = CryptorEngine.Encrypt(device_id + device_id, true, key);

        //            decrypt_data = ds.GetDocument1cBoxJson(device_id, encrypt_guid_string, num_base);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        result = false;
        //        return result;
        //    }


        //    if (decrypt_data.Trim() == "1000")
        //    {
        //        MessageBox.Show(" Этот ТСД еще не зарегистрирован " + device_id, "Результат запроса");
        //        result = false;
        //        return result;
        //    }

        //    decrypt_data = CryptorEngine.Decrypt(decrypt_data, true, key);
        //    Documents dokuments = JsonConvert.DeserializeObject<Documents>(decrypt_data);
        //    decrypt_data = "";
        //    string shop = dokuments.NickShop;//decrypt_data.Substring(device_id.Length, 3);

        //    bool shop_is_changer = false;
        //    //Проверить магазин не изменился ли он
        //    if (Program.get_code_shop() != shop)//магазин меняться при загрузке документов не должен
        //    {
        //        MessageBox.Show(" Попытка получения документов для другого магазина, загрузка документов отклонена ");
        //        result = false;
        //        return result;
        //    }

        //    if (!insert_value_shop_in_databse(shop))
        //    {
        //        MessageBox.Show("Произошли ошибки при загрузке данных, загрузка данных прервана");
        //        result = false;
        //        return result;
        //    }

        //    if (dokuments.ListDH.Count == 0)
        //    {
        //        textBox1.Text += " Нет документов для загрузки \r\n";
        //        return true;
        //    }

        //    SQLiteConnection conn = Program.ConnectForDataBase();
        //    SQLiteCommand command = new SQLiteCommand();
        //    SQLiteTransaction trans = null;

        //    try
        //    {
        //        conn.Open();
        //        trans = conn.BeginTransaction();

        //        textBox1.Text = "Удаление документов";
        //        string query = "";

        //        if (shop_is_changer)
        //        {
        //            query = " DELETE FROM dt WHERE guid NOT IN (SELECT guid FROM dh where status=3) ";
        //        }
        //        else
        //        {
        //            query = " DELETE FROM dt WHERE guid IN (SELECT guid FROM dh where status=0) AND guid NOT IN (SELECT guid FROM dh where status=3) ";
        //        }

        //        command = new SQLiteCommand(query, conn);
        //        command.Transaction = trans;
        //        command.ExecuteNonQuery();

        //        if (shop_is_changer)
        //        {
        //            query = " DELETE FROM dh WHERE guid NOT IN (SELECT guid FROM dh where status=3) ";
        //        }
        //        else
        //        {
        //            query = "DELETE FROM dh where status=0";
        //        }
        //        command = new SQLiteCommand(query, conn);
        //        command.Transaction = trans;
        //        command.ExecuteNonQuery();
        //        textBox1.Text = "Удаление табличных частей документов";

        //        foreach (DH dh in dokuments.ListDH)
        //        {
        //            query = "INSERT INTO dh(type," +
        //                "date," +
        //                "guid," +
        //                "info_1s," +
        //                "display_quantity," +
        //                "status," +
        //                "its_new," +
        //                "db_id,"+
        //                "allow_surplus) VALUES(" +
        //                dh.type+",'" +
        //                dh.date_1s + "','" +
        //                dh.guid_1s + "','" +
        //                dh.info_1s + "'," +
        //                Convert.ToInt16(Convert.ToBoolean(dh.display_quantity)) + "," +
        //                "0," +
        //                "0," +
        //                num_base + ","+
        //            Convert.ToInt16(Convert.ToBoolean(dh.allow_surplus)) + ")";
        //            command = new SQLiteCommand(query, conn);
        //            command.Transaction = trans;
        //            command.ExecuteNonQuery();
        //            command.Dispose();
        //        }
        //        dokuments.ListDH.Clear();
        //        dokuments.ListDH = null;

        //        int i = 0;
        //        foreach (DT dt in dokuments.ListDT)
        //        {
        //            {
        //                if (i % 1000 == 0)
        //                {
        //                    textBox1.Text = "Загружаются строки документов " + i.ToString() + " из " + dokuments.ListDT.Count.ToString() + " \r\n ";
        //                }

        //                query = "INSERT INTO dt(guid," +
        //                    "tovar_code," +
        //                    "quantity," +
        //                    "price_buy," +
        //                    "price," +
        //                    "line_number," +
        //                    "characteristic," +
        //                    "box," +
        //                    "quantity_shop) VALUES('" +
        //                    dt.guid_1s+"',"+
        //                    dt.tovar_code + "," +
        //                    dt.quantity_1s + "," +
        //                    dt.price_buy + "," +
        //                    dt.price + "," +
        //                    dt.line_number + ",'" +
        //                    dt.characteristic + "','" +
        //                    dt.box + "'," +
        //                    "0)";
        //                command = new SQLiteCommand(query, conn);
        //                command.Transaction = trans;
        //                command.ExecuteNonQuery();
        //                command.Dispose();
        //            }

        //        }
        //        textBox1.Text += " Документы загружены \r\n ";
        //        trans.Commit();
        //        conn.Close();

        //        dokuments.ListDT.Clear();
        //        dokuments.ListDT = null;
        //        dokuments = null;
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        result = false;
        //        if (trans == null)
        //        {
        //            trans.Rollback();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        result = false;
        //        if (trans == null)
        //        {
        //            trans.Rollback();
        //        }
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //    }

        //    GC.Collect();
        //    GC.WaitForPendingFinalizers();

        //    return result;
        //}


        private bool download_documents()
        {
            bool result = true;

            if (!upload_documents_json())// Неудачная отправка документов произошли какие то ошибки дальнейшая синхронизация невозможна
            {
                textBox1.Text += " Синхронизация прервана \r\n ";
                result = false;
                return result;
            }


            string guid_string = string.Empty;
            string decrypt_data = "";
            string device_id = Program.get_device_id();
            string key = device_id + CryptorEngine.get_count_day_tsd();
            int num_base = Program.GetDbId();
            if (num_base == -1)
            {
                return false;
            }
            try
            {
                WS.WS ds = new TSD.WS.WS();
                ds.Timeout = 200 * 1000;
                //Передается ид магазина и guid(ы) документов с 1 статусом которые уже есть на тсд, чтобы их не гонять повторно на тсд
                //если код магазина остается прежний

                int exists_doc_1_status = check_doc_1_status();
                if (exists_doc_1_status == -1)//произошли ошибки при получении статусов документов
                {
                    textBox1.Text += "\r\n Получение документов прервано ";
                    result = false;
                    return result;
                }
                else if (exists_doc_1_status == 1)
                {
                    string shop_locale = Program.get_code_shop();


                    //System.IO.StreamWriter sw = new System.IO.StreamWriter("\\query_doc.txt");
                    //sw.WriteLine(CryptorEngine.Encrypt(CryptorEngine.Encrypt(device_id + device_id, true, key), true, key));
                    //sw.Close();
                    //MessageBox.Show("1");                    
                    string shop_remote = ds.Get_Shop_On_Guid(device_id, CryptorEngine.Encrypt(device_id + device_id, true, key), num_base);
                    // MessageBox.Show(shop_remote);

                    if (shop_remote == "1000")
                    {
                        textBox1.Text += "\r\n Этот тсд не зарегистрирован ";
                        result = false;
                        return result;
                    }
                    if (shop_remote == "")
                    {
                        textBox1.Text += "\r\n У ТСД нет привязки к магазину ";
                        result = false;
                        return result;
                    }
                    if (shop_locale != shop_remote)
                    {
                        textBox1.Text += "\r\n В программе есть не завершенные документы, а у ТСД изменилась принадлежность к магазину, необходимо завершить все незавершенные документы ";
                        result = false;
                        return result;
                    }
                    else
                    {
                        guid_string = get_guid_1_status();//получить строку гуидов 
                    }

                    string encrypt_guid_string = CryptorEngine.Encrypt(device_id + guid_string + device_id, true, key);

                    //System.IO.StreamWriter sw = new System.IO.StreamWriter("\\query.txt");
                    //sw.WriteLine(CryptorEngine.Encrypt(encrypt_guid_string, true, key));
                    //sw.Close();                    
                    decrypt_data = ds.Get_Document_1c_Box(device_id, encrypt_guid_string, num_base);
                }
                else if (exists_doc_1_status == 0)
                {
                    string encrypt_guid_string = CryptorEngine.Encrypt(device_id + device_id, true, key);
                    //if (num_base == 0)
                    //{
                    //    decrypt_data = ds.Get_Document_1c(device_id, encrypt_guid_string, num_base);
                    //}
                    //else
                    //{
                    decrypt_data = ds.Get_Document_1c_Box(device_id, encrypt_guid_string, num_base);
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("download_documents " + ex.Message);
                result = false;
                return result;
            }


            if (decrypt_data.Trim() == "1000")
            {
                MessageBox.Show(" Этот ТСД еще не зарегистрирован " + device_id, "Результат запроса");
                result = false;
                return result;
            }

            if (decrypt_data == "-1")
            {
                MessageBox.Show(" Произошла ошибка при получении документов ");
                result = false;
                return result;
            }
            decrypt_data = CryptorEngine.Decrypt(decrypt_data, true, key);

            string shop = decrypt_data.Substring(device_id.Length, 3);

            bool shop_is_changer = false;
            //Проверить магазин не изменился ли он
            if (Program.get_code_shop() != shop)//магазин меняться при загрузке документов не должен
            {
                //shop_is_changer = true;
                MessageBox.Show(" Попытка получения документов для другого магазина, загрузка документов отклонена ");
                result = false;
                return result;
            }

            if (!insert_value_shop_in_databse(shop))
            {
                MessageBox.Show("Произошли ошибки при загрузке данных, загрузка данных прервана");
                result = false;
                return result;
            }

            if (decrypt_data.IndexOf("SHAPKASHAPKASTROKISTROKI") != -1)
            {
                textBox1.Text += " Нет документов для загрузки \r\n";
                return true;
            }

            int start_pos = decrypt_data.IndexOf("SHAPKA");
            int finish_pos = decrypt_data.Substring(start_pos + 6, decrypt_data.Length - start_pos - 6).IndexOf("SHAPKA");
            if (finish_pos == 0)
            {
                MessageBox.Show("Получены неполные данные или нет документов для этого ТСД, загрука невозможна");
                result = false;
                return result;
            }



            string shapka = decrypt_data.Substring(start_pos + 6, finish_pos);
            shapka = shapka.Substring(0, shapka.Length - 1);

            start_pos = decrypt_data.IndexOf("STROKI");
            finish_pos = decrypt_data.Substring(start_pos + 6, decrypt_data.Length - start_pos - 6).IndexOf("STROKI");
            if (finish_pos == 0)
            {
                MessageBox.Show("Получены неполные данные, нет строк для документов, загрука невозможна");
                result = false;
                return result;
            }

            string stroki = decrypt_data.Substring(start_pos + 6, finish_pos);
            stroki = stroki.Substring(0, stroki.Length - 1);
            StringBuilder sb = new StringBuilder();
            char[] delimiters = new char[] { '|' };
            string[] sh = shapka.Split(delimiters);
            string[] st = stroki.Split(delimiters);

            SQLiteConnection conn = Program.ConnectForDataBase();
            SQLiteCommand command = new SQLiteCommand();
            SQLiteTransaction trans = null;

            string query = "";

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                textBox1.Text = "Удаление документов";


                if (shop_is_changer)
                {
                    query = " DELETE FROM dt WHERE guid NOT IN (SELECT guid FROM dh where status=3) ";
                }
                else
                {
                    query = " DELETE FROM dt WHERE guid IN (SELECT guid FROM dh where status=0) AND guid NOT IN (SELECT guid FROM dh where status=3) ";
                }

                command = new SQLiteCommand(query, conn);
                command.Transaction = trans;
                command.ExecuteNonQuery();

                if (shop_is_changer)
                {
                    query = " DELETE FROM dh WHERE guid NOT IN (SELECT guid FROM dh where status=3) ";
                }
                else
                {
                    query = "DELETE FROM dh where status=0";
                }
                command = new SQLiteCommand(query, conn);
                command.Transaction = trans;
                command.ExecuteNonQuery();
                textBox1.Text = "Удаление табличных частей документов";


                for (int i = 0; i < sh.Length; i++)
                {
                    query = "INSERT INTO dh(type,date,guid,info_1s,display_quantity,status,its_new,allow_surplus,db_id) VALUES(" + sh[i] + ",0,0," + num_base + ");";
                    //query = "INSERT INTO dh(" + sh[i] + ",0" + ");";
                    command = new SQLiteCommand(query, conn);
                    command.Transaction = trans;
                    command.ExecuteNonQuery();
                    command.Dispose();
                }

                for (int i = 0; i < st.Length; i++)
                {
                    if (i % 1000 == 0)
                    {
                        textBox1.Text = "Загружаются строки документов " + i.ToString() + " из " + st.Length.ToString() + " \r\n ";
                    }

                    query = "INSERT INTO dt(guid,tovar_code,quantity,price_buy,price,line_number,characteristic,box,quantity_shop) VALUES(" + st[i] + ",0" + ")";
                    //query = "INSERT INTO dt(" + st[i] + ",0" + ")";
                    command = new SQLiteCommand(query, conn);
                    command.Transaction = trans;
                    command.ExecuteNonQuery();
                    command.Dispose();
                }

                textBox1.Text += " Документы загружены \r\n ";
                trans.Commit();
                conn.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(query);
                result = false;
                if (trans == null)
                {
                    trans.Rollback();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(query);
                result = false;
                if (trans == null)
                {
                    trans.Rollback();
                }
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

        private bool previous_check()
        {
            bool result = true;
            int num_base = Program.GetDbId();
            if (num_base == -1)
            {
                return false;
            }

            WS.WS ds = new TSD.WS.WS();
            ds.Timeout = 200 * 1000;
            string device_id = Program.get_device_id();
            int exists_doc_1_status = check_doc_1_status();
            if (exists_doc_1_status == -1)//произошли ошибки при получении статусов документов
            {
                result = false;
            }
            else if (exists_doc_1_status == 1)
            {
                string shop_locale = Program.get_code_shop();
                string shop_remote = ds.Get_Shop_On_Guid(device_id, CryptorEngine.Encrypt(device_id, true, CryptorEngine.get_count_day_tsd()), num_base);
                if (shop_remote == "1000")
                {
                    textBox1.Text = " \r\n Этот тсд не зарегистрирован ";
                    //MessageBox.Show(" Этот тсд не зарегистрирован ");
                }
                if (shop_locale != shop_remote)
                {
                    textBox1.Text = " \r\n В программе есть не завершенные документы, а у ТСД изменилась принадлежность к магазину, необходимо завершить все незавершенные документы ";
                    result = false;
                }
            }

            return result;
        }

        private void delete_document_on_status_3()
        {
            SQLiteConnection conn = Program.ConnectForDataBase();

            try
            {
                conn.Open();
                string query = "";
            }
            catch (SQLiteException ex)
            {

            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private bool check_for_loading_full_file()
        {
            bool result = true;
            textBox1.Text += "\r\n Проверка возможности быстрой загрузки полной базы данных ";

            SQLiteConnection conn = Program.ConnectForDataBase();
            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM dh WHERE status=1 OR status=2";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                object result_query = command.ExecuteScalar();
                if (result_query == null)
                {
                    result = false;
                }
                else
                {
                    if (Convert.ToInt32(result_query) > 0)
                    {
                        result = false;
                    }
                }
                conn.Close();
            }
            catch (SQLiteException ex)
            {
                result = false;
                textBox1.Text += "\r\n При проверке возможности быстрой загрузки ппроизошли ошибки " + ex.Message;
            }
            catch (Exception ex)
            {
                result = false;
                textBox1.Text += "\r\n При проверке возможности быстрой загрузки ппроизошли ошибки " + ex.Message;
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

        private void btn_execute_full_sinhronization_Click(object sender, EventArgs e)
        {

            if (DialogResult.Yes != MessageBox.Show(" Синхронизировать ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
            {
                return;
            }

            //if (check_for_loading_full_file())
            //{
            //    if (DialogResult.Yes == MessageBox.Show(" Есть возможность быстрой загрузки, использовать ее ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
            //    {
            //        btn_full_load_Click(null, null);
            //        return;
            //    }
            //    else
            //    {
            //        textBox1.Text += " Выбрана медленная загрузка \r\n ";
            //    } 
            //}

            //ПРЕДВАРИТЕЛЬНЫЕ ПРОВЕРКИ  
            //MessageBox.Show("Ghjdthbv cjtlbytybt");
            //if (!Program.ConnectionAvailable())
            //{
            //    textBox1.Text += " Синхронизация прервана \r\n ";
            //    return;
            //}

            textBox1.Text += " 1.Проверка наличия документов в центральной базе \r\n ";
            //сначала проверка все ли документы необходимо отправлять, возможно некоторые из ниху уже ранее были отправлены 
            //if (exists_document_in_central_base() == "-1") //произошли какие то ошибки дальнейшая синхронизация невозможна
            //{
            //    textBox1.Text += " Синхронизация прервана \r\n ";
            //    return;
            //}
            //textBox1.Text += " 1. Проверка наличия документов в центральной базе успешно \r\n";
            textBox1.Text += " 2. Попытка отправки документов в центральную базу \r\n ";
            //если мы здесь первый этап успешно выполнен

            //Отправляем документы со статусом 2 т.е. завершен в центральный офис
            //if (!upload_documents())// Неудачная отправка документов произошли какие то ошибки дальнейшая синхронизация невозможна
            //{
            //    textBox1.Text += " Синхронизация прервана \r\n ";
            //    return;
            //}

            if (!upload_documents_json())// Неудачная отправка документов произошли какие то ошибки дальнейшая синхронизация невозможна
            {
                textBox1.Text += " Синхронизация прервана \r\n ";
                return;
            }

            textBox1.Text += " 3. Попытка загрузки справочников \r\n ";
            if (!download_tmc()) // Неудачная загрузка справочников произошли какие то ошибки дальнейшая синхронизация невозможна
            {
                textBox1.Text += " Синхронизация прервана \r\n ";
                return;
            }

            //textBox1.Text += " 3. Попытка загрузки документов \r\n ";
            //if (!download_documents())
            //{
            //    textBox1.Text += " Синхронизация прервана \r\n ";
            //    return;
            //}

            textBox1.Text += " 3. Попытка загрузки документов \r\n ";
            if (!download_documents_json())
            {
                textBox1.Text += " Синхронизация прервана \r\n ";
                return;
            }


            //Program.shrink_database();

            textBox1.Text += "Синхронизация успешно завершена";

        }

        #region load_out_files

        //private void btn_load_out_files_Click(object sender, EventArgs e)
        //{

        //    if (File.Exists("\\Storage Card\\tovar.txt"))//Есть файл товаров, попробовать его загрузить
        //    {
        //        load_tovar_out_file();
        //    }

        //    if (File.Exists("\\Storage Card\\barcode.txt"))//Есть файл штрихкодов, попробовать его загрузить
        //    {
        //        load_barcodes_out_file();
        //    }

        //    if (File.Exists("\\Storage Card\\DH.txt") && File.Exists("\\Storage Card\\DT.txt"))//Есть файлы документов, попробовать их загрузить
        //    {
        //        load_documents_out_file();
        //    }

        //}

        private void load_tovar_out_file()
        {
            SQLiteConnection conn = Program.ConnectForDataBase();
            SQLiteCommand command = null;
            SQLiteTransaction trans = null;
            string query = "";
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                query = " DELETE FROM tovar ;";
                command = new SQLiteCommand(query, conn);
                command.Transaction = trans;
                command.ExecuteNonQuery();

                using (StreamReader sr = new StreamReader("\\Storage Card\\tovar.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        query = "INSERT INTO tovar(code,name,retail_price,purchase_price,its_deleted,nds) VALUES(" + line + ")";
                        command = new SQLiteCommand(query, conn);
                        command.Transaction = trans;
                        command.ExecuteNonQuery();
                    }
                }
                trans.Commit();
                conn.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
                if (trans != null)
                {
                    trans.Rollback();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (trans != null)
                {
                    trans.Rollback();
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            textBox1.Text = "Товары загрузились" + "\r\n";
        }

        private void load_barcodes_out_file()
        {
            SQLiteConnection conn = Program.ConnectForDataBase();
            SQLiteCommand command = null;
            SQLiteTransaction trans = null;
            string query = "";
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                query = " DELETE FROM barcodes ;";
                command = new SQLiteCommand(query, conn);
                command.Transaction = trans;
                command.ExecuteNonQuery();

                using (StreamReader sr = new StreamReader("\\Storage Card\\barcode.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        query = "INSERT INTO barcodes(tovar_code,barcode_code)VALUES(" + line + ")";
                        command = new SQLiteCommand(query, conn);
                        command.Transaction = trans;
                        command.ExecuteNonQuery();
                    }
                }
                trans.Commit();
                conn.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
                if (trans != null)
                {
                    trans.Rollback();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (trans != null)
                {
                    trans.Rollback();
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            textBox1.Text += "Штрихкоды загрузились" + "\r\n";
        }

        private void load_documents_out_file()
        {
            SQLiteConnection conn = Program.ConnectForDataBase();
            SQLiteCommand command = null;
            SQLiteTransaction trans = null;
            string query = "";
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                query = " DELETE FROM dh ;";
                command = new SQLiteCommand(query, conn);
                command.Transaction = trans;
                command.ExecuteNonQuery();

                using (StreamReader sr = new StreamReader("\\Storage Card\\DH.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        query = "INSERT INTO dh(type,date,guid,info_1s,status,display_quantity) VALUES(" + line + ")";
                        command = new SQLiteCommand(query, conn);
                        command.Transaction = trans;
                        command.ExecuteNonQuery();
                    }
                }

                query = " DELETE FROM dt ;";
                command = new SQLiteCommand(query, conn);
                command.Transaction = trans;
                command.ExecuteNonQuery();

                using (StreamReader sr = new StreamReader("\\Storage Card\\DT.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        query = "INSERT INTO dt(guid,tovar_code,characteristic,quantity,quantity_shop,price_buy,price,line_number,its_sent) VALUES(" + line + ")";
                        command = new SQLiteCommand(query, conn);
                        command.Transaction = trans;
                        command.ExecuteNonQuery();
                    }
                }
                trans.Commit();
                conn.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
                if (trans != null)
                {
                    trans.Rollback();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (trans != null)
                {
                    trans.Rollback();
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            textBox1.Text += "Документы загрузились" + "\r\n";
        }

        #endregion

       

       

        //private void btn_upload_logs_Click(object sender, EventArgs e)
        //{

        //    textBox1.Text = "Попытка передачи логов";
        //    WS.WS ds = new TSD.WS.WS();
        //    ds.Timeout = 200 * 1000;            
        //    string device_id = Program.get_device_id();

        //    int num_base = Program.GetDbId();                            

        //    SQLiteConnection conn = Program.ConnectForDataBase();
        //    SQLiteCommand command = null;
        //    //SQLiteTransaction trans = null;
        //    StringBuilder sb = new StringBuilder();
        //    string query = "";
        //    string nick_shop=Program.get_code_shop();


        //    //query = "DELETE FROM logs ;";
        //    //conn.Open();
        //    //command = new SQLiteCommand(query, conn);
        //    //command.ExecuteNonQuery();
        //    //return;

        //    try
        //    {
        //        conn.Open();
        //        //trans = conn.BeginTransaction();

        //        //query = "SELECT date,description FROM logs order by date;";
        //        //command = new SQLiteCommand(query, conn);
        //        //SQLiteDataReader reader = command.ExecuteReader();
        //        //while (reader.Read())
        //        //{
        //        //    string s = "";
        //        //    s = "'" + nick_shop + "','" + device_id + "','" + reader.GetDateTime(0).ToString("dd-MM-yyyy HH:mm:ss") + "','" + reader["description"].ToString().Trim() + "'|";
        //        //    sb.Append(s); 
        //        //}

        //        using (StreamReader sr = new StreamReader("\\Application\\Logs.txt"))
        //        {
        //            string line;
        //            while ((line = sr.ReadLine()) != null)
        //            {

        //                char[] delimiters = new char[] { ',' };
        //                string[] t = line.Split(delimiters);
        //                string s = "";
        //                s = "'" + nick_shop + "','" + device_id + "','" + t[0] + "','" + t[1] + "'|";
        //                sb.Append(s);
        //            }
        //        }


        //        string key = device_id + CryptorEngine.get_count_day_tsd();
        //        string encrypt_data = CryptorEngine.Encrypt(device_id + sb.ToString() + device_id, true, key);
        //        //System.IO.StreamWriter sw = new StreamWriter("\\Application\\query.txt",true);
        //        //sw.WriteLine(encrypt_data);
        //        //sw.Close();

        //        string result_upload = ds.Upload_logs(device_id, encrypt_data, num_base);
        //        if (result_upload == "1")
        //        {
        //            File.Delete("\\Application\\Logs.txt");
        //            //query = "DELETE FROM logs ;";
        //            //command = new SQLiteCommand(query, conn);
        //            //command.ExecuteNonQuery();
        //            textBox1.Text += "Результаты логгирования успешно переданы";
        //        }
        //        else
        //        {
        //            textBox1.Text += "Неудачная попытка передачи данных";
        //        }
        //        //conn.Close();
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
        //        //if (conn.State == ConnectionState.Open)
        //        //{
        //        //    conn.Close();
        //        //}
        //    }
        //}             
    }
}