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
    public partial class frmReservationSMS : System.Web.UI.Page
    {
        ReservationHandler resvh = new ReservationHandler();
        protected void Page_Load(object sender, EventArgs e)
        {
            txtSMS.Text = resvh.loadReservationSMS(long.Parse(Request["id"]));
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            //update reservation set SendSMS=1
            resvh.sendSMS(long.Parse(Request["id"]));
        }
    }
}
