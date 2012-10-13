<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmUserActions.aspx.cs"
    Inherits="Terry.CRM.Web.CRM.frmUserActions" MasterPageFile="~/MasterPage/Site.Master" %>
<%@ Register Assembly="Terry.WebControls.DropDownList" Namespace="Terry.WebControls"
    TagPrefix="tag" %>
<%@ Register Assembly="PagingGridView" Namespace="Terry.Web.Control" TagPrefix="cc1" %>
<%@ Register Assembly="Terry.WebControls.Tab" Namespace="Terry.WebControls" TagPrefix="tag" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
    <table id="wrp" cellpadding="0" cellspacing="0" align="center" style="height: 460px">
        <tr id="wrp_base">
            <td valign="top">
                <div id="wrapper">
                    <div id="navbar">
                        <span id="currentModule">
                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:re, lblHome%>" />
                            &gt;
                            <asp:Literal ID="lblActionType" runat="server" Text="<%$ Resources:re, lblAction%>" />
                        </span>
                    </div>
                     <div style="float: left; margin-top: 2px;">
                        客户名称：<asp:TextBox ID="txtKeyword" runat="server" Style="width: 165px"></asp:TextBox>
                                           
                        销售人员：<tag:DropDownList ID="ddlOwner" runat="server"></tag:DropDownList>
                        <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:re, lblSearch%>" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnAdd" runat="server" Text="<%$ Resources:re, lblNew%>" OnClick="btnAdd_Click" Visible="false" />
                        <asp:Label ID="lblAddTip" runat="server" Text="请先选择Tab，再新建电话、拜访等记录" />
                     </div>
                    <div id="toolbar" align="left">
                        <tag:Tab ID="Tab1" runat="server" >
                            <asp:ListItem Value="frmUserActions.aspx?type=" Text="全部记录"></asp:ListItem>
                            <asp:ListItem Value="frmUserActions.aspx?type=成交" Text="成交"></asp:ListItem>
                            <asp:ListItem Value="frmUserActions.aspx?type=电话" Text="电话"></asp:ListItem>
                            <asp:ListItem Value="frmUserActions.aspx?type=上门拜访" Text="拜访"></asp:ListItem>
                            <asp:ListItem Value="frmUserActions.aspx?type=报价" Text="报价"></asp:ListItem>
                            <asp:ListItem Value="frmUserActions.aspx?type=评论" Text="评论"></asp:ListItem>
                        </tag:Tab>
                        <br />
                        <br />
                        <br />
                        
                        <div id="divImportDeal" runat="server" class="sidebar_header_search" style="height: 80px;" visible="false">
                            <div align="left" style="float: left; height: 80px;">
                            1.下载<asp:HyperLink ID="lnkTemplate" runat="server" NavigateUrl="~/CRM/Deal.xls" Target="_blank">Excel模板</asp:HyperLink>
                            <br />
                            2.根据模板填写成交记录文件.<br />
                            3.您可以点击<b>[浏览.../选择文件]</b>选择成交记录的excel文件,点击<b>[上传]</b>,系统会自动导入成交记录.<br />
                            4.导入之后您可以点击<b>[分析好友关系]</b>, 根据同一订单号的不同人,自动生成好友关系.
                            </div>
                            <div align="right" style="float: right">
                                <table >
                                    <tr>
                                        <td>
                                            <asp:FileUpload ID="FileUpload1" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="上传" />
                                            
                                            <asp:Button ID="btnGetBuddy" runat="server" OnClick="btnGetBuddy_Click" Text="分析好友关系" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        
                    </div>
                    <div id="Grid">
                        <cc1:PagingGridView ID="gvData" runat="server" AutoGenerateColumns="False" CssClass="tableBorder"
                            HeaderStyle-CssClass="Head" RowStyle-CssClass="Row" DataKeyNames="" FooterStyle-CssClass="Foot"
                            Width="100%" ShowFooter="True" VirtualItemCount="-1" AllowPaging="True" OnPageIndexChanging="gvData_PageIndexChanging"
                            OnRowDataBound="gvData_RowDataBound" OnRowDeleting="gvData_RowDeleting" OrderBy=""
                            AllowSorting="True" OnSorting="gvData_Sorting">
                            <Columns>
                                <asp:BoundField DataField="MDate" SortExpression="MDate" HeaderText="<%$ Resources:re, lblACTBeginDate%>"
                                    DataFormatString="<%$ Resources:re, DateFormatString%>" ItemStyle-Width="10%" />
                                <asp:BoundField DataField="JoinPerson" SortExpression="JoinPerson" HeaderText="<%$ Resources:re, lblACTUser%>"
                                    ItemStyle-Width="10%" />
                                <asp:BoundField DataField="Type" SortExpression="Type" HeaderText="<%$ Resources:re, lblACTType%>"
                                    ItemStyle-Width="10%" />
                                <asp:BoundField DataField="custname" SortExpression="custname" HeaderText="<%$ Resources:re, lblCustomer%>"
                                    ItemStyle-Width="10%" />
                                <asp:BoundField DataField="ACTContent" SortExpression="ACTContent" HeaderText="<%$ Resources:re, lblACTDesc%>"
                                    ItemStyle-Width="60%" />
                            </Columns>
                        </cc1:PagingGridView>
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
