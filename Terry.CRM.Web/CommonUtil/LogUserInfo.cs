using System;
using System.Data;
using System.Configuration;
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
    public class LogUserInfo
    {
        public long LoginUserID { get; set; }
        public string LoginUserName { get; set; }
        public string LoginUserFullName { get; set; }
        public long LoginUserRoleID { get; set; }
        public int LoginUserRoleGrade { get; set; }
        public string LoginUserCompany { get; set; }
    }
}
