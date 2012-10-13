<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmProvinceEdit.aspx.cs"
    Inherits="Terry.CRM.Web.CRM.frmProvinceEdit" MasterPageFile="~/MasterPage/Site.Master" %>

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
                            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblCustProvince%>" />
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div class="DataDetailFrom">
                            <div class="DetailDt">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblProvinceID" runat="server" Text="<%$ Resources:re, lblID %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProvinceID" runat="server" tip="<%$ Resources:re, MsgID%>" usage="notempty"
                                                Width="100%">					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblProvince" runat="server" Text="<%$ Resources:re, lblCustProvince %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProvince" runat="server" tip="<%$ Resources:re, MsgCustProvince%>"
                                                usage="notempty" Width="100%">
					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:re, lblRegion %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRegion" runat="server" tip="<%$ Resources:re, MsgCustDistinct%>" usage="notempty"
                                                Width="100%">					
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
                        <asp:Button ID="btnDel" runat="server" Text="<%$ Resources:re, lblDelete%>" OnClick="btnDel_Click"
                            Visible="false" />
                        <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:re, lblBack%>" OnClick="btnBack_Click" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidID" runat="server" Value="" />
</asp:Content>
