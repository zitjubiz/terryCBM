<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCustomerDeal.aspx.cs"
    Inherits="Terry.CRM.Web.CRM.frmCustomerDeal" MasterPageFile="~/MasterPage/Site.Master" %>

<%@ Register Assembly="PagingGridView" Namespace="Terry.Web.Control" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/TextArea.ascx" TagName="TextArea" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
    <table id="wrp" cellpadding="0" cellspacing="0" align="center" style="height: 460px">
        <tr id="wrp_base">
            <td valign="top">
                <div id="wrapper">
                    <div id="main_content" class="content" style="width: 100%">
                        <div id="navbar">
                            <span id="currentModule">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:re, lblHome%>" />
                                &gt;<asp:Literal ID="lblCust" runat="server" Text="" />
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblCustomerDeal%>" />
                            </span>
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div id="Grid">
                            <cc1:PagingGridView ID="gvData" runat="server" AutoGenerateColumns="False" CssClass="tableBorder"
                                HeaderStyle-CssClass="Head" RowStyle-CssClass="Row" DataKeyNames="ID" FooterStyle-CssClass="Foot"
                                Width="100%" ShowFooter="True" VirtualItemCount="-1" AllowPaging="True" OnPageIndexChanging="gvData_PageIndexChanging"
                                OnRowDataBound="gvData_RowDataBound" OnRowDeleting="gvData_RowDeleting" OnRowEditing="gvData_RowEditing"
                                OrderBy="" AllowSorting="True" OnSorting="gvData_Sorting">
                                <PagerSettings Mode="NumericFirstLast"></PagerSettings>
                                <RowStyle CssClass="Row"></RowStyle>
                                <Columns>
                                    <asp:BoundField DataField="DealDate" SortExpression="DealDate" HeaderText="<%$ Resources:re, lblDealDate%>"
                                        DataFormatString="<%$ Resources:re, DateFormatString%>" />
                                    <asp:BoundField DataField="ContractNum" SortExpression="ContractNum" HeaderText="<%$ Resources:re, lblContractNum%>" />
                                    <asp:BoundField DataField="Product" SortExpression="Product" HeaderText="<%$ Resources:re, lblProduct%>" />
                                    <asp:BoundField DataField="Currency" SortExpression="Currency" HeaderText="<%$ Resources:re, lblCurrency%>" />
                                    <asp:BoundField DataField="UnitPrice" SortExpression="UnitPrice" HeaderText="<%$ Resources:re, lblUnitPrice%>" />
                                    <%-- ticket--%>
                                    <asp:BoundField DataField="ProdSerialNum" SortExpression="ProdSerialNum" HeaderText="机票号" />
                                    <asp:BoundField DataField="ReferenceNum" SortExpression="ReferenceNum" HeaderText="Amadeus预订号" />
                                    <asp:BoundField DataField="DealOwnerName" SortExpression="DealOwnerName" HeaderText="销售人员" />
                                    <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="<%$ Resources:re, lblStatus%>" />
                                    <asp:TemplateField ShowHeader="False" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Button ID="lnkEdit" runat="server" CommandName="Edit" CssClass="linkButton"
                                                Text="<%$ Resources:re, lblEdit%>"></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Button ID="lnkDel" runat="server" CommandName="Delete" CssClass="linkButton"
                                                Text="<%$ Resources:re, lblDelete%>"></asp:Button>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle CssClass="Foot"></FooterStyle>
                                <PagerStyle CssClass="Pager"></PagerStyle>
                                <HeaderStyle CssClass="Head"></HeaderStyle>
                            </cc1:PagingGridView>
                        </div>
                    </div>
                    <div class="clearer">
                        <!--  -->
                    </div>
                    <div class="DataDetailFrom">
                        <div class="DetailDt">
                            <table width="100%" border="1">
                                <tr>
                                    <td colspan="4" class="AddressTitle">
                                        <asp:Label ID="lblAction" runat="server" Text="编辑"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="<%$ Resources:re, lblDealDate%>"></asp:Label>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDealDate" runat="server" tip="<%$ Resources:re, MsgCustModifyDate%>"
                                            Enabled="true" usage="notempty" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                                            Width="80%">
                                        </asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:re, lblContractNum%>"></asp:Label>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtContractNum" runat="server" MaxLength="50" Width="80%" usage="notempty"
                                            tip="<%$ Resources:re, MsgInnerReferenceID%>"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        购买产品:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlProduct" runat="server" usage="notempty" tip="购买产品不能为空"
                                            Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                         <asp:Label ID="lblCustOwnerID" runat="server" Text="<%$ Resources:re, lblCustOwnerID %>"></asp:Label>:
                                    </td>
                                    <td>
                                    <asp:DropDownList ID="txtCustOwnerID" runat="server" tip="<%$ Resources:re, MsgCustOwnerID%>"
                                                usage="notempty" Width="120px">
                                            </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="Span1">数量</span> :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQty" runat="server" Width="70%" MaxLength="50" usage="num+" tip="数量必须是数字"></asp:TextBox>
                                        <asp:DropDownList ID="ddlUnit" runat="server">
                                            <%--<asp:ListItem>MT</asp:ListItem>
                                            <asp:ListItem>KG</asp:ListItem>--%>
                                            <asp:ListItem>PCS</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <span id="Span2">价格</span> :
                                    </td>
                                    <td nowrap="nowrap" width="400px">
                                        <asp:TextBox ID="txtUnitPrice" runat="server" Width="80%" MaxLength="50" usage="num"
                                            tip="价格必须是数字"></asp:TextBox>
                                        <asp:DropDownList ID="ddlCurrency" runat="server">
                                            <asp:ListItem>EUR</asp:ListItem>
