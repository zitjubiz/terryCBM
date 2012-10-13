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
    public partial class frmProvinceEdit : BasePage
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
            var entity = (CRMProvince)svr.LoadById(typeof(CRMProvince), "ProvinceID", hidID.Value);
            if (entity != null)
            {
                txtProvinceID.Text = entity.ProvinceID.ToString();
                if (entity.Province != null)
                {
                    txtProvince.Text = entity.Province.ToString();
                }
                if (entity.Region != null)
                {
                    txtRegion.Text = entity.Region.ToString();
                }
            }

        }
        private CRMProvince GetSaveEntity()
        {
            var entity = new CRMProvince();
            if (string.IsNullOrEmpty(txtProvinceID.Text.Trim()) == false)
                entity.ProvinceID = int.Parse(txtProvinceID.Text.Trim());
            if (string.IsNullOrEmpty(txtProvince.Text.Trim()) == false)
                entity.Province = txtProvince.Text.Trim();
            if (string.IsNullOrEmpty(txtRegion.Text.Trim()) == false)
                entity.Region = txtRegion.Text.Trim();
            return entity;
        }
        private void CleanFrm()
        {
            txtProvinceID.Text = "";
            txtProvince.Text = "";
            txtRegion.Text = "";
        }
        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = GetSaveEntity();
                entity = svr.Save(entity);
                hidID.Value = entity.ProvinceID.ToString();
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
                svr.DeleteById(typeof(CRMProvince), "ProvinceID", hidID.Value);
                this.ShowDeleteOK();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmProvince.aspx");
        }
    }
}

