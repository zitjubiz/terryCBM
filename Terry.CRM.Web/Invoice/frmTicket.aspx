<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true"
    CodeBehind="frmTicket.aspx.cs" Inherits="Terry.CRM.Web.Invoice.frmTicket" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblTicket %>" />
                                
                            </span>
                            
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div class="sidebar_header_search" style="height: 70px;">
                            <div id="divTicket" runat="server" style="float: left; width: 100%; margin-top: 4px;">
                            </div>
                            <div style="float: left; margin-top: 0px; width: 100%">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="5%">
                                            地区<asp:DropDownList ID="ddlDept" Width="50px" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlDept_SelectedIndexChanged">
                                                <asp:ListItem Value="">所有</asp:ListItem>
                                                <asp:ListItem Value="21">杜塞</asp:ListItem>
                                                <asp:ListItem Value="22">斯图</asp:ListItem>
                                                <asp:ListItem Value="23">科隆</asp:ListItem>
                                                <asp:ListItem Value="61">麻城</asp:ListItem>
                                                <asp:ListItem Value="62">阿纳姆</asp:ListItem>
                                                <asp:ListItem Value="24">代理</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td width="5%">
                                            月份<asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                                                <asp:ListItem Value="">所有</asp:ListItem>
                                                <asp:ListItem>01</asp:ListItem>
                                                <asp:ListItem>02</asp:ListItem>
                                                <asp:ListItem>03</asp:ListItem>
                                                <asp:ListItem>04</asp:ListItem>
                                                <asp:ListItem>05</asp:ListItem>
                                                <asp:ListItem>06</asp:ListItem>
                                                <asp:ListItem>07</asp:ListItem>
                                                <asp:ListItem>08</asp:ListItem>
                                                <asp:ListItem>09</asp:ListItem>
                                                <asp:ListItem>10</asp:ListItem>
                                                <asp:ListItem>11</asp:ListItem>
                                                <asp:ListItem>12</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="lblInnerReferenceID" runat="server" Text="<%$ Resources:re, lblInnerReferenceID%>"></asp:Label>
                                            <asp:TextBox ID="txtInnerReferenceID" runat="server" usage="" Width="100px">
                                            </asp:TextBox>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="lblOwner" runat="server">销售人员</asp:Label>
                                            <tag:DropDownList ID="ddlOwner" Width="100px" runat="server">
                                            </tag:DropDownList>
                                        </td>
                                        <td width="10%">
                                            Email
                                            <asp:TextBox ID="txtEmail" runat="server" Style="width: 100px"></asp:TextBox>
                                        </td>
                                        <td width="10%">
                                            客户名称<asp:TextBox ID="txtKeyword" runat="server" Style="width: 100px"></asp:TextBox>
                                        </td>
                                        <td width="10%">
                                            电话<asp:TextBox ID="txtTel" runat="server" Style="width: 100px" onkeydown="if(event.keyCode==13) $('#ctl00_CPH1_btnSearch').click();"></asp:TextBox>
                                        </td>
                                        <td width="10%">
                                            金额<asp:TextBox ID="txtAmount" runat="server" Style="width: 80px" onkeydown="if(event.keyCode==13) $('#ctl00_CPH1_btnSearch').click();"></asp:TextBox>
                                        </td>
                                        <td width="300px">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <div class="sidebar_search_btn" style="float: right; padding-top: 10px;">
                                                            <asp:Button ID="btnSearch" runat="server" Width="50px" Text="<%$ Resources:re, lblSearch%>"
                                                                OnClick="btnSearch_Click" />
                                                            <asp:Button ID="btnReset" runat="server" Width="90px" Text="<%$ Resources:re, lblResetFilter%>"
                                                                OnClick="btnReset_Click" />
                                                            <asp:Button ID="btnNew" runat="server" Width="80px" Text="新增" OnClick="btnNew_Click" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    </table>
                                <br />
                            </div>
                        </div>
                        <div id="Grid" style="float: left;">
                            <cc1:PagingGridView ID="gvData" runat="server" AutoGenerateColumns="false" CssClass="tableBorder"
                                HeaderStyle-CssClass="Head" RowStyle-CssClass="Row" DataKeyNames="ID" FooterStyle-CssClass="Foot"
                                Width="100%" ShowFooter="True" VirtualItemCount="-1" AllowPaging="True" OnPageIndexChanging="gvData_PageIndexChanging"
                                OnRowDataBound="gvData_RowDataBound" OnRowDeleting="gvData_RowDeleting" OrderBy="CustName"
                                AllowSorting="True" OnSorting="gvData_Sorting" EnableViewState="false">
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="30"></PagerSettings>
                                <RowStyle CssClass="Row"></RowStyle>
                                <Columns>
                                    <asp:BoundField DataField="InnerReferenceID" SortExpression="InnerReferenceID" HeaderText="<%$ Resources:re, lblInnerReferenceID%>" />
                                    <asp:BoundField DataField="CustName" SortExpression="CustName" HeaderText="<%$ Resources:re, lblCustFullName%>" />
                                    <asp:BoundField DataField="CustTel" SortExpression="CustTel" HeaderText="<%$ Resources:re, lblCustTel%>" />
                                    <asp:BoundField DataField="TotalAmount" SortExpression="TotalAmount" HeaderText="<%$ Resources:re, lblTotalAmount%>" />
                                    <asp:BoundField DataField="BookingDate" SortExpression="ModifyDate" DataFormatString="{0:d}"
                                        HeaderText="<%$ Resources:re, lblBookingDate%>" />
                                </Columns>
                                <FooterStyle CssClass="Foot"></FooterStyle>
                                <PagerStyle CssClass="Pager"></PagerStyle>
                                <HeaderStyle CssClass="Head"></HeaderStyle>
                            </cc1:PagingGridView>
                        </div>
                        <span style="color: #ff3300; font-size: x-large">■</span>未收款 <span style="color: Orange;
                            font-size: x-large">■</span>已取消 <span style="color: Purple; font-size: x-large">■</span>退票
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
