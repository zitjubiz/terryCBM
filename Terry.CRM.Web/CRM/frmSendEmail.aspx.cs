using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Terry.CRM.Service;
using System.Collections;
using System.IO;
using System.Text;
using Terry.CRM.Web.CommonUtil;
using System.Net.Mail;
using System.Threading;
using System.Configuration;
using Terry.CRM.Entity;

namespace Terry.CRM.Web.CRM
{
    public partial class frmSendEmail : BasePage
    {
        private CustomerService svr = new CustomerService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Authentication(enumModule.EmailMarketing);

                ddlEmailGroup.BindDropDownListAndSelect(svr.GetEmailGroup(base.LoginUserID), "GroupName", "GroupName");

                if (Request["from"] != null && Request["from"].ToLower() == "search")
                {
                    radEmailGroup.SelectedIndex = 0;
                    PanelOthers.Visible = true;
                    GetDataFromSearch();
                }
                else
                {
                    PanelOthers.Visible = false;
                    radEmailGroup.SelectedIndex = 1;
                }
            }
        }

        private void GetDataFromSearch()
        {
            if (Session["EmailFilter"] != null)
            {
                string Filter = Session["EmailFilter"].ToString();

                string TableName = "vw_CRMCustomer2";
                string OrderBy = "CustName";
                Listmail.DataSource = svr.SearchByCriteria(TableName, out recordCount, Filter, OrderBy);
                Listmail.DataTextField = "CustName";
                Listmail.DataValueField = "CustEmail";
                Listmail.DataBind();
            }
            else
            {
                Listmail.Items.Clear();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Listmail.Items.Count == 0)
            {
                this.ShowMessage("客户群的Email不能为空");
                return;
            }
            if(CheckUploadFileSize(FileUpload1,FileUpload2,FileUpload3)!=0)
            {
                this.ShowMessage("附件大小必须小于2M");
                return;
            }
            UploadFile(FileUpload1, HiddenField1);
            UploadFile(FileUpload2, HiddenField2);
            UploadFile(FileUpload3, HiddenField3);
            //有2种发送方式,所有人只发一封,或者每人单独发一封
            ClaEmail eml = new ClaEmail();
            
             Attachment[] attachments = new Attachment[3];
            if(HiddenField1.Value!="")
                attachments[0]= new Attachment(HiddenField1.Value);
            if(HiddenField2.Value!="")
                attachments[1]= new Attachment(HiddenField2.Value);
            if(HiddenField3.Value!="")
                attachments[2]= new Attachment(HiddenField3.Value);
            if (ConfigurationManager.AppSettings["mailOneByOne"] == "0")
            {
                ///第一种 BCC
                string BCCEmailList = "";
                foreach (ListItem item in Listmail.Items)
                {
                    BCCEmailList += item.Value + ",";
                }
                BCCEmailList = BCCEmailList.Substring(0, BCCEmailList.Length - 1);

                eml.SendMailBCC("NoReply@Fitt-group.com",BCCEmailList, txtSubject.Text, txtbody.Value, ClaEmail.EmailBodyFormat.HTML, attachments);
            }
            else
            {
                ///第二种,smtp有限制,163的邮件只能连续发15封,只能用自建服务器的SMTP
                foreach (ListItem item in Listmail.Items)
                {
                    eml.SendMail(item.Value, txtSubject.Text, "Dear " + item.Text + " <br/><br/>" + txtbody.Value, ClaEmail.EmailBodyFormat.HTML, attachments);
                    Thread.Sleep(500);
                }
            }
            this.ShowMessage(@"email 发送完成,但因为网络及其他原因,不能100%保证已发到收件人信箱,
            请检查发件人邮箱是否有发送失败回信.");
        }

        /// <summary>
        /// 检查附件文件大小
        /// </summary>
        /// <param name="files"></param>
        /// <returns>0=OK,1=filesize为0,2=size超过2M</returns>
        private int CheckUploadFileSize(params FileUpload[] files)
        {
            foreach (FileUpload file in files)
            {
                string IstrFileName = file.PostedFile.FileName;

                if (!string.IsNullOrEmpty(IstrFileName))
                {
                    if (file.PostedFile.ContentLength == 0)
                        return 1;
                    else if (file.PostedFile.ContentLength >= (2 * 1024 * 1024))
                        return 2;
                }               
            } 
            return 0;
        }
        private void UploadFile(FileUpload fUploadFile, HiddenField hidFile)
        {
            string IstrFileName = fUploadFile.PostedFile.FileName;
            string IstrFileNamePath = "";
            string IstrFileFolder = Server.MapPath("~/Upload/Attachment/");

            if (string.IsNullOrEmpty(IstrFileName))
            {
                hidFile.Value = IstrFileNamePath;
                return;
            }

            IstrFileName = Path.GetFileName(IstrFileName);
            string strTmp = HttpUtility.UrlEncode(IstrFileName, Encoding.UTF8);
            IstrFileName = HttpUtility.UrlDecode(strTmp);

            if ((!Directory.Exists(IstrFileFolder)))
            {
                Directory.CreateDirectory(IstrFileFolder);
            }

            IstrFileNamePath = IstrFileFolder + IstrFileName;

            fUploadFile.PostedFile.SaveAs(IstrFileNamePath);
            hidFile.Value = IstrFileNamePath;

            return;
        }

        protected void btnSaveGroup_Click(object sender, EventArgs e)
        {
            if (txtGroupName.Text.Trim() == "")
            {
                this.ShowMessage("请输入客户群名字");
                return;
            }
            //email 没有重复?
            ArrayList arrName = new ArrayList();
            ArrayList arrEmail = new ArrayList();
            foreach (ListItem item in Listmail.Items)
            {
                arrName.Add(item.Text);
                arrEmail.Add(item.Value);
            }
            if (svr.SaveEmailGroup(arrName, arrEmail, txtGroupName.Text, base.LoginUserID) == false)
                this.ShowSaveFail("客户群名字重复");
            else
                ddlEmailGroup.BindDropDownListAndSelect(svr.GetEmailGroup(base.LoginUserID), "GroupName", "GroupName");
        }

        protected void ddlEmailGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmailGroup.SelectedValue != "")
            {
                Listmail.DataSource = svr.GetEmailGroup(ddlEmailGroup.SelectedValue, base.LoginUserID);
                Listmail.DataTextField = "PersonName";
                Listmail.DataValueField = "Email";
                Listmail.DataBind();
            }
            else
            {
                Listmail.Items.Clear();
            }
        }

        protected void radEmailGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radEmailGroup.SelectedIndex == 0)
            {
                
                GetDataFromSearch();
                if(Listmail.Items.Count>0)
                    PanelOthers.Visible = true;
            }
            else
            {
                PanelOthers.Visible = false;
                if (ddlEmailGroup.SelectedValue != "")
                    ddlEmailGroup_SelectedIndexChanged(null, null);
                else
                    Listmail.Items.Clear();
            }
        }
    }
}