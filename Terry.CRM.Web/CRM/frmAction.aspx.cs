
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
    public partial class frmAction : BasePage
    {
        private const string EditURL = "frmActionEdit.aspx";
        private ActionService svr = new ActionService();
        private int iActType = 0;
        private int iCustID = 0;
        private void BindData()
        {

            if (string.IsNullOrEmpty(Request["AcType"]) || string.IsNullOrEmpty(Request["CustID"]))
            {
                btnNew.Enabled = false;
                return;

            }

            //add search criteria
            string Filter = string.Empty;
            if (ViewState["keyword"] != null)
            {
                if (!string.IsNullOrEmpty((String)ViewState["keyword"]))
                {
                    switch (ddlSearch.SelectedValue)
                    {
                        case "ACTID":
                            Filter = "ACTID=" + ViewState["keyword"] + "";
                            break;
                        default:
                            Filter = ddlSearch.SelectedValue + "=\"" + ViewState["keyword"] + "\"";
                            break;
                    }

                }

            }
            //只显示该用户的该类型的拜访记录
            Filter = "ACTType=" + Request["AcType"] + " && ACTCustID=" + Request["CustID"];
            var entity = (vw_CRMCustomer)svr.LoadById(typeof(vw_CRMCustomer), "CustID", Request["CustID"]);
            if (Request["AcType"] == "1")
                lblActionType.Text = entity.CustName+ " "+ GetREMes("lblActionTel");
            else if (Request["AcType"] == "2")
                lblActionType.Text = entity.CustName + " " + GetREMes("lblActionVisit");
            else
                lblActionType.Text = entity.CustName + " " + GetREMes("lblActionBid");

            IList<vw_CRMAction> ilist = svr.SearchByCriteria(gvData.PageIndex, base.GridViewPageSize, out recordCount,
                iActType, iCustID, gvData.OrderBy);

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
            int.TryParse(Request["AcType"], out iActType);
            int.TryParse(Request["CustID"], out iCustID);

            if (!Page.IsPostBack)
            {
                Authentication(enumModule.Customer);
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
                string Url = EditURL + "?Id=" + sId + "&ACType=" + Request["ACType"] + "&CustID=" + Request["CustID"];
                e.Row.Attributes.Add("onclick", "onEdit2('" + Url + "'); return false;");
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
            Response.Redirect(EditURL + "?id=0&ACType=" + Request["ACType"] + "&CustID=" + Request["CustID"]);
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
