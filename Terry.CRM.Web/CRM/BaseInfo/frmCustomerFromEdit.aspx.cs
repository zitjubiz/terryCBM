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
    public partial class frmCustomerFromEdit : BasePage
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
            var entity = (CRMCustomerFrom)svr.LoadById(typeof(CRMCustomerFrom), "ID", hidID.Value);
            if (entity != null)
            {
                txtID.Text = entity.ID.ToString();
                if (entity.CustFrom != null)
                {
                    txtCustFrom.Text = entity.CustFrom.ToString();
                }
            }

        }
        private CRMCustomerFrom GetSaveEntity()
        {
            var entity = new CRMCustomerFrom();
            if (string.IsNullOrEmpty(txtID.Text.Trim()) == false)
                entity.ID = int.Parse(txtID.Text.Trim());
            if (string.IsNullOrEmpty(txtCustFrom.Text.Trim()) == false)
                entity.CustFrom = txtCustFrom.Text.Trim();
            return entity;
        }
        private void CleanFrm()
        {
            txtID.Text = "";
            txtCustFrom.Text = "";
        }
        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = GetSaveEntity();
                entity = svr.Save(entity);
                hidID.Value = entity.ID.ToString();
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
                svr.DeleteById(typeof(CRMCustomerFrom), "ID", hidID.Value);
                this.ShowDeleteOK();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCustomerFrom.aspx");
        }
    }
}

