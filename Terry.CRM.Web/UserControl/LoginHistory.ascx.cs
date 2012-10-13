using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Terry.CRM.Entity;
using Terry.CRM.Service;

namespace Terry.CRM.Web.UserControl
{
    public partial class LoginHistory : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ObjectDataSource1.SelectParameters["Begin"].DefaultValue = DateTime.Now.AddMonths(-3).ToString();
            ObjectDataSource1.SelectParameters["End"].DefaultValue = DateTime.Now.AddDays(1).ToString();
            gvLoginHistory.DataBind();
        }
    }
}
