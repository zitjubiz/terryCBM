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

namespace Terry.CRM.Web
{
    public partial class AccessDeny : BasePage
    {
        public string ErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrMsg = GetREMes("MsgAccessDeny");
            ShowMessage(ErrMsg);
        }

    }
}
