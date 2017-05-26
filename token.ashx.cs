using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Text;
using System.Configuration;
using WeiXinPublic.lib;
using System.Web.Security;
using System.Data;

namespace WeiXinPublic
{
    /// <summary>
    /// Summary description for token
    /// </summary>
    public class token : IHttpHandler
    {

        public string GenerateTimeStamp(DateTime dt)
        {
            // Default implementation of UNIX time of the current UTC time  
            TimeSpan ts = dt.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public void ProcessRequest(HttpContext context)
        {
            string postString = string.Empty;
            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "POST")
            {
                using (Stream stream = HttpContext.Current.Request.InputStream)
                {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    postString = Encoding.UTF8.GetString(postBytes);
                }
                if (!string.IsNullOrEmpty(postString))
                {
                    DataSet ds = XmlMethod.GetDataSetByXml(postString);
                    string strMsgType = ds.Tables[0].Rows[0]["MsgType"].ToString().ToLower();
                    string strOpenWeiXinID = ds.Tables[0].Rows[0]["FromUserName"].ToString();
                    string strToUserName = ds.Tables[0].Rows[0]["ToUserName"].ToString();// 我们是收件人，同样我们回消息 是发件人。

                    if (strMsgType == "event") //用户点击了按钮
                    {
                        string strSubscribe = "";
                        string strEventKey = "";
                        try
                        {
                            strSubscribe = ds.Tables[0].Rows[0]["Event"].ToString().ToLower();
                            strEventKey = ds.Tables[0].Rows[0]["EventKey"].ToString();
                        }
                        catch (Exception ce)
                        {
                            strSubscribe = "-1";
                        }

                        if (strSubscribe == "subscribe")//说明是关注我们
                        {
                            ReplyOperate(context, "subscribe.xml", strOpenWeiXinID, strToUserName);
                        }
                        else if (strSubscribe == "unsubscribe" || strSubscribe == "view")//不做操作
                        { 
                        
                        }
                        else
                        {
                            ReplyOperate(context, strEventKey, strOpenWeiXinID, strToUserName);
                        }
                    }
                    else if (strMsgType == "text") //用户回复了内容
                    {
                        string strContent = ds.Tables[0].Rows[0]["Content"].ToString();

                        //收到消息后,需要告诉他！
                        context.Response.Write("<xml>"
                        + "<ToUserName><![CDATA[" + strOpenWeiXinID + "]]></ToUserName>"
                        + "<FromUserName><![CDATA[" + strToUserName + "]]></FromUserName>"
                        + "<CreateTime>" + GenerateTimeStamp(DateTime.Now) + "</CreateTime>"
                        + "<MsgType><![CDATA[text]]></MsgType>"
                        + "<Content><![CDATA[" + strContent + "]]></Content>"
                        + "</xml>");
                    }
                }
            }
            else
            {
                Auth(); //微信接入的测试
            }
        }

        private void Auth()
        {
            HttpContext.Current.Response.ContentType = "text/plain";

            string token = WeixinConfig.WeixinToken;
            string echoString = HttpContext.Current.Request.QueryString["echoStr"];
            string signature = HttpContext.Current.Request.QueryString["signature"];
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = HttpContext.Current.Request.QueryString["nonce"];

            //RecordLog.WriteLog("log", "微信接口测试:" + HttpContext.Current.Request.Url.ToString());
            //RecordLog.WriteLog("log", "echoString:" + echoString);
            //RecordLog.WriteLog("log", "signature:" + signature);
            //RecordLog.WriteLog("log", "timestamp:" + timestamp);
            //RecordLog.WriteLog("log", "nonce:" + nonce);

            if (CheckSignature(token, signature, timestamp, nonce))
            {
                if (!string.IsNullOrEmpty(echoString))
                {
                    HttpContext.Current.Response.Write(echoString);
                    HttpContext.Current.Response.End();
                }
            }
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        public bool CheckSignature(string token, string signature, string timestamp, string nonce)
        {
            string[] ArrTmp = { token, timestamp, nonce };

            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);

            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();

            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //缓存读取的图文消息
        public static Dictionary<string, string> dReplays = new Dictionary<string, string>();
        /// <summary>
        /// 回复图文消息功能
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="eventKey">对应事件，对应文件</param>
        /// <param name="strOpenWeiXinID">用户id</param>
        /// <param name="strToUserName">公众号id</param>
        private void ReplyOperate(HttpContext context, string eventKey, string strOpenWeiXinID, string strToUserName)
        {
            if (eventKey != "")
            {
                string strReply = "";
                if (dReplays.ContainsKey(eventKey))
                {
                    strReply = dReplays[eventKey];
                }
                else
                {
                    try
                    {
                        FileStream fs1 = new FileStream(context.Server.MapPath("\\autoReply\\" + eventKey), FileMode.Open);
                        StreamReader sr = new StreamReader(fs1, Encoding.UTF8);
                        strReply = sr.ReadToEnd();
                        sr.Close();
                        fs1.Close();
                        dReplays.Add(eventKey, strReply);
                    }
                    catch (Exception)
                    {
                    }
                }
                string strReplyResult = strReply.Replace("$ToUserName$", strOpenWeiXinID).Replace("$FromUserName$", strToUserName).Replace("$CreateTime$", GenerateTimeStamp(DateTime.Now));
                context.Response.Write(strReplyResult);
            }
            else
            {
                context.Response.Write("<xml>"
                    + "<ToUserName><![CDATA[" + strOpenWeiXinID + "]]></ToUserName>"
                    + "<FromUserName><![CDATA[" + strToUserName + "]]></FromUserName>"
                    + "<CreateTime>" + GenerateTimeStamp(DateTime.Now) + "</CreateTime>"
                    + "<MsgType><![CDATA[text]]></MsgType>"
                    + "<Content><![CDATA[系统繁忙,请稍后再试！]]></Content>"
                    + "</xml>");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}