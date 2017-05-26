using System;
using System.Collections.Generic;
using System.Web;
using WeiXinPublic.lib;
using System.Text;
using System.Data;
using System.Web.Security;

namespace WeiXinPublic
{
    /// <summary>
    /// Summary description for JsConfig
    /// </summary>
    public class JsConfig : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            StringBuilder strConfig = new StringBuilder();
            string strNonceStr = CreateNonceStr(16);
            string strTimestamp = CreateTimestamp(DateTime.Now);
            strConfig.Append("var _CONFIG = {");
            strConfig.Append("WeixinDomain:'" + WeixinConfig.WeixinDomain + "',");//微信网站域名
            strConfig.Append("WeixinAppID:'" + WeixinConfig.WeixinAppID + "',");//微信appid
            strConfig.Append("Signature:'" + GetSignature(WX_Jsapi_Ticket(), strNonceStr, strTimestamp, context.Request.UrlReferrer.ToString()));//jsapi 签名
            strConfig.Append("NonceStr:'" + strNonceStr + "',");//页面加载生成随机字符串
            strConfig.Append("Timestamp:'" + strTimestamp + "'");//时间戳
            strConfig.Append("}");
            context.Response.Write(strConfig);
        }

        #region 微信 Access_Token

        public static string strAccess_Token = "";//微信 Access_Token
        public static DateTime dateAccess_Token;//微信 Access_Token 缓存时间记录
        public string WX_Access_Token()
        {
            if (strAccess_Token == "" || DateTime.Now.AddMinutes(-100) >= dateAccess_Token)
            {
                dateAccess_Token = DateTime.Now;
                string urlToken = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + WeixinConfig.WeixinAppID + "&secret=" + WeixinConfig.WeixinAppSecret;
                string strResult = HttpSendRequest.SendRequestByGet(urlToken);
                DataTable dtToken = JsonMethod.JsonToDataTable(strResult);
                if (dtToken != null)
                {
                    strAccess_Token = dtToken.Rows[0]["access_token"].ToString();
                }
            }
            return strAccess_Token;
        }
        #endregion

        #region  Jsapi_Ticket 签名使用
        public static string strJsapi_Ticket = "";//微信分享 签名使用
        public static DateTime dateJsapi_Ticket;//微信分享缓存时间记录
        public string WX_Jsapi_Ticket()
        {
            if (strJsapi_Ticket == "" || DateTime.Now.AddMinutes(-100) >= dateJsapi_Ticket)
            {
                dateJsapi_Ticket = DateTime.Now;
                string strAccess_Token = WX_Access_Token();
                if (strAccess_Token != "")
                {
                    string urlGetticket = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + strAccess_Token + "&type=jsapi";
                    string strRequest = HttpSendRequest.SendRequestByGet(urlGetticket);
                    DataTable dtTicket = JsonMethod.JsonToDataTable(strRequest);
                    strJsapi_Ticket = dtTicket.Rows[0]["ticket"].ToString();
                }
            }
            return strJsapi_Ticket;
        }

        public string GetSignature(string token, string nonceStr, string timestamp, string url)
        {
            string[] ArrTmp = { "jsapi_ticket=" + token, "noncestr=" + nonceStr, "timestamp=" + timestamp, "url=" + url };

            Array.Sort(ArrTmp);
            string tmpStr = string.Join("&", ArrTmp);
            tmpStr = tmpStr.Trim('&');
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            return tmpStr.ToLower();
        }
        #endregion

        #region 生成随机字符串
        public string CreateNonceStr(int count)
        {
            Random rd = new Random();
            string str = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string result = "";
            for (int i = 0; i < count; i++)
            {
                result += str[rd.Next(str.Length)];
            }
            return result;
        }
        #endregion

        #region 生成时间戳
        public string CreateTimestamp(DateTime dt)
        {
            TimeSpan ts = dt.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}