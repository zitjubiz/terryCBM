using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using Terry.CRM.Service;
using System.Web.Caching;
using log4net;
using log4net.Config;
using System.IO;
using System.Web.Hosting;
using Terry.CRM.Web.CommonUtil;


namespace Terry.CRM.Web
{
    public class Global : System.Web.HttpApplication
    {
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //写一个全局的定时器,5分钟执行一次
        private static System.Timers.Timer tmr = new System.Timers.Timer(300*1000);
        private string BackupDir = HostingEnvironment.MapPath("~/App_Backup");
        protected void Application_Start(object sender, EventArgs e)
        {
            //load config from web.config
            XmlConfigurator.Configure();

            //load config from seperate file log4net.config
            //XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
            
            tmr.Elapsed += new System.Timers.ElapsedEventHandler(tmr_Elapsed);
            tmr.AutoReset = true;
            tmr.Enabled = true;

            if (!Directory.Exists(BackupDir))
                Directory.CreateDirectory(BackupDir);
        }

        void tmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            tmr.Enabled = false;
            //每天18:00定时备份数据库到C:\backup目录
            
            if (DateTime.Now.Hour == 0)
            {                
                BaseService svr = new BaseService();
                String DatabaseName = svr.dataCtx.Connection.Database;
                String Pattern =DatabaseName+"_Full_"+ DateTime.Now.ToString("yyyyMMdd")+"*";
                String[] files =Directory.GetFiles(BackupDir, Pattern);
                if (files.Length ==0)
                {
                    svr.dataCtx.ExecuteCommand("exec [usp_BackupDatabase] '" + DatabaseName + "','" + BackupDir + "','F'");
                    log4netHelper.Info("Backuping DB " + DatabaseName + " at " + BackupDir);
                }
                else if (files.Length == 1)
                {
                    string ZipFileName = BackupDir + "\\" + DatabaseName + "_Full_" + DateTime.Now.ToString("yyyyMMdd") + ".zip";
                    Zipper.Zip(files[0], ZipFileName, "zt!@#456");
                    log4netHelper.Info("Backuping DB " + DatabaseName + " at " + BackupDir);
                }
                else 
                {
                }
            }
            tmr.Enabled = true;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

            Exception ex = HttpContext.Current.Server.GetLastError();
            logger.Error("\r\n Client IP:" + Request.UserHostAddress + 
                "\r\nURL:" + Request.Url + "\r\nException:" + Server.GetLastError().Message, ex);   
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}