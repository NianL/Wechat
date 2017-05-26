using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;

namespace WeiXinPublic.lib
{
    public class WeixinConfig
    {
        public static string WeixinToken = ConfigurationManager.AppSettings["WeixinToken"].ToString();
        public static string WeixinAppID = ConfigurationManager.AppSettings["WeixinAppID"].ToString();
        public static string WeixinAppSecret = ConfigurationManager.AppSettings["WeixinAppSecret"].ToString();
        public static string WeixinDomain = ConfigurationManager.AppSettings["WeixinDomain"].ToString();
    }
}