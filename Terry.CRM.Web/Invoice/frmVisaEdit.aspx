<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true"
    CodeBehind="frmVisaEdit.aspx.cs" Inherits="Terry.CRM.Web.Invoice.frmVisaEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Terry.WebControls.DropDownList" Namespace="Terry.WebControls"
    TagPrefix="tag" %>
<%@ Register Assembly="PagingGridView" Namespace="Terry.Web.Control" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/TextArea.ascx" TagName="TextArea" TagPrefix="uc1" %>
<asp:Content ID ="ctHead" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
    
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
                                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblVisa%>" />
                                            </td>
                                            <td align="right" nowrap="nowrap">
                                                <div style="float: right">
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
                                                <td>
                                                    <asp:Label ID="lblDept" runat="server" Text="<%$ Resources:re, lblDepartment %>"></asp:Label>
                                                    :
                                                </td>
                                                <td width="20%">
                                                    <asp:DropDownList ID="ddlDept" runat="server" tip="<%$ Resources:re, MsgDepName%>"
                                                        onchange="$('#ctl00_CPH1_txtDepAddress').val(this.value);" usage="notempty" Width="98%"
                                                        AutoPostBack="True" OnSelectedIndexChanged="txtBookingDate_TextChanged">
                                                        <asp:ListItem Value="">---选择---</asp:ListItem>
                                                        <asp:ListItem Value="Schadow str.44 40212 Düsseldorf">Düsseldorf</asp:ListItem>
                                                        <asp:ListItem Value="Königstr.22 70173 Stuttgart">Stuttgart</asp:ListItem>
                                                        <asp:ListItem Value="Burgmauer 6•50667 Köln">Köln</asp:ListItem>
                                                        <asp:ListItem Value="Bahnhofstr.27, 90402 Nürnberg">Nürnberg</asp:ListItem>
                                                        <asp:ListItem Value="Stationsplein 8-K 6221 BT,Maastricht">Maastricht</asp:ListItem>
                                                        <asp:ListItem Value="Willemsplein 13  6811 KB Arnhem">Arnhem</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:HiddenField ID="hidDepId" runat="server" />
                                                </td>
                                                <td width="10%">
                                                    <asp:Label ID="lblDepAddress" runat="server" Text="<%$ Resources:re, lblDepAddress %>"></asp:Label>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDepAddress" runat="server" tip="<%$ Resources:re, MsgDepAddress%>"
                                                        usage="notempty" Width="98%">					
                                                    </asp:TextBox>
                                                </td>
                                                 <td width="10%">
                                                    <asp:Label ID="lblBookingDate" runat="server" Text="<%$ Resources:re, lblBookingDate %>"></asp:Label>
                                                    :
                                                </td>
                                                <td width="20%">
                                                    <asp:TextBox ID="txtBookingDate" runat="server" AutoComplete="off" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                                                        usage="notempty" Width="98%" AutoPostBack="True" OnTextChanged="txtBookingDate_TextChanged"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>                                               
                                                <td width="10%">
                                                    <asp:Label ID="lblInnerReferenceID" runat="server" Text="<%$ Resources:re, lblInnerReferenceID %>"></asp:Label>
                                                    :
                                                </td>
                                                <td width="20%">
                                                    <asp:TextBox ID="txtInnerReferenceID" runat="server" tip="<%$ Resources:re, MsgInnerReferenceID%>"
                                                        usage="notempty" Width="98%">					
                                                    </asp:TextBox>
                                                </td>
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
                                                        usage="notempty" Width="98%">
					
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                
                                                <td>
                                                    <asp:Label ID="lblCustEmail" runat="server" Text="<%$ Resources:re, lblContactEmail %>"></asp:Label>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustEmail" runat="server" tip="<%$ Resources:re, MsgContactEmail%>"
                                                        Width="98%">					
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCustAddress" runat="server" Text="<%$ Resources:re, lblCustAddress %>"></asp:Label>
                                                    :
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtCustAddress" runat="server" tip="<%$ Resources:re, MsgCustAddress%>"
                                                        usage="" Width="99%">
					
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
                                                    <asp:Label ID="lblPayMethod" runat="server" Text="<%$ Resources:re, lblPayMethod %>"></asp:Label>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:ComboBox ID="ddlPayMethod" runat="server" tip="<%$ Resources:re, MsgPayMethod%>"
                                                        usage="notempty" Width="250px">
                                                        <asp:ListItem>Zahlung bei Abholung</asp:ListItem>
                                                        <asp:ListItem>ab sofort</asp:ListItem>
                                                        <asp:ListItem>zahlbar bis         </asp:ListItem>
                                                        <asp:ListItem>Betrag dankend erhalten !</asp:ListItem>
                                                    </asp:ComboBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPaymentDate" runat="server" Text="<%$ Resources:re, lblPaymentDate %>"></asp:Label>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPaymentDate" runat="server" tip="<%$ Resources:re, MsgPaymentDate%>"
                                                     AutoComplete="off" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"  Width="98%" />
                                                </td>
                                            </tr>
                                            <tr>
                                            <td><asp:Label ID="Label1" runat="server" Text="<%$ Resources:re, lblRemark %>"></asp:Label>
                                                    :</td>
                                                <td colspan="5">                                                   
                                                    <asp:TextBox ID="txtRemark" runat="server" tip=""
                                                        usage="" Width="99.5%">					
                                                    </asp:TextBox>
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
                                                    <asp:Label ID="Label4" runat="server" Text="签证人信息"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="25%" colspan="2">
                                                <asp:Button ID="btnAddPerson" runat="server" Text="增加签证人" OnClick="btnAddPerson_Click" />
                                                </td>
                                                <td width="15%">
                                                </td>
                                                <td width="15%">
                                                </td>
                                                <td width="10%">
                                                    
                                                </td>
                                                <td width="40%" align="right">
                                                    总金额:<asp:Label ID="lblTotalAmount" runat="server" ForeColor="Red">0.00</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <div id="Grid" style="float: left;">
                                                        <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="False" CssClass="tableBorder"
                                                            HeaderStyle-CssClass="Head" RowStyle-CssClass="Row" DataKeyNames="ID" FooterStyle-CssClass="Foot"
                                                            Width="100%" ShowFooter="True" OnRowDataBound="gvData_RowDataBound">
                                                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="30"></PagerSettings>
                                                            <RowStyle CssClass="Row"></RowStyle>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="签证人姓名">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtVisaName" runat="server" Width="98%" Text='<%#Bind("VisaName") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="15%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="国籍">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtCountry" runat="server" Width="98%" Text='<%#Bind("Country") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="6%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="护照">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPassport" runat="server" Width="98%" Text='<%#Bind("Passport") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="8%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="签证类型">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlVisaType" runat="server"  Text='<%#Bind("VisaType") %>'>
                                                                            <asp:ListItem Value="">-----</asp:ListItem>
                                                                            <asp:ListItem Value="一次">一次</asp:ListItem>
                                                                            <asp:ListItem Value="两次">两次</asp:ListItem>
                                                                            <asp:ListItem Value="多次">多次</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="5%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="加急">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlUrgent" runat="server" Text='<%#Bind("IsUrgent") %>'>
                                                                            <asp:ListItem Value="False">否</asp:ListItem>
                                                                            <asp:ListItem Value="True">是</asp:ListItem>    
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="3%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="入境日期">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtEntryDate" runat="server" Width="98%" 
                                                                         AutoComplete="off" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"/>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="7%" />
                                                                </asp:TemplateField>
                                                               <asp:TemplateField HeaderText="申请日期">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtApplyDate" runat="server" Width="98%" 
                                                                         AutoComplete="off" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"/>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="7%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="取证日期">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtApproveDate" runat="server" Width="98%" 
                                                                         AutoComplete="off" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"/>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="7%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="取证号码">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtEmbassySerialNum" runat="server" Width="98%" Text='<%#Bind("EmbassySerialNum") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="8%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="领馆收费">
                                                                <HeaderStyle Wrap="false" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtEmbassyFee" runat="server" Width="98%" Text='<%#Bind("EmbassyFee") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="4%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="签证中心收费">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtVisaCenterFee" runat="server" Width="98%" Text='<%#Bind("VisaCenterFee") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="5%" />
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="邮费">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPostFee" runat="server" Width="98%" Text='<%#Bind("PostFee") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="4%" />
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="服务费">
                                                                 <HeaderStyle Wrap="false" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtServiceFee" runat="server" Width="98%" Text='<%#Bind("ServiceFee") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="4%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="出生证明">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtBirthCert" runat="server" Width="98%" Text='<%#Bind("BirthCert") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="4%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="HK Pass">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtHKPass" runat="server" Width="98%" Text='<%#Bind("HKPass") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="5%" />
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
            <asp:PostBackTrigger ControlID="btnCANX" />
            <asp:PostBackTrigger ControlID="btnExcel" />
        </Triggers>  
    </asp:UpdatePanel>
    <script type="text/javascript">
  //<![CDATA[

        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        // initializeRequest→beginRequest→ PageLoad→endRequest
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
</asp:Content>
