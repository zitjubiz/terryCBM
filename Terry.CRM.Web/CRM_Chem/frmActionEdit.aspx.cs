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
using Terry.CRM.Entity;
using Terry.CRM.Service;
using System.Collections.Generic;

namespace Terry.CRM.Web.CRM
{
    public partial class frmActionEdit_Chemical : BasePage
    {
        private ActionService svr = new ActionService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["id"]))
                return;

            if (!Page.IsPostBack)
            {
                Authentication(enumModule.Customer,btnComment);
                btnDel.Attributes.Add("onclick", "onDel('" + btnDel.UniqueID + "');return false;");
                hidID.Value = Request["id"];
                BindData();
                if (txtACTType.SelectedValue == "1")
                    lblActionType.Text = GetREMes("lblActionTel");
                else if(txtACTType.SelectedValue == "2")
                    lblActionType.Text = GetREMes("lblActionVisit");
                else if (txtACTType.SelectedValue == "3")
                    lblActionType.Text = GetREMes("lblActionBid");

                BindUser();
                BindComment();
            }
        }
        private void BindUser()
        {
            //bind Products
            DDCLUser.DataSource = svr.GetUser();
            DDCLUser.DataTextField = "UserName";
            DDCLUser.DataValueField = "UserID";
            DDCLUser.DataBind();
            //bind action partispate person
            var Users = svr.GetJoinUsers(long.Parse(hidID.Value));
            foreach (var p in Users)
            {
                foreach (ListItem item in DDCLUser.Items)
                {
                    //从数据库查出绑定,或者新建时,默认选中录入人
                    if (item.Value == p.ACTUser.ToString() || 
                        (hidID.Value=="0" &&item.Value == base.LoginUserID.ToString()))
                        item.Selected = true;
                }
            }
        }

        private void BindComment()
        {
            //只显示该用户的该类型的拜访记录

            IList<CRMActionComment> ilist = svr.GetActionComments(int.Parse(hidID.Value));
            recordCount = ilist.Count;
            gvData.DataSource = ilist;
            gvData.VirtualItemCount = ilist.Count;
            gvData.DataBind();

        }

        private void BindData()
        {
            //TODO: bind Dropdownlist,change accordingly
            txtACTCustID.BindDropDownList(svr.GetCustomer(), "CustName", "CustID");
            txtACTType.BindDropDownList(svr.GetActionType(), "ACTType", "ID");

            //bind entity
            var entity = (vw_CRMAction)svr.LoadById(typeof(vw_CRMAction), "ACTID", hidID.Value);
            if (entity != null)
            {
                txtACTCustID.Text = entity.CustID.ToString();
                lblCust.Text = entity.CustName;
                txtACTID.Text = entity.ACTID.ToString();

                if (entity.ACTSubject != null)
                {
                    txtACTSubject.Text = entity.ACTSubject.ToString();
                }
                if (entity.ACTType != null)
                {
                    txtACTType.SelectedByValue(entity.ACTTypeID.ToString());
                }
                if (entity.ACTBeginDate != null)
                {
                    txtACTBeginDate.Text = ((DateTime)entity.ACTBeginDate).ToString(GetREMes("DateTimeFormatStringCS"));
                }
                if (entity.ACTEndDate != null)
                {
                    txtACTEndDate.Text = ((DateTime)entity.ACTEndDate).ToString(GetREMes("DateTimeFormatStringCS"));
                }
                if (entity.ACTContent != null)
                {
                    txtACTContent.Text = entity.ACTContent.ToString();
                }

                if (entity.ACTCDate != null)
                {
                    txtACTCDate.Text = entity.ACTCDate.ToString(GetREMes("DateTimeFormatStringCS"));
                }
                txtACTCUserID.Value = entity.ACTCUser.ToString();
                if (entity.ACTCUserName != null)
                {
                    txtACTCUser.Text = entity.ACTCUserName.ToString();
                }

                if (entity.ACTModifyDate != null)
                {
                    txtACTModifyDate.Text = entity.ACTModifyDate.ToString(GetREMes("DateTimeFormatStringCS"));
                }
                txtACTModifyUserID.Value = entity.ACTModifyUser.ToString();
                if (entity.ACTModifyUserName != null)
                {
                    txtACTModifyUser.Text = entity.ACTModifyUserName.ToString();
                }


            }
            else
            {
                //新建行动记录
                txtACTType.SelectedByValue(Request["ACType"]);
                txtACTCustID.SelectedByValue(Request["CustID"]);
                lblCust.Text = txtACTCustID.SelectedItem.Text;
                txtACTCDate.Text = DateTime.Now.ToString(GetREMes("DateTimeFormatStringCS"));
                txtACTCUserID.Value = base.LoginUserID.ToString();
                txtACTCUser.Text = base.LoginUserName;
                txtACTModifyDate.Text = DateTime.Now.ToString(GetREMes("DateTimeFormatStringCS"));
                txtACTModifyUserID.Value = base.LoginUserID.ToString();
                txtACTModifyUser.Text = base.LoginUserName;
            }

        }

        private CRMAction GetSaveEntity()
        {
            var entity = new CRMAction();
            if (string.IsNullOrEmpty(txtACTID.Text.Trim()) == false)
                entity.ACTID = int.Parse(txtACTID.Text.Trim());
            if (string.IsNullOrEmpty(txtACTSubject.Text.Trim()) == false)
                entity.ACTSubject = txtACTSubject.Text.Trim();
            if (string.IsNullOrEmpty(txtACTType.Text.Trim()) == false)
                entity.ACTType = int.Parse(txtACTType.Text.Trim());
            if (string.IsNullOrEmpty(txtACTBeginDate.Text.Trim()) == false)
                entity.ACTBeginDate = DateTime.Parse(txtACTBeginDate.Text.Trim());
            if (string.IsNullOrEmpty(txtACTEndDate.Text.Trim()) == false)
                entity.ACTEndDate = DateTime.Parse(txtACTEndDate.Text.Trim());
            if (string.IsNullOrEmpty(txtACTContent.Text.Trim()) == false)
                entity.ACTContent = txtACTContent.Text.Trim();
            if (string.IsNullOrEmpty(txtACTCustID.Text.Trim()) == false)
                entity.ACTCustID = int.Parse(txtACTCustID.Text.Trim());

            if (string.IsNullOrEmpty(txtACTCDate.Text.Trim()) == false)
                entity.ACTCDate = DateTime.Parse(txtACTCDate.Text.Trim());
            if (string.IsNullOrEmpty(txtACTCUser.Text.Trim()) == false)
                entity.ACTCUser = int.Parse(txtACTCUserID.Value.Trim());

            //Modify时用现在时间和现在login的用户
            entity.ACTModifyDate = DateTime.Now;
            entity.ACTModifyUser = base.LoginUserID;
            entity.IsActive = true;
            return entity;
        }

        private void CleanFrm()
        {
            txtACTID.Text = "";
            txtACTSubject.Text = "";
            txtACTType.Text = "";
            txtACTBeginDate.Text = "";
            txtACTEndDate.Text = "";
            txtACTContent.Text = "";
            txtACTCustID.Text = "";
            txtACTCDate.Text = "";
            txtACTCUser.Text = "";
            txtACTModifyDate.Text = "";
            txtACTModifyUser.Text = "";
        }

        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = GetSaveEntity();
                List<CRMUser> UList = new List<CRMUser>();
                if (DDCLUser.SelectedValuesToString() != "")
                { 
                    string[] arr = DDCLUser.SelectedValuesToString().Split(',');
                    foreach (var ID in arr)
                    {
                        var p = new CRMUser();
                        p.UserID = long.Parse(ID);
                        UList.Add(p);
                    }                
                }

                entity = svr.Save(entity,UList);
                hidID.Value = entity.ACTID.ToString();
                this.ShowSaveOK();
                btnBack_Click(null, null);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        //Click Delete Button
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                //Soft Delete Action
                svr.SoftDeleteById(typeof(CRMAction), "ACTID", hidID.Value);
                this.ShowDeleteOK();
                btnBack_Click(null, null);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            string ActType, CustID;
            ActType = txtACTType.SelectedValue;
            CustID = txtACTCustID.SelectedValue;

            Response.Redirect("frmUserActions.aspx?type=&cust=" + lblCust.Text + "&user=&custid=" + CustID);
        }

        protected void btnComment_Click(object sender, EventArgs e)
        {
            if( txtComment.Text.Trim()!=null && hidID.Value!="0")
            {
                var entity = new CRMActionComment();
                entity.ACTID = int.Parse(hidID.Value);
                entity.CreateDate = DateTime.Now;
                entity.CreateUser = base.LoginUserID;
                entity.Comment = txtComment.Text.Trim();
                svr.Save(entity);
                BindComment();
            }
        }

        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
            BindComment();
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
                var u =(CRMUser)svr.LoadById(typeof(CRMUser), "UserID", e.Row.Cells[2].Text);
                if(u!=null)
                    e.Row.Cells[2].Text = u.UserName;
            }
        }
        //Delete a Row
        protected void gvData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                //DeleteRow(gvData.DataKeys[e.RowIndex].Value.ToString());

            }
            catch (Exception)
            {
                ShowMessage(GetREMes("MsgCannotDelete"));
            }
            BindComment();
        }
        //Sorting
        protected void gvData_Sorting(object sender, GridViewSortEventArgs e)
        {
            BindData();
        }
    }
}

