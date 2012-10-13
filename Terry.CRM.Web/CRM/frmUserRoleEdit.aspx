<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmUserRoleEdit.aspx.cs" 
Inherits="Terry.CRM.Web.CRM.frmUserRoleEdit"   MasterPageFile="~/MasterPage/Site.Master"%>
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
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblUserRole%>" />
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div class="DataDetailFrom">
                            <div class="DetailDt">
                                <table width="100%">
								<tr>
				                    <td><asp:Label ID="lblID" runat="server" Text="<%$ Resources:re, lblID %>"></asp:Label>
                        :</td>
                    <td><asp:TextBox ID="txtID" runat="server" tip="<%$ Resources:re, MsgID%>" usage="notempty" Enabled="false"   width="100%">
					
					</asp:TextBox>
                    <td>
                    <td>
                    </td>	
				                    <td><asp:Label ID="lblUserID" runat="server" Text="<%$ Resources:re, lblUserID %>"></asp:Label>
                        :</td>
                    <td><asp:DropDownList ID="txtUserID" runat="server" tip="<%$ Resources:re, MsgUserID%>" usage="notempty"    width="100%">
					
					</asp:DropDownList>
                    <td>
                    <td>
                    </td>	
				                    <td><asp:Label ID="lblRoleID" runat="server" Text="<%$ Resources:re, lblRoleID %>"></asp:Label>
                        :</td>
                    <td><asp:DropDownList ID="txtRoleID" runat="server" tip="<%$ Resources:re, MsgRoleID%>" usage="notempty"    width="100%">
					
					</asp:DropDownList>
                    <td>
                    <td>
                    </td>	
												

		                        </tr></table>
                                <div id="divErrorMessage">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearer">
                        <!--  -->
                    </div>
                    <div class="button">
                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:re, lblSave%>" 
                            check="true" onclick="btnSave_Click" />
                        <asp:Button ID="btnDel" runat="server" Text="<%$ Resources:re, lblDelete%>" 
                            onclick="btnDel_Click" />
                        <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:re, lblBack%>" 
                            onclick="btnBack_Click" />
					</div>
                </div>
            </td>
        </tr>
    </table>
	    <asp:HiddenField ID="hidID" runat="server" Value="" />
</asp:Content>
				
		