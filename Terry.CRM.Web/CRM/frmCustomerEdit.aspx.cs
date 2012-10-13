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
using Terry.CRM.Web.CommonUtil;

namespace Terry.CRM.Web.CRM
{
    public partial class frmCustomerEdit : BasePage
    {
        private CustomerService svr = new CustomerService();
        private ContactService ctSvr = new ContactService();
        protected override void OnInit(EventArgs e)
        {
            GenerateProd();

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Authentication(enumModule.Customer, btnSave1);

            if (string.IsNullOrEmpty(Request["id"]))
                return;


            if (!Page.IsPostBack)
            {
                if (ConfigurationManager.AppSettings["Industry"].ToLower() == "chemical")
                {
                    tblChemical.Visible = true;
                    tblTicket.Visible = false;
                }
                else if (ConfigurationManager.AppSettings["Industry"].ToLower() == "ticket")
                {
                    tblChemical.Visible = false;
                    tblTicket.Visible = true;
                }
                //btnDel.Attributes.Add("onclick", "onDel('" + btnDel.UniqueID + "');return false;");
                hidID.Value = Request["id"];
                BindData();
            }
        }

        private void GenerateProd()
        {
            HtmlTableRow _row;
            HtmlTableRow _parentRow = new HtmlTableRow();
            HtmlTableCell _cell;
            TextBox _obj;
            DataRow dr;
            string Brand, Usage, Remark;
            //每级产品3位编号
            int ProdCodeLength = 3;

            DataTable dt = svr.SearchByCriteria("CRMProduct", out recordCount, "and len(code)<=6", "code");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                _row = new HtmlTableRow();
                _row.ID = dr["Code"].ToString();
                //每级产品3位编号
                if (_row.ID.Length == ProdCodeLength)
                {
                    _cell = new HtmlTableCell();
                    _cell.ColSpan = 4;
                    _cell.InnerHtml = "<span class=\"folder\">" + dr["Product"] + "</span>";
                    _row.Cells.Add(_cell);
                    _parentRow = _row;
                }
                else
                {
                    _row.Attributes.Add("class", "child-of-ctl00_CPH1_" + _row.ID.Substring(0, ProdCodeLength));
                    _cell = new HtmlTableCell();
                    _cell.InnerHtml = "<span class=\"file\">" + dr["Product"] + "</span>";
                    _row.Cells.Add(_cell);
                    //bind exist Data
                    var cp = svr.GetProdUsageBrand(Request["id"], dr["ProdID"].ToString());
                    if (cp == null)
                    {
                        Usage = "";
                        Brand = "";
                        Remark = "";
                    }
                    else
                    {
                        Usage = cp.Usage;
                        Brand = cp.Brand;
                        Remark = cp.Remark;
                        _parentRow.Style.Add("background-color", "#3875A7");
                    }
                    //add 用量
                    _cell = new HtmlTableCell();
                    _obj = new TextBox();
                    _obj.ID = "txtUsage_" + _row.ID;
                    _obj.Width = Unit.Percentage(98);
                    _obj.Text = Usage;
                    _cell.Controls.Add(_obj);
                    _row.Cells.Add(_cell);
                    //add 牌号
                    _cell = new HtmlTableCell();
                    _obj = new TextBox();
                    _obj.ID = "txtBrand_" + _row.ID;
                    _obj.Width = Unit.Percentage(98);
                    _obj.Text = Brand;
                    _cell.Controls.Add(_obj);
                    _row.Cells.Add(_cell);

                    //add 备注
                    _cell = new HtmlTableCell();
                    _obj = new TextBox();
                    _obj.ID = "txtRemark_" + _row.ID;
                    _obj.Width = Unit.Percentage(98);
                    _obj.Text = Remark;
                    _cell.Controls.Add(_obj);
                    _row.Cells.Add(_cell);
                }
                tbProd.Controls.Add(_row);
            }
        }

