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
    public partial class frmCustomerIndustryEdit : BasePage
    {
        private BaseService svr = new BaseService();

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
            var entity = (CRMCustomerIndustry)svr.LoadById(typeof(CRMCustomerIndustry), "IndustryID", hidID.Value);
            if (entity != null)
            {
                txtIndustryID.Text = entity.IndustryID.ToString();
                if (entity.Industry != null)
                {
                    txtIndustry.Text = entity.Industry.ToString();
                }
            }

        }
        private CRMCustomerIndustry GetSaveEntity()
        {
            var entity = new CRMCustomerIndustry();
            if (string.IsNullOrEmpty(txtIndustryID.Text.Trim()) == false)
                entity.IndustryID = int.Parse(txtIndustryID.Text.Trim());
            if (string.IsNullOrEmpty(txtIndustry.Text.Trim()) == false)
                entity.Industry = txtIndustry.Text.Trim();
            return entity;
        }
        private void CleanFrm()
        {
            txtIndustryID.Text = "";
            txtIndustry.Text = "";
        }
        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = GetSaveEntity();
                entity = svr.Save(entity);
                hidID.Value = entity.IndustryID.ToString();
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
                svr.DeleteById(typeof(CRMCustomerIndustry), "IndustryID", hidID.Value);
                this.ShowDeleteOK();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCustomerIndustry.aspx");
        }
    }
}

