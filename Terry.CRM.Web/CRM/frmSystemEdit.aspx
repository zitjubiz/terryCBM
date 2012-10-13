<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmSystemEdit.aspx.cs"
    Inherits="Terry.CRM.Web.CRM.frmSystemEdit" MasterPageFile="~/MasterPage/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
    <table id="wrp" cellpadding="0" cellspacing="0" align="center">
        <tr id="wrp_base">
            <td valign="top">
                <div id="wrapper">
                    <div id="main_content" class="content" style="width: 100%">
                        <div id="navbar">
                            <span id="currentModule" />
                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:re, lblHome%>" />
                            &gt;
                            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblSystem%>" />
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div class="DataDetailFrom">
                            <div class="DetailDt">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSYSID" runat="server" Text="<%$ Resources:re, lblSYSID %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSYSID" runat="server" tip="<%$ Resources:re, MsgSYSID%>" usage="notempty"
                                                Width="100%">
					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSYSName" runat="server" Text="<%$ Resources:re, lblSYSName %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSYSName" runat="server" tip="<%$ Resources:re, MsgSYSName%>"
                                                usage="notempty" Width="100%">
					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSYSWeb" runat="server" Text="<%$ Resources:re, lblSYSWeb %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSYSWeb" runat="server" tip="<%$ Resources:re, MsgSYSWeb%>" usage=""
                                                Width="100%">
					
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSYSContact" runat="server" Text="<%$ Resources:re, lblSYSContact %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSYSContact" runat="server" tip="<%$ Resources:re, MsgSYSContact%>"
                                                usage="" Width="100%">
					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSYSContactTel" runat="server" Text="<%$ Resources:re, lblSYSContactTel %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSYSContactTel" runat="server" tip="<%$ Resources:re, MsgSYSContactTel%>"
                                                usage="" Width="100%">
					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSYSCDate" runat="server" Text="<%$ Resources:re, lblSYSCDate %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSYSCDate" runat="server" tip="<%$ Resources:re, MsgSYSCDate%>"
                                                usage="" onClick="WdatePicker({dateFmt:'yyyy-MM-dd '})" Width="100%">
					
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSYSBeginDate" runat="server" Text="<%$ Resources:re, lblSYSBeginDate %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSYSBeginDate" runat="server" tip="<%$ Resources:re, MsgSYSBeginDate%>"
                                                usage="" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" Width="100%">
					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSYSExpiryDate" runat="server" Text="<%$ Resources:re, lblSYSExpiryDate %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSYSExpiryDate" runat="server" tip="<%$ Resources:re, MsgSYSExpiryDate%>"
                                                usage="" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" Width="100%">
					
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
                        <asp:Button ID="btnDel" runat="server"  Visible="false" Text="<%$ Resources:re, lblDelete%>" OnClick="btnDel_Click" />
                        <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:re, lblBack%>" OnClick="btnBack_Click" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidID" runat="server" Value="" />
</asp:Content>