<%--                                            <asp:ListItem>USD</asp:ListItem>                                            
                                            <asp:ListItem>RMB</asp:ListItem>--%>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtAmount" runat="server" Width="80%" usage="" Visible="false" tip="数量不能为空"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:re, lblPriceTerm%>"></asp:Label>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPayTerm" runat="server" Width="80%" MaxLength="50" usage=""
                                            tip="付款方式不能为空"></asp:TextBox>
                                    </td>
                                    <td>
                                        <span id="Span6">成交状态</span> :
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="RadStatus" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="正常" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="取消" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                
                                <tr id="trTicket" runat="server">
                                    <td>
                                        <span id="Span4">机票号</span> :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProdSerialNum" runat="server" Width="80%" MaxLength="50" usage=""
                                            tip=""></asp:TextBox>
                                    </td>
                                    <td>
                                        <span id="Span5">Amadeus预订号</span> :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReferenceNum" runat="server" Width="80%" MaxLength="100" usage=""
                                            tip=""></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="Span7">备注</span> :
                                    </td>
                                    <td colspan="3">
                                        <uc1:TextArea ID="txtRemark" runat="server" MaxLength="200" Width="600px" />
                                    </td>
                                </tr>
                            </table>
                            <div id="divErrorMessage">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clearer">
                    <!--  -->
                </div>
                <div class="button">
                    <asp:Button ID="btnNew" runat="server" Text="<%$ Resources:re, lblNew %>" 
                        OnClick="btnNew_Click" />
                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:re, lblSave%>" check="true"
                        OnClick="btnSave_Click" />
                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:re, lblBack%>" OnClick="btnBack_Click" />
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidID" runat="server" Value="" />
    <asp:HiddenField ID="hidSaveMsg" runat="server" Value="<%$ Resources:re, MsgSaveConfirm%>" />
    <asp:HiddenField ID="hidDelMsg" runat="server" Value="<%$ Resources:re, MsgDeleteConfirm%>" />
    <asp:HiddenField ID="hidBtnDel" runat="server" Value="" />
</asp:Content>
