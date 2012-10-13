<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="frmCustomerBrief.aspx.cs" Inherits="Terry.CRM.Web.CRM.frmCustomerBrief"
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
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblCustomer%>" />
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
                                                <asp:ListItem Value="Product" Text="<%$ Resources:re, lblCustomer%>"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:re, lblSearch%>" OnClick="btnSearch_Click" />
                                            
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
                                HeaderStyle-CssClass="Head" RowStyle-CssClass="Row" DataKeyNames="CustID" FooterStyle-CssClass="Foot"
                                Width="100%" ShowFooter="True" VirtualItemCount="-1" AllowPaging="True" OnPageIndexChanging="gvData_PageIndexChanging"
                                OnRowDataBound="gvData_RowDataBound" OnRowDeleting="gvData_RowDeleting" OrderBy=""
                                AllowSorting="True" OnSorting="gvData_Sorting">
                                <Columns>
                                    
                                    <asp:BoundField DataField="CustName" SortExpression="CustName" HeaderText="<%$ Resources:re, lblCustName%>" />
                                    <asp:BoundField DataField="CustFullName" SortExpression="CustFullName" HeaderText="<%$ Resources:re, lblCustFullName%>" />
                                    <asp:BoundField DataField="CustType" SortExpression="CustType" HeaderText="<%$ Resources:re, lblCustType%>" />
                                    <asp:BoundField DataField="CommissionFactor" SortExpression="CommissionFactor" HeaderText="<%$ Resources:re, lblCommissionFactor%>" />
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
