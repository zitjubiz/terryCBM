<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="frmCustomer.aspx.cs" Inherits="Terry.CRM.Web.CRM.frmCustomer"
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
                        <div class="sidebar_header_search" style="height: 128px;">
                            <div id="divChemical" runat="server" style="float: left; margin-top: 4px;">
                                使用产品:<asp:DropDownList ID="ddlProduct" runat="server">
                                </asp:DropDownList>
                                市场部门:<asp:DropDownList ID="ddlCategory" runat="server">
                                    <asp:ListItem Value="" Text="<%$ Resources:re, DDLSelected%>"></asp:ListItem>
                                    <asp:ListItem Value="1">特殊塑料</asp:ListItem>
                                    <asp:ListItem Value="2">通用塑料</asp:ListItem>
                                    <asp:ListItem Value="3">工程塑料</asp:ListItem>
                                    <asp:ListItem Value="4">出口薄膜</asp:ListItem>
                                    <asp:ListItem Value="5">特殊化学品</asp:ListItem>
                                    <asp:ListItem Value="6">清洗剂</asp:ListItem>
                                </asp:DropDownList>
                                有无违约:<asp:DropDownList ID="ddlCancel" runat="server">
                                    <asp:ListItem Value="" Text="<%$ Resources:re, DDLSelected%>"></asp:ListItem>
                                    <asp:ListItem Value="0">无违约</asp:ListItem>
                                    <asp:ListItem Value="1">有违约</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div id="divTicket" runat="server" style="float: left; width: 100%; margin-top: 4px;">
                                <table width="100%">
                                    <tr>
                                        <td width="10%">
                                            性别<asp:DropDownList ID="ddlGender" runat="server" tip="<%$ Resources:re, MsgGender%>"
                                                usage="" Width="100px">
                                                <asp:ListItem Value="" Text="<%$ Resources:re, DDLSelected%>"></asp:ListItem>
                                                <asp:ListItem Value="M">MR</asp:ListItem>
                                                <asp:ListItem Value="F">MRS</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="lblPassport" runat="server" Text="<%$ Resources:re, lblPassport%>"></asp:Label>
                                            <asp:TextBox ID="txtPassport" runat="server" usage="" Width="100px">
                                            </asp:TextBox>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="lblParentCompany" runat="server" Text="<%$ Resources:re, lblParentCompany%>"></asp:Label>
                                            <asp:TextBox ID="txtParentCompany" runat="server" usage="" Width="100px">
                                            </asp:TextBox>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="lblUseOwnMoney" runat="server" Text="<%$ Resources:re, lblUseOwnMoney%>"></asp:Label>
                                            <asp:DropDownList ID="ddlUseOwnMoney" runat="server" usage="" Width="100px">
                                                <asp:ListItem Value="" Text="<%$ Resources:re, DDLSelected%>"></asp:ListItem>
                                                <asp:ListItem Value="1">是</asp:ListItem>
                                                <asp:ListItem Value="0">否</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="lblFavoriteProd" runat="server" Text="<%$ Resources:re, lblFavoriteProd%>"></asp:Label>
                                            <asp:DropDownList ID="ddlFavoriteProd" runat="server" Width="100px" onchange="$('#ctl00_CPH1_txtFavoriteProd').val(this.value);">
                                                <asp:ListItem Value="" Text="<%$ Resources:re, DDLSelected%>"></asp:ListItem>
                                                <asp:ListItem>汉莎航空</asp:ListItem>
                                                <asp:ListItem>荷兰航空</asp:ListItem>
                                                <asp:ListItem>奥地利航空</asp:ListItem>
                                                <asp:ListItem>国泰航空</asp:ListItem>
                                                <asp:ListItem>土耳其航空</asp:ListItem>
                                                <asp:ListItem>芬兰航空</asp:ListItem>
                                                <asp:ListItem>中国国航</asp:ListItem>
                                                <asp:ListItem>南方航空</asp:ListItem>
                                                <asp:ListItem>东方航空</asp:ListItem>
                                                <asp:ListItem>海南航空</asp:ListItem>
                                                <asp:ListItem>深圳航空</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="lblPreferPrice" runat="server" Text="<%$ Resources:re, lblPreferPrice%>"></asp:Label>
                                            <asp:DropDownList ID="ddlPreferPrice" runat="server" usage="" Width="100px">
                                                <asp:ListItem Value="" Text="<%$ Resources:re, DDLSelected%>"></asp:ListItem>
                                                <asp:ListItem Value="商务舱">商务舱</asp:ListItem>
                                                <asp:ListItem Value="经济舱">经济舱</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="lblPreferPlace" runat="server" Text="<%$ Resources:re, lblPreferPlace%>"></asp:Label>
                                            <asp:DropDownList ID="ddlPreferPlace" runat="server" Width="100px" onchange="$('#ctl00_CPH1_txtPreferPlace').val($('#ctl00_CPH1_txtPreferPlace').val()+','+this.value);">
                                                <asp:ListItem Value="" Text="<%$ Resources:re, DDLSelected%>"></asp:ListItem>
                                                <asp:ListItem>慕尼黑</asp:ListItem>
                                                <asp:ListItem>法兰克福</asp:ListItem>
                                                <asp:ListItem>杜塞尔多夫</asp:ListItem>
                                                <asp:ListItem>柏林</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="lblTravelDay" runat="server" Text="<%$ Resources:re, lblTravelDay%>"></asp:Label>
                                            <asp:DropDownList ID="ddlTravelDay" runat="server" RepeatDirection="Horizontal" RepeatColumns="3"
                                                usage="" Width="100px">
                                                <asp:ListItem Value="" Text="<%$ Resources:re, DDLSelected%>"></asp:ListItem>
                                                <asp:ListItem>暑假</asp:ListItem>
                                                <asp:ListItem>寒假</asp:ListItem>
                                                <asp:ListItem>春节</asp:ListItem>
                                                <asp:ListItem>圣诞</asp:ListItem>
                                                <asp:ListItem>五一</asp:ListItem>
                                                <asp:ListItem>十一</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td width="20%">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="float: left; margin-top: 0px; width: 100%">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="10%">
                                            客户类型<asp:DropDownList ID="ddlCustType" runat="server" Width="100px">
                                            </asp:DropDownList>
                                        </td>
                                        <td width="10%">
                                            上次消费日期<asp:DropDownList ID="ddlLastDeal" runat="server" Width="100px">
                                                <asp:ListItem Value="" Text="<%$ Resources:re, DDLSelected%>"></asp:ListItem>
                                                <asp:ListItem Value="60">2个月前</asp:ListItem>
                                                <asp:ListItem Value="180">6个月前</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td width="200px">
                                            客户级别<asp:DropDownList ID="ddlGrade" runat="server" Width="180px">
                                            </asp:DropDownList>
                                        </td>
                                        <td width="10%">
                                            地区<asp:DropDownList ID="ddlRegion" runat="server" Width="100px">
                                                <asp:ListItem Value="">---选择---</asp:ListItem>
                                                <asp:ListItem>巴登-符腾堡（Baden-Württemberg）</asp:ListItem>
                                                <asp:ListItem>巴伐利亚（Bayern）</asp:ListItem>
                                                <asp:ListItem>柏林(Berlin）</asp:ListItem>
                                                <asp:ListItem>勃兰登堡（Brandenburg）</asp:ListItem>
                                                <asp:ListItem>不来梅（Bremen）</asp:ListItem>
                                                <asp:ListItem>汉堡（Hamburg）</asp:ListItem>
                                                <asp:ListItem>黑森（Hessen）</asp:ListItem>
                                                <asp:ListItem>梅克伦堡-前波莫瑞州（Mecklenburg-Vorpommern）</asp:ListItem>
                                                <asp:ListItem>下萨克森（Niedersachsen）</asp:ListItem>
                                                <asp:ListItem>北莱茵-威斯特法伦（Nordrhein-Westfalen）</asp:ListItem>
                                                <asp:ListItem>莱茵兰-普法尔茨（Rheinland-Pfalz）</asp:ListItem>
                                                <asp:ListItem>萨尔（Saarland）</asp:ListItem>
                                                <asp:ListItem>萨克森（Sachsen）</asp:ListItem>
                                                <asp:ListItem>萨克森-安哈尔特（Sachsen-Anhalt）</asp:ListItem>
                                                <asp:ListItem>石勒苏益格-荷尔斯泰因（Schleswig-Holstein）</asp:ListItem>
                                                <asp:ListItem>图林根（Thüringen）</asp:ListItem>
                                            </asp:DropDownList>
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
                                        <td width="20%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <div class="sidebar_search_btn" style="float: right; padding-top: 10px;">
                                                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:re, lblSearch%>" OnClick="btnSearch_Click" />
                                                <asp:Button ID="btnReset" runat="server" Width="120px" Text="<%$ Resources:re, lblResetFilter%>"
                                                    OnClick="btnReset_Click" />
                                                <asp:Button ID="btnNew" runat="server" Width="120px" Text="新增客户信息" OnClick="btnNew_Click" />
                                                <asp:Button ID="btnEmail" runat="server" Width="150px" Text="对搜索结果发送邮件" OnClick="btnEmail_Click" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </div>
                        </div>
                        <div id="Grid" style="float: left;">
                            <cc1:PagingGridView ID="gvData" runat="server" AutoGenerateColumns="false" CssClass="tableBorder"
                                HeaderStyle-CssClass="Head" RowStyle-CssClass="Row" DataKeyNames="CustID" FooterStyle-CssClass="Foot"
                                Width="100%" ShowFooter="True" VirtualItemCount="-1" AllowPaging="True" OnPageIndexChanging="gvData_PageIndexChanging"
                                OnRowDataBound="gvData_RowDataBound" OnRowDeleting="gvData_RowDeleting" OrderBy="CustName"
                                AllowSorting="True" OnSorting="gvData_Sorting" EnableViewState="false">
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="30"></PagerSettings>
                                <RowStyle CssClass="Row"></RowStyle>
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IdleDays" HeaderText=""
                                        Visible="false">
                                        <HeaderStyle Width="60px" />
                                        <ItemTemplate>
                                            <asp:Image ID="imgIdle" runat="server" BorderWidth="0" ImageUrl="../images/report.gif" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CustCode" SortExpression="CustCode" HeaderText="<%$ Resources:re, lblCustCode%>"
                                        Visible="false" />
                                    <asp:BoundField DataField="CustFullName" SortExpression="CustFullName" HeaderText="<%$ Resources:re, lblCustFullName%>" />
                                    <asp:BoundField DataField="CustType" SortExpression="CustType" HeaderText="<%$ Resources:re, lblCustTypeID%>" />
                                    <asp:BoundField DataField="CustRelation" SortExpression="CustRelation" HeaderText="<%$ Resources:re, lblCustomerRelation%>" />
                                    <asp:BoundField DataField="CustTel" SortExpression="CustTel" HeaderText="<%$ Resources:re, lblCustTel%>" />
                                    <asp:BoundField DataField="UseOwnMoney" SortExpression="UseOwnMoney" HeaderText="<%$ Resources:re, lblUseOwnMoney%>" />
                                    <asp:BoundField DataField="LatestDealDate" SortExpression="LatestDealDate" DataFormatString="{0:d}"
                                        HeaderText="<%$ Resources:re, lblLatestDealDate%>" />
                                    <asp:BoundField DataField="FavoriteProd" SortExpression="FavoriteProd" HeaderText="<%$ Resources:re, lblFavoriteProd%>" />
                                    <asp:BoundField DataField="PreferPrice" SortExpression="PreferPrice" HeaderText="<%$ Resources:re, lblPreferPrice%>" />
                                    <asp:BoundField DataField="PreferPlace" SortExpression="PreferPlace" HeaderText="<%$ Resources:re, lblPreferPlace%>" />
                                    <asp:TemplateField ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkTel" runat="server" NavigateUrl='<%# Eval("CustID", "frmActionEdit.aspx?id=0&ACType=1&CustID={0}") %>'
                                                Text="<img src='../images/tel.gif'/>"></asp:HyperLink>
                                            <asp:HyperLink ID="lnkVisit" runat="server" NavigateUrl='<%# Eval("CustID", "frmActionEdit.aspx?id=0&ACType=2&CustID={0}") %>'
                                                Text="<img src='../images/visit.gif'/>"></asp:HyperLink>
                                            <asp:HyperLink ID="lnkTicket" runat="server" NavigateUrl='<%# Eval("CustID", "~/Invoice/frmTicketEdit.aspx?id=0&Custid={0}") %>'
                                                Text="<img src='../images/ticket.gif'/>"></asp:HyperLink>
                                            <asp:HyperLink ID="lnkVisum" runat="server" NavigateUrl='<%# Eval("CustID", "~/Invoice/frmVisaEdit.aspx?id=0&Custid={0}") %>'
                                                Text="<img src='../images/visum.gif'/>"></asp:HyperLink>
                                            <asp:HyperLink ID="lnkBid" runat="server" NavigateUrl='<%# Eval("CustID", "frmActionEdit.aspx?id=0&ACType=3&CustID={0}") %>'
                                                Text="<img src='../images/bid.gif'/>"></asp:HyperLink>
                                            <asp:HyperLink ID="lnkDeal" runat="server" NavigateUrl='<%# Eval("CustID", "frmCustomerDeal.aspx?CustID={0}") %>'
                                                Text="<img src='../images/deal.gif'/>" ></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle CssClass="Foot"></FooterStyle>
                                <PagerStyle CssClass="Pager"></PagerStyle>
                                <HeaderStyle CssClass="Head"></HeaderStyle>
                            </cc1:PagingGridView>
                            <div style="display: none">
                                <img src="../images/report.gif" border="0" alt="" /><asp:Label ID="lblIdle" runat="server"
                                    Text="<%$ Resources:re, lblIdle15days%>" />
                                <span style="color: Red; font-size: x-large">■</span> 3个月内无成交 <span style="color: Yellow;
                                    font-size: x-large">■</span>1～ 3个月内有成交 <span style="color: Green; font-size: x-large">
                                        ■</span>1个月内有成交 <span style="color: #d4d4d4; font-size: x-large">■</span>从来没有成交
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
