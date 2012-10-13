using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Terry.CRM.Web.UserControl
{
    public partial class Top10Sale : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string USD2RMB = ConfigurationManager.AppSettings["USD2RMB"];
            string EUR2RMB = ConfigurationManager.AppSettings["EUR2RMB"];
            string Currency = ConfigurationManager.AppSettings["Currency"].ToUpper();
            string Filter = "  where DealDate<=@EndDate and DealDate>=@BeginDate ";
            if (string.IsNullOrEmpty(Request["DepID"]) == false)
                Filter += " and DepID=" + Request["DepID"];

            if (Currency == "USD")
            {
                this.Chart1.Series[0].Label = "$#VAL{D2}";
                SqlDataSource1.SelectCommand = @"SELECT TOP 10 [DealOwnerName],
sum(case Currency when 'RMB' then  TotalAmount/" + USD2RMB + @" 
when 'EUR' then TotalAmount*" + EUR2RMB + "/" + USD2RMB + @" else TotalAmount end)as TotalAmount,
Currency='USD'   FROM [vw_CRMCustomerDeal]" + Filter + " group by DealOwnerName order by TotalAmount desc";
            }
            else if (Currency == "EUR")
            {
                this.Chart1.Series[0].Label = "€#VAL{D2}";
                SqlDataSource1.SelectCommand = @"SELECT TOP 10 [DealOwnerName],
sum(case Currency when 'RMB' then  TotalAmount/" + EUR2RMB + @" 
when 'USD' then TotalAmount*" + USD2RMB + "/" + EUR2RMB + @" else TotalAmount end)as TotalAmount,
Currency='EUR'   FROM [vw_CRMCustomerDeal]" + Filter + " group by DealOwnerName order by TotalAmount desc";

            }
            else if (Currency == "RMB")
            {
                this.Chart1.Series[0].Label = "￥#VAL{D2}";
                SqlDataSource1.SelectCommand = @"SELECT TOP 10 [DealOwnerName],
sum(case Currency when 'EUR' then  TotalAmount*" + EUR2RMB + @" 
when 'USD' then TotalAmount*" + USD2RMB + @" else TotalAmount end)as TotalAmount,
Currency='RMB'   FROM [vw_CRMCustomerDeal] " + Filter + " group by DealOwnerName order by TotalAmount desc";
            }
        }
    }
}