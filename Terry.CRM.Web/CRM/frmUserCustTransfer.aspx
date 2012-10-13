<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmUserCustTransfer.aspx.cs"
    Inherits="Terry.CRM.Web.CRM.frmUserCustTransfer" MasterPageFile="~/MasterPage/Site.Master" %>

<%@ Register Assembly="Terry.WebControls.DropDownList" Namespace="Terry.WebControls"
    TagPrefix="tag" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
<script type="text/javascript">
    function toggleAll(CheckBoxList) {
        var chkElements =document.getElementById(CheckBoxList).getElementsByTagName("input");
        for (var i = 0; i < chkElements.length; i++) {

            if (chkElements[i].type == "checkbox")
                chkElements[i].checked = ! chkElements[i].checked;
        }
        return false;
    }
</script>
    <table id="wrp" cellpadding="0" cellspacing="0" align="center" style="height: 460px">
        <tr id="wrp_base">
            <td valign="top">
                <div id="wrapper">
                    <div id="main_content" class="content" style="width: 100%">
                        <div id="navbar">
                            <span id="currentModule">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:re, lblHome%>" />
                                &gt;
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblUserCustTransfer%>" />
                            </span>
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div id="Grid">
                            <asp:Wizard ID="wizTran" runat="server" 
                                onactivestepchanged="wizTran_ActiveStepChanged" 
                                onfinishbuttonclick="wizTran_FinishButtonClick" >
                                <WizardSteps>
                                    <asp:WizardStep StepType="Start">
                                        Step 1: 选择转出员工，<tag:DropDownList ID="ddlFromUser" runat="server" DataTextField="UserName"
                                            DataValueField="UserID">
                                        </tag:DropDownList>
                                    </asp:WizardStep>
                                    <asp:WizardStep StepType="Step">
                                        Step 2: 选择其客户
                                        <asp:button ID="btnToggle" runat="server" Text="全选" OnClientClick="return toggleAll('ctl00_CPH1_wizTran_cblCustomers');" />
                                        <br />
                                        <asp:CheckBoxList ID="cblCustomers" runat="server" DataTextField="CustName"
                                            DataValueField="CustID" RepeatColumns="3">
                                        </asp:CheckBoxList>
                                    </asp:WizardStep>
                                    <asp:WizardStep StepType="Step">
                                        Step 3: 选择转入员工<tag:DropDownList ID="ddlToUser" runat="server" DataTextField="UserName"
                                            DataValueField="UserID">
                                        </tag:DropDownList>
                                    </asp:WizardStep>
                                    <asp:WizardStep StepType="Finish">
                                        Step 4: <asp:Literal ID="lblUserCustTransferDesc" runat="server" Text="<%$ Resources:re, lblUserCustTransferDesc%>" />
                                        
                                        <br />
                                        <asp:CheckBox ID="chkDelUser" runat="server" Text="同时删除该员工" />
                                    </asp:WizardStep>
                                </WizardSteps>
                            </asp:Wizard>
                        </div>
                    </div>
                    <div class="clearer">
                        <!--  -->
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidSaveMsg" runat="server" Value="<%$ Resources:re, MsgSaveConfirm%>" />
    <asp:HiddenField ID="hidDelMsg" runat="server" Value="<%$ Resources:re, MsgDeleteConfirm%>" />
    <asp:HiddenField ID="hidBtnDel" runat="server" Value="" />
</asp:Content>
