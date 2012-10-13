<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmSchedule.aspx.cs" Inherits="Terry.CRM.Web.CRM.frmSchedule"
    MasterPageFile="~/MasterPage/Site.Master" %>
<%@ Register Assembly="BaseControls" Namespace="BaseControls" TagPrefix="bc" %>
<%@ Register Assembly="Terry.WebControls.DropDownList" Namespace="Terry.WebControls"
    TagPrefix="tag" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH1" runat="server">
    <link href="../../css/BaseCalendar.css" rel="stylesheet" type="text/css" media="screen" />
<script type="text/javascript">
    function EditTask(id,day,person)
    {
     showPopWin(day,"frmTaskEdit.aspx?day="+ day +"&person=" + person+"&id="+id , 350, 160, null);
    }

</script>

    <table id="wrp" cellpadding="0" cellspacing="0" align="center" style="height: 500px">
        <tr id="wrp_base">
            <td valign="middle" align="center">
            <div style="float: right">
                <asp:Label ID="lblCustOwnerID" runat="server" Text="<%$ Resources:re, lblCustOwnerID %>"></asp:Label>
                :<tag:DropDownList ID="txtCustOwnerID" AutoPostBack="true" runat="server" tip="<%$ Resources:re, MsgCustOwnerID%>"
                    usage="notempty" Width="120px" 
                    onselectedindexchanged="txtCustOwnerID_SelectedIndexChanged">
                </tag:DropDownList>
            </div>            
              <bc:BaseCalendar ID="BaseCalendar1" runat="server" 
                NavPrevFormat='<a href="frmSchedule.aspx?d={0:d}">{0:MMM}</a>'
                NavNextFormat='<a href="frmSchedule.aspx?d={0:d}">{0:MMM}</a>'
                UrlVisibleDateAttribute="d"  
                
                CssClass="simple"  
                
                CssHeaderNavigation="simple_nav_row"
                CssHeaderNavigationPrevious="simple_navprev"
                CssHeaderNavigationCurrent="simple_navcurrent"
                CssHeaderNavigationNext="simple_navnext"
                
                CssHeaderDayOfWeek="simple_dof"
                
                CssBodyDayOtherMonth="simple_other_month"
                CssBodyDayToday="simple_today"  
                CssBodyDayWeekend="simple_weekend" onrenderbodyday="BaseCalendar1_RenderBodyDay"    
              />
            </td>
        </tr>
    </table>
    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" Style="display: none;"
    OnClick="btnRefresh_Click" />
</asp:Content>
