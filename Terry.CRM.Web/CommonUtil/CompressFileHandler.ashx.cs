using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text;
using System.Web.SessionState;

namespace Terry.CRM.Web.CommonUtil
{
    /// <summary>
    /// CompressFileHandler 的摘要说明
    /// </summary>
    public class CompressFileHandler : IHttpHandler, IReadOnlySessionState
    {
        protected string Session_ID = ConfigurationManager.AppSettings["SessionID"];

        public void ProcessRequest(HttpContext context)
        {
            //App_Backup目录只有admin才能访问
            //upload 目录则所有普通用户都能访问
            //匿名用户不能下载任何文件
            string LoginUser =GetLoginUserName();
            bool IsVisitAppBackupDir =context.Request.RawUrl.IndexOf("app_backup", StringComparison.InvariantCultureIgnoreCase)>-1;
            if (LoginUser == "" || (IsVisitAppBackupDir && LoginUser != "admin"))
            {
                context.Response.ContentType = "text/html"; //IE要设成这样才能显示
                context.Response.Write("你没有权限下载此类文件。");
            }
            else
            {
                StartDownload(context, context.Request.RawUrl);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="downloadFile"></param>
        private void StartDownload(HttpContext context, string downloadFile)
        {
            context.Response.Buffer = true;
            context.Response.Clear();
            context.Response.AddHeader("content-disposition","attachment; filename=" 
                + HttpUtility.UrlEncode(downloadFile.Substring(downloadFile.LastIndexOf("/")+1), Encoding.UTF8));
            //context.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
            context.Response.ContentType ="application/octet-stream";
            context.Response.WriteFile(downloadFile);
            context.Response.Flush();
            context.Response.End();

        }
        private string GetLoginUserName()
        {
            
            if (HttpContext.Current.Session!=null && 
                HttpContext.Current.Session[Session_ID] != null)
            {
                LogUserInfo myUser = (LogUserInfo)HttpContext.Current.Session[Session_ID];
                if (myUser != null)
                {
                    return myUser.LoginUserName.ToLower();
                }
                else
                    return "";
            }
            else
                return "";
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}