<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Top10Sale.ascx.cs" Inherits="Terry.CRM.Web.UserControl.Top10Sale" %>
销售业绩前10名<br />
<!-- TOP 10 Sale  -->
<asp:Chart ID="Chart1" runat="server" DataSourceID="SqlDataSource1" 
    Width="900px" Height="480px">
    <Series>
        <asp:Series Name="Series1" XValueMember="DealOwnerName" YValueMembers="TotalAmount" 
            IsValueShownAsLabel="True" Label="€#VAL{D2}">
        </asp:Series>
    </Series>
    <ChartAreas>
        <asp:ChartArea Name="ChartArea1">
            <AxisX IsLabelAutoFit="False" IntervalAutoMode="VariableCount">
                <MajorGrid  Enabled="false" />
                <LabelStyle Angle="0" Interval="1" />
            </AxisX>
            <AxisY>
                <MajorGrid Enabled="false" />
            </AxisY>
        </asp:ChartArea>
    </ChartAreas>
</asp:Chart>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Terry.CRM.Entity.Properties.Settings.CRMConnectionString %>"
    SelectCommand="SELECT TOP 10 [DealOwnerName], sum(case Currency when 'RMB' then  TotalAmount/9 else TotalAmount end)as TotalAmount,
Currency='EUR'  FROM vw_CRMCustomerDeal where DealDate<=@EndDate and DealDate>=@BeginDate group by DealOwnerName order by TotalAmount desc">
    <SelectParameters>
        <asp:QueryStringParameter DefaultValue="2011-1-1" Name="BeginDate" QueryStringField="BeginDate"
            Type="DateTime" />
        <asp:QueryStringParameter DefaultValue="2015-1-1" Name="EndDate" QueryStringField="EndDate"
            Type="DateTime" />
    </SelectParameters>
</asp:SqlDataSource>
