using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BA_MobileGPS.Utilities
{
    /// <summary>
    /// Logger phục vụ cho xe chiều về
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// Namth  21/10/2017   created
    /// </Modified>
    public class Logger
    {
        private static readonly object obj = true;

        private const string DEBUG = "Debug";

        private const string ERROR = "Error";

        private static bool IsDebugEnabled
        {
            get
            {
                return true;
            }
        }

        private static bool IsErrorEnabled
        {
            get
            {
                return true;
            }
        }

        private static readonly string WorkingFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public static void WriteDebug(string content)
        {
            WriteDebug(string.Empty, content);
        }

        public static void WriteDebug(string fileName, string content)
        {
            // Nếu bật debug thì mới ghi thông tin
            if (IsDebugEnabled)
            {
                string formatContent = string.Format("{0}:{1}", DateTime.Now.ToString("HH-mm-ss"), content);

                WriteFile(DateTime.Now, DEBUG, fileName, formatContent);
            }
        }

        public static void WriteDebug(string fileName, string logType, string content)
        {
            // Nếu bật debug thì mới ghi thông tin
            if (IsDebugEnabled)
            {
                string formatContent = string.Format("{0}:{1} - {2}", DateTime.Now.ToString("HH-mm-ss"), logType, content);

                WriteFile(DateTime.Now, DEBUG, fileName, formatContent);
            }
        }

        public static void WriteError(string content)
        {
            WriteError(string.Empty, content);
        }

        public static void WriteError(string fileName, string content)
        {
            // Nếu bật Error thì mới ghi thông tin
            if (IsErrorEnabled)
            {
                string formatContent = string.Format("{0}: {1}", DateTime.Now.ToString("HH-mm-ss"), content);

                WriteFile(DateTime.Now, ERROR, fileName, formatContent);
            }
        }

        public static void WriteError(Exception ex)
        {
            WriteError(string.Format("{0}{1}{2}----------------------------{3}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine));
        }

        public static void WriteError(string fileName, Exception ex)
        {
            WriteError(fileName, string.Format("{0}{1}{2}----------------------------{3}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine));
        }

        public static string ReadDebug(DateTime time, string fileName)
        {
            return ReadFile(time, DEBUG, fileName);
        }

        public static string ReadError(DateTime time, string fileName)
        {
            return ReadFile(time, ERROR, fileName);
        }

        public static string ReadFile(string filePath)
        {
            string content = string.Empty;
            try
            {
                // Nếu file tồn tại thì mới cho phép đọc
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = File.OpenText(filePath))
                    {
                        content = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return content;
        }

        public static IEnumerable<string> GetDebugFiles(DateTime time)
        {
            return GetFiles(time, DEBUG);
        }

        public static IEnumerable<string> GetErrorFiles(DateTime time)
        {
            return GetFiles(time, ERROR);
        }

        /// <summary>
        /// Ghi nội dung ra file có cấu trúc
        /// Files\dd-MM-yyyy\LogLevel\FileName.txt
        /// </summary>
        private static bool WriteFile(string filePath, string content)
        {
            var result = false;

            lock (obj)
            {
                // Tạo thư mục nếu như thư mục chưa tồn tại
                var folder = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                // Tạo file nếu như file chưa tồn tại
                if (!File.Exists(filePath))
                {
                    using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                    {
                        fs.Close();
                    }
                }

                // Thực hiện ghi file
                using (var sw = new StreamWriter(filePath, true, Encoding.UTF8))
                {
                    sw.WriteLine(content);
                    result = true;
                }
            }
            return result;
        }

        private static void WriteFile(DateTime time, string logLevel, string fileName, string content)
        {
            try
            {
                var path = Path.Combine(string.Format("{0}/{1}/{2}/", WorkingFolder, time.ToString("dd-MM-yyyy"), logLevel));

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var file = string.Format("{0}{1}.txt", path, string.IsNullOrEmpty(fileName) ? time.ToString("HH-mm") : fileName);

                WriteFile(file, content);
            }
            catch
            {
            }
        }

        private static string ReadFile(DateTime time, string logLevel, string fileName)
        {
            string content = string.Empty;
            try
            {
                var filePath = Path.Combine(string.Format("{0}/{1}/{2}/", WorkingFolder, time.ToString("dd-MM-yyyy"), logLevel));

                using (StreamReader reader = File.OpenText(filePath))
                {
                    content = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return content;
        }

        /// <summary>
        /// Lấy ra tập các file trong thư mục
        /// </summary>
        /// <param name="time">Thời gian</param>
        /// <param name="logLevel">Level Log là gì? Debug or Error</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  25/10/2017   created
        /// </Modified>
        private static IEnumerable<string> GetFiles(DateTime time, string logLevel)
        {
            IEnumerable<string> fileNames = null;
            try
            {
                var folderPath = Path.Combine(string.Format("{0}/{1}/{2}/", WorkingFolder, time.ToString("dd-MM-yyyy"), logLevel));

                // Nếu thư mục tồn tại.
                if (Directory.Exists(folderPath))
                {
                    var files = Directory.GetFiles(folderPath);

                    if (files != null && files.Length > 0)
                    {
                        fileNames = files.Select(f => new FileInfo(f)).OrderByDescending(f => f.LastWriteTime).Select(f => f.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return fileNames;
        }
    }
}