<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmTaskEdit.aspx.cs" Inherits="Terry.CRM.Web.CRM.GTD.frmTaskEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../css/lightBlue.css" rel="stylesheet" type="text/css" />
    <link href="../../css/jquery.alerts.css" rel="stylesheet" type="text/css" media="screen" />

    <script type="text/javascript" src="../../js/checkform.js"></script>

    <script type="text/javascript" src="../../js/jquery-1.3.2.min.js"></script>

    <script type="text/javascript" src="../../js/jquery.alerts.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 350px">
        <table width="350px">
            <tr>
                <td width="120">
                    <asp:Label ID="lblTaskTime" runat="server" Text="<%$ Resources:re, lblTaskDate %>"></asp:Label>
                    :
                </td>
                <td width="180">
                    <asp:DropDownList ID="ddlTaskTime" runat="server" Width="100%">
                        <asp:ListItem>8:00</asp:ListItem>
                        <asp:ListItem>9:00</asp:ListItem>
                        <asp:ListItem>10:00</asp:ListItem>
                        <asp:ListItem>11:00</asp:ListItem>
                        <asp:ListItem>12:00</asp:ListItem>
                        <asp:ListItem>13:00</asp:ListItem>
                        <asp:ListItem>14:00</asp:ListItem>
                        <asp:ListItem>15:00</asp:ListItem>
                        <asp:ListItem>16:00</asp:ListItem>
                        <asp:ListItem>17:00</asp:ListItem>
                        <asp:ListItem>18:00</asp:ListItem>
                        <asp:ListItem>19:00</asp:ListItem>
                        <asp:ListItem>20:00</asp:ListItem>
                        <asp:ListItem>21:00</asp:ListItem>
                        <asp:ListItem>22:00</asp:ListItem>
                        <asp:ListItem>23:00</asp:ListItem>
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="120">
                    <asp:Label ID="lblTask" runat="server" Text="<%$ Resources:re, lblTask %>"></asp:Label>
                    :
                </td>
                <td width="180">
                    <asp:TextBox ID="txtTask" runat="server" tip="任务不能为空" usage="notempty" Width="100%"
                        MaxLength="20">					
                    </asp:TextBox>
                </td>
            </tr>
             <tr>
                <td width="120">
                    <asp:Label ID="lblStatus1" runat="server" Text="<%$ Resources:re, lblStatus %>"></asp:Label>
                    :
                </td>
                <td width="180">
                <asp:Label ID="lblStatus" runat="server" ></asp:Label>
                </td>
            </tr>
        </table>
        <div id="divErrorMessage">
        </div>
        <div class="clearer">
            <!--  -->
        </div>
        <p></p>
        <div class="button">
            <asp:Button ID="btnNew" runat="server" Text="<%$ Resources:re, lblNew%>"  Width="50px"
                OnClick="btnNew_Click" />
            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:re, lblSave%>" check="true" Width="50px"
                OnClick="btnSave_Click" />
            <asp:Button ID="btnDel"  runat="server" Text="<%$ Resources:re, lblDelete%>" Width="50px"
                OnClick="btnDel_Click" />
            <asp:Button ID="btnApprove" runat="server" Text="<%$ Resources:re, lblApprove%>"  Width="50px"
                OnClick="btnApprove_Click" />
            <asp:Button ID="btnReject" runat="server" Text="<%$ Resources:re, lblReject%>"  Width="50px"
                OnClick="btnReject_Click" />
            <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:re, lblBack%>" OnClick="btnBack_Click"  Width="50px"/>
            <asp:HiddenField ID="hidID" runat="server" Value="" />
            <asp:HiddenField ID="hidDay" runat="server" Value="" />
            <asp:HiddenField ID="hidPerson" runat="server" Value="" />
            <asp:HiddenField ID="hidSaveMsg" runat="server" Value="<%$ Resources:re, MsgSaveConfirm%>" />
            <asp:HiddenField ID="hidDelMsg" runat="server" Value="<%$ Resources:re, MsgDeleteConfirm%>" />
            <asp:HiddenField ID="hidBtnDel" runat="server" Value="" />
            <br />
            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </div>
    </form>
</body>
</html>
