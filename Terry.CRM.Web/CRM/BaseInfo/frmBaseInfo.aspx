<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmBaseInfo.aspx.cs" Inherits="Terry.CRM.Web.CRM.frmBaseInfo"
    MasterPageFile="~/MasterPage/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
    <table id="wrp" cellpadding="0" cellspacing="0" align="center" height="460">
        <tr id="wrp_base">
            <td valign="top">
                <div id="wrapper">
                    <table align="center" width="80%" cellpadding="10" cellspacing="10" style="padding-top: 100px;">
                        <tr>
                            <td align="center">
                                <asp:HyperLink ID="lnk1" runat="server" NavigateUrl="~/CRM/BaseInfo/frmContactType.aspx"><img src="../../images/notepad.jpg" border="0" alt=""/></asp:HyperLink>
                            </td>
                            <td align="center">
                                <asp:HyperLink ID="lnk2" runat="server" NavigateUrl="~/CRM/BaseInfo/frmCustomerEmpNum.aspx"><img alt="" src="../../images/notepad.jpg"  border="0"/></asp:HyperLink>
                            </td>
                            <td align="center">
                                <asp:HyperLink ID="lnk3" runat="server" NavigateUrl="~/CRM/BaseInfo/frmCustomerFrom.aspx"><img alt="" src="../../images/notepad.jpg"  border="0"/></asp:HyperLink>
                            </td>
                            <td align="center">
                                <asp:HyperLink ID="lnk5" runat="server" 
                                    NavigateUrl="~/CRM/BaseInfo/frmProvince.aspx"><img alt="" src="../../images/notepad.jpg"  border="0"/></asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:re, lblContactType%>" />
                            </td>
                            <td align="center">
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:re, lblCustomerEmpNum%>" />
                            </td>
                            <td align="center">
                                <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:re, lblCustomerFrom%>" />
                            </td>
                            <td align="center">
                                <asp:Literal ID="Literal7" runat="server" 
                                    Text="<%$ Resources:re, lblCustProvince%>" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/CRM/BaseInfo/frmCustomerStatus.aspx"><img src="../../images/notepad.jpg" border="0" alt=""/></asp:HyperLink>
                            </td>
                            <td align="center">
                                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/CRM/BaseInfo/frmCustomerType.aspx"><img alt="" src="../../images/notepad.jpg"  border="0"/></asp:HyperLink>
                            </td>
                            <td align="center">
                                <asp:HyperLink ID="lnk4" runat="server" NavigateUrl="~/CRM/BaseInfo/frmCustomerRelation.aspx"><img alt="" src="../../images/notepad.jpg"  border="0"/></asp:HyperLink>
                            </td>
                            <td align="center">
                            <asp:HyperLink ID="HyperLink3" runat="server" 
                                    NavigateUrl="~/CRM/BaseInfo/frmCountry.aspx"><img alt="" src="../../images/notepad.jpg"  border="0"/></asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:re, lblCustomerStatus%>" />
                            </td>
                            <td align="center">
                                <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:re, lblCustomerType%>" />
                            </td>
                            <td align="center">
                                <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:re, lblCustomerRelation%>" />
                            </td>
                            <td align="center">
                             <asp:Literal ID="Literal8" runat="server" 
                                    Text="<%$ Resources:re, lblCountry%>" />
                            </td>
                        </tr>
                        <%--                        <tr>
                            <td align="center">
                                <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/CRM/frmProduct.aspx"><img src="../images/GIN.png" border="0" alt=""/></asp:HyperLink>
                            </td>
                            <td align="center">
                            </td>
                            <td align="center">
                            </td>
                            <td align="center">
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Literal ID="Literal9" runat="server" Text="<%$ Resources:re, lblProduct%>" />
                            </td>
                            <td align="center">
                            </td>
                            <td align="center">
                            </td>
                            <td align="center">
                            </td>
                        </tr> --%>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
