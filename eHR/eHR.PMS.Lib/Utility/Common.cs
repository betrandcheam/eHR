﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Globalization;

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

        public static void LogException(Exception exc)
        {
            if (exc != null)
            {
                Log(exc.Message);
                Log(exc.StackTrace);

                if (exc.InnerException != null)
                {
                    Log(exc.InnerException.Message);
                    Log(exc.InnerException.StackTrace);
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
    }
}
