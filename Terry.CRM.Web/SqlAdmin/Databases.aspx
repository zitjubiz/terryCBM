<%@ Register TagPrefix="Toolbar" TagName="Server" Src="Toolbars/ServerToolbar.ascx" %>
<%@ Register TagPrefix="Location" TagName="Server" Src="Toolbars/ServerLocation.ascx" %>
<%@ Register TagPrefix="Toolbar" TagName="HelpLogout" Src="Toolbars/HelpLogoutToolbar.ascx" %>
<%@ Page language="c#" Codebehind="Databases.aspx.cs" AutoEventWireup="True" Inherits="SqlWebAdmin.databases" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Web Data Administrator - Databases</title>
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
									<Toolbar:HelpLogout Runat="server" id="HelpLogout" HelpTopic="databases"></Toolbar:HelpLogout>
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
						<Toolbar:Server Selected="Databases" Runat="server" id="ServerToolbar"></Toolbar:Server>
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
												表
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
												<table width="100%" cellspacing="0" cellpadding="0" border="0">
													<tr>
														<td align="right">
															<asp:HyperLink Runat="server" CssClass="createLink" ID="AddNewDatabaseHyperLink" NavigateUrl="CreateDatabase.aspx">
																<img src="images/new.gif" width="16" height="16" alt="" border="0">
																<span style="position:relative; top: -3px;">建立新数据库</span>
															</asp:HyperLink>
														</td>
													</tr>
												</table>
												<br>
												<asp:datagrid id="DatabasesDataGrid" runat="server" GridLines="None" Border="0" AutoGenerateColumns="False"
													Width="100%" CellPadding="4" CellSpacing="1">
													<ItemStyle CssClass="tableItems"></ItemStyle>
													<HeaderStyle CssClass="tableHeader"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn HeaderText="数据库名">
															<HeaderStyle Wrap="False"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
															<ItemTemplate>
																<A href='<%# String.Format("tables.aspx?database={0}", DataBinder.Eval(Container.DataItem, "encodedname")) %>'>
																	<IMG src="images/db_ico.gif" align="absMiddle" border="0"></A> &nbsp;
																<asp:HyperLink id=Hyperlink1 runat="server" NavigateUrl='<%# String.Format("tables.aspx?database={0}", DataBinder.Eval(Container.DataItem, "encodedname")) %>' cssclass="databaseListBlack" text='<%# "&amp;nbsp;" + DataBinder.Eval(Container.DataItem, "name") %>'>
																</asp:HyperLink>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="size" HeaderText="大小" DataFormatString="{0}">
															<HeaderStyle Wrap="False" HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
														</asp:BoundColumn>
														<asp:TemplateColumn HeaderText="编辑">
															<HeaderStyle Wrap="False" HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
															<ItemTemplate>
																<asp:HyperLink id=EditDatabase runat="server" NavigateUrl='<%# String.Format("tables.aspx?database={0}", DataBinder.Eval(Container.DataItem, "encodedname")) %>' cssclass="databaseListAction" text="edit">编辑</asp:HyperLink>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="查询">
															<HeaderStyle Wrap="False" HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
															<ItemTemplate>
																<asp:HyperLink id=QueryDatabase runat="server" NavigateUrl='<%# String.Format("querydatabase.aspx?database={0}", DataBinder.Eval(Container.DataItem, "encodedname")) %>' cssclass="databaseListAction" text="query">查询</asp:HyperLink>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="删除">
															<HeaderStyle Wrap="False" HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
															<ItemTemplate>
																<asp:HyperLink id=DeleteDatabase runat="server" NavigateUrl='<%# String.Format("deletedatabase.aspx?database={0}", DataBinder.Eval(Container.DataItem, "encodedname")) %>' cssclass="databaseListAction" text="delete">删除</asp:HyperLink>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
												</asp:datagrid>
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
