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
    public partial class frmCountryEdit : BasePage
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
            var entity = (CRMCountry)svr.LoadById(typeof(CRMCountry), "CountryID", hidID.Value);
            if (entity != null)
            {
                txtCountryID.Text = entity.CountryID.ToString();
                if (entity.Country != null)
                {
                    txtCountry.Text = entity.Country.ToString();
                }
            }

        }
        private CRMCountry GetSaveEntity()
        {
            var entity = new CRMCountry();
            if (string.IsNullOrEmpty(txtCountryID.Text.Trim()) == false)
                entity.CountryID = int.Parse(txtCountryID.Text.Trim());
            if (string.IsNullOrEmpty(txtCountry.Text.Trim()) == false)
                entity.Country = txtCountry.Text.Trim();
            return entity;
        }
        private void CleanFrm()
        {
            txtCountryID.Text = "";
            txtCountry.Text = "";
        }
        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = GetSaveEntity();
                entity = svr.Save(entity);
                hidID.Value = entity.CountryID.ToString();
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
                svr.DeleteById(typeof(CRMCountry), "CountryID", hidID.Value);
                this.ShowDeleteOK();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCountry.aspx");
        }
    }
}

