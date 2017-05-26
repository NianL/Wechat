<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="getOpenid.aspx.cs" Inherits="WeiXinPublic.getOpenid" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1.0,maximum-scale=1.0,minimum-scale=1.0,user-scalable=no,target-densitydpi = medium-dpi">
    <meta name="format-detection" content="telephone=no">
    <meta name="apple-touch-fullscreen" content="YES">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        openid:<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br />
        userinfo:<asp:Label ID="Label2" runat="server" Text="Label"></asp:Label><br />
    </div>
    <div id="UserInfo"></div>
    </form>
</body>

<script type="text/javascript">
    var _UserInfo = <%=Session["wx_userinfo"].ToString() %>;
    document.getElementById("UserInfo").innerHTML = _UserInfo.nickname + "<br />"
        + "<img style='width:200px;height:200px;' src='" + _UserInfo.headimgurl + "' />";
</script>
</html>
