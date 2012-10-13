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
using System.Collections.Generic;

namespace Terry.CRM.Web.CRM
{
    public partial class frmUserCustTransfer : BasePage
    {
        private UserService svr = new UserService();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                Authentication(enumModule.TransferCustomer);
                ddlFromUser.BindDropDownList(svr.GetUserByDep(), "UserName", "UserID");
                ddlToUser.BindDropDownList(svr.GetUserByDep(), "UserName", "UserID");

            }
        }

        protected void wizTran_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            svr.TransferCustomer(cblCustomers.SelectedValue, ddlToUser.SelectedValue);
            base.ShowSaveOK();
        }

        /// <summary>
        /// ActiveStepIndex start from zero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wizTran_ActiveStepChanged(object sender, EventArgs e)
        {
            if (wizTran.ActiveStepIndex == 1)
                if(!string.IsNullOrEmpty(ddlFromUser.SelectedValue))
                    cblCustomers.BindCheckBoxList(svr.GetCustomerByUserId(ddlFromUser.SelectedValue), "custName", "custId");
        }
    }
}
