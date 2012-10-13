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
    public partial class frmReservationTimeTable : frmBase
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
            CommonUtil.BindDropDownList(this.ddlStoreNum, eh.LoadStoreInfo(), "storeName".Trim(), "storeNum".Trim());
        }
        private void bindEmployee()
        {
            EmployeeParas paras = new EmployeeParas();

            //paras.storeNum = ddlStoreNum.SelectedItem.Value;
            paras.modelId = modelInfo.getModelId(modelInfo.ModelId.reservation);
            PageRecord page = null;
            this.rptEmp.DataSource = eh.LoadEmployeeByParasForPick(paras, ref page);
            this.rptEmp.DataBind();

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindEmployee();
            btnSave.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem Item in rptEmp.Items)
            {
                DropDownList ddlcheckin, ddlcheckout;
                CheckBox chkIsLeave,chkSel;
                TextBox txtAnnual, txtSick, txtOther;
                chkIsLeave = (CheckBox)Item.FindControl("chkIsLeave");
                chkSel = (CheckBox)Item.FindControl("chkSel");
                txtAnnual = (TextBox)Item.FindControl("txtAnnualLeave");
                txtSick = (TextBox)Item.FindControl("txtSickLeave");
                txtOther = (TextBox)Item.FindControl("txtOtherLeave");
                ddlcheckin = (DropDownList)Item.FindControl("ddlCheckIn");
                ddlcheckout = (DropDownList)Item.FindControl("ddlCheckOut");
                HiddenField empNum = (HiddenField)Item.FindControl("employeeNum");
                //---add work store field
                //-- 如果選擇了,就是該分店的當天上班的員工. 員工每天上班的分店是不固定的
                if (chkSel.Checked)
                { 
                    tblReservationTime entity = new tblReservationTime();
                    entity.workdate = (DateTime)DateSelector1.DateValue;
                    entity.checkinTime = ddlcheckin.SelectedItem.Text;
                    entity.checkoutTime = ddlcheckout.SelectedItem.Text;
                    entity.employeeNum = empNum.Value;
                    entity.isLeave = chkIsLeave.Checked;
                    entity.AnnualLeave = txtAnnual.Text.Trim();
                    entity.SickLeave = txtSick.Text.Trim();
                    entity.OtherLeave = txtOther.Text.Trim();
                   
                    entity.workStore = ddlStoreNum.SelectedValue;
                    rh.saveReservationTime(entity);                
                }

            }
            this.ShowSavaOK("");

        }

        protected void rptEmp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DropDownList ddlcheckin, ddlcheckout;
            CheckBox chkIsLeave, chkSel;
            TextBox txtAnnual, txtSick, txtOther;
            chkIsLeave = (CheckBox)e.Item.FindControl("chkIsLeave");
            chkSel = (CheckBox)e.Item.FindControl("chkSel");
            txtAnnual = (TextBox)e.Item.FindControl("txtAnnualLeave");
            txtSick = (TextBox)e.Item.FindControl("txtSickLeave");
            txtOther = (TextBox)e.Item.FindControl("txtOtherLeave");
            ddlcheckin = (DropDownList)e.Item.FindControl("ddlCheckIn");
            ddlcheckout = (DropDownList)e.Item.FindControl("ddlCheckOut");
            //default value
            CommonUtil.SetDropDownSelectedByText(ddlcheckin, "11:00");
            CommonUtil.SetDropDownSelectedByText(ddlcheckout, "19:30");
            //check db saved data
            vw_Employee emp = (vw_Employee)e.Item.DataItem;


            DataTable dt = eh.getReservationTime(emp.employeeNum, (DateTime)DateSelector1.DateValue);
            if (dt != null && dt.Rows.Count == 1)
            {
                CommonUtil.SetDropDownSelectedByText(ddlcheckin, dt.Rows[0]["checkinTime"].ToString());
                CommonUtil.SetDropDownSelectedByText(ddlcheckout, dt.Rows[0]["checkoutTime"].ToString());
                chkIsLeave.Checked = (bool)dt.Rows[0]["isLeave"];
                txtAnnual.Text = dt.Rows[0]["AnnualLeave"].ToString();
                txtSick.Text = dt.Rows[0]["SickLeave"].ToString();
                txtOther.Text = dt.Rows[0]["OtherLeave"].ToString();
                //選中員工
                if (dt.Rows[0]["workStore"].ToString() == ddlStoreNum.SelectedValue)
                    chkSel.Checked = true;
            }
            else
            { 
                //在未定義當天的時間錶時,原来店面定义了下属员工,默认被选中. 
                if (emp.storeNum == ddlStoreNum.SelectedValue)
                    chkSel.Checked = true;
            
            }


        }
    }
}
