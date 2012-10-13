<%@ Page Language="C#" AutoEventWireup="true"  CodeBehind="frmReservationEdit.aspx.cs" Inherits="MemDBSystem.frm.frmReservationEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/DateSelector.ascx" TagName="DateSelector" TagPrefix="uc1" %>
<%@ Register Assembly="ASPnetPagerV2netfx2_0" Namespace="CutePager" TagPrefix="cc2" %>

<%@ Register src="../UserControl/TimePicker.ascx" tagname="TimePicker" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>預約信息維護</title>
    <link href="../css/Styles.css" rel="stylesheet" type="text/css" />
    <link href="../css/PagerStyle.css" rel="stylesheet" type="text/css" />
     <link href="../css/autocomplete.css" rel="stylesheet" type="text/css" />
   
    <script type="text/javascript" src="../js/Common.js"></script>
    <script type="text/javascript" src="../js/prototype.js"></script>
    <script type="text/javascript" src="../js/autocomplete.js"></script>

<script type="text/javascript" >
      function chk()
      {
        var checkbox = new Array();
        checkbox.length =arguments.length;

        for(var i=0;i<checkbox.length;i++)
        {
            checkbox[i] = arguments[i];
        }
            if(checkbox[checkbox.length-1].checked)
                checkbox[checkbox.length-2].checked =false;
            
      }            

</script>

    <script type="text/javascript" language="javascript" event="onkeydown" for="document">
	    <!--	    
        if(event.keyCode==13 && event.srcElement.type!='button' && event.srcElement.type!='textarea' && 
        event.srcElement.type!='image' && event.srcElement.type!='submit' && event.srcElement.type!='reset' && 
        event.srcElement.type!='file' && event.srcElement.type!='image' && event.srcElement.type!='' &&
        !event.srcElement.ignoreEnterTap)
        event.keyCode=9;
       
        if(event.keyCode == 27 )
            window.close();
	    -->

      </script>

</head>
<body onunload="">
    <form id="form1" runat="server"  >
     <div>
         <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods ="true" />
     </div>
    <div>
    <table id="tblRowDetail" runat="server" width="100%" bgcolor="#F5F5F5" cellpadding="2"
            cellspacing="0" bordercolorlight="#e4e4e4" bordercolordark="#ffffff" bordercolor="#d2d2d2"
            border="1">
            <tr>
            <td  colspan ="2">
              <asp:HiddenField ID="HidReservationID" runat="server" />
              <asp:HiddenField ID="HidMemberNum" runat="server" />
              <asp:HiddenField ID="hidIsInput" runat="server" Value="0"/>
             </td>
            </tr>
            
             <tr>
             <td class="formRowHead">
                 <asp:Label ID="Label14" runat="server" Text="分店名稱："></asp:Label>
                </td>
                <td>
                <asp:DropDownList ID="ddlStoreNum" runat="server" TabIndex="1" Width="150px" 
                        AutoPostBack="True" onselectedindexchanged="ddlStoreNum_SelectedIndexChanged" ></asp:DropDownList>
                </td>     
                <td class="formRowHead" nowrap="nowrap">
                    <asp:Label ID="Label4" runat="server" Text="員工："></asp:Label>
                </td>
                <td >
                    <asp:DropDownList ID="ddlEmployee" runat="server"  Width="150px" 
                        DataTextField="EmployeeNum" DataValueField="EmployeeNum" TabIndex="2"></asp:DropDownList></td>
                   
              </tr>
              <tr>
              <td class="formRowHead" >
                    <asp:Label ID="Label5" runat="server" Text="起止時間："></asp:Label>
                </td>
                <td colspan="3"> 
                        <uc1:DateSelector ID="dtEventDate" runat="server"  EnabledRequired="False" TabIndex="3"/>
                        <uc2:TimePicker ID="eventStart" runat="server" TabIndex="4" />
                        <uc2:TimePicker ID="eventEnd" runat="server"  TabIndex="5"/>
                        </td>
                </tr>
              <tr>
              <td class="formRowHead" >
                    <asp:Label ID="Label1" runat="server" Text="會員："></asp:Label>
                </td>
                <td  colspan="3" nowrap="nowrap"> 
                        <asp:ImageButton ID="btnPickSharePerson" runat="server" BorderWidth="0px" CausesValidation="False"
                            ImageAlign="AbsMiddle" ImageUrl="~/img/lookup.gif" OnClientClick="return openPickSharePersonFrm('yes');"
                             ToolTip="選擇會員" />
                    名稱<asp:TextBox ID="txtSharePersonEngName" runat="server" SkinID="TextBoxM" MaxLength="20"  TabIndex="6" ></asp:TextBox>
                     電話<asp:TextBox ID="txtMobile" runat="server" SkinID="TextBoxM" MaxLength="10"  TabIndex="7"
                           ></asp:TextBox> 非會員預約,請直接輸入名字和電話
                           <div>
                           <asp:Button runat ="server" id ="btnSentM" class="formField"  text="SMS" 
                          OnClientClick="window.open('frmReservationSMS.aspx?id='+$F(HidReservationID));" />
