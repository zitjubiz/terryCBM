<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReservationLog.aspx.cs" Inherits="MemDBSystem.frm.frmReservationLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<meta http-equiv="pragma" content="no-cache" />

    <title>预约日志</title>
    <link href="../css/Styles.css" rel="stylesheet" type="text/css" />
    <style>
        .Log
        {
             font-size:13px;
             padding-left:5px;
             padding-top:5px;
    	}
    </style>
</head>
<body style="background:#fff">
    <form id="form1" runat="server">
    <div class="Log">
        <asp:Repeater ID="Repeater1" runat="server">        
        <ItemTemplate>
            <span class="Log"><%# Eval("LogInfo") %> </span><br />
        </ItemTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>
