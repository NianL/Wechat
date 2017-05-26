//var createNonceStr = function () {
//    return Math.random().toString(36).substr(2, 15);
//};

//var createTimestamp = function () {
//    return parseInt(new Date().getTime() / 1000) + '';
//};

//var raw = function (args) {
//    var keys = Object.keys(args);
//    keys = keys.sort()
//    var newArgs = {};
//    keys.forEach(function (key) {
//        newArgs[key.toLowerCase()] = args[key];
//    });

//    var string = '';
//    for (var k in newArgs) {
//        string += '&' + k + '=' + newArgs[k];
//    }
//    string = string.substr(1);
//    return string;
//};

///**
//* @synopsis 签名算法 
//*
//* @param jsapi_ticket 用于签名的 jsapi_ticket
//* @param url 用于签名的 url ，注意必须动态获取，不能 hardcode
//*
//* @returns
//*/
//var sign = function (jsapi_ticket, url) {
//    var ret = {
//        jsapi_ticket: _CONFIG.Jsapi_Ticket,
//        nonceStr: createNonceStr(),
//        timestamp: createTimestamp(),
//        url: url
//    };
//    var string = raw(ret);
//    console.log(string);
//    ret.signature = hex_sha1(string);
//    return ret;
//};

//var now_sign = sign(_CONFIG.Jsapi_Ticket, location.href);
wx.config({
    debug: false,
    appId: _CONFIG.WeixinAppID,
    timestamp: _CONFIG.Timestamp,
    nonceStr: _CONFIG.NonceStr,
    signature: _CONFIG.Signature,
    jsApiList: [
                'onMenuShareTimeline',
                'onMenuShareAppMessage',
                'onMenuShareQQ',
                'onMenuShareWeibo',
            ]
});


wx.ready(function () {
    SetShare(ShareInfo);//设置微信分享功能
});


function SetShare(si) {
    // 2. 分享接口
    // 2.1 监听“分享给朋友”，按钮点击、自定义分享内容及分享结果接口
    wx.onMenuShareAppMessage({
        title: si.title,
        desc: si.desc,
        link: location.href,
        imgUrl: si.imgUrl,
        trigger: function (res) {
            // 不要尝试在trigger中使用ajax异步请求修改本次分享的内容，因为客户端分享操作是一个同步操作，这时候使用ajax的回包会还没有返回
            //alert('用户点击发送给朋友');
        },
        success: function (res) {
            //alert('已分享');
        },
        cancel: function (res) {
            //alert('已取消');
        },
        fail: function (res) {
            //alert(JSON.stringify(res));
        }
    });
    //alert("分享给朋友");

    // 2.2 监听“分享到朋友圈”按钮点击、自定义分享内容及分享结果接口
    wx.onMenuShareTimeline({
        title: si.title,
        desc: si.desc,
        link: location.href,
        imgUrl: si.imgUrl,
        trigger: function (res) {
            // 不要尝试在trigger中使用ajax异步请求修改本次分享的内容，因为客户端分享操作是一个同步操作，这时候使用ajax的回包会还没有返回
            //alert('用户点击分享到朋友圈');
        },
        success: function (res) {
            //alert('已分享');
        },
        cancel: function (res) {
            //alert('已取消');
        },
        fail: function (res) {
            //alert(JSON.stringify(res));
        }
    });
    //alert("分享到朋友圈");

    // 2.3 监听“分享到QQ”按钮点击、自定义分享内容及分享结果接口
    wx.onMenuShareQQ({
        title: si.title,
        desc: si.desc,
        link: location.href,
        imgUrl: si.imgUrl,
        trigger: function (res) {
            //alert('用户点击分享到QQ');
        },
        complete: function (res) {
            //alert(JSON.stringify(res));
        },
        success: function (res) {
            //alert('已分享');
        },
        cancel: function (res) {
            //alert('已取消');
        },
        fail: function (res) {
            //alert(JSON.stringify(res));
        }
    });
    //alert("分享到QQ");

    // 2.4 监听“分享到微博”按钮点击、自定义分享内容及分享结果接口
    wx.onMenuShareWeibo({
        title: si.title,
        desc: si.desc,
        link: location.href,
        imgUrl: si.imgUrl,
        trigger: function (res) {
            //alert('用户点击分享到微博');
        },
        complete: function (res) {
            //alert(JSON.stringify(res));
        },
        success: function (res) {
            //alert('已分享');
        },
        cancel: function (res) {
            //alert('已取消');
        },
        fail: function (res) {
            //alert(JSON.stringify(res));
        }
    });
    //alert("分享到微博");
}