<script type="text/javascript">
  new Autocomplete('txtSharePersonEngName', { serviceUrl:'../service/package.ashx',  
         onSelect: function(value, data){
        var arr = value.split(" ");
        
        $("txtSharePersonEngName").value = arr[0];
        $("txtMobile").value = arr[1];
        $("txtServiceName").value = arr[2];
        $("txtPackage").value ="";
        for(var i=3;i<arr.length-1;i++)        
            $("txtPackage").value += arr[i]+" ";
        
        $("txtMemberNum").value = data;
      }
 });
</script><asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                    <ContentTemplate>
                        <asp:Label ID="lblSendSMS" runat="server" Text="SMS Sent!" Visible="false"></asp:Label>
                    <asp:Timer ID="Timer1" runat="server" Interval="3000" ontick="Timer1_Tick"></asp:Timer>
                    
                    </ContentTemplate>
                    
                    
                    </asp:UpdatePanel>  </div>                
                        </td>
                </tr>
                <tr>
                <td class="formRowHead">
                    會員編號/套票</td>
                <td  colspan="3"> 
                    會員編號<asp:TextBox ID="txtMemberNum" runat="server" SkinID="TextBoxM" MaxLength="20"  TabIndex="8" 
                           ></asp:TextBox>
                           套票<asp:TextBox ID="txtPackage" runat="server" SkinID="TextBoxM" MaxLength="20"  TabIndex="9" 
                           width=350></asp:TextBox></td>
              </tr>
            
           
                <tr>
                <td class="formRowHead">
                    <asp:Label ID="Label2" runat="server" Text="服務項目："></asp:Label>
                </td>
                <td  colspan="3"> 
                <asp:ImageButton ID="btnPickService" runat="server" BorderWidth="0px" CausesValidation="False"
                            ImageAlign="AbsMiddle" ImageUrl="~/img/lookup.gif" OnClientClick="return selService('yes');"
                             ToolTip="選擇服務" />
                     編號<asp:TextBox ID="txtServiceNum" runat="server" SkinID="TextBoxM" MaxLength="50" TabIndex="10"></asp:TextBox>
<script type="text/javascript">
  new Autocomplete('txtServiceNum', { serviceUrl:'../service/ServiceData.ashx',  
         onSelect: function(value, data){
        $("txtServiceNum").value= data;
        var arr = value.split(" ");
        $("txtServiceName").value = arr[0];   
      }
 });
