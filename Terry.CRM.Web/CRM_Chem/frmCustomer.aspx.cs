 
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
using System.Drawing;

namespace Terry.CRM.Web.CRM
{		
    public partial class frmCustomer_Chemical : BasePage
    {
        private const string EditURL = "frmCustomerEdit.aspx";
        private CustomerService svr = new CustomerService();

        /// <summary>
        /// sales只能看到自己所属的客户，产品经理可以看到有使用其所属产品的客户，老板可以看到所有
        /// 根据CRMRoleProd判断权限
        /// </summary>
        private void BindData()
        {
            //add search criteria
            string Filter = string.Empty;
            string RegionProvinces = string.Empty;  //地区包括的省份

            //var p = svr.GetRoleRelativeProducts(base.LoginUserRoleID);
            //long[] custList=null;
            //long[] custListByProd = null;
            switch (base.LoginUserRoleGrade)
            {
                case (int)enumRoleGrade.Sales://sales 能看到自己所属的客户，以及无限级下属的客户 TODO！！！！
                case (int)enumRoleGrade.SalesManager:
                    Filter = " and CustOwnerID in ( select userid from dbo.GetSubordinateUser("+ base.LoginUserID +"))";

                    break;
                case (int)enumRoleGrade.ProdManager: //Prod Manager can see its prods and it's customer as sales
                    Filter = "and CustId in (select CustId from vw_CRMRoleCustomer where RoleId=" + base.LoginUserRoleID.ToString() + ")";
                    //custList = svr.GetRoleRelativeCustomer(base.LoginUserRoleID);
                    break;
                case (int)enumRoleGrade.Boss: //Boss
                    Filter = " and 1=1";
                    break;
                default:
                    Filter = "and 1=0";
                    break;
            }
            //按违约情况查询
            switch (ddlCancel.SelectedValue)
            {
                case ""://all
                    break;
                case "0": //normal
                    Filter += " and CancelCnt=0";
                    break;
                case "1":
                    Filter += " and CancelCnt>0";
                    break;
            }
            //按销售人员查找
            if(ddlOwner.SelectedValue!="")
                Filter += " and CustOwnerID=" + ddlOwner.SelectedValue ;

            //按客户级别查找
            //if (ddlGrade.SelectedValue != "")
            //    Filter += " and CustRelation='" + ddlGrade.SelectedValue +"'";
            //按客户类型查找
            if (ddlCustType.SelectedValue != "")
                Filter += " and CustType=N'" + ddlCustType.SelectedValue + "'";
            //按地区查找
            if (string.IsNullOrEmpty(ddlRegion.SelectedValue) == false)
                Filter += " and Region=N'" + ddlRegion.SelectedItem.Text.Trim() + "'";
            //按省份查找
            if (string.IsNullOrEmpty(ddlProvince.SelectedValue) == false)
                Filter += " and CustProvince=N'" + ddlProvince.SelectedItem.Text.Trim() + "'";

            //按名称查询客户
            if (ViewState["keyword"] != null)
            {
                if (!string.IsNullOrEmpty((String)ViewState["keyword"]))
                {
                    Filter += " and (CustCode like '%" + ViewState["keyword"] +
                       "%' or CustName like N'%" + ViewState["keyword"] +
                       "%' or CustFullName like N'%" + ViewState["keyword"] + "%') ";
                }
            }
            //按客户涉及的产品的类别来查找
            if (ddlCategory.SelectedValue != "")
                Filter += " and CustID in (select custID from vw_CRMCustomerProd where CatID=" + ddlCategory.SelectedValue + ")";
            //按产品使用情况查询客户
            if (ddlProduct.SelectedValue != "")
            {
                Filter += @" and CustID in (select custID from vw_CRMCustomerProd where Code like '" + ddlProduct.SelectedValue + "%')";
            }
            ////按产品使用情况查询客户
            //if (ddlProduct.SelectedValue != "")
            //{
            //    custListByProd = svr.GetProdRelativeCustomer(long.Parse(ddlProduct.SelectedValue));
            //}

            //IList<vw_CRMCustomer2> ilist = svr.SearchByCriteria(gvData.PageIndex, base.GridViewPageSize,
            //    out recordCount, Filter, gvData.OrderBy, custList, custListByProd, base.LoginUserID);
            
            string TableName = "vw_CRMCustomer2 ";
            string OrderBy;

            if (ViewState["OrderBy"] == null)
                OrderBy = "CustName";
            else
                OrderBy = ViewState["OrderBy"].ToString();

            gvData.DataSource = svr.SearchByCriteria(TableName, gvData.PageIndex, base.GridViewPageSize,
                               out recordCount, Filter, OrderBy);
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
            Authentication(enumModule.Customer);
            if (!Page.IsPostBack)
            {
                ddlProduct.BindDropDownListAndSelect(svr.GetProductDDL(), "Proddl", "Code");

                //用Terry.WebControl.Dropdownlist时要用svr.GetUserByDep()把OptGroup查出来,必须绑定UserID
                ddlOwner.BindDropDownListAndSelect(svr.GetUserByDep(), "UserName", "UserID");

                //ddlGrade.BindDropDownListAndSelect(svr.GetCustRelation(), "Relation", "Relation");
                ddlProvince.BindDropDownListAndSelect(svr.GetProvince(), "Province", "Province");
                ddlCustType.BindDropDownListAndSelect(svr.GetCustType(), "CustType", "CustType");
                ddlRegion.BindDropDownListAndSelect(svr.GetRegion(), "Region", "Region");
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
                e.Row.Cells[0].Text = GetREMes("lblTotalRecords")+ "  " + recordCount.ToString();
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.Image imgIdle = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgIdle");
                DataRowView drv =e.Row.DataItem as DataRowView;
                //根据成交时间不同,显示不同的颜色,绿,黄,红,灰
                if (String.IsNullOrEmpty(drv["LatestDealDate"].ToString()))
                    e.Row.BackColor = ColorTranslator.FromHtml("#F1F3F5");
                else
                {
                    TimeSpan ts = DateTime.Now - DateTime.Parse(drv["LatestDealDate"].ToString());
                    if (ts.Days <= 30)
                        e.Row.BackColor = Color.LightGreen;
                    else if (ts.Days > 30 && ts.Days <= 90)
                        e.Row.BackColor = Color.Yellow;
                    else
                        e.Row.BackColor = Color.Red;
                }

                var IdleDays = int.Parse(drv["IdleDays"].ToString());
                //现在行动记录变成15天没联系才提醒...
                if (IdleDays > 15)
                    imgIdle.ImageUrl = "../images/report.gif";
                else
                    imgIdle.ImageUrl = "../images/setup.png";

                //最后一列是按钮,该单元格不要onclick
                string sId = gvData.DataKeys[e.Row.RowIndex].Value.ToString();
                for (int i = 0; i < e.Row.Cells.Count - 2; i++)
                {
                    e.Row.Cells[i].Attributes.Add("onclick", "onEdit('" + sId + "','" + EditURL + "',true); return false;");
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
            Response.Redirect(EditURL+"?id=0");
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

    }
}
		