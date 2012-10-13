<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="frmCustomerEdit.aspx.cs"
    Inherits="Terry.CRM.Web.CRM.frmCustomerEdit_Chemical" MasterPageFile="~/MasterPage/Site.Master" %>

<%@ Register Assembly="Terry.WebControls.DropDownList" Namespace="Terry.WebControls"
    TagPrefix="tag" %>
<%@ Register Assembly="PagingGridView" Namespace="Terry.Web.Control" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
    <script type="text/javascript" src="../js/jquery.treeTable.js"></script>
    <script type="text/javascript" src="../js/popBox.min.js"></script>
    <link href="../css/popBox.css" rel="stylesheet" type="text/css" />
    <link href="../css/Master.css" rel="stylesheet" type="text/css" />
    <link href="../css/Master.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        $(document).ready(function () {
            $("#tblProd").treeTable();

            // Make visible that a row is clicked
            $("table#tblProd tbody tr").mousedown(function () {
                $("tr.selected").removeClass("selected"); // Deselect currently selected rows
                $(this).addClass("selected");
                $(this).toggleBranch(); //双击行时展开/收缩下属
            });

            // Make sure row is selected when span is clicked
            $("table#tblProd tbody tr span").mousedown(function () {
                $($(this).parents("tr")[0]).trigger("mousedown");
            });
            //prod textbox popup
            $('input[id*="txtBrand_"]').popBox();

        });
        function AddNew() {
            document.getElementById("<% =hidID.ClientID%>").value = '0';
        }
    </script>
    <table id="wrp" cellpadding="0" cellspacing="0" align="center">
        <tr id="wrp_base">
            <td valign="top">
                <div id="wrapper">
                    <div id="main_content" class="content" style="width: 100%">
                        <div id="navbar">
                            <table width="100%">
                                <tr>
                                    <td width="80%">
                                        <span id="currentModule" />
                                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:re, lblHome%>" />
                                        &gt;
                                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblCustomer%>" />
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
                                <table width="100%" cellpadding="2" cellspacing="2">
                                    <tr>
                                        <td width="10%">
                                            <asp:Label ID="lblCustCode" runat="server" Text="<%$ Resources:re, lblCustCode %>"></asp:Label>
                                            :
                                        </td>
                                        <td width="20%">
                                            <asp:TextBox ID="txtCustCode" runat="server" tip="<%$ Resources:re, MsgCustCode%>"
                                                usage="" Width="98%" Enabled="false">					
                                            </asp:TextBox>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="lblCustName" runat="server" Text="<%$ Resources:re, lblCustName %>"></asp:Label>
                                            :
                                        </td>
                                        <td width="20%">
                                            <asp:TextBox ID="txtCustName" runat="server" tip="<%$ Resources:re, MsgCustName%>"
                                                usage="notempty" Width="98%">					
                                            </asp:TextBox>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="lblCustFullName" runat="server" Text="<%$ Resources:re, lblCustFullName %>"></asp:Label>
                                            :
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox ID="txtCustFullName" runat="server" tip="<%$ Resources:re, MsgCustFullName%>"
                                                usage="notempty" Width="98%">					
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCustTypeID" runat="server" Text="<%$ Resources:re, lblCustTypeID %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="txtCustTypeID" runat="server" tip="<%$ Resources:re, MsgCustTypeID%>"
                                                usage="notempty" Width="98%">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCustEmpNumID" runat="server" Text="<%$ Resources:re, lblCustEmpNumID %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="txtCustEmpNumID" runat="server" tip="<%$ Resources:re, MsgCustEmpNumID%>"
                                                usage="" Width="98%">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblArrivalPort" runat="server" Text="<%$ Resources:re, lblArrivalPort %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtArrivalPort" runat="server" tip="<%$ Resources:re, MsgArrivalPort%>"
                                                usage="" Width="98%">					
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPayMethod" runat="server" Text="<%$ Resources:re, lblPayMethod %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPayMethod" runat="server" tip="<%$ Resources:re, MsgPayMethod%>"
                                                usage="" Width="98%">					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPurchaseChannels" runat="server" Text="<%$ Resources:re, lblPurchaseChannels %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPurchaseChannels" runat="server" tip="<%$ Resources:re, MsgPurchaseChannels%>"
                                                usage="" Width="98%">					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSaleChannels" runat="server" Text="<%$ Resources:re, lblSaleChannels %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSaleChannels" runat="server" tip="<%$ Resources:re, MsgSaleChannels%>"
                                                usage="" Width="98%">					
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCustWeb" runat="server" Text="<%$ Resources:re, lblCustWeb %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCustWeb" runat="server" tip="<%$ Resources:re, MsgCustWeb%>"
                                                usage="" Width="98%">					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCustProvince" runat="server" Text="<%$ Resources:re, lblCustProvince %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlProvince" runat="server" Width="60px" onchange="$('#ctl00_CPH1_txtCustProvince').val(this.value);">
                                                <asp:ListItem Value="">-----</asp:ListItem>
                                                <asp:ListItem>广东</asp:ListItem>
                                                <asp:ListItem>浙江</asp:ListItem>
                                                <asp:ListItem>江苏</asp:ListItem>
                                                <asp:ListItem>山东</asp:ListItem>
                                                <asp:ListItem>福建</asp:ListItem>
                                                <asp:ListItem>湖北</asp:ListItem>
                                                <asp:ListItem>上海</asp:ListItem>
                                                <asp:ListItem>四川</asp:ListItem>
                                                <asp:ListItem>其他</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtCustProvince" runat="server" tip="<%$ Resources:re, MsgCustProvince%>"
                                                usage="" Width="120px">
					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCustAddress" runat="server" Text="<%$ Resources:re, lblCustAddress %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCustAddress" runat="server" tip="<%$ Resources:re, MsgCustAddress%>"
                                                usage="" Width="98%">					
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCustTel" runat="server" Text="<%$ Resources:re, lblCustTel %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCustTel" runat="server" tip="<%$ Resources:re, MsgCustTel%>"
                                                usage="" Width="98%">
					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCustFax" runat="server" Text="<%$ Resources:re, lblCustFax %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCustFax" runat="server" tip="<%$ Resources:re, MsgCustFax%>"
                                                usage="" Width="98%">
					
                                            </asp:TextBox>
                                        </td>
                                         <td>
                                            <asp:Label ID="lblCommissionFactor" runat="server" Text="<%$ Resources:re, lblCommissionFactor %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCommissionFactor" runat="server" usage="num+" Width="98%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                       <td></td>
                                       <td></td>
                                        <td>
                                            <asp:RadioButtonList ID="radBid" runat="server" RepeatDirection="Horizontal" Visible="false">
                                                <asp:ListItem Value="True">是</asp:ListItem>
                                                <asp:ListItem Value="False" Selected="True">否</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCustRelationID" runat="server" Visible="false" Text="<%$ Resources:re, lblCustRelationID %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="txtCustRelationID" runat="server" tip="<%$ Resources:re, MsgCustRelationID%>"
                                                usage="" Width="98%" Visible="false">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSave1" runat="server" Text="<%$ Resources:re, lblSave%>" check="true"
                                                OnClick="btnSave_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            &nbsp;
                                            <div id="divErrorMessage">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" class="AddressTitle">
                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:re, lblContact%>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <!-- Contact Person --->
                                    <tr>
                                        <td colspan="6">
                                            <table cellpadding="2" width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblContactType1" runat="server" Text="老板"></asp:Label>
                                                        <asp:TextBox ID="txtCT1" runat="server" usage="" Width="160" ReadOnly="true" />
                                                        <asp:ImageButton ID="imgCT1" runat="server" ImageUrl="../images/note.png" alt="老板"
                                                            check="true" BorderWidth="0" OnClick="imgCT1_Click" OnClientClick="return showContact(1);" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblContactType2" runat="server" Text="采购"></asp:Label>
                                                        <asp:TextBox ID="txtCT2" runat="server" usage="" Width="160" ReadOnly="true" />
                                                        <asp:ImageButton ID="imgCT2" runat="server" ImageUrl="../images/note.png" alt="采购"
                                                            check="true" BorderWidth="0" OnClick="imgCT1_Click" OnClientClick="return showContact(2);" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblContactType3" runat="server" Text="技术"></asp:Label>
                                                        <asp:TextBox ID="txtCT3" runat="server" usage="" Width="160" ReadOnly="true" />
                                                        <asp:ImageButton ID="imgCT3" runat="server" ImageUrl="../images/note.png" alt="技术"
                                                            check="true" BorderWidth="0" OnClick="imgCT1_Click" OnClientClick="return showContact(3);" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblContactType4" runat="server" Text="跟单"></asp:Label>
                                                        <asp:TextBox ID="txtCT4" runat="server" usage="" Width="160" ReadOnly="true" />
                                                        <asp:ImageButton ID="imgCT4" runat="server" ImageUrl="../images/note.png" alt="跟单"
                                                            check="true" BorderWidth="0" OnClick="imgCT1_Click" OnClientClick="return showContact(4);" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" class="AddressTitle">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:re, lblProdUsage%>"></asp:Label>
                                            （紫色的文件夹代表客户有使用相关产品，点击可展开）
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" valign="top">
                                            <table id="tblProd" class="TreeProd">
                                                <thead>
                                                    <tr>
                                                        <th width="25%">
                                                            产品
                                                        </th>
                                                        <th width="20%">
                                                            用量
                                                        </th>
                                                        <th width="25%">
                                                            牌号
                                                        </th>
                                                        <th width="30%">
                                                            备注
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbProd" runat="server">
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMaterial" runat="server" Text="<%$ Resources:re, lblMaterial %>"></asp:Label>
                                            :
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtMaterial" runat="server" tip="<%$ Resources:re, MsgMaterial%>"
                                                MaxLength="255" usage="" Width="98%" TextMode="MultiLine">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCustProduct" runat="server" Text="<%$ Resources:re, lblCustProduct %>"></asp:Label>
                                            :
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtCustProduct" runat="server" MaxLength="255" usage="" Width="98%"
                                                TextMode="MultiLine">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCustInfo" runat="server" Text="<%$ Resources:re, lblCustInfo %>"></asp:Label>
                                            :
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtCustInfo" runat="server" tip="<%$ Resources:re, MsgCustInfo%>"
                                                MaxLength="255" usage="" Width="98%" TextMode="MultiLine">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCustRisk" runat="server" Text="<%$ Resources:re, lblCustRisk %>"></asp:Label>
                                            :
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtCustRisk" runat="server" tip="<%$ Resources:re, MsgCustRisk%>"
                                                MaxLength="255" usage="" Width="98%" TextMode="MultiLine">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCustBackground" runat="server" Text="<%$ Resources:re, lblCustBackground %>"></asp:Label>
                                            :
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtCustBackground" runat="server" tip="<%$ Resources:re, MsgCustBackground%>"
                                                MaxLength="255" usage="" Width="98%" TextMode="MultiLine">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td>
                                            <asp:Label ID="lblCustCity" runat="server" Text="<%$ Resources:re, lblCustCity %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCustCity" runat="server" tip="<%$ Resources:re, MsgCustCity%>"
                                                usage="" Width="98%">
					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCustDistinct" runat="server" Text="<%$ Resources:re, lblCustDistinct %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCustDistinct" runat="server" tip="<%$ Resources:re, MsgCustDistinct%>"
                                                usage="" Width="98%">
					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCustIndustryID" runat="server" Text="<%$ Resources:re, lblCustIndustryID %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="txtCustIndustryID" runat="server" tip="<%$ Resources:re, MsgCustIndustryID%>"
                                                usage="" Width="98%">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCustCDate" runat="server" Text="<%$ Resources:re, lblCustCDate %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCustCDate" runat="server" tip="<%$ Resources:re, MsgCustCDate%>"
                                                usage="notempty" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" Enabled="false"
                                                Width="98%">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCustCUserID" runat="server" Text="<%$ Resources:re, lblCustCUserID %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="txtCustCUserID" runat="server" />
                                            <asp:TextBox ID="txtCustCUserName" runat="server" tip="<%$ Resources:re, MsgCustCUserID%>"
                                                Enabled="false" usage="notempty" Width="98%">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
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
                                    <tr style="display: none;">
                                        <td>
                                            <asp:Label ID="lblCustCountryID" runat="server" Text="<%$ Resources:re, lblCustCountryID %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="txtCustCountryID" runat="server" tip="<%$ Resources:re, MsgCustCountryID%>"
                                                usage="" Width="98%">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSYSID" runat="server" Text="<%$ Resources:re, lblSYSID %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="txtSYSID" runat="server" tip="<%$ Resources:re, MsgSYSID%>"
                                                usage="notempty" Width="98%">
                                            </asp:DropDownList>
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
                    <asp:Button ID="btnTel" runat="server" Text="<%$ Resources:re, lblAction%>" check="true" Width="120px"
                        OnClick="btnTel_Click" OnClientClick="actionView(1);" />
