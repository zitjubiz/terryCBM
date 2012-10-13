<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="foot.ascx.cs" Inherits="Terry.CRM.Web.UserControl.foot" %>

<table id="wrp" cellpadding="0" cellspacing="0" align="center">
        <tr id="wrp_footer">
            <td>
                <div id="bottom">
                    <div id="footer_links" class="footer_links">
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/CRM/frmVerUpdate.aspx">&nbsp;</asp:HyperLink>
                    <asp:Label ID="Literal1" CssClass="footer_links_support" runat="server" Text="<%$ Resources:re, lblFooter%>"></asp:Label>
                    <asp:HyperLink ID="lnkCompany" runat="server" Target="_blank" NavigateUrl="http://www.2simplework.com">简约工作,让你工作更简单</asp:HyperLink>
                    V 2.4 release at 2012/8/19</div>
                </div>
                <div id="global_blocker">
                    </div>
            </td>
        </tr>
</table>