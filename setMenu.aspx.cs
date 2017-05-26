using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiXinPublic.lib;
using System.IO;
using System.Text;
using System.Data;

namespace WeiXinPublic
{
    public partial class setMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JsConfig jsConfig = new JsConfig();
            string strAccess_token = jsConfig.WX_Access_Token();
            try
            {
                //获取菜单信息 json 格式
                FileStream fs1 = new FileStream(Server.MapPath("\\menu.txt"), FileMode.Open);
                StreamReader sr = new StreamReader(fs1, Encoding.UTF8);
                string menu = sr.ReadToEnd();
                sr.Close();
                fs1.Close();
                //修改菜单接口对应 url 参数 access_token
                string strUrl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + strAccess_token;
                menu = menu.Replace("$domain$", WeixinConfig.WeixinDomain);
                //发送一个Post请求过去 返回修改结果
                Response.Write(HttpSendRequest.SendRequestByPost(strUrl, menu));
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            
        }
    }
}