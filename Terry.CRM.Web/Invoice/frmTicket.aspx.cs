using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Terry.CRM;
using Terry.CRM.Entity;
using Terry.CRM.Service;
using System.Data;
using System.Drawing;
using Terry.CRM.Web.CommonUtil;

namespace Terry.CRM.Web.Invoice
{
    public partial class frmTicket : BasePage
    {
        private const string EditURL = "frmTicketEdit.aspx";
        private BillingService svr = new BillingService();

        private void BindData()
        {

            //add search criteria
            string Filter = string.Empty;

            //Chemical: sales只能看到自己所属的客户，产品经理可以看到有使用其所属产品的客户，老板可以看到所有
            //Ticket: sales能看到所有的客户
            switch (base.LoginUserRoleGrade)
            {
                case (int)enumRoleGrade.Sales://sales
                    Filter = "and CustOwnerID=" + base.LoginUserID;
                    lblOwner.Visible = false;
                    ddlOwner.Visible = false;
                    break;
                case (int)enumRoleGrade.ProdManager: //Prod Manager can see its prods and it's customer as sales
                    Filter = "and CustId in (select CustId from vw_CRMRoleCustomer where RoleId=" + base.LoginUserRoleID.ToString() + ")";
                    break;
                case (int)enumRoleGrade.DepManager: //Dep Manager can see its department customers
                    Filter = "and depid in (select depid from CRMRoleDep where RoleId=" + base.LoginUserRoleID.ToString() + ")";
                    break;
                case (int)enumRoleGrade.Boss: //Boss
                    Filter = "and 1=1";
                    break;
                default:
                    Filter = "and 1=0";
                    break;
            }

            //按销售人员查找
            if (ddlOwner.SelectedValue != "")
                Filter += " and CustOwnerID=" + ddlOwner.SelectedValue;
            //按名称查询客户
            if (ViewState["keyword"] != null)
            {
                if (!string.IsNullOrEmpty((String)ViewState["keyword"]))
                {
                    Filter += " and CustName like '%" + ViewState["keyword"] + "%' ";
                }
            }

            //按电话查找
            if (txtTel.Text != "")
                Filter += " and CustTel like '%" + txtTel.Text + "%'";
            //按订单金额查找
            if (txtAmount.Text != "")
                Filter += " and TotalAmount=" + txtAmount.Text + "";
            //按email查找
            if (txtEmail.Text != "")
                Filter += " and CustEmail like '%" + txtEmail.Text + "%'";
            //按订单号查找
            //62-1205021   这份账单是 荷兰阿纳姆的，但是 出现在了德国 杜塞的分类里面，而且是置顶的
            if(ddlDept.Text!="")
                Filter += " and InnerReferenceID like '" + ddlDept.Text + "%'";
            if (txtInnerReferenceID.Text != "")
                Filter += " and InnerReferenceID like '%" + txtInnerReferenceID.Text + "%'";
            //else
            //{
            //    if (ddlDept.Text != "" && ddlMonth.Text != "")
            //        Filter += " and InnerReferenceID like '" + ddlDept.Text + "-" + DateTime.Now.Year.ToString().Substring(2) + ddlMonth.Text +"%'";
            //}

            string TableName = "BillTicket ";
            string OrderBy;

            if (ViewState["OrderBy"] == null)
                OrderBy = "InnerReferenceID desc";
            else
                OrderBy = ViewState["OrderBy"].ToString();

            gvData.DataSource = svr.SearchByCriteria(TableName, gvData.PageIndex, base.GridViewPageSize,
                                out recordCount, Filter, OrderBy);
            gvData.PageSize = base.GridViewPageSize;
            gvData.VirtualItemCount = recordCount;
            gvData.DataBind();

            return;



        }



        #region Common Code

        protected void Page_Load(object sender, EventArgs e)
        {
            Authentication(enumModule.Customer);
            if (!Page.IsPostBack)
            {
                ddlOwner.BindDropDownListAndSelect(svr.GetUserByDep(), "UserName", "UserID");
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
                //Button btnDel = (Button)e.Row.FindControl("lnkDel");
                //btnDel.Attributes.Add("onclick", "onDel('" + btnDel.UniqueID + "');return false;");
                var drv = e.Row.DataItem as DataRowView;
                
                //未收款的红色(any person's bankstatement is blank)
                string TicketID = gvData.DataKeys[e.Row.RowIndex].Value.ToString();

                if (svr.IsPaid(TicketID)==false)
                {
                    e.Row.BackColor = Color.FromArgb(0xFF,0x33,0x00);
                    e.Row.ForeColor = Color.White;
                }
                //退票的紫色
                if ((decimal)drv["TotalAmount"]<0)
                    e.Row.BackColor = Color.Purple;

                //取消的橙色
                if (drv["Status"].ToString() == "I")
                    e.Row.BackColor = Color.Orange;

                string sId = gvData.DataKeys[e.Row.RowIndex].Value.ToString();

                for (int i = 0; i < e.Row.Cells.Count - 1; i++)
                {
                    e.Row.Cells[i].Attributes.Add("onclick", "onEdit('" + sId + "','" + EditURL + "',true); return false;");
                }
            }
        }
        //Delete a Row
        private void DeleteRow(string Id)
        {

        }
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
            if (ViewState["OrderBy"] == null || ViewState["OrderBy"].ToString()!= e.SortExpression)
                ViewState["OrderBy"] = e.SortExpression ;
            else
                ViewState["OrderBy"] = e.SortExpression +" desc";
            BindData();
        }

        //Click New Button
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditURL + "?id=0");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

            ddlOwner.SelectedIndex = -1;
            txtKeyword.Text = "";
            txtInnerReferenceID.Text = "";
            txtEmail.Text = "";
            txtTel.Text = "";
            BindData();


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
                log4netHelper.Error("", ex);
            }
        }


        #endregion

        protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMonth.Text == "")
                txtInnerReferenceID.Text = ddlDept.Text + "-";
            else
                txtInnerReferenceID.Text = ddlDept.Text + "-" + DateTime.Now.Year.ToString().Substring(2) + ddlMonth.Text;
            BindData();
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlDept_SelectedIndexChanged(sender, e);
        }



    }
}