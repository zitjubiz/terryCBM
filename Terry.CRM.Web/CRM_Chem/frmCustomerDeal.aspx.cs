using System;
using System.Collections;
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
using System.Collections.Generic;

namespace Terry.CRM.Web.CRM
{
    public partial class frmCustomerDeal_Chemical : BasePage
    {
        private const string EditURL = "frmCustomerDeal.aspx";
        private CustomerService svr = new CustomerService();

       private void BindData()
        {
            //add search criteria
            string Filter = " and CustID=" + Request["CustID"];
            lblCust.Text = svr.LoadById(Request["CustID"]).CustName;
            string OrderBy = gvData.OrderBy;
            if (OrderBy == "")
                OrderBy = "DealDate Desc";
            var ilist = svr.SearchByCriteria(typeof(vw_CRMCustomerDeal), gvData.PageIndex, base.GridViewPageSize,
                out recordCount, Filter, OrderBy);

            gvData.DataSource = ilist;
            gvData.PageSize = base.GridViewPageSize;
            gvData.VirtualItemCount = recordCount;
            gvData.EditIndex = -1;
            gvData.DataBind();
        }

        private void DeleteRow(string Id)
        {
            try
            {
                svr.DeleteById(typeof(CRMCustomerDeal), "ID", Id);
                this.ShowDeleteOK();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
        }
        //获取输入数据
        private CRMCustomerDeal GetSaveEntity()
        {
            var entity = new CRMCustomerDeal();
            entity.ID = long.Parse(hidID.Value);
            if (string.IsNullOrEmpty(Request["CustID"]) == false)
                entity.CustID = int.Parse(Request["CustID"]);

            if (string.IsNullOrEmpty(txtBrand.Text.Trim()) == false)
                entity.Brand = txtBrand.Text.Trim();

            if (string.IsNullOrEmpty(txtContractNum.Text.Trim()) == false)
                entity.ContractNum = txtContractNum.Text.Trim();

            if (string.IsNullOrEmpty(txtDealDate.Text.Trim()) == false)
                entity.DealDate = DateTime.Parse(txtDealDate.Text);

            if (string.IsNullOrEmpty(txtPayTerm.Text.Trim()) == false)
                entity.PayTerm = txtPayTerm.Text.Trim();

            if (string.IsNullOrEmpty(txtQty.Text.Trim()) == false)
                entity.QtyDesc = txtQty.Text;

            if (string.IsNullOrEmpty(txtUnitPrice.Text.Trim()) == false)
                entity.UnitPriceDesc = txtUnitPrice.Text;
            try
            {
                entity.TotalAmount = entity.UnitPrice * entity.Qty;
            }
            catch(Exception){}

            if (string.IsNullOrEmpty(ddlUnit.Text.Trim()) == false)
                entity.Unit = ddlUnit.Text.Trim();

            if (string.IsNullOrEmpty(ddlCurrency.Text.Trim()) == false)
                entity.Currency = ddlCurrency.Text.Trim();

            if (string.IsNullOrEmpty(ddlProduct.Text.Trim()) == false)
                entity.ProdID = int.Parse(ddlProduct.Text.Trim());


            if (string.IsNullOrEmpty(txtShipment.Text.Trim()) == false)
                entity.Shipment =txtShipment.Text.Trim();

                entity.StockCategory = txtStockCat.Text.Trim();

            entity.Status = int.Parse(RadStatus.SelectedValue);
            entity.Remark = txtRemark.Text.Trim();

            //entity.ReferenceNum = txtReferenceNum.Text.Trim();
            //entity.ProdSerialNum = txtProdSerialNum.Text.Trim();
            entity.DealOwner = long.Parse(txtCustOwnerID.SelectedValue);

            entity.ModifyDate = DateTime.Now;
            entity.ModifyUserID = base.LoginUserID;
            return entity;
        }
        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Save();
                this.ShowSaveOK();
                ClearInput();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        private void ClearInput()
        {
            hidID.Value = "0";
            txtAmount.Text = "";
            txtBrand.Text = "";
            txtContractNum.Text = "";
            txtDealDate.Text = "";
            txtPayTerm.Text = "";
            txtQty.Text = "";
            txtUnitPrice.Text ="";
            txtRemark.Text = "";
            ddlUnit.Text = "";
            ddlCurrency.Text = "MT";
            ddlProduct.Text = "";
            RadStatus.Text = "0";
            txtStockCat.Text = "";
            txtShipment.Text = "";

        }

        private void Save()
        {
            //get Customer entity
            var entity = GetSaveEntity();
            entity = svr.SaveDeal(entity);
            BindData();
        }

        //返回
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCustomerEdit.aspx?id=" + Request["CustID"]);
        }
        #region Common Code

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["CustID"]))
                return;

            Authentication(enumModule.Customer);
            if (!Page.IsPostBack)
            {
                hidID.Value = "0";
                ddlProduct.BindDropDownListAndSelect(svr.GetProductDDL(), "Proddl", "ProdID");
                txtCustOwnerID.BindDropDownListAndSelect(svr.GetUser(), "UserName", "UserID");
                txtCustOwnerID.Text = base.LoginUserID.ToString();
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
                if (e.Row.Cells[8].Text == "0")
                    e.Row.Cells[8].Text = "正常";
                else
                    e.Row.Cells[8].Text = "违约";

                Button btnDel = (Button)e.Row.FindControl("lnkDel");
                btnDel.Attributes.Add("onclick", "onDel('" + btnDel.UniqueID + "');return false;");
                //string sId = gvData.DataKeys[e.Row.RowIndex].Value.ToString();
                //e.Row.Attributes.Add("onclick", "onEdit('" + sId + "','" + EditURL + "'); return false;");
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
        protected void gvData_RowEditing(object sender, GridViewEditEventArgs e)
        {
            var entity = (CRMCustomerDeal)svr.LoadById(typeof(CRMCustomerDeal), "ID", gvData.DataKeys[e.NewEditIndex].Value.ToString());
            if (entity != null)
            {
                hidID.Value = entity.ID.ToString();
                txtAmount.Text = entity.TotalAmount.ToString();
                txtBrand.Text=entity.Brand;
                txtContractNum.Text = entity.ContractNum;
                txtDealDate.Text= ((DateTime)entity.DealDate).ToString("yyyy-MM-dd");
                txtPayTerm.Text = entity.PayTerm;
                txtQty.Text = entity.QtyDesc.ToString();
                txtUnitPrice.Text = entity.UnitPriceDesc.ToString();
                txtRemark.Text = entity.Remark;
                ddlUnit.Text = entity.Unit;
                ddlCurrency.Text =entity.Currency;
                ddlProduct.Text = entity.ProdID.ToString();
                RadStatus.Text = entity.Status.ToString();
                txtStockCat.Text = entity.StockCategory;
                txtShipment.Text = entity.Shipment;
                //txtProdSerialNum.Text = entity.ProdSerialNum;
                //txtReferenceNum.Text = entity.ReferenceNum;
                txtCustOwnerID.Text = entity.DealOwner.ToString();
            }
            gvData.EditIndex = -1;
            BindData();
        }
        //Sorting
        protected void gvData_Sorting(object sender, GridViewSortEventArgs e)
        {
            BindData();
        }



        #endregion
    }
}
