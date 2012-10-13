<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCustomerTypeEdit.aspx.cs"
    Inherits="Terry.CRM.Web.CRM.frmCustomerTypeEdit" MasterPageFile="~/MasterPage/Site.Master" %>

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
                            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblCustomerType%>" />
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div class="DataDetailFrom">
                            <div class="DetailDt">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCustTypeID" runat="server" Text="<%$ Resources:re, lblCustTypeID %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCustTypeID" runat="server" tip="<%$ Resources:re, MsgCustTypeID%>"
                                                usage="notempty" Width="100%">
					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCustType" runat="server" Text="<%$ Resources:re, lblCustType %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCustType" runat="server" tip="<%$ Resources:re, MsgCustType%>"
                                                usage="notempty" Width="100%">
					
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
                        <asp:Button ID="btnDel" runat="server" Text="<%$ Resources:re, lblDelete%>" OnClick="btnDel_Click" Visible="false"  />
                        <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:re, lblBack%>" OnClick="btnBack_Click" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidID" runat="server" Value="" />
</asp:Content>
