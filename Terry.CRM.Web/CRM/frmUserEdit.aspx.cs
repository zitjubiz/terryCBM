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
using System.Collections.Generic;

namespace Terry.CRM.Web.CRM
{
    public partial class frmUserEdit : BasePage
    {
        //private int LicenseCnt = 1; //零售价600一个用户,量多有折扣
        private UserService svr = new UserService();
        private SystemService sys = new SystemService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["id"])) return;
            if (!Page.IsPostBack)
            {
                btnDel.Attributes.Add("onclick", "onDel('" + btnDel.UniqueID + "');return false;");
                hidID.Value = Request["id"];
                BindData();
                BindRole();
                EnumLicenseCnt();
                if (svr.GetActiveUserCount() >= (int)ViewState["LicenseCnt"])
                    btnSave.Enabled = false;
            }
            if (hidID.Value != "0")
                txtPassword.Attributes.Remove("usage");

        }

        private void EnumLicenseCnt()
        {
            var SysInfo = sys.LoadById("1");

            for (int i = 0; i <= 200; i = i + 5)
            {
                if (SysInfo.SYSLicenseCnt == svr.Encypt(SysInfo.SYSName, i.ToString()))
                {
                    ViewState["LicenseCnt"] = i;
                    break;
                }
            }
        }
        private void BindRole()
        {
            //bind roles below current login user's grade
            txtRole.DataSource = svr.GetRoleBelowGrade(base.LoginUserRoleGrade);
            txtRole.DataTextField = "Role";
            txtRole.DataValueField = "RoleID";
            txtRole.DataBind();
            //set which role the user belong to
            var Roles = svr.GetRole(long.Parse(hidID.Value));
            foreach (var r in Roles)
            {
                foreach (ListItem item in txtRole.Items)
                {
                    if (item.Value == r.RoleID.ToString())
                        item.Selected = true;
                }
            }
        }

        private void BindData()
        {
            //TODO: bind Dropdownlist,change accordingly
            txtBossID.BindDropDownListAndSelect(svr.GetUser(), "UserName", "UserID");
            txtSYSID.BindDropDownList(svr.GetSystem(), "SYSName", "SYSID");
            txtDept.BindDropDownListAndSelect(svr.GetDepartment(), "DepName", "DepID");

            //bind entity
            var entity = (vw_CRMUser)svr.LoadById(typeof(vw_CRMUser), "UserID", hidID.Value);
            if (entity != null)
            {
                hidID.Value = entity.UserID.ToString();

                if (entity.UserName != null)
                {
                    txtUserName.Text = entity.UserName.ToString();
                }
                if (entity.UserFullName != null)
                {
                    txtUserFullName.Text = entity.UserFullName.ToString();
                }
                if (entity.Password != null)
                {
                    txtPassword.Text = entity.Password.ToString();
                }
                if (entity.Mobile != null)
                {
                    txtMobile.Text = entity.Mobile.ToString();
                }
                if (entity.CDate != null)
                {
                    txtCDate.Text = entity.CDate.ToString();
                }
                if (entity.ModifyDate != null)
                {
                    txtModifyDate.Text = entity.ModifyDate.ToString();
                }
                if (entity.BossID != null)
                {
                    txtBossID.Text = entity.BossID.ToString();
                }

                if (entity.DepID != null)
                {
                    txtDept.SelectedByValue(entity.DepID.ToString());
                }
                if (entity.Email != null)
                {
                    txtEmail.Text = entity.Email;
                }
                txtCUser.Text = entity.CUserName;
                txtCUserID.Value = entity.CUser.ToString();

                txtModifyUser.Text = entity.ModifyUserName;
                txtModifyUserID.Value = entity.ModifyUser.ToString();

                txtIsActive.Text = entity.IsActive.ToString();
                txtSYSID.Text = entity.SYSID.ToString();
            }
            else
            {
                txtCDate.Text = DateTime.Now.ToString(GetREMes("DateTimeFormatStringCS"));
                txtCUserID.Value = base.LoginUserID.ToString();
                txtCUser.Text = base.LoginUserName;
                txtModifyDate.Text = DateTime.Now.ToString(GetREMes("DateTimeFormatStringCS"));
                txtModifyUserID.Value = base.LoginUserID.ToString();
                txtModifyUser.Text = base.LoginUserName;
            }

        }

        private CRMUser GetSaveEntity()
        {
            var entity = new CRMUser();
            if (string.IsNullOrEmpty(hidID.Value.Trim()) == false)
                entity.UserID = int.Parse(hidID.Value.Trim());
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()) == false)
                entity.UserName = txtUserName.Text.Trim();
            if (string.IsNullOrEmpty(txtUserFullName.Text.Trim()) == false)
                entity.UserFullName = txtUserFullName.Text.Trim();
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()) == false)
                entity.Password = txtPassword.Text.Trim();
            if (string.IsNullOrEmpty(txtMobile.Text.Trim()) == false)
                entity.Mobile = txtMobile.Text.Trim();
            if (string.IsNullOrEmpty(txtIsActive.Text.Trim()) == false)
                entity.IsActive = Boolean.Parse(txtIsActive.Text.Trim());
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()) == false)
                entity.Email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(txtBossID.Text.Trim()) == false)
                entity.BossID = int.Parse(txtBossID.Text.Trim());
            if (string.IsNullOrEmpty(txtSYSID.Text.Trim()) == false)
                entity.SYSID = int.Parse(txtSYSID.Text.Trim());

            if (string.IsNullOrEmpty(txtCDate.Text.Trim()) == false)
                entity.CDate = DateTime.Parse(txtCDate.Text.Trim());
            if (string.IsNullOrEmpty(txtCUser.Text.Trim()) == false)
                entity.CUser = int.Parse(txtCUserID.Value.Trim());

            if (string.IsNullOrEmpty(txtDept.Text.Trim()) == false)
                entity.DepID = int.Parse(txtDept.SelectedValue.Trim());

            entity.ModifyDate = DateTime.Now;
            entity.ModifyUser = base.LoginUserID;
            return entity;
        }

        private void CleanFrm()
        {
            hidID.Value = "";
            txtUserName.Text = "";
            txtUserFullName.Text = "";
            txtPassword.Text = "";
            txtMobile.Text = "";
            txtCDate.Text = "";
            txtCUser.Text = "";
            txtModifyDate.Text = "";
            txtModifyUser.Text = "";
            txtIsActive.Text = "";
            txtBossID.Text = "";
            txtSYSID.Text = "";
            txtEmail.Text = "";
        }

        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (svr.GetActiveUserCount() >= (int)ViewState["LicenseCnt"])
            {
                this.ShowMessage("你最多能创建" + ViewState["LicenseCnt"].ToString() + "个用户");
                return;
            }

            try
            {
                var entity = GetSaveEntity();
                //----get role--------
                List<CRMRole> RList = new List<CRMRole>();
                var r = new CRMRole();
                r.RoleID = long.Parse(txtRole.SelectedValue);
                RList.Add(r);

                entity = svr.Save(entity, RList);
                hidID.Value = entity.UserID.ToString();
                this.ShowSaveOK();
                Response.Redirect("frmUser.aspx");
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
                svr.SoftDeleteById(hidID.Value);
                this.ShowDeleteOK();
                Response.Redirect("frmUser.aspx");
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmUser.aspx");
        }
    }
}

