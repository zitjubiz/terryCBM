<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuddyList.ascx.cs" Inherits="Terry.CRM.Web.UserControl.BuddyList" %>
<asp:Repeater ID="rptBuddy" runat="server" onitemcommand="rptBuddy_ItemCommand" 
    onitemdatabound="rptBuddy_ItemDataBound">
<ItemTemplate>
<span nowrap>
<img src="../images/superior.png" border="0" alt="" />
<asp:HiddenField ID="hidCustID" runat="server" Value='<%# Eval("CustID")%>' />
<asp:HyperLink ID="lnkName" runat="server" NavigateUrl='<%# Eval("CustID", "~/CRM/frmCustomerEdit.aspx?ID={0}") %>' Text='<%# Eval("CustName")%>'></asp:HyperLink>
<asp:ImageButton ID="btnDel" runat="server" ImageUrl="../images/delete.png" CommandName="Delete" />
&nbsp;&nbsp;</span>
</ItemTemplate>

</asp:Repeater>
<br />
<asp:Label ID="lblAdd" runat="server" Text="选择一个客户添加为好友"></asp:Label>
<asp:DropDownList ID="ddlCust" runat="server"></asp:DropDownList>
<asp:Button ID="btnAdd" runat="server" onclick="btnAdd_Click" Text="添加好友" />