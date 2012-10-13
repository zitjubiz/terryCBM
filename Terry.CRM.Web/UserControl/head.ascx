<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="head.ascx.cs" Inherits="Terry.CRM.Web.UserControl.head" %>
<table id="wrp" cellpadding="0" cellspacing="0" align="center">
    <tr id="wrp_base">
        <td valign="top">
            <div id="wrapper">
                <div id="header">
                    <div class="logo">
                        <asp:Image ID="ImgLogo" runat="server" ImageUrl="~/images/logo.png" Width="470px" /></div>
                    <div class="panel">
                        <div style="background-image: none;">
                            <ul style="margin-right: 10px; padding-right: 0;">
                                <li id="user_information"><a class="user_name" href="#">
                                    <asp:Label ID="lblLoginUserID" runat="server"></asp:Label></a> </li>
                                <li>
                                    <asp:HyperLink ID="lnkChangePwd" runat="server" Text="<%$ Resources:re, lblChangePassword%>"
                                        NavigateUrl="~/CRM/frmChangePwd.aspx"></asp:HyperLink>
                                    &nbsp; &nbsp;|&nbsp;&nbsp;<a href="#"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:re, lblHelp%>" /></a>
                                    &nbsp; &nbsp;|&nbsp;&nbsp;
                                    <asp:HyperLink ID="lnkLogout" runat="server" Text="<%$ Resources:re, lblLogout%>"
                                        NavigateUrl="~/Login.aspx"></asp:HyperLink>
                                </li>
                            </ul>
                            <a href="#" style="float: left">
                                <asp:Image ID="user_avatar_large" runat="server" ImageUrl="~/images/user_avatar_large.png" />
                            </a>
                        </div>
                    </div>
                </div>
                <div class="menu">
                    <ul id="ulHome">
                        <li>
                            <h2>
                                <asp:HyperLink ID="lnkHome" runat="server" Text="<%$ Resources:re, lblHome%>" NavigateUrl="~/default.aspx"></asp:HyperLink>
                            </h2>
                        </li>
                    </ul>
                    <ul id="ulInvoice" runat="server">
                        <li>
                            <h2>
                                <asp:HyperLink ID="lnkInvoice" runat="server" Text="<%$ Resources:re, lblInvoice%>"
                                    NavigateUrl="~/Invoice/frmTicket.aspx"></asp:HyperLink>
                            </h2>
                            <ul>
                                <li>
                                    <asp:HyperLink ID="lnkTicket" runat="server" Text="<%$ Resources:re, lblTicket%>"
                                        NavigateUrl="~/Invoice/frmTicket.aspx"></asp:HyperLink>
                                </li>
                                <li>
                                    <asp:HyperLink ID="lnkVisa" runat="server" Text="<%$ Resources:re, lblVisa%>" NavigateUrl="~/Invoice/frmVisa.aspx"></asp:HyperLink>
                                </li>
                                <%--<li>
                                    <asp:HyperLink ID="lnkDaily" runat="server" Text="每日出票/对账" NavigateUrl="~/Invoice/frmDailyIssue.aspx"></asp:HyperLink>
                                </li>--%>
                                <li>
                                    <asp:HyperLink ID="lnkBillReport" runat="server" Text="<%$ Resources:re, lblReport%>" NavigateUrl="~/Invoice/frmBillReport.aspx"></asp:HyperLink>
                                </li>
                            </ul>
                        </li>
                    </ul>
                    <ul id="ulCustomer" runat="server">
                        <li>
                            <h2>
                                <asp:HyperLink ID="lnkCustomer" runat="server" Text="<%$ Resources:re, lblCustomer%>"
                                    NavigateUrl="~/CRM/frmCustomer.aspx"></asp:HyperLink>
                            </h2>
                        </li>
                    </ul>
                    <ul id="ulAction" runat="server">
                        <li>
                            <h2>
                                <asp:HyperLink ID="lnkAction" runat="server" Text="<%$ Resources:re, lblAction%>"
                                    NavigateUrl="~/CRM/frmUserActions.aspx"></asp:HyperLink>
                            </h2>
                        </li>
                    </ul>
                    <ul id="ulEmail" runat="server">
                        <li>
                            <h2>
                                <asp:HyperLink ID="lnkEmail" runat="server" Text="<%$ Resources:re, lblEmailMarketing%>"
                                    NavigateUrl="~/CRM/frmSendEmail.aspx"></asp:HyperLink>
                            </h2>
                        </li>
                    </ul>
                    <ul id="ulSchedule" runat="server">
                        <li>
                            <h2>
                                <asp:HyperLink ID="lnkSchedule" runat="server" Text="<%$ Resources:re, lblSchedule%>"
                                    NavigateUrl="~/CRM/GTD/frmSchedule.aspx"></asp:HyperLink></h2>
                            
                        </li>
                    </ul>
                    <ul id="ulMantis" runat="server">
                        <li>
                            <h2>
                                <asp:HyperLink ID="lnkMantis" runat="server" Text="<%$ Resources:re, lblTaskManage%>"
                                    NavigateUrl="http://task.2simplework.com" Target="_blank"></asp:HyperLink></h2>
                        </li>
                    </ul>
                    <ul id="ulReport" runat="server">
                        <li>
                            <h2>
                                <asp:HyperLink ID="lnkReport" runat="server" Text="<%$ Resources:re, lblReport%>"
                                    NavigateUrl="~/CRM/frmReport.aspx"></asp:HyperLink></h2>
                        </li>
                    </ul>
                    <ul id="ulCustBrief" runat="server">
                        <li>
                            <h2>
                                <asp:HyperLink ID="lnkCustBrief" runat="server" Text="<%$ Resources:re, lblCustBrief%>"
                                    NavigateUrl="~/CRM_Chem/frmCustomerBrief.aspx"></asp:HyperLink></h2>
                        </li>
                    </ul>
                     <ul id="ulProduct" runat="server" visible="false">
                        <li>
                            <h2>
                                <asp:HyperLink ID="lnkProduct" runat="server" Text="<%$ Resources:re, lblProduct%>"
                                    NavigateUrl="~/CRM/frmProduct.aspx"></asp:HyperLink></h2>
                        </li>
                    </ul>
                    <ul id="ulSys" runat="server">
                        <li>
                            <h2>
                                <a href="#">
                                    <asp:Literal ID="lblSystem" runat="server" Text="<%$ Resources:re, lblCtrlPanel%>"></asp:Literal>
                                </a>
                            </h2>
                            <ul>
                                <li>
                                    <asp:HyperLink ID="hlUser" runat="server" Text="<%$ Resources:re, lblUser%>" NavigateUrl="~/CRM/frmUser.aspx"></asp:HyperLink>
                                </li>
                                <li>
                                    <asp:HyperLink ID="hlDept" runat="server" Text="<%$ Resources:re, lblDepartment%>"
                                        NavigateUrl="~/CRM/frmDepartment.aspx"></asp:HyperLink>
                                </li>
                                <li>
                                    <asp:HyperLink ID="hlRole" runat="server" Text="<%$ Resources:re, lblRole%>" NavigateUrl="~/CRM/frmRole.aspx"></asp:HyperLink>
                                </li>
                                <li>
                                    <asp:HyperLink ID="hlProd" runat="server" Text="<%$ Resources:re, lblProduct%>" NavigateUrl="~/CRM/frmProduct.aspx"></asp:HyperLink>
                                </li>
                                <li>
                                    <asp:HyperLink ID="hlCategory" runat="server" Text="<%$ Resources:re, lblCategory%>"
                                        NavigateUrl="~/CRM/frmCategory.aspx"></asp:HyperLink>
                                </li>
                                <li>
                                    <asp:HyperLink ID="hlUserTransfer" runat="server" Text="<%$ Resources:re, lblUserCustTransfer%>"
                                        NavigateUrl="~/CRM/frmUserCustTransfer.aspx"></asp:HyperLink><ul>
                                        </ul>
                                </li>
                                <li>
                                    <asp:HyperLink ID="hlLoginHistory" runat="server" Text="<%$ Resources:re, lblUserLoginHistory%>"
                                        NavigateUrl="~/CRM/frmUserLoginHistory.aspx"></asp:HyperLink>
                                </li>
                                <li>
                                    <asp:HyperLink ID="hlProvince" runat="server" Text="<%$ Resources:re, lblCustProvince%>"
                                        NavigateUrl="~/CRM/BaseInfo/frmProvince.aspx"></asp:HyperLink>
                                </li>
                                <li>
                                    <asp:HyperLink ID="hlBaseInfo" runat="server" Text="<%$ Resources:re, lblBaseInfo%>"
                                        NavigateUrl="~/CRM/BaseInfo/frmBaseInfo.aspx"></asp:HyperLink>
                                </li>
                            </ul>
                        </li>
                    </ul>
                    <div class="header_search_box" style="display: none">
                        <a id="search_input_clear" onmousedown="return false;" onclick="$j('#search_input_new').val('Search files').blur();box.search_real_close();return false"
                            style="display: none;" href="#"></a>
                        <input type="text" value="Search files" id="search_input_new" style="width: 103px" />
                    </div>
                </div>
                <div class="clearer">
                    <!--  -->
                </div>
            </div>
        </td>
    </tr>
</table>