        //绑定联系人
        private void BindContact()
        {
            var ct = ctSvr.LoadById(hidID.Value, 1);
            string tel;
            if (ct != null)
            {
                if (string.IsNullOrEmpty(ct.ContactMobile))
                    tel = ct.ContactTel;
                else
                    tel = ct.ContactMobile;

                txtCT1.Text = ct.ContactName + " " + tel;
            }
            ct = ctSvr.LoadById(hidID.Value, 2);
            if (ct != null)
            {
                if (string.IsNullOrEmpty(ct.ContactMobile))
                    tel = ct.ContactTel;
                else
                    tel = ct.ContactMobile;

                txtCT2.Text = ct.ContactName + " " + tel;
            }
            ct = ctSvr.LoadById(hidID.Value, 3);
            if (ct != null)
            {
                if (string.IsNullOrEmpty(ct.ContactMobile))
                    tel = ct.ContactTel;
                else
                    tel = ct.ContactMobile;

                txtCT3.Text = ct.ContactName + " " + tel;
            }
            ct = ctSvr.LoadById(hidID.Value, 4);
            if (ct != null)
            {
                if (string.IsNullOrEmpty(ct.ContactMobile))
                    tel = ct.ContactTel;
                else
                    tel = ct.ContactMobile;

                txtCT4.Text = ct.ContactName + " " + tel;
            }

        }

        //绑定数据
        private void BindData()
        {
            //TODO: bind Dropdownlist,change accordingly
            ddlProvince.BindDropDownListAndSelect(svr.GetProvince(), "Province", "Province");
            txtCustTypeID.BindDropDownList(svr.GetCustType(), "CustType", "CustTypeID");
            txtCustRelationID.BindDropDownList(svr.GetCustRelation(), "Relation", "RelationID");
            txtCustIndustryID.BindDropDownList(svr.GetCustIndustry(), "Industry", "IndustryID");
            txtCustEmpNumID.BindDropDownListAndSelect(svr.GetCustEmpNum(), "EmpNum", "ID");
            txtCustCountryID.BindDropDownListAndSelect(svr.GetCountry(), "Country", "CountryID");
            txtCustOwnerID.BindDropDownListAndSelect(svr.GetUserByDep(), "UserName", "UserID");
            txtSYSID.BindDropDownList(svr.GetSystem(), "SYSName", "SYSID");

            //bind entity
            var entity = (vw_CRMCustomer)svr.LoadById(typeof(vw_CRMCustomer), "CustID", hidID.Value);
            if (entity != null)
            {
                BindContact();
                hidID.Value = entity.CustID.ToString();
                txtCustCode.Text = IsNull(entity.CustCode);
                txtCustName.Text = IsNull(entity.CustName);
                txtCustFullName.Text = IsNull(entity.CustFullName);
                txtCustTypeID.Text = entity.CustTypeID.ToString();
                txtCustRelationID.Text = IsNull(entity.CustRelationID);
                txtCustIndustryID.Text = IsNull(entity.CustIndustryID);
                txtCustEmpNumID.Text = IsNull(entity.CustEmpNumID);
                txtCustInfo.Text = IsNull(entity.CustInfo);
                txtCustCountryID.Text = IsNull(entity.CustCountryID);
                txtCustProvince.Text = IsNull(entity.CustProvince);
                ddlProvince.Text = IsNull(entity.CustProvince);
                txtCustCity.Text = IsNull(entity.CustCity);
                txtCustDistinct.Text = IsNull(entity.CustDistinct);
                txtCustAddress.Text = IsNull(entity.CustAddress);
                txtCustTel.Text = IsNull(entity.CustTel);

                if (string.IsNullOrEmpty(Request["Tel"]) == false)
                    txtCustTel.Text += "," + Request["Tel"];

                txtCustFax.Text = IsNull(entity.CustFax);
                txtCustWeb.Text = IsNull(entity.CustWeb);
                txtCustEmail.Text = IsNull(entity.CustEmail);

                txtArrivalPort.Text = entity.CustPort;
                txtPayMethod.Text = entity.CustPayMethod;
                txtPurchaseChannels.Text = entity.CustPurchaseChannels;
                txtSaleChannels.Text = entity.CustSaleChannels;
                txtMaterial.Text = entity.CustMaterial;
                
                txtCommissionFactor.Text = entity.CommissionFactor.ToString();
                txtCustProduct.Text = entity.CustProduct;

                txtCustCDate.Text = IsNull(entity.CustCDate);
                txtCustModifyDate.Text = IsNull(entity.CustModifyDate);
                txtCustCUserID.Value = entity.CustCUserID.ToString();
                txtCustModifyUserID.Value = entity.CustModifyUserID.ToString();
                txtCustCUserName.Text = entity.CustCUserName;
                txtCustModifyUserName.Text = entity.CustModifyUserName;
                txtCustOwnerID.Text = entity.CustOwnerID.ToString();
                txtSYSID.Text = entity.SYSID.ToString();
                //Ticket Fields
                chkTravelDay.SetChecked(entity.TravelDay, ",");
                BuddyList1.strBuddyList = entity.BuddyList;


                ddlPreferPrice.Text = entity.PreferPrice;
                ddlUseOwnMoney.Text = entity.UseOwnMoney.ToString();
                ddlRequireVisa.Text = entity.RequireVisa.ToString();
                ddlGender.Text = entity.Gender;

                if(entity.BirthDay!=null)
                    txtBirthday.Text = ((DateTime)entity.BirthDay).ToString(DateFormatString);

                txtFavoriteDest.Text = entity.FavoriteDest;
                txtFavoriteProd.Text = entity.FavoriteProd;
                txtPreferPlace.Text = entity.PreferPlace;
                txtBudget.Text = entity.Budget;
                txtPosition.Text = entity.Position;
                txtParentCompany.Text = entity.ParentCompany;
                txtPassport.Text = entity.Passport;
                txtPassportExpiryDate.Text = entity.PassportExpiryDate;
                //get audit log
                gvAuditLog.DataSource = svr.GetAuditLog(entity.CustID, txtCustCDate.Text, txtCustCUserName.Text, txtCustCUserID.Value);
                gvAuditLog.DataBind();
            }
            else
            {
                txtCustCDate.Text = DateTime.Now.ToString(GetREMes("DateTimeFormatStringCS"));
                txtCustCUserID.Value = base.LoginUserID.ToString();
                txtCustCUserName.Text = base.LoginUserName.ToLower();
                txtCustModifyDate.Text = DateTime.Now.ToString(GetREMes("DateTimeFormatStringCS"));
                txtCustModifyUserID.Value = base.LoginUserID.ToString();
                txtCustModifyUserName.Text = base.LoginUserName.ToLower();
                txtCustOwnerID.Text = base.LoginUserID.ToString();
                txtCommissionFactor.Text = "1";

                if (string.IsNullOrEmpty(Request["Tel"])==false)
                    txtCustTel.Text = Request["Tel"];



            }

            //销售人员不能修改所属客户
            if (base.LoginUserRoleGrade != (int)enumRoleGrade.Boss)
                txtCustOwnerID.Enabled = false;


        }

