<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReservationTimeTable.aspx.cs" Inherits="MemDBSystem.frm.frmReservationTimeTable" %>
<%@ Register src="../UserControl/DateSelector.ascx" tagname="DateSelector" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>選擇分店员工</title>
    <link href="../css/Styles.css" rel="stylesheet" type="text/css" />
    <link href="../css/PagerStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/Common.js"></script>
    <script type="text/javascript" src="../js/prototype.js"></script>
    <style>
            .Row {
	        padding-right: 0px;
	        padding-left: 3px;
	        background: #87AEC5;
	        padding-bottom: 3px;
	        padding-top: 3px;
	        margin:2px;
        }
        .Row:hover{background: #dedede;}

    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    <table width=100%>
        <tr>
            <td valign="top" nowrap="nowrap" >
            <div style="float:right"><h3>選擇分店上班员工</h3></div>
                <table >
                    <tr>
                        <td>
                <uc1:DateSelector ID="DateSelector1" runat="server" TabIndex="1"/>
                        </td>
                        <td >分店：
                            <asp:DropDownList ID="ddlStoreNum" runat="server" CssClass="formField"  TabIndex="2" 
                     Width="114px"></asp:DropDownList></td>
                     <td>
                     </td>
                     <td>
                         
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="查詢" onclick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>

            <td valign="top">
            <div>請定義每日每個分店的上班員工和上班時間</div>
                <asp:Repeater ID="rptEmp" runat="server" onitemdatabound="rptEmp_ItemDataBound">
                <ItemTemplate>
                <div class="Row">
                    <asp:CheckBox ID="chkSel" runat="server" />
                    <asp:HiddenField ID="employeeNum" runat="server" Value =<%#Eval("employeeNum") %> />
                    <asp:Label  ID="lblEmp" runat="server" Width="100px" Text=<%#Eval("employeeNum") %>></asp:Label>
                     上班時間 <asp:DropDownList ID="ddlCheckIn" runat="server">
                   <asp:ListItem>11:00</asp:ListItem>
                   <asp:ListItem>11:30</asp:ListItem>
                   <asp:ListItem>12:00</asp:ListItem>
                   <asp:ListItem>12:30</asp:ListItem>
                   <asp:ListItem>13:00</asp:ListItem>
                   <asp:ListItem>13:30</asp:ListItem>
                   <asp:ListItem>14:00</asp:ListItem>
                   <asp:ListItem>14:30</asp:ListItem>
                   <asp:ListItem>15:00</asp:ListItem>
                   <asp:ListItem>15:30</asp:ListItem>
                   <asp:ListItem>16:00</asp:ListItem>
                   <asp:ListItem>16:30</asp:ListItem>
                   <asp:ListItem>17:00</asp:ListItem>
                   <asp:ListItem>17:30</asp:ListItem>
                   <asp:ListItem>18:00</asp:ListItem>
                   <asp:ListItem>18:30</asp:ListItem>
                   <asp:ListItem>19:00</asp:ListItem>
                   <asp:ListItem>19:30</asp:ListItem>
                   <asp:ListItem>20:00</asp:ListItem>
                   <asp:ListItem>20:30</asp:ListItem>
                   <asp:ListItem>21:00</asp:ListItem>
                   <asp:ListItem>21:30</asp:ListItem>
                   <asp:ListItem>22:00</asp:ListItem>
                   
                   </asp:DropDownList>
                   下班時間 <asp:DropDownList ID="ddlCheckOut" runat="server">
                   <asp:ListItem>11:00</asp:ListItem>
                   <asp:ListItem>11:30</asp:ListItem>
                   <asp:ListItem>12:00</asp:ListItem>
                   <asp:ListItem>12:30</asp:ListItem>
                   <asp:ListItem>13:00</asp:ListItem>
                   <asp:ListItem>13:30</asp:ListItem>
                   <asp:ListItem>14:00</asp:ListItem>
                   <asp:ListItem>14:30</asp:ListItem>
                   <asp:ListItem>15:00</asp:ListItem>
                   <asp:ListItem>15:30</asp:ListItem>
                   <asp:ListItem>16:00</asp:ListItem>
                   <asp:ListItem>16:30</asp:ListItem>
                   <asp:ListItem>17:00</asp:ListItem>
                   <asp:ListItem>17:30</asp:ListItem>
                   <asp:ListItem>18:00</asp:ListItem>
                   <asp:ListItem>18:30</asp:ListItem>
                   <asp:ListItem>19:00</asp:ListItem>
                   <asp:ListItem>19:30</asp:ListItem>
                   <asp:ListItem>20:00</asp:ListItem>
                   <asp:ListItem>20:30</asp:ListItem>
                   <asp:ListItem>21:00</asp:ListItem>
                   <asp:ListItem>21:30</asp:ListItem>
                   <asp:ListItem>22:00</asp:ListItem>                   </asp:DropDownList>   
                   <asp:CheckBox ID ="chkIsLeave" runat="server" Text="今日休息" />   
                   例假  <asp:TextBox ID="txtAnnualLeave" runat="server" Width="100"></asp:TextBox>
                   病假  <asp:TextBox ID="txtSickLeave" runat="server" Width="100"></asp:TextBox>
                   事假  <asp:TextBox ID="txtOtherLeave" runat="server" Width="100"></asp:TextBox>
                   </div>
                </ItemTemplate>
                </asp:Repeater>        
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" Text="保存" 
                    Visible="False"  OnClientClick=" return confirmSave();" />
                <asp:Button ID="btnBack" runat="server" Text="返回" OnClientClick="javascript:window.location='frmReservationMain.aspx';return false;" />
            </td>
        </tr>
    </table>
    
    </form>
</body>
</html>
