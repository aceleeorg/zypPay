<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="changePwd.aspx.cs" Inherits="CashierMgr.changePwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改密码</title>
      <link href="styles/css.css" rel="stylesheet" type="text/css" />
       <script src="Scripts/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery.validate.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table  class="table_1">
    <tr>
    <td>原密码：</td>
    <td>
        <asp:TextBox ID="txtOldPwd" runat="server"  class="required"></asp:TextBox></td>
    </tr>
       <tr>
    <td>新密码：</td>
    <td>
        <asp:TextBox ID="txtPwd1" runat="server" class="required"></asp:TextBox></td>
    </tr>
      <tr>
    <td>确认密码：</td>
    <td>
        <asp:TextBox ID="txtPwd2" runat="server" class="required"></asp:TextBox></td>
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
