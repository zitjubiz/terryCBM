using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Terry.CRM.Entity;
using Terry.CRM.Service;
using Terry.CRM.Web.CommonUtil;
using Terry.CRM.Web;
using System.Data;

namespace Terry.CRM.Web.UserControl
{

    public partial class BuddyList : System.Web.UI.UserControl
    {
        private CustomerService svr = new CustomerService();

        public string strBuddyList
        {
            get { return (ViewState["strBuddyList"] == null) ? "" : ViewState["strBuddyList"].ToString(); }

            set { ViewState["strBuddyList"] = value; }
        } //split by "|"

        //public string strBuddyNameList
        //{
        //    get { return (ViewState["strBuddyNameList"] == null) ? "" : ViewState["strBuddyNameList"].ToString(); }

        //    set { ViewState["strBuddyNameList"] = value; }
        //} //split by "|"

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {    //bind cust list to ddlcust
                enumRoleGrade grade = (enumRoleGrade)((BasePage)this.Page).LoginUserRoleGrade;
                long userID=((BasePage)this.Page).LoginUserID;
                long roleID = ((BasePage)this.Page).LoginUserRoleID;

                ddlCust.BindDropDownListAndSelect(svr.GetCustNameTel(grade,userID,roleID), "CustNameTel", "CustIDName");

                BindData();
            }

        }
        private void BindData()
        {
            DataTable dt = new DataTable();

            if (string.IsNullOrEmpty(strBuddyList))
            {
                rptBuddy.DataSource = dt;
                rptBuddy.DataBind();
                return;
            }

            //get currnt cust's buddy list, bind to repeater

            string[] arrBuddy = strBuddyList.Split('|');
            string seperator = " ";//用来分隔名字和电话的字符串
            string strBuddyNameList = "";
            foreach (string buddy in arrBuddy)
            {
                var obj = svr.LoadById(buddy);
                if (obj != null)
                {
                    string buddyName = obj.CustName;
                    string buddyTel = obj.CustTel;
                    strBuddyNameList += buddyName + seperator + buddyTel + "|";
                }
            }
            string[] arrBuddyName = strBuddyNameList.Split('|');

            dt.Columns.Add("CustId", typeof(string));
            dt.Columns.Add("CustName", typeof(string));
            for (int i = 0; i < arrBuddy.Length; i++)
            {
                string buddyId = arrBuddy[i];
                string buddyName = arrBuddyName[i];
                if (!string.IsNullOrEmpty(buddyId))
                {
                    dt.Rows.Add(buddyId, buddyName);
                }
            }

            rptBuddy.DataSource = dt;
            rptBuddy.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string CustID = ddlCust.SelectedItem.Value.Split('|')[0];
            //string CustName = ddlCust.SelectedItem.Value.Split('|')[1];
            string[] arrBuddyID = strBuddyList.Split('|');

            //如果已存在，就不再加了
            if (arrBuddyID.Contains(CustID))
                return;

            if (string.IsNullOrEmpty(strBuddyList))
            {
                strBuddyList += CustID;
            }
            else
            {
                strBuddyList += "|" + CustID;
            }

            BindData();
        }
        // ABC|DEF|GHI
        protected void rptBuddy_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                //HyperLink lnk = (HyperLink)e.Item.FindControl("lnkName");
                //strBuddyNameList = strBuddyNameList.Replace("|" + lnk.Text, ""); //buddy not in line-front
                //strBuddyNameList = strBuddyNameList.Replace(lnk.Text + "|", "");  //buddy in line-front
                //strBuddyNameList = strBuddyNameList.Replace(lnk.Text, "");  //only one buddy
                HiddenField hidCustId = (HiddenField)e.Item.FindControl("hidCustId");
                strBuddyList = strBuddyList.Replace("|" + hidCustId.Value, ""); //buddy not in line-front
                strBuddyList = strBuddyList.Replace(hidCustId.Value + "|", "");  //buddy in line-front
                strBuddyList = strBuddyList.Replace(hidCustId.Value, "");  //only one buddy
                e.Item.Controls.Clear();
                BindData();
            }
        }

        protected void rptBuddy_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ImageButton btnDel = (ImageButton)e.Item.FindControl("btnDel");
            btnDel.Attributes.Add("onclick", "onDel('" + btnDel.UniqueID + "');return false;");
        }
    }
}