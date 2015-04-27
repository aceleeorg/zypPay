<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="commonInfo.aspx.cs" Inherits="CashierMgr.commonInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div>
    欢迎进入收银台管理系统
    </div>
     <div>
    当前商户共：<asp:Label ID="lblClientCount" runat="server" Text="0"></asp:Label>家
    </div>
    <div>
    当前操作员共：<asp:Label ID="lblOprCount" runat="server" Text="0"></asp:Label>人
    </div>
    </div>
    </form>
</body>
</html>
