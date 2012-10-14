<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true"
    CodeBehind="frmTicketEdit.aspx.cs" Inherits="Terry.CRM.Web.Invoice.frmTicketEdit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Terry.WebControls.DropDownList" Namespace="Terry.WebControls"
    TagPrefix="tag" %>
<%@ Register Assembly="PagingGridView" Namespace="Terry.Web.Control" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/TextArea.ascx" TagName="TextArea" TagPrefix="uc1" %>
<asp:Content ID ="ctHead" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
    <script type="text/javascript">
        function init() {
            // 在这里写你的代码...
            var txtChangeFee = document.getElementById("ctl00_CPH1_txtChangeFee_TextBox");
            if(txtChangeFee!=null)
                txtChangeFee.focus();
            var txtInnerReferenceID = document.getElementById("ctl00_CPH1_txtInnerReferenceID");
            if(txtInnerReferenceID!=null)    
                txtInnerReferenceID.focus();

            var node = document.getElementById("ctl00_CPH1_rptTour_ctl00_gvTour_ctl02_txtFlightNum");
            var node1 = document.getElementById("ctl00_CPH1_rptTour_ctl01_gvTour_ctl02_txtFlightNum");
            var node2 = document.getElementById("ctl00_CPH1_rptTour_ctl02_gvTour_ctl02_txtFlightNum");
            var node3 = document.getElementById("ctl00_CPH1_rptTour_ctl03_gvTour_ctl02_txtFlightNum");
            var node4 = document.getElementById("ctl00_CPH1_rptTour_ctl04_gvTour_ctl02_txtFlightNum");

            if (node != null)
                addEvent(node, 'paste', function (e) { OnPaste('ctl00_CPH1_rptTour_ctl00_btnPaste', e); });
            if (node1 != null)
                addEvent(node1, 'paste', function (e) { OnPaste('ctl00_CPH1_rptTour_ctl01_btnPaste', e); });
            if (node2 != null)
                addEvent(node2, 'paste', function (e) { OnPaste('ctl00_CPH1_rptTour_ctl02_btnPaste', e); });
            if (node3 != null)
                addEvent(node3, 'paste', function (e) { OnPaste('ctl00_CPH1_rptTour_ctl03_btnPaste', e); });
            if (node4 != null)
                addEvent(node4, 'paste', function (e) { OnPaste('ctl00_CPH1_rptTour_ctl04_btnPaste', e); });


            function OnPaste(buttonid, e) {
                if (window.clipboardData) //IE
                {
                    pasteData(buttonid, e);
                    e.returnValue = false;
                }

                if (e.clipboardData) {  //Chrome
                    pasteData(buttonid, e);
                    if (e.preventDefault) {
                        e.preventDefault();
                    }
                    if (e.stopPropagation) {
                        e.stopPropagation();
                    }

                }

            }

        }
        addEvent(window, 'load', init);

        //buttonid =ctl00_CPH1_rptTour_ctl00_btnPaste
        function pasteData(buttonid, event) {
            //行分割
            var opLineBreak = new RegExp("\\r\\n", "g");

            //单元格分割 \s*\t+
            var opTab = new RegExp("\\s*\\t+", "g");

            //粘贴板数据
            var opData = ""; //clipboardData.getData("text");

            if (window.clipboardData && window.clipboardData.getData) { // IE
                opData = window.clipboardData.getData('text');
            } else if (event.clipboardData && event.clipboardData.getData) {
                opData = event.clipboardData.getData('text/plain');
            }
            //alert(opData);
            //行
            var opRows = opData.split(opLineBreak);
            var lpRowID;
            var opRecord;
            var lpRecordIndex;


            for (var i = 0; i < opRows.length; i++) {
                var opCells = opRows[i].split(opTab);
                var prefix = buttonid.substring(0, buttonid.length - 8);
                var textboxs = $("input[id*='" + prefix + "gvTour_ctl0" + (i + 2) + "_txt']");

                for (var q = 0; q < opCells.length; q++) {
                    textboxs[q].value = opCells[q];

                }
            }
        }

    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <table id="wrp" cellpadding="0" cellspacing="0" align="center">
                <tr id="wrp_base">
                    <td valign="top">
                        <div id="wrapper">
                            <div id="main_content" class="content" style="width: 100%">
                                <div id="navbar">
                                    <table>
                                        <tr>
                                            <td width="80%">
                                                <span id="currentModule" />
                                                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:re, lblHome%>" />
                                                &gt;
                                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblTicket%>" />
                                            </td>
                                            <td align="right" nowrap="nowrap">
                                            
                                                <div style="float: right">
                                                <span> <asp:Label ID="lblTicketType" runat="server" Text="<%$ Resources:re, lblBillType %>"></asp:Label>
                                                    :<asp:DropDownList ID="ddlBillType" runat="server" tip="<%$ Resources:re, MsgCustOwnerID%>"
                                                        usage="notempty" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="txtBookingDate_TextChanged">
                                                        <asp:ListItem Value="0">正常</asp:ListItem>
                                                        <asp:ListItem Value="1">退票</asp:ListItem>
                                                        <asp:ListItem Value="2">改票</asp:ListItem>
                                                    </asp:DropDownList></span>
                                                    <asp:Label ID="lblCustOwnerID" runat="server" Text="<%$ Resources:re, lblCustOwnerID %>"></asp:Label>
                                                    :<tag:DropDownList ID="txtCustOwnerID" runat="server" tip="<%$ Resources:re, MsgCustOwnerID%>"
                                                        usage="notempty" Width="120px">
                                                    </tag:DropDownList>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="toolbar" align="right">
                                </div>
                                <div class="DataDetailFrom">
                                    <div class="DetailDt">
                                        <table id="tblBasic" width="100%" cellpadding="2" cellspacing="2">
                                            <tr>
                                                <td colspan="6" class="AddressTitle">
                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:re, lblBaseInfo%>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td >
                                                    <asp:Label ID="lblDept" runat="server" Text="<%$ Resources:re, lblDepartment %>"></asp:Label>
                                                    :
                                                </td>
                                                <td width="20%">
                                                    <asp:DropDownList ID="ddlDept" runat="server" tip="<%$ Resources:re, MsgDepName%>"
                                                        onchange="$('#ctl00_CPH1_txtDepAddress').val(this.value);" usage="notempty" Width="98%"
                                                        AutoPostBack="True" OnSelectedIndexChanged="txtBookingDate_TextChanged">
                                                        <asp:ListItem Value="">---选择---</asp:ListItem>
                                                        <asp:ListItem Value="Schadowstr.44, 40212 Düsseldorf">Düsseldorf</asp:ListItem>
                                                        <asp:ListItem Value="Königstr.22 70173 Stuttgart">Stuttgart</asp:ListItem>
                                                        <asp:ListItem Value="Burgmauer 6•50667 Köln">Köln</asp:ListItem>
                                                        <asp:ListItem Value="Bahnhofstr.27, 90402 Nürnberg">Nürnberg</asp:ListItem>                                                        
                                                        <asp:ListItem Value="Stationsplein 8-K 6221 BT,Maastricht">Maastricht</asp:ListItem>
                                                        <asp:ListItem Value="Willemsplein 13  6811 KB Arnhem">Arnhem</asp:ListItem>
                                                        <asp:ListItem Value="Schadowstr.44, 40212 Düsseldorf   ">二级代理</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:HiddenField ID="hidDepId" runat="server" />
                                                </td>
                                                <td width="10%">
                                                    <asp:Label ID="lblDepAddress" runat="server" Text="<%$ Resources:re, lblDepAddress %>"></asp:Label>
                                                    :
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtDepAddress" runat="server" tip="<%$ Resources:re, MsgDepAddress%>"
                                                        usage="notempty" Width="98%">					
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%" nowrap="nowrap">
                                                    <asp:Label ID="lblBookingDate" runat="server" Text="<%$ Resources:re, lblBookingDate %>"></asp:Label>
                                                    :
                                                </td>
                                                <td width="20%">
                                                    <asp:TextBox ID="txtBookingDate" runat="server" AutoComplete="off" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                                                        usage="notempty" Width="98%" AutoPostBack="True" OnTextChanged="txtBookingDate_TextChanged"></asp:TextBox>
                                                </td>
                                                <td width="15%" nowrap="nowrap">
                                                    <asp:Label ID="lblInnerReferenceID" runat="server" Text="<%$ Resources:re, lblInnerReferenceID %>"></asp:Label>
                                                    :
                                                </td>
                                                <td width="23%">
                                                    <asp:TextBox ID="txtInnerReferenceID" runat="server" tip="<%$ Resources:re, MsgInnerReferenceID%>"
                                                        usage="notempty" Width="98%">					
                                                    </asp:TextBox>
                                                </td>
                                                <td width="10%"  nowrap="nowrap">
                                                    <asp:Label ID="lblDestination" runat="server" Text="<%$ Resources:re, lblDestination %>"></asp:Label>
                                                    :
                                                </td>
                                                <td width="24%">
                                                    <asp:DropDownList ID="ddlDestination" runat="server" tip="<%$ Resources:re, MsgDestination%>"
                                                        Width="98%">
                                                        <asp:ListItem Value="Drittland">第三国</asp:ListItem>
                                                        <asp:ListItem Value="EU">欧盟</asp:ListItem>
                                                        <asp:ListItem Value="Inland">德国内陆</asp:ListItem>
                                                        <asp:ListItem Value="Vers.">Vers.</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCustName" runat="server" Text="<%$ Resources:re, lblCustFullName %>"></asp:Label>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustName" runat="server" tip="<%$ Resources:re, MsgCustName%>"
                                                        usage="notempty" Width="98%">					
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCustTel" runat="server" Text="<%$ Resources:re, lblCustTel %>"></asp:Label>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustTel" runat="server" tip="<%$ Resources:re, MsgCustTel%>"
                                                        Width="98%">
					
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCustEmail" runat="server" Text="<%$ Resources:re, lblContactEmail %>"></asp:Label>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustEmail" runat="server" tip="<%$ Resources:re, MsgContactEmail%>"
                                                         Width="98%">					
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblParentCompany" runat="server" Text="<%$ Resources:re, lblParentCompany %>"></asp:Label>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParentCompany" runat="server" tip="<%$ Resources:re, lblParentCompany%>"
                                                      Width="98%"/>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCustAddress" runat="server" Text="<%$ Resources:re, lblCustAddress %>"></asp:Label>
                                                    :
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtCustAddress" runat="server" tip="<%$ Resources:re, MsgCustAddress%>"
                                                      Width="99%">
					
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCustFax" runat="server" Text="<%$ Resources:re, lblCustFax %>"></asp:Label>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustFax" runat="server" tip="<%$ Resources:re, lblCustFax%>"
                                                      Width="98%"/>
                                                </td>
                                                <td nowrap="nowrap">
                                                    <asp:Label ID="lblMaxLuggage" runat="server" Text="<%$ Resources:re, lblMaxLuggage %>"></asp:Label>
                                                    
                                                </td>
                                                <td colspan="3">
                                                   
                                                    <asp:TextBox ID="txtMaxLuggage" runat="server" tip="<%$ Resources:re, MsgMaxLuggage%>"
                                                        usage="notempty" Width="99%">			
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblAirlines" runat="server" Text="<%$ Resources:re, lblAirline %>"></asp:Label>
                                                    :
                                                </td>
                                                <td >
                                                    <asp:ComboBox ID="txtAirline" runat="server" tip="<%$ Resources:re, MsgAirline%>"
                                                      Width="230px" AutoCompleteMode="SuggestAppend" >
                                                         
                                                        <asp:ListItem>AB–Air Berlin</asp:ListItem>
                                                        <asp:ListItem>AF–Air France</asp:ListItem>
                                                        <asp:ListItem>AY–Finnair</asp:ListItem>
                                                        <asp:ListItem>BA–British Airways</asp:ListItem>
                                                        <asp:ListItem>CA–Air China</asp:ListItem>
                                                        <asp:ListItem>CI–China Airlines</asp:ListItem>
                                                        <asp:ListItem>CZ–China Southern Airlines</asp:ListItem>
                                                        <asp:ListItem>CX–Cathay Pacific</asp:ListItem>
                                                        <asp:ListItem>EK–Emirates</asp:ListItem>
                                                        <asp:ListItem>EY–Etihad Airways</asp:ListItem>
                                                        <asp:ListItem>KL–KLM - Royal Dutch Airlines</asp:ListItem>
                                                        <asp:ListItem>LH–Lufthansa</asp:ListItem>
                                                        <asp:ListItem>MH–Malaysia Airlines</asp:ListItem>
                                                        <asp:ListItem>MU–China Eastern Airlines</asp:ListItem>
                                                        <asp:ListItem>QR–Qatar Airways</asp:ListItem>
                                                        <asp:ListItem>SK–SAS Scandinavian Airlines</asp:ListItem>
                                                        <asp:ListItem>4U–Germanwings</asp:ListItem>
                                                        <asp:ListItem>VY-Vueling Airlines</asp:ListItem>
                                                        <asp:ListItem>HU-Hainan Airlines</asp:ListItem>
                                                        <asp:ListItem>LX-SWISS Airlines</asp:ListItem>
                                                        
                                                    </asp:ComboBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblAccessory" runat="server" Text="<%$ Resources:re, lblAccessory %>"></asp:Label>     
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="txtAccessory" runat="server" tip="<%$ Resources:re, MsgAccessory%>"
                                                        usage="notempty" Width="99%">
                                                        <asp:ListItem>Flug inkl. Flughafensteuern inkl. Rail&Fly(OW)</asp:ListItem>
                                                        <asp:ListItem>Flug inkl. Flughafensteuern，Rail&Fly fuer Erwachsene</asp:ListItem>
                                                        <asp:ListItem>Flug inkl. Flughafensteuern und Rail&Fly</asp:ListItem>
                                                        <asp:ListItem>Flug inkl. Flughafensteuern</asp:ListItem>
                                                    </asp:DropDownList>
                                                    
                                                </td>
                                                
                                            </tr>
                                            <tr style="height:25px">
                                                <td nowrap="nowrap">
                                                    <asp:Label ID="lblCancellationFee" runat="server" Text="<%$ Resources:re, lblCancellationFee %>"></asp:Label>
                                                    
                                                </td>
                                                <td nowrap="nowrap">
                                                    <asp:ComboBox ID="txtCancellationFee" runat="server" tip="<%$ Resources:re, MsgCancellationFee%>"
                                                        usage="notempty" Width="230px"  >
                                                        <asp:ListItem>250.00 p. P. (vor dem Abflug)</asp:ListItem>
                                                        <asp:ListItem>300.00 p. P. (vor dem Abflug)</asp:ListItem>
                                                        <asp:ListItem>100.00 p. P. (vor dem Abflug)</asp:ListItem>
                                                        <asp:ListItem>nicht gestattet</asp:ListItem>
                                                    </asp:ComboBox>
                                                </td>
                                                <td nowrap="nowrap">
                                                    <asp:Label ID="lblChangeFee" runat="server" Text="<%$ Resources:re, lblChangeFee %>"></asp:Label>
                                                    
                                                </td>
                                                <td nowrap="nowrap">
                                                    <asp:ComboBox ID="txtChangeFee" runat="server" tip="<%$ Resources:re, MsgChangeFee%>"
                                                        usage="notempty" Width="230px"  >
                                                        <asp:ListItem>150.00 p. P. ( je nach Verfügbarkeit)</asp:ListItem>
                                                        <asp:ListItem>130.00 p. P. ( je nach Verfügbarkeit)</asp:ListItem>
                                                        <asp:ListItem>110.00 p. P. ( je nach Verfügbarkeit)</asp:ListItem>
                                                        <asp:ListItem>nicht gestattet</asp:ListItem>
                                                    </asp:ComboBox>
                                                </td>
                                                <td nowrap="nowrap">
                                                    <asp:Label ID="lblPaymentDate" runat="server" Text="<%$ Resources:re, lblPaymentDate %>"></asp:Label>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPaymentDate" runat="server" tip="<%$ Resources:re, MsgPaymentDate%>"
                                                        usage="" Width="98%" />
                                                </td>
                                            </tr>
                                             <tr>
                                             <td><asp:Label ID="lblBankAccount" Visible="false" runat="server" Text="<%$ Resources:re, lblBankAccount %>"></asp:Label>
                                                    </td>
                                                <td colspan="5">
                                                   <asp:TextBox ID="txtBankAccount" Visible="false" runat="server" tip="<%$ Resources:re, MsgBankAccount%>"
                                                        usage="" Width="99%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    &nbsp;
                                                    <div id="divErrorMessage">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <table id="tblTicketPerson" runat="server" width="100%" cellpadding="2" cellspacing="2"
                                            border="0">
                                            <tr>
                                                <td colspan="6" class="AddressTitle">
                                                    <asp:Label ID="Label4" runat="server" Text="乘客信息"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="25%" colspan="2">
                                                <asp:Button ID="btnAddPerson" runat="server" Text="增加乘客" OnClick="btnAddPerson_Click" />
                                                 <asp:Button ID="btnAddTenPerson" runat="server" Text="增加10个乘客" OnClick="btnAddPerson_Click" />
                                                </td>
                                                <td width="15%">
                                                </td>
                                                <td width="15%">
                                                </td>
                                                <td width="10%">
                                                    
                                                </td>
                                                <td width="40%" align="right">
                                                    总金额:<asp:Label ID="lblTotalAmount" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <div id="Grid" style="float: left;">
                                                        <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="False" CssClass="tableBorder"
                                                            HeaderStyle-CssClass="Head" RowStyle-CssClass="Row" DataKeyNames="ID" FooterStyle-CssClass="Foot"
                                                            Width="100%" ShowFooter="True" OnRowDataBound="gvData_RowDataBound" >
                                                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="30"></PagerSettings>
                                                            <RowStyle CssClass="Row"></RowStyle>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="乘客姓名">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtOwnerName" runat="server" Width="98%" Text='<%#Bind("OwnerName") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="25%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="是否显示在发票上">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkShowOnInvoice" runat="server" Text="显示" Checked='<%#Bind("IsShowOnInvoice") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="15%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="合并显示行程">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlMergeWith" runat="server">
                                                                            <asp:ListItem Value="">---选择---</asp:ListItem>
                                                                            <asp:ListItem Value="Prev">上一行</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="15%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="金额">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPrice" runat="server" Text='<%# Bind("Price") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="银行对账信息" >
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtBankStatement" runat="server" Width="98%" Text='<%#Bind("BankStatement") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="25%" />
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="部分付款" >
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkPayPartly" runat="server" Checked='<%#Bind("PayNotEnough") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="15%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <FooterStyle CssClass="Foot"></FooterStyle>
                                                            <PagerStyle CssClass="Pager"></PagerStyle>
                                                            <HeaderStyle CssClass="Head"></HeaderStyle>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <table id="tblTicketTour" runat="server" width="100%" cellpadding="2" cellspacing="2">
                                            <tr>
                                                <td colspan="6" class="AddressTitle">
                                                    <asp:Label ID="Label1" runat="server" Text="行程信息 "></asp:Label><span style="color: Red;
                                                        background-color: White;">(请先保存乘客信息后再填写) 一个预订号只填一个价格</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:Repeater ID="rptTour" runat="server" OnItemDataBound="rptTour_ItemDataBound"
                                                        OnItemCommand="rptTour_ItemCommand" EnableViewState="true">
                                                        <ItemTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td nowrap>
                                                                        <asp:Label ID="lblPerson" runat="server" Text="乘客姓名" />
                                                                        <asp:TextBox ID="txtOwnerName" runat="server" Width="250px" ReadOnly="true" Text='<%#Bind("OwnerName") %>' />
                                                                        <asp:Button ID="btnAddTour" runat="server" Text="增加行程" CommandName="Add" />
                                                                        <asp:Button ID="btnPaste" runat="server" Text="粘贴" OnClientClick="pasteData(this.id,event);" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:GridView ID="gvTour" runat="server" AutoGenerateColumns="False" CssClass="tableBorder"
                                                                            HeaderStyle-CssClass="Head" RowStyle-CssClass="Row" DataKeyNames="ID" FooterStyle-CssClass="Foot"
                                                                            Width="100%" ShowFooter="False">
                                                                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="30"></PagerSettings>
                                                                            <RowStyle CssClass="Row"></RowStyle>
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="航班号">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtFlightNum" runat="server" Width="98%" Text='<%#Bind("FlightNum") %>' />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="10%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="航班日期">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtFlightDate" runat="server" Width="98%" Text='<%#Bind("FlightDate") %>' />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="10%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="起飞地点">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtFlightFrom" runat="server" Width="98%" Text='<%#Bind("FlightFrom") %>' />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="12%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="降落地点">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtFlightTo" runat="server" Width="98%" Text='<%#Bind("FlightTo") %>' />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="12%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="起飞时间">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtFlightStartTime" runat="server" Width="98%" Text='<%#Bind("FlightStartTime") %>' />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="8%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="降落时间">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtFlightEndTime" runat="server" Width="98%" Text='<%#Bind("FlightEndTime") %>' />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="8%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="售价">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtPrice" runat="server" Width="98%" Text='<%#Bind("Price") %>'
                                                                                            usage="num" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="8%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="机票号" >
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtFlightTicketNum" runat="server" Width="98%" Text='<%#Bind("FlightTicketNum") %>' />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="12%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Amadeus预订号" >
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtOuterReferenceID" runat="server" Width="98%" Text='<%#Bind("OuterReferenceID") %>' />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="12%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="成本" >
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtCost" runat="server" Width="98%" Text='<%#Bind("Cost") %>' usage="^num" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="8%" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <FooterStyle CssClass="Foot"></FooterStyle>
                                                                            <PagerStyle CssClass="Pager"></PagerStyle>
                                                                            <HeaderStyle CssClass="Head"></HeaderStyle>
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </td>
                                            </tr>
                                        </table>
                                        <table id="tblModifyInfo_closed"  width="100%" cellpadding="2" cellspacing="2">
                                            <tr>
                                                <td colspan="6" class="AddressTitle">
                                                    <a href="" onclick="ToggleDiv( 'tblModifyInfo' ); return false;">
                                                        <img border="0" src="../images/plus.png" alt="-" /></a>
                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:re, lblAuditLog%>"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <table id="tblModifyInfo_open" class="hidden" width="100%" cellpadding="2" cellspacing="2">
                                            <tr>
                                        <td colspan="6" class="AddressTitle">
                                            <a href="" onclick="ToggleDiv( 'tblModifyInfo' ); return false;">
                                                <img border="0" src="../images/minus.png" alt="-" /></a>
                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:re, lblAuditLog%>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" align="center">
                                            <asp:GridView ID="gvAuditLog" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                CellPadding="4"  ForeColor="#333333" GridLines="Both"
                                                Width="98%" align="center" onrowdatabound="gvAuditLog_RowDataBound">
                                                <RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" />
                                                <HeaderStyle BackColor="ActiveCaption" HorizontalAlign="Left" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>                                                 
                                                    <asp:BoundField DataField="ActionAt" HeaderText="修改时间" SortExpression="ActionAt" ItemStyle-Width="10%"/>
                                                    <asp:BoundField DataField="UserName" HeaderText="用户名" SortExpression="UserName" ItemStyle-Width="10%" />
                                                    <asp:BoundField DataField="ActionLog" HeaderText="日志" HtmlEncode="false"  />
                                                </Columns>                                                
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                            <tr style="display: none;">
                                                <td width="10%">
                                                    <asp:Label ID="lblCustCDate" runat="server" Text="<%$ Resources:re, lblCustCDate %>"></asp:Label>
                                                    :
                                                </td>
                                                <td width="20%">
                                                    <asp:TextBox ID="txtCustCDate" runat="server" tip="<%$ Resources:re, MsgCustCDate%>"
                                                        usage="notempty" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" Enabled="false"
                                                        Width="98%">
                                                    </asp:TextBox>
                                                </td>
                                                <td width="10%">
                                                    <asp:Label ID="lblCustCUserID" runat="server" Text="<%$ Resources:re, lblCustCUserID %>"></asp:Label>
                                                    :
                                                </td>
                                                <td width="20%">
                                                    <asp:HiddenField ID="txtCustCUserID" runat="server" />
                                                    <asp:TextBox ID="txtCustCUserName" runat="server" tip="<%$ Resources:re, MsgCustCUserID%>"
                                                        Enabled="false" usage="notempty" Width="98%">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td>
                                                    <asp:Label ID="lblCustModifyDate" runat="server" Text="<%$ Resources:re, lblCustModifyDate %>"></asp:Label>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustModifyDate" runat="server" tip="<%$ Resources:re, MsgCustModifyDate%>"
                                                        Enabled="false" usage="notempty" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"
                                                        Width="98%">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCustModifyUserID" runat="server" Text="<%$ Resources:re, lblCustModifyUserID %>"></asp:Label>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:HiddenField ID="txtCustModifyUserID" runat="server" />
                                                    <asp:TextBox ID="txtCustModifyUserName" runat="server" tip="<%$ Resources:re, MsgCustModifyUserID%>"
                                                        usage="notempty" Width="98%" Enabled="false">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="clearer">
                            <!--  -->
                        </div>
                        <div class="button">
                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:re, lblSave%>" check="true"
                                OnClick="btnSave_Click" />
                            <asp:Button ID="btnExcel" runat="server" Text="<%$ Resources:re, lblExcel%>" check="true"
                                OnClick="btnExcel_Click" Width="125px" />
                           
                            <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:re, lblBack%>" OnClick="btnBack_Click" />
                            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" Style="display: none;"
                                OnClick="btnRefresh_Click" />
                            <asp:Button ID="btnCANX" runat="server" Text="<%$ Resources:re, lblCancel%>" OnClick="btnCANX_Click" />
                            
                            <asp:Button ID="btnNew" runat="server" Text="<%$ Resources:re, lblNew%>" OnClick="btnNew_Click" />
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hidID" runat="server" Value="" />
            <asp:TextBox ID="hidCustID" runat="server" Text="" style=" display:none" tip="<%$ Resources:re, MsgCustID%>" />
            <asp:HiddenField ID="hidSaveMsg" runat="server" Value="<%$ Resources:re, MsgSaveConfirm%>" />
            <asp:HiddenField ID="hidDelMsg" runat="server" Value="<%$ Resources:re, MsgDeleteConfirm%>" />
            <asp:HiddenField ID="hidBtnDel" runat="server" Value="" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
            <asp:PostBackTrigger ControlID="btnBack" />
            <asp:PostBackTrigger ControlID="btnRefresh" />
            <asp:PostBackTrigger ControlID="btnAddPerson" />
            <asp:PostBackTrigger ControlID="btnAddTenPerson" />
            <asp:PostBackTrigger ControlID="btnExcel" />
            <asp:PostBackTrigger ControlID="btnCANX" />
