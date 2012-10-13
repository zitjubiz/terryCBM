<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmRoleEdit.aspx.cs" Inherits="Terry.CRM.Web.CRM.frmRoleEdit"
    MasterPageFile="~/MasterPage/Site.Master" %>

<%@ Register Assembly="DropDownCheckList" Namespace="Terry.Web.Control" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
    <table id="wrp" cellpadding="0" cellspacing="0" align="center">
        <tr id="wrp_base">
            <td valign="top">
                <div id="wrapper">
                    <div id="main_content" class="content" style="width: 100%">
                        <div id="navbar">
                            <span id="currentModule" />
                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:re, lblHome%>" />
                            &gt;
                            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblRole%>" />
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div class="DataDetailFrom">
                            <div class="DetailDt">
                                <table width="100%" border="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRole" runat="server" Text="<%$ Resources:re, lblRole %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRole" runat="server" tip="<%$ Resources:re, MsgRole%>" usage="notempty">
					
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            角色等级
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlGrade" runat="server" AutoPostBack="true" 
                                                onselectedindexchanged="ddlGrade_SelectedIndexChanged">
                                                <asp:ListItem Value="1">销售人员(只能看到本人以及下属的客户)</asp:ListItem>
                                                <asp:ListItem Value="2">产品经理(能看到有权限的产品对应的区域客户)</asp:ListItem>
                                                <%--<asp:ListItem Value="3">部门经理(能看到有权限的部门对应的客户)</asp:ListItem>
                                                <asp:ListItem Value="7">财务(只能看到财务信息)</asp:ListItem>--%>
                                                <asp:ListItem Value="4">销售总监(能看到下属客户，修改客户系数)</asp:ListItem>
                                                <asp:ListItem Value="8">人事(只能看到员工信息)</asp:ListItem>
                                                <asp:ListItem Value="9">行政管理层(能看到所有客户信息)</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trProd" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lblProduct" runat="server" Text="<%$ Resources:re, lblProduct %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <cc2:DropDownCheckList ID="DDCLProduct" runat="server" ClientCodeLocation="../js/DropDownCheckList.js"
                                                DropImageSrc="../images/expand.gif" RepeatColumns="2" DisplayTextWidth="380"
                                                Width="100%" TextWhenNoneChecked="-----Select-----" CheckListCssClass="checkbox">
                                            </cc2:DropDownCheckList>
                                        </td>
                                        <td colspan="2" width="300px">
                                            <asp:Label ID="lblCustProvince" runat="server" Text="<%$ Resources:re, lblCustProvince %>"></asp:Label>
                                            :                                            
                                            <cc2:DropDownCheckList ID="DDCLProvince" runat="server" ClientCodeLocation="../js/DropDownCheckList.js"
                                                DropImageSrc="../images/expand.gif" RepeatColumns="4" DisplayTextWidth="380"
                                                Width="100%" TextWhenNoneChecked="-----Select-----" CheckListCssClass="checkbox">
                                            </cc2:DropDownCheckList>
                                        </td>
                                    </tr>
                                    <tr id="trDep" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:re, lblDepartment %>"></asp:Label>
                                            :
                                        </td>
                                        <td>
                                            <cc2:DropDownCheckList ID="DDCLDep" runat="server" ClientCodeLocation="../js/DropDownCheckList.js"
                                                DropImageSrc="../images/expand.gif" RepeatColumns="2" DisplayTextWidth="400"
                                                Width="100%" TextWhenNoneChecked="-----Select-----" CheckListCssClass="checkbox">
                                            </cc2:DropDownCheckList>
                                        </td>
                                        <td colspan="2" width="300px">
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap">
                                            <asp:Label ID="lblModuleRight" runat="server" Text="<%$ Resources:re, lblModuleRight %>"></asp:Label>
                                            :
                                        </td>
                                        <td colspan="3">
                                            <cc2:DropDownCheckList ID="DDCLModule" runat="server" ClientCodeLocation="../js/DropDownCheckList.js"
                                                DropImageSrc="../images/expand.gif" RepeatColumns="4" DisplayTextWidth="400"
                                                Width="100%" TextWhenNoneChecked="-----Select-----" CheckListCssClass="checkbox" Visible="false">
                                            </cc2:DropDownCheckList>
                                            <asp:Repeater ID="rptMod" runat="server">
                                                <ItemTemplate>
                                                    <div>
                                                        <div style="float: left; width: 150px;">
                                                            <asp:HiddenField ID="HidID" runat="server" Value='<%# Eval("ModuleID") %>' />
                                                            <asp:Label ID="lblMod" runat="server" Text='<%# Eval("Module") %>' />
                                                        </div>
                                                        <div>
                                                            <asp:CheckBoxList ID="cblRight" runat="server" RepeatDirection="Horizontal">
                                                                <asp:ListItem Text="只读" Value="1" />
                                                                <asp:ListItem Text="新建" Value="2" />
                                                                <asp:ListItem Text="编辑" Value="3" />
                                                                <asp:ListItem Text="删除" Value="4" />
                                                            </asp:CheckBoxList>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                </table>
                                <div id="divErrorMessage">
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
                        <asp:Button ID="btnDel" runat="server" Text="<%$ Resources:re, lblDelete%>" OnClick="btnDel_Click" />
                        <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:re, lblBack%>" OnClick="btnBack_Click" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidID" runat="server" Value="" />
</asp:Content>
