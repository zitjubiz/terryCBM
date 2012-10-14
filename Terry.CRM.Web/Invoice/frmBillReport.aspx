<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true"
    CodeBehind="frmBillReport.aspx.cs" Inherits="Terry.CRM.Web.Invoice.frmBillReport" %>

<%@ Register Assembly="Terry.WebControls.DropDownList" Namespace="Terry.WebControls"
    TagPrefix="tag" %>
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
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblReport %>" />
                            </span>
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div class="sidebar_header_search" style="height: 60px;">
                            <div  style="float: left; width: 100%; margin-top: 4px;">
                            </div>
                            <div style="float: left; margin-top: 0px; width: 100%">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="30%" align="right">
                                            <asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label>
                                            <asp:DropDownList ID="ddlYear" runat="server">
                                                <asp:ListItem>2011</asp:ListItem>
                                                <asp:ListItem>2012</asp:ListItem>
                                                <asp:ListItem>2013</asp:ListItem>
                                                <asp:ListItem>2014</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblMonth" runat="server" Text="Month"></asp:Label>
                                            <asp:DropDownList ID="ddlMonth" runat="server">
                                                <asp:ListItem>1</asp:ListItem>
                                                <asp:ListItem>2</asp:ListItem>
                                                <asp:ListItem>3</asp:ListItem>
                                                <asp:ListItem>4</asp:ListItem>
                                                <asp:ListItem>5</asp:ListItem>
                                                <asp:ListItem>6</asp:ListItem>
                                                <asp:ListItem>7</asp:ListItem>
                                                <asp:ListItem>8</asp:ListItem>
                                                <asp:ListItem>9</asp:ListItem>
                                                <asp:ListItem>10</asp:ListItem>
                                                <asp:ListItem>11</asp:ListItem>
                                                <asp:ListItem>12</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td width="20%">
                                            
                                        </td>
                                        <td width="300px">
                                            <asp:Button ID="btnVisum" runat="server" OnClick="btnVisum_Click" Text="送签流水(签证)" />
                                            <asp:Button ID="btnVisumAccount" runat="server" OnClick="btnVisumAccount_Click" Text="会计总账(签证) 德国" />
                                        <asp:Button ID="btnVisumAccountNL" runat="server" OnClick="btnVisumAccountNL_Click" Text="会计总账(签证) 荷兰" />
                                        </td>
                                    </tr>
                                </table>
                                
                            </div>
                        </div>
                        <div class="sidebar_header_search" style="height: 60px;">
                            <div  runat="server" style="float: left; width: 100%; margin-top: 4px;">
                            </div>
                            <div style="float: left; margin-top: 0px; width: 100%">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="30%" align="right">
                                            <asp:Label ID="Label1" runat="server" Text="Year"></asp:Label>
                                            <asp:DropDownList ID="ddlYear1" runat="server">
                                                <asp:ListItem>2011</asp:ListItem>
                                                <asp:ListItem>2012</asp:ListItem>
                                                <asp:ListItem>2013</asp:ListItem>
                                                <asp:ListItem>2014</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="Label2" runat="server" Text="Month"></asp:Label>
                                            <asp:DropDownList ID="ddlMonth1" runat="server">
                                                <asp:ListItem>1</asp:ListItem>
                                                <asp:ListItem>2</asp:ListItem>
                                                <asp:ListItem>3</asp:ListItem>
                                                <asp:ListItem>4</asp:ListItem>
                                                <asp:ListItem>5</asp:ListItem>
                                                <asp:ListItem>6</asp:ListItem>
                                                <asp:ListItem>7</asp:ListItem>
                                                <asp:ListItem>8</asp:ListItem>
                                                <asp:ListItem>9</asp:ListItem>
                                                <asp:ListItem>10</asp:ListItem>
                                                <asp:ListItem>11</asp:ListItem>
                                                <asp:ListItem>12</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td width="10%" align="right">部门</td>
                                        <td width="10%">                                        
                                            <asp:DropDownList ID="ddlDept" runat="server" tip="<%$ Resources:re, MsgDepName%>"
                                                usage="notempty" Width="98%" >
                                                <asp:ListItem Value="0">---选择---</asp:ListItem>
                                                <asp:ListItem Value="21">杜塞尔多夫</asp:ListItem>
                                                <asp:ListItem Value="22">斯图加特</asp:ListItem>
                                                <asp:ListItem Value="23">科隆</asp:ListItem>
                                                <asp:ListItem Value="25">纽伦堡</asp:ListItem>
                                                <asp:ListItem Value="61">马城</asp:ListItem>
                                                <asp:ListItem Value="62">阿纳姆</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td width="300px" nowrap>
                                        <asp:Button ID="btnDailyIssue" runat="server" OnClick="btnDailyIssue_Click" Visible="false"
                                    Text="每日出票(机票)" />
                                            <asp:Button ID="btnTicketAccount" runat="server" OnClick="btnTicketAccount_Click"
                                    Text="会计总账(机票)" />                                            
                                            <asp:Button ID="btnMonthReport" runat="server" 
                                    Text="每月明细(机票)" onclick="btnMonthReport_Click" />
                                
                                        </td>
                                    </tr>
                                </table>
                                
                                <br />
                            </div>
                        </div>
                        <div class="sidebar_header_search" style="height: 60px;">
                            <div id="Div1"  runat="server" style="float: left; width: 100%; margin-top: 4px;">
                            </div>
                            <div style="float: left; margin-top: 0px; width: 100%">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="30%" align="right">
                                            <asp:Label ID="Label3" runat="server" Text="Year"></asp:Label>
                                            <asp:DropDownList ID="ddlYear2" runat="server">
                                                <asp:ListItem>2011</asp:ListItem>
                                                <asp:ListItem>2012</asp:ListItem>
                                                <asp:ListItem>2013</asp:ListItem>
                                                <asp:ListItem>2014</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="Label4" runat="server" Text="Month"></asp:Label>
                                            <asp:DropDownList ID="ddlMonth2" runat="server">
                                                <asp:ListItem Value="1">1</asp:ListItem>
                                                <asp:ListItem Value="2">2</asp:ListItem>
                                                <asp:ListItem Value="3">3</asp:ListItem>
                                                <asp:ListItem Value="4">4</asp:ListItem>
                                                <asp:ListItem Value="5">5</asp:ListItem>
                                                <asp:ListItem Value="6">6</asp:ListItem>
                                                <asp:ListItem Value="7">7</asp:ListItem>
                                                <asp:ListItem Value="8">8</asp:ListItem>
                                                <asp:ListItem Value="9">9</asp:ListItem>
                                                <asp:ListItem Value="10">10</asp:ListItem>
                                                <asp:ListItem Value="11">11</asp:ListItem>
                                                <asp:ListItem Value="12">12</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td width="10%" align="right">部门</td>
                                        <td width="10%">                                        
                                            <asp:DropDownList ID="ddlDept2" runat="server" tip="<%$ Resources:re, MsgDepName%>"
                                                usage="notempty" Width="98%" >
                                            </asp:DropDownList>
                                        </td>
                                        <td width="300px" nowrap>
                                        <asp:Button ID="btnSalesByPerson" runat="server"
                                    Text="销售人员业绩计算" onclick="btnSalesByPerson_Click" />
                                    
                                        </td>
                                    </tr>
                                </table>
                                
                                <br />
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
