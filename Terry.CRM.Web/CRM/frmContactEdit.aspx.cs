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
    public partial class frmContactEdit : BasePage
    {
        private ContactService svr = new ContactService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Authentication(enumModule.Customer);
                btnDel.Attributes.Add("onclick", "onDel('" + btnDel.UniqueID + "');return false;");
                hidID.Value = Request["id"];
                BindData();
            }
        }
        //按ID绑定或者按CustID和TypeID绑定
        private void BindData()
        {
            //TODO: bind Dropdownlist,change accordingly
            txtContactTypeID.BindDropDownList(svr.GetContactType(), "ContactType", "ID");
            txtContactCustID.BindDropDownList(svr.GetCustomer(), "CustName", "CustID");

            //bind entity
            vw_CRMContact entity; 
            if(Request["id"]=="")
                entity = (vw_CRMContact)svr.LoadById(typeof(vw_CRMContact), "ContactID", hidID.Value);
            else
                entity = svr.LoadById(Request["CustID"],int.Parse(Request["TypeID"]));

            if (entity != null)
            {
                txtContactID.Text = entity.ContactID.ToString();
                txtContactCustID.Text = entity.ContactCustID.ToString();
                txtContactSex.Text = entity.ContactSex.ToString();

                if (entity.ContactName != null)
                {
                    txtContactName.Text = entity.ContactName.ToString();
                }

                if (entity.ContactTypeID != null)
                {
                    txtContactTypeID.Text = entity.ContactTypeID.ToString();
                }
                if (entity.ContactDept != null)
                {
                    txtContactDept.Text = entity.ContactDept.ToString();
                }
                if (entity.ContactTitle != null)
                {
                    txtContactTitle.Text = entity.ContactTitle.ToString();
                }
                if (entity.ContactTel != null)
                {
                    txtContactTel.Text = entity.ContactTel.ToString();
                }
                if (entity.ContactMobile != null)
                {
                    txtContactMobile.Text = entity.ContactMobile.ToString();
                }
                if (entity.ContactEmail != null)
                {
                    txtContactEmail.Text = entity.ContactEmail.ToString();
                }
                if (entity.ContactMSN != null)
                {
                    txtContactMSN.Text = entity.ContactMSN.ToString();
                }
                if (entity.ContactQQ != null)
                {
                    txtContactQQ.Text = entity.ContactQQ.ToString();
                }
                if (entity.ContactFax != null)
                {
                    txtContactFax.Text = entity.ContactFax.ToString();
                }
            }
            else
            {
                txtContactCustID.SelectedByValue(Request["CustID"]);
                txtContactTypeID.SelectedByValue(Request["TypeID"]);

            }

        }

        private CRMContact GetSaveEntity()
        {
            var entity = new CRMContact();
            if (string.IsNullOrEmpty(txtContactID.Text.Trim()) == false)
                entity.ContactID = int.Parse(txtContactID.Text.Trim());
            if (string.IsNullOrEmpty(txtContactName.Text.Trim()) == false)
                entity.ContactName = txtContactName.Text.Trim();
            if (string.IsNullOrEmpty(txtContactSex.Text.Trim()) == false)
                entity.ContactSex = Boolean.Parse(txtContactSex.Text.Trim());
            if (string.IsNullOrEmpty(txtContactTypeID.Text.Trim()) == false)
                entity.ContactTypeID = int.Parse(txtContactTypeID.Text.Trim());
            if (string.IsNullOrEmpty(txtContactDept.Text.Trim()) == false)
                entity.ContactDept = txtContactDept.Text.Trim();
            if (string.IsNullOrEmpty(txtContactTitle.Text.Trim()) == false)
                entity.ContactTitle = txtContactTitle.Text.Trim();
            if (string.IsNullOrEmpty(txtContactTel.Text.Trim()) == false)
                entity.ContactTel = txtContactTel.Text.Trim();
            if (string.IsNullOrEmpty(txtContactMobile.Text.Trim()) == false)
                entity.ContactMobile = txtContactMobile.Text.Trim();
            if (string.IsNullOrEmpty(txtContactEmail.Text.Trim()) == false)
                entity.ContactEmail = txtContactEmail.Text.Trim();
            if (string.IsNullOrEmpty(txtContactMSN.Text.Trim()) == false)
                entity.ContactMSN = txtContactMSN.Text.Trim();
            if (string.IsNullOrEmpty(txtContactQQ.Text.Trim()) == false)
                entity.ContactQQ = txtContactQQ.Text.Trim();
            if (string.IsNullOrEmpty(txtContactFax.Text.Trim()) == false)
                entity.ContactFax = txtContactFax.Text.Trim();
            if (string.IsNullOrEmpty(txtContactCustID.Text.Trim()) == false)
                entity.ContactCustID = int.Parse(txtContactCustID.Text.Trim());

            entity.IsActive = true;
            return entity;
        }

        private void CleanFrm()
        {
            txtContactID.Text = "";
            txtContactName.Text = "";
            txtContactSex.Text = "";
            txtContactTypeID.SelectedIndex = 0;
            txtContactDept.Text = "";
            txtContactTitle.Text = "";
            txtContactTel.Text = "";
            txtContactMobile.Text = "";
            txtContactEmail.Text = "";
            txtContactMSN.Text = "";
            txtContactQQ.Text = "";
            txtContactFax.Text = "";
        }
        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = GetSaveEntity();
                entity = svr.Save(entity);
                hidID.Value = entity.ContactID.ToString();
                lblMsg.Text = GetREMes("MsgSaveOK");
                //this.ShowSaveOK();
                //清空页面，方便新增
                //CleanFrm();
                btnBack_Click(null, null);
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
                svr.SoftDeleteById(typeof(CRMContact), "ContactID", hidID.Value);
                this.ShowDeleteOK();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterClientScriptBlock(typeof(string),"cls",
            @"<script type='text/javascript'>
            window.parent.document.getElementById('ctl00_CPH1_btnRefresh').click();
            window.parent.hidePopWin(false);</script>");
            //if (Request["CustID"] == null)
            //    Response.Redirect("frmContact.aspx");
            //else
            //    Response.Redirect("frmCustomerEdit.aspx?id=" + Request["CustID"]);
        }
    }
}

