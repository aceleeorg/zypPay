<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewUser.aspx.cs" Inherits="CashierMgr.NewUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>新建用户</title>
    <link href="styles/css.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery.js" type="text/javascript"></script>
    <link href="Scripts/jqueryEasyUI/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jqueryEasyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jqueryEasyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.validate.js" type="text/javascript"></script>
    <script type="text/javascript">

        jQuery(function () {

            $("#txtClient").combobox({
                url: "BaseHandler.ashx?type=client",
                valueField: "ClientID",
                textField: "ClientName",
                panelHeight: "auto"
            });
        }); 

        var myloader = function (param, success, error) {
            var q = param.q || '';
            if (q.length <= 1) { return false }
            $.ajax({
                url: 'CashierService.asmx/GetClient',
                dataType: 'jsonp',
                data: {
                    featureClass: "P",
                    style: "full",
                    maxRows: 20,
                    name_startsWith: q
                },
                success: function (data) {
                    var o = data;
                    alert(o);
                    var items = $.map(data.geonames, function (item) {
                        return {
                            id: item.ClientID,
                            name: item.ClientName
                        };
                    });
                    success(items);
                },
                error: function () {
                    error.apply(this, arguments);
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="table_1">
            <tr>
                <td>
                    操作员姓名：
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" class="required" Width="180"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    是否管理员：
                </td>
                <td>
                    <asp:DropDownList ID="ddlIsAdmin" runat="server" Width="180px">
                        <asp:ListItem Text="否" Value="0"></asp:ListItem>
                        <asp:ListItem Text="是" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    商户：
                </td>
                <td>

                <input id="txtClient" runat="server" style="width:250px" />
                  
                    <%-- <asp:TextBox ID="txtContact" runat="server"></asp:TextBox>--%>
                   
                </td>
            </tr>
            <%-- <tr>
    <td>地址：</td>
    <td>
        <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox></td>
    </tr>--%>
        </table>
        <div style="text-align: center">
            <asp:Button ID="btnCommit" CssClass="btnoperator" runat="server" Text="提交" OnClick="btnCommit_Click" /></div>
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
