<%@ Import Namespace="SqlAdmin" %>
<%@ Register TagPrefix="Location" TagName="Database" Src="Toolbars/DatabaseLocation.ascx" %>
<%@ Register TagPrefix="Location" TagName="Server" Src="Toolbars/ServerLocation.ascx" %>
<%@ Register TagPrefix="Toolbar" TagName="Server" Src="Toolbars/ServerToolbar.ascx" %>
<%@ Register TagPrefix="Toolbar" TagName="Database" Src="Toolbars/DatabaseToolbar.ascx" %>
<%@ Register TagPrefix="Toolbar" TagName="HelpLogout" Src="Toolbars/HelpLogoutToolbar.ascx" %>
<%@ Page language="c#" Codebehind="CreateDatabaseRole.aspx.cs" AutoEventWireup="True" Inherits="SqlWebAdmin.CreateDatabaseRole" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Web Data Administrator - Roles</title>
		<link rel="shortcut icon" href="favicon.ico">
			<link rel="stylesheet" type="text/css" href="admin.css">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<FORM id="WebForm1" method="post" runat="server">
			<TABLE width="100%" height="100%" cellSpacing="0" cellPadding="0" border="0">
				<!-- FIRST ROW: HEADER -->
				<tr>
					<td colspan="3" valign="bottom" align="left" width="100%" height="36" bgcolor="#c0c0c0">
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<!--BEGIN ONE LINE-->
								<td valign="bottom" width="308"><img src="images/logo_top.gif" width="308" height="36" alt="" border="0"></td>
								<!--END ONE LINE-->
								<td valign="bottom" align="right" width="100%">
									<Toolbar:HelpLogout Runat="server" id="HelpLogout" HelpTopic="storedprocedures"></Toolbar:HelpLogout>
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
									<Location:Server Runat="Server" id="ServerLocation"></Location:Server>
									<Location:Database Runat="Server" id="DatabaseLocation"></Location:Database>
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
					<td bgcolor="#6699ff" valign="top" align="center" width="172" height="100%">
						<Toolbar:Server Runat="server" id="ServerToolbar"></Toolbar:Server>
						<Toolbar:Database Selected="Roles" Runat="server" ID="DatabaseToolbar"></Toolbar:Database>
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
									<!-- SECTION HEADER: START -->
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td class="databaseListHeader">
												建立数据库角色
											</td>
										</tr>
										<!-- SECTION HEADER: END -->
										<!-- SECTION: START -->
										<tr>
											<!--BEGIN ONE LINE-->
											<td height="3" valign="middle" background="images/blue_dotted_line.gif"><img src="images/blue_dotted_line.gif" width="150" height="3" alt="" border="0"></td>
											<!--END ONE LINE-->
										</tr>
										<tr>
											<!--BEGIN ONE LINE-->
											<td height="4" valign="middle"><img src="images/spacer.gif" width="1" height="4" alt="" border="0"></td>
											<!--END ONE LINE-->
										</tr>
										<tr>
											<td bgcolor="white" class="databaseListItem">
												<table border="0" cellspacing="0" cellpadding="0" width="400">
													<tr>
														<td class="databaseListItem" nowrap>
															角色名:
														</td>
														<td class="databaseListItem">
															<asp:TextBox ID="RoleName" MaxLength="32" Runat="server" />
															<asp:RequiredFieldValidator ControlToValidate="RoleName" ErrorMessage="* Requied" Display="Dynamic" Runat="server"
																id="RequiredFieldValidator1" />
														</td>
													</tr>
													<tr>
														<td class="databaseListItem" valign="top" nowrap>
															角色类型:
														</td>
														<td class="databaseListItem">
															SQL Server supports two types of database roles: standard roles, which contain 
															members, and database roles, which require a password.
															<br>
															<asp:RadioButtonList id="RoleType" Runat="server">
																<asp:ListItem Value="Standard" />
																<asp:ListItem Value="Application" />
															</asp:RadioButtonList>
															<asp:RequiredFieldValidator ControlToValidate="RoleType" ErrorMessage="* Required" Display="Dynamic" Runat="server"
																id="RequiredFieldValidator2" />
														</td>
													</tr>
													<tr>
														<td colspan="2" align="right" class="databaseListItem">
															<asp:Button Text="Create Role" Runat="server" id="Button1" />
														</td>
													</tr>
													<tr>
														<td colspan="2" class="databaseListItem">
															<asp:Label ID="ErrorMessage" Runat="server" />
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<!-- Section END -->
										<!-- Section Footer START -->
									</table>
									<br>
									<!-- Section Footer END -->
									<!-- Page content END -->
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
