<%@ Control Language="c#" AutoEventWireup="True" Codebehind="DatabaseToolbar.ascx.cs" Inherits="SqlWebAdmin.DatabaseToolbar" %>
<table cellSpacing="0" cellPadding="0" width="148" border="0" style="TABLE-LAYOUT: fixed">
	<tr>
		<td height="12">
			&nbsp;
		</td>
	</tr>
	<!-- SECTION HEADING: START -->
	<tr>
		<td class="menuHeader">
			���ݿ�
		</td>
	</tr>
	<!-- SECTION HEADING: END -->
	<!-- SECTION ITEM: START -->
	<tr>
		<!--BEGIN ONE LINE-->
		<td height="3" valign="middle"><img src="images/blue_dotted_line.gif" width="150" height="3" alt="" border="0"></td>
		<!--END ONE LINE-->
	</tr>
	<tr>
		<td id="TablesTd" height="20" runat="server">
			<asp:HyperLink id="TablesHyperLink" CssClass="leftMenu" onMouseOver="this.style.color='#ffffff';"
				onMouseOut="this.style.color='#000000';" runat="server"><img src="images/table_ico.gif" align="Absmiddle" border="0">&nbsp;��</asp:HyperLink>
		</td>
	</tr>
	<!-- SECTION ITEM: END -->
	<!-- SECTION ITEM: START -->
	<tr>
		<td id="StoredProceduresTd" height="20" runat="server">
			<asp:HyperLink id="StoredProceduresHyperLink" CssClass="leftMenu" onMouseOver="this.style.color='#ffffff';"
				onMouseOut="this.style.color='#000000';" runat="server"><img src="images/sproc_ico.gif" align="Absmiddle" border="0"> &nbsp;�洢����</asp:HyperLink>
		</td>
	</tr>
	<!-- SECTION ITEM: END -->
	<!-- SECTION ITEM: START -->
	<tr>
		<td id="QueryTd" height="20" runat="server">
			<asp:HyperLink id="QueryHyperLink" CssClass="leftMenu" onMouseOver="this.style.color='#ffffff';"
				onMouseOut="this.style.color='#000000';" runat="server"><img src="images/query_ico.gif" align="Absmiddle" border="0">&nbsp;ִ��SQL���</asp:HyperLink>
		</td>
	</tr>
	<!-- SECTION ITEM: END -->
	<!-- SECTION ITEM: START -->
	<tr>
		<td id="PropertiesTd" height="20" runat="server">
			<asp:HyperLink id="PropertiesHyperLink" CssClass="leftMenu" onMouseOver="this.style.color='#ffffff';"
				onMouseOut="this.style.color='#000000';" runat="server"><img src="images/prop_ico.gif" align="Absmiddle" border="0">&nbsp;����</asp:HyperLink>
		</td>
	</tr>
	<!-- SECTION ITEM: END -->
	<!-- SECTION ITEM: START -->
	<tr>
		<td id="UsersTd" height="20" runat="server">
			<asp:HyperLink id="UsersHyperLink" CssClass="leftMenu" onMouseOver="this.style.color='#ffffff';"
				onMouseOut="this.style.color='#000000';" runat="server"><img src="images/user_ico.gif" align="Absmiddle" border="0">&nbsp;�û�</asp:HyperLink>
		</td>
	</tr>
	<!-- SECTION ITEM: END -->
	<!-- SECTION ITEM: START -->
	<tr>
		<td id="RolesTd" height="20" runat="server">
			<asp:HyperLink id="RolesHyperLink" CssClass="leftMenu" onMouseOver="this.style.color='#ffffff';"
				onMouseOut="this.style.color='#000000';" runat="server"><img src="images/serverroles_ico.gif" align="Absmiddle" border="0">&nbsp;��ɫ</asp:HyperLink>
		</td>
	</tr>
	<!-- SECTION ITEM: END -->
</table>
