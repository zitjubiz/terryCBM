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
    public partial class frmDepartmentEdit : BasePage
    {
        private BaseService svr = new BaseService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Authentication(enumModule.Dept);
                btnDel.Attributes.Add("onclick", "onDel('" + btnDel.UniqueID + "');return false;");
                hidID.Value = Request["id"];
                BindData();
            }
        }

        private void BindData()
        {
            //TODO: bind Dropdownlist,change accordingly

            //bind entity
            var entity = (CRMDepartment)svr.LoadById(typeof(CRMDepartment), "DepID", hidID.Value);
            if (entity != null)
            {
                txtDepID.Text = entity.DepID.ToString();
                txtDepAddress.Text = entity.DepAddress;
                if (entity.DepName != null)
                {
                    txtDepName.Text = entity.DepName.ToString();
                }
            }

        }
        private CRMDepartment GetSaveEntity()
        {
            var entity = new CRMDepartment();
            if (string.IsNullOrEmpty(txtDepID.Text.Trim()) == false)
                entity.DepID = int.Parse(txtDepID.Text.Trim());
            if (string.IsNullOrEmpty(txtDepName.Text.Trim()) == false)
                entity.DepName = txtDepName.Text.Trim();
            if (string.IsNullOrEmpty(txtDepAddress.Text.Trim()) == false)
                entity.DepAddress = txtDepAddress.Text.Trim();
            return entity;
        }
        private void CleanFrm()
        {
            txtDepID.Text = "";
            txtDepName.Text = "";
            txtDepAddress.Text = "";
        }
        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = GetSaveEntity();
                entity = svr.Save(entity);
                hidID.Value = entity.DepID.ToString();
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
                svr.DeleteById(typeof(CRMDepartment), "DepID", hidID.Value);
                this.ShowDeleteOK();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmDepartment.aspx");
        }
    }
}
