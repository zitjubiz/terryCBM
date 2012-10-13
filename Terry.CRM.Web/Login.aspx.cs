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
using System.Management;

namespace Terry.CRM.Web
{
    public partial class Login : BasePage
    {
        protected string strAlert;
        private UserService svr = new UserService();
        private SystemService sys = new SystemService();
        protected void Page_Load(object sender, EventArgs e)
        {
            strAlert = base.GetREMes("MsgAlert");
            if (!Page.IsPostBack)
            {   //检查软件是否注册,根据CPU号进行一系列算法,与DB的Key比较
                lblServerID.Text = GetHDid(); //security exception on 1and1
                var SysInfo = sys.LoadById("1");
                if (SysInfo.SYSKey != svr.Encypt(lblServerID.Text, SysInfo.SYSName))
                {
                    if(DateTime.Now.DayOfWeek== DayOfWeek.Monday || DateTime.Now.DayOfWeek== DayOfWeek.Tuesday)
                        btnLogin.Enabled = false;

                    lblCompany.Text = SysInfo.SYSName;
                    lblIllegal.Visible = true;
                    lblIllegal.Text = "您使用的是未注册的软件.请联系<a href='http://www.2simplework.com'>2simplework.com</a>获取注册码";
                    txtKey.Visible = true;
                    btnReg.Visible = true;
                }
                else
                {
                    btnLogin.Enabled = true;
                    lblIllegal.Visible = true;
                    lblIllegal.Text = "该软件专为" + SysInfo.SYSName + "定制";
                    txtKey.Visible = false;
                    btnReg.Visible = false;
                }
                if (Request["ShowLicense"] == "1")
                {
                    lblIllegal.Visible = true;
                    lblIllegal.Text = "该软件专为" + SysInfo.SYSName + "定制，使用人数为(" + EnumLicenseCnt().ToString() + ")";
                    txtKey.Visible = true;
                    btnLicense.Visible = true;
                }
                txtloginid.Text = base.LastLoginUser;
                DisplayByCulture();
            }

        }
        /// <summary>
        /// use companyname & license count to hash
        /// </summary>
        /// <returns></returns>
        private int EnumLicenseCnt()
        {
            var SysInfo = sys.LoadById("1");

            for (int i = 0; i <= 200; i = i + 5)
            {
                if (SysInfo.SYSLicenseCnt == svr.Encypt(SysInfo.SYSName, i.ToString()))
                {
                    return i;
                }
            }
            return 1;
        }

        private void DisplayByCulture()
        {
            imgLogo.Src = "~/images/Logo_" + this.CurrentCulture + ".png";
            btnLogin.ImageUrl = "~/images/Login_" + this.CurrentCulture + ".gif";
            ddlLanguage.SelectedByValue(this.CurrentCulture);
        }

        private void InsertLoginHistory(long UserId, int Status)
        {
            CRMLoginHistory entity = new CRMLoginHistory();
            entity.UserId = UserId;
            entity.LoginAt = DateTime.Now;
            entity.ClientBrowser = GetBrowser();
            entity.ClientOS = GetOSVersion();
            entity.LoginIP = GetIPAddress();
            entity.Status = Status;
            svr.InsertLoginHistory(entity);


        }

        private void AddOnlineUserList(string LoginUserName)
        {
            /* 登录用户存入全局Application对象中,
                    * 如果已存在,则修改系统之前分配的Guid标识
                    */
            string m_strUserOnlineID = Guid.NewGuid().ToString();
            Dictionary<string, string> userlist;

            if (Application["OnlineUserList"] == null)
            {
                userlist = new Dictionary<string, string>();
            }
            else
            {
                userlist = Application["OnlineUserList"] as Dictionary<string, string>;
            }

            if (userlist.ContainsKey(LoginUserName))
            {
                userlist[LoginUserName] = m_strUserOnlineID;
            }
            else
            {
                userlist.Add(LoginUserName, m_strUserOnlineID);
            }
            Application.Add("OnlineUserList", userlist);
            CookieHelper.SetCookie("UserOnlineID", m_strUserOnlineID);
        }

        protected void btnLogin_Click(object sender, ImageClickEventArgs e)
        {
            int ErrCode = 0;
            vw_CRMUser user;
            if (svr.CRMUsers.Count() == 0)
                user = new vw_CRMUser() { UserID = 0, UserFullName = "admin", Role = "admin", RoleID = 1, RoleGrade = 9 };
            else
            {
                user = svr.Validate(txtloginid.Text.Trim(), txtpwd.Text.Trim(), ref ErrCode);
            }
            if (ErrCode == 0)
            {
                string LoginUserName = txtloginid.Text.Trim().ToUpper();
                if (LoginUserName != "ADMIN")
                    InsertLoginHistory(user.UserID, 0);

                AddOnlineUserList(LoginUserName);
                //---save login user info into session------
                LogUserInfo usrInfo = new LogUserInfo();
                usrInfo.LoginUserID = user.UserID;
                usrInfo.LoginUserName = LoginUserName;
                usrInfo.LoginUserFullName = user.UserFullName;
                usrInfo.LoginUserRoleID = (long)user.RoleID;
                usrInfo.LoginUserRoleGrade = (int)user.RoleGrade;
                usrInfo.LoginUserCompany = user.SYSID.ToString();

                Session[Session_ID] = usrInfo;


                base.StrCulture = ddlLanguage.SelectedValue;
                Response.Cookies["TerryCRMLang"].Value = ddlLanguage.SelectedValue;
                base.LastLoginUser = txtloginid.Text;
                FormsAuthentication.RedirectFromLoginPage(usrInfo.LoginUserID.ToString(), false);
                //Response.Redirect("~/default.aspx");
            }
            else
            {
                if (user != null)
                    InsertLoginHistory(user.UserID, 2); //2=密码错误

                ((Label)this.FindControl("lblJScript")).Text = GetREMes("MsgLoginError");
                //Page.ClientScript.RegisterClientScriptBlock(typeof(string),"loginfail",
                //    string.Format("<script>jAlert('{0}');</script>", GetREMes("MsgLoginError")));
            }
        }

        protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.StrCulture = ddlLanguage.SelectedValue;
            HttpContext.Current.Response.Cookies["TerryCRMLang"].Value = ddlLanguage.SelectedValue;
            Response.Redirect("login.aspx");
        }

        private string GetBrowser()
        {
            string ClientBrowser = Request.Browser.Browser + Request.Browser.Version;
            if (Request.UserAgent.IndexOf("Chrome") > 0)
                return "Chrome";
            if (Request.UserAgent.IndexOf("Maxthon/3") > 0)
                return "Maxthon 3.0";
            return ClientBrowser;
        }

        private string GetOSVersion()
        {
            string agent = HttpContext.Current.Request["HTTP_USER_AGENT"].ToString();
            if (agent.Contains("Windows NT 5.0"))
                return "Windows 2000";
            else if (agent.Contains("Windows NT 5.2"))
                return "Windows 2003";
            else if (agent.Contains("Windows NT 5.1"))
                return "Windows XP";
            else if (agent.Contains("Mac"))
                return "Mac";
            else if (agent.Contains("Unix"))
                return "Unix";
            else if (agent.Contains("Windows NT 6.1"))
                return "Windows 7"; //or Windows 2008 R2
            else
            {   //Windows NT 6.0
                return "Windows Vista"; //or Windows 2008
            }

        }

        /// <summary>
        /// Win7 IIS7.5,localhost will get ::1 as IP address
        /// </summary>
        /// <returns></returns>
        private string GetIPAddress()
        {
            string IP = "";
            try
            {
                if (HttpContext.Current.Request["HTTP_X_FORWARDED_FOR"] != null)
                {
                    string[] IPS = HttpContext.Current.Request["HTTP_X_FORWARDED_FOR"].ToString().Split(",;".ToCharArray());
                    for (int i = 0; i < IPS.Length; i++)
                    {
                        if (IPS[i].Trim() != "" && IPS[i].Trim().Substring(0, 3) != "10." && IPS[i].Trim().Substring(0, 7) != "192.168" && IPS[i].Trim().Substring(0, 6) != "127.0.")
                            IP = IPS[i];

                    }
                    if (IP == "")
                        IP = HttpContext.Current.Request["REMOTE_ADDR"];
                    if (IP == "")
                        IP = HttpContext.Current.Request.UserHostAddress.ToString();
                }
                else if (HttpContext.Current.Request["REMOTE_ADDR"] != null)
                    IP = HttpContext.Current.Request["REMOTE_ADDR"].ToString();
                else if (HttpContext.Current.Request.UserHostAddress != null)
                    IP = HttpContext.Current.Request.UserHostAddress.ToString();
            }
            catch (Exception e)
            {
                string message = e.Message.ToString() + "\r\n" + e.StackTrace;
                throw;
            }
            return IP;
        }

        ///   <summary>    
        ///   获取cpu序列号        
        ///   </summary>    
        ///   <returns> string </returns>    
        public string GetCpuInfo()
        {
            string cpuInfo = " ";
            using (ManagementClass cimobject = new ManagementClass("Win32_Processor"))
            {
                ManagementObjectCollection moc = cimobject.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                    mo.Dispose();
                }
            }
            return cpuInfo.ToString();
        }

        ///   <summary>    
        ///   获取硬盘ID        
        ///   </summary>    
        ///   <returns> string </returns>    
        public string GetHDid()
        {
            string HDid = " ";
            try
            {
                using (ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive"))
                {
                    ManagementObjectCollection moc1 = cimobject1.GetInstances();
                    foreach (ManagementObject mo in moc1)
                    {
                        HDid = (string)mo.Properties["Model"].Value;
                        mo.Dispose();
                        break;
                    }
                }
            }
            catch (Exception)
            {
                HDid = "Sharehost";
                throw;
            }

            return HDid.Trim().ToString();
        }

        ///   <summary>    
        ///   获取网卡硬件地址    
        ///   </summary>    
        ///   <returns> string </returns>    
        public string GetMacAddress()
        {
            string MoAddress = " ";
            using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                ManagementObjectCollection moc2 = mc.GetInstances();
                foreach (ManagementObject mo in moc2)
                {
                    if ((bool)mo["IPEnabled"] == true)
                        MoAddress = mo["MacAddress"].ToString();
                    mo.Dispose();
                }
            }
            return MoAddress.ToString();
        }

        protected void btnReg_Click(object sender, EventArgs e)
        {
            var SysInfo = sys.LoadById("1");

            if (((Button)sender).ID == "btnReg")
            {
                if (txtKey.Text.Trim() == svr.Encypt(lblServerID.Text, SysInfo.SYSName))
                    SysInfo.SYSKey = txtKey.Text.Trim();
            }
            else
            {
                SysInfo.SYSLicenseCnt = txtKey.Text.Trim();
            }
            sys.Save(SysInfo);
            Response.Redirect("login.aspx");
        }
    }
}
