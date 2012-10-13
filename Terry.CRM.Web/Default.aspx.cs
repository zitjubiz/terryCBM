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
using Terry.CRM.Entity;
using Terry.CRM.Service;

namespace Terry.CRM.Web
{
    public partial class _Default : BasePage
    {
        private BaseService svr = new BaseService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (Session[Session_ID] == null)
                {
                    ShowMessage(GetREMes("MsgSessionTimeOut"));
                    Response.Write("<script>parent.window.location.href='Login.aspx'</script>");
                    Response.End();
                    return;
                }
                getAnnouce();
                ShowExpiryAlert();

                if (this.Industry == "Chemical")
                {
                    lnk2.NavigateUrl = "CRM_Chem/frmCustomer.aspx";
                    lnk4.NavigateUrl = "CRM/GTD/frmSchedule.aspx";
                    Literal4.Text = this.GetREMes("lblSchedule");
                }
                else
                {
                    lnk2.NavigateUrl = "CRM/frmCustomer.aspx";
                    lnk4.NavigateUrl = "http://task.2simplework.com";
                    Literal4.Text = this.GetREMes("lblTaskManage");
                }

            }
        }
        private void ShowExpiryAlert()
        {
            string Js = "";
            DataTable dt = svr.GetTopN(typeof(CRMSystem), 1, "", "SYSID desc");
            if (dt.Rows.Count == 1)
            {
                DateTime dtExpiry = DateTime.Parse(dt.Rows[0]["SYSExpiryDate"].ToString());
                TimeSpan ts = dtExpiry -DateTime.Now;
                if (ts.Days < 45 && ts.Days >= 0)
                    Js = @"系统的保修期还有" + ts.Days + "天，请联系zitjubiz@hotmail.com获得延期保修服务";
                if (ts.Days < 0)
                    Js = @"系统已过保修期，为保证您的服务质量，请迅速联系13711792205获得延期保修服务";

                
            }
            else
            {
                Js = @"你使用的是没有注册售后服务的系统，为保证您的服务质量，请迅速联系zitjubiz@hotmail.com获得保修服务";
            }
            lblExpiry.Text = Js;
            
        }
        private void getAnnouce()
        {
            DataTable dt = svr.GetTopN(typeof(CRMAnnouce), 1, "", "ID desc");
            if (dt.Rows.Count == 1)
            {
                lblSubject.Text = dt.Rows[0]["subject"].ToString();
                lblContent.Text = dt.Rows[0]["ContentDesc"].ToString();
            }
        }
    }
}
