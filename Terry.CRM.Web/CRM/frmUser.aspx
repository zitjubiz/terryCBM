<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="frmUser.aspx.cs" Inherits="Terry.CRM.Web.CRM.frmUser"
    MasterPageFile="~/MasterPage/Site.Master" %>

<%@ Register Assembly="PagingGridView" Namespace="Terry.Web.Control" TagPrefix="cc1" %>
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
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblActiveUsers%>" />
                            </span>
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div class="sidebar_header_search">
                            <div class="sidebar_search_box">
                                <asp:TextBox ID="txtKeyword" runat="server" Style="width: 165px"></asp:TextBox>
                            </div>
                            <div class="sidebar_search_btn">
                                <table>
                                    <tr>
                                        <td style="padding-left: 10px">
                                            <asp:DropDownList ID="ddlSearch" runat="server" Width="150px">
                                                <asp:ListItem Value="UserName" Text="<%$ Resources:re, lblUserName%>"></asp:ListItem>
                                                <asp:ListItem Value="UserFullName" Text="<%$ Resources:re, lblUserFullName%>"></asp:ListItem>
                                                <asp:ListItem Value="Mobile" Text="<%$ Resources:re, lblMobile%>"></asp:ListItem>
                                                <asp:ListItem Value="DepName" Text="<%$ Resources:re, lblDepartment%>"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:re, lblSearch%>" OnClick="btnSearch_Click" />
                                            <asp:Button ID="btnNew" runat="server" Text="<%$ Resources:re, lblNew%>" OnClick="btnNew_Click" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div id="Grid">
                            <cc1:PagingGridView ID="gvData" runat="server" AutoGenerateColumns="False" CssClass="tableBorder"
                                HeaderStyle-CssClass="Head" RowStyle-CssClass="Row" DataKeyNames="UserID" FooterStyle-CssClass="Foot"
                                Width="100%" ShowFooter="True" VirtualItemCount="-1" AllowPaging="True" OnPageIndexChanging="gvData_PageIndexChanging"
                                OnRowDataBound="gvData_RowDataBound" OnRowDeleting="gvData_RowDeleting" OrderBy="DepName"
                                AllowSorting="True" OnSorting="gvData_Sorting">
                                <Columns>
                                    <%-- 
                                    <asp:BoundField DataField="UserID" SortExpression="UserID" HeaderText="<%$ Resources:re, lblUserID%>" />
                                    --%>
                                    <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="<%$ Resources:re, lblUserName%>" />
                                    <asp:BoundField DataField="UserFullName" SortExpression="UserFullName" HeaderText="<%$ Resources:re, lblUserFullName%>" />
                                    <asp:BoundField DataField="Mobile" SortExpression="Mobile" HeaderText="<%$ Resources:re, lblMobile%>" />
                                    <asp:BoundField DataField="BossName" SortExpression="BossName" HeaderText="<%$ Resources:re, lblBossID%>" />
                                    <asp:BoundField DataField="DepName" SortExpression="DepName" HeaderText="<%$ Resources:re, lblDepName%>" />
                                    <asp:BoundField DataField="Role" SortExpression="Role" HeaderText="<%$ Resources:re, lblRole%>" />
                                    <asp:BoundField DataField="CDate" SortExpression="CDate" HeaderText="<%$ Resources:re, lblCDate%>"
                                        DataFormatString="<%$ Resources:re, DateFormatString%>" />
                                    <asp:BoundField DataField="ModifyDate" SortExpression="ModifyDate" HeaderText="<%$ Resources:re, lblModifyDate%>"
                                        DataFormatString="<%$ Resources:re, DateFormatString%>" />
                                    <asp:TemplateField ShowHeader="False" ItemStyle-HorizontalAlign="Center" Visible="false">
                                        <ItemTemplate>
                                            <asp:Button ID="lnkDel" runat="server" CommandName="Delete" CssClass="linkButton"
                                                Text="<%$ Resources:re, lblDelete%>"></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </cc1:PagingGridView>
                        </div>
                    </div>
                    <asp:HyperLink ID="lnkActiveUser" runat="server" NavigateUrl="frmUser.aspx" Text="<%$ Resources:re, lblActiveUsers%>"></asp:HyperLink>
                    &nbsp; &nbsp;
                    <asp:HyperLink ID="lnkInactiveUser" runat="server" NavigateUrl="frmUser.aspx?status=I" Text="<%$ Resources:re, lblInactiveUsers%>"></asp:HyperLink>
                    <div class="clearer">
                        <!--  -->
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidSaveMsg" runat="server" Value="<%$ Resources:re, MsgSaveConfirm%>" />
    <asp:HiddenField ID="hidDelMsg" runat="server" Value="<%$ Resources:re, MsgDeleteConfirm%>" />
    <asp:HiddenField ID="hidBtnDel" runat="server" Value="" />
</asp:Content>
