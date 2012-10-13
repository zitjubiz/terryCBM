<%@ Page Language="C#" AutoEventWireup="True" Inherits="Terry.CRM.Web.CRM.frmUserLoginHistory" 
MasterPageFile="~/MasterPage/Site.Master" Codebehind="frmUserLoginHistory.aspx.cs" %>

<%@ Register Src="~/UserControl/LoginHistory.ascx" TagName="LoginHistory" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
    <table id="wrp" cellpadding="0" cellspacing="0" align="center" style="height: 460px">
        <tr id="wrp_base">
            <td valign="top">
                <div id="wrapper">
                    <div id="main_content" class="content" style="width: 100%">
                        <div id="navbar">
                            <span id="currentModule">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:re, lblHome%>" />
                                &gt;
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblUserLoginHistory%>" />
                            </span>
                        </div>
                        <div id="Grid">
                           
                            <uc1:LoginHistory ID="LoginHistory1" runat="server" />
                            <div class="clearer">
                                <!--  -->
                            </div>
                        </div>
                    </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidSaveMsg" runat="server" Value="<%$ Resources:re, MsgSaveConfirm%>" />
    <asp:HiddenField ID="hidDelMsg" runat="server" Value="<%$ Resources:re, MsgDeleteConfirm%>" />
    <asp:HiddenField ID="hidBtnDel" runat="server" Value="" />
</asp:Content>
