<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewAgent.aspx.cs" Inherits="CashierMgr.NewAgent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>新增代理</title>
     <link href="styles/css.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery.validate.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>

    <table  class="table_1">
    <tr>
    <td>代理商名称：</td>
    <td>
        <asp:TextBox ID="txtName" runat="server" class="required"></asp:TextBox></td>
    </tr>
   
     <tr>
    <td>联系人：</td>
    <td>
        <asp:TextBox ID="txtContact" runat="server"></asp:TextBox></td>
    </tr>
      <tr>
    <td>联系电话：</td>
    <td>
        <asp:TextBox ID="txtTel" runat="server"></asp:TextBox></td>
    </tr>
     <tr>
    <td>地址：</td>
    <td>
        <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox></td>
    </tr>
      <tr>
    <td>代理类型：</td>
    <td>
        <asp:DropDownList ID="ddlAgentType" runat="server" class="required">
        </asp:DropDownList>
       </td>
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
