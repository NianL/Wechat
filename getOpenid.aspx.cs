using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiXinPublic.lib;
using System.Data;

namespace WeiXinPublic
{
    public partial class getOpenid : WeiXinPublic.lib.Onload
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = Session["wx_openid"].ToString();
            Label2.Text = Session["wx_userinfo"].ToString();
        }
    }
}