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
using DayPilot.Web.Ui;
using Terry.CRM.Service;

namespace Terry.CRM.Web.CRM
{
    public partial class frmReservationMain : BasePage
    {
        ReservationService rh = new ReservationService();
        long BookID;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!Page.IsPostBack)
            {
                bindStore();
                //非管理員只能看当天的预约,不能查找
                //if (this.LogOnUserRight != rightGroup.ADMIN)
                //{ 
                //     DateSelector1.Enabled = false;
                //     ddlSearch.Enabled = false; 
               
                //}
                //CommonUtil.SetTextBoxReadOnlyByAttr(true, txtAnnualLeave, txtOtherLeave, txtSickLeave);
            }

        }
        private void bindStore()
        {
            //CommonUtil.BindDropDownList(this.ddlStoreNum, eh.LoadStoreInfo(), "storeName".Trim(), "storeNum".Trim());
        }

        /// <summary>
        /// 顯示所有請假的內容
        /// </summary>
        private void bindLeave()
        { 
            //load Leave remark
            //txtAnnualLeave.Text = "";
            //txtSickLeave.Text = "";
            //txtOtherLeave.Text = "";
            //DataTable dt=rh.getReservationLeave(((DateTime)DateSelector1.DateValue).ToShortDateString());
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if(dt.Rows[i]["AnnualLeave"].ToString().Trim()!="")
            //        txtAnnualLeave.Text += dt.Rows[i]["employeeNum"].ToString() + ":" + dt.Rows[i]["AnnualLeave"].ToString() + " ";
            //    if (dt.Rows[i]["SickLeave"].ToString().Trim() != "")
            //        txtSickLeave.Text += dt.Rows[i]["employeeNum"].ToString() + ":" + dt.Rows[i]["SickLeave"].ToString() + " ";
            //    if (dt.Rows[i]["OtherLeave"].ToString().Trim() != "")
            //        txtOtherLeave.Text += dt.Rows[i]["employeeNum"].ToString() + ":" + dt.Rows[i]["OtherLeave"].ToString() + " ";
			 
            //}
        
        }
        private void bindEmployee()
        {
           
            this.dlEmployee.DataSource = rh.GetEmployeesByOfficeAndDay("", "2012-08-23");
            this.dlEmployee.DataBind();
            if (dlEmployee.Items.Count > 0)
            {
                lblMsg.Visible = false;
                bindLeave();
                trSearch.Visible = false;
                trEmp.Visible = true;
                trLeave.Visible = true;
            }
            else
            { 
                lblMsg.Visible = true;
                trSearch.Visible = false;
                trEmp.Visible = true;
                trLeave.Visible = false;

            }

        }
        private void bindReservation(int BookID)
        {
            
        }
        /// <summary>
        /// 如果按预约编号,电话来查找,出来另外一个界面.
        /// 按日期,分店,则出来所有可以预约的员工列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            if (ddlSearch.SelectedValue == "ID" && txtSearch.Text.Trim() != "")
            {
                long.TryParse(txtSearch.Text.Trim(), out BookID);
                dlSearch.DataSource = rh.loadReservationByID(BookID);
                dlSearch.DataBind();
                trSearch.Visible = true;
                trEmp.Visible = false;
                trLeave.Visible=false;
            }
            else if (ddlSearch.SelectedValue == "Mobile" && txtSearch.Text.Trim() != "")
            {
                dlSearch.DataSource = rh.loadReservationByMobile(txtSearch.Text.Trim());
                dlSearch.DataBind();
                if(dlSearch.Items.Count==0)
                    this.ShowMessage("最近都沒有預約記錄");

                trSearch.Visible = true;
                trEmp.Visible = false;
                trLeave.Visible = false;
            }
            else
            { 
                bindEmployee();            
            
            }
        }


        protected void DayPilotCalendar1_EventClick(object sender, DayPilot.Web.Ui.EventClickEventArgs e)
        {
            this.ShowMessage("EventClick:" + e.Value.ToString());
        }

        protected void DayPilotCalendar1_FreeTimeClick(object sender, DayPilot.Web.Ui.FreeClickEventArgs e)
        {
            this.ShowMessage("FreeTimeClick:" + e.Start.ToString());
        }


        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            bindEmployee();
        }

        protected void dlEmployee_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            //tblEmployee emp = (tblEmployee)e.Item.DataItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            DayPilotCalendar dpc = (DayPilotCalendar)e.Item.FindControl("DayPilotCalendar1");
            if (ddlSearch.SelectedValue == "" || txtSearch.Text.Trim() == "")
            { 
                //if (DateSelector1.DateValue != null)
                //    dpc.StartDate = (DateTime)DateSelector1.DateValue;
            }
            else
            {
                if (ddlSearch.SelectedValue == "ID" && txtSearch.Text.Trim() != "")
                {
                    long.TryParse(txtSearch.Text.Trim(), out BookID);
                    dpc.StartDate = rh.getReservationDateByID(BookID);
                }
                else if (ddlSearch.SelectedValue == "Mobile" && txtSearch.Text.Trim() != "")
                {
                    dpc.StartDate = rh.getReservationDateByMobile(txtSearch.Text.Trim());
                }
            }
            //get Business Exclude Hours
            dpc.BusinessExcludeHours = rh.getBusinessExcludeHours(drv["employeeNum"].ToString(), dpc.StartDate);
            //bind data
            dpc.DataSource = rh.getReservation(drv["employeeNum"].ToString(), dpc.StartDate, dpc.Days);
            dpc.DataBind();            

        }

        protected void DayPilotCalendar1_DataBound(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(sender.ToString());
        }

        protected void dlSearch_ItemDataBound(object sender, DataListItemEventArgs e)
        {

        }

        protected void lnkLog_Click(object sender, EventArgs e)
        {
            //Page.ClientScript.RegisterClientScriptBlock(typeof(string),"log","<script>window.open('frmReservationLog.aspx?date=" + DateSelector1.DateValue.ToString() + "','_blank');</script>");
        }
    }
}
