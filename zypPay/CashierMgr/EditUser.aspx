<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="CashierMgr.EditUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>编辑用户</title>
     <link href="styles/css.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery.validate.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table  class="table_1">
    <tr>
    <td>操作员姓名：</td>
    <td>
        <asp:TextBox ID="txtName" runat="server" class="required" Width="180" Enabled="false"></asp:TextBox></td>
    </tr>
      <tr>
    <td>是否管理员：</td>
    <td>
        <asp:DropDownList ID="ddlIsAdmin" runat="server" Width="180px">
        <asp:ListItem Text="否" Value="0"></asp:ListItem>
          <asp:ListItem Text="是" Value="1"></asp:ListItem>
        </asp:DropDownList>
       </td>
    </tr>
     <tr>
    <td>商户：</td>
    <td>
       <%-- <asp:TextBox ID="txtContact" runat="server"></asp:TextBox>--%>
        <asp:DropDownList ID="ddlClient" runat="server" Width ="180" class="required">
        </asp:DropDownList>
        </td>
    </tr>
    <%-- <tr>
    <td>地址：</td>
    <td>
        <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox></td>
    </tr>--%>
    </table>
       <div style="text-align:center"> 
    <asp:Button ID="btnCommit"  CssClass="btnoperator" runat="server" Text="提交" onclick="btnCommit_Click" /></div>
       
    </div>
    </div>
    </form>
    <script type="text/javascript">
        $().ready(function () {
            $("#form1").validate();
        });
    </script>
</body>
</html>
