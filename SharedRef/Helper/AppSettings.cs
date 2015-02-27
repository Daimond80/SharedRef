using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SharedRef.Helper
{
    /// <summary>
    /// Настройки приложения
    /// </summary>
    public static class AppSettings
    {

        public static string ConfigsPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ConfigsPath"];
            }
        }

        public static string ConnetionString
        {
            get
            {
                return ConfigurationManager.AppSettings["ConnectionString"];
            }
        }
    }

}