<%--                    <asp:Button ID="btnVisit" runat="server" Text="<%$ Resources:re, lblVisitRecords%>"
                        check="true" OnClick="btnVisit_Click" OnClientClick="actionView(2);" />--%>
                    <asp:Button ID="btnDeal" runat="server" Text="<%$ Resources:re, lblDealRecords%>"
                        check="true" OnClick="btnDeal_Click" OnClientClick="dealView();" />
                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:re, lblBack%>" OnClick="btnBack_Click" />
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" Style="display: none;"
                        OnClick="btnRefresh_Click" />
                    <asp:Button ID="btnDel" runat="server" Text="<%$ Resources:re, lblDelete%>" OnClick="btnDel_Click" />
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidID" runat="server" Value="" />
    <asp:HiddenField ID="hidSaveMsg" runat="server" Value="<%$ Resources:re, MsgSaveConfirm%>" />
    <asp:HiddenField ID="hidDelMsg" runat="server" Value="<%$ Resources:re, MsgDeleteConfirm%>" />
    <asp:HiddenField ID="hidBtnDel" runat="server" Value="" />
    <script type="text/javascript">
        function actionView(actype) {
            var id = document.getElementById("<% =hidID.ClientID%>").value;
            if (id == "0")
                jAlert("请先保存客户信息，再录入电话或拜访记录");
            else {
                var txtCustName = document.getElementById("<% =txtCustName.ClientID%>").value;
                window.open("frmUserActions.aspx?CustID=" + id+"&Cust=" + txtCustName);
            }
        }
        function dealView() {
            var id = document.getElementById("<% =hidID.ClientID%>").value;
            if (id == "0")
                jAlert("请先保存客户信息，再录入成交记录");
            else
                window.open("frmCustomerDeal.aspx?CustID=" + id);
        }
        function showContact(type) {
            var id = document.getElementById("<% =hidID.ClientID%>").value;
            if (id == "0")
                jAlert("请先保存客户信息，再录入联系人");
            else
                showPopWin("联络信息", "frmContactEdit.aspx?CustID=" + id + "&TypeID=" + type + "", 500, 150, null);

            return false;
        }
    </script>
</asp:Content>
