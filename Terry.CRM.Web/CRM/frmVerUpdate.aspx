<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmVerUpdate.aspx.cs" Inherits="Terry.CRM.Web.CRM.frmVerUpdate" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>CRM</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content="all,archive" name="robots" />
    <meta name="author" content="Terry Zhang" />
    <meta name="Copyright" content="2simplework.com" />
    <link href="../css/jquery.alerts.css" rel="stylesheet" type="text/css" media="screen" />
    <link href="../css/jquery.treeTable.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.autocomplete.css" rel="stylesheet" type="text/css" media="screen" />
    <link href="../css/lightBlue.css" rel="stylesheet" type="text/css" />
    <link href="../css/subModal.css" rel="stylesheet" type="text/css" />
     </head>
<body>
<form id="form1" runat="server">
<table id="wrp" cellpadding="0" cellspacing="0" align="center" style="height: 460px">
    <tr id="wrp_base">
        <td valign="top">
            <div id="wrapper">
                <div id="main_content" class="content" style="width: 100%">
                    <div id="navbar">
                        <span id="currentModule">
                            <asp:literal id="Literal1" runat="server" text="<%$ Resources:re, lblHome%>" />
                            &gt;
                            <asp:literal id="Literal2" runat="server" text="<%$ Resources:re, lblVerUpdate%>" />
                        </span>
                    </div>
                    <div id="toolbar" align="right">
                    </div>
                    <div id="Grid">
                        <asp:fileupload id="FileUpload1" runat="server" />
                        <asp:button id="btnUpload" runat="server" onclick="btnUpload_Click" text="上传" />
                    <br />
                        <asp:button id="btnChangeCustCode" runat="server" onclick="btnChangeCustCode_Click" text="修改客户编号" />
                    </div>
                </div>
                <div class="clearer">
                    <!--  -->
                </div>
            </div>
        </td>
    </tr>
</table>
<asp:hiddenfield id="hidSaveMsg" runat="server" value="<%$ Resources:re, MsgSaveConfirm%>" />
<asp:hiddenfield id="hidDelMsg" runat="server" value="<%$ Resources:re, MsgDeleteConfirm%>" />
<asp:hiddenfield id="hidBtnDel" runat="server" value="" />
</form>
</body>
</html>