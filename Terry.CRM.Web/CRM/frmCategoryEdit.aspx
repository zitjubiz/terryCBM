<%@ Page Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true"
    CodeBehind="frmCategoryEdit.aspx.cs" Inherits="Terry.CRM.Web.CRM.frmCategoryEdit"
    Title="Category Edit" %>

<%@ Register Assembly="DropDownCheckList" Namespace="Terry.Web.Control" TagPrefix="cc2" %>
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
                            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblCategory%>" />
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div class="DataDetailFrom">
                            <div class="DetailDt">
                                <table width="100%" border="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCategory" runat="server" Text="<%$ Resources:re, lblCategory %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCategory" runat="server" tip="<%$ Resources:re, MsgCategory%>"
                                                usage="notempty">					
                                            </asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblProduct" runat="server" Text="<%$ Resources:re, lblProduct %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <cc2:DropDownCheckList ID="DDCLProduct" runat="server" ClientCodeLocation="../js/DropDownCheckList.js"
                                                DropImageSrc="../images/expand.gif" RepeatColumns="2" DisplayTextWidth="400"
                                                Width="100%" TextWhenNoneChecked="-----Select-----" CheckListCssClass="checkbox">
                                            </cc2:DropDownCheckList>
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
    <asp:TextBox ID="txtCategoryID" runat="server" tip="<%$ Resources:re, MsgID%>" usage="notempty"
        Width="10" Enabled="false" Style="display: none;" />
</asp:Content>
