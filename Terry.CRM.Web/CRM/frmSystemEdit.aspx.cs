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

namespace Terry.CRM.Web.CRM
{
    public partial class frmSystemEdit : BasePage
    {
        private SystemService svr = new SystemService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                btnDel.Attributes.Add("onclick", "onDel('" + btnDel.UniqueID + "');return false;");
                hidID.Value = Request["id"];
                BindData();
            }
        }

        private void BindData()
        {
            //TODO: bind Dropdownlist,change accordingly

            //bind entity
            var entity = (CRMSystem)svr.LoadById(typeof(CRMSystem), "SYSID", hidID.Value);
            if (entity != null)
            {
                txtSYSID.Text = entity.SYSID.ToString();
                if (entity.SYSName != null)
                {
                    txtSYSName.Text = entity.SYSName.ToString();
                }
                if (entity.SYSWeb != null)
                {
                    txtSYSWeb.Text = entity.SYSWeb.ToString();
                }
                if (entity.SYSContact != null)
                {
                    txtSYSContact.Text = entity.SYSContact.ToString();
                }
                if (entity.SYSContactTel != null)
                {
                    txtSYSContactTel.Text = entity.SYSContactTel.ToString();
                }
                if (entity.SYSCDate != null)
                {
                    txtSYSCDate.Text = ((DateTime)entity.SYSCDate).ToString(DateFormatString);
                }
                if (entity.SYSBeginDate != null)
                {
                    txtSYSBeginDate.Text = ((DateTime)entity.SYSBeginDate).ToString(DateFormatString);
                }
                if (entity.SYSExpiryDate != null)
                {
                    txtSYSExpiryDate.Text = ((DateTime)entity.SYSExpiryDate).ToString(DateFormatString);
                }
            }

        }
        private CRMSystem GetSaveEntity()
        {
            var entity = new CRMSystem();
            if (string.IsNullOrEmpty(txtSYSID.Text.Trim()) == false)
                entity.SYSID = int.Parse(txtSYSID.Text.Trim());
            if (string.IsNullOrEmpty(txtSYSName.Text.Trim()) == false)
                entity.SYSName = txtSYSName.Text.Trim();
            if (string.IsNullOrEmpty(txtSYSWeb.Text.Trim()) == false)
                entity.SYSWeb = txtSYSWeb.Text.Trim();
            if (string.IsNullOrEmpty(txtSYSContact.Text.Trim()) == false)
                entity.SYSContact = txtSYSContact.Text.Trim();
            if (string.IsNullOrEmpty(txtSYSContactTel.Text.Trim()) == false)
                entity.SYSContactTel = txtSYSContactTel.Text.Trim();
            if (string.IsNullOrEmpty(txtSYSCDate.Text.Trim()) == false)
                entity.SYSCDate = DateTime.Parse(txtSYSCDate.Text.Trim());
            if (string.IsNullOrEmpty(txtSYSBeginDate.Text.Trim()) == false)
                entity.SYSBeginDate = DateTime.Parse(txtSYSBeginDate.Text.Trim());
            if (string.IsNullOrEmpty(txtSYSExpiryDate.Text.Trim()) == false)
                entity.SYSExpiryDate = DateTime.Parse(txtSYSExpiryDate.Text.Trim());
            return entity;
        }
        private void CleanFrm()
        {
            txtSYSID.Text = "";
            txtSYSName.Text = "";
            txtSYSWeb.Text = "";
            txtSYSContact.Text = "";
            txtSYSContactTel.Text = "";
            txtSYSCDate.Text = "";
            txtSYSBeginDate.Text = "";
            txtSYSExpiryDate.Text = "";
        }
        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = GetSaveEntity();
                entity = svr.Save(entity);
                hidID.Value = entity.SYSID.ToString();
                this.ShowSaveOK();
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
                svr.DeleteById(typeof(CRMSystem), "SYSID", hidID.Value);
                this.ShowDeleteOK();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmSystem.aspx");
        }
    }
}

