<%@ Register TagPrefix="Toolbar" TagName="HelpLogout" Src="Toolbars/HelpLogoutToolbar.ascx" %>
<%@ Page language="c#" Codebehind="Default.aspx.cs" AutoEventWireup="True" Inherits="SqlWebAdmin.Login" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Web Data Administrator - Login</title>
		<link rel="shortcut icon" href="favicon.ico">
			<link rel="stylesheet" type="text/css" href="admin.css">
				<script language="javascript" src="Global.js"></script>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<FORM id="WebForm1" method="post" runat="server">
			<TABLE width="100%" height="100%" cellSpacing="0" cellPadding="0" border="0">
				<!-- FIRST ROW: HEADER -->
				<tr>
					<td colspan="3" valign="bottom" align="left" width="100%" height="49" bgcolor="#c0c0c0">
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<!--BEGIN ONE LINE-->
								<td valign="bottom" width="308"><img src="images/logo_top.gif" width="308" height="36" alt="" border="0"></td>
								<!--END ONE LINE-->
								<td valign="bottom" align="right" width="100%">
									<Toolbar:HelpLogout Runat="server" id="HelpLogout" HelpTopic="login"></Toolbar:HelpLogout>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<!-- FIRST ROW: HEADER -->
				<!-- SECOND ROW: CRUMBS -->
				<tr>
					<!--BEGIN ONE LINE-->
					<td align="left" bgcolor="#99ccff" background="images/blue_back.gif" width="172" height="26"><img src="images/logo_bottom.gif" width="172" height="26" alt="" border="0"></td>
					<!--END ONE LINE-->
					<td align="left" bgColor="#99ccff" background="images/blue_back.gif" width="100%" height="26">
						<table width="100%" height="26" cellSpacing="0" cellPadding="0" border="0" style="TABLE-LAYOUT:fixed">
							<tr>
								<td width="12">
									&nbsp;
								</td>
								<td valign="middle" align="left" width="100%" height="26">
								</td>
							</tr>
						</table>
					</td>
					<!--BEGIN ONE LINE-->
					<td align="left" bgcolor="#99ccff" width="12" height="26"><img src="images/blue_back_right.gif" width="12" height="26" alt="" border="0"></td>
					<!--END ONE LINE-->
				</tr>
				<!-- SECOND ROW: CRUMBS -->
				<!-- THIRD ROW: BOTTOM SECTION -->
				<tr>
					<!-- START NAVIGATION SECTION -->
					<td bgcolor="#6699ff" valign="top" align="center" width="172" height="100%"><FONT face="宋体"></FONT>
					</td>
					<!-- END NAVIGATION SECTION -->
					<!-- START CONTENT SECTION -->
					<td valign="top" align="left">
						<table cellSpacing="0" cellPadding="0" border="0" width="100%">
							<tr>
								<!--BEGIN ONE LINE-->
								<td valign="bottom" colSpan="2" height="8" width="100%"><img src="images/spacer.gif" width="1" height="8" alt="" border="0"></td>
								<!--END ONE LINE-->
							</tr>
							<tr>
								<!--BEGIN ONE LINE-->
								<td align="left" width="12"><img src="images/spacer.gif" width="12" height="1" alt="" border="0"></td>
								<!--END ONE LINE-->
								<td align="left" class="databaseListItem" width="100%">
									<!-- PAGE CONTENT: START -->
									<asp:Label id="LogoutInfoLabel" runat="server" Visible="False">你已退出登录</asp:Label>
									<asp:Label id="LoginInfoLabel" runat="server" Visible="False" Font-Size="10 pt" Font-Bold="true"> SQL Server 2000 WEB 管理工具</asp:Label>
									<br>
									<br>
									<asp:Label id="lblCredMsg" runat="server" Font-Size="8 pt" Visible="False">Please enter your SQL Server credentials:</asp:Label>
									<br>
									<br>
									<table border="0" cellpadding="0" cellspacing="1">
										<tr>
											<td class="databaseListItem">SQL Server用户名</td>
											<td class="databaseListItem" width="50">&nbsp;</td>
											<td class="databaseListItem"><asp:textbox id="UsernameTextBox" runat="server" Columns="35"></asp:textbox></td>
											<td class="databaseListItem"><asp:requiredfieldvalidator id="UsernameRequiredFieldValidator" runat="server" ErrorMessage="必须输入用户名" ControlToValidate="UsernameTextBox"
													Display="Dynamic"></asp:requiredfieldvalidator></td>
										</tr>
										<tr>
											<td class="databaseListItem">用户名登录口令</td>
											<td class="databaseListItem" width="50">&nbsp;</td>
											<td class="databaseListItem"><asp:textbox id="PasswordTextBox" runat="server" Columns="35" TextMode="Password"></asp:textbox></td>
											<td class="databaseListItem"></td>
										</tr>
										<tr>
											<td class="databaseListItem">数据库服务器</td>
											<td class="databaseListItem" width="50">&nbsp;</td>
											<td class="databaseListItem">
												<P><asp:textbox id="ServerTextBox" runat="server" Columns="35"></asp:textbox></P>
												<P><FONT face="宋体">系统默认服务器</FONT></P>
											</td>
											<td class="databaseListItem"><asp:requiredfieldvalidator id="ServerRequiredFieldValidator" runat="server" ControlToValidate="ServerTextBox"
													Display="Dynamic"></asp:requiredfieldvalidator></td>
										</tr>
										<TR valign="top">
											<TD class="databaseListItem">
												<asp:Label id="lblAuth" runat="server" Visible="False">Authentication<br>Method</asp:Label></TD>
											<TD class="databaseListItem" width="50"></TD>
											<TD class="databaseListItem" nowrap>
												<asp:RadioButtonList id="AuthRadioButtonList" runat="server" AutoPostBack="True" Visible="False" onselectedindexchanged="AuthRadioButtonList_SelectedIndexChanged">
													<asp:ListItem Value="windows">Windows Integrated</asp:ListItem>
													<asp:ListItem Value="sql" Selected="True">SQL Login</asp:ListItem>
												</asp:RadioButtonList></TD>
											<TD class="databaseListItem"></TD>
										</TR>
										<tr>
											<td class="databaseListItem" colspan="3" align="right">
												<asp:Button id="LoginButton" CSSClass="button" onMouseOver="this.style.color='#808080';" onMouseOut="this.style.color='#000000';"
													runat="server" Text="登 录" onclick="LoginButton_Click"></asp:Button>
											</td>
											<td class="databaseListItem"></td>
										</tr>
									</table>
									<br>
									<asp:Label id="ErrorLabel" runat="server" Visible="False" ForeColor="red"></asp:Label>
									<!-- PAGE CONTENT: END -->
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<!-- THIRD ROW: BOTTOM SECTION -->
			</TABLE>
		</FORM>
	</body>
</HTML>
