<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmActionEdit.aspx.cs"
    Inherits="Terry.CRM.Web.CRM.frmActionEdit" MasterPageFile="~/MasterPage/Site.Master" %>

<%@ Register Assembly="PagingGridView" Namespace="Terry.Web.Control" TagPrefix="cc1" %>
<%@ Register Assembly="DropDownCheckList" Namespace="Terry.Web.Control" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
    <table id="wrp" cellpadding="0" cellspacing="0" align="center" style="height: 460px;">
        <tr id="wrp_base">
            <td valign="top">
                <div id="wrapper">
                    <div id="main_content" class="content" style="width: 100%">
                        <div id="navbar">
                            <span id="currentModule" />
                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:re, lblHome%>" />
                            &gt;<asp:Literal ID="lblCust" runat="server" Text="" />
                            <asp:Literal ID="lblActionType" runat="server" Text="" />
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div class="DataDetailFrom">
                            <div class="DetailDt">
                                <table width="100%" cellpadding="1" cellspacing="1" style="border-collapse: collapse;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblACTType" runat="server" Text="<%$ Resources:re, lblACTType %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="txtACTType" runat="server" tip="<%$ Resources:re, MsgACTType%>"
                                            visible="false"  usage="" Width="100%">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlActionType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlActionType_SelectedIndexChanged">
                                                <asp:ListItem Value="1">电话记录</asp:ListItem>
                                                <asp:ListItem Value="2">上门拜访</asp:ListItem>
                                                <asp:ListItem Value="3">报价记录</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="150px">
                                            <asp:Label ID="lblACTSubject" runat="server" Text="<%$ Resources:re, lblACTSubject %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            
                                            <asp:TextBox ID="txtACTSubject" runat="server" tip="<%$ Resources:re, MsgACTSubject%>"
                                                usage="notempty" Width="220px">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblACTBeginDate" runat="server" Text="<%$ Resources:re, lblACTBeginDate %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtACTBeginDate" runat="server" tip="<%$ Resources:re, MsgACTBeginDate%>"
                                                AutoComplete="off" usage="notempty" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"
                                                Width="220px">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblACTNextBuyDate" runat="server" Text="<%$ Resources:re, lblACTNextBuyDate %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtACTNextBuyDate" runat="server" AutoComplete="off" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                                                Width="220px">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblACTUser" runat="server" Text="<%$ Resources:re, lblACTUser %>"></asp:Label>
                                            :
                                        </td>
                                        <td colspan="5">
                                            <cc2:DropDownCheckList ID="DDCLUser" runat="server" ClientCodeLocation="../js/DropDownCheckList.js"
                                                DropImageSrc="../images/expand.gif" RepeatColumns="2" DisplayTextWidth="220"
                                                Width="220" TextWhenNoneChecked="-----Select-----" CheckListCssClass="checkbox">
                                            </cc2:DropDownCheckList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblACTContent" runat="server" Text="<%$ Resources:re, lblACTContent %>"></asp:Label>
                                            :
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtACTContent" runat="server" tip="<%$ Resources:re, MsgACTContent%>"
                                                usage="notempty" Width="99%" Height="160" TextMode="MultiLine">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblACTCDate" runat="server" Text="<%$ Resources:re, lblACTCDate %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtACTCDate" runat="server" tip="<%$ Resources:re, MsgACTCDate%>"
                                                usage="notempty" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" Width="220px"
                                                Enabled="false">					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblACTCUser" runat="server" Text="<%$ Resources:re, lblACTCUser %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="txtACTCUserID" runat="server" />
                                            <asp:TextBox ID="txtACTCUser" runat="server" tip="<%$ Resources:re, MsgACTCUser%>"
                                                usage="notempty" Width="220px" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblACTModifyDate" runat="server" Text="<%$ Resources:re, lblACTModifyDate %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtACTModifyDate" runat="server" tip="<%$ Resources:re, MsgACTModifyDate%>"
                                                usage="notempty" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" Width="220px"
                                                Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblACTModifyUser" runat="server" Text="<%$ Resources:re, lblACTModifyUser %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="txtACTModifyUserID" runat="server" />
                                            <asp:TextBox ID="txtACTModifyUser" runat="server" tip="<%$ Resources:re, MsgACTModifyUser%>"
                                                usage="notempty" Width="220px" Enabled="false">					
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td>
                                            <asp:Label ID="lblACTOPPID" runat="server" Text="<%$ Resources:re, lblCustomer %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="txtACTCustID" runat="server" tip="<%$ Resources:re, MsgCustomer%>"
                                                usage="" Width="100%">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblACTEndDate" runat="server" Text="<%$ Resources:re, lblACTEndDate %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtACTEndDate" runat="server" tip="<%$ Resources:re, MsgACTEndDate%>"
                                                AutoComplete="off" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" Width="100%">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <div id="divErrorMessage">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Grid">
                        <div style="float: left; width: 70%;">
                            <cc1:PagingGridView ID="gvData" runat="server" AutoGenerateColumns="False" CssClass="tableBorder"
                                HeaderStyle-CssClass="Head" RowStyle-CssClass="Row" DataKeyNames="ACTID" FooterStyle-CssClass="Foot"
                                Width="100%" ShowFooter="True" VirtualItemCount="-1" AllowPaging="True" OnPageIndexChanging="gvData_PageIndexChanging"
                                OnRowDataBound="gvData_RowDataBound" OnRowDeleting="gvData_RowDeleting" OrderBy=""
                                AllowSorting="True" OnSorting="gvData_Sorting" PagerSettings-Mode="NextPrevious">
                                <PagerStyle HorizontalAlign="Right" />
                                <Columns>
                                    <asp:BoundField DataField="CreateDate" SortExpression="CreateDate" HeaderText="<%$ Resources:re, lblACTBeginDate%>"
                                        DataFormatString="<%$ Resources:re, DateFormatString%>" />
                                    <asp:BoundField DataField="Comment" SortExpression="Comment" HeaderText="<%$ Resources:re, lblACTSubject%>" />
                                    <asp:BoundField DataField="CreateUser" SortExpression="CreateUser" HeaderText="<%$ Resources:re, lblACTCUser%>" />
                                    <asp:TemplateField ShowHeader="False" ItemStyle-HorizontalAlign="Center" Visible="false">
                                        <ItemTemplate>
                                            <asp:Button ID="lnkDel" runat="server" CommandName="Delete" CssClass="linkButton"
                                                Text="<%$ Resources:re, lblDelete%>"></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </cc1:PagingGridView>
                        </div>
                        <div style="float: right; width: 25%; margin-left: 10px;">
                            <div class="head">
                                交流意见</div>
                            <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Height="72px" Width="229px"></asp:TextBox>
                            <asp:Button ID="btnComment" runat="server" Text="发表意见" OnClick="btnComment_Click" />
                        </div>
                    </div>
                    <div class="clearer">
                        <!--  -->
                    </div>
                    <div class="button">
                        <br />
                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:re, lblSave%>" check="true"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnDel" Visible="false" runat="server" Text="<%$ Resources:re, lblDelete%>"
                            OnClick="btnDel_Click" />
                        <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:re, lblBack%>" OnClick="btnBack_Click" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidID" runat="server" Value="" />
    <asp:HiddenField ID="hidSaveMsg" runat="server" Value="<%$ Resources:re, MsgSaveConfirm%>" />
    <asp:HiddenField ID="hidDelMsg" runat="server" Value="<%$ Resources:re, MsgDeleteConfirm%>" />
    <asp:HiddenField ID="hidBtnDel" runat="server" Value="" />
    <asp:TextBox ID="txtACTID" runat="server" tip="<%$ Resources:re, MsgACTID%>" usage="notempty"
        Visible="false" Width="100%">
    </asp:TextBox>
</asp:Content>
