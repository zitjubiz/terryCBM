<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="frmCustomer.aspx.cs" Inherits="Terry.CRM.Web.CRM.frmCustomer_Chemical"
    MasterPageFile="~/MasterPage/Site.Master" %>

<%@ Register Assembly="Terry.WebControls.DropDownList" Namespace="Terry.WebControls"
    TagPrefix="tag" %>
<%@ Register Assembly="PagingGridView" Namespace="Terry.Web.Control" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
    <table id="wrp" cellpadding="0" cellspacing="0" align="center" style="height: 440px">
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
                        <div class="sidebar_header_search" style="height: 70px;">
                            <table>
                                <tr>
                                    <td>
                                        <div style="float: left; margin-top: 2px;">
                                            �ͻ����ƣ�<asp:TextBox ID="txtKeyword" runat="server" Style="width: 165px"></asp:TextBox>
                                            ʹ�ò�Ʒ��<asp:DropDownList ID="ddlProduct" runat="server">
                                            </asp:DropDownList>
                                            ������Ա��<tag:DropDownList ID="ddlOwner" runat="server">
                                            </tag:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="sidebar_search_btn">
                                            <table>
                                                <tr>
                                                    <td style="padding-left: 10px">
                                                        <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:re, lblSearch%>" OnClick="btnSearch_Click" />
                                                        <asp:Button ID="btnNew" runat="server" Text="<%$ Resources:re, lblNew%>" OnClick="btnNew_Click" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div style="float: left; margin-top: 2px;" >
                                            �ͻ����ͣ�<asp:DropDownList ID="ddlCustType" runat="server" Width="100px">
                                            </asp:DropDownList>
                                            <%--�ͻ�����<asp:DropDownList ID="ddlGrade" runat="server"></asp:DropDownList>--%>
                                            ������<asp:DropDownList ID="ddlRegion" runat="server">
                                                <asp:ListItem Value="">����</asp:ListItem>
                                                <asp:ListItem Value="CS">����</asp:ListItem>
                                                <asp:ListItem Value="CE">����</asp:ListItem>
                                                <asp:ListItem Value="CN">����</asp:ListItem>
                                                <asp:ListItem Value="CW">����</asp:ListItem>
                                            </asp:DropDownList>
                                            ʡ�ݣ�
                                            <asp:DropDownList ID="ddlProvince" runat="server">
                                            </asp:DropDownList>
                                            �г����ţ�<asp:DropDownList ID="ddlCategory" runat="server">
                                                <asp:ListItem Value="">����</asp:ListItem>
                                                <asp:ListItem Value="1">��������</asp:ListItem>
                                                <asp:ListItem Value="2">ͨ������</asp:ListItem>
                                                <asp:ListItem Value="3">��������</asp:ListItem>
                                                <asp:ListItem Value="4">���ڱ�Ĥ</asp:ListItem>
                                                <asp:ListItem Value="5">���⻯ѧƷ</asp:ListItem>
                                                <asp:ListItem Value="6">��ϴ��</asp:ListItem>
                                            </asp:DropDownList>
                                            ����ΥԼ:<asp:DropDownList ID="ddlCancel" runat="server">
                                                <asp:ListItem Value="">����</asp:ListItem>
                                                <asp:ListItem Value="0">��ΥԼ</asp:ListItem>
                                                <asp:ListItem Value="1">��ΥԼ</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="Grid">
                            <cc1:PagingGridView ID="gvData" runat="server" AutoGenerateColumns="False" CssClass="tableBorder"
                                HeaderStyle-CssClass="Head" RowStyle-CssClass="Row" DataKeyNames="CustID" FooterStyle-CssClass="Foot"
                                Width="100%" ShowFooter="True" VirtualItemCount="-1" AllowPaging="True" OnPageIndexChanging="gvData_PageIndexChanging"
                                OnRowDataBound="gvData_RowDataBound" OnRowDeleting="gvData_RowDeleting" OrderBy=""
                                AllowSorting="True" OnSorting="gvData_Sorting">
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IdleDays" HeaderText="">
                                        <HeaderStyle Width="60px" />
                                        <ItemTemplate>
                                            <asp:Image ID="imgIdle" runat="server" BorderWidth="0" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CustCode" SortExpression="CustCode" HeaderText="<%$ Resources:re, lblCustCode%>" />
                                    <asp:BoundField DataField="CustName" SortExpression="CustName" HeaderText="<%$ Resources:re, lblCustName%>" />
                                    <asp:BoundField DataField="CustType" SortExpression="CustType" HeaderText="<%$ Resources:re, lblCustTypeID%>" />
                                    <asp:BoundField DataField="Region" SortExpression="Region" HeaderText="<%$ Resources:re, lblRegion%>" />
                                    <asp:BoundField DataField="CustTel" SortExpression="CustTel" HeaderText="<%$ Resources:re, lblCustTel%>" />
                                    <asp:BoundField DataField="CustOwner" SortExpression="CustOwner" HeaderText="<%$ Resources:re, lblCustOwnerID%>" />
                                    <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-Width="190px">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkTel" runat="server" NavigateUrl='<%# Eval("CustID", "frmActionEdit.aspx?id=0&ACType=1&CustID={0}") %>'
                                                Text="<img src='../images/tel.gif'/>"></asp:HyperLink>
                                            <asp:HyperLink ID="lnkVisit" runat="server" NavigateUrl='<%# Eval("CustID", "frmActionEdit.aspx?id=0&ACType=2&CustID={0}") %>'
                                                Text="<img src='../images/visit.gif'/>"></asp:HyperLink>
                                            <asp:HyperLink ID="lnkBid" runat="server" NavigateUrl='<%# Eval("CustID", "frmActionEdit.aspx?id=0&ACType=3&CustID={0}") %>'
                                                Text="<img src='../images/bid.gif'/>"></asp:HyperLink>
                                            <asp:HyperLink ID="lnkDeal" runat="server" NavigateUrl='<%# Eval("CustID", "frmCustomerDeal.aspx?CustID={0}") %>'
                                                Text="<img src='../images/deal.gif'/>"></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </cc1:PagingGridView>
                            <div>
                                <img src="../images/report.gif" border="0" alt="" /><asp:Label ID="lblIdle" runat="server"
                                    Text="<%$ Resources:re, lblIdle15days%>" />
                                <span style="color: Red; font-size: x-large">��</span> 3�������޳ɽ� <span style="color: Yellow;
                                    font-size: x-large">��</span>1�� 3�������гɽ� <span style="color: Green; font-size: x-large">
                                        ��</span>1�������гɽ� <span style="color: #d4d4d4; font-size: x-large">��</span>����û�гɽ�
                            </div>
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
