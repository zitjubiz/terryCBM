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
using System.Diagnostics;

namespace Terry.CRM.Web.CRM
{
    public partial class frmUserActions : BasePage
    {
        private BaseService svr = new BaseService();
        private UserService usr = new UserService();
        private CustomerService cus = new CustomerService();

        /// <summary>
        /// 化工: ACTContent dbo.vw_CRMCustomerDeal.CustName + N'购买了' + dbo.vw_CRMCustomerDeal.Product + N',牌号为:' + isnull(dbo.vw_CRMCustomerDeal.Brand,'') + N',数量:' + CAST(dbo.vw_CRMCustomerDeal.Qty
        ///               AS varchar(20)) + CAST(dbo.vw_CRMCustomerDeal.Unit AS varchar(20)) AS actcontent
        ///Ticket:               
        /// </summary>
        private void BindData()
        {

            //只显示该用户的记录
            string Filter = string.Empty;
            string OrderBy = "MDate Desc";
            switch (base.LoginUserRoleGrade)
            {
                case (int)enumRoleGrade.Sales://sales 能看到自己创建的联系记录以及属于自己的客户之前的联系记录（联系人可能不是自己）,以及下属的联系记录
                case (int)enumRoleGrade.SalesManager: 
                Filter = " and (MUser='" + base.LoginUserName + 
                    "' or CustId in (select CustId from CRMCustomer where CustOwnerID in (select userid from dbo.GetSubordinateUser("+ base.LoginUserID +"))))";
                    break;
                case (int)enumRoleGrade.ProdManager: //市场部能看到自己角色有权限的区域和产品对应的客户的联系记录
                    Filter = " and ( CustId in (select CustId from vw_CRMRoleCustomer where RoleId=" + base.LoginUserRoleID.ToString() + ")";
                    Filter += " or (MUser='" + base.LoginUserName + 
                        "'  or joinPerson like '" + base.LoginUserName +"%'"
                        + " or joinPerson like '%," + base.LoginUserName +"%'))";
                    break;
                case (int)enumRoleGrade.DepManager: //部门经理能看到部门所属的客户信息,CPG没这个职位，FITT才有
                    Filter = " and CustId in (select CustId from vw_CRMRoleDepCustomer where RoleId=" + base.LoginUserRoleID.ToString() + ")";
                    break;
                case (int)enumRoleGrade.Boss:
                case (int)enumRoleGrade.HR: //Boss or HR
                    Filter = " and 1=1";
                    break;
                default:
                    Filter = "and 1=0";
                    break;
            }
            if (string.IsNullOrEmpty(Request["type"]) == false)
                Filter += " and type =N'" + Request["type"] + "'";
            if (string.IsNullOrEmpty(Request["custid"]) == false)
                Filter += " and CustId =" + Request["custid"] + "";
            if (string.IsNullOrEmpty(Request["cust"]) == false)
                Filter += " and CustName like '%" + Request["cust"] + "%'";
            if (string.IsNullOrEmpty(Request["user"]) == false)
                Filter += " and MUser =N'" + Request["user"] + "'";

            if (gvData.OrderBy != "")
                OrderBy = gvData.OrderBy;
            DataTable ilist = svr.SearchByCriteria(typeof(vw_CRMUserAction), gvData.PageIndex,
                base.GridViewPageSize, out recordCount, Filter, OrderBy);

            gvData.DataSource = ilist;
            gvData.PageSize = base.GridViewPageSize;
            gvData.VirtualItemCount = recordCount;
            gvData.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string url = "frmUserActions.aspx?type=" + Request["type"] + "&custid=" + Request["custid"];
            if (ddlOwner.SelectedValue != "")
                url += "&user=" + ddlOwner.SelectedItem.Text;
            if (txtKeyword.Text.Trim() != "")
                url += "&cust=" + txtKeyword.Text.Trim();
            Response.Redirect(url);
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string ACType ="1";
            if (!String.IsNullOrEmpty(Request["type"]))
            {
                if (Request["type"] == "电话")
                    ACType = "1";
                else if (Request["type"].Contains("拜访"))
                    ACType = "2";
                else if (Request["type"] == "报价")
                    ACType = "3";
                else if (Request["type"] == "成交")
                {
                    Response.Redirect("frmCustomerDeal.aspx?CustID=" + Request["custid"]);
                    return;
                }
            }

            Response.Redirect("frmActionEdit.aspx?id=0&ACType=" + ACType + "&CustID=" + Request["custid"]);
            
        }
        #region Common Code

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Request["type"] == "成交" && ConfigurationManager.AppSettings["Industry"].ToLower() == "ticket")
                    divImportDeal.Visible = true;
                else
                    divImportDeal.Visible = false;

                Authentication(enumModule.Customer);
                ddlOwner.BindDropDownListAndSelect(svr.GetUserByDep(), "UserName", "UserID");
                ddlOwner.SelectedByText(Request["user"]);
                txtKeyword.Text = Request["cust"];
                string UrlParm ="&cust=" + Request["Cust"] + "&user=" + Request["User"];

                if (!String.IsNullOrEmpty(Request["custid"]))
                    UrlParm += "&custid=" + Request["Custid"];

                if (!String.IsNullOrEmpty(Request["custid"]) && !String.IsNullOrEmpty(Request["type"]))
                {
                    btnAdd.Visible = true;                    
                    lblAddTip.Visible = false;  
                }
                else
                {
                    lblAddTip.Visible = true;  
                    btnAdd.Visible = false;
                }

