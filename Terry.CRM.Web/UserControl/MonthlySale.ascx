<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MonthlySale.ascx.cs"
    Inherits="Terry.CRM.Web.UserControl.MonthlySale" %>
月度业绩走势<br />
<!-- Monthly Amount  -->
<asp:Chart ID="Chart1" runat="server" DataSourceID="SqlDataSource1" Width="450px">
    <Series>
        <asp:Series Name="Series1" XValueMember="Month" YValueMembers="TotalAmount" IsValueShownAsLabel="True"
            Label="$#VAL{D2}" ChartType="Line" YValuesPerPoint="4">
        </asp:Series>
    </Series>
    <ChartAreas>
        <asp:ChartArea Name="ChartArea1">
            <AxisX>
                <MajorGrid Enabled="false" />
            </AxisX>
        </asp:ChartArea>
    </ChartAreas>
</asp:Chart>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Terry.CRM.Entity.Properties.Settings.CRMConnectionString %>"
    SelectCommand="SELECT Month(DealDate) as [Month], sum(case Currency when 'RMB' then  TotalAmount/6.5 else TotalAmount end)as TotalAmount,
Currency='USD'  FROM [vw_CRMCustomerDeal] group by Month(DealDate)">
     <SelectParameters>
        <asp:QueryStringParameter DefaultValue="2011-1-1" Name="BeginDate" QueryStringField="BeginDate"
            Type="DateTime" />
        <asp:QueryStringParameter DefaultValue="2015-1-1" Name="EndDate" QueryStringField="EndDate"
            Type="DateTime" />
    </SelectParameters>
 </asp:SqlDataSource>
