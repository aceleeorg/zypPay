<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CashierMgr.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>收银台管理系统登录</title>
    <link href="styles/css.css" rel="stylesheet" type="text/css" />
    <style type="text/css" >
    .btn_login { font-family:"Arial", "Tahoma", "微软雅黑", "雅黑"; border:0; vertical-align:middle;  line-height:18px; font-size:18px }
    </style>
</head>


<body style="background:url(images/bg_login.jpg) no-repeat center bottom">
  <form id="form1" runat="server"  class="wrapper_login">
<%--<div>--%>
	<div class="logo_login"><img src="images/logo.png" /></div>
  <div class="content_login">
    <p class="user"><asp:TextBox ID="txtOprID" placeholder="操作员编号" runat="server" TabIndex="1"></asp:TextBox></p>
     <p class="client"><asp:TextBox ID="txtClientID" placeholder="商户号" runat="server" TabIndex="1"></asp:TextBox></p>
  <p class="password">   <asp:TextBox ID="txtPwd" placeholder="密码" runat="server" TextMode="Password" TabIndex="2"></asp:TextBox></p>
       <asp:Button ID="btnLogin" onmouseover="this.style.backgroundPosition='left -40px'" onmouseout="this.style.backgroundPosition='left top'" runat="server" Text="登录" CssClass="btn_login" onclick="btnLogin_Click" TabIndex="3" />
    </div>
<%--</div>--%>
 </form>
</body>
</html>
