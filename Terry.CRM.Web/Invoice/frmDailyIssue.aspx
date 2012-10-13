<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true"
    CodeBehind="frmDailyIssue.aspx.cs" Inherits="Terry.CRM.Web.Invoice.frmDailyIssue" %>

<%@ Register Assembly="Terry.WebControls.DropDownList" Namespace="Terry.WebControls"
    TagPrefix="tag" %>
<%@ Register Assembly="PagingGridView" Namespace="Terry.Web.Control" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
    <script type="text/javascript">
        function init() {
            // 在这里写你的代码...

            var node = document.getElementById("ctl00_CPH1_gvIssue_ctl02_txtFlightTicketNum");

            if (node != null)
                addEvent(node, 'paste', function (e) { OnPaste('ctl00_CPH1_gvIssue_ctl02_btnPaste', e); });


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
                var ctrlId = (i + 2);
                if (ctrlId.toString().length == 1)
                    ctrlId = "0" + ctrlId;
                var opCells = opRows[i].split(opTab);
                var prefix = buttonid.substring(0, buttonid.length - 8);
                var textboxs = $("input[id*='ctl00_CPH1_gvIssue_ctl" + ctrlId + "_txt']");

                for (var q = 0; q < opCells.length; q++) {
                    textboxs[q].value = opCells[q];

                }
            }
        }

    </script>
    <table id="wrp" cellpadding="0" cellspacing="0" align="center" style="height: 440px">
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
                                        <asp:Literal ID="Literal2" runat="server" Text="每日出票/对账" />
                                    </td>
                                    <td align="right" nowrap="nowrap">
                                        <div style="float: right;display:none;">
                                            <asp:Label ID="lblIssueDate" runat="server" Text="出票日期"></asp:Label>
                                            :<asp:TextBox ID="txtIssueDate" runat="server" AutoComplete="off" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                                                usage="notempty" Width="80"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <div class="sidebar_header_search" style="height: 60px;">
                            <div align="left" style="float: left; height: 50px;">
                            1. 下载<asp:HyperLink ID="lnkTemplate" runat="server" NavigateUrl="~/Invoice/Daily.xls"
                                                Target="_blank">Excel模板</asp:HyperLink><br />
                            2.根据模板填写每日出票记录文件.<br />
                            3.然后点击<b>[浏览.../选择文件]</b>选择刚填写好excel文件,点击<b>[上传]</b>,系统会自动导入记录.<br />
                                
                            </div>
                            <div align="right" style="float: right">
                                <table width="200px">
                                    <tr>
                                        <td>
                                            <asp:FileUpload ID="FileUpload1" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="上传" />
                                            
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div id="Grid" style="float: left; display:none;">
                            <table id="tblTicketTour" runat="server" width="100%" cellpadding="2" cellspacing="2">
                                <tr>
                                    <td colspan="6" class="AddressTitle">
                                        <asp:Label ID="Label1" runat="server" Text="每日出票信息 "></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Button ID="btnPaste" runat="server" Text="粘贴" OnClientClick="pasteData(this.id,event);" Visible="false" />
                                        <asp:GridView ID="gvIssue" runat="server" AutoGenerateColumns="False" CssClass="tableBorder"
                                            HeaderStyle-CssClass="Head" RowStyle-CssClass="Row" DataKeyNames="ID" FooterStyle-CssClass="Foot"
                                            Width="100%" ShowFooter="False" Visible="false">
                                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="30"></PagerSettings>
                                            <RowStyle CssClass="Row"></RowStyle>
                                            <Columns>
                                                <asp:TemplateField HeaderText="机票号">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFlightTicketNum" runat="server" Width="98%" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amadeus预订号">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtOuterReferenceID" runat="server" Width="98%" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="成本">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCost" runat="server" Width="98%" usage="^num" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="乘客">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtOwnerName" runat="server" Width="98%" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="20%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="内部订单号">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtInnerReferenceID" runat="server" Width="98%" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="20%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="银行对账信息">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBankStatement" runat="server" Width="98%" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="20%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle CssClass="Foot"></FooterStyle>
                                            <PagerStyle CssClass="Pager"></PagerStyle>
                                            <HeaderStyle CssClass="Head"></HeaderStyle>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="clearer">
                        <!--  -->
                    </div>
                    <div class="button">
                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:re, lblSave%>" check="true"
                            OnClick="btnSave_Click" Visible="false" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidSaveMsg" runat="server" Value="<%$ Resources:re, MsgSaveConfirm%>" />
    <asp:HiddenField ID="hidDelMsg" runat="server" Value="<%$ Resources:re, MsgDeleteConfirm%>" />
    <asp:HiddenField ID="hidBtnDel" runat="server" Value="" />
</asp:Content>
