<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditMerchant.aspx.cs" Inherits="CashierMgr.EditMerchant" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>编辑商户</title>
     <link href="styles/css.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery.validate.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <table  class="table_1">
    <tr>
    <td>商户名：</td>
    <td>
        <asp:TextBox ID="txtName" runat="server"  Enabled="false"></asp:TextBox></td>
    </tr>
       <tr>
    <td>PID：</td>
    <td>
        <asp:TextBox ID="txtPID" runat="server" class="required"></asp:TextBox></td>
    </tr>
      <tr>
    <td>KEY：</td>
    <td>
        <asp:TextBox ID="txtKey" runat="server" class="required"></asp:TextBox></td>
    </tr>
     <tr>
    <td>支付宝账号：</td>
    <td>
        <asp:TextBox ID="txtAlipayAccount" runat="server" class="required"></asp:TextBox></td>
    </tr>
     <tr>
    <td>联系人：</td>
    <td>
        <asp:TextBox ID="txtContact" runat="server"></asp:TextBox></td>
    </tr>
     <tr>
    <td>地址：</td>
    <td>
        <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox></td>
    </tr>
    </table>
    <div style="text-align:center"> 
    <asp:Button ID="btnCommit"  CssClass="btnoperator" runat="server" Text="提交" onclick="btnCommit_Click" /></div>
       
    </div>
    </form>
    <script type="text/javascript">
        $().ready(function () {
            $("#form1").validate();
        });
    </script>
</body>
</html>
