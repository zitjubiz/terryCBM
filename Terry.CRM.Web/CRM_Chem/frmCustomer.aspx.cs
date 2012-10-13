 
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
        /// salesֻ�ܿ����Լ������Ŀͻ�����Ʒ������Կ�����ʹ����������Ʒ�Ŀͻ����ϰ���Կ�������
        /// ����CRMRoleProd�ж�Ȩ��
        /// </summary>
        private void BindData()
        {
            //add search criteria
            string Filter = string.Empty;
            string RegionProvinces = string.Empty;  //����������ʡ��

            //var p = svr.GetRoleRelativeProducts(base.LoginUserRoleID);
            //long[] custList=null;
            //long[] custListByProd = null;
            switch (base.LoginUserRoleGrade)
            {
                case (int)enumRoleGrade.Sales://sales �ܿ����Լ������Ŀͻ����Լ����޼������Ŀͻ� TODO��������
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
            if(ddlOwner.SelectedValue!="")
                Filter += " and CustOwnerID=" + ddlOwner.SelectedValue ;

            //���ͻ��������
            //if (ddlGrade.SelectedValue != "")
            //    Filter += " and CustRelation='" + ddlGrade.SelectedValue +"'";
            //���ͻ����Ͳ���
            if (ddlCustType.SelectedValue != "")
                Filter += " and CustType=N'" + ddlCustType.SelectedValue + "'";
            //����������
            if (string.IsNullOrEmpty(ddlRegion.SelectedValue) == false)
                Filter += " and Region=N'" + ddlRegion.SelectedItem.Text.Trim() + "'";
            //��ʡ�ݲ���
            if (string.IsNullOrEmpty(ddlProvince.SelectedValue) == false)
                Filter += " and CustProvince=N'" + ddlProvince.SelectedItem.Text.Trim() + "'";

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
            //���ͻ��漰�Ĳ�Ʒ�����������
            if (ddlCategory.SelectedValue != "")
                Filter += " and CustID in (select custID from vw_CRMCustomerProd where CatID=" + ddlCategory.SelectedValue + ")";
            //����Ʒʹ�������ѯ�ͻ�
            if (ddlProduct.SelectedValue != "")
            {
                Filter += @" and CustID in (select custID from vw_CRMCustomerProd where Code like '" + ddlProduct.SelectedValue + "%')";
            }
            ////����Ʒʹ�������ѯ�ͻ�
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

                //��Terry.WebControl.DropdownlistʱҪ��svr.GetUserByDep()��OptGroup�����,�����UserID
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
                //���ݳɽ�ʱ�䲻ͬ,��ʾ��ͬ����ɫ,��,��,��,��
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
                //�����ж���¼���15��û��ϵ������...
                if (IdleDays > 15)
                    imgIdle.ImageUrl = "../images/report.gif";
                else
                    imgIdle.ImageUrl = "../images/setup.png";

                //���һ���ǰ�ť,�õ�Ԫ��Ҫonclick
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
		