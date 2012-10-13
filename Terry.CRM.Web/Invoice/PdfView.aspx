<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PdfView.aspx.cs" Inherits="Terry.CRM.Web.PdfView" %>

<%@ Register Src="UCViewSwf.ascx" TagName="UCViewSwf" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%">
        <tr>
            <td align="center" style="width: 500px">
                <a href="<%=GetPdfUrl() %>" target="_blank">下载PDF</a><br />
            </td>
        </tr>
        <tr>
            <td style="height: 310px">
                <div>
                    <uc1:UCViewSwf ID="UCViewSwf1" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td align="center" style="width: 500px">
                <a href="<%=GetPdfUrl() %>" target="_blank">下载PDF</a>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
