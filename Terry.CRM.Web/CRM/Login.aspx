
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Terry.CRM.Web.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login Page</title>
    <script language="javascript" type="text/javascript" src="Js/checkform.js"></script>
    <script language="javascript" type="text/javascript" src="Js/jquery-1.3.2.min.js"></script>
    <script language="javascript" type="text/javascript" src="Js/jquery.alerts.js"></script>
    <!-- Example script -->
    <script type="text/javascript">

        $(document).ready(function () {
            $("#txtloginid").focus();
            if ($("#lblJScript").text() != "")
                jAlert($("#lblJScript").text(), '<%=strAlert%>');
        }
			);
			
    </script>
    <link href="css/jquery.alerts.css" rel="stylesheet" type="text/css" media="screen" />
    <link href="css/lightBlue.css" rel="stylesheet" type="text/css" />
</head>
<body>
<!--[if lte IE 6]><script src="js/ie6warning.js"></script><script>window.onload=function(){e("../images/")}</script><![endif]-->
    <form id="form1" runat="server">
    <div>
        <table width="800" border="0" cellpadding="0" cellspacing="0" align="center">
            <tr>
                <td valign="top">
                </td>
                <td bgcolor="#ffffff" height="30">
                    &nbsp;
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td valign="top">
                
                    <%--<img src="images/2sw.jpg" alt="2simplework" border="0" />--%>
                </td>
                <td>
                    <img id="imgLogo" runat="server" src="images/logo.png" alt="Logo" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    &nbsp;</td>
                <td bgcolor="#ffffff">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td style="width: 155px; height: 300px;" valign="top">
                                <br />
                                <br />
                            </td>
                            <td width="10" style="height: 42px">
                                &nbsp;
                            </td>
                            <td width="635" valign="top" align="center" style="height: 42px">
                                <!--main content table-->
                                <br />
                                <br />
                                <table id="Table4" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td style="width: 25%; height: 31px;" align="right">
                                            <asp:Label ID="lblLoginUser" runat="server" Text="<%$ Resources:re, lblLoginUser%>"></asp:Label>
                                            ：
                                        </td>
                                        <td align="left" colspan="2" valign="middle" style="height: 31px">
                                            <asp:TextBox ID="txtloginid" runat="server" Width="100px" usage="notempty" tip="<%$ Resources:re, MsgLoginUser%>"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="25" style="width: 25%" align="right">
                                            <asp:Label ID="lblPassword" runat="server" Text="<%$ Resources:re, lblPassword%>"></asp:Label>
                                            ：
                                        </td>
                                        <td align="left" colspan="2" valign="middle">
                                            <asp:TextBox ID="txtpwd" runat="server" TextMode="password" Width="100px" usage="notempty"
                                                tip="<%$ Resources:re, MsgPassword%>"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td height="25" style="width: 25%" align="right">
                                            <asp:Label ID="lblLanguage" runat="server" Text="<%$ Resources:re, lblLanguage%>"></asp:Label>：
                                        </td>
                                        <td align="left" colspan="2" valign="middle">
                                            <asp:DropDownList ID="ddlLanguage" runat="server" Width="105px" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged">
                                                <asp:ListItem Text="<%$ Resources:re, lblEnglish%>" Value="en-US"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:re, lblSimpChinese%>" Value="zh-CN"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:re, lblTradChinese%>" Value="zh-TW"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="35" style="width: 25%" align="right">
                                            &nbsp;
                                        </td>
                                        <td align="left" colspan="2" valign="middle">
                                            <asp:ImageButton ID="btnLogin" runat="server" ImageUrl="images/login.gif" BorderWidth="0"
                                                check="true" OnClick="btnLogin_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%">
                                            &nbsp;
                                        </td>
                                        <td valign="middle" align="left" height="45" style="width: 44%">
                                            <div id="divErrorMessage">
                                            </div>
                                        </td>
                                        <td width="27%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="bottom">
                    &nbsp;</td>
            </tr>
        </table>
    </div>
    <!-- IE Exception: unable to modify the parent container element --->
    <asp:Label runat="server" ID="lblJScript" Style="display: none;"></asp:Label>
    <div align="center">
                <asp:Label ID="lblCompany" runat="server" Text="" /><br />
                <asp:Label ID="lblServerID0" runat="server" Text="Server ID:" />
                <asp:Label ID="lblServerID" runat="server" Text="" /><br />
                <asp:Label ID="lblIllegal" runat="server" Text="" />
                <br />
                <asp:TextBox ID="txtKey" runat="server" Visible="False" Width="300px"
                onfocus="javascript:this.select()">please input register key</asp:TextBox>
                <asp:Button ID="btnReg" runat="server" onclick="btnReg_Click" Text="注册" 
                    Visible="False" />
                <asp:Button ID="btnLicense" runat="server" onclick="btnReg_Click" Text="更改license数量" 
                    Visible="False" />
    </div>
    </form>
    </body>
</html>