</script>                              
                名稱<asp:TextBox ID="txtServiceName" runat="server" SkinID="TextBoxM" MaxLength="50"  TabIndex="11" 
                        Width="150" ></asp:TextBox>備註<asp:TextBox ID="txtRemark" runat="server" SkinID="TextBoxM" MaxLength="50"  TabIndex="11" 
                        Width="150" ></asp:TextBox>
                    </td>
              </tr>
            
           
                <tr>
                <td class="formRowHead">
                    <asp:Label ID="Label6" runat="server" Text="選項："></asp:Label>
                </td>
                <td colspan="3"> 
                    <asp:CheckBox ID="chkCanChange" runat="server" TabIndex="12"  Text="客戶指定員工服務" BackColor="Red" />
                    <asp:CheckBox ID="chkIsArrived" runat="server" TabIndex="13" GroupName="isState"  Text="客戶已經到達"   BackColor="Yellow"
                        onpropertychange="onArriveChange();" onclick="chk(chkCancel,this);" />
                    <asp:CheckBox ID="chkCancel" runat="server" TabIndex="14"  BackColor="#FFDDFF"
                        GroupName="isState" Text="客戶失约" onpropertychange="onArriveChange();"
                        onclick="chk(chkIsArrived,this);"/>
                     <asp:CheckBox ID="chkColor" runat="server" TabIndex="15"   
                    Text="格子顯示其他顔色:" />
                    <asp:DropDownList ID="ddlColor" runat="server">
                    <asp:ListItem Value="#5ECB41">綠色</asp:ListItem>
                    <asp:ListItem Value="#D847F2">紫色</asp:ListItem>
                    <asp:ListItem Value="Gray">灰色</asp:ListItem>
                    </asp:DropDownList>
                    </td>
              </tr>
                 <tr>
                <td class="formRowHead">
                    <asp:Label ID="Label3" runat="server" Text="是否確認預訂："></asp:Label>
                </td>
                <td colspan="3"> 

                    <asp:CheckBox ID="chkConfirmOK" runat="server" TabIndex="15" 
                      Text="OK" onclick="chk(chkConfirmVM,this);"/>
                    <asp:CheckBox ID="chkConfirmVM" runat="server" TabIndex="16" 
                         Text="VM" onclick="chk(chkConfirmOK,this);" />
                    
                    </td>
              </tr>           
           
             <tr>

                <td class="formRowHead">
                    <asp:Label ID="Label7" runat="server" Text="錄入人員："></asp:Label>
                </td>
                <td colspan="3"> 
                    <asp:Label ID="lblLogonUser" runat="server" Text=""></asp:Label>
                    
                    </td>
              </tr>
              <tr>
                <td >
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSave" runat="server" CssClass="formField" Text="保存" UseSubmitBehavior="false" TabIndex="15"
                        OnClientClick="onSave(this)" onclick="btnSave_Click" />
                        <asp:Button ID="btnInputBill" runat="server" CssClass="formField" Text="入單" 
                        TabIndex="16" OnClientClick="return onIsPut(this)" onclick="btnInputBill_Click"/>
                    <asp:Button ID="btnDelete" runat="server" CssClass="formField"  Text="刪除" TabIndex="17"
                        OnClientClick="return confirmDelete()" CausesValidation="False" 
                        onclick="btnDelete_Click" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="formField" Text="關閉" TabIndex="18"  OnClientClick="window.close();return false;"
                        CausesValidation="False"  /><asp:HiddenField ID="HidSms" runat="server" />
                </td>
            </tr>
            
   </table> 
   </div> 
   
    <script type="text/javascript" language="javascript"  >

        function onSave(btn) {
            //customerSubmitOnSave(btn);
            //confirmSave();
        }
        
        function openPickSharePersonFrm(isClick) {
            if (window.showModalDialog) {
                var va = window.showModalDialog("pickFrm/pickSharePerson.aspx?IsClick=" + isClick  + "&fixCache=" + new Date().getTime(), null, "status=0;dialogHeight=385px;dialogWidth=720px;help=0");
                setFormValues(va);
            } else {
            window.open("pickFrm/pickSharePerson.aspx?IsClick=" + isClick + "&memberNum=" + document.getElementById("ddlMemberNum")  + "&fixCache=" + new Date().getTime(), "pickmember", "status=0,height=385px,width=720px;help=0");
            }
            return false;
        }
        
        function setFormValues(va) {
            if (typeof (va) != "undefined") {
                document.getElementById("txtSharePersonEngName").value = va.secField;
                document.getElementById("txtMobile").value = va.fthField;
                document.getElementById("HidMemberNum").value = va.fstField;
                document.getElementById("txtMemberNum").value = va.fstField;
            }
        }
        function selService(isClick) {
        var url = "pickFrm/pickServers.aspx?IsClick=" + isClick + "&fixCache=" + new Date().getTime();
            if (window.showModalDialog) {
                var va = window.showModalDialog(url, null, "status=0;dialogHeight=385px;dialogWidth=720px;help=0");
                setServiceValues(va);
            } else {
            window.open(url, "win_service", "status=0,height=385px,width=720px;help=0");
            }
            return false;
        }

        function setServiceValues(va) {
            if (typeof (va) != "undefined") {
                if ($("txtServiceNum"))
                $("txtServiceNum").value = va.fstField;

            if ($("txtServiceName"))
                $("txtServiceName").value = va.secField;

            if ($("txtServiceType"))
                $("txtServiceType").value = va.thdField;

            if ($("hidTimeOnce"))
            {
                $("hidTimeOnce").value = va.fivField;
            }
            if ($("hidPrice"))
                $("hidPrice").value = va.sixField;
            }
        }

        function reBindEmployee() {
            var storeNum = $F("ddlStoreNum");
            PageMethods.GetEmployeeByStoreNum(storeNum, bindEmployeeDll);
        }

        function bindEmployeeDll(result) {
            if (typeof (result) != "undefined") {
                var employeeDll = $("ddlEmployee");
                var option = employeeDll.options;
                employeeDll.length = 0;
                var name = "---Select---";
                var value = "";
                option.add(new Option(name, value));
                for (var i = 0; result && result.length && i < result.length; i++) {
                    var nameLength = String(result[i]).length;
                    option.add(new Option(result[i].substring(4, nameLength), result[i].substring(0, 4)));
                }
            }
        }

        function onInputBill(btn) {
        }

        function onArriveChange() {
            var isArrived = document.getElementById("chkIsArrived").checked;
            var isInput = $F("hidIsInput");
            if (isInput == "1") {
                $("btnInputBill").Enabled = false;
            }
            else
                $("btnInputBill").Enabled = isArrived;
        }

        function onIsPut(btn) {
            var isInput = $F("hidIsInput");
            if (isInput == "1") {
                alert('此預約已入單,請檢查!');
                return false;
            }
             
        }

    </script>
    </form>
    <p>
        &nbsp;</p>
</body>
</html>