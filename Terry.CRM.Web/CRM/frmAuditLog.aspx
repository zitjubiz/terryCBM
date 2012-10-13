<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="frmAuditLog.aspx.cs" Inherits="Terry.CRM.Web.CRM.frmAuditLog"
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
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblAuditLog%>" />
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
                                                <asp:ListItem Value="LogID" Text="<%$ Resources:re, lblLogID%>"></asp:ListItem>
                                                <asp:ListItem Value="LogTableName" Text="<%$ Resources:re, lblLogTableName%>"></asp:ListItem>
                                                <asp:ListItem Value="LogPKID" Text="<%$ Resources:re, lblLogPKID%>"></asp:ListItem>
                                                <asp:ListItem Value="LogContent" Text="<%$ Resources:re, lblLogContent%>"></asp:ListItem>
                                                <asp:ListItem Value="LogDate" Text="<%$ Resources:re, lblLogDate%>"></asp:ListItem>
                                                <asp:ListItem Value="LogUser" Text="<%$ Resources:re, lblLogUser%>"></asp:ListItem>
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
                                HeaderStyle-CssClass="Head" RowStyle-CssClass="Row" DataKeyNames="LogID" FooterStyle-CssClass="Foot"
                                Width="100%" ShowFooter="True" VirtualItemCount="-1" AllowPaging="True" OnPageIndexChanging="gvData_PageIndexChanging"
                                OnRowDataBound="gvData_RowDataBound" OnRowDeleting="gvData_RowDeleting" OrderBy=""
                                AllowSorting="True" OnSorting="gvData_Sorting">
                                <Columns>
                                    <asp:BoundField DataField="LogID" SortExpression="LogID" HeaderText="<%$ Resources:re, lblLogID%>" />
                                    <asp:BoundField DataField="LogTableName" SortExpression="LogTableName" HeaderText="<%$ Resources:re, lblLogTableName%>" />
                                    <asp:BoundField DataField="LogPKID" SortExpression="LogPKID" HeaderText="<%$ Resources:re, lblLogPKID%>" />
                                    <asp:BoundField DataField="LogContent" SortExpression="LogContent" HeaderText="<%$ Resources:re, lblLogContent%>" />
                                    <asp:BoundField DataField="LogDate" SortExpression="LogDate" HeaderText="<%$ Resources:re, lblLogDate%>"
                                        DataFormatString="<%$ Resources:re, DateFormatString%>" />
                                    <asp:BoundField DataField="LogUser" SortExpression="LogUser" HeaderText="<%$ Resources:re, lblLogUser%>" />
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
