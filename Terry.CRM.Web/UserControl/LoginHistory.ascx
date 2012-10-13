<%@ Control Language="C#" AutoEventWireup="true" Inherits="Terry.CRM.Web.UserControl.LoginHistory" Codebehind="LoginHistory.ascx.cs" %>
<asp:GridView ID="gvLoginHistory" runat="server" AllowPaging="True" 
    AutoGenerateColumns="False" CellPadding="4" DataSourceID="ObjectDataSource1" 
    ForeColor="#333333" GridLines="None" Width="98%" >
    <RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" />
    <Columns>
        <asp:BoundField DataField="UserName" HeaderText="用户名" 
            SortExpression="UserName" />
        <asp:BoundField DataField="LoginIP" HeaderText="IP地址" 
            SortExpression="LoginIP" />
        <asp:BoundField DataField="LoginAt" HeaderText="登录时间" 
            SortExpression="LoginAt" />
        <asp:BoundField DataField="ClientBrowser" HeaderText="浏览器" 
            SortExpression="ClientBrowser" />
        <asp:BoundField DataField="ClientOS" HeaderText="操作系统" 
            SortExpression="ClientOS" />
        <asp:BoundField DataField="Status" HeaderText="状态码" 
            SortExpression="Status" />
    </Columns>
    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
    <EditRowStyle BackColor="#2461BF" />
    <AlternatingRowStyle BackColor="White" />
</asp:GridView>
<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
    SelectMethod="GetLoginHistory" TypeName="Terry.CRM.Service.UserService" 
    OldValuesParameterFormatString="original_{0}">
    <SelectParameters>
        <asp:Parameter DefaultValue="2012-1-1" Name="Begin" Type="DateTime" />
        <asp:Parameter DefaultValue="2013-1-1" Name="End" Type="DateTime" />
        <asp:Parameter DefaultValue="" Name="UserID" Type="Object" />
    </SelectParameters>
</asp:ObjectDataSource>

