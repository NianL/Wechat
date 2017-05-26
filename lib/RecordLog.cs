using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace WeiXinPublic.lib
{
    public class RecordLog
    {
        public static string logPath = "";
        public static string logFielPrefix = "";

        //RecordLog.WriteLog("", "A test Msg.");

        ///<summary>
        /// 保存日志的文件夹
        ///</summary>
        public static string LogPath
        {
            get
            {
                if (logPath == string.Empty)
                {
                    if (System.Web.HttpContext.Current == null)
                    {
                        // Windows Forms 应用
                        logPath = AppDomain.CurrentDomain.BaseDirectory;
                    }
                    else
                    {
                        // Web 应用
                        logPath = AppDomain.CurrentDomain.BaseDirectory + @"";
                    }

                }
                return logPath;
            }
            set { logPath = value; }
        }


        ///<summary>
        /// 日志文件前缀
        ///</summary>
        public static string LogFielPrefix
        {
            get { return logFielPrefix; }
            set { logFielPrefix = value; }
        }

        ///<summary>
        /// 写日志
        ///</summary>
        public static void WriteLog(string logFile, string msg)
        {
            try
            {
                System.IO.StreamWriter sw = System.IO.File.AppendText(
                    LogPath + LogFielPrefix + logFile + " " +
                    DateTime.Now.ToString("yyyyMMdd") + ".txt"
                    );
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + GetIP() + " " + msg);
                sw.Close();
            }
            catch
            { }
        }


        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result = String.Empty;

            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result))
            {
                return "127.0.0.1";
            }
            return result;
        }
    }

}
