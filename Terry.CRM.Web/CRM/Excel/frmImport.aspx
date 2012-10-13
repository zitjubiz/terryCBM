<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmImport.aspx.cs" 
Inherits="Terry.CRM.Web.CRM.Excel.frmImport" MasterPageFile="~/MasterPage/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
    <asp:Button ID="btnImport" runat="server" onclick="btnImport_Click" 
        Text="Import" />
</asp:Content>