<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TradeInfoList.aspx.cs" Inherits="CashierMgr.TradeInfoList" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="styles/css.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/My97DatePicker/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
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
    <td>商户号：</td>
    <td>
        <asp:TextBox ID="txtMerchantID" runat="server"></asp:TextBox>
   
            </td>
    <td>商户名：</td>
     <td> <input id="txtName" name="txtName"   type="text"  
            runat="server"  /></td>
            <td>操作员编号：</td>
                <td> <input id="txtOprID" name="txtOprID"   type="text"  
            runat="server"  /></td>
    <td><asp:Button
                    ID="btnQuery" runat="server" Text="查询" CssClass="btnoperator" OnClick="btnQuery_Click" /></td>
    <td>&nbsp;</td>
    </tr>
     <tr>
    <td>开始时间：</td>
    <td><input id="txtStartTime"   onClick="WdatePicker()"  type="text"  
            runat="server" /></td>
    <td>结束时间：</td>
     <td><input id="txtEndTime"  onClick="WdatePicker()"
                 type="text"   runat="server" /></td>
    <td></td> <td></td>
    <td>&nbsp;</td>
    </tr>
    </table>
        &nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;
    </div>
    <div align="center" id="workspace" style="width: 90%;">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
      <asp:GridView ID="dataGrid" runat="server" CssClass ="table_1" 
                AutoGenerateColumns="false" >
        <Columns>
        <asp:BoundField HeaderText="流水号" DataField="LocalOrderNo" />
           <asp:BoundField HeaderText="商户号" DataField="ClientID" />
            <asp:BoundField HeaderText="交易日期" DataField="TradeTime" />
            <asp:BoundField HeaderText="交易类型" DataField="TradeType" />
            <asp:BoundField HeaderText="交易金额" DataField="Amount" />
                   <asp:BoundField HeaderText="操作员" DataField="OprID" />
        </Columns>
         <EmptyDataTemplate>
         <tr>
         <th>流水号</th>
         <th>商户号</th>
          <th>交易日期</th>
           <th>交易金额</th>
            <th>交易类型</th>
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
    </form>
</body>
</html>
