using System;
using System.IO;
using System.Text;

namespace TSD
{
    public static class ShortcutHelper
    {
        public static void CreateShortcutIfNotExists()
        {
            try
            {
                // ★ Гарантируем слэш на конце
                string startupFolder = Program.get_startup_folder_path();
                if (!startupFolder.EndsWith("\\"))
                    startupFolder += "\\";

                string targetExe = startupFolder + "StarterTSD.exe";

                // Если стартера нет, ярлык не создаем
                if (!File.Exists(targetExe))
                    return;

                string shortcutName = "TSDApp.lnk";

                // --- Рабочий стол (ОЗУ) ---
                string desktop = "\\Windows\\Desktop\\" + shortcutName;
                if (!File.Exists(desktop))
                {
                    CreateManualShortcut(desktop, targetExe);
                }

                // --- Энергонезависимая память ---
                string appDesktopFolder = "\\Application\\Desktop";
                string appDesktop = appDesktopFolder + "\\" + shortcutName;
                if (!File.Exists(appDesktop))
                {
                    if (!Directory.Exists(appDesktopFolder))
                        Directory.CreateDirectory(appDesktopFolder);

                    CreateManualShortcut(appDesktop, targetExe);
                }
            }
            catch (Exception)
            {
                // Игнорируем ошибки
            }
        }

        private static void CreateManualShortcut(string shortcutPath, string targetExe)
        {
            // 1. Оборачиваем путь в кавычки, чтобы Windows CE поняла пробелы
            string pathWithQuotes = "\"" + targetExe + "\"";

            // 2. Формат: ДлинаСтроки#Путь (длина считается ВКЛЮЧАЯ кавычки)
            string lnkContent = pathWithQuotes.Length.ToString() + "#" + pathWithQuotes;

            // 3. Конвертируем в байты. 
            // ВАЖНО: используем новый UTF8Encoding(false), чтобы НЕ записывать BOM (невидимый символ в начале файла)
            byte[] bytes = new UTF8Encoding(false).GetBytes(lnkContent);

            // 4. Записываем байты в файл
            using (FileStream fs = new FileStream(shortcutPath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);

                // 5. Добавляем нулевой байт в конец файла (маркер конца строки для Windows CE)
                fs.WriteByte(0);
            }
        }
    }
}