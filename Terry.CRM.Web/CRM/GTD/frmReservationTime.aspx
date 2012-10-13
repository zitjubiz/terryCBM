<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReservationTime.aspx.cs" Inherits="MemDBSystem.frm.frmReservationTime" %>

<%@ Register src="../UserControl/DateSelector.ascx" tagname="DateSelector" tagprefix="uc1" %>

<%@ Register src="../UserControl/TimePicker.ascx" tagname="TimePicker" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>员工上班时间</title>
    <link href="../css/Styles.css" rel="stylesheet" type="text/css" />
    <link href="../css/PagerStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/Common.js"></script>
    <script type="text/javascript" src="../js/prototype.js"></script>

    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    <table class="style1">
        <tr>
            <td valign="top" nowrap="nowrap" >
            <div style="float:right"><h3>员工上班时间</h3></div>
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
                <asp:DataList ID="dlEmployee" runat="server" RepeatColumns="80" TabIndex="3" onitemdatabound="dlEmployee_ItemDataBound"  
                    >
                <ItemTemplate>
                <td width="130">
                <div style="font-weight:bold;text-align:center"><%#Eval("employeeNum")%></div>
                <asp:HiddenField ID="employeeNum" runat="server" Value =<%#Eval("employeeNum") %> />
                <table  Class="gvformField" ><tr><td>
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
                   
                   </asp:DropDownList><br />
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
                </td></tr>
                <tr><td>
                <asp:CheckBox ID ="chkIsLeave" runat="server" Text="今日休息" />  
                </td></tr>
                <tr><td nowrap=nowrap>
                例假 <asp:TextBox ID="txtAnnualLeave" runat="server" Width="100"></asp:TextBox>
                </td></tr>
                <tr><td nowrap=nowrap>
                病假  <asp:TextBox ID="txtSickLeave" runat="server" Width="100"></asp:TextBox>
                </td></tr>
                <tr><td nowrap=nowrap>
                事假  <asp:TextBox ID="txtOtherLeave" runat="server" Width="100"></asp:TextBox>
                </td></tr>                                
                </table>

                </td>                   
                </ItemTemplate>
                </asp:DataList>          
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
    <asp:Button ID="btnRefresh" runat="server" Height="1px" Text="" 
        Width="1px" onclick="btnRefresh_Click" style="display:none" />
    </form>
</body>
</html>
