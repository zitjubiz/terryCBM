
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Terry.CRM.Web._Default"
    MasterPageFile="~/MasterPage/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
    <table id="wrp" cellpadding="0" cellspacing="0" align="center" height="460">
        <tr id="wrp_base">
            <td valign="top">
                <div>
                    <img alt="公告" src="images/info.gif" border="0" width="20" height="20" />
                    <asp:Literal ID="lblSubject" runat="server" Text=""  Visible="false"/>
                    &nbsp;&nbsp;
                    <asp:Literal ID="lblContent" runat="server" Text="" /></div>
                <div id="wrapper">
                    <table align="center" width="80%" cellpadding="10" cellspacing="10" style="padding-top: 100px;">
                        <tr>
                            <td align="center">
                                <asp:HyperLink ID="lnk2" runat="server" NavigateUrl="~/CRM/frmCustomer.aspx"><img alt="" src="images/m_contact.gif"  border="0"/></asp:HyperLink>
                            </td>
                            <td align="center">
                                <asp:HyperLink ID="lnk3" runat="server" NavigateUrl="~/CRM/frmUserActions.aspx"><img alt="" src="images/m_opp.gif"  border="0"/></asp:HyperLink>
                            </td>
                            <td align="center">
                                <asp:HyperLink ID="lnk4" runat="server" NavigateUrl="http://task.2simplework.com" Target="_blank"><img alt="" src="images/m_system.gif"  border="0"/></asp:HyperLink>
                            </td>
                            <td align="center">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblCustomer%>" />
                            </td>
                            <td align="center">
                                <asp:Literal ID="Literal3" runat="server" 
                                    Text="<%$ Resources:re, lblAction%>" />
                            </td>
                            <td align="center">
                                <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:re, lblTaskManage%>" />
                            </td>
                            <td align="center">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <%--&nbsp; <a href="upload/callnum.zip" target="_blank">来电显示客户端下载</a>--%>
                            </td>
                            <td align="center">
                                &nbsp;
                            </td>
                            <td align="center">
                                <%--&nbsp;<a href="upload/task.zip" target="_blank">任务提醒客户端下载</a>--%>
                            </td>
                            <td align="center">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                &nbsp;
                            </td>
                            <td align="left" colspan="2">
                            </td>
                            <td align="center">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <asp:Label runat="server" ID="lblExpiry" Style="display: none;"></asp:Label>
    <script type="text/javascript">
        $(document).ready(function () {
            var tips = document.getElementById("ctl00_CPH1_lblExpiry").innerHTML;
            if (tips != "") {
                $.messager.lays(450, 250);
                $.messager.show("保修提示", tips,9000);
            }
        });
    </script>
</asp:Content>