        //获取输入数据
        private CRMCustomer GetSaveEntity()
        {
            var entity = new CRMCustomer();
            if (string.IsNullOrEmpty(hidID.Value.Trim()) == false)
                entity.CustID = int.Parse(hidID.Value.Trim());
            //现在CustCode改成自动生成
            //if (string.IsNullOrEmpty(txtCustCode.Text.Trim()) == false)
            //    entity.CustCode = txtCustCode.Text.Trim();
            entity.CustCode = PinYin.GetChineseSpell(txtCustName.Text);

            if (string.IsNullOrEmpty(txtCustName.Text.Trim()) == false)
                entity.CustName = txtCustName.Text.Trim();
            if (string.IsNullOrEmpty(txtCustFullName.Text.Trim()) == false)
                entity.CustFullName = txtCustFullName.Text.Trim();
            if (string.IsNullOrEmpty(txtCustTypeID.Text.Trim()) == false)
                entity.CustTypeID = int.Parse(txtCustTypeID.Text.Trim());
            if (string.IsNullOrEmpty(txtCustRelationID.Text.Trim()) == false)
                entity.CustRelationID = int.Parse(txtCustRelationID.Text.Trim());
            if (string.IsNullOrEmpty(txtCustIndustryID.Text.Trim()) == false)
                entity.CustIndustryID = int.Parse(txtCustIndustryID.Text.Trim());
            if (string.IsNullOrEmpty(txtCustEmpNumID.Text.Trim()) == false)
                entity.CustEmpNumID = int.Parse(txtCustEmpNumID.Text.Trim());
            if (string.IsNullOrEmpty(txtCustInfo.Text.Trim()) == false)
                entity.CustInfo = txtCustInfo.Text.Trim();
            if (string.IsNullOrEmpty(txtCustCountryID.Text.Trim()) == false)
                entity.CustCountryID = int.Parse(txtCustCountryID.Text.Trim());
            //只有选择了其他,才用textbox的值
            if (ddlProvince.Text == "其他")
            {
                if (string.IsNullOrEmpty(txtCustProvince.Text.Trim()) == false)
                    entity.CustProvince = txtCustProvince.Text.Trim();
            }
            else
            {
                entity.CustProvince = ddlProvince.Text;
            }

            if (string.IsNullOrEmpty(txtCustCity.Text.Trim()) == false)
                entity.CustCity = txtCustCity.Text.Trim();
            if (string.IsNullOrEmpty(txtCustDistinct.Text.Trim()) == false)
                entity.CustDistinct = txtCustDistinct.Text.Trim();
            if (string.IsNullOrEmpty(txtCustAddress.Text.Trim()) == false)
                entity.CustAddress = txtCustAddress.Text.Trim();
            if (string.IsNullOrEmpty(txtCustTel.Text.Trim()) == false)
                entity.CustTel = txtCustTel.Text.Trim();
            if (string.IsNullOrEmpty(txtCustFax.Text.Trim()) == false)
                entity.CustFax = txtCustFax.Text.Trim();
            if (string.IsNullOrEmpty(txtCustWeb.Text.Trim()) == false)
                entity.CustWeb = txtCustWeb.Text.Trim();
            if (string.IsNullOrEmpty(txtCustEmail.Text.Trim()) == false)
                entity.CustEmail = txtCustEmail.Text.Trim();
            if (string.IsNullOrEmpty(txtCustCDate.Text.Trim()) == false)
                entity.CustCDate = DateTime.Parse(txtCustCDate.Text.Trim());
            if (string.IsNullOrEmpty(txtCustCUserID.Value.Trim()) == false)
                entity.CustCUserID = int.Parse(txtCustCUserID.Value.Trim());
            if (string.IsNullOrEmpty(txtCustOwnerID.Text.Trim()) == false)
                entity.CustOwnerID = int.Parse(txtCustOwnerID.Text.Trim());
            if (string.IsNullOrEmpty(txtSYSID.Text.Trim()) == false)
                entity.SYSID = int.Parse(txtSYSID.Text.Trim());

            if (string.IsNullOrEmpty(txtArrivalPort.Text.Trim()) == false)
                entity.CustPort = txtArrivalPort.Text.Trim();
            if (string.IsNullOrEmpty(txtPayMethod.Text.Trim()) == false)
                entity.CustPayMethod = txtPayMethod.Text;
            if (string.IsNullOrEmpty(txtPurchaseChannels.Text.Trim()) == false)
                entity.CustPurchaseChannels = txtPurchaseChannels.Text;
            if (string.IsNullOrEmpty(txtSaleChannels.Text.Trim()) == false)
                entity.CustSaleChannels = txtSaleChannels.Text;
            if (string.IsNullOrEmpty(txtMaterial.Text.Trim()) == false)
                entity.CustMaterial = txtMaterial.Text;

            entity.CustModifyDate = DateTime.Now;
            entity.CustModifyUserID = base.LoginUserID;
            //entity.CustBidPrice = txtBidPrice.Text;
            double cf = 0.0;
            double.TryParse(txtCommissionFactor.Text, out cf);
            entity.CommissionFactor = cf;
            entity.CustProduct = txtCustProduct.Text;
            entity.IsActive = true;
            //??还没想好Status字段用途，P，A，还是审批流程步骤1,2,3,4
            entity.Status = "A";

            //Ticket Fields
            if (string.IsNullOrEmpty(ddlGender.Text) == false)
                entity.Gender = ddlGender.SelectedValue;
            if (string.IsNullOrEmpty(txtBirthday.Text) == false)
                entity.BirthDay = Convert.ToDateTime(txtBirthday.Text);

            if (string.IsNullOrEmpty(ddlPreferPrice.Text) == false)
                entity.PreferPrice = ddlPreferPrice.Text;
            if (string.IsNullOrEmpty(ddlUseOwnMoney.Text) == false)
                entity.UseOwnMoney = Convert.ToBoolean(ddlUseOwnMoney.Text);
            if (string.IsNullOrEmpty(ddlRequireVisa.Text) == false)
                entity.RequireVisa = Convert.ToBoolean(ddlRequireVisa.Text);
            if (string.IsNullOrEmpty(txtFavoriteDest.Text) == false)
                entity.FavoriteDest = txtFavoriteDest.Text;
            if (string.IsNullOrEmpty(txtFavoriteProd.Text) == false)
                entity.FavoriteProd = txtFavoriteProd.Text;
            if (string.IsNullOrEmpty(txtPreferPlace.Text) == false)
                entity.PreferPlace = txtPreferPlace.Text;
            if (string.IsNullOrEmpty(txtBudget.Text) == false)
                entity.Budget = txtBudget.Text;
            if (string.IsNullOrEmpty(txtPosition.Text) == false)
                entity.Position = txtPosition.Text;
            if (string.IsNullOrEmpty(txtParentCompany.Text) == false)
                entity.ParentCompany = txtParentCompany.Text;
            if (string.IsNullOrEmpty(txtPassport.Text) == false)
                entity.Passport = txtPassport.Text;
            if (string.IsNullOrEmpty(txtPassportExpiryDate.Text) == false)
                entity.PassportExpiryDate = txtPassportExpiryDate.Text;

            if (string.IsNullOrEmpty(chkTravelDay.Text) == false)
                entity.TravelDay = chkTravelDay.GetChecked(",");

                entity.BuddyList = BuddyList1.strBuddyList;
            return entity;
        }

