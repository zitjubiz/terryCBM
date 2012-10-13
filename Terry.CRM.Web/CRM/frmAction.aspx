<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="frmAction.aspx.cs" Inherits="Terry.CRM.Web.CRM.frmAction"
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
                                <asp:Literal ID="lblActionType" runat="server" Text="<%$ Resources:re, lblAction%>" />
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
                                            <asp:DropDownList ID="ddlSearch" runat="server" Width="150px" Enabled="false">
                                                <asp:ListItem Value="ACTID" Text="<%$ Resources:re, lblACTID%>"></asp:ListItem>
                                                <asp:ListItem Value="ACTSubject" Text="<%$ Resources:re, lblACTSubject%>"></asp:ListItem>
                                                <asp:ListItem Value="ACTType" Text="<%$ Resources:re, lblACTType%>"></asp:ListItem>
                                                <asp:ListItem Value="ACTBeginDate" Text="<%$ Resources:re, lblACTBeginDate%>"></asp:ListItem>
                                                <asp:ListItem Value="ACTEndDate" Text="<%$ Resources:re, lblACTEndDate%>"></asp:ListItem>
                                                <asp:ListItem Value="ACTContent" Text="<%$ Resources:re, lblACTContent%>"></asp:ListItem>
                                                <asp:ListItem Value="CustID" Text="<%$ Resources:re, lblACTOPPID%>"></asp:ListItem>
                                                <asp:ListItem Value="ACTCDate" Text="<%$ Resources:re, lblACTCDate%>"></asp:ListItem>
                                                <asp:ListItem Value="ACTCUser" Text="<%$ Resources:re, lblACTCUser%>"></asp:ListItem>
                                                <asp:ListItem Value="ACTModifyDate" Text="<%$ Resources:re, lblACTModifyDate%>"></asp:ListItem>
                                                <asp:ListItem Value="ACTModifyUser" Text="<%$ Resources:re, lblACTModifyUser%>"></asp:ListItem>
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
                                HeaderStyle-CssClass="Head" RowStyle-CssClass="Row" DataKeyNames="ACTID" FooterStyle-CssClass="Foot"
                                Width="100%" ShowFooter="True" VirtualItemCount="-1" AllowPaging="True" OnPageIndexChanging="gvData_PageIndexChanging"
                                OnRowDataBound="gvData_RowDataBound" OnRowDeleting="gvData_RowDeleting" OrderBy=""
                                AllowSorting="True" OnSorting="gvData_Sorting">
                                <Columns>
                                    <asp:BoundField DataField="ACTBeginDate" SortExpression="ACTBeginDate" HeaderText="<%$ Resources:re, lblACTBeginDate%>"
                                        DataFormatString="<%$ Resources:re, DateFormatString%>" />
                                    <asp:BoundField DataField="JoinPerson" SortExpression="JoinPerson" HeaderText="<%$ Resources:re, lblACTUser%>" />
                                    <asp:BoundField DataField="ACTSubject" SortExpression="ACTSubject" HeaderText="<%$ Resources:re, lblACTSubject%>" />
                                    <asp:BoundField DataField="ACTContent" SortExpression="ACTContent" 
                                    HeaderText="<%$ Resources:re, lblACTContent%>"  
                                     DataFormatString="<%$ Resources:re, DateFormatString%>"/>
                                    
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
