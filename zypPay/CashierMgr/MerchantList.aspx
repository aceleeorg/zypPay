<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MerchantList.aspx.cs" Inherits="CashierMgr.MerchantList" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>商户列表</title>
     <link href="styles/css.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jqueryEasyUI/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jqueryEasyUI/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jqueryEasyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var dialog;
        function newClient() {
       
            dialog= $('#newDiv').dialog("open");
        }

        function update(id) {
            //EditMerchant.aspx
            $('#updateIframe')[0].src = "EditMerchant.aspx?id=" + id;
            dialog= $('#updateDiv').dialog("open");
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
                商户管理</span></div>
        <hr />
    <div align="center"   style="width: 90%;">
    <table class="querytable" >
    <tr>
    <td> 商户名：</td>
    <td><input id="txtMerchantID" name="txtMerchantID"
                 type="text"   runat="server" /></td>
                 <td><asp:Button
                    ID="btnQuery" runat="server" Text="查询" CssClass="btnoperator" OnClick="btnQuery_Click" /></td>
   <td> <asp:Button ID="btnAdd" Visible="false" CssClass="btnoperator" runat="server" Text="新增"  OnClientClick="newClient();" />&nbsp;&nbsp;&nbsp;
<input type="button" id="btnNew" onclick ="newClient();" value="新增" runat="server" class="btnoperator"/>
</td>
    </tr>
    </table>
     
           </div>
    <div align="center" id="workspace" style="width: 90%;">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <asp:GridView ID="dataGrid" runat="server" CssClass ="table_1" 
            AutoGenerateColumns="false" onrowcommand="dataGrid_RowCommand">
        <Columns>
        <asp:BoundField HeaderText="商户编号" DataField="ClientID" />
           <asp:BoundField HeaderText="商户名称" DataField="ClientName" />
             <asp:BoundField HeaderText="激活码" DataField="ActiveCode" />
            <asp:BoundField HeaderText="联系人" DataField="Contact" />
                <asp:BoundField HeaderText="地址" DataField="Address" />

                 <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                               <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Bind("ClientID") %>'  
                               CommandName ="cmdUpdate"  runat="server">
                               <img src='images/edit.png' border='0'></asp:LinkButton>
                                   
                                   
                                     <asp:LinkButton ID="LinkButton2" CommandArgument='<%# Bind("ClientID") %>' 
                                     OnClientClick="return confirm('请确认是否要删除该商户');"
                                     CommandName ="cmdDelete"  runat="server"><img src='images/delete.png' border='0'>
                                      </asp:LinkButton>
                                <%--    <asp:ImageButton ID="btnUpdate" runat="server"  ImageUrl="~/images/edit.png"  CommandName ="cmdUpdate"
                                        ToolTip="修改"   CommandArgument='<%# Bind("ClientID") %>' />
                                 
                                    <asp:ImageButton ID="btnDelete" runat="server"   Text="删除" OnClientClick="return confirm('请确认是否要删除该商户');"
                                        ImageUrl="~/images/delete.png"   CommandName ="cmdDelete"  CommandArgument='<%# Bind("ClientID") %>' 
                                        ToolTip="删除" />--%>
                                 
                                </ItemTemplate>
                            
                            </asp:TemplateField>
        </Columns>
         <EmptyDataTemplate>
         <tr>
         <th>商户编号</th>
         <th>商户名称</th>
           <th>激活码</th>
         <th>联系人</th>
        
          <th>地址</th>

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
        </ContentTemplate>
        </asp:UpdatePanel>
        
        </div>
     
         <div id="newDiv" class="easyui-window" resizable="false" closed="true" modal="true" title="新增"
                style="width: 500px; height: 300px; overflow:hidden;">
                <iframe scrolling="no" id='newIframe' frameborder="0" src="NewMerchant.aspx"
                    style="width: 100%; height: 100%;"></iframe>
            </div>
            <div id="updateDiv" class="easyui-window" resizable="false"  closed="true" modal="true" title="编辑" 
            style="width: 500px;
                height: 300px; overflow:hidden;">
                <iframe scrolling="auto" id='updateIframe' frameborder="0" src="" style="width: 100%;
                    height: 100%;"></iframe>
            </div>
  
    </form>
</body>
</html>
