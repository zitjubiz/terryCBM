using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Threading;
using System.Globalization;
using Terry.CRM;
using Terry.CRM.Entity;
using Terry.CRM.Service;
using System.IO;
using System.Collections.Generic;

namespace Terry.CRM.Web
{
    public class BasePage:System.Web.UI.Page
    {
        protected string Session_ID = ConfigurationManager.AppSettings["SessionID"];//"session_terrycrm";
        protected string Industry = ConfigurationManager.AppSettings["Industry"]; //Chemical /Ticket
        protected string DateFormatString ;
        protected string DateTimeFormatString ;

        protected string StrCulture;
        public int GridViewPageSize = 13;
        protected int recordCount;

        public BasePage()
        {
            DateFormatString = GetREMes("DateFormatStringCS");
            DateTimeFormatString = GetREMes("DateTimeFormatStringCS");
        }

        public void Authentication(enumModule Module,params Button[] SaveButtons)
        {
            if (Session[Session_ID] == null)
            {
                ShowMessage(GetREMes("MsgSessionTimeOut"));
                FormsAuthentication.RedirectToLoginPage();
                //Response.Write("<script>parent.window.location.href='../Login.aspx'</script>");
                Response.End();
                return;
            }
            else
            {
                /* 获取客户端的用户在线标识Guid
                * 如果标识Guid与服务端不一致,则重定向到重复登录页面
                 */
                string m_strUserOnlineID = CookieHelper.GetCookie("UserOnlineID").Value;
                if (!string.IsNullOrEmpty(m_strUserOnlineID))
                {
                    Dictionary<string, string> userlist = Application["OnlineUserList"] as Dictionary<string, string>;
                    if (m_strUserOnlineID != userlist[LoginUserName])
                    {
                        FormsAuthentication.RedirectToLoginPage();
                    }
                }
                /******** End *******/
                //Check Login User's role has right on specific module?
                BaseService svr = new BaseService();
                var rights =svr.GetRoleAccessRight(LoginUserRoleID, Module);
                if (rights==null||!rights.ReadOnly && !rights.New && !rights.Edit && !rights.Del)
                {
                    Response.Redirect("~/AccessDeny.aspx");
                    return;
                }
                else
                {
                    Button btnNew, btnDel, btnSave;

                    if (this.Master != null)
                    {                       
                        btnNew = (Button)this.Master.FindControl("CPH1").FindControl("btnNew");
                        btnDel = (Button)this.Master.FindControl("CPH1").FindControl("btnDel");
                        btnSave = (Button)this.Master.FindControl("CPH1").FindControl("btnSave");
                    }
                    else 
                    { 
                        btnNew = (Button)this.FindControl("btnNew");
                        btnDel = (Button)this.FindControl("btnDel");
                        btnSave = (Button)this.FindControl("btnSave");
                    }
                    if (!rights.New) { if (btnNew != null) btnNew.Enabled = false; }
                    if (!rights.Del) { if (btnDel != null) btnDel.Enabled = false; }

                    if (string.IsNullOrEmpty(Request["id"]))
                        return;

                    int id = Convert.ToInt32(Request["id"]);
                    //例如客户信息,销售录入后,是不能更改的,所以要区分新增和修改的权利
                    //id=0& New=false, id>0 & Edit=false
                    if ((!rights.Edit && id > 0) || (!rights.New && id == 0))
                    {
                        if (btnSave != null)
                            btnSave.Enabled = false;
                        foreach (var item in SaveButtons)
                        {
                            item.Enabled = false;
                        }
                    }                   
                }
            }
        }

        protected string CurrentCulture
        {
            get 
            {
                if (Request.Cookies["TerryCRMLang"] != null)
                    return (String)Request.Cookies["TerryCRMLang"].Value;
                else
                    return System.Globalization.CultureInfo.CurrentUICulture.Name;
            }
        }

        protected string LastLoginUser
        {
            get
            {
                if (Request.Cookies["TerryCRMLastLoginUser"] != null)
                    return (String)Request.Cookies["TerryCRMLastLoginUser"].Value;
                else
                    return "";
            }
            set 
            {
                Response.Cookies["TerryCRMLastLoginUser"].Value = value;
            }
        }

        protected override void InitializeCulture()
        {
            if (Request.Cookies["TerryCRMLang"] != null)
                StrCulture = (String)Request.Cookies["TerryCRMLang"].Value;

            if (!string.IsNullOrEmpty(StrCulture))
            { 
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(StrCulture);
            
            }
            base.InitializeCulture();
        }

