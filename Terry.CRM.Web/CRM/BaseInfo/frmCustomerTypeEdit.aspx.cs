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
    public partial class frmCustomerTypeEdit : BasePage
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
            var entity = (CRMCustomerType)svr.LoadById(typeof(CRMCustomerType), "CustTypeID", hidID.Value);
            if (entity != null)
            {
                txtCustTypeID.Text = entity.CustTypeID.ToString();
                if (entity.CustType != null)
                {
                    txtCustType.Text = entity.CustType.ToString();
                }
            }

        }
        private CRMCustomerType GetSaveEntity()
        {
            var entity = new CRMCustomerType();
            if (string.IsNullOrEmpty(txtCustTypeID.Text.Trim()) == false)
                entity.CustTypeID = int.Parse(txtCustTypeID.Text.Trim());
            if (string.IsNullOrEmpty(txtCustType.Text.Trim()) == false)
                entity.CustType = txtCustType.Text.Trim();
            return entity;
        }
        private void CleanFrm()
        {
            txtCustTypeID.Text = "";
            txtCustType.Text = "";
        }
        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = GetSaveEntity();
                entity = svr.Save(entity);
                //hidID.Value = entity.CustTypeID.ToString();
                //this.ShowSaveOK();
                Response.Redirect("frmCustomerType.aspx");
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
                svr.DeleteById(typeof(CRMCustomerType), "CustTypeID", hidID.Value);
                this.ShowDeleteOK();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCustomerType.aspx");
        }
    }
}

