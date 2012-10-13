
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

namespace Terry.CRM.Web.CRM
{
    public partial class frmRole : BasePage
    {
        private const string EditURL = "frmRoleEdit.aspx";
        private BaseService svr = new BaseService();
        private void BindData()
        {
            //add search criteria

            string Filter = string.Empty;
            if (base.LoginUserRoleGrade != (int)enumRoleGrade.Boss)
                Filter = " RoleGrade< " + base.LoginUserRoleGrade;
            else
                Filter = "";

            if (ViewState["keyword"] != null)
            {
                if (!string.IsNullOrEmpty((String)ViewState["keyword"]))
                {
                    switch (ddlSearch.SelectedValue)
                    {
                        case "RoleID":
                            Filter = "and RoleID=" + ViewState["keyword"] + "";
                            break;
                        default:
                            Filter = "and " +ddlSearch.SelectedValue + " like '%" + ViewState["keyword"] + "%'";
                            break;
                    }

                }

            }
            string OrderBy = gvData.OrderBy;
            if (OrderBy == "")
                OrderBy = "RoleID";
            //一页列出来
            GridViewPageSize = 50;
            DataTable ilist = svr.SearchByCriteria(typeof(CRMRole), gvData.PageIndex, GridViewPageSize, out recordCount, Filter, OrderBy);


            gvData.DataSource = ilist;
            gvData.PageSize = GridViewPageSize;
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
                Authentication(enumModule.Role);
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
                Button btnDel = (Button)e.Row.FindControl("lnkDel");
                btnDel.Attributes.Add("onclick", "onDel('" + btnDel.UniqueID + "');return false;");
                string sId = gvData.DataKeys[e.Row.RowIndex].Value.ToString();
                e.Row.Attributes.Add("onclick", "onEdit('" + sId + "','" + EditURL + "'); return false;");
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

    }
}
