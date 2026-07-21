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
    public partial class DocumentInventory1 : Form
    {

        public string guid = "";
        public string typ_doc = "";
        public int    its_new = 0;
        public string status="";
        public string info1c = "";
        public string box = "0";


        public DocumentInventory1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.Load += new EventHandler(DocumentInventory1_Load);
           
        }

        public bool write_new_document(string info_1s)
        {
            bool result = true;


            using (SQLiteConnection conn = Program.ConnectForDataBase())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO dh(type,date,guid,info_1s,status,display_quantity,its_new,db_id,allow_surplus)" +
                        " VALUES(@type,@date,@guid,@info_1s,@status,@display_quantity,@its_new,@db_id,@allow_surplus)";
                    SQLiteParameter _type = new SQLiteParameter("type", typ_doc);
                    SQLiteParameter _date = new SQLiteParameter("date", DateTime.Now);
                    SQLiteParameter _guid = new SQLiteParameter("guid", guid);
                    SQLiteParameter _info_1s = new SQLiteParameter("info_1s", info_1s);
                    SQLiteParameter _status = new SQLiteParameter("status", SqlDbType.Int);
                    _status.Value = 0;
                    SQLiteParameter _allow_surplus = new SQLiteParameter("allow_surplus", SqlDbType.Int);
                    _allow_surplus.Value = 0;
                    SQLiteParameter _display_quantity = new SQLiteParameter("display_quantity", SqlDbType.Int);
                    _display_quantity.Value = 1;
                    SQLiteParameter _its_new = new SQLiteParameter("its_new", SqlDbType.Int);
                    _display_quantity.Value = this.its_new;
                    int db_id = Program.GetDbId();
                    if (db_id == -1)
                    {
                        return false;
                    }

                    SQLiteParameter _db_id = new SQLiteParameter("db_id", SqlDbType.Int);
                    _display_quantity.Value = db_id;
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.Add(_type);
                        command.Parameters.Add(_date);
                        command.Parameters.Add(_guid);
                        command.Parameters.Add(_info_1s);
                        command.Parameters.Add(_status);
                        command.Parameters.Add(_display_quantity);
                        command.Parameters.Add(_its_new);
                        command.Parameters.Add(_db_id);
                        command.Parameters.Add(_allow_surplus);
                        command.ExecuteNonQuery();
                    }

                    //Вставить строку с нулевым количеством 
                    //query = " INSERT INTO dt(guid,tovar_code,characteristic,quantity,quantity_shop,price_buy,price,line_number,its_sent,box,box_status)VALUES('" +
                    //       guid+"'," +
                    //       "12345,'" +
                    //       "'," +
                    //       "0" + "," +
                    //       "0," +
                    //       "0" + "," +
                    //       "0" + "," +
                    //       "1," +
                    //       "1" + ",'" +
                    //       "0','" +
                    //       "т');";

                    //command = new SQLiteCommand(query, conn);
                    //command.ExecuteNonQuery();
                    //command.Dispose();
                    //conn.Close();
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
        

        protected override void OnKeyDown(KeyEventArgs e)
        {
           // base.OnKeyDown(e);
            if (e.KeyCode == Keys.D1)
            {
                btn_start_continue_Click(null, null);               
            }
            else if (e.KeyCode == Keys.D2)
            {
                btn_show_divergence_Click(null, null);
            }
            //else if (e.KeyCode == Keys.D3)
            //{
            //    btn_box_Click(null,null);
            //}
            else if (e.KeyCode == Keys.D9)
            {
                btn_complete_Click(null,null);
            }             
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }            
        }


        private void DocumentInventory1_Load(object sender, EventArgs e)
        {
             SQLiteConnection conn = Program.ConnectForDataBase();
             try
             {
                 conn.Open();
                 string query = "SELECT type,date,info_1s,status,its_new FROM dh where guid=@guid";
                 SQLiteParameter _guid = new SQLiteParameter("guid", guid);
                 SQLiteCommand command = new SQLiteCommand(query, conn);
                 command.Parameters.Add(_guid);
                 
                 SQLiteDataReader reader = command.ExecuteReader();
                 while (reader.Read())
                 {                     
                     string type = reader["type"].ToString();
                     string date = reader.GetDateTime(1).ToString("dd-MM-yyyy");
                     string txt_info_1s = reader["info_1s"].ToString();
                     string txt_status = reader["status"].ToString();
                     its_new = Convert.ToInt16(reader["its_new"].ToString());
                     if(txt_status=="2")
                     {
                         btn_complete.Enabled = false;
                         btn_start_continue.Enabled = false;
                     }

                     string shapka = "";
                     if (type == "1")
                     {
                         shapka = "Инвентаризация";
                     }
                     if (type == "4")
                     {
                         shapka = "Сбор для печати ценников";
                     }
                     if (type == "5")
                     {
                         shapka = "Сбор сроков номенклатуры";
                     }
                     this.Text = shapka +" от " + date;
                     label_decription_document.Text = txt_info_1s;
                 }
                 reader.Close();
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
             //if (typ_doc == "4")
             //{
             //    btn_box.Enabled = true;
             //}
        }

        /// <summary>        
        /// Обновить статус документа на рабочий
        /// </summary>
        /// <returns></returns>
        private void UpdateStatus(string guid)
        {
            using (SQLiteConnection conn = Program.ConnectForDataBase())
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE dh SET status=1 WHERE guid=@guid";
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@guid", guid);
                        command.ExecuteNonQuery();
                        status = "Р";
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("UpdateStatus\r\n"+ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("UpdateStatus\r\n" + ex.Message);
                }
            }
        }


        private void btn_start_continue_Click(object sender, EventArgs e)
        {
            if ((typ_doc != "4")&&(typ_doc != "5"))
            {
                //WorkWithBarcode wb = new WorkWithBarcode();
                ////wb.label_decription_document.Text = this.label_decription_document.Text;
                ////wb.tovar_code
                //wb.guid = guid;
                //wb.typ_doc = this.typ_doc;
                //wb.its_new = this.its_new;
                //wb.ShowDialog();
                //wb.Dispose();
                //Boxes boxes = new Boxes();
                //boxes.guid = guid;
                //boxes.typ_doc = this.typ_doc;
                //boxes.its_new = this.its_new;
                //boxes.TopMost = true;
                //boxes.status = status;
                //boxes.info1c = info1c;
                //boxes.ShowDialog();
                //boxes.Dispose();
                //boxes = null;
                ////Program.write_log("Close boxes");
                //GC.Collect();
                //// Ожидание завершения всех финализаторов
                //GC.WaitForPendingFinalizers();
                try
                {
                    using (Boxes boxes = new Boxes())
                    {
                        boxes.guid = guid;
                        boxes.typ_doc = this.typ_doc;
                        boxes.its_new = this.its_new;
                        boxes.TopMost = true;
                        boxes.status = status;
                        boxes.info1c = info1c;
                        boxes.ShowDialog();
                        if (status == "Н")
                        {
                            UpdateStatus(guid);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.TopMost = true;
                this.Refresh();
            }
            else
            {
                //CheckAndPrintPrices chp = new CheckAndPrintPrices();
                //chp.guid = guid;
                //chp.typ_doc = this.typ_doc;
                //chp.its_new = this.its_new;
                //chp.ShowDialog();                
                //chp.Dispose();
                //chp = null;
                
                //GC.Collect();
                //// Ожидание завершения всех финализаторов
                //GC.WaitForPendingFinalizers();
                try
                {
                    using (CheckAndPrintPrices chp = new CheckAndPrintPrices())
                    {
                        chp.guid = guid;
                        chp.typ_doc = this.typ_doc;
                        chp.its_new = this.its_new;
                        chp.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.TopMost = true;
                this.Refresh();
            }
        }
            

        private void btn_show_divergence_Click(object sender, EventArgs e)
        {
            using (DocumentList doc = new DocumentList())
            {
                doc.guid = guid;
                doc.label_decription_document = this.label_decription_document.Text;
                doc.its_new = false;
                doc.typ_doc = this.typ_doc;
                doc.ShowDialog();
            }           
        }


        /// <summary>
        /// Проверка на наличие строк у документа, если строк нет
        /// то не завершаем документ и не отправляем его
        /// </summary>
        /// <returns></returns>
        private bool check_have_stroki()
        {            
            bool result = true;
            using (SQLiteConnection conn = Program.ConnectForDataBase())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM dt WHERE guid='" + guid + "'";
                    using (SQLiteCommand commаnd = new SQLiteCommand(query, conn))
                    {
                        object result_query = commаnd.ExecuteScalar();
                        if (Convert.ToInt32(result_query) == 0)
                        {
                            result = false;
                        }
                    }                    
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

        private bool check_have_not_zero_string()
        {
            bool result = true;

            using (SQLiteConnection conn = Program.ConnectForDataBase())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT SUM(quantity_shop) FROM dt WHERE guid='" + guid + "'";
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        object result_qery = command.ExecuteScalar();
                        if (Convert.ToInt32(result_qery) == 0)
                        {
                            result = false;
                        }
                    }                    
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
            }              
    
            return result;
        }
        


        private void btn_complete_Click(object sender, EventArgs e)
        {

            bool error = false;
            //if (MessageBox.Show("Вы хотите завершить документ ?", "Завершение документа", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            if(DialogResult.Yes != MessageBox.Show("Вы хотите завершить документ ?","", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
            {
                return;
            }
            

            //Проверить на наличие строк и на заполненость хотя бы одной строки документа с заполненным количеством.
            if (!check_have_stroki())
            {
                MessageBox.Show(" В документе нет строк, он не может быть завершен ");
                return;
            }

            if (!check_have_not_zero_string())
            {
                MessageBox.Show(" В документе нет обработанных строк, он не может быть завершен ");
                return;
            }

            Program.BackupDatabases();

            using (SQLiteConnection conn = Program.ConnectForDataBase())
            {

                try
                {
                    conn.Open();
                    string query = " UPDATE dh SET status=2 WHERE guid='" + guid + "'";
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        command.ExecuteNonQuery();
                    }                    
                    
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message);
                    error = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    error = true;
                }
            }
            if (!error)
            {
                this.Close();
            }                
             
            this.Close();
            
        }

        private void btn_box_Click(object sender, EventArgs e)
        {

            if (typ_doc != "4")
            {
                using (Boxes boxes = new Boxes())
                {
                    boxes.guid = guid;
                    boxes.typ_doc = this.typ_doc;
                    boxes.its_new = this.its_new;
                    boxes.TopMost = true;
                    boxes.status = status;
                    boxes.info1c = info1c;
                    boxes.ShowDialog();
                }                
            }           
        }

        //private void btn_delete_Click(object sender, EventArgs e)
        //{
        //    if (DialogResult.Yes != MessageBox.Show("Вы хотите безвозвратно удалить документ ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
        //    {
        //        return;
        //    }

        //    SQLiteConnection conn = Program.ConnectForDataBase();
        //    SQLiteTransaction tran = null;

        //    try
        //    {
        //        conn.Open();
        //        tran = conn.BeginTransaction();
        //        string query = " DELETE FROM dh WHERE guid='" + guid + "'";
        //        SQLiteCommand command = new SQLiteCommand(query, conn);
        //        command.Transaction = tran;
        //        command.ExecuteNonQuery();
        //        query = " DELETE FROM dt WHERE guid='" + guid + "'";
        //        command = new SQLiteCommand(query, conn);
        //        command.Transaction = tran;
        //        command.ExecuteNonQuery();
        //        tran.Commit();
        //        tran.Dispose();
        //        command.Dispose();
        //        conn.Close();                
        //        this.Close();
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        if (tran != null)
        //        {
        //            tran.Rollback();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        if (tran != null)
        //        {
        //            tran.Rollback();
        //        }
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //    }
        //    conn.Dispose();                
        //}
        
    }
}