        //获取使用产品情况
        private IList<CRMCustomerProd> GetSaveProdInfo()
        {
            List<CRMCustomerProd> Lst = new List<CRMCustomerProd>();

            DataTable dt = svr.SearchByCriteria("CRMProduct", out recordCount, " and len(code)=6", "code");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtUsage = (TextBox)tbProd.FindControl("txtUsage_" + dt.Rows[i]["Code"].ToString());
                TextBox txtBrand = (TextBox)tbProd.FindControl("txtBrand_" + dt.Rows[i]["Code"].ToString());
                TextBox txtRemark = (TextBox)tbProd.FindControl("txtRemark_" + dt.Rows[i]["Code"].ToString());

                if (txtBrand == null || txtUsage == null || txtRemark == null) continue;

                //Usage,Brand其中之一有数据
                if (!(string.IsNullOrEmpty(txtUsage.Text) && string.IsNullOrEmpty(txtBrand.Text)))
                {
                    CRMCustomerProd cp = new CRMCustomerProd();
                    if (string.IsNullOrEmpty(hidID.Value.Trim()) == false)
                        cp.CustID = long.Parse(hidID.Value);
                    else
                        cp.CustID = 0;
                    cp.ProdID = long.Parse(dt.Rows[i]["ProdID"].ToString());
                    cp.Usage = txtUsage.Text;
                    cp.Brand = txtBrand.Text;
                    cp.Remark = txtRemark.Text;
                    Lst.Add(cp);
                }
            }
            return Lst;

        }
        //清空
        private void CleanFrm()
        {
            hidID.Value = "";
            txtCustCode.Text = "";
            txtCustName.Text = "";
            txtCustFullName.Text = "";
            txtCustTypeID.Text = "";
            txtCustRelationID.SelectedIndex = 0;
            txtCustIndustryID.SelectedIndex = 0;
            txtCustEmpNumID.SelectedIndex = 0;
            txtCustCountryID.SelectedIndex = 0;
            txtCustOwnerID.SelectedIndex = 0;
            txtSYSID.SelectedIndex = 0;
            txtCustInfo.Text = "";
            txtCustProvince.Text = "";
            txtCustCity.Text = "";
            txtCustDistinct.Text = "";
            txtCustAddress.Text = "";
            txtCustTel.Text = "";
            txtCustFax.Text = "";
            txtCustWeb.Text = "";
            txtCustEmail.Text = "";
            txtCustCDate.Text = "";
            txtCustCUserID.Value = "";
            txtCustModifyDate.Text = "";
            txtCustModifyUserID.Value = "";
            txtFavoriteDest.Text = "";
            txtFavoriteProd.Text = "";
            txtPreferPlace.Text = "";
            txtBudget.Text = "";
            txtPosition.Text = "";
            txtParentCompany.Text = "";
            txtPassport.Text = "";
            txtPassportExpiryDate.Text = "";
        }

        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //新增客户时,检查客户全名是否已存在
            if (hidID.Value == "0")
            {
                DataTable dt = svr.GetTopN(typeof(CRMCustomer), 1, "CustFullName=\"" + txtCustFullName.Text + "\"", "");
                if (dt.Rows.Count == 1)
                {

                    this.ShowMessage("客户全名已存在! <a href=\"frmCustomerEdit.aspx?id=" + dt.Rows[0]["CustID"] + "&Tel=" + Request["Tel"] + "\">把电话合并到已存在客户</a>");
                    return;
                }

            }

            try
            {
                Save();
                this.ShowSaveOK();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
                log4netHelper.Error("", ex);
            }

        }

        private void Save()
        {
            //get Customer entity
            var entity = GetSaveEntity();
            entity = svr.Save(entity, GetSaveProdInfo());
            hidID.Value = entity.CustID.ToString();
            txtCustModifyDate.Text = entity.CustModifyDate.ToString();
            txtCustModifyUserName.Text = base.LoginUserName.ToLower();

        }

        //Click Delete Button
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                svr.SoftDeleteById(typeof(CRMCustomer), "CustID", hidID.Value);
                svr.AddAuditLog(long.Parse(hidID.Value), base.LoginUserID, "删除客户资料");
                this.ShowDeleteOK();
                Response.Redirect("frmCustomer.aspx");
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
                log4netHelper.Error("", ex);
            }

        }

        //返回
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCustomer.aspx");
        }
        //电话拜访记录
        protected void btnTel_Click(object sender, EventArgs e)
        {
            if (hidID.Value == "0" && btnSave.Enabled)
                Save();
            ////ClientScript.RegisterClientScriptBlock(typeof(string),"tel",
            //    "<script>window.open('frmAction.aspx?ACType=1&CustID=" + hidID.Value+"');</script>");
        }

        //拜访记录
        protected void btnVisit_Click(object sender, EventArgs e)
        {
            if (hidID.Value == "0" && btnSave.Enabled)
                Save();
            //ClientScript.RegisterClientScriptBlock(typeof(string), "visit",
            //    "<script>window.open('frmAction.aspx?ACType=2&CustID=" + hidID.Value + "');</script>");
        }

        //成交记录
        protected void btnDeal_Click(object sender, EventArgs e)
        {
            if (hidID.Value == "0" && btnSave.Enabled)
                Save();
            //Response.Redirect("frmCustomerDeal.aspx?CustID=" + hidID.Value);
        }

        //点击联系人图标，弹出窗口
        protected void imgCT1_Click(object sender, ImageClickEventArgs e)
        {
            //save customer
            if (hidID.Value=="0" && btnSave.Enabled)
                Save();

            //pop up contact info
            //ImageButton btn = (ImageButton)sender;
            //string CTType = btn.ID.Substring(btn.ID.Length - 1);
            //ClientScript.RegisterStartupScript(typeof(string), "ct", "<script>showPopWin('联络信息','frmContactEdit.aspx?CustID=" + Request["Id"] + "&TypeID=" + CTType + "', 500, 150, null);</script>");
        }

        //刷新
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gvAuditLog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                e.Row.Cells[2].Text = e.Row.Cells[2].Text.Replace(";", ";<br/>");
            }
        }

        protected void btnTicket_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Invoice/frmTicketEdit.aspx?id=0&Custid="+ hidID.Value);
        }

        protected void btnVisa_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Invoice/frmVisaEdit.aspx?id=0&Custid=" + hidID.Value);
        }
    }
}

