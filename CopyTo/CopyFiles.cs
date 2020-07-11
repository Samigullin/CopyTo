using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CopyTo
{
    public static class CopyFiles
    {

        static string status = "Готов" ;

        /// <summary> Статус копирования </summary>
        public static string Status 
        { 
            get 
            {
                return status;
            }
            set
            {
                status = value;
                NotifyStaticPropertyChanged();
            }                
        }
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        private static void NotifyStaticPropertyChanged([CallerMemberName] string propertyName = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Копирование одного файла
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="from">Откуда</param>
        /// <param name="to">Куда</param>
        public static void copyOneFile(string fileName, string from, string to)
        {
            Status = "Идет копирование";
            FileStream fsIn = new FileStream(from + "\\" + fileName, FileMode.Open);
            FileStream fsOut = new FileStream(to + "\\" + fileName, FileMode.Create);
            byte[] bt = new byte[1048756];
            int readByte;

            try
            {
                while ((readByte = fsIn.Read(bt, 0, bt.Length)) > 0)
                {
                    fsOut.Write(bt, 0, readByte);
                }

                //File.Copy(from + "\\" + fileName, to + "\\" + fileName, true);

            }
            catch (Exception ex)
            {
                Status = ex.Message;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                fsIn.Close();
                fsOut.Close();
            }

            Status = fileName + " скопирован";
        }

        /// <summary>
        /// Копирование всех файлов в директории
        /// </summary>
        /// <param name="from">Начаьлная директория</param>
        /// <param name="to">Конечная директория</param>
        public static void execCopy(string from, string to)
        {
            Status = "Идет копирование";
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(from);
                DirectoryInfo dirInfoTo = new DirectoryInfo(to);
                foreach (FileInfo file in dirInfo.GetFiles("*.*"))
                {
                    File.Copy(file.FullName, to + "\\" + file.Name, true);
                }
            }
            catch (Exception ex)
            {
                Status = ex.Message;
                MessageBox.Show(ex.Message);
            }

            Status = "Готов!";
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
