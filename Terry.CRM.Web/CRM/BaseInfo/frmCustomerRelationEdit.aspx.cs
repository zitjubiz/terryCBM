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
    public partial class frmCustomerRelationEdit : BasePage
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
            var entity = (CRMCustomerRelation)svr.LoadById(typeof(CRMCustomerRelation), "RelationID", hidID.Value);
            if (entity != null)
            {
                txtRelationID.Text = entity.RelationID.ToString();
                if (entity.Relation != null)
                {
                    txtRelation.Text = entity.Relation.ToString();
                }
            }

        }
        private CRMCustomerRelation GetSaveEntity()
        {
            var entity = new CRMCustomerRelation();
            if (string.IsNullOrEmpty(txtRelationID.Text.Trim()) == false)
                entity.RelationID = int.Parse(txtRelationID.Text.Trim());
            if (string.IsNullOrEmpty(txtRelation.Text.Trim()) == false)
                entity.Relation = txtRelation.Text.Trim();
            return entity;
        }
        private void CleanFrm()
        {
            txtRelationID.Text = "";
            txtRelation.Text = "";
        }
        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = GetSaveEntity();
                entity = svr.Save(entity);
                //hidID.Value = entity.RelationID.ToString();
                //this.ShowSaveOK();
                Response.Redirect("frmCustomerRelation.aspx");
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
                svr.DeleteById(typeof(CRMCustomerRelation), "RelationID", hidID.Value);
                this.ShowDeleteOK();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCustomerRelation.aspx");
        }
    }
}