<%--            <asp:PostBackTrigger ControlID="btnChange" />--%>
        </Triggers>  
    </asp:UpdatePanel>
    <script type="text/javascript">
  //<![CDATA[

        var a1;

        function OnLoad() {

            //回调函数, value是suggestion
            var onAutocompleteSelect = function (value, data) {

                var arr = value.split("__");
                $('#ctl00_CPH1_txtCustName').val(arr[0]);
                $('#ctl00_CPH1_txtCustTel').val(arr[1]);
                $('#ctl00_CPH1_txtCustEmail').val(arr[2]);
                $('#ctl00_CPH1_txtCustAddress').val(arr[3]);
                $('#ctl00_CPH1_txtParentCompany').val(arr[4]);
                $('#ctl00_CPH1_hidCustID').val(data);

                //alert($('#ctl00_CPH1_txtCustName').attr('value'));
                
            }

            var options = {
                serviceUrl: '../service/Customer.ashx',
                width: 850,
                delimiter: /(,|;)\s*/,
                onSelect: onAutocompleteSelect,
                deferRequestBy: 0, //miliseconds
                params: { country: 'Yes' }
            };

            a1 = $('#ctl00_CPH1_txtCustName').autocomplete(options);

            //去掉浏览器自帶的提示
            $('#navigation a').each(function () {
                $(this).click(function (e) {
                    var element = $(this).attr('href');
                    $('html').animate({ scrollTop: $(element).offset().top }, 300, null, function () { document.location = element; });
                    e.preventDefault();
                });
            });

        }
        jQuery(OnLoad);
        //for UpdatePanel
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(OnLoad);
  
//]]>
</script>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <iframe id="iframeViewSwf" runat="server" width="100%" height="150px" frameborder="0">
            </iframe>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
