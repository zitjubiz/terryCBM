<%@ Page Language="C#" AutoEventWireup="True" CodeFile="ProductImport.aspx.cs"
    Inherits="Terry.ECommerce.Web.ProductImport" %>

<%@ Register Src="~/usercontrol/foot.ascx" TagName="foot" TagPrefix="foot" %>
<%@ Register Src="~/usercontrol/head.ascx" TagName="head" TagPrefix="head" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Import</title>
    <link href="../css/css2.css" rel="stylesheet" type="text/css" />
    <link href="../css/lightBlue.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <br />
    <br />
    <head:head ID="head1" runat="server" />
    <br />
    <div align="center">
        <table width="600" border="0" cellspacing="0" cellpadding="3" align="center">
            <tr>
                <td>
                    <div align="left">
                        <font color="#FFFFFF">
                            <asp:Literal ID="lit_page_header" runat="server" Text="Batch Product Import"></asp:Literal></font></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <asp:Literal ID="lit_cust_js" runat="server"></asp:Literal>
                        &nbsp;<img src="../images/excel.gif" alt="excel" />&nbsp;<a href="CSVTemplate.csv"
                            target="_blank">Download Excel Template File</a>
                    </div>
                    <div>
                        If your product info contain non-english character, please save excel in UTF-8 format</div>
                    <div>
                        <br />
                    </div>
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                    <asp:Button ID="Button1" runat="server" Text="Back" OnClientClick="window.location='Product.aspx';return false;" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
