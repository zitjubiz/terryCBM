<%@ Page Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="frmSendEmail.aspx.cs" Inherits="Terry.CRM.Web.CRM.frmSendEmail" %>

<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
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
                            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblEmail%>" />
                        </div>
                        <div id="toolbar" align="right">
                        </div>
                        <div class="DataDetailFrom">
                            <table id="Table1" cellspacing="2" cellpadding="2" width="100%" align="center" border="0">
                                <tr style="height: 29px" bgcolor="#dde9ea">
                                    <td align="center" colspan="3">
                                        <font size="5"><strong>邮件营销</strong></font>
                                    </td>
                                </tr>
                                <tr style="height: 26px" bgcolor="#dde9ea">
                                    <td>
                                        Email客户群
                                    </td>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="radEmailGroup" runat="server" AutoPostBack="True" 
                                                        RepeatDirection="Horizontal" 
                                                        onselectedindexchanged="radEmailGroup_SelectedIndexChanged">
                                                        <asp:ListItem Value="New" Selected="True">客户搜索结果</asp:ListItem>
                                                        <asp:ListItem Value="Existing">已存客户群</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlEmailGroup" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlEmailGroup_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 26px" bgcolor="#dde9ea">
                                    <td>
                                        Email 列表
                                    </td>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:ListBox ID="Listmail" runat="server" Width="550px"></asp:ListBox>
                                                </td>
                                                <td style="display: none">
                                                    <asp:TextBox ID="txtOthersAddEmail" runat="server"></asp:TextBox>&nbsp;
                                                    <asp:Button ID="btnAddEmail" runat="server" Text="增加email"></asp:Button>
                                                    <br />
                                                    <br />
                                                    <asp:Button ID="btnDelEmail" runat="server" Text="删除选中的email"></asp:Button>&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnDelAll" runat="server" Text="删除列表里面所有email"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="PanelOthers" runat="server">
                                            
                                            <%--可以从txt文本文件(格式为每行一个email地址)导入email地址
                                                    <asp:FileUpload ID="fileEmail" runat="server" Width="150px" />
                                                    <asp:Button ID="btnImport" runat="server" Text="从txt文件导入email地址"></asp:Button>
                                                    <br />--%>
                                            保存成名字为<asp:TextBox ID="txtGroupName" runat="server"></asp:TextBox>的客户群<asp:Button
                                                ID="btnSaveGroup" Text="保存客户群" runat="server" OnClick="btnSaveGroup_Click" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr style="height: 26px" bgcolor="#dde9ea">
                                    <td>
                                        BCC Email(密送)
                                        <br />
                                        (不包括在群里)
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtEmailAdditional" Width="550px" runat="server"></asp:TextBox><font
                                            color="#ff0000"></font>
                                    </td>
                                </tr>
                                <tr style="height: 26px" bgcolor="#dde9ea">
                                    <td>
                                        主题
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtSubject" runat="server" Width="550px" MaxLength="255"></asp:TextBox><font
                                            color="#ff0000">*</font>
                                    </td>
                                </tr>
                                <tr style="height: 26px" bgcolor="#dde9ea">
                                    <td>
                                        <p>
                                            内容</p>
                                    </td>
                                    <td nowrap>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <fckeditorv2:FCKeditor ID="txtbody" runat="server" Width="100%" Height="250px" ToolbarSet="Basic"
                                                        BasePath="../FCKeditor/">
                                                    </fckeditorv2:FCKeditor>
                                                </td>
                                                <td>
                                                    <font color="#ff0000">*</font>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 26px" bgcolor="#dde9ea">
                                    <td style="height: 29px">
                                        附件1(小于2M)
                                    </td>
                                    <td style="height: 29px" colspan="2">
                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                    </td>
                                </tr>
                                <tr style="height: 26px" bgcolor="#dde9ea">
                                    <td>                                        
                                        附件2(小于2M)
                                    </td>
                                    <td colspan="2">
                                        <asp:FileUpload ID="FileUpload2" runat="server" />
                                    </td>
                                </tr>
                                <tr style="height: 26px" bgcolor="#dde9ea">
                                    <td>
                                        附件3(小于2M)
                                    </td>
                                    <td colspan="2">
                                        <asp:FileUpload ID="FileUpload3" runat="server" />
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
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:re, lblBack%>" OnClientClick="history.back();" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidID" runat="server" Value="" />
    <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
    <asp:HiddenField ID="HiddenField2" runat="server" Value="" />
    <asp:HiddenField ID="HiddenField3" runat="server" Value="" />
</asp:Content>
