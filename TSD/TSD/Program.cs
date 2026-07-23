using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using System.Data.SQLite;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Net;

namespace TSD
{
    static class Program
    {
        ///// <summary>
        ///// The main entry point for the application.
        ///// </summary>
        //[MTAThread]
        //static void Main()
        //{

        //    if (!BaseExist)
        //    {
        //        Program.CreateDataBase();
        //        Program.write_setting(1);
        //    }
        //    //BackupDatabases();
        //    AddColumnDateExpiration();
        //    if (File.Exists(Program.get_startup_folder_path() + "old_TSD.exe"))
        //    {
        //        File.Delete(Program.get_startup_folder_path() + "old_TSD.exe");
        //    }
        //    //CheckAndDownloadStarter(); 
        //    string starterPath = Program.get_startup_folder_path() + "StarterTSD.exe";
        //    if (!File.Exists(starterPath))
        //    {
        //        Program.DownloadStarter(true); // false = тихий режим
        //    }
        //    string dllPath = Program.get_startup_folder_path() + "Newtonsoft.Json.Compact.dll";
        //    if (!File.Exists(dllPath))
        //    {
        //        Program.DownloadJsonDll(true);
        //    }

        //    Application.Run(new MainForm());
        //}

        ///// <summary>
        ///// The main entry point for the application.
        ///// </summary>
        [MTAThread]
        static void Main()
        {
            // 1. ПРОВЕРЯЕМ БАЗУ ДАННЫХ (переименование старой и конфликт файлов)
            if (!MigrateDatabaseName())
            {
                return; // Прерываем запуск, если обнаружено два файла базы
            }

            // 2. ПРОВЕРЯЕМ СУЩЕСТВОВАНИЕ БАЗЫ (с новым именем)
            if (!BaseExist)
            {
                Program.CreateDataBase();
                Program.write_setting(1);
            }
            AddColumnDateExpiration();

            // 3. УДАЛЯЕМ СТАРЫЙ EXE ФАЙЛ, ЕСЛИ ОН ОСТАЛСЯ ПОСЛЕ ОБНОВЛЕНИЯ
            if (File.Exists(Program.get_startup_folder_path() + "old_TSD.exe"))
            {
                File.Delete(Program.get_startup_folder_path() + "old_TSD.exe");
            }

            // 4. ПРОВЕРКА И СКАЧИВАНИЕ НЕДОСТАЮЩИХ ФАЙЛОВ (Starter и DLL)
            bool need_restart = false; // Флаг: нужно ли перезапускать программу

            string starterPath = Program.get_startup_folder_path() + "StarterTSD.exe";
            if (!File.Exists(starterPath))
            {
                if (Program.DownloadStarter(false))
                {
                    need_restart = true;
                }
            }

            string dllPath = Program.get_startup_folder_path() + "Newtonsoft.Json.Compact.dll";
            if (!File.Exists(dllPath))
            {
                if (Program.DownloadJsonDll(false))
                {
                    need_restart = true;
                }
            }

            // Если хотя бы один файл скачался, выходим, чтобы пользователь перезапустил программу
            if (need_restart)
            {
                MessageBox.Show("Скачаны недостающие файлы. Перезапустите программу.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                return; // Прерываем выполнение Main, MainForm не откроется
            }

            // 5. ЗАПУСК ГЛАВНОЙ ФОРМЫ
            Application.Run(new MainForm());
        }

        /// <summary>
        /// Проверяет наличие базы данных. Если новой БД нет, но есть старая (TSD.db), переименовывает её.
        /// </summary>
        /// <summary>
        /// Проверяет наличие базы данных. 
        /// Возвращает true, если можно продолжать работу. 
        /// Возвращает false, если есть конфликт (два файла) или ошибка.
        /// </summary>
        private static bool MigrateDatabaseName()
        {
            string newDbPath = PathForBases; // "\\Application\\TSD_database.db"
            string oldDbPath = "\\Application\\TSD.db"; // Старое имя

            try
            {
                // Если старый файл существует, с ним нужно что-то сделать
                if (File.Exists(oldDbPath))
                {
                    // Проверяем, существует ли уже файл с новым именем
                    if (File.Exists(newDbPath))
                    {
                        // КОНФЛИКТ: Оба файла на месте. Ничего не удаляем, просим пользователя разобраться.
                        MessageBox.Show("Обнаружено два файла базы данных (TSD.db и TSD_database.db). Удалите лишний файл и запустите программу снова.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return false; // Прерываем запуск
                    }
                    else
                    {
                        // Конфликта нет, просто переименовываем старую базу в новую
                        File.Move(oldDbPath, newDbPath);
                    }
                }
                return true; // Все хорошо, можно работать
            }
            catch (Exception)
            {
                // Если что-то пошло не так, тоже останавливаем запуск, чтобы не навредить
                return false;
            }
        }


        //private static string pathForBases;
        private static string passwordForBases;
        private static string conectionString;
        private static string characteristic_guid;
        private static string tovar_code;
        //private static string pathForMemoryCard;
        //private static int rotate;
        //private static string agentcode;
        private static bool baseExist;
        //private static string codeBase;
        //private static string guid = null;


        public static bool DownloadJsonDll(bool showMessages)
        {
            string dllPath = Program.get_startup_folder_path() + "Newtonsoft.Json.Compact.dll";

            try
            {
                using (TSD.WS.WS ws = new TSD.WS.WS())
                {
                    // Если запускаем по кнопке, таймаут длинный. Если при старте - короткий (15 сек).
                    ws.Timeout = showMessages ? 12000000 : 15000;

                    string device_id = Program.get_device_id();
                    string key = device_id + CryptorEngine.get_count_day_tsd();
                    string web_query = CryptorEngine.Encrypt(device_id + "|" + device_id, true, key);

                    byte[] answer = ws.GetDll(device_id, web_query, Program.GetDbId());

                    if (answer.Length > 1000)
                    {
                        using (FileStream fs = File.OpenWrite(dllPath))
                        {
                            fs.Write(answer, 0, answer.Length);
                        }

                        if (showMessages)
                        {
                            MessageBox.Show("Dll успешно получена, необходимо перезапустить программу");
                        }
                        return true; // Успех
                    }
                    else
                    {
                        // В CF используем Length вместо Count()
                        string response = UTF8Encoding.UTF8.GetString(answer, 0, answer.Length);

                        if (showMessages)
                        {
                            if (response == "1000")
                            {
                                MessageBox.Show("Этот тсд еще не зарегистрирован ");
                            }
                            else
                            {
                                MessageBox.Show("При получении обновления произошли ошибки, попробуйте позже");
                            }
                        }
                        return false; // Ошибка
                    }
                }
            }
            catch (Exception ex)
            {
                if (showMessages)
                {
                    MessageBox.Show("Ошибка сети: " + ex.Message);
                }
                return false; // Ошибка
            }
        }

        public static bool DownloadStarter(bool showMessages)
        {
            string starterPath = Program.get_startup_folder_path() + "StarterTSD.exe";

            try
            {
                using (TSD.WS.WS ws = new TSD.WS.WS())
                {
                    // Если запускаем по кнопке, таймаут длинный. Если при старте - короткий (15 сек).
                    ws.Timeout = showMessages ? 12000000 : 15000;

                    string device_id = Program.get_device_id();
                    string key = device_id + CryptorEngine.get_count_day_tsd();
                    string web_query = CryptorEngine.Encrypt(device_id + "|" + device_id, true, key);

                    byte[] answer = ws.GetStarterTSD(device_id, web_query, Program.GetDbId());

                    if (answer.Length > 1000)
                    {
                        using (FileStream fs = File.OpenWrite(starterPath))
                        {
                            fs.Write(answer, 0, answer.Length);
                        }

                        if (showMessages)
                        {
                            MessageBox.Show("StarterTSD.exe успешно получен, необходимо перезапустить программу");
                            MessageBox.Show("Впредь запускайте программу через файл StarterTSD");
                        }
                        return true; // Успех
                    }
                    else
                    {
                        string response = UTF8Encoding.UTF8.GetString(answer, 0, answer.Length);

                        if (showMessages)
                        {
                            if (response == "1000")
                            {
                                MessageBox.Show("Этот тсд еще не зарегистрирован ");
                            }
                            else if (response == "-2")
                            {
                                MessageBox.Show("Файл StarterTSD.exe не найден на сервере!");
                            }
                            else
                            {
                                MessageBox.Show("При получении обновления произошли ошибки, попробуйте позже");
                            }
                        }
                        return false; // Ошибка
                    }
                }
            }
            catch (Exception ex)
            {
                if (showMessages)
                {
                    MessageBox.Show("Ошибка сети: " + ex.Message);
                }
                return false; // Ошибка
            }
        }

        

        public static string get_startup_folder_path()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Replace(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".exe", "");
        }

        public static int GetDbId()
        {
            int result = -1;

            SQLiteConnection conn = Program.ConnectForDataBase();
            try
            {
                conn.Open();
                string query = "SELECT db_id from constants";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                object result_query = command.ExecuteScalar();
                if (result_query != null)
                {
                    result = Convert.ToInt16(result_query);
                }
                else
                {
                    MessageBox.Show(" Заполните в настройках организацию ");
                }
                command.Dispose();
                conn.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(" Ошибка при получении db_id "+ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Ошибка при получении db_id " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            conn.Dispose();

            return result;
        }

        public static string CharacteristicGuid
        {
            get
            {
                return characteristic_guid;
            }
            set
            {
                characteristic_guid = value;
            }
        }

        public static string TovarCode
        {
            get
            {
                return tovar_code;
            }
            set
            {
                tovar_code = value;
            }
        }

        /// <summary>
        /// Проверка доступности интернет соединения
        /// </summary>
        /// <returns></returns>
       public static bool ConnectionAvailable()
       {
            try
            {
                string strServer = "http://ya.ru";
                HttpWebRequest reqFP = (HttpWebRequest)HttpWebRequest.Create(strServer);
                HttpWebResponse rspFP = (HttpWebResponse)reqFP.GetResponse();
                if (HttpStatusCode.OK == rspFP.StatusCode)
                {
                    // HTTP = 200 - Интернет безусловно есть!
                    //MessageBox.Show("OK");
                    rspFP.Close();
                    return true;
                }
                else
                {
                    // сервер вернул отрицательный ответ, возможно что инета нет
                    rspFP.Close();
                    MessageBox.Show("Ошибка при создании интернет соединения");
                    return false;
                }
            }
            catch (Exception)
            {                
                MessageBox.Show("Ошибка при создании интернет соединения");
                return false;
            }
       }
    


        public static bool BaseExist
        {
            get
            {
                baseExist = false;
                if (File.Exists(Program.PathForBases))
                {
                    SQLiteConnection conn = Program.ConnectForDataBase();
                    baseExist = false;
                    try
                    {
                        conn.Open();
                        baseExist = true;
                        conn.Close();
                    }
                    catch
                    {

                    }
                    finally
                    {
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
                return baseExist;
            }          
        }
        
        private static string ConectionString
        {
            get
            {
                if (conectionString == null)
                {
                    //conectionString += "Data Source =" + Program.PathForBases + ";Password =" + Program.PasswordForBases;
                    //conectionString += "Data Source =" + Program.PathForBases + ";New=True;Version=3";
                    conectionString += "Data Source =" + Program.PathForBases + ";Version=3";
                }
                return conectionString;
            }
        }
        private static string PasswordForBases
        {
            get
            {
                if (passwordForBases == null)
                {
                    //passwordForBases = "SuperSecurePassword_2007";
                    passwordForBases = "1";
                }
                return passwordForBases;
            }

        }

        public static SQLiteConnection ConnectForDataBase()
        {
            SQLiteConnection conn = null;
            conn = new SQLiteConnection(Program.ConectionString);
            return conn;
        }

        /// <summary>
        /// Признак для новых документов поступления
        /// чтобы при добавлении товара все 
        /// время не появлялся диалог
        /// товар не найден
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static int its_new_document(string guid)
        {

            int result = -1;
            SQLiteConnection conn = Program.ConnectForDataBase();
            try
            {
                conn.Open();
                string query = "SELECT its_new FROM dh WHERE guid='" + guid + "'";
                SQLiteCommand command = new SQLiteCommand(query, conn);                
                object result_query = command.ExecuteScalar();
                if (result_query != null)
                {
                    result = Convert.ToInt16(result_query);
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(" Ошибка при получении места создания документа "+ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Ошибка при получении места создания документа " + ex.Message);
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

        //public static string get_guis_1_status()
        //{
        //    string result = "";//есть такие документы

        //    SQLiteConnection conn = Program.ConnectForDataBase();
        //    try
        //    {
        //        string query = "SELECT guid FROM dh WHERE status=1";
        //        SQLiteCommand command = new SQLiteCommand(query, conn);
        //        SQLiteDataReader reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            result += "'" + reader["guid"].ToString() + "',";
        //        }
        //        reader.Close();
        //        conn.Close();

        //        result = result.Substring(0, result.Length - 1);
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        result = "-1";
        //        MessageBox.Show(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        result = "-1";
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

        public static void shrink_database()
        {
            //SqlCeEngine engine = new SqlCeEngine(Program.ConectionString);
            //engine.Compact(null);
            //engine.Shrink();
 
        }
        //public static string PathForMemoryCard
        //{
        //    get
        //    {
        //        if (pathForMemoryCard == null)
        //        {
        //            //if (Directory.Exists("\\Storage Card") == true)
        //            //{
        //            //    pathForMemoryCard += "\\Storage Card";
        //            //}
        //            //else if (Directory.Exists("\\SDMMC Disk") == true)
        //            //{
        //            //    pathForMemoryCard += "\\SDMMC Disk";
        //            //}
        //            //else if (Directory.Exists("\\SD Disk") == true)
        //            //{
        //            //    pathForMemoryCard += "\\SD Disk";
        //            //}
        //            //else if (Directory.Exists("\\SD-MMCard") == true)
        //            //{
        //            //    pathForMemoryCard += "\\SD-MMCard";
        //            //}
        //            //else if (Directory.Exists("\\Карта Памяти") == true)
        //            //{
        //            //    pathForMemoryCard += "\\Карта Памяти";
        //            //}
        //            //else
        //            //{
        //                pathForMemoryCard = "\\";
        //            //}
        //            //try
        //            //{

        //            //}
        //            return pathForMemoryCard;
        //        }
        //        else
        //        {
        //            return pathForMemoryCard;
        //        }
        //    }
        //}
        //public static string PathForBases
        //{
        //    get
        //    {
        //        //if (pathForBases == null)
        //        //{
        //            //if (Directory.Exists("\\Storage Card")==true)
        //            //{
        //            //    pathForBases+="\\Storage Card\\Data.sdf";
        //            //}
        //            //else if (Directory.Exists("\\SDMMC Disk")==true)
        //            //{
        //            //    pathForBases+="\\SDMMC Disk\\Data.sdf";
        //            //}
        //            //else if (Directory.Exists("\\SD Disk")==true)
        //            //{
        //            //    pathForBases+="\\SD Disk\\Data.sdf";
        //            //}
        //            //else
        //            //{
        //            //    pathForBases+="Data.sdf";
        //            //}
        //            //return "\\Storage Card\\TSD.db";
        //            return "\\Application\\TSD.db";
        //            //return "\\TSD.sdf";
        //            //return pathForBases;
        //        //}
        //        //else
        //        //{
        //            //return pathForBases;
        //        //}
        //    }
        //}

        public static string PathForBases
        {
            get
            {
                // Возвращаем новое имя файла базы данных
                return "\\Application\\TSD_database.db";
            }
        }

        //public static string UploadLogs()
        //{

        //    //textBox1.Text = "Попытка передачи логов";
        //    string result_upload = "-1";
        //    WS.WS ds = new TSD.WS.WS();
        //    ds.Timeout = 200 * 1000;
        //    string device_id = Program.get_device_id();

        //    int num_base = Program.GetDbId();

        //    //SQLiteConnection conn = Program.ConnectForDataBase();
        //    //SQLiteCommand command = null;
        //    //SQLiteTransaction trans = null;
        //    StringBuilder sb = new StringBuilder();
        //    //string query = "";
        //    string nick_shop = Program.get_code_shop();
                     

        //    try
        //    {
        //        //conn.Open();              

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

        //        result_upload = ds.Upload_logs(device_id, encrypt_data, num_base);
        //        if (result_upload == "1")
        //        {
        //            File.Delete("\\Application\\Logs.txt");
        //        }                  
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }            
        //    return result_upload;
        //}             


        //public static void write_log(string log_message)
        //{
        //    using (StreamWriter sw = new StreamWriter("\\Application\\Logs.txt",true))
        //    {
        //        sw.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "," + log_message);                
        //    }
        //    GC.Collect();
        //    GC.WaitForPendingFinalizers();
            
            //SQLiteConnection conn = Program.ConnectForDataBase();
            //try
            //{
            //    conn.Open();
            //    string query = " INSERT INTO logs(date,description)VALUES('"+
            //                   DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + log_message + "')";                    
            //    SQLiteCommand command = new SQLiteCommand(query, conn);
            //    command.ExecuteNonQuery();                
            //    conn.Close();
            //}
            //catch (SQLiteException ex)
            //{
            //    MessageBox.Show(" Ошибка при логировании " + ex.Message);

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(" Ошибка при логировании " + ex.Message);
            //}
            //finally
            //{
            //    if (conn.State == ConnectionState.Open)
            //    {
            //        conn.Close();                    
            //    }
            //}
            //conn.Dispose();
        //}

        //public static void write_log(string log_message, SQLiteConnection conn)
        //{
        //    using (StreamWriter sw = new StreamWriter("\\Application\\Logs.txt",true))
        //    {
        //        sw.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "," + log_message);
        //    }
        //    //SQLiteConnection conn = Program.ConnectForDataBase();
        //    //try
        //    //{
        //    //    //conn.Open();
        //    //    string query = " INSERT INTO logs(date,description)VALUES('" +
        //    //                   DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + log_message + "')";
        //    //    SQLiteCommand command = new SQLiteCommand(query, conn);
        //    //    command.ExecuteNonQuery();
        //    //    //conn.Close();
        //    //}
        //    //catch (SQLiteException ex)
        //    //{
        //    //    MessageBox.Show(" Ошибка при логировании " + ex.Message);

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageBox.Show(" Ошибка при логировании " + ex.Message);
        //    //}
        //    //finally
        //    //{
        //    //    if (conn.State == ConnectionState.Open)
        //    //    {
        //    //        //conn.Close();
        //    //    }
        //    //}
        //    //conn.Dispose();
        //}

        public static string get_code_shop()
        {
            string result = "";

            SQLiteConnection conn = Program.ConnectForDataBase();
            try
            {
                conn.Open();
                string query = " SELECT shop FROM shop ";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                object result_query = command.ExecuteScalar();
                if (result_query != null)
                {
                    result = result_query.ToString();
                }
                conn.Close();                
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(" Ошибка при получении кода магазина " + ex.Message);

            }
            catch (Exception ex)
            {
                MessageBox.Show(" Ошибка при получении кода магазина " + ex.Message);
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
        
        //***********************************
        //определение номера девайса и соотвественно проверка 
        //типа будем работать на этом девайсе
        private static Int32 METHOD_BUFFERED = 0;
        private static Int32 FILE_ANY_ACCESS = 0;
        private static Int32 FILE_DEVICE_HAL = 0x00000101;
        private const Int32 ERROR_NOT_SUPPORTED = 0x32;
        private const Int32 ERROR_INSUFFICIENT_BUFFER = 0x7A;
        private static Int32 IOCTL_HAL_GET_DEVICEID = ((FILE_DEVICE_HAL) << 16) | ((FILE_ANY_ACCESS) << 14) | ((21) << 2) | (METHOD_BUFFERED);
        [DllImport("coredll.dll", SetLastError = true)]
        private static extern bool KernelIoControl(Int32 dwIoControlCode, IntPtr lpInBuf, Int32 nInBufSize, byte[] lpOutBuf, Int32 nOutBufSize, ref Int32 lpBytesReturned);
        public static string get_device_id()
        {
            // Initialize the output buffer to the size of a Win32 DEVICE_ID structure
            byte[] outbuff = new byte[20];
            Int32 dwOutBytes;
            bool done = false;

            Int32 nBuffSize = outbuff.Length;

            // Set DEVICEID.dwSize to size of buffer.  Some platforms look at
            // this field rather than the nOutBufSize param of KernelIoControl
            // when determining if the buffer is large enough.
            //
            BitConverter.GetBytes(nBuffSize).CopyTo(outbuff, 0);
            dwOutBytes = 0;


            // Loop until the device ID is retrieved or an error occurs
            while (!done)
            {
                if (KernelIoControl(IOCTL_HAL_GET_DEVICEID, IntPtr.Zero, 0, outbuff, nBuffSize, ref dwOutBytes))
                {
                    done = true;
                }
                else
                {
                    int error = Marshal.GetLastWin32Error();
                    switch (error)
                    {
                        case ERROR_NOT_SUPPORTED:
                            throw new NotSupportedException("IOCTL_HAL_GET_DEVICEID is not supported on this device", new Win32Exception(error));

                        case ERROR_INSUFFICIENT_BUFFER:
                            // The buffer wasn't big enough for the data.  The
                            // required size is in the first 4 bytes of the output
                            // buffer (DEVICE_ID.dwSize).
                            nBuffSize = BitConverter.ToInt32(outbuff, 0);
                            outbuff = new byte[nBuffSize];

                            // Set DEVICEID.dwSize to size of buffer.  Some
                            // platforms look at this field rather than the
                            // nOutBufSize param of KernelIoControl when
                            // determining if the buffer is large enough.
                            //
                            BitConverter.GetBytes(nBuffSize).CopyTo(outbuff, 0);
                            break;

                        default:
                            throw new Win32Exception(error, "Unexpected error");
                    }
                }
            }

            Int32 dwPresetIDOffset = BitConverter.ToInt32(outbuff, 0x4);    // DEVICE_ID.dwPresetIDOffset
            Int32 dwPresetIDSize = BitConverter.ToInt32(outbuff, 0x8);      // DEVICE_ID.dwPresetSize
            Int32 dwPlatformIDOffset = BitConverter.ToInt32(outbuff, 0xc);  // DEVICE_ID.dwPlatformIDOffset
            Int32 dwPlatformIDSize = BitConverter.ToInt32(outbuff, 0x10);   // DEVICE_ID.dwPlatformIDBytes
            StringBuilder sb = new StringBuilder();

            for (int i = dwPresetIDOffset; i < dwPresetIDOffset + dwPresetIDSize; i++)
            {
                sb.Append(String.Format("{0:X2}", outbuff[i]));
            }

            sb.Append("-");
            for (int i = dwPlatformIDOffset; i < dwPlatformIDOffset + dwPlatformIDSize; i++)
            {
                sb.Append(String.Format("{0:X2}", outbuff[i]));

            }
            return sb.ToString();
            //return "12345";
            //return "00195B2FF60701082F18-0403020E3000";

        }



        public static void BackupDatabases()
        {
            return;
            if (!Directory.Exists("\\Application\\BackUp"))
            {
                Directory.CreateDirectory("\\Application\\BackUp");
            }
            if (!Directory.Exists("\\Application\\BackUp"))
            {
                MessageBox.Show("Не удалось создать папку для копирования");
            }
            PlaySound ps = new PlaySound();
            ps.PlaySound_WAV("\\Windows\\infbeg.wav");
            SortedList<int, string> dict = new SortedList<int, string>();
            string new_file_name = "";
            //string[] database_files = Directory.GetFiles("\\Storage Card", "*.db");
            string[] database_files = Directory.GetFiles("\\Application\\BackUp", "*.db");
            foreach (string str in database_files)
            {
                int first_touch = str.IndexOf("TSD");
                int second_touch = str.IndexOf(".");
                new_file_name = str.Substring(first_touch + 3, second_touch - first_touch - 3);
                dict.Add(Convert.ToInt32(new_file_name), str);
            }

            int count_files = dict.Values.Count;

            if (count_files > 0)
            {
                //Получить новое имя файла
                new_file_name = dict.Values[count_files - 1];
                int first_touch = new_file_name.IndexOf("TSD");
                int second_touch = new_file_name.IndexOf(".");
                new_file_name = new_file_name.Substring(first_touch + 3, second_touch - first_touch - 3);
                //new_file_name = "\\Storage Card\\TSD" + (int.Parse(new_file_name) + 1).ToString() + ".db";
                new_file_name = "\\Application\\BackUp\\TSD" + (int.Parse(new_file_name) + 1).ToString() + ".db";
            }
            else
            {
                //new_file_name = "\\Storage Card\\TSD1.db";
                new_file_name = "\\Application\\BackUp\\TSD1.db";
            }


            //DateTime start = DateTime.Now;
            
            
            string file_destination = new_file_name;
            //string file_destination_temp = "\\Storage Card\\TSD" + DateTime.Now.Hour.ToString() + "_temp.db";
            //if (File.Exists(file_destination))
            //{
            //    File.Move(file_destination, file_destination_temp);
            //}
            using (SQLiteConnection conn_source = new SQLiteConnection(Program.ConectionString))
            {
                using (SQLiteConnection conn_destination = new SQLiteConnection("Data Source =" + file_destination + ";Version=3"))
                {
                    try
                    {
                        conn_source.Open();
                        conn_destination.Open();
                        conn_source.BackupDatabase(conn_destination, "main", "main", -1, null, -1);
                        //conn_source.Close();
                        //conn_destination.Close();
                        if (dict.Keys.Count > 9)
                        {
                            for (int i = 0; i < dict.Keys.Count - 9; i++)
                            {
                                File.Delete(dict.Values[i]);
                            }
                        }
                        //if (File.Exists(file_destination_temp))
                        //{
                        //    File.Delete(file_destination_temp);
                        //}
                    }
                    catch (SQLiteException)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                    catch (Exception)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                }
            }
            //finally
            //{
            //    if (conn_source.State == ConnectionState.Open)
            //    {
            //        conn_source.Close();
            //    }
            //    if (conn_destination.State == ConnectionState.Open)
            //    {
            //        conn_destination.Close();
            //    }
            //}
            //conn_source.Dispose();
            //conn_destination.Dispose();

            ps.PlaySound_WAV("\\Windows\\startup.wav");

            //MessageBox.Show((DateTime.Now-start).TotalSeconds.ToString());
        }

        public static bool exist_code_shop()
        {
            string shop = Program.get_code_shop();
            return(shop != "");
        }


        private static void AddColumnDateExpiration()
        {                
            SQLiteConnection conn = null;            
            using (conn = new SQLiteConnection(Program.ConectionString))
            {
                try
                {
                    conn.Open();                    
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "ALTER TABLE dt ADD COLUMN date_expiration DATETIME DEFAULT '1900-01-01';";                    
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        private static bool verify_db_id()
        {
            bool result = false;

            SQLiteConnection conn = Program.ConnectForDataBase();
            try
            {
                conn.Open();
                string query = "SELECT db_id FROM constants";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                object result_query = command.ExecuteScalar();
                if (result_query == null)
                {
                    result = true;
                }
                else
                {
                    query = "SELECT COUNT(*) FROM dh WHERE db_id<>" + result_query.ToString() + " AND status<>3";
                    command = new SQLiteCommand(query, conn);
                    int count_doc = Convert.ToInt32(command.ExecuteScalar());
                    result = true;

                    if (count_doc != 0)
                    {
                        result = false;
                        MessageBox.Show(" Существуют не переданные документы предыдущей базы ");
                    }
                    else
                    {
                        result = true;
                    }
                }
                command.Dispose();
                conn.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(" Ошибки при проверке db_id " + ex.Message);
                result = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Ошибки при проверке db_id " + ex.Message);
                result = false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            conn.Dispose();

            return result;
        }



        public static bool write_setting(int num_base)
        {
            bool result = true;

            if (!verify_db_id())
            {
                result = false;
                return result;
            }
            if (num_base == -1)
            {
                num_base = 1;
            }

            SQLiteConnection conn = Program.ConnectForDataBase();
            try
            {
                conn.Open();
                string query = "UPDATE constants SET db_id = " + num_base.ToString();
                SQLiteCommand command = new SQLiteCommand(query, conn);
                int rowsaffected = command.ExecuteNonQuery();
                if (rowsaffected == 0)
                {
                    query = "INSERT INTO constants(db_id)VALUES(" + num_base.ToString() + ")";
                    command = new SQLiteCommand(query, conn);
                    command.ExecuteNonQuery();
                }
                command.Dispose();
                conn.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(" Ошибки при записи настроек " + ex.Message);
                result = false;
            }
            catch (Exception ex)
            {
                result = false;
                MessageBox.Show(" Ошибки при записи настроек " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            conn.Dispose();

            return result;
        }


        public static bool CreateDataBase()
        {
            try
            {
                //SQLiteConnection 
                SQLiteConnection conn = null;
                SQLiteTransaction myTrans;
                SQLiteConnection.CreateFile(Program.PathForBases);
                //SqlCeEngine engine = new SqlCeEngine(Program.ConectionString);                
                //engine.CreateDatabase();
                //engine.Compact(Program.ConectionString);
                conn = new SQLiteConnection(Program.ConectionString);                
                conn.Open();                
                myTrans = conn.BeginTransaction();

                SQLiteCommand cmd = conn.CreateCommand();
                
                //cmd.CommandText = "CREATE TABLE docs_header(" +
                //    " shop  nvarchar(3) , " +                              //Код магазина 
                //    " type smallint , " +                              //Вид документа                              
                //    " ident nvarchar(36) ," +                                  //Идентификатор из 1с                                      
                //    " date datetime ,"+                                       //Дата документа                     
                //    " guid_1s nvarchar(36) , " +
                //    " comment nvarchar(100),"+
                //    " status smallint);";
                //cmd.Transaction = myTrans;  
                //cmd.ExecuteNonQuery();

                cmd.CommandText =" CREATE TABLE tovar("+
                    " code  int NOT NULL,"+
                    " name  nvarchar(100) NOT NULL," +
                    " retail_price numeric(10,2), " +
                    " purchase_price numeric(10,2)," +
                    " its_deleted smallint NOT NULL,"+
                    " nds int NOT NULL)";
                cmd.Transaction = myTrans;
                cmd.ExecuteNonQuery();
                              

                cmd.CommandText = "CREATE INDEX Ind_tovar ON tovar(code)";
                cmd.Transaction = myTrans;
                cmd.ExecuteNonQuery();

                //cmd.CommandText = " CREATE TABLE barcode(" +
                //    " tovar_code int NOT NULL," +
                //    " barcode nvarchar(13) NOT NULL)";
                //cmd.Transaction = myTrans;
                //cmd.ExecuteNonQuery();

                cmd.CommandText = " CREATE TABLE barcodes(" +
                  " tovar_code int NOT NULL," +
                  " barcode_code nvarchar(13) NOT NULL)";
                cmd.Transaction = myTrans;
                cmd.ExecuteNonQuery();

                //cmd.CommandText = "CREATE UNIQUE INDEX Ind_barcodes_barcode_code ON barcodes(barcode_code)";
                cmd.CommandText = "CREATE INDEX Ind_barcodes_barcode_code ON barcodes(barcode_code)";
                cmd.Transaction = myTrans;
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE INDEX Ind_barcodes_tovar_code ON barcodes(tovar_code)";
                cmd.Transaction = myTrans;
                cmd.ExecuteNonQuery();

                //cmd.CommandText = " CREATE TABLE tovar2(" +
                //   " code  int NOT NULL," +
                //   " name  nvarchar(100) NOT NULL," +
                //   " retail_price numeric(10,2), " +
                //   " purchase_price numeric(10,2)," +
                //   " its_deleted smallint NOT NULL," +
                //   " nds int NOT NULL)";
                //cmd.ExecuteNonQuery();

                cmd.CommandText =" CREATE TABLE characteristic("+
                    " tovar_code  int NOT NULL,"+
                    " guid nvarchar(36) NOT NULL," +
                    " name nvarchar(100) NOT NULL,"+
                    " retail_price_characteristic numeric(10,2))";
                cmd.Transaction = myTrans;
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE INDEX Ind_characteristic ON characteristic(tovar_code,guid)";
                cmd.Transaction = myTrans;
                cmd.ExecuteNonQuery();

                cmd.CommandText = " CREATE TABLE shop("+
                    " shop nvarchar(3) NOT NULL)";
                cmd.Transaction = myTrans;
                cmd.ExecuteNonQuery();

                cmd.CommandText = " CREATE TABLE sync(" +
                   " datesync datetime NOT NULL)";
                cmd.Transaction = myTrans;
                cmd.ExecuteNonQuery();

                cmd.CommandText = " CREATE TABLE constants(" +
                    " db_id smallint NOT NULL,date nvarchar(10))";
                cmd.Transaction = myTrans;
                cmd.ExecuteNonQuery();

                //Установим первичную дату загрузки данных
                //***********************************************
                cmd.CommandText = " INSERT INTO constants(db_id,date)VALUES(0,'01.01.2000') ";
                cmd.Transaction = myTrans;
                cmd.ExecuteNonQuery();
                //***********************************************

                cmd.CommandText = " CREATE TABLE last_scaned(" +
                    " guid nvarchar(36) NOT NULL,"+
                    " description nvarchar(300) NOT NULL)";
                cmd.Transaction = myTrans;
                cmd.ExecuteNonQuery();

                cmd.CommandText = " CREATE TABLE dh(" +
                   " type smallint NOT NULL,"+
                   " date datetime NOT NULL," +
                   " guid nvarchar(36) NOT NULL," +
                   " info_1s nvarchar(200),"+
                   " status smallint,"+
                   " display_quantity smallint,"+
                   " its_new smallint," +
                   " db_id smallint NOT NULL ,"+
                   " allow_surplus smallint NOT NULL " +
                   ")";
                cmd.Transaction = myTrans;
                cmd.ExecuteNonQuery();

                cmd.CommandText = " CREATE TABLE dt(" +
                    " guid nvarchar(36) NOT NULL," +
                    " tovar_code int NOT NULL," +
                    " characteristic nvarchar(36),"+
                    " quantity int NOT NULL," + //это количество в 1с планируемое 
                    " quantity_shop int," +     //это количество в магазине фактическое
                    " price_buy numeric(10,2)," +
                    " price numeric(10,2),"+
                    " line_number int, "+
                    " its_sent smallint ,"+
                    " box nvarchar(10)," +
                    " box_status nvarchar(1),"+
                    " date_expiration datetime default '1900-01-01' " +
                    ")";
                cmd.Transaction = myTrans;
                cmd.ExecuteNonQuery();

                //cmd.CommandText = "CREATE INDEX Ind_dt ON dt(line_number,guid,tovar_code,characteristic)";
                cmd.CommandText = "CREATE INDEX Ind_dt ON dt(guid,tovar_code,line_number)";
                cmd.Transaction = myTrans;
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE INDEX Ind_dt_box ON dt(guid,box)";
                cmd.Transaction = myTrans;
                cmd.ExecuteNonQuery();

                //добавляем логирование чтобы найти зависание 
                cmd.CommandText = " CREATE TABLE logs(" +
                    " date datetime NOT NULL," +
                    " description nvarchar(300) NOT NULL)";
                cmd.Transaction = myTrans;
                cmd.ExecuteNonQuery();

                myTrans.Commit();
                conn.Close();
                myTrans.Dispose();
                cmd.Dispose();
                conn.Dispose();
                return true;
            }

            catch(SQLiteException ex)
            {
                MessageBox.Show(" Ошибки при создании бд " + ex.Message);
                return false;
            }

        }


    }
}