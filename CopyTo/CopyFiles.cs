using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CopyTo
{
    public static class CopyFiles
    {
        public static void copyOneFile(string fileName, string from, string to)
        {
            try
            {
                File.Copy(from + "\\" + fileName, to + "\\" + fileName, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Выполнить копирование
        /// </summary>
        /// <param name="from">Начаьлная директория</param>
        /// <param name="to">Конечная директория</param>
        public static void execCopy(string from, string to)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(from);
                foreach (FileInfo file in dirInfo.GetFiles("*.*"))
                {
                    File.Copy(file.FullName, to + "\\" + file.Name, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Анализировать содержимое начальной директории
        /// </summary>
        /// <param name="dir">Директория</param>
        /// <returns>Список файлов</returns>
        internal static List<MyFileClass> analizeDir(string dir)
        {
            List<MyFileClass> list = new List<MyFileClass>();

            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                foreach (FileInfo file in dirInfo.GetFiles("*.*"))
                {
                    list.Add(new MyFileClass() { Name = file.Name, Date = file.LastWriteTime.ToShortDateString() + ", " +  file.LastWriteTime.ToShortTimeString(), Size = (file.Length/1024).ToString() });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return list;
        }

        public static void loadConfig( out string _from, out string _to)
        {
            _from = "";
            _to = "";

            if (!File.Exists("config.cfg")) // Проверка наличия файла конфигурации
            {
                Environment.Exit(-1);
            }

            StreamReader _config = new StreamReader("config.cfg");

            while (!_config.EndOfStream)
            {
                string _readString = _config.ReadLine();
                if (_readString.StartsWith("#")) continue; //пропускаем комменты
                else
                {
                    if (_readString.StartsWith("FROM"))
                    {
                        _from = Convert.ToString(_readString.Split(' ')[1]);
                    }
                    if (_readString.StartsWith("TO"))
                    {
                        _to = Convert.ToString(_readString.Split(' ')[1]);
                    }
                }
            }
        }
    }


}
