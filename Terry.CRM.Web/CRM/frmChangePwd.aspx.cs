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
using Terry.CRM;
using Terry.CRM.Entity;
using Terry.CRM.Service;

namespace Terry.CRM.Web.CRM
{
    public partial class ChangePwd : BasePage
    {
        private UserService svr = new UserService();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ErrCode=0;
            svr.Validate(base.LoginUserName, txtOld.Text.Trim(), ref ErrCode);
            if (ErrCode>0)
            {
                ShowMessage(GetREMes("MsgChangePwdFail"));
                return;
            }

            try
            {
                CRMUser u;
                u = (CRMUser)svr.LoadById(typeof(CRMUser), "UserID", base.LoginUserID.ToString());
                if (txtPwd.Text.Trim() != "")
                    u.Password = svr.Encypt(u.UserName, txtPwd.Text.Trim());
                else
                    u.Password = u.Password;
                u.ModifyDate = DateTime.Now;
                u.ModifyUser = base.LoginUserID;
                svr.Save(u);
                this.ShowSaveOK();
            }
            catch (Exception ex)
            {
                ShowSaveFail(ex.Message);
            }
        }
    }
}
