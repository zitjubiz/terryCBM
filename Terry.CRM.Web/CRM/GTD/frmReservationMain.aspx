<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReservationMain.aspx.cs" Inherits="Terry.CRM.Web.CRM.frmReservationMain" %>

<%@ Register assembly="DayPilot" namespace="DayPilot.Web.Ui" tagprefix="DayPilot" %>

<%--<%@ Register src="../UserControl/DateSelector.ascx" tagname="DateSelector" tagprefix="uc1" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>預約信息管理</title>
    <link href="../css/Styles.css" rel="stylesheet" type="text/css" />
    <link href="../css/PagerStyle.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function book(id)
        {
            var va = window.open("FrmReservationEdit.aspx?id="+ id+"&date="+ new Date().toUTCString(),null, "status=0;dialogHeight=385px;dialogWidth=720px;help=0");
            document.getElementById("btnRefresh").click();
        }
        function add(evt,emp)
        {
//            alert("add:"+evt);
            var va = window.open("FrmReservationEdit.aspx?id=0&emp="+ emp+"&eventstart="+ evt+"&d="+ new Date().toUTCString(),null, "status=0;dialogHeight=385px;dialogWidth=720px;help=0");
            document.getElementById("btnRefresh").click();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    <table class="style1">
        <tr>
            <td valign="top" nowrap="nowrap" >
            <div style="float:right"><h3>預約管理</h3></div>
                <table >
                    <tr>
                        <td>
                <%--<uc1:DateSelector ID="DateSelector1" runat="server" TabIndex="1"/>--%>
                        </td>
                        <td>分店：
                            <asp:DropDownList ID="ddlStoreNum" runat="server" CssClass="formField" 
                     Width="100px" TabIndex="2"></asp:DropDownList></td>
                     <td><asp:DropDownList ID="ddlSearch" runat="server" CssClass="formField"  TabIndex="3"
                     Width="114px">
                     <asp:ListItem Value="">--請選擇--</asp:ListItem>
                     <asp:ListItem Value="ID">預約編號</asp:ListItem>
                     <asp:ListItem Value="Mobile">電話號碼</asp:ListItem>
                     </asp:DropDownList>
                     <asp:TextBox ID="txtSearch" runat="server" TabIndex="4"></asp:TextBox>
                     </td>
                     <td></td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="查詢" onclick="btnSearch_Click" />
                            <a href="frmReservationTimeTable.aspx">員工上班時間設定</a> &nbsp;
                            <asp:LinkButton Text="預約脩改日誌" runat="server" ID="lnkLog" onclick="lnkLog_Click"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trSearch" runat="server">
        <td>
        <div id="divSearch" style=" vertical-align:top;">
        <asp:DataList ID="dlSearch" runat="server" RepeatColumns="50"  
                    onitemdatabound="dlSearch_ItemDataBound"  GridLines="Both">
                <ItemTemplate>
                <td valign="top" style="width:200px"><a href="frmReservationEdit.aspx?id=<%#Eval("reservationID") %>" target="_blank">
                <%#Eval("Name") %> <%#Eval("storeName") %> <%#(Boolean)Eval("CanChange")==false? "":Eval("employeeNum")%> <%# DataBinder.Eval(Container.DataItem, "eventstart", "{0:dd/MM/yyyy}") %>
</a></td>
                </ItemTemplate>
         </asp:DataList>                  
        </div>
        </td></tr>
        <tr id="trEmp" runat="server">

            <td valign="top">
                <asp:DataList ID="dlEmployee" runat="server" RepeatColumns="50"  
                    onitemdatabound="dlEmployee_ItemDataBound">
                <ItemTemplate>
                <td >
                <div style="font-weight:bold; text-align:center" ><%#Eval("employeeNum") %></div>
                <DayPilot:DayPilotCalendar ID="DayPilotCalendar1" runat="server"  BusinessBeginsHour="11"
                    BusinessEndsHour="22" Days="1" DataEndField="eventEnd" 
                    DataStartField="eventStart" DataTextField="Name" DataValueField="ReservationID" 
                    oneventclick="DayPilotCalendar1_EventClick" 
                    onfreetimeclick="DayPilotCalendar1_FreeTimeClick" 
                    EventClickJavaScript="book('{0}');" 
                    
                        FreeTimeClickJavaScript='<%# "add(\"{0}\",\""+ DataBinder.Eval(Container.DataItem,"employeeNum") +"\");" %>' 
                        ShowHeader="true" ShowHours="true" Width="160" HourHeight="90" 
                        Font-Size="XX-Small" ondatabound="DayPilotCalendar1_DataBound"  
                         DataBackColorField="BackColor"
                         DataFontColorField="FontColor"
                         NonBusinessBackColor="#666666" WorkingDay="AllWeak"  />
                </td> 
                <td width="2"></td>                  
                </ItemTemplate>
                </asp:DataList>  
                <asp:Label ID="lblMsg" runat="server" Visible="false">請先設定當日該分店的上班員工</asp:Label>        
            </td>
        </tr>
        <tr id="trLeave" runat="server" visible="false">
            <td>
                
                例假 <asp:TextBox ID="txtAnnualLeave" runat="server" Width="600" CssClass="formFieldReadOnly"></asp:TextBox><br />
                
                病假  <asp:TextBox ID="txtSickLeave" runat="server" Width="600" CssClass="formFieldReadOnly"></asp:TextBox><br />
                
                事假  <asp:TextBox ID="txtOtherLeave" runat="server" Width="600" CssClass="formFieldReadOnly"></asp:TextBox><br />
                </td>
        </tr>
    </table>
    <asp:Button ID="btnRefresh" runat="server" Height="1px" Text="" 
        Width="1px" onclick="btnRefresh_Click" style="display:none" />
    </form>
</body>
</html>
