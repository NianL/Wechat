<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="js-sdk.aspx.cs" Inherits="WeiXinPublic.js_sdk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1.0,maximum-scale=1.0,minimum-scale=1.0,user-scalable=no,target-densitydpi = medium-dpi">
    <meta name="format-detection" content="telephone=no">
    <meta name="apple-touch-fullscreen" content="YES">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <script src="JsConfig.ashx" type="text/javascript"></script>
    <script type="text/javascript">
        //分享功能使用
        var ShareInfo = {
            title: "分享页面测试",
            desc: "分享页面的简介测试",
            imgUrl: _CONFIG.WeixinDomain + "/2015-05-18_113332.png"
        };
    </script>
    <script src="js/sha1.js" type="text/javascript"></script>
    <script src="js/jweixin-1.0.0.js" type="text/javascript"></script>
    <script src="js/share.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.onload = function () {
            document.getElementById("txtTitle").value = ShareInfo.title;
            document.getElementById("txtDesc").value = ShareInfo.desc;
            document.getElementById("txtImgUrl").value = ShareInfo.imgUrl;
        };

        function UpdateShare() {
            var sInfo = {
                title: document.getElementById("txtTitle").value,
                desc: document.getElementById("txtDesc").value,
                imgUrl: document.getElementById("txtImgUrl").value
            };
            SetShare(sInfo);
            alert("修改成功!");
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    我要分享:<br />
    标题：<input id="txtTitle" type="text" /><br />
    简介：<input id="txtDesc" type="text" /><br />
    截图：<input id="txtImgUrl" type="text" /><br />
    <input type="button" value="修改分享内容" onclick="UpdateShare();" />
    </div>
    </form>
</body>
</html>
