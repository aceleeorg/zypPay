<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="CashierMgr.UserList" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>操作员列表</title>
    <link href="styles/css.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jqueryEasyUI/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jqueryEasyUI/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jqueryEasyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var dialog;
        function newOpr() {

            dialog = $('#newDiv').dialog("open");
        }

        function update(id) {
            //EditMerchant.aspx
            $('#updateIframe')[0].src = "EditUser.aspx?id=" + id;
            dialog = $('#updateDiv').dialog("open");
        }

        function closeDialog() {
            
            $('#newDiv').dialog('close');
            $('#updateDiv').dialog('close');
            $('#btnQuery').click();
        }


        $().ready(function () {
            //update(18000);
        });
 
    </script>
</head>
<body>
    <form id="form1" runat="server">
   <div align="left" style="margin: 10px">
            <span style="font-size: inherit; font-style: inherit; font-weight: bold; color: Green;">
                操作员管理</span></div>
        <hr />
    <div align="center"   style="width: 90%;">
    <table class="querytable" >
    <tr>
    <td>操作员姓名：</td>
    <td><input id="txtUsrName" name="txtUsrName"   type="text"  
            runat="server" /></td>
    <td>商户名：</td>
     <td><input id="txtMerchantName" name="txtMerchantName"
                 type="text"   runat="server" /></td>
                 <td>商户号：</td>
     <td><input id="txtMerchantID" name="txtMerchantID"
                 type="text"   runat="server" /></td>
    <td><asp:Button
                    ID="btnQuery" runat="server" Text="查询" CssClass="btnoperator" OnClick="btnQuery_Click" /></td>
    <td><input type="button" onclick ="newOpr();" value="新增" class="btnoperator"/></td>
    </tr>
    </table>
        &nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnAdd" Visible="false" CssClass="btnoperator" runat="server" Text="新增" OnClick="btnAdd_Click" />&nbsp;&nbsp;&nbsp;
    </div>
    <asp:Literal ID="litJs" runat="server"></asp:Literal>
    <div align="center" id="workspace" style="width: 90%;">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
      <asp:GridView ID="dataGrid" runat="server" CssClass ="table_1" 
                AutoGenerateColumns="false" >
        <Columns>
        <asp:BoundField HeaderText="操作员编号" DataField="OprID" />
           <asp:BoundField HeaderText="操作员姓名" DataField="OprName" />
              <asp:BoundField HeaderText="商户号" DataField="ClientID" />
                <asp:BoundField HeaderText="商户名" DataField="ClientName" />

                 <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Bind("OprID") %>'  OnCommand="btnUpdate_Command"  runat="server"><img src='images/edit.png' border='0'></asp:LinkButton>
                                   
                                   
                                     <asp:LinkButton ID="LinkButton2" CommandArgument='<%# Bind("OprID") %>' 
                                     OnClientClick="return confirm('请确认是否要删除该操作员');"
                                      OnCommand="btnDelete_Command"  runat="server"><img src='images/delete.png' border='0'>
                                      </asp:LinkButton>
                                   <%-- <asp:ImageButton ID="btnUpdate" runat="server"  ImageUrl="~/images/edit.png"  CommandName ="cmdUpdate"
                                        ToolTip="修改"   CommandArgument='<%# Bind("OprID") %>'
                                        OnCommand="btnUpdate_Command"  />--%>
                                 
                                   <%-- <asp:ImageButton ID="btnDelete" runat="server"   Text="删除"
                                    OnClientClick="return confirm('请确认是否要删除该操作员');"
                                        ImageUrl="~/images/delete.png"   CommandName ="cmdDelete"  CommandArgument='<%# Bind("OprID") %>'
                                      OnCommand="btnDelete_Command"   ToolTip="删除"  />--%>
                                 
                                </ItemTemplate>
                            
                            </asp:TemplateField>
        </Columns>
         <EmptyDataTemplate>
         <tr>
         <th>操作员编号</th>
         <th>操作员姓名</th>
         <th>商户号</th>
          <th>商户名</th>
           <th>操作</th>
         </tr>
         <tr>
         <td colspan="5">暂无数据</td>
         </tr>
         </EmptyDataTemplate>
        </asp:GridView>
       
        <div style="text-align:center;padding-top:20px;">
        <webdiyer:aspnetpager id="AspNetPager1" runat="server" firstpagetext="首页" lastpagetext="尾页"
            nextpagetext="下一页" prevpagetext="上一页"  custominfotextalign="Left" layouttype="Table"
            currentpagebuttonclass="cpb" cssclass="paginator"
            pagesize="10" showpageindexbox="Never" visible="true" enabletheming="true" alwaysshow="true"
            onpagechanged="AspNetPager1_PageChanged">
        </webdiyer:aspnetpager>
        </div>
     
        </ContentTemplate>
        </asp:UpdatePanel>
        
        </div>
     
         <div id="newDiv" class="easyui-window" resizable="false" closed="true" modal="true" title="新增"
                style="width: 400px; height: 300px; overflow:hidden;">
                <iframe scrolling="no" id='newIframe' frameborder="0" src="NewUser.aspx"
                    style="width: 100%; height: 100%;"></iframe>
            </div>
            <div id="updateDiv" class="easyui-window" resizable="false"  closed="true" modal="true" title="编辑" 
            style="width: 400px;
                height: 300px; overflow:hidden;">
                <iframe scrolling="auto" id='updateIframe' frameborder="0" src="" style="width: 100%;
                    height: 100%;"></iframe>
            </div>
 
    </form>
</body>
</html>
