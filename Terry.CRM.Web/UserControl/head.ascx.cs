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

namespace Terry.CRM.Web.UserControl
{
    public partial class head : System.Web.UI.UserControl
    {
        protected string Session_ID = ConfigurationManager.AppSettings["SessionID"];
        protected string Industry = ConfigurationManager.AppSettings["Industry"];
        BasePage page = HttpContext.Current.Handler as BasePage;
        public string LoginUserName = "";
        BaseService svr = new BaseService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (page == null)
                    throw new Exception("Web页面必须继承BasePage");

                ImgLogo.ImageUrl = "~/images/Logo_" + this.CurrentCulture + ".png";
                GetUserName();
                if (Industry == "Ticket")
                {
                    ulSchedule.Visible = false;
                    ulCustBrief.Visible = false;
                    ulProduct.Visible = false;
                }
                else if (Industry == "Chemical")
                {
                    lnkCustomer.NavigateUrl = "~/CRM_Chem/frmCustomer.aspx";
                    ulInvoice.Visible = false;
                    ulEmail.Visible = false;
                    ulMantis.Visible = false;
                    ulReport.Visible = false;
                }
                var rights = svr.GetRoleAccessRight(page.LoginUserRoleID, enumModule.EmailMarketing);
                if (rights == null || !rights.ReadOnly && !rights.New && !rights.Edit && !rights.Del)
                    lnkEmail.Visible = false;
                rights = svr.GetRoleAccessRight(page.LoginUserRoleID, enumModule.Report);
                if (rights == null || !rights.ReadOnly && !rights.New && !rights.Edit && !rights.Del)
                { 
                    lnkReport.Visible = false;
                    lnkBillReport.Visible = false;
                
                }
                rights = svr.GetRoleAccessRight(page.LoginUserRoleID, enumModule.Customer);
                if (rights == null || !rights.ReadOnly && !rights.New && !rights.Edit && !rights.Del)
                {
                    ulCustomer.Visible = false;
                    ulAction.Visible = false;
                }
                rights = svr.GetRoleAccessRight(page.LoginUserRoleID, enumModule.Schedule);
                if (rights == null || !rights.ReadOnly && !rights.New && !rights.Edit && !rights.Del)
                {
                    ulSchedule.Visible = false;
                } 
                //销售人员=1,市场部=2,部门经理=3 都不能看控制面板
                if (page.LoginUserRoleGrade == (int)enumRoleGrade.Sales ||
                    page.LoginUserRoleGrade == (int)enumRoleGrade.ProdManager||
                    page.LoginUserRoleGrade == (int)enumRoleGrade.DepManager ||
                    page.LoginUserRoleGrade == (int)enumRoleGrade.SalesManager)
                {
                    ulSys.Visible = false;
                }
                
                HideUnauthorizedLinks();
                           
            }

        }

        //把没有权限的link隐藏起来
        private void HideUnauthorizedLinks()
        {
            var rights = svr.GetRoleAccessRight(page.LoginUserRoleID, enumModule.User);
            if (rights == null || !rights.ReadOnly && !rights.New && !rights.Edit && !rights.Del)
                hlUser.Visible = false;
            rights = svr.GetRoleAccessRight(page.LoginUserRoleID, enumModule.Dept);
            if (rights == null || !rights.ReadOnly && !rights.New && !rights.Edit && !rights.Del)
                hlDept.Visible = false;
            rights = svr.GetRoleAccessRight(page.LoginUserRoleID, enumModule.Role);
            if (rights == null || !rights.ReadOnly && !rights.New && !rights.Edit && !rights.Del)
                hlRole.Visible = false;
            rights = svr.GetRoleAccessRight(page.LoginUserRoleID, enumModule.Product);
            if (rights == null || !rights.ReadOnly && !rights.New && !rights.Edit && !rights.Del)
            {
                hlProd.Visible = false;
                hlCategory.Visible = false;
                ulProduct.Visible = false;
            }
            rights = svr.GetRoleAccessRight(page.LoginUserRoleID, enumModule.TransferCustomer);
            if (rights == null || !rights.ReadOnly && !rights.New && !rights.Edit && !rights.Del)
                hlUserTransfer.Visible = false;

            rights = svr.GetRoleAccessRight(page.LoginUserRoleID, enumModule.LoginHistory);
            if (rights == null || !rights.ReadOnly && !rights.New && !rights.Edit && !rights.Del)
                hlLoginHistory.Visible = false;

            //HRLIN 角色
            rights = svr.GetRoleAccessRight(page.LoginUserRoleID, enumModule.CustomerBrief);
            if (rights == null || !rights.ReadOnly && !rights.New && !rights.Edit && !rights.Del)
                ulCustBrief.Visible = false;
            else
            {
                hlCategory.Visible = false;
                hlProvince.Visible = false;
            }
  
            //这个维护基础数据的页面只给IT管理员
            if (lblLoginUserID.Text.ToLower() == "admin")
                hlBaseInfo.Visible = true;
            else
                hlBaseInfo.Visible = false;

        }

        private void GetUserName()
        {

            if (Session[Session_ID] != null)
            {
                LogUserInfo myUser = (LogUserInfo)Session[Session_ID];
                if (myUser != null)
                {
                    LoginUserName = myUser.LoginUserName;
                }
            }
            lblLoginUserID.Text = LoginUserName;
        }

        protected string CurrentCulture
        {
            get
            {
                if (Request.Cookies["TerryCRMLang"] != null)
                    return (String)Request.Cookies["TerryCRMLang"].Value;
                else
                    return "zh-CN";
            }
        }
    }
}