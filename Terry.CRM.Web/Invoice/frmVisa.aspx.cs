using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Terry.CRM;
using Terry.CRM.Entity;
using Terry.CRM.Service;
using System.Drawing;
using Terry.CRM.Web.CommonUtil;

namespace Terry.CRM.Web.Invoice
{
    public partial class frmVisa : BasePage
    {
        private const string EditURL = "frmVisaEdit.aspx";
        private BillingService svr = new BillingService();

        private void BindData()
        {

            //add search criteria
            string Filter = string.Empty;

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
            //按email查找
            if (txtEmail.Text != "")
                Filter += " and CustEmail like '%" + txtEmail.Text + "%'";
            //按email查找
            if (txtInnerReferenceID.Text != "")
                Filter += " and InnerReferenceID like '%" + txtInnerReferenceID.Text + "%'";

            string TableName = "BillVisa ";
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
                var drv = e.Row.DataItem as DataRowView;
                
                //未收款的红色(paydate=null)
                if (drv["PayDate"] == null || string.IsNullOrEmpty(drv["PayDate"].ToString()))
                {
                    e.Row.BackColor = Color.Red;
                    e.Row.ForeColor = Color.White;
                }

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
            if (ViewState["OrderBy"] == null || ViewState["OrderBy"].ToString() != e.SortExpression)
                ViewState["OrderBy"] = e.SortExpression;
            else
                ViewState["OrderBy"] = e.SortExpression + " desc";
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

    }
}