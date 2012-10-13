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
    public partial class frmReservationEdit : frmBase
    {
        EmployeeHandler emph = new EmployeeHandler();
        ReservationHandler resvh = new ReservationHandler();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.Authentication(modelInfo.getModelId(modelInfo.ModelId.reservation));
                //新增预约 鎖定员工同时段
                if (Request["id"] == "0")
                {
                    //一次预约要1.5小时,锁定3个格子...
                    DateTime dtStart = DateTime.Parse(Request["eventstart"]);
                    AddLock(Request["emp"], dtStart.ToString(), 60);
                    AddLock(Request["emp"], dtStart.AddMinutes(30).ToString(), 60);
                    AddLock(Request["emp"], dtStart.AddMinutes(60).ToString(), 60);
                
                }

                HidReservationID.Value = Request["id"];
                lblLogonUser.Text = base.LogOnUserName;
                //bindEmployee(); modify by zhouxq on 2009-06-23 
                bindEmpByStore("");

                //service 要手工编辑
                //CommonUtil.SetTextBoxReadOnlyByAttr(true, txtServiceName);
                eventStart.MinuteInterval = 15;
                eventEnd.MinuteInterval = 15;
                bindata();
                
            }
        }


        //新增预约在同一员工,同一时段的冲突的提示

        //mark someone occupy this time range with sb 60 secs
        private void AddLock(string User, string StartTime,long LockSeconds)
        {
            string key = User + StartTime;

            if (Application[key] != null)
            {
                //string secs = Application[key].ToString().Substring(Application[key].ToString().IndexOf("_"));
                string usr = Application[key].ToString().Substring(0,Application[key].ToString().IndexOf("_"));
                if (usr != base.LogOnUserName)
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(string), "locked", "<script>alert('員工該時段正被" + usr + "鎖定,請稍候再試.');window.close();</script>");
                    return;
                }
            }
            else
            {
                Application.Add(User + StartTime, base.LogOnUserName+"_"+ DateTime.Now.AddSeconds(LockSeconds));
            }
        }

        private void bindEmpByStore(string strStoreNum)
        {
            DataTable dt =resvh.LoadAllEmployeeForReservation();
            CommonUtil.BindDropDownListAndSelect(ddlStoreNum ,emph.LoadStoreInfo(),"storeName","storeNum");
            CommonUtil.BindDropDownList(ddlEmployee,dt , "employeeNum", "employeeNum");

           if (strStoreNum != "")
           {
               CommonUtil.SetDropDownSelectedByValue(ddlStoreNum, strStoreNum);
               CommonUtil.BindDropDownList(ddlEmployee, resvh.LoadEmployeeDataByStoreNum(strStoreNum), "employeeNum", "employeeNum");
           }

           if (Request["emp"] != null)
           {
               CommonUtil.SetDropDownSelectedByValue(ddlEmployee, Request["emp"]);
               vw_Employee emp = emph.Loadvw_EmployeeByNum(Request["emp"]);
               CommonUtil.SetDropDownSelectedByValue(ddlStoreNum, emp.storeNum);
           }
           
       }

        private void bindEmployee()
        {
            //得去掉不能预约的员工,和休假的员工
            ddlEmployee.DataSource = emph.LoadAllEmployeeNum();
            ddlEmployee.DataBind();
            if (Request["emp"] != null)
            { 
                CommonUtil.SetDropDownSelectedByValue(ddlEmployee, Request["emp"]);
            }
            //编辑预约的员工下拉框,可以修改( 这样预约编号不会改变) 
            //原来预约的员工休息时,可以转給其他员工接手
            //ddlEmployee.Enabled = false;

        }
        private void bindata()
        {
            vw_Reservation v= resvh.loadReservation(long.Parse(Request["id"]));
            if (v != null)
            {
                string strStoreNum = v.storeNum;
                bindEmpByStore(strStoreNum);
                ddlEmployee.SelectedIndex = ddlEmployee.Items.IndexOf(ddlEmployee.Items.FindByValue(v.employeeNum));
                if (string.IsNullOrEmpty(v.memberNum))
                {
                    //non-member
                    txtSharePersonEngName.Text = v.CustomerName;
                    txtMobile.Text = v.CustomerTel;

                }
                else
                {
                    if(string.IsNullOrEmpty(v.chnName))
                        txtSharePersonEngName.Text = v.CustomerName;
                    else
                        txtSharePersonEngName.Text = v.chnName;
                    if (string.IsNullOrEmpty(v.mobilePhone))
                        txtMobile.Text = v.CustomerTel;
                    else
                        txtMobile.Text = v.mobilePhone;
                }

                //txtSharePersonChnName.Text = v.chnName;
                txtServiceName.Text = v.SeviceDesc;
                
                txtServiceNum.Text = v.serviceNum;
                HidMemberNum.Value = v.memberNum;
                //display member num and package
                txtMemberNum.Text = v.memberNum;
                txtPackage.Text = v.PackageDesc;
                if (v.IsInput != null)
                {
                    if (v.IsInput == true)
                    { 
                        hidIsInput.Value = "1";
                        //入單后预约就不能保存删除
                        btnSave.Enabled = false;
                        btnDelete.Enabled = false;
                        btnSentM.Enabled = false;
                    
                    }
                }
                //isConfirm = null/OK/VM
                if (v.IsConfirm == "OK")
                    chkConfirmOK.Checked = true;
                else if(v.IsConfirm=="VM")
                    chkConfirmVM.Checked = true;
                //SendSMS
                if (v.SendSMS == true)
                    lblSendSMS.Visible = true;
                txtRemark.Text = v.Remark;
                //是否指定其他顔色
                if (v.OtherColor != null)
                {
                    chkColor.Checked = true;
                    ddlColor.SelectedIndex = ddlColor.Items.IndexOf(ddlColor.Items.FindByValue(v.OtherColor));
                }

                //store date to combine with timepicker with a new date value
                dtEventDate.DateValue = DateTime.Parse(v.eventStart.ToShortDateString());
                eventStart.TimeValue = v.eventStart;
                eventEnd.TimeValue = v.eventEnd;
                chkCanChange.Checked = v.CanChange;
                chkIsArrived.Checked = v.IsArrived;
                chkCancel.Checked = v.IsCancel;
                //还没有入單,客人已到达时,才可以入單
                if (hidIsInput.Value != "1" && v.IsArrived==true)
                    this.btnInputBill.Enabled = true;
                else
                    this.btnInputBill.Enabled = false;
                btnSentM.Enabled = true;

            }
            else
            {
                dtEventDate.DateValue = DateTime.Parse(Request["eventstart"]);
                eventStart.TimeValue = DateTime.Parse(Request["eventstart"]);
                eventEnd.TimeValue = DateTime.Parse(Request["eventstart"]).AddMinutes(90);
                chkIsArrived.Enabled =false;
                chkCancel.Enabled = false;
                this.btnInputBill.Enabled = false;
                btnSentM.Enabled = false;

            }

            
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            tblReservation r = new tblReservation();
            bool isAddnew =false;
            //弹出窗口发送了sms之后,保存时,sendsms=true
            if (HidSms.Value == "1")
                r.SendSMS = true;

            r.employeeNum = FixedJsString(ddlEmployee.SelectedItem.Value);
            //r.eventStart = DateTime.Parse(HidEventStart.Value);
            //r.eventEnd = DateTime.Parse(HidEventEnd.Value);
            r.eventStart = DateTime.Parse(System.Convert .ToDateTime(dtEventDate.DateValue).ToString("yyyy-MM-dd") + " " + eventStart.TimeValue.Value.ToShortTimeString());
            r.eventEnd = DateTime.Parse(System.Convert.ToDateTime(dtEventDate.DateValue).ToString("yyyy-MM-dd") + " " + eventEnd.TimeValue.Value.ToShortTimeString());
            
            //daypilot control won't display if eventend < eventstart, so please check this...
            if (r.eventEnd < r.eventStart)
            {   //swap value
                DateTime tmp = r.eventStart;
                r.eventStart = r.eventEnd;
                r.eventEnd = tmp;
            }

            //r.memberNum = FixedJsString(HidMemberNum.Value);
            r.memberNum = FixedJsString(txtMemberNum.Text);

            if (txtPackage.Text.Trim() != "")
                r.PackageDesc = FixedJsString(txtPackage.Text);
            else
                r.PackageDesc = null;

            r.serviceNum = FixedJsString(txtServiceNum.Text);
            r.SeviceDesc= FixedJsString(txtServiceName.Text);
            if (Request["id"] == "0")
            {
                r.reservationID = 0;
                isAddnew = true;
            }
            else
            { 
                r.reservationID = long.Parse(Request["id"]);
                //if can't input bill(btnInputBill.enabled=false),r.IsInput=true
                //r.IsInput = ! btnInputBill.Enabled;
                //客人已到达时,才可以入单
                if (hidIsInput.Value == "1")
                    r.IsInput = true;
                else
                    r.IsInput = false;
            
            }
            r.logOnUser = this.LogOnUserID.Trim();
            //store these info for non-member reservation
            r.CustomerName = txtSharePersonEngName.Text;
            r.CustomerTel = txtMobile.Text;
            r.CanChange = chkCanChange.Checked;
            r.IsArrived = chkIsArrived.Checked;
            r.IsCancel = chkCancel.Checked;
            r.Remark = txtRemark.Text.Trim();
            //增加其他格子顔色選項
            if (chkColor.Checked)
                r.OtherColor = ddlColor.SelectedValue;
            else
                r.OtherColor = null;

            if (chkConfirmOK.Checked)
                r.IsConfirm = "OK";
            else if (chkConfirmVM.Checked)
                r.IsConfirm = "VM";
            else
                r.IsConfirm = null;

            //---------check conflict------------
            string ErrMsg = "";
            if (false == resvh.checkConflict(r.reservationID, r.employeeNum, r.eventStart, r.eventEnd, ref ErrMsg))
            { 
                this.ShowMessage(ErrMsg);
                return;
            } 

            r=resvh.saveReservation(isAddnew, r);

            //---write log ----
            //change log to database
           // string strEmpoyeeName = this.getEmployeeNameByNum(r.employeeNum);
            string msg =  "職員:" + LogOnUserName+"保存了"+ r.reservationID.ToString() + "號預約 客戶:" + r.CustomerName + " 電話:" + r.CustomerTel;
            msg += " 預約師傅:" + ddlEmployee.SelectedItem.Text+ (r.CanChange? " (指定)":"");  
            msg += " 時間:" + r.eventStart + " 至" + eventEnd.TimeValue.Value.ToShortTimeString();

            if(txtRemark.Text.Trim()!="")
                msg += " 備註:" + txtRemark.Text.Trim();

            msg += " 服務:" + txtServiceName.Text + (r.IsArrived?" 已到達":"")+(r.IsCancel?" 已取消":"");
            msg += " 修改時間:" + DateTime.Now.ToString();

            LogInfo log = new LogInfo();
            log.op = LogOnUserName;
            log.opDate = DateTime.Now;
            log.eventDate = r.eventStart;
            log.client = r.CustomerName;
            log.tel = r.CustomerTel;
            log.employee = ddlEmployee.SelectedItem.Text;
            log.logInfo = msg;
            resvh.WriteLog(log);
            //CommonUtil.WriteLog(msg);

            Page.ClientScript.RegisterStartupScript(typeof(string), "cls", "<script>window.close();window.opener.document.getElementById('btnRefresh').click();</script>");

            //this.CloseWindow();
            
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            resvh.deleteReservation(int.Parse(Request["id"]));
            //---write log ----
            string msg = "職員:" + LogOnUserName + "刪除了"+ Request["id"]  + "號預約 客戶:" + txtSharePersonEngName.Text + " 電話:" + txtMobile.Text ;
            msg += " 預約師傅:" + ddlEmployee.SelectedItem.Text;
            msg += " 時間:" + dtEventDate.DateValue.ToString() + " " + eventStart.TimeValue.Value.ToShortTimeString() + " 至" + eventEnd.TimeValue.Value.ToShortTimeString();
            msg += " 服務:" + txtServiceName.Text ;
            msg += " 修改時間:" + DateTime.Now.ToString();
            LogInfo log = new LogInfo();
            log.op = LogOnUserName;
            log.opDate = DateTime.Now;
            log.eventDate = (DateTime)dtEventDate.DateValue;
            log.client = txtSharePersonEngName.Text.Replace("'", "''");
            log.tel = txtMobile.Text.Replace("'","''");
            log.employee = ddlEmployee.SelectedItem.Text;
            log.logInfo = msg;
            resvh.WriteLog(log);

            Page.ClientScript.RegisterStartupScript(typeof(string), "cls", "<script>window.close();window.opener.document.getElementById('btnRefresh').click();</script>");

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //this.Btn_Close("frmReservationMain.aspx");
        }

        [System.Web.Services.WebMethod]
        static public string[] GetEmployeeByStoreNum(string strStoreNum)
        {
            string[] numList = null;
            if (string.IsNullOrEmpty(strStoreNum)) return numList;
            DataTable table = new ReservationHandler().LoadEmployeeDataByStoreNum(strStoreNum);
            if (table == null) return numList;
            numList = new string[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
                numList[i] = table.Rows[i]["employeeNum"].ToString().Trim() + table.Rows[i]["employeeName"].ToString().Trim();
            return numList;
        }

        protected void ddlStoreNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strStoreNum = ddlStoreNum.SelectedValue;
            CommonUtil.BindDropDownList(ddlEmployee, resvh.LoadEmployeeDataByStoreNum(strStoreNum), "employeeNum", "employeeNum");
        }

        protected void btnInputBill_Click(object sender, EventArgs e)
        {
            //frmCousumeMaintenance.aspx?action=input&memberNum=&frmName=frmCousumeMain
            string url = "frmCousumeMaintenance.aspx?action={0}&reservationID={1}&frmName={2}&memberNum={3}";
            url = string.Format(url, "input", HidReservationID.Value, "frmReservationEdit",txtMemberNum.Text);
            Response.Redirect(url);
        }

        protected void chkCancel_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            bindata();
        }

    }
}
