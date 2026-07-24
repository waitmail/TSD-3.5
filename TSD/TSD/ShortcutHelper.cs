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
                string startupFolder = Program.get_startup_folder_path();
                if (!startupFolder.EndsWith("\\"))
                    startupFolder += "\\";

                string targetExe = startupFolder + "StarterTSD.exe";

                if (!File.Exists(targetExe))
                    return;

                string shortcutName = "TSDApp.lnk";

                // --- Рабочий стол (ОЗУ) ---
                string desktop = "\\Windows\\Desktop\\" + shortcutName;
                if (!File.Exists(desktop))
                    CreateManualShortcut(desktop, targetExe);

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
                // Игнорируем
            }
        }

        private static void CreateManualShortcut(string shortcutPath, string targetExe)
        {
            string lnkContent = targetExe.Length.ToString() + "#" + targetExe;
            byte[] bytes = Encoding.Default.GetBytes(lnkContent);

            using (FileStream fs = new FileStream(shortcutPath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }
    }
}