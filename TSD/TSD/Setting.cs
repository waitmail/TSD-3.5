using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Diagnostics;


namespace TSD
{
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
            this.Load += new EventHandler(Setting_Load);
            this.KeyPreview = true;
            check_new_version();
        }


        //private void check_new_version()
        //{
        //    TSD.WS.WS ws = new TSD.WS.WS();
        //    string device_id = Program.get_device_id();
        //    string key = device_id + CryptorEngine.get_count_day_tsd();

        //    /*string CryptorEngine.Decrypt(device_id,true,key);*/
        //    Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        //    string str_version = version.ToString().Substring(0, 2) + "." + version.ToString().Substring(3, 2) + "." + version.ToString().Substring(6, 5);
        //    string web_query =  CryptorEngine.Encrypt(device_id+"|"+str_version, true, key);
        //    /*System.IO.StreamWriter sw = new System.IO.StreamWriter("\\query.txt");
        //    sw.WriteLine(device_id);
        //    sw.WriteLine(CryptorEngine.Encrypt(web_query, true, key));
        //    sw.Close();*/
            
        //    int num_base = Program.GetDbId();
        //    if (num_base == -1)
        //    {
        //        return;
        //    }

        //    string result_web_query="";

        //    try
        //    {                
        //        result_web_query = ws.ExistsUpdateProrgam(device_id, web_query, num_base);

        //        if (result_web_query == "1000")
        //        {
        //            MessageBox.Show("Этот тсд еще не зарегистрирован ");
        //        }
        //        else if (result_web_query == "")
        //        {
        //            lbl_have_new_version.Text = " У вас установлена актуальная версия программы  "; 
        //        }
        //        else
        //        {
        //            string answer = CryptorEngine.Decrypt(result_web_query, true, key);
        //            string answer_modify = "";
        //            if (answer != version.ToString())
        //            {
        //                answer = answer.ToString().Substring(0, 2) + "." + answer.ToString().Substring(3, 2) + "." + answer.ToString().Substring(6, 5);
        //                answer_modify = answer.ToString().Substring(0, 2) + "." + answer.ToString().Substring(3, 2) + "." + answer.ToString().Substring(6, 5).Replace(".", "");
        //                lbl_have_new_version.Text = " Имеется новая версия программы  " + answer_modify;// CryptorEngine.Decrypt(received, true, key).Replace(".", "-");
        //                lbl_have_new_version.Tag = answer;
        //                btn_get_new_program.Enabled = true;
        //            }
        //            else
        //            {
        //                lbl_have_new_version.Text = " У вас установлена актуальная версия программы  "; 
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void Setting_Load(object sender, EventArgs e)
        //{
            
        //    string startup_folder_path = Program.get_startup_folder_path();

        //    if (File.Exists(startup_folder_path + "Newtonsoft.Json.Compact.dll") || File.Exists(startup_folder_path + "newtonsoft.json.compact.dll"))
        //    {
        //        btn_get_dll.Enabled = false;
        //    }
        //    if (File.Exists(startup_folder_path + "StarterTSD.exe"))
        //    {
        //        btn_get_starter.Enabled = false;
        //    }

        //    //ws.GetExistDocumentTSD(Program.get_device_id,,Program.GetDbId());
        //    //cmb_bases.Items.Add("Чистый дом У");
        //    cmb_bases.Items.Add("Не магазин");
        //    //cmb_bases.Items.Add("Одежда");
        //    cmb_bases.Items.Add("Магазин");
        //    //cmb_bases.Items.Add("Е-сеть");            

        //    load_setting();            
        //    lbl_guid.Text = Program.get_device_id();
            
        //    FileInfo fi = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().FullName);
        //    string rem_version = fi.Name.Substring(13, 11);            
        //    //Process process =new Process();
        //    //process.StartInfo.FileName = "myProg1.exe";

        //    Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        //    lbl_version.Text = " Версия программы "+version.ToString().Substring(0, 2) + "." + version.ToString().Substring(3, 2) + "." + version.ToString().Substring(6, 5).Replace(".","");
        //}

        private void check_new_version()
        {
            TSD.WS.WS ws = new TSD.WS.WS();
            string device_id = Program.get_device_id();
            string key = device_id + CryptorEngine.get_count_day_tsd();

            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            // Формируем строку вида "День.Месяц.Год" из основных номеров версий
            //string str_version = version.Major + "." + version.Minor + "." + version.Build;
            string str_version = version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;

            string web_query = CryptorEngine.Encrypt(device_id + "|" + str_version, true, key);

            int num_base = Program.GetDbId();
            if (num_base == -1)
            {
                return;
            }

            string result_web_query = "";

            try
            {
                result_web_query = ws.ExistsUpdateProrgam(device_id, web_query, num_base);

                if (result_web_query == "1000")
                {
                    MessageBox.Show("Этот тсд еще не зарегистрирован ");
                }
                else if (result_web_query == "")
                {
                    lbl_have_new_version.Text = " У вас установлена актуальная версия программы  ";
                }
                else
                {
                    //string answer = CryptorEngine.Decrypt(result_web_query, true, key);
                    //if (answer != str_version)
                    //{
                    //    // answer приходит в формате "dd.MM.yyyy" (например "05.04.2025")
                    //    lbl_have_new_version.Text = " Имеется новая версия программы  " + answer;
                    //    lbl_have_new_version.Tag = answer;
                    //    btn_get_new_program.Enabled = true;
                    //}
                    //else
                    //{
                    //    lbl_have_new_version.Text = " У вас установлена актуальная версия программы  ";
                    //}
                    string answer = CryptorEngine.Decrypt(result_web_query, true, key);
                    if (answer != str_version)
                    {
                        // Если сервер вернул дату с ".0" на конце, отрезаем последние 2 символа для красоты на экране
                        if (answer.EndsWith(".0"))
                        {
                            answer = answer.Substring(0, answer.Length - 2);
                        }
                        lbl_have_new_version.Text = " Имеется новая версия программы  " + answer;
                        lbl_have_new_version.Tag = answer; // В Tag пойдет чистая дата 11.10.2023
                        btn_get_new_program.Enabled = true;
                    }
                    else
                    {
                        // ВОЗВРАЩАЕМ ЭТОТ БЛОК:
                        lbl_have_new_version.Text = " У вас установлена актуальная версия программы  ";
                        btn_get_new_program.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            string startup_folder_path = Program.get_startup_folder_path();

            if (File.Exists(startup_folder_path + "Newtonsoft.Json.Compact.dll") || File.Exists(startup_folder_path + "newtonsoft.json.compact.dll"))
            {
                btn_get_dll.Enabled = false;
            }
            if (File.Exists(startup_folder_path + "StarterTSD.exe"))
            {
                btn_get_starter.Enabled = false;
            }

            cmb_bases.Items.Add("Не магазин");
            cmb_bases.Items.Add("Магазин");

            load_setting();
            lbl_guid.Text = Program.get_device_id();

            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            // Выводим версию без опасных Substring
            //lbl_version.Text = " Версия программы " + version.Major + "." + version.Minor + "." + version.Build;
            lbl_version.Text = " Версия программы " + version.Major + "." + version.Minor + "." + version.Build + version.Revision;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btn_close_Click(null, null);
            }
            if (e.KeyCode == Keys.D0)
            {
                btn_write_setting_Click(null, null);
            }
            if (e.KeyCode == Keys.D1)
            {
                btn_close_Click(null, null);
            }
            if (e.KeyCode == Keys.D2)
            {
                btn_get_new_program_Click(null, null);
            }
            if (e.KeyCode == Keys.D3)
            {
                btn_get_dll_Click(null, null);
            }
            if (e.KeyCode == Keys.D4)
            {
                btn_get_starter_Click(null, null);
            }
        }




        private void load_setting()
        {
            SQLiteConnection conn = Program.ConnectForDataBase();
            try
            {
                conn.Open();
                string query = "SELECT db_id FROM constants";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                object db_id = command.ExecuteScalar();
                if ((db_id == null)||(Convert.ToInt16(db_id) == 0))
                {
                    if (cmb_bases.Items.Count == 2)
                    {
                        cmb_bases.SelectedIndex = 1;
                    }
                    else
                    {
                        cmb_bases.SelectedIndex = 2;
                    }
                }
                else
                {
                    int num_base = Convert.ToInt16(db_id);
                    if (cmb_bases.Items.Count == 2)
                    {
                        if (num_base > 1)
                        {
                            cmb_bases.SelectedIndex = 1;
                        }
                        else
                        {
                            cmb_bases.SelectedIndex = num_base;
                        }
                    }
                    else
                    {
                        cmb_bases.SelectedIndex = num_base;
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
            conn.Dispose();
        }


        //private bool verify_db_id()
        //{
        //    bool result = false;

        //    SQLiteConnection conn = Program.ConnectForDataBase();
        //    try
        //    {
        //        conn.Open();
        //        string query = "SELECT db_id FROM constants";
        //        SQLiteCommand command = new SQLiteCommand(query, conn);
        //        object result_query = command.ExecuteScalar();
        //        if (result_query == null)
        //        {
        //            result = true;
        //        }
        //        else
        //        {
        //            query = "SELECT COUNT(*) FROM dh WHERE db_id<>" + result_query.ToString() + " AND status<>3";
        //            command = new SQLiteCommand(query, conn);
        //            int count_doc = Convert.ToInt32(command.ExecuteScalar());
        //            result = true;

        //            if (count_doc != 0)
        //            {
        //                result = false;
        //                MessageBox.Show(" Существуют не переданные документы предыдущей базы ");
        //            }
        //            else
        //            {
        //                result = true;
        //            }                 
        //        }
        //        command.Dispose();
        //        conn.Close();
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        MessageBox.Show(" Ошибки при проверке db_id " + ex.Message);
        //        result = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(" Ошибки при проверке db_id " + ex.Message);
        //        result = false;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //    }

        //    conn.Dispose();

        //    return result;
        //}


        //private void write_setting()
        //{
        //    bool error = false;

        //    if (!verify_db_id())
        //    {
        //        return;
        //    }

        //    SQLiteConnection conn = Program.ConnectForDataBase();
        //    try
        //    {
        //        conn.Open();
        //        string query = "UPDATE constants SET db_id = "+cmb_bases.SelectedIndex.ToString();
        //        SQLiteCommand command = new SQLiteCommand(query, conn);
        //        int rowsaffected = command.ExecuteNonQuery();
        //        if (rowsaffected == 0)
        //        {
        //            query = "INSERT INTO constants(db_id)VALUES(" + cmb_bases.SelectedIndex.ToString()+")";
        //            command = new SQLiteCommand(query, conn);
        //            command.ExecuteNonQuery();
        //        }
        //        command.Dispose();
        //        conn.Close();
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        MessageBox.Show(" Ошибки при записи настроек "+ex.Message);
        //        error = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        error = true;
        //        MessageBox.Show(" Ошибки при записи настроек " + ex.Message);                
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //    }
        //    conn.Dispose();
        //    if (!error)
        //    {
        //        this.Close();
        //    }
        //}

        private void btn_write_setting_Click(object sender, EventArgs e)
        {
            if (Program.write_setting(cmb_bases.SelectedIndex))
            {
                this.Close();
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_get_new_program_Click(object sender, EventArgs e)
        {
            try
            {
                string startup_folder_path = Program.get_startup_folder_path();
                TSD.WS.WS ws = new TSD.WS.WS();
                string device_id = Program.get_device_id();
                string key = device_id + CryptorEngine.get_count_day_tsd();
                string web_query = CryptorEngine.Encrypt(Program.get_device_id() + "|" + lbl_have_new_version.Tag.ToString(), true, key);
                byte[] answer = ws.GetUpdateProgram(Program.get_device_id(), web_query, Program.GetDbId());     
          
                if (answer.Length > 1000)
                {
                    using (FileStream fs = File.OpenWrite(startup_folder_path + "_TSD.exe"))
                    {
                        fs.Write(answer, 0, answer.Length);
                    }
                    if (File.Exists(startup_folder_path + "_TSD.exe"))
                    {
                        File.Move(startup_folder_path + "TSD.exe", startup_folder_path + "old_TSD.exe");
                        File.Move(startup_folder_path + "_TSD.exe", startup_folder_path + "TSD.exe");
                        File.Delete(startup_folder_path + "_TSD.exe");
                    }

                    MessageBox.Show("Обновление получено, необходимо перезапустить программу");
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    //Application.Exit();
                    /*if (File.Exists("/Application/StarterTSD.exe"))
                    {
                        Process process = new Process();
                        process.StartInfo.FileName = "/Application/TSD.exe";
                        process.StartInfo.Arguments = "";
                        process.Start();
                    }*/
                }
                else
                {
                    MessageBox.Show("При получении обновления произошли ошибки, попробуйте позже");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при получении обновления "+"\r\n"+ex.Message);
            }
        }

        private void btn_get_dll_Click(object sender, EventArgs e)
        {
            if (!btn_get_dll.Enabled)
            {
                return;
            }
            // Вызываем общий метод с показом сообщений
            if (Program.DownloadJsonDll(true))
            {
                // Если успешно скачалось, закрываем форму
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void btn_get_starter_Click(object sender, EventArgs e)
        {
            if (!btn_get_starter.Enabled)
            {
                return;
            }            

            // Вызываем общий метод с показом сообщений
            if (Program.DownloadStarter(true))
            {
                // Если успешно скачалось, закрываем форму
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void btn_send_db_Click(object sender, EventArgs e)
        {
            try
            {
                this.Text = "Отправка БД...";
                this.Refresh();

                // 1. Получаем путь к рабочей базе
                string originalDbPath = Program.PathForBases; // ВАЖНО: замените на ваш реальный метод получения пути
                string tempDbPath = "\\Temp\\tsd_dump.db"; // Временная папка в памяти ТСД

                // 2. Копируем базу, чтобы снять блокировку (если программа пишет в неё)
                if (File.Exists(tempDbPath))
                {
                    File.Delete(tempDbPath);
                }
                File.Copy(originalDbPath, tempDbPath);

                // 3. Читаем скопированную базу в массив байтов (через FileStream для .NET CF)
                byte[] dbBytes;
                using (System.IO.FileStream fs = new System.IO.FileStream(tempDbPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    using (System.IO.BinaryReader br = new System.IO.BinaryReader(fs))
                    {
                        dbBytes = br.ReadBytes((int)fs.Length);
                    }
                }

                // Удаляем временный файл
                File.Delete(tempDbPath);

                // 4. Отправляем на сервер
                TSD.WS.WS ws = new TSD.WS.WS();
                string device_id = Program.get_device_id();
                string key = device_id + CryptorEngine.get_count_day_tsd();

                // Шифруем GUID для проверки авторизации
                string auth_data = CryptorEngine.Encrypt(device_id, true, key);

                this.Text = "Загрузка на сервер...";
                this.Refresh();

                // Вызываем веб-метод
                string result = ws.UploadDatabase(device_id, auth_data, dbBytes, Program.GetDbId());

                if (result == "1")
                {
                    MessageBox.Show("База успешно отправлена разработчику!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    MessageBox.Show("Ошибка отправки: " + result, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
            finally
            {
                this.Text = "Настройки"; // Возвращаем заголовок формы
            }
        }
    }
}