        public string GetREMes(string reKey)
        {
            if (!string.IsNullOrEmpty(reKey))
            { 
                object mes=GetGlobalResourceObject("re", reKey);
                if (mes != null)
                    return mes.ToString().Trim();
                else
                    return "";
            }
            else
                return "";
        }

        public void ShowSaveFail(string Msg)
        {
            string strMessage = GetREMes("MsgSaveFail")+ " "+ GetREMes(Msg);
            if (GetREMes(Msg) == null) strMessage += Msg;
            this.ShowMessage(strMessage);
        }

        public void ShowSaveOK()
        {
            string strMessage = GetREMes("MsgSaveOK");
            this.ShowMessage(strMessage);
        }

        public void ShowDeleteOK()
        {
            string strMessage = GetREMes("MsgDeleteOK");
            this.ShowMessage(strMessage);
        }

        public void ShowMessage(string message)
        {
            
            message = FixedJsString(message);
            string js_alert = string.Format("<script>Alert('{0}');</script>", message);

            if (this.MasterPageFile != null)
            {
                Label alert = (Label)Master.FindControl("lblJScript");
                if (alert != null)
                    alert.Text = Server.HtmlEncode(message);
            }
            else
                ClientScript.RegisterClientScriptBlock(typeof(string), "msgbox", js_alert);
        }

        public void ShowMessage(Control ctr, string tipMsg)
        {
            tipMsg = FixedJsString(tipMsg);
            ScriptManager.RegisterStartupScript(ctr, this.GetType(), "ShowMessage_OnUpdatePanel", "alert('" + tipMsg + "');", true);
        }

        protected string FixedJsString(object str_in)
        {
            if (str_in is System.DBNull) return "";
            if (str_in == null) return "";
            string result = str_in.ToString();
            if (string.IsNullOrEmpty(result)) return "";

            if (result != null)
            {
                result = result.Replace("'", "\\'").Replace(System.Environment.NewLine, "\\n");
            }
            return result.Trim();
        }

        protected string IsNull(object obj)
        {
            if (obj != null)
                return obj.ToString();
            else
                return "";
        }

        public void DownloadFileAsAttachment(string FullFileName)
        {
            FileInfo DownloadFile = new FileInfo(FullFileName);
            if (DownloadFile.Exists)
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.Buffer = false;
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFile.Name, System.Text.Encoding.UTF8));
                Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                Response.WriteFile(DownloadFile.FullName);
                Response.Flush();
                Response.End();
            }
        }
        #region Login Info

        public long LoginUserID
        {
            get
            {
                long LoginUserID=0;
                if (Session[Session_ID] != null)
                {
                    LogUserInfo myUser = (LogUserInfo)Session[Session_ID];
                    if (myUser != null)
                    {
                        LoginUserID = myUser.LoginUserID;
                    }
                }
                return LoginUserID;
            }
        }

        public string LoginUserName
        {
            get
            {
                string LoginUserName = "";
                if (Session[Session_ID] != null)
                {
                    LogUserInfo myUser = (LogUserInfo)Session[Session_ID];
                    if (myUser != null)
                    {
                        LoginUserName = myUser.LoginUserName;
                    }
                }
                return LoginUserName;
            }
        }

        public string LoginUserFullName
        {
            get
            {
                string LoginUserFullName = "";
                if (Session[Session_ID] != null)
                {
                    LogUserInfo myUser = (LogUserInfo)Session[Session_ID];
                    if (myUser != null)
                    {
                        LoginUserFullName = myUser.LoginUserFullName;
                    }
                }
                return LoginUserFullName;
            }
        }

        public long LoginUserRoleID
        {
            get
            {
                long LoginUserRoleID=0 ;
                if (Session[Session_ID] != null)
                {
                    LogUserInfo myUser = (LogUserInfo)Session[Session_ID];
                    if (myUser != null)
                    {
                        LoginUserRoleID = myUser.LoginUserRoleID;
                    }
                }
                return LoginUserRoleID;
            }
        }

        public int LoginUserRoleGrade
        {
            get
            {
                int LoginUserRoleGrade = 0;
                if (Session[Session_ID] != null)
                {
                    LogUserInfo myUser = (LogUserInfo)Session[Session_ID];
                    if (myUser != null)
                    {
                        LoginUserRoleGrade = myUser.LoginUserRoleGrade;
                    }
                }
                return LoginUserRoleGrade;
            }
        }

        public string LoginUserCompany
        {
            get
            {
                string LoginUserCompany = "";
                if (Session[Session_ID] != null)
                {
                    LogUserInfo myUser = (LogUserInfo)Session[Session_ID];
                    if (myUser != null)
                    {
                        LoginUserCompany = myUser.LoginUserCompany;
                    }
                }
                return LoginUserCompany;
            }
        }


        #endregion
    }
}