                Tab1.Items[0].Value = "frmUserActions.aspx?type=" + UrlParm;
                Tab1.Items[1].Value = "frmUserActions.aspx?type=成交" + UrlParm;
                Tab1.Items[2].Value = "frmUserActions.aspx?type=电话" + UrlParm;
                Tab1.Items[3].Value = "frmUserActions.aspx?type=上门拜访" + UrlParm;
                Tab1.Items[4].Value = "frmUserActions.aspx?type=报价" + UrlParm;
                Tab1.Items[5].Value = "frmUserActions.aspx?type=评论" + UrlParm;

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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string Url = ((DataRowView)e.Row.DataItem)["url"].ToString()+"&returnUrl=frmUserActions.aspx";
                e.Row.Attributes.Add("onclick", "window.location='" + Url + "'; return false;");
            }
        }
        //Delete a Row
        protected void gvData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

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
                filename = Server.MapPath("~/Upload/Excel/") + "Deal" + Session.SessionID.Substring(0, 2) + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                FileUpload1.SaveAs(filename);
                btnUpload.Enabled = false;
                ExtractExcelData(filename);
                btnUpload.Enabled = true;
                //取完数据之后删除
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                this.ShowMessage("Import Succeed!");
            }
        }

        /// <summary>
        /// A 内部订单号
        /// B 客人姓名 title	
        /// C售价	
        /// D备注	
        /// E负责销售人员
        /// F机票号	
        /// G Amadeus预订号
        /// H 起飞日期
        /// </summary>
        /// <param name="filename"></param>
        private void ExtractExcelData(string filename)
        {
            CRMCustomerDeal Deal;
            FileStream file = new FileStream(filename, FileMode.Open);
            HSSFWorkbook wb = new HSSFWorkbook(file);
            HSSFSheet sht;
            sht = wb.GetSheetAt(0); //取第一个sheet
            //取行Excel的最大行数          
            int rowsCount = sht.PhysicalNumberOfRows;

            //第1行是header,不是数据
            for (int i = 1; i < rowsCount; i++)
            {
                //如果内部订单号是空,跳过
                if (sht.GetStringCellValue(i,"A") == "")
                    continue;

                Deal = new CRMCustomerDeal();
                Deal.ContractNum = sht.GetStringCellValue(i, "A");
                //如果单元格为空，GetCell方法会返回null,自己定义个扩展方法返回非空
                string UserName = sht.GetStringCellValue(i, "E");
                var objUser = usr.LoadByUserName(UserName);
                if (objUser != null)
                    Deal.DealOwner = objUser.UserID;
                //产品序列号,机票号
                Deal.ProdSerialNum = sht.GetStringCellValue(i, "F");
                //外部amadeus订单号
                Deal.ReferenceNum = sht.GetStringCellValue(i, "G");


                try
                {
                    Deal.DealDate = Convert.ToDateTime(sht.GetDateCellValue(i, "H"));

                }
                catch { }

                string CustName = sht.GetStringCellValue(i, "B");
                long CustID = cus.GetCustIDByName(CustName);
                if (CustID > 0)
                    Deal.CustID = CustID;
                else
                {
                    //新建一个客户
                    CRMCustomer entity = GetNewCustomer(CustName, Deal.DealOwner);
                    entity = cus.Save(entity, null);
                    Deal.CustID = entity.CustID;
                }
                Deal.ProdID = 1;//现暂固定为1=机票
                Deal.Qty = 1;
                Deal.Unit = "PCS";
                try
                {
                    Deal.UnitPrice = Convert.ToDecimal(sht.GetStringCellValue(i, "C"));
                    //Deal.UnitPrice = Convert.ToDecimal(sht.GetStringCellValue(i, "C"));
                }
                catch (Exception)
                {
                    Deal.UnitPrice = 0;
                }
                Deal.TotalAmount = Deal.UnitPrice;
                Deal.Currency = "EUR";
                Deal.Remark = sht.GetStringCellValue(i, "D");
                //现在CANX是取消，是放在客户名字里，但是否应该分开单列一列？
                if (CustName.Contains("(CANX)"))
                    Deal.Status = 1; //取消
                else
                    Deal.Status = 0; //正常

                Deal.CreateDate = DateTime.Now;
                Deal.CreateUserID = base.LoginUserID;
                Deal.ModifyDate = DateTime.Now;
                Deal.ModifyUserID = base.LoginUserID;


                try
                {
                    cus.SaveDeal(Deal);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + Deal.ContractNum + " ");
                    this.ShowMessage("Please check " + Deal.ContractNum + " " + ex.Message);
                }
            }
            file.Close();



        }
        //获取输入数据
        private CRMCustomer GetNewCustomer(string CustName, long? CustOwnerID)
        {
            var entity = new CRMCustomer();
            entity.CustName = CustName.Trim();
            entity.CustFullName = CustName.Trim();
            entity.CustTypeID = 3;
            entity.CustRelationID = 6;
            entity.CustIndustryID = 1;
            //entity.CustEmpNumID = 1;
            //entity.CustCountryID = 37; //china
            entity.CustCDate = DateTime.Now;
            entity.CustCUserID = base.LoginUserID;
            if(CustOwnerID==null)
                entity.CustOwnerID = 0;
            else
                entity.CustOwnerID = (long)CustOwnerID;
            entity.SYSID = 1;
            entity.CustModifyDate = DateTime.Now;
            entity.CustModifyUserID = base.LoginUserID;           
            entity.CommissionFactor = 1.0;
            entity.IsActive = true;
            entity.Status = "A";

            //Ticket Fields
            if (CustName.Contains(" MRS"))
                entity.Gender = "F";
            else
                entity.Gender = "M";

            return entity;
        }

        //根据成交的Amadeus预订号分析好友关系
        protected void btnGetBuddy_Click(object sender, EventArgs e)
        {
            cus.BuddyAnalysis();
            this.ShowMessage("分析完成,将根据成交记录自动更新好友关系.");
        }

    }
}
