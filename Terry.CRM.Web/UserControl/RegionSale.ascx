<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegionSale.ascx.cs" Inherits="Terry.CRM.Web.UserControl.RegionSale" %>
地区业绩比例<br />
<asp:Chart ID="Chart1" runat="server" DataSourceID="SqlDataSource1" 
    Width="450px">
    <Series>
        <asp:Series Name="Series1" XValueMember="CustProvince" 
            YValueMembers="TotalAmount" ChartType="Doughnut" ChartArea="ChartArea1" 
            IsValueShownAsLabel="True" Label=" #VALX  #PERCENT{P}">
        </asp:Series>
    </Series>
    <ChartAreas>
        <asp:ChartArea Name="ChartArea1">
        </asp:ChartArea>
    </ChartAreas>
</asp:Chart>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Terry.CRM.Entity.Properties.Settings.CRMConnectionString %>"
    SelectCommand="SELECT  [CustProvince], sum(case Currency when 'RMB' then  TotalAmount/6.5 else TotalAmount end)as TotalAmount,
Currency='USD'  FROM [vw_CRMCustomerDeal] d inner join dbo.vw_CRMCustomer c on 
d.custid=c.custid where DealDate<=@EndDate and DealDate>=@BeginDate group by [CustProvince] order by TotalAmount desc">
    <SelectParameters>
        <asp:QueryStringParameter DefaultValue="2011-1-1" Name="BeginDate" QueryStringField="BeginDate"
            Type="DateTime" />
        <asp:QueryStringParameter DefaultValue="2015-1-1" Name="EndDate" QueryStringField="EndDate"
            Type="DateTime" />
    </SelectParameters>
</asp:SqlDataSource>
