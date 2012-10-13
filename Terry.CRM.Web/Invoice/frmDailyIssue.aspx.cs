using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Terry.CRM.Entity;
using Terry.CRM.Service;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using Terry.CRM.Web.CommonUtil;

namespace Terry.CRM.Web.Invoice
{
    public partial class frmDailyIssue : BasePage
    {
        BillingService svr = new BillingService();

        #region Old

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Authentication(enumModule.BankStatement);
            }
            //if (!IsPostBack)
            //    AddEmptyIssues();
        }
        private void AddEmptyIssues()
        {
            IList<BillDailyIssue> IssueList = new List<BillDailyIssue>();
            if (IssueList.Count == 0)
                for (int i = 0; i < 15; i++)
                {
                    IssueList.Add(new BillDailyIssue());
                }

            gvIssue.DataSource = IssueList;
            gvIssue.DataBind();
        }

        private IList<BillDailyIssue> BindIssue(bool AddNew)
        {
            List<BillDailyIssue> lis = new List<BillDailyIssue>();

            BillDailyIssue Issue;
            TextBox txtFlightTicketNum, txtOuterReferenceID, txtOwnerName, txtCost, txtInnerReferenceID,txtBankStatement;
            //保存现有的gridview 数据
            for (int i = 0; i < gvIssue.Rows.Count; i++)
            {
                if (gvIssue.Rows[i].Visible == false) continue;

                txtFlightTicketNum = (TextBox)gvIssue.Rows[i].FindControl("txtFlightTicketNum");
                txtOuterReferenceID = (TextBox)gvIssue.Rows[i].FindControl("txtOuterReferenceID");
                txtInnerReferenceID = (TextBox)gvIssue.Rows[i].FindControl("txtInnerReferenceID");
                txtBankStatement = (TextBox)gvIssue.Rows[i].FindControl("txtBankStatement");
                txtOwnerName = (TextBox)gvIssue.Rows[i].FindControl("txtOwnerName");
                txtCost = (TextBox)gvIssue.Rows[i].FindControl("txtCost");

                if (txtFlightTicketNum.Text.Trim() != "")
                {
                    Issue = new BillDailyIssue();
                    Issue.IssueDate = DateTime.Parse(txtIssueDate.Text);
                    Issue.OwnerName = txtOwnerName.Text.Trim();
                    Issue.FlightTicketNum = txtFlightTicketNum.Text.Trim();
                    Issue.OuterReferenceID = txtOuterReferenceID.Text.Trim();
                    if (txtCost.Text != "")
                        Issue.Cost = decimal.Parse(txtCost.Text.Trim());
                    Issue.InnerReferenceID = txtInnerReferenceID.Text.Trim();
                    Issue.BankStatement = txtBankStatement.Text.Trim();
                    lis.Add(Issue);
                }
            }
            //新增一行
            if (AddNew)
            {
                Issue = new BillDailyIssue();
                Issue.IssueDate = DateTime.Parse(txtIssueDate.Text);
                Issue.OwnerName = "";
                lis.Add(Issue);
            }
            return lis;
        }

        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            try
            {
                Save();
                this.ShowSaveOK();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
                log4netHelper.Error("", ex);
            }

        }

        private void Save()
        {
            svr.SaveIssue(BindIssue(false));
        }

        #endregion

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.PostedFile != null)
            {
                string filename = FileUpload1.FileName;
                string fileExt = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
                if (fileExt != "xls")
                {
                    ClientScript.RegisterClientScriptBlock(typeof(string), "xls", "<script>alert('Please select Excel file!');</script>");
                    return;
                }
                filename = Server.MapPath("~/Upload/Excel/") + "Daily" + Session.SessionID.Substring(0, 2) + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                FileUpload1.SaveAs(filename);
                btnUpload.Enabled = false;
                ExtractExcelData(filename);
                btnUpload.Enabled = true;
                //取完数据之后删除
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                this.ShowMessage("导入数据成功!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        private void ExtractExcelData(string filename)
        {
            List<BillDailyIssue> lis = new List<BillDailyIssue>();
            BillDailyIssue Deal;
            FileStream file = new FileStream(filename, FileMode.Open);
            HSSFWorkbook wb = new HSSFWorkbook(file);
            HSSFSheet sht;
            sht = wb.GetSheetAt(0); //取第一个sheet
            //取行Excel的最大行数          
            int rowsCount = sht.PhysicalNumberOfRows;

            DateTime IssueDate = sht.GetRow(1).GetCell(0).DateCellValue;//第2行第1列是出票日期
            //第1行是header,不是数据,第3行开始
            for (int i = 2; i < rowsCount; i++)
            {
                //如果内部订单号是空,跳过
                if (sht.GetStringCellValue(i, "A") == "")
                    continue;

                Deal = new BillDailyIssue();
                Deal.FlightTicketNum = sht.GetStringCellValue(i, "A");
                //外部amadeus订单号
                Deal.OuterReferenceID = sht.GetStringCellValue(i, "B");                
                Deal.Cost = (decimal)sht.GetDoubleCellValue(i, "C");
                Deal.OwnerName = sht.GetStringCellValue(i, "D");
                Deal.InnerReferenceID = sht.GetStringCellValue(i, "E");
                Deal.BankStatement = sht.GetStringCellValue(i, "F");
                Deal.IssueDate = IssueDate;
                lis.Add(Deal);
            }
            file.Close();

            svr.SaveIssue(lis);

        }

    }
}