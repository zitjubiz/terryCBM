using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Terry.CRM.Service;
using Terry.CRM.Entity;

namespace Terry.CRM.Web.CRM
{
    public partial class frmReport : BasePage
    {
        BaseService svr = new BaseService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Authentication(enumModule.Report);

                txtBeginDate.Text = Request["BeginDate"];
                txtEndDate.Text = Request["EndDate"];
                ddlDept.BindDropDownListAndSelect(svr.GetDepartment(), "DepName", "DepID");
                ddlDept.Text = Request["DepID"];
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DateTime dtBegin, dtEnd;
            if(txtBeginDate.Text!="")
                dtBegin =DateTime.Parse(txtBeginDate.Text);
            else
                dtBegin = DateTime.Now.AddMonths(-1);

            if(txtEndDate.Text!="")
                dtEnd =DateTime.Parse(txtEndDate.Text);
            else
                dtEnd = DateTime.Now;

            if(ddlDept.SelectedValue!="")
                Response.Redirect("frmReport.aspx?BeginDate=" + dtBegin.ToString("yyyy-MM-dd") + "&EndDate=" + dtEnd.ToString("yyyy-MM-dd")+"&DepID="+ ddlDept.SelectedValue);
            else
                Response.Redirect("frmReport.aspx?BeginDate=" + dtBegin.ToString("yyyy-MM-dd") + "&EndDate=" + dtEnd.ToString("yyyy-MM-dd"));
        }
    }
}
