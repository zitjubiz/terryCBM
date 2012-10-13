using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Terry.CRM.Entity;
using Terry.CRM.Service;
using System.Collections.Generic;

namespace Terry.CRM.Web.CRM
{
    public partial class frmCategoryEdit : BasePage
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
                BindCatProduct();
            }
        }

        private void BindCatProduct()
        {
            //bind Products(2nd level)
            DataTable dt = svr.SearchByCriteria("CRMProduct", out recordCount, " and len(code)=3", "code");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                dr["Product"] = dr["Product"] + "(" + dr["Code"] + ")";
            }
            DDCLProduct.DataSource = dt;
            DDCLProduct.DataTextField = "Product";
            DDCLProduct.DataValueField = "Code";
            DDCLProduct.DataBind();
            //bind Category relative products
            var Prods = svr.GetCatProducts(long.Parse(hidID.Value));
            foreach (var p in Prods)
            {
                foreach (ListItem item in DDCLProduct.Items)
                {
                    if (item.Value == p.ProdCode.ToString())
                        item.Selected = true;
                }
            }
        }


        private void BindData()
        {
            //bind entity
            var entity = (CRMCategory)svr.LoadById(typeof(CRMCategory), "CatID", hidID.Value);
            if (entity != null)
            {
                txtCategoryID.Text = entity.CatID.ToString();
                if (entity.Category != null)
                {
                    txtCategory.Text = entity.Category.ToString();
                }
            }

        }
        private CRMCategory GetSaveEntity()
        {
            var entity = new CRMCategory();
            if (string.IsNullOrEmpty(txtCategoryID.Text.Trim()) == false)
                entity.CatID = int.Parse(txtCategoryID.Text.Trim());
            if (string.IsNullOrEmpty(txtCategory.Text.Trim()) == false)
                entity.Category = txtCategory.Text.Trim();
            return entity;
        }
        private void CleanFrm()
        {
            txtCategoryID.Text = "";
            txtCategory.Text = "";
        }
        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = GetSaveEntity();

                List<CRMProduct> ProdList = new List<CRMProduct>();
                string[] arrP = DDCLProduct.SelectedValuesToString().Split(',');
                foreach (var Code in arrP)
                {
                    if (!string.IsNullOrEmpty(ID))
                    {
                        var p = new CRMProduct();
                        p.Code = Code.Trim();
                        ProdList.Add(p);

                    }
                }


                entity = svr.Save(entity, ProdList);
                hidID.Value = entity.CatID.ToString();
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
                //级联删除Cat和CateProds
                svr.DeleteById(typeof(CRMCategory), "CatID", hidID.Value);
                this.ShowDeleteOK();
                Response.Redirect("frmCategory.aspx");
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCategory.aspx");
        }
    }
}
