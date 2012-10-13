<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="frmProduct.aspx.cs" Inherits="Terry.CRM.Web.CRM.frmProduct"
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
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblProduct%>" />
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
                                                <asp:ListItem Value="ProdID" Text="<%$ Resources:re, lblID%>"></asp:ListItem>
                                                <asp:ListItem Value="Code" Text="<%$ Resources:re, lblProductCode%>"></asp:ListItem>
                                                <asp:ListItem Value="Product" Text="<%$ Resources:re, lblProduct%>"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:re, lblSearch%>" OnClick="btnSearch_Click" />
                                            <asp:Button ID="btnNew" runat="server" Text="<%$ Resources:re, lblNew%>" OnClick="btnNew_Click" />
                                            <asp:Button ID="btnExcel" runat="server" Width="180px" Text="<%$ Resources:re, lblExcel%>" OnClick="btnExcel_Click" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div id="Grid">
                            <cc1:PagingGridView ID="gvData" runat="server" AutoGenerateColumns="False" CssClass="tableBorder"
                                HeaderStyle-CssClass="Head" RowStyle-CssClass="Row" DataKeyNames="ProdID" FooterStyle-CssClass="Foot"
                                Width="100%" ShowFooter="True" VirtualItemCount="-1" AllowPaging="True" OnPageIndexChanging="gvData_PageIndexChanging"
                                OnRowDataBound="gvData_RowDataBound" OnRowDeleting="gvData_RowDeleting" OrderBy=""
                                AllowSorting="True" OnSorting="gvData_Sorting">
                                <Columns>
                                    <asp:BoundField DataField="ProdID" SortExpression="ProdID" HeaderText="<%$ Resources:re, lblID%>" />
                                    <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="<%$ Resources:re, lblProductCode%>" />
                                    <asp:BoundField DataField="Product" SortExpression="Product" HeaderText="<%$ Resources:re, lblProduct%>" />
                                    <asp:BoundField DataField="ProductFactor" SortExpression="ProductFactor" HeaderText="<%$ Resources:re, lblProductFactor%>" />
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
