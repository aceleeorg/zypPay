<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CashierMgr._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>收银台管理系统</title>
    <link href="styles/css.css" rel="stylesheet" type="text/css" />
   <link href="Scripts/jqueryEasyUI/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jqueryEasyUI/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jqueryEasyUI/jquery.easyui.min.js" type="text/javascript"></script>

     <script type="text/javascript">
         function showmenu2(obj, flag) {
             if (flag == 'f') {
                 if ($(obj).find("ul.second_tree").is(":hidden")) {
                     $(obj).addClass("current");
                     $(obj).find("ul.second_tree").show();
                 } else {
                     $(obj).removeClass("current");
                     $(obj).find("ul.second_tree").hide();
                 }

             } else if (flag == 's') {
                 $(".third_tree .current").removeClass("current");
                 if ($(obj).find("ul.third_tree").is(":hidden")) {
                     $(obj).find("a:eq(0)").addClass("current");
                     $(obj).find("ul.third_tree").show();
                 } else {
                     $(obj).find("ul.third_tree").hide();
                 }
             } else {
                 $(".third_tree .current").removeClass("current");
                 $(obj).find("a").addClass("current");
             }
         }

         $(function () {
             $(".first_tree>li").click(function (event) {
                 showmenu2(this, 'f');
                 event.stopPropagation();
             })

             $("ul.second_tree").hide();
             $(".second_tree>li").click(function (event) {
                 showmenu2(this, 's');
                 event.stopPropagation();
             })

             $("ul.third_tree").hide();
             $(".third_tree>li").click(function (event) {
                 showmenu2(this, 't');
                 event.stopPropagation();
             })

             $(".current").removeClass("current");
         })


         function openpage(url) {
             document.getElementById("frameMain").src = url;
         }

         function IFrameResize() {
             var iframe = document.getElementById("frameMain");
             var pagebody = document.getElementById("main-area");

             var height = document.documentElement.clientHeight -90;
             var width = document.documentElement.clientWidth - 200;
             iframe.width = width + "px";
             iframe.height = height + "px";

         }

         function logout() {
             var url = getRootPath();
             window.location.href = url + "/Login.aspx";
         }

         function getRootPath() {
             var strFullPath = window.document.location.href;
             var strPath = window.document.location.pathname;
             var pos = strFullPath.indexOf(strPath);
             var prePath = strFullPath.substring(0, pos);
             var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
             return (prePath + postPath);
         }


         var dialog;
         function changePwd() {

             dialog = $('#pwdDiv').dialog("open");
         }

       

         function closeDialog() {

             $('#pwdDiv').dialog('close');

             logout();
         }

    </script>
</head>
<body>
    <form id="form1" runat="server">
  <div id="top">
  <div id="header">
    <div class="content_header">
    	<div class="logo"><a href="#"><img src="images/logo.png" /></a></div>
        <div class="user">
        	<div class="wrapper_img">
            	<img src="images/icon_user.gif" />
            </div>
            <p>Hi, <a href="#" id="usrName" runat="server"></a></p>
              <p> <a href="#" id="aChangePwd" onclick="changePwd();"  runat="server">修改密码</a></p>
            <p> <a href="#" class="exit" onclick="logout();">退出</a></p>
        </div>
     </div>
   </div>

    <div id="pwdDiv" class="easyui-window" resizable="false" closed="true" modal="true" title="修改密码"
                style="width: 400px; height: 270px; overflow:hidden;">
                <iframe scrolling="no" id='newIframe' frameborder="0" src="changePwd.aspx"
                    style="width: 100%; height: 100%;"></iframe>
            </div>
</div>
 
 
 
 <div id="mainbody">
   <div class="side">
    <ul class="first_tree">
    	<li class="current"><a href="#">基本信息管理</a>
        	<ul class="second_tree">
            	<li>
                <a href="#" class="current" onclick="openpage('TradeInfoList.aspx');">交易查询</a>
                	<%--<ul class="third_tree">
                    	<li><a href="#" class="current">代理商查询</a></li>
                    	<li><a href="#">代理商维护</a></li>
                    </ul>--%>
                </li>
                <%--<li><a href="#" onclick="openpage('AgentList.aspx');">代理管理</a></li>--%>
            	<li><a href="#" onclick="openpage('UserList.aspx');">用户管理</a></li>
                <li><a href="#" onclick="openpage('MerchantList.aspx');">商户管理</a></li>
            </ul>
        </li>
    	
    	
    </ul>
   </div>

   <div class="main" id="main-area">
     <iframe id="frameMain" onload="IFrameResize()" src="commonInfo.aspx"
                frameborder="0"></iframe>
   </div>
 </div>
    </form>
</body>
</html>
