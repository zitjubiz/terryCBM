
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
    public partial class frmCustomer : BasePage
    {
        private const string EditURL = "frmCustomerEdit.aspx";
        private CustomerService svr = new CustomerService();

        /// <summary>
        /// salesֻ�ܿ����Լ������Ŀͻ�����Ʒ������Կ�����ʹ����������Ʒ�Ŀͻ����ϰ���Կ�������
        /// ����CRMRoleProd�ж�Ȩ��
        /// </summary>
        private void BindData()
        {

            //add search criteria
            string Filter = string.Empty;

            //Chemical: salesֻ�ܿ����Լ������Ŀͻ�����Ʒ������Կ�����ʹ����������Ʒ�Ŀͻ����ϰ���Կ�������
            //Ticket: sales�ܿ������еĿͻ�
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
                    Filter = "and CustId in (select CustId from vw_CRMRoleDepCustomer where RoleId=" + base.LoginUserRoleID.ToString() + ")";
                    break;
                case (int)enumRoleGrade.Boss: //Boss
                    Filter = "and 1=1";
                    break;
                default:
                    Filter = "and 1=0";
                    break;
            }
            //����������
            if (!string.IsNullOrEmpty(ddlRegion.SelectedValue))
                Filter += " and Region=N'" + ddlRegion.SelectedValue + "'";

            //��ΥԼ�����ѯ
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
            //��������Ա����
            if (ddlOwner.SelectedValue != "")
                Filter += " and CustOwnerID=" + ddlOwner.SelectedValue;
            //���ͻ����Ͳ���
            if (ddlCustType.SelectedValue != "")
                Filter += " and CustType=N'" + ddlCustType.SelectedValue + "'";
            //�ϴγɽ�����DATEDIFF(day, ISNULL(MAX(a.ACTModifyDate), '2000-1-1'), 

            if (ddlLastDeal.SelectedValue != "")
                Filter += " and DATEDIFF(day, ISNULL(LatestDealDate, '2000-1-1'),getdate())>=" + ddlLastDeal.SelectedValue;
            //���ͻ��������
            if (ddlGrade.SelectedValue != "")
                Filter += " and CustRelation= N'" + ddlGrade.SelectedValue + "'";

            //���ͻ��漰�Ĳ�Ʒ�����������
            if (ddlCategory.SelectedValue != "")
                Filter += " and CustID in (select custID from vw_CRMCustomerProd where CatID=" + ddlCategory.SelectedValue + ")";

            //�����Ʋ�ѯ�ͻ�
            if (ViewState["keyword"] != null)
            {
                if (!string.IsNullOrEmpty((String)ViewState["keyword"]))
                {
                    Filter += " and (CustCode like '%" + ViewState["keyword"] +
                        "%' or CustName like N'%" + ViewState["keyword"] + 
                        "%' or CustFullName like N'%" + ViewState["keyword"] + "%') ";
                }
            }
            //����Ʒʹ�������ѯ�ͻ�
            if (ddlProduct.SelectedValue != "")
            {
                Filter += @" and CustID in (select custID from vw_CRMCustomerProd where Code like '" + ddlProduct.SelectedValue + "%')";
            }

            //���Ա����
            if (ddlGender.SelectedValue != "")
                Filter += " and Gender='" + ddlGender.SelectedValue + "'";
            //���绰����
            if (txtTel.Text != "")
                Filter += " and CustTel like '%" + txtTel.Text + "%'";
            //��email����
            if (txtEmail.Text != "")
                Filter += " and CustEmail like '%" + txtEmail.Text + "%'";
            //�����պŲ���
            if (txtPassport.Text != "")
                Filter += " and Passport=N'" + txtPassport.Text + "'";
            //��������˾����
            if (txtParentCompany.Text != "")
                Filter += " and ParentCompany like N'%" + txtParentCompany.Text + "%'";
            //���Ƿ��Լ�Ǯ����
            if (ddlUseOwnMoney.SelectedValue != "")
                Filter += " and UseOwnMoney=" + ddlUseOwnMoney.SelectedValue + "";
            //�� FavoriteProd����
            if (ddlFavoriteProd.SelectedValue != "")
                Filter += " and FavoriteProd =N'" + ddlFavoriteProd.SelectedValue + "'";
            //��PreferPrice����
            if (ddlPreferPrice.SelectedValue != "")
                Filter += " and PreferPrice ='" + ddlPreferPrice.SelectedValue + "'";
            //��PreferPlace����
            if (ddlPreferPlace.SelectedValue != "")
                Filter += " and PreferPlace =N'" + ddlPreferPlace.SelectedValue + "'";
            //��ddlTravelDay����
            if (ddlTravelDay.SelectedValue != "")
                Filter += " and TravelDay like N'%" + ddlTravelDay.SelectedValue + "%'";
            
            Session["EmailFilter"] = Filter;

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

            return;



        }



        #region Common Code

        protected void Page_Load(object sender, EventArgs e)
        {
            Authentication(enumModule.Customer);
            if (!Page.IsPostBack)
            {
                ddlProduct.BindDropDownListAndSelect(svr.GetProductDDL(), "Proddl", "Code");
                ddlOwner.BindDropDownListAndSelect(svr.GetUserByDep(), "UserName", "UserID");
                ddlGrade.BindDropDownListAndSelect(svr.GetCustRelation(), "Relation", "Relation");
                ddlCustType.BindDropDownListAndSelect(svr.GetCustType(), "CustType", "CustType");
                ddlRegion.BindDropDownListAndSelect(svr.GetRegion(), "Region", "Region");                

                BindData();
            }
            if (ConfigurationManager.AppSettings["Industry"].ToLower() == "chemical")
            {
                divChemical.Visible = true;
                divTicket.Visible = false;
                gvData.Columns[6].Visible = false;
                gvData.Columns[7].Visible = false;
                gvData.Columns[8].Visible = false;
                gvData.Columns[9].Visible = false;
                gvData.Columns[10].Visible = false;
            }
            else if (ConfigurationManager.AppSettings["Industry"].ToLower() == "ticket")
            {
                divChemical.Visible = false;
                divTicket.Visible = true;
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
                if (ConfigurationManager.AppSettings["Industry"].ToLower() == "chemical")
                {
                    e.Row.FindControl("lnkTicket").Visible = false;
                    e.Row.FindControl("lnkVisum").Visible = false;
                }
                else
                {
                    e.Row.FindControl("lnkTel").Visible = false;
                    e.Row.FindControl("lnkVisit").Visible = false;

                    e.Row.FindControl("lnkBid").Visible = false;
                    e.Row.FindControl("lnkDeal").Visible = false;
                }
                //Button btnDel = (Button)e.Row.FindControl("lnkDel");
                //btnDel.Attributes.Add("onclick", "onDel('" + btnDel.UniqueID + "');return false;");
                //�Ƿ��Լ�Ǯ true/false-> ��/��
                if (e.Row.Cells[6].Text == "True")
                    e.Row.Cells[6].Text = "��";
                else if (e.Row.Cells[6].Text == "False")
                    e.Row.Cells[6].Text = "����";
                else
                    e.Row.Cells[6].Text = "";
                System.Web.UI.WebControls.Image imgIdle = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgIdle");

                if (e.Row.DataItem.GetType() == typeof(vw_CRMCustomer2))
                {
                    vw_CRMCustomer2 cust = (vw_CRMCustomer2)e.Row.DataItem;

                    //���ݳɽ�ʱ�䲻ͬ,��ʾ��ͬ����ɫ,��,��,��,��
                    if (cust.LatestDealDate == null)
                        e.Row.BackColor = ColorTranslator.FromHtml("#F1F3F5");
                    else
                    {
                        TimeSpan ts = DateTime.Now - (DateTime)cust.LatestDealDate;
                        if (ts.Days <= 30)
                            e.Row.BackColor = Color.LightGreen;
                        else if (ts.Days > 30 && ts.Days <= 90)
                            e.Row.BackColor = Color.Yellow;
                        else
                            e.Row.BackColor = Color.Red;
                    }

                    var IdleDays = cust.IdleDays;
                    //�����ж���¼���15��û��ϵ������...
                    if (IdleDays > 15)
                        imgIdle.ImageUrl = "../images/report.gif";
                    else
                        imgIdle.ImageUrl = "../images/setup.png";
                }
                string sId = gvData.DataKeys[e.Row.RowIndex].Value.ToString();

                //���һ���ǰ�ť,�õ�Ԫ��Ҫonclick
                for (int i = 0; i < e.Row.Cells.Count - 2; i++)
                {
                    e.Row.Cells[i].Attributes.Add("onclick", "onEdit('" + sId + "','" + EditURL + "'); return false;");
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
        //Click email Button
        protected void btnEmail_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmSendEmail.aspx?from=search");
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ddlCancel.SelectedIndex = -1;
            ddlCategory.SelectedIndex = -1;
            ddlCustType.SelectedIndex = -1;
            ddlFavoriteProd.SelectedIndex = -1;
            ddlGender.SelectedIndex = -1;
            ddlGrade.SelectedIndex = -1;
            ddlOwner.SelectedIndex = -1;
            ddlPreferPlace.SelectedIndex = -1;
            ddlPreferPrice.SelectedIndex = -1;
            ddlProduct.SelectedIndex = -1;
            ddlRegion.SelectedIndex = -1;
            ddlTravelDay.SelectedIndex = -1;
            ddlUseOwnMoney.SelectedIndex = -1;
            txtKeyword.Text = "";
            txtParentCompany.Text = "";
            txtPassport.Text = "";
            txtTel.Text = "";
            BindData();

            //foreach (Control ctl in Page.FindControl("ctl00").FindControl("CPH1").Controls)
            //{
            //    if (ctl.GetType() == typeof(DropDownList))
            //        ((DropDownList)ctl).SelectedIndex = -1;
            //    if (ctl.GetType() == typeof(TextBox))
            //        ((TextBox)ctl).Text = "";
            //}
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
