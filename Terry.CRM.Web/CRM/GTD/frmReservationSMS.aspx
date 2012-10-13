<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReservationSMS.aspx.cs" Inherits="MemDBSystem.frm.frmReservationSMS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SMS</title>
    <script type="text/javascript" language="javascript">
        function sendSMS()
        {
            alert('發送成功');
            window.opener.document.getElementById("HidSms").value="1";
            window.close();
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="Label1" runat="server" Text="SMS:"></asp:Label>
        <asp:TextBox ID="txtSMS" runat="server" TextMode="MultiLine" Width="550px" Height="50px"></asp:TextBox>
        <br /><asp:Button ID="btnSend" runat="server" Text="發送" 
            OnClientClick="sendSMS()" onclick="btnSend_Click" />
    
    </div>
    </form>
</body>
</html>
