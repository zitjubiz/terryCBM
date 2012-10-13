<%@ Register TagPrefix="Toolbar" TagName="HelpLogout" Src="Toolbars/HelpLogoutToolbar.ascx" %>
<%@ Register TagPrefix="Toolbar" TagName="Database" Src="Toolbars/DatabaseToolbar.ascx" %>
<%@ Register TagPrefix="Toolbar" TagName="Server" Src="Toolbars/ServerToolbar.ascx" %>
<%@ Register TagPrefix="Location" TagName="Server" Src="Toolbars/ServerLocation.ascx" %>
<%@ Register TagPrefix="Location" TagName="Database" Src="Toolbars/DatabaseLocation.ascx" %>
<%@ Register TagPrefix="Location" TagName="Table" Src="Toolbars/TableLocation.ascx" %>
<%@ Page language="c#" Codebehind="Columns.aspx.cs" AutoEventWireup="True" Inherits="SqlWebAdmin.edittable" Trace="false" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Web Data Administrator - Table Columns</title>
		<link rel="shortcut icon" href="favicon.ico">
			<link rel="stylesheet" type="text/css" href="admin.css">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<FORM id="Tables" method="post" runat="server">
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
									<Toolbar:HelpLogout Runat="server" id="HelpLogout" HelpTopic="columns"></Toolbar:HelpLogout>
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
									<Location:Table Runat="Server" id="TableLocation"></Location:Table>
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
						<Toolbar:Database Runat="server" ID="DatabaseToolbar"></Toolbar:Database>
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
												数据库字段名
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
															<asp:HyperLink Runat="server" CssClass="createLink" ID="AddNewColumnHyperLink">
																<img src="images/new.gif" width="16" height="16" alt="" border="0">
																<span style="position:relative; top: -3px;">建立新列</span>
															</asp:HyperLink>
														</td>
													</tr>
												</table>
												<br>
												<asp:datagrid id="ColumnsDataGrid" runat="server" Border="0" AutoGenerateColumns="False" GridLines="None"
													Width="100%" CellPadding="4" CellSpacing="1">
													<ItemStyle CssClass="tableItems"></ItemStyle>
													<HeaderStyle CssClass="tableHeader"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn HeaderText="主键">
															<HeaderStyle Wrap="False"></HeaderStyle>
															<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:Image Visible='<%# ((bool)DataBinder.Eval(Container.DataItem, "key")) %>' runat="server" ImageUrl="images/key.gif" ID="Image1">
																</asp:Image>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="ID">
															<HeaderStyle Wrap="False"></HeaderStyle>
															<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:Image Visible='<%# ((bool)DataBinder.Eval(Container.DataItem, "id")) %>' runat="server" ImageUrl="images/checkmark.gif" ID="Image2">
																</asp:Image>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="name" HeaderText="名" DataFormatString="{0}">
															<HeaderStyle Wrap="False"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
														</asp:BoundColumn>
														<asp:TemplateColumn HeaderText="名">
															<HeaderStyle Wrap="False"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
															<ItemTemplate>
																<asp:HyperLink id="Hyperlink1" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "name") %>' cssclass="databaseListBlack" NavigateUrl='<%# String.Format("editcolumn.aspx?database={0}&table={1}&column={2}", Server.UrlEncode(Request["database"]), Server.UrlEncode(Request["table"]), DataBinder.Eval(Container.DataItem, "encodedname")) %>'>
																</asp:HyperLink>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="datatype" HeaderText="类型" DataFormatString="{0}">
															<HeaderStyle Wrap="False"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
														</asp:BoundColumn>
														<asp:TemplateColumn HeaderText="大小">
															<HeaderStyle Wrap="False"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
															<ItemTemplate>
																<%# DataBinder.Eval(Container.DataItem, "size") %>
																<%# ((((int)DataBinder.Eval(Container.DataItem, "precision")) != 0) &&
                                                                    (((int)DataBinder.Eval(Container.DataItem, "scale")) != 0)) ?
                                                                    String.Format("({0}, {1})", (int)DataBinder.Eval(Container.DataItem, "precision"), (int)DataBinder.Eval(Container.DataItem, "scale")) : "" %>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="空">
															<HeaderStyle Wrap="False"></HeaderStyle>
															<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:Image runat="server" ImageUrl='<%# String.Format("images/{0}.gif", ((bool)DataBinder.Eval(Container.DataItem, "nulls")) ? "checkmark" : "checknomark") %>' ID="Image3">
																</asp:Image>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="default" HeaderText="默认" DataFormatString="{0}">
															<HeaderStyle Wrap="False"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
														</asp:BoundColumn>
														<asp:TemplateColumn HeaderText="编辑">
															<HeaderStyle Wrap="False" HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
															<ItemTemplate>
																<asp:HyperLink id=EditColumn runat="server" NavigateUrl='<%# String.Format("editcolumn.aspx?database={0}&amp;table={1}&amp;column={2}", Server.UrlEncode(Request["database"]), Server.UrlEncode(Request["table"]), DataBinder.Eval(Container.DataItem, "encodedname")) %>' cssclass="databaseListAction" text="edit">编辑</asp:HyperLink>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="删除">
															<HeaderStyle Wrap="False" HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
															<ItemTemplate>
																<asp:HyperLink id=DeleteColumn runat="server" NavigateUrl='<%# String.Format("deletecolumn.aspx?database={0}&amp;table={1}&amp;column={2}", Server.UrlEncode(Request["database"]), Server.UrlEncode(Request["table"]), DataBinder.Eval(Container.DataItem, "encodedname")) %>' cssclass="databaseListAction" text="delete">删除</asp:HyperLink>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
												</asp:datagrid>
												<asp:label id="NoColumnsLabel" runat="server" EnableViewState="False" Font-Bold="true" Font-Size="10">There are no columns to display.</asp:label>
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
