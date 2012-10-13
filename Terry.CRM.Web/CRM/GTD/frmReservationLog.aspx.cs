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
using MemDBHandler;
using MemDBEntity;
using MemDBCommon;
using MemDBSystem.common;

namespace MemDBSystem.frm
{
    public partial class frmReservationLog : frmBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";

            bindLog();
        }

        private void bindLog()
        {
            ReservationHandler h = new ReservationHandler();
            DataTable dt =h.ReadLog(DateTime.Parse(Request["date"]));
            Repeater1.DataSource = dt;
            Repeater1.DataBind();
        }
    }
}
