<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateSelector.ascx.cs" Inherits="CSL.CProd.Web.ascx.DateSelector" %>
<table id="tbl_control" cellspacing="0" cellpadding="0" border="0">
    <tr>
        <td style="text-align: center; height:20px">
            <asp:TextBox ID="txt_Date" runat="server" Columns="6" Width="70px" MaxLength="10" ></asp:TextBox>
        </td>
        <td valign="bottom">
            <asp:Image ID="imgCalendar" runat="server" Width="18px" Height="16px" ImageUrl="~/UserControl/cal/calendar.jpg">
            </asp:Image>
            </td>
            <td>
             
          <%--  <asp:RequiredFieldValidator runat="server" ID="reqDate" ControlToValidate="txt_Date"
                ErrorMessage="&lt;b&gt;Required Field Missing&lt;/b&gt;&lt;" />  --%>
            <asp:RegularExpressionValidator ID="RegularExpressionDate" runat="server"
	            ControlToValidate="txt_Date"  ErrorMessage="<b>Invalid</b>"
	            ValidationExpression="^((((0[13578])|(1[02]))[\/]?(([0-2][0-9])|(3[01])))|(((0[469])|(11))[\/]?(([0-2][0-9])|(30)))|(02[\/]?[0-2][0-9]))[\/]?\d{4}$" Visible="False"></asp:RegularExpressionValidator>
           
        </td>
    </tr>
</table>