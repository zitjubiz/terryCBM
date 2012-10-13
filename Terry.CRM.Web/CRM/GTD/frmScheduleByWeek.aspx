<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmScheduleByWeek.aspx.cs"
    Inherits="Terry.CRM.Web.CRM.frmScheduleByWeek" MasterPageFile="~/MasterPage/Site.Master" %>

<%@ Register Assembly="BaseControls" Namespace="BaseControls" TagPrefix="bc" %>
<%@ Register Assembly="Terry.WebControls.DropDownList" Namespace="Terry.WebControls"
    TagPrefix="tag" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
    <link href="../../css/BaseCalendar.css" rel="stylesheet" type="text/css" media="screen" />
    <script type="text/javascript">
        function EditTask(id, day, person) {
            showPopWin(day, "frmTaskEdit.aspx?day=" + day + "&person=" + person + "&id=" + id, 350, 160, null);
        }

    </script>
    <table id="wrp" cellpadding="0" cellspacing="0" align="center" style="height: 500px">
        <tr id="wrp_base">
            <td valign="top" align="center">
                <div style="float: right">
                    <asp:Label ID="lblCustOwnerID" runat="server" Text="<%$ Resources:re, lblCustOwnerID %>"></asp:Label>
                    :<tag:DropDownList ID="txtCustOwnerID" AutoPostBack="true" runat="server" tip="<%$ Resources:re, MsgCustOwnerID%>"
                        usage="notempty" Width="120px" OnSelectedIndexChanged="txtCustOwnerID_SelectedIndexChanged">
                    </tag:DropDownList>
                </div>
                2012年8月27日 – 9月2日 农历七月十一 ~ 七月十七<br />
                <asp:Repeater ID="rptWeek" runat="server">
                <HeaderTemplate>
                <table class="simple" border=1>
                <tr class="wk-daynames">
                            <td class="wk-tzlabel" style="width: 60px" >
                                &nbsp;</td>
                            <th title="8/27 (周一) 十一" scope="col">
                                <div class="wk-dayname">
                                    <span class="ca-cdp21787 wk-daylink" style="cursor: pointer; text-decoration: underline;">
                                        8/27 (周一) </span></div>
                            </th>
                            <th title="8/28 (周二) 十二" scope="col">
                                <div class="wk-dayname wk-today">
                                    <span class="ca-cdp21788 wk-daylink" style="cursor: pointer;">8/28 (周二) </span></div>
                            </th>
                            <th title="8/29 (周三) 十三" scope="col">
                                <div class="wk-dayname wk-tomorrow">
                                    <span class="ca-cdp21789 wk-daylink" style="cursor: pointer;">8/29 (周三) </span></div>
                            </th>
                            <th title="8/30 (周四) 十四" scope="col">
                                <div class="wk-dayname">
                                    <span class="ca-cdp21790 wk-daylink" style="cursor: pointer;">8/30 (周四)</span></div>
                            </th>
                            <th title="8/31 (周五) 十五" scope="col">
                                <div class="wk-dayname">
                                    <span class="ca-cdp21791 wk-daylink" style="cursor: pointer;">8/31 (周五) </span></div>
                            </th>
                            <th title="9/1 (周六) 十六" scope="col">
                                <div class="wk-dayname">
                                    <span class="ca-cdp21793 wk-daylink" style="cursor: pointer;">9/1 (周六) </span></div>
                            </th>
                            <th title="9/2 (周日) 十七" scope="col">
                                <div class="wk-dayname">
                                    <span class="ca-cdp21794 wk-daylink" style="cursor: pointer;">9/2 (周日) </span></div>
                            </th>
                            <th class="wk-dummyth"  style="width: 17px;">
                                &nbsp;
                            </th>
                        </tr>
                        </table>
                </HeaderTemplate>
                <ItemTemplate>
                <tr><td>ddd</td></tr>
                </ItemTemplate>
                </asp:Repeater>
       
            </td>
        </tr>
    </table>
    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" Style="display: none;"
        OnClick="btnRefresh_Click" />
</asp:Content>
