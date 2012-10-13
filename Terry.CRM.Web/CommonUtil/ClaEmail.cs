
using System.Configuration;
using System.Collections;
using System.Data;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Net.Mail;
//using ConfigurationManager;


namespace Terry.CRM.Web.CommonUtil
{
    public class ClaEmail
    {
        public enum EmailBodyFormat
        { 
            Text=1,
            HTML=2
        }
        //用指定的信箱发信,所以不指定From
        public void SendMail(string mailTo, string subject, string body, EmailBodyFormat Format,params Attachment[] attachments)
        {

            SmtpClient mail = new SmtpClient();
            //实例
            mail.Host =ConfigurationManager.AppSettings["smtp"];
            //发信主机
            //mail.Credentials.GetCredential("smtp.163.com", 25, "Network"); //发信认证主机及端口
            mail.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["mailFrom"], 
                ConfigurationManager.AppSettings["mailFromPWD"]);

            //发件人
            MailMessage msg = new MailMessage(ConfigurationManager.AppSettings["mailFrom"], mailTo, subject, body);
            
            foreach (var item in attachments)
	        {
                if(item!=null)
                    msg.Attachments.Add(item);		 
	        }
            
            if(ConfigurationManager.AppSettings["mailToCC"]!="")
                msg.CC.Add(ConfigurationManager.AppSettings["mailToCC"]);

            if (ConfigurationManager.AppSettings["mailToBcc"] != "")
                msg.Bcc.Add(ConfigurationManager.AppSettings["mailToBcc"]);

            if (Format == EmailBodyFormat.HTML)
                msg.IsBodyHtml = true;
            else
                msg.IsBodyHtml = false;

            try
            {
                mail.Send(msg);

            }
            catch (Exception)
            {

                throw;
            }
        }
        
        //用指定的信箱发信,收件人用NoReply@XXX.com,真正收件人放在BCC里面
        public void SendMailBCC(string mailTo, string mailBCC,string subject, string body, EmailBodyFormat Format, params Attachment[] attachments)
        {

            SmtpClient mail = new SmtpClient();
            //实例
            mail.Host = ConfigurationManager.AppSettings["smtp"];
            //发信主机
            mail.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["mailFrom"],
                ConfigurationManager.AppSettings["mailFromPWD"]);

            //发件人
            MailMessage msg = new MailMessage(ConfigurationManager.AppSettings["mailFrom"], mailTo, subject, body);

            foreach (var item in attachments)
            {
                if (item != null)
                    msg.Attachments.Add(item);
            }

            if (ConfigurationManager.AppSettings["mailToCC"] != "")
                msg.CC.Add(ConfigurationManager.AppSettings["mailToCC"]);

            msg.Bcc.Add(mailBCC);
            if (ConfigurationManager.AppSettings["mailToBcc"] != "")
                msg.Bcc.Add(ConfigurationManager.AppSettings["mailToBcc"]);

            if (Format == EmailBodyFormat.HTML)
                msg.IsBodyHtml = true;
            else
                msg.IsBodyHtml = false;

            try
            {
                mail.Send(msg);

            }
            catch (Exception)
            {

                throw;
            }
        }
          
        //用匿名SMTP发信,指定From
        public void SendMail(string mailFrom, string mailTo, string subject, string body)
        {
            MailMessage msg = new MailMessage(mailFrom, mailTo, subject, body);
            SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["smtp"]);
            smtp.Send(msg);
        }

    }
}

