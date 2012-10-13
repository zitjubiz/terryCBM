
using System;
using System.Collections;
using System.Collections.Generic;
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
using System.IO;
using NPOI.HSSF.UserModel;

namespace Terry.CRM.Web.CRM
{
    public partial class frmCustomerBrief : BasePage
    {
        private const string EditURL = "";
        private BaseService svr = new BaseService();
        private void BindData()
        {
            //add search criteria
            string Filter = string.Empty;
            if (ViewState["keyword"] != null)
            {
                if (!string.IsNullOrEmpty((String)ViewState["keyword"]))
                {
                    switch (ddlSearch.SelectedValue)
                    {
                        case "CustID":
                            Filter = " and CustID=" + ViewState["keyword"] + "";
                            break;
                        default:
                            Filter = " and " + ddlSearch.SelectedValue + " like '%" + ViewState["keyword"] + "%'";
                            break;
                    }

                }

            }
            string OrderBy = gvData.OrderBy;
            if (OrderBy == "")
                OrderBy = "CustID";
            //SQL ��ѯ
            DataTable ilist = svr.SearchByCriteria(typeof(vw_CRMCustomer), gvData.PageIndex, base.GridViewPageSize, out recordCount, Filter, OrderBy);


            gvData.DataSource = ilist;
            gvData.PageSize = base.GridViewPageSize;
            gvData.VirtualItemCount = recordCount;
            gvData.DataBind();
        }

        private void DeleteRow(string Id)
        {

        }

        #region Common Code

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Authentication(enumModule.CustomerBrief);
                BindData();
            }
        }


        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
            BindData();
        }
        //Row Data Bound
        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                // for the Footer, display the running totals
                e.Row.Cells[0].ColumnSpan = e.Row.Cells.Count;
                e.Row.Cells[0].Text = GetREMes("lblTotalRecords") + "  " + recordCount.ToString();
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Visible = false;
                }
            }

        }
        //Delete a Row
        protected void gvData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                DeleteRow(gvData.DataKeys[e.RowIndex].Value.ToString());

            }
            catch (Exception)
            {
                ShowMessage(GetREMes("MsgCannotDelete"));
            }
            BindData();
        }
        //Sorting
        protected void gvData_Sorting(object sender, GridViewSortEventArgs e)
        {
            BindData();
        }

        //Click New Button
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditURL + "?id=0");
        }
        //Click Search Button
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["keyword"] = txtKeyword.Text.Trim();
                BindData();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }


        #endregion

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            ExportExcel();
        }

        private void ExportExcel()
        {
            DataTable dt = svr.SearchByCriteria("vw_CRMCustomer", "CustName,CustFullName,CustType,CommissionFactor", "", "");

            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "CRM\\Excel\\Product_Template.xls", FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
            HSSFSheet sheet1 = hssfworkbook.GetSheet("�ͻ���Ϣ");
            //row,cell���Ǵ�0��ʼ����
            //��1��title,��������  
            HSSFCellStyle cellStyle = hssfworkbook.CreateCellStyle();
            //- ϸ��Ե
            cellStyle.BorderBottom = HSSFCellStyle.BORDER_THIN;
            cellStyle.BorderLeft = HSSFCellStyle.BORDER_THIN;
            cellStyle.BorderRight = HSSFCellStyle.BORDER_THIN;
            cellStyle.BorderTop = HSSFCellStyle.BORDER_THIN;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    HSSFCell cell = sheet1.CreateRow(i + 1).CreateCell(j);
                    cell.CellStyle = cellStyle;
                    cell.SetCellValue(dr[j].ToString());
                }

            }
            //Excel�ļ��ڱ��򿪵�ʱ���Զ������㶨λ�ڵ�Ԫ��
            sheet1.GetRow(0).GetCell(0).SetAsActiveCell();

            //Force excel to recalculate all the formula while open
            sheet1.ForceFormulaRecalculation = true;
            hssfworkbook.ActiveSheetIndex = 0;
            string FullFileName = AppDomain.CurrentDomain.BaseDirectory + "Upload\\Excel\\Customer_" +
                DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + ".xls";
            file = new FileStream(FullFileName, FileMode.Create);
            hssfworkbook.Write(file);
            
            file.Close();
            DownloadFileAsAttachment(FullFileName);

        }
    }
}
