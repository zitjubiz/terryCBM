using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Terry.CRM;
using Terry.CRM.Entity;
using Terry.CRM.Service;
using Terry.CRM.Web.CommonUtil;

namespace Terry.CRM.Web.CRM
{
    public partial class frmVerUpdate : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Authentication(enumModule.BaseInfo);
            }
        }

        public void ShowMessage(string message)
        {

            //message = FixedJsString(message);
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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //上传的程序段
            //这是文件将上传到的服务器的绝对目录
            string strBaseLocation = Server.MapPath("../Upload/CRMUpdate/");
            string UnzipPath = strBaseLocation+"\\"+ DateTime.Now.ToString("yyyyMMdd");
            if (FileUpload1.PostedFile.ContentLength != 0) //判断选取对话框选取的文件长度是否为0
            {
                string NewName = UnzipPath + FileUpload1.PostedFile.FileName;
                FileUpload1.PostedFile.SaveAs(NewName);
                //执行上传,并自动根据日期为文件命名,确保不重复
                ShowMessage("上传成功");
                Zipper.UnZip(NewName, UnzipPath);
                //move dir
                
            }
        }

        protected void btnChangeCustCode_Click(object sender, EventArgs e)
        { 
            CustomerService svr = new CustomerService();
            DataTable dt =svr.GetCustomer();
            string custCode ="";
            for (int i = 0; i < dt.Rows.Count; i++)
			{
			    custCode =PinYin.GetChineseSpell(dt.Rows[i]["CustName"].ToString());
                svr.dataCtx.ExecuteCommand("update crmcustomer set custcode='" + custCode + "' where custID=" + dt.Rows[i]["CustID"]);
			}
            
        }
    
    }
}
