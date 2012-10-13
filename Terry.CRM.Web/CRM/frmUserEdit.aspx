<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmUserEdit.aspx.cs" Inherits="Terry.CRM.Web.CRM.frmUserEdit"
    MasterPageFile="~/MasterPage/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
    <table id="wrp" cellpadding="0" cellspacing="0" align="center" style="height: 460px">
        <tr id="wrp_base">
            <td valign="top">
                <div id="wrapper">
                    <div id="main_content" class="content" style="width: 100%">
                        <div id="navbar">
                            <span id="currentModule" />
                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:re, lblHome%>" />
                            &gt;
                            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblUser%>" />
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div class="DataDetailFrom">
                            <div class="DetailDt">
                                <table width="98%" cellpadding="2" cellspacing="2">
                                    <tr>
                                        <td width="10%">
                                            <asp:Label ID="lblUserName" runat="server" Text="<%$ Resources:re, lblUserName %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUserName" runat="server" tip="<%$ Resources:re, MsgUserName%>"
                                                usage="notempty" Width="150px">
					
                                            </asp:TextBox>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="lblUserFullName" runat="server" Text="<%$ Resources:re, lblUserFullName %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUserFullName" runat="server" tip="<%$ Resources:re, MsgUserFullName%>"
                                                usage="notempty" Width="150px" AutoComplete="off">
					
                                            </asp:TextBox>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="lblPassword" runat="server" Text="<%$ Resources:re, lblPassword %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPassword" runat="server" tip="<%$ Resources:re, MsgPassword%>"
                                                usage="notempty" Width="150px" TextMode="Password" AutoComplete="off">
					
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblEmail" runat="server" Text="<%$ Resources:re, lblEmail %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmail" runat="server"  Width="150px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMobile" runat="server" Text="<%$ Resources:re, lblMobile %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMobile" runat="server" tip="<%$ Resources:re, MsgMobile%>" usage="notempty"
                                                Width="150px">
					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblIsActive" runat="server" Text="<%$ Resources:re, lblIsActive %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="txtIsActive" runat="server" tip="<%$ Resources:re, MsgIsActive%>"
                                                usage="notempty" Width="150px">
                                                <asp:ListItem Value="true">是</asp:ListItem>
                                                <asp:ListItem Value="false">否</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDept" runat="server" Text="<%$ Resources:re, lblDepartment %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="txtDept" runat="server" tip="<%$ Resources:re, MsgDepName%>"
                                                usage="" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBossID" runat="server" Text="<%$ Resources:re, lblBossID %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="txtBossID" runat="server" tip="<%$ Resources:re, MsgBossID%>"
                                                usage="" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRole" runat="server" Text="<%$ Resources:re, lblRole %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="txtRole" runat="server" tip="<%$ Resources:re, MsgRole%>" usage="notempty"
                                                Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCDate" runat="server" Text="<%$ Resources:re, lblCDate %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCDate" runat="server" tip="<%$ Resources:re, MsgCDate%>" usage="notempty"
                                                onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" Width="150px" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCUser" runat="server" Text="<%$ Resources:re, lblCUser %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="txtCUserID" runat="server" />
                                            <asp:TextBox ID="txtCUser" runat="server" tip="<%$ Resources:re, MsgCUser%>" usage="notempty"
                                                Width="150px" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                        <td style="display: none">
                                            <asp:Label ID="lblSYSID" runat="server" Text="<%$ Resources:re, lblSYSID %>"></asp:Label>
                                            :
                                        </td>
                                        <td style="display: none">
                                            <asp:DropDownList ID="txtSYSID" runat="server" tip="<%$ Resources:re, MsgSYSID%>"
                                                usage="notempty" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblModifyDate" runat="server" Text="<%$ Resources:re, lblModifyDate %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtModifyDate" runat="server" tip="<%$ Resources:re, MsgModifyDate%>"
                                                usage="notempty" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" Width="150px"
                                                Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblModifyUser" runat="server" Text="<%$ Resources:re, lblModifyUser %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="txtModifyUserID" runat="server" />
                                            <asp:TextBox ID="txtModifyUser" runat="server" tip="<%$ Resources:re, MsgModifyUser%>"
                                                usage="notempty" Width="150px" Enabled="false">
                                            </asp:TextBox>
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
                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:re, lblSave%>" check="true"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnDel" runat="server" Text="<%$ Resources:re, lblStop%>" OnClick="btnDel_Click" />
                        <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:re, lblBack%>" OnClick="btnBack_Click" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidID" runat="server" Value="" />
    <asp:HiddenField ID="hidDelMsg" runat="server" Value="<%$ Resources:re, MsgDeleteConfirm%>" />
    <asp:HiddenField ID="hidBtnDel" runat="server" Value="" />
</asp:Content>
