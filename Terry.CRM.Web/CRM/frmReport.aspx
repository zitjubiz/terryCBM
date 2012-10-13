<%@ Page Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true"
    CodeBehind="frmReport.aspx.cs" Inherits="Terry.CRM.Web.CRM.frmReport" Title="Report" %>

<%@ Register Assembly="PagingGridView" Namespace="Terry.Web.Control" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/Top10Cust.ascx" TagName="Top10Cust" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/Top10Sale.ascx" TagName="Top10Sale" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/MonthlySale.ascx" TagName="MonthlySale" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/RegionSale.ascx" TagName="RegionSale" TagPrefix="uc4" %>
<%@ Register src="../UserControl/DepSale.ascx" tagname="DepSale" tagprefix="uc5" %>
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
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblReport%>" />
                            </span>
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div class="sidebar_header_search">
                            <div>
                                <asp:Label ID="Label1" runat="server" Text="开始日期"></asp:Label>
                                <asp:TextBox ID="txtBeginDate" runat="server" AutoComplete="off" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                                    Width="120px">
                                </asp:TextBox>
                                <asp:Label ID="Label2" runat="server" Text="结束日期"></asp:Label>
                                <asp:TextBox ID="txtEndDate" runat="server" AutoComplete="off" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                                    Width="120px">
                                </asp:TextBox>
                                 <asp:Label ID="Label3" runat="server" Text="部门"></asp:Label>
                                <asp:DropDownList ID="ddlDept" runat="server" tip="<%$ Resources:re, MsgDepName%>"
                                                usage="" Width="150px">
                                </asp:DropDownList>
                                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:re, lblSearch%>" OnClick="btnSearch_Click" />
                            </div>
                            <div class="sidebar_search_btn">
                            </div>
                        </div>
                        <div id="Grid">
                            <table>
                                <tr>
                                    <td colspan=2>
                                        <uc1:Top10Cust ID="Top10Cust1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan=2>
                                        <uc2:Top10Sale ID="Top10Sale1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <uc3:MonthlySale ID="MonthlySale1" runat="server" />
                                    </td>
                                
                                    <td><uc5:DepSale ID="DepSale1" runat="server" />
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    <%--<uc4:RegionSale ID="RegionSale1" runat="server" />--%>
                                        
                                    
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="clearer">
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidSaveMsg" runat="server" Value="<%$ Resources:re, MsgSaveConfirm%>" />
    <asp:HiddenField ID="hidDelMsg" runat="server" Value="<%$ Resources:re, MsgDeleteConfirm%>" />
    <asp:HiddenField ID="hidBtnDel" runat="server" Value="" />
</asp:Content>
