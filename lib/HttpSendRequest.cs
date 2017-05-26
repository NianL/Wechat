using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Net;
using System.Text;

namespace WeiXinPublic.lib
{
    public class HttpSendRequest
    {
        public static string SendRequestByPost(string postUrl, string postData)
        {
            string strResult = "";
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(postUrl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                strResult = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                strResult = "";
            }
            return strResult;
        }

        public static string SendRequestByGet(string url)
        {
            string strResult = "";
            try
            {
                CookieContainer cookieContainer = new CookieContainer();
                HttpWebRequest wrq = (HttpWebRequest)HttpWebRequest.Create(url);
                wrq.CookieContainer = cookieContainer;
                wrq.Method = "GET";
                //wrq.ProtocolVersion = HttpVersion.Version10;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                HttpWebResponse wrp = (HttpWebResponse)wrq.GetResponse();
                System.IO.StreamReader sr = new StreamReader(wrp.GetResponseStream(), System.Text.Encoding.UTF8);
                strResult = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                strResult = "";
            }
            return strResult;
        }

    }
}