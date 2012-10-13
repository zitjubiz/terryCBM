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
using MemDBCommon;
using MemDBEntity;
using MemDBHandler;
using MemDBSystem.common;

namespace MemDBSystem.frm
{
    public partial class frmReservationTime : frmBase
    {
        EmployeeHandler eh = new EmployeeHandler();
        ReservationHandler rh = new ReservationHandler();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.Authentication(modelInfo.getModelId(modelInfo.ModelId.reservation));
                DateSelector1.DateValue = DateTime.Now;
                bindStore();
            }

        }
        private void bindStore()
        {
            CommonUtil.BindDropDownListAndSelect(this.ddlStoreNum, eh.LoadStoreInfo(), "storeName".Trim(), "storeNum".Trim());
        }
        private void bindEmployee()
        {
            EmployeeParas paras = new EmployeeParas();
            
            paras.storeNum = ddlStoreNum.SelectedItem.Value;
            paras.modelId = modelInfo.getModelId(modelInfo.ModelId.reservation);
            PageRecord page = null;
            this.dlEmployee.DataSource = eh.LoadEmployeeByParasForPick(paras, ref page);
            this.dlEmployee.DataBind();

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindEmployee();
            btnSave.Visible = true;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            foreach (DataListItem item in dlEmployee.Items)
            {
                DropDownList ddlcheckin, ddlcheckout;
                CheckBox chkIsLeave;
                TextBox txtAnnual, txtSick, txtOther;
                chkIsLeave = (CheckBox)item.FindControl("chkIsLeave");
                txtAnnual = (TextBox)item.FindControl("txtAnnualLeave");
                txtSick = (TextBox)item.FindControl("txtSickLeave");
                txtOther = (TextBox)item.FindControl("txtOtherLeave");
                ddlcheckin = (DropDownList)item.FindControl("ddlCheckIn");
                ddlcheckout = (DropDownList)item.FindControl("ddlCheckOut");
                HiddenField empNum = (HiddenField)item.FindControl("employeeNum");

                tblReservationTime entity = new tblReservationTime();
                entity.workdate = (DateTime)DateSelector1.DateValue;
                entity.checkinTime = ddlcheckin.SelectedItem.Text;
                entity.checkoutTime = ddlcheckout.SelectedItem.Text;
                entity.employeeNum = empNum.Value;
                entity.isLeave = chkIsLeave.Checked;
                entity.AnnualLeave = txtAnnual.Text.Trim();
                entity.SickLeave = txtSick.Text.Trim();
                entity.OtherLeave = txtOther.Text.Trim();
                rh.saveReservationTime(entity);
            }
            this.ShowSavaOK("");
        }

        protected void dlEmployee_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DropDownList ddlcheckin, ddlcheckout;
            ddlcheckin = (DropDownList)e.Item.FindControl("ddlCheckIn");
            ddlcheckout = (DropDownList)e.Item.FindControl("ddlCheckOut");
            //default value
            CommonUtil.SetDropDownSelectedByText(ddlcheckin, "11:00");
            CommonUtil.SetDropDownSelectedByText(ddlcheckout, "19:30");

            CheckBox chkIsLeave;
            TextBox txtAnnual, txtSick, txtOther;
            chkIsLeave = (CheckBox)e.Item.FindControl("chkIsLeave");
            txtAnnual = (TextBox)e.Item.FindControl("txtAnnualLeave");
            txtSick = (TextBox)e.Item.FindControl("txtSickLeave");
            txtOther = (TextBox)e.Item.FindControl("txtOtherLeave");

            //check db saved data
            vw_Employee emp = (vw_Employee)e.Item.DataItem;
            DataTable dt=eh.getReservationTime(emp.employeeNum, (DateTime)DateSelector1.DateValue);
            if (dt != null && dt.Rows.Count == 1)
            {
                CommonUtil.SetDropDownSelectedByText(ddlcheckin, dt.Rows[0]["checkinTime"].ToString());
                CommonUtil.SetDropDownSelectedByText(ddlcheckout, dt.Rows[0]["checkoutTime"].ToString());
                chkIsLeave.Checked = (bool)dt.Rows[0]["isLeave"];
                txtAnnual.Text = dt.Rows[0]["AnnualLeave"].ToString();
                txtSick.Text = dt.Rows[0]["SickLeave"].ToString();
                txtOther.Text = dt.Rows[0]["OtherLeave"].ToString();
            }
        }

    }
}
