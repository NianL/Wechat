using System;
using System.Collections.Generic;
using System.Web;
using System.Data;

namespace WeiXinPublic.lib
{
    public class Onload : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            //访问页面时候页面设置 session 获取 openid
            if (Session["openid"] == null)
            {
                string strCode = Request.QueryString["code"];
                if (strCode == null || strCode == "")
                {
                    string strUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=123#wechat_redirect";
                    string httpUrl = Server.UrlEncode(Request.Url.AbsoluteUri.ToString());
                    strUrl = string.Format(strUrl, WeixinConfig.WeixinAppID, httpUrl);

                    //RecordLog.WriteLog("log", "获取OpenId:" + strUrl);
                    Response.Redirect(strUrl);
                }
                else
                {
                    string strUser = GetAccess_token(strCode);
                    //RecordLog.WriteLog("log", "获取userInfo:" + strUser);
                    DataTable dt = JsonMethod.JsonToDataTable(strUser);
                    Session["wx_openid"] = dt.Rows[0]["openid"].ToString();
                    Session["wx_userinfo"] = GetSnsapi_userinfo(dt.Rows[0]["access_token"].ToString(), dt.Rows[0]["openid"].ToString());
                    //Response.Write(dt.Rows[0]["openid"]);
                    //Response.End();
                }
            }
            base.OnLoad(e);
        }

        private string GetAccess_token(string code)
        {
            //code 动态验证 用于获取用户信息
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?"
                    + "appid=" + WeixinConfig.WeixinAppID + "&secret=" + WeixinConfig.WeixinAppSecret + "&code=" + code + "&grant_type=authorization_code";
            return HttpSendRequest.SendRequestByGet(url);
        }

        private string GetSnsapi_userinfo(string access_token,string openid)
        {
            //如果网页授权作用域为snsapi_userinfo，则此时开发者可以通过access_token和openid拉取用户信息了
            string url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN";
            return HttpSendRequest.SendRequestByGet(url);
        }
    }
}