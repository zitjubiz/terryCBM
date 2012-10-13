<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmChangePwd.aspx.cs" Inherits="Terry.CRM.Web.CRM.ChangePwd"
    MasterPageFile="~/MasterPage/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">

    <script  type="text/javascript" >
function check()
{
    var msg= "";
    if(document.getElementById("ctl00_CPH1_txtPwd").value!=document.getElementById("ctl00_CPH1_txtPwd1").value)    
       return false
    else
        return true;

}

</script>
<p></p>
<div  class="sidebar_text">
    <table class="tableborder" align="center" width="350" cellspacing="1" cellpadding="0">
        <tr >
            <td colspan="2" height="30">
                <div align="center" class="sidebar_header">
                    <asp:Literal runat="server" Text="<%$ Resources:re,lblChangePassword %>"></asp:Literal></div>
            </td>
        </tr>
        <tr class="Row">
            <td width="150">
                <asp:Literal runat="server" Text="<%$ Resources:re,lblOldPassword %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtOld" runat="server" usage="notempty" Width="95%" tip="<%$ Resources:re, MsgPassword%>" MaxLength="30" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr class="Row">
            <td width="150">
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:re,lblNewPassword %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtPwd" runat="server" usage="notempty" Width="95%" tip="<%$ Resources:re, MsgPassword%>" MaxLength="30" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr class="Row">
            <td width="150" valign="top">
                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re,lblRetypePassword %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtPwd1" runat="server" usage="check()" Width="95%" tip="<%$ Resources:re,MsgRetypePassword %>" MaxLength="30" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr class="Row">
            <td width="150" valign="top">
                &nbsp;</td>
            <td>
                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:re, lblSave%>" Width="80px" check="true" 
					
                        onclick="btnSave_Click" ></asp:Button>

            </td>
        </tr>
        <tr >
            <td  valign="top" colspan="2">
                <div id="divErrorMessage"></div>
            </td>
        </tr>
    </table>

  </div>  
</asp:Content>
