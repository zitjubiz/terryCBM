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
    public partial class frmAddSales : BasePage
    {
        private UserService svr = new UserService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["id"]))
                hidID.Value = "0";

            if (!Page.IsPostBack)
            {
                BindData();
            }

            if (hidID.Value != "0")
                txtPassword.Attributes.Remove("usage");

        }


        private void BindData()
        {
            //TODO: bind Dropdownlist,change accordingly
            txtBossID.BindDropDownListAndSelect(svr.GetUser(), "UserName", "UserID");
            txtSYSID.BindDropDownList(svr.GetSystem(), "SYSName", "SYSID");
            //txtDept.BindDropDownListAndSelect(svr.GetDepartment(), "DepName", "DepID");

            //bind entity
            var entity = (vw_CRMUser)svr.LoadById(typeof(vw_CRMUser), "UserID", hidID.Value);
            if (entity != null)
            {
                txtUserID.Text = entity.UserID.ToString();

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
            if (string.IsNullOrEmpty(txtUserID.Text.Trim()) == false)
                entity.UserID = int.Parse(txtUserID.Text.Trim());
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()) == false)
                entity.UserName = txtUserName.Text.Trim();
            if (string.IsNullOrEmpty(txtUserFullName.Text.Trim()) == false)
                entity.UserFullName = txtUserFullName.Text.Trim();
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()) == false)
                entity.Password = txtPassword.Text.Trim();
            if (string.IsNullOrEmpty(txtMobile.Text.Trim()) == false)
                entity.Mobile = txtMobile.Text.Trim();

            entity.IsActive = true;

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
            txtUserID.Text = "";
            txtUserName.Text = "";
            txtUserFullName.Text = "";
            txtPassword.Text = "";
            txtMobile.Text = "";
            txtCDate.Text = "";
            txtCUser.Text = "";
            txtModifyDate.Text = "";
            txtModifyUser.Text = "";
            txtBossID.Text = "";
            txtSYSID.Text = "";
            txtEmail.Text = "";
        }

        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
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
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }



    }
}

