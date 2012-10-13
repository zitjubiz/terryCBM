<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmProductEdit.aspx.cs"
    Inherits="Terry.CRM.Web.CRM.frmProductEdit" MasterPageFile="~/MasterPage/Site.Master" %>

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
                            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblProduct%>" />
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div class="DataDetailFrom">
                            <div class="DetailDt">
                                <table width="100%">
                                    <tr style="display:none;">
                                        <td>
                                            <asp:Label ID="lblProdID" runat="server" Text="<%$ Resources:re, lblID %>"></asp:Label>
                                            :
                                        </td>
                                        <td colspan=3>
                                            <asp:TextBox ID="txtProdID" runat="server" tip="<%$ Resources:re, MsgProdID%>" usage="notempty"
                                                Enabled="false">
					
                                            </asp:TextBox>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td >
                                            <asp:Label ID="lblCode" runat="server" Text="<%$ Resources:re, lblProductCode %>"></asp:Label>
                                            :
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtCode" runat="server" usage="notempty" Width="250px">
                                            </asp:TextBox>
                                        </td>
                                        <td >
                                            <asp:Label ID="lblProductFactor" runat="server" Text="<%$ Resources:re, lblProductFactor %>"></asp:Label>
                                            :
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtProductFactor" runat="server" usage="num+" Width="250px">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span  />
                                            <asp:Label ID="lblProduct" runat="server" Text="<%$ Resources:re, lblProduct %>"></asp:Label>
                                        </td>
                                        <td>
                                            <span />
                                            <asp:TextBox ID="txtProduct" runat="server" tip="<%$ Resources:re, MsgProduct%>" Width="250px"
                                                usage="notempty">					
                                            </asp:TextBox>
                                        </td>
                                       <td>
                                            
                                            <asp:Label ID="lblFullName" runat="server" Text="<%$ Resources:re, lblProductDesc %>"></asp:Label>
                                        </td>
                                        <td>
                                            
                                            <asp:TextBox ID="txtProductFullName" runat="server" usage="notempty" Width="250px">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        
                                        
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
                        <asp:Button ID="btnPrev" runat="server" Text="<%$ Resources:re, lblPrevious%>" OnClientClick="return goPrev();"  />
                        <asp:Button ID="btnNext" runat="server" Text="<%$ Resources:re, lblNext%>" OnClientClick="return goNext();" />
                        <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:re, lblBack%>" OnClick="btnBack_Click" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidID" runat="server" Value="" />
    <script type="text/javascript">
        function goPrev() {
            var prevId =<% =int.Parse(Request["id"])-1 %>;
            if(prevId>=0)
                window.location = 'frmProductEdit.aspx?id='+prevId;
            return false;
        }
        function goNext() {
            window.location = 'frmProductEdit.aspx?id=<% =int.Parse(Request["id"])+1 %>';
            return false;
        }
    </script>
</asp:Content>
