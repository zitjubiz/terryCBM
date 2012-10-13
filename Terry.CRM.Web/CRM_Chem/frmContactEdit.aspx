<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmContactEdit.aspx.cs"
    Inherits="Terry.CRM.Web.CRM.frmContactEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/lightBlue.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.alerts.css" rel="stylesheet" type="text/css" media="screen" />
    
    <script type="text/javascript" src="../js/checkform.js"></script>
    <script type="text/javascript" src="../js/jquery-1.3.2.min.js"></script>
    <script type="text/javascript" src="../js/jquery.alerts.js"></script>

</head>
<body>
    <form id="form1" runat="server">

            <div style="width: 480px">
                <table width="480px">
                    <tr>
                        <td width="120">
                            <asp:Label ID="lblContactName" runat="server" Text="<%$ Resources:re, lblContactName %>"></asp:Label>
                            :
                        </td>
                        <td width="120">
                            <asp:TextBox ID="txtContactName" runat="server" tip="<%$ Resources:re, MsgContactName%>"
                                usage="notempty" Width="100%">
					
                            </asp:TextBox>
                        </td>
                        <td width="120">
                            <asp:Label ID="lblContactSex" runat="server" Text="<%$ Resources:re, lblContactSex %>"></asp:Label>
                            :
                        </td>
                        <td width="120">
                            <asp:DropDownList ID="txtContactSex" runat="server" tip="<%$ Resources:re, MsgContactSex%>"
                                usage="notempty" Width="100%">
                                <asp:ListItem Value="True">男</asp:ListItem>
                                <asp:ListItem Value="False">女</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblContactTel" runat="server" Text="<%$ Resources:re, lblContactTel %>"></asp:Label>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtContactTel" runat="server" tip="<%$ Resources:re, MsgContactTel%>"
                                usage="" Width="100%">
					
                            </asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblContactFax" runat="server" Text="<%$ Resources:re, lblContactFax %>"></asp:Label>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtContactFax" runat="server" tip="<%$ Resources:re, MsgContactFax%>"
                                usage="" Width="100%">
					
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblContactMobile" runat="server" Text="<%$ Resources:re, lblContactMobile %>"></asp:Label>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtContactMobile" runat="server" tip="<%$ Resources:re, MsgContactMobile%>"
                                usage="" Width="100%">
					
                            </asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblContactEmail" runat="server" Text="<%$ Resources:re, lblContactEmail %>"></asp:Label>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtContactEmail" runat="server" tip="<%$ Resources:re, MsgContactEmail%>"
                                usage="" Width="100%">
					
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblContactMSN" runat="server" Text="<%$ Resources:re, lblContactMSN %>"></asp:Label>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtContactMSN" runat="server" tip="<%$ Resources:re, MsgContactMSN%>"
                                usage="" Width="100%">
					
                            </asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblContactQQ" runat="server" Text="<%$ Resources:re, lblContactQQ %>"></asp:Label>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtContactQQ" runat="server" tip="<%$ Resources:re, MsgContactQQ%>"
                                usage="" Width="100%">
					
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td>
                            <asp:Label ID="lblContactDept" runat="server" Text="<%$ Resources:re, lblContactDept %>"></asp:Label>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtContactDept" runat="server" tip="<%$ Resources:re, MsgContactDept%>"
                                usage="" Width="100%">
					
                            </asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblContactTitle" runat="server" Text="<%$ Resources:re, lblContactTitle %>"></asp:Label>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtContactTitle" runat="server" tip="<%$ Resources:re, MsgContactTitle%>"
                                usage="" Width="100%">
					
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td>
                            <asp:Label ID="lblContactCustID" runat="server" Text="<%$ Resources:re, lblContactCustID %>"></asp:Label>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="txtContactCustID" runat="server" tip="<%$ Resources:re, MsgContactCustID%>"
                                usage="notempty" Width="100%">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblContactTypeID" runat="server" Text="<%$ Resources:re, lblContactTypeID %>"></asp:Label>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="txtContactTypeID" runat="server" tip="<%$ Resources:re, MsgContactTypeID%>"
                                usage="" Width="100%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <div id="divErrorMessage">
                
                </div>
                <div class="clearer">
                    <!--  -->
                </div>
                <div class="button">
                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:re, lblSave%>" check="true"
                        OnClick="btnSave_Click" />
                    <asp:Button ID="btnDel" Visible="false" runat="server" Text="<%$ Resources:re, lblDelete%>"
                        OnClick="btnDel_Click" />
                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:re, lblBack%>" OnClick="btnBack_Click" />
                    <asp:HiddenField ID="hidID" runat="server" Value="" />
                    <asp:HiddenField ID="hidSaveMsg" runat="server" Value="<%$ Resources:re, MsgSaveConfirm%>" />
                    <asp:HiddenField ID="hidDelMsg" runat="server" Value="<%$ Resources:re, MsgDeleteConfirm%>" />
                    <asp:HiddenField ID="hidBtnDel" runat="server" Value="" />
                    <asp:TextBox ID="txtContactID" runat="server" usage="notempty" Visible="false" Width="100%" />
                </div>
            </div>

    </form>
</body>
</html>
