<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDepartmentEdit.aspx.cs"
    Inherits="Terry.CRM.Web.CRM.frmDepartmentEdit" MasterPageFile="~/MasterPage/Site.Master" %>

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
                            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblDepartment%>" />
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div class="DataDetailFrom">
                            <div class="DetailDt">
                                <table width="98%">
                                    <tr>
                                        <td style="display:none">
                                            <asp:Label ID="lblDepID" runat="server" Text="<%$ Resources:re, lblID %>"></asp:Label>
                                            :
                                        </td>
                                        <td style="display:none">
                                            <asp:TextBox ID="txtDepID" runat="server" tip="<%$ Resources:re, MsgID%>" usage="notempty"
                                                Width="100px" Enabled="false">
					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDepName" runat="server" Text="<%$ Resources:re, lblDepName %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDepName" runat="server" tip="<%$ Resources:re, MsgDepName%>"
                                                usage="notempty" Width="200px">
					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDepAddress" runat="server" Text="<%$ Resources:re, lblAddress %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDepAddress" runat="server" tip="<%$ Resources:re, MsgAddress%>"
                                                usage="notempty" Width="400px">
					
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
                        <asp:Button ID="btnDel" runat="server" Text="<%$ Resources:re, lblDelete%>" OnClick="btnDel_Click" />
                        <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:re, lblBack%>" OnClick="btnBack_Click" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidID" runat="server" Value="" />
</asp:Content>
