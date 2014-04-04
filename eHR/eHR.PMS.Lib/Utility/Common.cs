using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Text.RegularExpressions;

namespace eHR.PMS.Lib.Utility
{
    public static class Common
    {
        #region List Objects

        public static bool IsNullOrEmptyList<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }
            var collection = enumerable as ICollection<T>;
            if (collection != null)
            {
                return collection.Count < 1;
            }
            return !enumerable.Any();
        }

        #endregion

        #region Logging

        public static void Log(string filePath, string message)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                FileStream fStream = new FileStream(filePath, FileMode.Append, FileAccess.Write);
                BufferedStream bfs = new BufferedStream(fStream);
                StreamWriter sWriter = new StreamWriter(bfs);
                sWriter.WriteLine(DateTime.Now + ":" + message);
                sWriter.Close();
            }
        }

        public static void Log(string message)
        {
            Log(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), message);
        }

        public static void LogException(Exception exc, string filePath)
        {
            if (exc != null)
            {
                Log(exc.Message, filePath);
                Log(exc.StackTrace, filePath);

                if (exc.InnerException != null)
                {
                    Log(exc.InnerException.Message, filePath);
                    Log(exc.InnerException.StackTrace, filePath);
                }
            }
        }

        #endregion

        #region DateTime

        public static string ChangeDateFormat(string str)
        {
            DateTime t = DateTime.Parse(str);
            return t.ToString("dd/MM/yyyy");
        }

        public static string ChangeDateFormatVS(string str)
        {
            DateTime t = DateTime.ParseExact(str, "dd/MM/yyyy", null);
            return t.ToString("yyyy-MM-dd");
        }

        #endregion

        #region Email
        
        public static bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format. 
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        #endregion

        #region Strings

        public static string ReplaceLineBreaksForDatabase(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.TrimEnd('\r', '\n').Replace("\n", Environment.NewLine);
            }
            return str;
        }

        #endregion
    }
}
