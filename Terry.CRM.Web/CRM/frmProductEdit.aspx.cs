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
    public partial class frmProductEdit : BasePage
    {
        private BaseService svr = new BaseService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Authentication(enumModule.Product);
                btnDel.Attributes.Add("onclick", "onDel('" + btnDel.UniqueID + "');return false;");
                hidID.Value = Request["id"];
                BindData();
            }
        }

        private void BindData()
        {

            //bind entity
            var entity = (CRMProduct)svr.LoadById(typeof(CRMProduct), "ProdID", hidID.Value);
            if (entity != null)
            {

                txtProdID.Text = entity.ProdID.ToString();
                if (entity.Product != null)
                {
                    txtProduct.Text = entity.Product.ToString();
                }
                if (entity.Code != null)
                {
                    txtCode.Text = entity.Code.ToString();
                }
                if (entity.ProductFullName != null)
                {
                    txtProductFullName.Text = entity.ProductFullName.ToString();
                }
                if (entity.ProductFactor != null)
                {
                    txtProductFactor.Text = entity.ProductFactor.ToString();
                }
            }

        }
        private CRMProduct GetSaveEntity()
        {
            var entity = new CRMProduct();
            if (string.IsNullOrEmpty(txtProdID.Text.Trim()) == false)
                entity.ProdID = int.Parse(txtProdID.Text.Trim());
            if (string.IsNullOrEmpty(txtProduct.Text.Trim()) == false)
                entity.Product = txtProduct.Text.Trim();
            if (string.IsNullOrEmpty(txtCode.Text.Trim()) == false)
                entity.Code = txtCode.Text.Trim();
            if (string.IsNullOrEmpty(txtProductFullName.Text.Trim()) == false)
                entity.ProductFullName = txtProductFullName.Text.Trim();
            if (string.IsNullOrEmpty(txtProductFactor.Text.Trim()) == false)
                entity.ProductFactor = float.Parse(txtProductFactor.Text.Trim());
            return entity;
        }
        private void CleanFrm()
        {
            txtProdID.Text = "";
            txtProduct.Text = "";
            txtProductFullName.Text = "";
        }
        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = GetSaveEntity();
                entity = svr.Save(entity);
                hidID.Value = entity.ProdID.ToString();
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
                svr.DeleteById(typeof(CRMProduct), "ProdID", hidID.Value);
                this.ShowDeleteOK();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmProduct.aspx");
        }
    }
}

