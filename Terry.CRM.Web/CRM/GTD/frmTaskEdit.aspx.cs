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

namespace Terry.CRM.Web.CRM.GTD
{
    public partial class frmTaskEdit : BasePage
    {
        private GTDService svr = new GTDService();
        private UserService usrSvr = new UserService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Authentication(enumModule.Schedule);
                hidID.Value = Request["id"];
                if (hidID.Value != "0" && (base.LoginUserRoleGrade == (int)enumRoleGrade.Boss ||
                    base.LoginUserRoleGrade == (int)enumRoleGrade.SalesManager))
                {
                    btnApprove.Visible = true;
                    btnReject.Visible = true;
                }
                else
                {
                    btnApprove.Visible = false;
                    btnReject.Visible = false;
                }
                if (hidID.Value == "0")
                {
                    btnDel.Visible = false;
                }
                else
                {
                    btnDel.Visible = true;
                    btnDel.Attributes.Add("onclick", "onDel('" + btnDel.UniqueID + "');return false;");
                }
                
                hidDay.Value = Request["day"];
                hidPerson.Value = Request["person"];
                BindData();
                ViewState["bNeedRefresh"] = false;
            }
        }
        //按ID绑定或者按CustID和TypeID绑定
        private void BindData()
        {
           //bind entity
            CRMCalendar entity =null;
            if (hidID.Value != "0")
                entity = (CRMCalendar)svr.LoadById(hidID.Value);

            if (entity != null)
            {
                if (entity.Task != null)
                {
                    txtTask.Text = entity.Task;
                }

                if (entity.TaskDate != null)
                {
                    ddlTaskTime.Text = entity.TaskDate.ToString("%H:mm");
                }

                string approvalUserName = "";
                if(entity.ApprovalUser!=null)
                    approvalUserName = usrSvr.LoadById(entity.ApprovalUser.ToString()).UserName;

                switch (entity.Status)
	            {
                    case 0:
                        lblStatus.Text = "";
                        break;
                    case 1:
                        lblStatus.Text = approvalUserName + "审批通过√";
                        break;
                    case 2:
                        lblStatus.Text = "被" + approvalUserName + "否决×";
                        break;
		            default:
                        lblStatus.Text = "";
                        break;
	            }      

            }

        }

        private CRMCalendar GetSaveEntity()
        {
            var entity = new CRMCalendar();
            if (hidID.Value != "0")
            {
                entity = svr.LoadById(hidID.Value);
                entity.ModifyDate = DateTime.Now;
                entity.ModifyUser = base.LoginUserID;
            }
            else
            {
                entity.CDate = DateTime.Now;
                entity.CUser = base.LoginUserID;
                entity.ModifyDate = DateTime.Now;
                entity.ModifyUser = base.LoginUserID;
            }
            entity.UserName = hidPerson.Value;
            entity.Status = 0;// 0 =draft 1= approved 2=reject,9=finish
            if(ddlTaskTime.SelectedValue!="")
                entity.TaskDate = DateTime.ParseExact(hidDay.Value+ " "+ ddlTaskTime.SelectedValue,"yyyyMMdd H:mm",null);
            else
                entity.TaskDate = DateTime.ParseExact(hidDay.Value , "yyyyMMdd", null);
            if (string.IsNullOrEmpty(txtTask.Text.Trim()) == false)
                entity.Task = txtTask.Text.Trim();
            return entity;
        }

        private void CleanFrm()
        {
            hidID.Value = "0";
            ddlTaskTime.SelectedIndex = 0;
            txtTask.Text = "";
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = GetSaveEntity();
                entity.Status = 1;
                entity.ApprovalUser = base.LoginUserID;
                entity = svr.Save(entity);
                lblMsg.Text = GetREMes("MsgSaveOK");
                ViewState["bNeedRefresh"] = true;
                btnBack_Click(null, null);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
        }
        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = GetSaveEntity();
                entity.Status = 2;
                entity.ApprovalUser = base.LoginUserID;
                entity = svr.Save(entity);
                lblMsg.Text = GetREMes("MsgSaveOK");
                ViewState["bNeedRefresh"] = true;
                btnBack_Click(null, null);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            CleanFrm();
        }
        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = GetSaveEntity();
                entity = svr.Save(entity);
                lblMsg.Text = GetREMes("MsgSaveOK");
                //hidID.Value = entity.ID.ToString();
                CleanFrm();
                ViewState["bNeedRefresh"] = true;
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }
        //Click Delete Button
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                svr.DeleteById(hidID.Value);
                ViewState["bNeedRefresh"] = true;
                btnBack_Click(null, null);
                //this.ShowDeleteOK();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if ((bool)ViewState["bNeedRefresh"]==true)
                ClientScript.RegisterClientScriptBlock(typeof(string), "cls",
                @"<script type='text/javascript'>
                window.parent.document.getElementById('ctl00_CPH1_btnRefresh').click();
                window.parent.hidePopWin(false);</script>");
            else
                ClientScript.RegisterClientScriptBlock(typeof(string), "cls",
                @"<script type='text/javascript'>
                window.parent.hidePopWin(false);</script>");

        }
    }
}
