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
    public partial class MainForm : Form
    {

        System.Windows.Forms.Timer timer_backup_data = new Timer();
        //System.Threading.Timer timer_send_data = new System.Threading.Timer();
        private PowerStatus ps = new PowerStatus();

        public MainForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.Load += new EventHandler(MainForm_Load);
            this.Paint += new PaintEventHandler(MainForm_Paint);
            //PlaySound ps = new PlaySound();
            //ps.PlaySound_WAV("\\Windows\\Decode.wav");
            //Program.write_log("Start programm");
            
        }


        /// <summary>
        /// Отправим данные если таковые есть
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_send_data_Tick(object sender, EventArgs e)
        {
        //    SQLiteConnection conn = Program.ConnectForDataBase();
        //    string list_guid = "'";
        //    try
        //    {
        //        conn.Open();
        //        string query = "SELECT DISTINCT guid FROM dt WHERE its_sent=1";
        //        SQLiteCommand command = new SQLiteCommand(query, conn);
        //        SQLiteDataReader reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            list_guid += reader["guid"].ToString()+"',";
        //        }
        //        reader.Close();

        //        list_guid = list_guid.Substring(0, list_guid.Length - 1);

        //        //получить guid устройства
        //        string device_id = Program.get_device_id();

        //        //получить код магазина
        //        string shop = Program.get_code_shop();
        //        if (shop == "")
        //        {
        //            return;
        //        }
        //        ////////////////////////////////////////////////////
        //        query = "SELECT type,date,guid,info_1s FROM dh WHERE guid in(" + list_guid+");";
        //        command = new SQLiteCommand(query, conn);
        //        reader = command.ExecuteReader();
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("SHAPKA");
        //        //поля на сервере 
        //        //shop,type,tsd_ident,date_tsd,guid_1s,comment,datetime_unloading
        //        while (reader.Read())
        //        {
        //            string s = "'" + shop + "'," +
        //                reader["type"].ToString() + ",'" +
        //                device_id + "','" +
        //                reader.GetDateTime(1).ToString("dd-MM-yyyy HH:mm:ss") + "','" +
        //                reader["guid"].ToString() + "','" +
        //                //(reader["info_1s"].ToString().Trim().Length > 100 ? reader["info_1s"].ToString().Substring(0, 100) : reader["info_1s"].ToString()) + "'|";
        //                reader["info_1s"].ToString() + "'|";
        //            sb.Append(s);
        //            //break;
        //        }
        //        reader.Close();
        //        sb.Append("SHAPKA");

        //        sb.Append("STROKI");

        //        //SELECT [guid_1s]      ,[line_number]      ,[tovar_code]      ,[characteristic]      ,[quantity]      ,[quantity_1s]      ,[price_buy]      ,[price]  FROM [cash_8].[dbo].[tsd_docs_table]
        //        StringBuilder after_sending = new StringBuilder();
        //        query = " SELECT guid,line_number,tovar_code,characteristic,quantity_shop,quantity,price_buy,price from dt WHERE its_sent=1 ";// 
        //        command = new SQLiteCommand(query, conn);
        //        reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            string s = "'" + reader["guid"] + "'," +
        //                reader["line_number"].ToString() + "," +
        //                //reader["line_number"].ToString() + "," +
        //                reader["tovar_code"].ToString() + ",'" +
        //                reader["characteristic"].ToString() + "'," +
        //                reader["quantity_shop"].ToString() + "," + //quantity
        //                reader["quantity"].ToString() + "," +      //quantity_1s                        
        //                reader["price_buy"].ToString().Replace(",", ".") + "," +
        //                reader["price"].ToString().Replace(",", ".") + "|";
        //            sb.Append(s);
        //            after_sending.Append(" UPDATE dt SET its_sent=0 WHERE guid='"+reader["guid"]+"' AND line_number = "+reader["line_number"].ToString()+";");
        //            //break; 
        //        }
        //        reader.Close();
        //        conn.Close();
        //        sb.Append("STROKI");
        //        ////////////////////////////////////////////////////
                
        //        ////////////////////////////////////////////////////
        //        DS.DS ds = new TSD.DS.DS();
        //        string key = device_id + CryptorEngine.get_count_day_tsd();
        //        string encrypt_data = CryptorEngine.Encrypt(device_id + sb.ToString() + device_id, true, key);
        //        //*****************************************************                
        //        //System.IO.StreamWriter sw = new System.IO.StreamWriter("\\timer_query.txt");
        //        //sw.WriteLine(device_id);
        //        //sw.WriteLine(encrypt_data);
        //        //sw.Close();
        //        //return;
        //        //*****************************************************
        //        string result_upload = ds.UploadDocumentTSD(device_id, encrypt_data);
        //        if (result_upload == "1")//Необходимо обнулить статус к отправке
        //        {
        //            clear_status_its_sent(after_sending);                  
        //        }
                                
        //        reader.Close();
        //        conn.Close();
        //        command.Dispose();
        //        reader.Dispose();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //    }
        }

        private void clear_status_its_sent(StringBuilder after_sending)
        {
            SQLiteConnection conn = Program.ConnectForDataBase();
            SQLiteTransaction trans = null;

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string[] query = after_sending.ToString().Split(';');
                foreach (string s in query)
                {
                    SQLiteCommand command = new SQLiteCommand(s, conn);
                    command.Transaction = trans;
                    command.ExecuteNonQuery();
                }
                trans.Commit();
                conn.Close();
            }
            catch (Exception)
            {
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
        }

        void MainForm_Paint(object sender, PaintEventArgs e)
        {
            label_powerstatus.Text = ps.ReportPowerStatus("main") + " | " + ps.ReportPowerStatus("");
        }

        void MainForm_Load(object sender, EventArgs e)
        {            
            timer_backup_data.Interval = 3600000;//1800000;
            timer_backup_data.Enabled = true;
            timer_backup_data.Tick += new EventHandler(timer_backup_data_Tick);
            get_zagolovok();
            //Program.UploadLogs();
        }

        private void timer_backup_data_Tick(object sender, EventArgs e)
        {
            Program.BackupDatabases();
        }

        private void get_zagolovok()
        {           
           this.Text = Program.get_code_shop()+" "+Program.get_device_id()+" 08.10.2017";
        }
                
        protected override void OnKeyDown(KeyEventArgs e)
        {
            //base.OnKeyDown(e);
            
            if (e.KeyCode == Keys.D0)
            {
                btn_change_data_Click(null, null); 
            }
            else if (e.KeyCode == Keys.D1)
            {
                btn_view_tovar_Click(null, null);
            }
            else if (e.KeyCode == Keys.D6)
            {
                button2_Click(null, null);
            }
            else if (e.KeyCode == Keys.D5)
            {
                btn_inventory_Click(null, null);
            }
            else if (e.KeyCode == Keys.D3)
            {
                btn_goods_receipt_Click(null, null);
            }
            else if (e.KeyCode == Keys.D4)
            {
                btn_customer_order_Click(null, null);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.D7)
            {
                btn_setting_Click(null, null);
            }
            else if (e.KeyCode == Keys.D2)
            {
                btn_check_price_Click(null, null);
            }
        }

        private void btn_inventory_Click(object sender, EventArgs e)
        {
            if (!Program.exist_code_shop())
            {
                MessageBox.Show("Необходимо выполнить полную синхронизацию");
                return;
            }
            using (JoutnalInventory ji = new JoutnalInventory())
            {
                ji.typ_doc = 1;
                ji.ShowDialog();
            }
        }
        
        private void btn_change_data_Click(object sender, EventArgs e)
        {
            this.timer_backup_data.Enabled = false;
            using (ChageData cd = new ChageData())
            {
                cd.ShowDialog();
            }
            this.timer_backup_data.Enabled = true;            
        }
        
        private void btn_Esc_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void btn_goods_receipt_Click(object sender, EventArgs e)
        {
            //if (!Program.exist_code_shop())
            //{
            //    MessageBox.Show("Необходимо выполнить полную синхронизацию");
            //    return;
            //}
            //this.timer_backup_data.Enabled = false;
            //JoutnalInventory ji = new JoutnalInventory();
            //ji.typ_doc = 2;
            //ji.ShowDialog();
            //ji.Dispose();
            //this.timer_backup_data.Enabled = true;         
            if (!Program.exist_code_shop())
            {
                MessageBox.Show("Необходимо выполнить полную синхронизацию");
                return;
            }

            this.timer_backup_data.Enabled = false;

            using (JoutnalInventory ji = new JoutnalInventory())
            {
                ji.typ_doc = 2;
                ji.ShowDialog();
            }

            this.timer_backup_data.Enabled = true;
        }

        private void btn_full_load_Click(object sender, EventArgs e)
        {

        }

        private void btn_customer_order_Click(object sender, EventArgs e)
        {
            if (!Program.exist_code_shop())
            {
                MessageBox.Show("Необходимо выполнить полную синхронизацию");
                return;
            }
            this.timer_backup_data.Enabled = false;
            JoutnalInventory ji = new JoutnalInventory();
            ji.typ_doc = 3;
            ji.ShowDialog();
            ji.Dispose();
            this.timer_backup_data.Enabled = true;             
        }

        private void btn_setting_Click(object sender, EventArgs e)
        {
            Setting st = new Setting();
            DialogResult dr = st.ShowDialog();
            if (dr == DialogResult.Cancel)
            {
                this.Close();
            }
        }

        private void btn_check_price_Click(object sender, EventArgs e)
        {
            //if (!Program.exist_code_shop())
            //{
                
            //    MessageBox.Show("Необходимо выполнить полную синхронизацию");
            //    return;
            //}
            //this.timer_backup_data.Enabled = false;
            //JoutnalInventory ji = new JoutnalInventory();
            //ji.typ_doc = 4;
            //ji.ShowDialog();
            //ji.Dispose();
            //this.timer_backup_data.Enabled = true;      
            if (!Program.exist_code_shop())
            {
                MessageBox.Show("Необходимо выполнить полную синхронизацию");
                return;
            }

            this.timer_backup_data.Enabled = false;

            using (JoutnalInventory ji = new JoutnalInventory())
            {
                ji.typ_doc = 4;
                ji.ShowDialog();
            }

            this.timer_backup_data.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!Program.exist_code_shop())
            {
                MessageBox.Show("Необходимо выполнить полную синхронизацию");
                return;
            }
            this.timer_backup_data.Enabled = false;
            using (JoutnalInventory ji = new JoutnalInventory())
            {
                ji.typ_doc = 5;
                ji.ShowDialog();
            }            
            this.timer_backup_data.Enabled = true;      
        }

        private void btn_view_tovar_Click(object sender, EventArgs e)
        {
            //if (!Program.exist_code_shop())
            //{

            //    MessageBox.Show("Необходимо выполнить полную синхронизацию");
            //    return;
            //}
            //this.timer_backup_data.Enabled = false;
            //JoutnalInventory ji = new JoutnalInventory();
            //ji.typ_doc = 5;
            //ji.ShowDialog();
            //ji.Dispose();
            //this.timer_backup_data.Enabled = true;     
            if (!Program.exist_code_shop())
            {
                MessageBox.Show("Необходимо выполнить полную синхронизацию");
                return;
            }

            this.timer_backup_data.Enabled = false;

            using (JoutnalInventory ji = new JoutnalInventory())
            {
                ji.typ_doc = 5;
                ji.ShowDialog();
            }

            this.timer_backup_data.Enabled = true;
        }
    }
}