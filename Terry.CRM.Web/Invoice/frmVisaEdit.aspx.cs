using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Terry.CRM.Entity;
using Terry.CRM.Service;
using Terry.CRM.Web.CommonUtil;
using System.Data;
//using PDFHelper.Helper;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.HSSF.Util;

namespace Terry.CRM.Web.Invoice
{
    public partial class frmVisaEdit : BasePage
    {
        private VisaService svr = new VisaService();
        private CustomerService CustSvr = new CustomerService();

        protected void Page_Load(object sender, EventArgs e)
        {
            Authentication(enumModule.Customer);

            if (string.IsNullOrEmpty(Request["id"]))
                return;


            if (!Page.IsPostBack)
            {

                //btnDel.Attributes.Add("onclick", "onDel('" + btnDel.UniqueID + "');return false;");
                hidID.Value = Request["id"];
                BindData();
                
            }
            Page.Title += txtInnerReferenceID.Text;

            
        }
        //绑定数据
        private void BindData()
        {
            //TODO: bind Dropdownlist,change accordingly
            txtCustOwnerID.BindDropDownListAndSelect(svr.GetUserByDep(), "UserName", "UserID");


            //bind entity
            var entity = (vw_BillVisa)svr.LoadById(typeof(vw_BillVisa), "ID", hidID.Value);
            if (entity != null)
            {
                hidID.Value = entity.ID.ToString();
                hidDepId.Value = entity.DepId.ToString();

                ddlDept.SelectedByText(entity.DepName);
                txtDepAddress.Text = entity.DepAddress;
                txtInnerReferenceID.Text = entity.InnerReferenceID;
                txtBookingDate.Text = entity.BookingDate.ToString(DateFormatString);

                hidCustID.Text = entity.CustID.ToString();
                txtCustName.Text = IsNull(entity.CustName);
                txtCustTel.Text = IsNull(entity.CustTel);
                txtCustEmail.Text = IsNull(entity.CustEmail);
                txtCustAddress.Text = IsNull(entity.CustAddress);
                txtParentCompany.Text = IsNull(entity.ParentCompany);
                txtCustOwnerID.Text = entity.CustOwnerID.ToString();
                lblTotalAmount.Text = IsNull(entity.TotalAmount);

                BindInfoByDep();
                //combox need insert one
                ddlPayMethod.Items.Insert(0, entity.PayMethod);
                ddlPayMethod.Text = entity.PayMethod;

                if (entity.PayDate != null)
                    txtPaymentDate.Text = ((DateTime)entity.PayDate).ToString("yyyy-MM-dd");
                txtRemark.Text = entity.Remark;

                txtCustCDate.Text = IsNull(entity.CreateDate);
                txtCustModifyDate.Text = IsNull(entity.ModifyDate);
                txtCustCUserID.Value = entity.CreateUserID.ToString();
                txtCustModifyUserID.Value = entity.ModifyUserID.ToString();
                txtCustCUserName.Text = entity.CreateUserName;
                txtCustModifyUserName.Text = entity.ModifyUserName;

                //取得乘客名单
                gvData.DataSource = svr.getVisaPerson(entity.ID);
                gvData.DataBind();
                //get audit log
                gvAuditLog.DataSource = svr.GetAuditLog(entity.ID, txtCustCDate.Text, txtCustCUserName.Text, txtCustCUserID.Value);
                gvAuditLog.DataBind();
            }
            else
            {
                txtCustCDate.Text = DateTime.Now.ToString(DateTimeFormatString);
                txtCustCUserID.Value = base.LoginUserID.ToString();
                txtCustCUserName.Text = base.LoginUserName.ToLower();
                txtCustModifyDate.Text = DateTime.Now.ToString(DateTimeFormatString);
                txtCustModifyUserID.Value = base.LoginUserID.ToString();
                txtCustModifyUserName.Text = base.LoginUserName.ToLower();
                //录入人不一定是销售人员本人.所以取消默认值
                //txtCustOwnerID.Text = base.LoginUserID.ToString();
                txtBookingDate.Text = DateTime.Now.ToString(DateFormatString);
                //从客户信息那里过来
                if (string.IsNullOrEmpty(Request["CustID"]) == false)
                {

                    var Customer = (vw_CRMCustomer)svr.LoadById(typeof(vw_CRMCustomer), "CustID", Request["CustID"]);
                    if (Customer != null)
                    {
                        hidCustID.Text = Request["CustID"];
                        txtCustName.Text = IsNull(Customer.CustFullName);
                        txtCustTel.Text = IsNull(Customer.CustTel);
                        txtCustEmail.Text = IsNull(Customer.CustEmail);
                        txtCustAddress.Text = IsNull(Customer.CustAddress);
                        txtParentCompany.Text = IsNull(Customer.ParentCompany);
                    }
                }
            }


        }

        //获取输入数据
        private BillVisa GetSaveEntity()
        {
            var entity = new BillVisa();
            if (string.IsNullOrEmpty(hidID.Value.Trim()) == false)
                entity.ID = int.Parse(hidID.Value.Trim());
            //hardcode according ddldep text
            entity.DepName = ddlDept.SelectedItem.Text;

            if (string.IsNullOrEmpty(hidDepId.Value.Trim()) == false)
                entity.DepId = int.Parse(hidDepId.Value.Trim());
            else
            {
                if (entity.DepName.Contains("Düsseldorf"))
                    entity.DepId = 1;
                else if (entity.DepName.Contains("Stuttgart"))
                    entity.DepId = 2;
                else if (entity.DepName.Contains("Maastricht") || entity.DepName.Contains("Arnhem"))
                    entity.DepId = 3;
                else if (entity.DepName.Contains("肇庆"))
                    entity.DepId = 4;
                else if (entity.DepName.Contains("Köln"))
                    entity.DepId = 5;
            }

            if (string.IsNullOrEmpty(txtDepAddress.Text.Trim()) == false)
                entity.DepAddress = txtDepAddress.Text.Trim();

            if (string.IsNullOrEmpty(txtInnerReferenceID.Text.Trim()) == false)
                entity.InnerReferenceID = txtInnerReferenceID.Text.Trim();
            if (string.IsNullOrEmpty(txtBookingDate.Text.Trim()) == false)
                entity.BookingDate = DateTime.Parse(txtBookingDate.Text.Trim());

            if (string.IsNullOrEmpty(txtCustAddress.Text.Trim()) == false)
                entity.CustAddress = txtCustAddress.Text.Trim();
            else
                entity.CustAddress = "";
            if (string.IsNullOrEmpty(txtCustTel.Text.Trim()) == false)
                entity.CustTel = txtCustTel.Text.Trim();

            entity.CustEmail = txtCustEmail.Text.Trim();
            entity.ParentCompany = txtParentCompany.Text.Trim();

            if (string.IsNullOrEmpty(txtCustName.Text.Trim()) == false)
                entity.CustName = txtCustName.Text.Trim();

            if (hidCustID.Text != "")
                entity.CustID = long.Parse(hidCustID.Text);
            else
                entity.CustID = CustSvr.GetCustIDByName(entity.CustName,
                    entity.CustAddress, entity.CustTel, entity.CustEmail,
                    base.LoginUserID, int.Parse(txtCustOwnerID.Text.Trim()), true);


            entity.Currency = "EUR";
            entity.PayMethod = ddlPayMethod.Text;
            if (!string.IsNullOrEmpty(txtPaymentDate.Text))
                entity.PayDate = DateTime.Parse(txtPaymentDate.Text);
            entity.Remark = txtRemark.Text.Trim();
            if (string.IsNullOrEmpty(txtCustCDate.Text.Trim()) == false)
                entity.CreateDate = DateTime.Parse(txtCustCDate.Text.Trim());
            if (string.IsNullOrEmpty(txtCustCUserID.Value.Trim()) == false)
                entity.CreateUserID = int.Parse(txtCustCUserID.Value.Trim());
            if (string.IsNullOrEmpty(txtCustOwnerID.Text.Trim()) == false)
                entity.CustOwnerID = int.Parse(txtCustOwnerID.Text.Trim());

            entity.Status = 'A';
            entity.ModifyDate = DateTime.Now;
            entity.ModifyUserID = base.LoginUserID;

            return entity;
        }

        //清空
        private void CleanFrm()
        {
            hidID.Value = "";

            txtCustName.Text = "";
            ;
            txtCustOwnerID.SelectedIndex = 0;

            txtCustAddress.Text = "";
            txtCustTel.Text = "";

            txtCustEmail.Text = "";
            txtCustCDate.Text = "";
            txtCustCUserID.Value = "";
            txtCustModifyDate.Text = "";
            txtCustModifyUserID.Value = "";

        }

        private void BindInfoByDep()
        {
            if (txtInnerReferenceID.Text.StartsWith("28") || txtInnerReferenceID.Text.StartsWith("51") ||
                txtInnerReferenceID.Text.StartsWith("52") || txtInnerReferenceID.Text.StartsWith("53") )
            {
                ddlPayMethod.Items.Clear();
                ddlPayMethod.Items.Add("ahlung bei Abholung");
                ddlPayMethod.Items.Add("ab sofort");
                ddlPayMethod.Items.Add("zahlbar bis dd.MM.yyyy");
                ddlPayMethod.Items.Add("Betrag dankend erhalten !");

                gvData.Columns[10].Visible = false;
                gvData.Columns[11].Visible = true;
                gvData.Columns[13].Visible = true;
                gvData.Columns[14].Visible = true;
            }
            else
            {
                ddlPayMethod.Items.Clear();
                ddlPayMethod.Items.Add("pay on Pickup");
                ddlPayMethod.Items.Add("cash received on dd.MM.yyyy");
                ddlPayMethod.Items.Add("pay before dd.MM.yyyy ");

                gvData.Columns[10].Visible = true;
                gvData.Columns[11].Visible = false;
                gvData.Columns[13].Visible = false;
                gvData.Columns[14].Visible = false;
            }
        }

        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //新增时,检查内部订单号是否已存在
            if (hidID.Value == "0")
            {
                DataTable dt = svr.GetTopN(typeof(BillVisa), 1, "InnerReferenceID=\"" + txtInnerReferenceID.Text + "\"", "");
                if (dt.Rows.Count == 1)
                {
                    this.ShowMessage("内部订单号已存在!");
                    return;
                }

            }        

            try
            {
                Save();
                this.ShowSaveOK();
                BindData();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
                log4netHelper.Error("",ex);
            }

        }

        private void Save()
        {
            //get entity
            var entity = GetSaveEntity();
            entity = svr.Save(entity, BindPerson(false));
            hidID.Value = entity.ID.ToString();
            txtCustModifyDate.Text = entity.ModifyDate.ToString();
            txtCustModifyUserName.Text = base.LoginUserName.ToLower();

        }

        protected void gvAuditLog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                e.Row.Cells[2].Text = e.Row.Cells[2].Text.Replace(";", ";<br/>");
            }
        }

        private void GermanVisa()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("部门名称", ddlDept.SelectedItem.Text);
            dic.Add("部门地址", txtDepAddress.Text);
            dic.Add("预订日期", DateTime.Parse(txtBookingDate.Text).ToString("dd.MM.yyyy"));
            dic.Add("内部订单号", txtInnerReferenceID.Text);
            dic.Add("客户全名", txtCustName.Text);
            dic.Add("电话", txtCustTel.Text);
            dic.Add("电邮", txtCustEmail.Text);
            dic.Add("所属公司", txtParentCompany.Text);
            dic.Add("客户地址", txtCustAddress.Text);
            dic.Add("付款方式", ddlPayMethod.Text);
            dic.Add("付款日期", txtPaymentDate.Text);
            dic.Add("签证名单", BindPerson(false));

            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Visa_Template.xls", FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
            HSSFSheet sheet1 = hssfworkbook.GetSheet("Sheet1");
            //row,cell都是从0开始计数
            sheet1.GetRow(6).GetCell(0).SetCellValue("Fujian Int. Travel Tang (FITT) • " + dic["部门地址"].ToString());
            sheet1.GetRow(8).GetCell("F").SetCellValue(dic["部门名称"].ToString());
            sheet1.GetRow(8).GetCell("H").SetCellValue(dic["预订日期"].ToString());
            if (dic["所属公司"].ToString().Trim() == "")
                sheet1.GetRow(9).GetCell(0).SetCellValue(dic["客户全名"].ToString() + "\n" + dic["客户地址"].ToString());
            else
                sheet1.GetRow(9).GetCell(0).SetCellValue(dic["客户全名"].ToString() + "\n"
                    + dic["所属公司"].ToString() + "\n" + dic["客户地址"].ToString());

            sheet1.GetRow(13).GetCell(0).SetCellValue("T   " + dic["电话"].ToString() + "\nE   " + dic["电邮"].ToString());
            sheet1.GetRow(15).GetCell(0).SetCellValue("Rechnung Nr. " + dic["内部订单号"].ToString());

            var PersonList = dic["签证名单"] as List<BillVisaPerson>;

            sheet1.GetRow(18).GetCell(1).SetCellValue(PersonList.Count);

            int startRow = 21;
            //各人价格小计固定在34~38行
            for (int i = 0; i < PersonList.Count; i++)
            {
                //Einmalige Einreisen,Zweimalige Einreisen,Mehrmalige Einreisen 6 Monate
                if (PersonList[i].VisaType == "一次")
                    sheet1.GetRow(startRow + i).GetCell("C").SetCellValue("Einmalige Einreisen");
                else if (PersonList[i].VisaType == "两次")
                    sheet1.GetRow(startRow + i).GetCell("C").SetCellValue("Zweimalige Einreisen");
                else
                    sheet1.GetRow(startRow + i).GetCell("C").SetCellValue("Mehrmalige Einreisen 6 Monate");

                sheet1.GetRow(startRow + i).GetCell("F").SetCellValue((double)PersonList[i].EmbassyFee);
            }
            for (int i = PersonList.Count; i < 5; i++)
            {
                sheet1.GetRow(startRow + i).Hide();

            }
            startRow += 5;
            //--------HKPass & Birth Cert------------------------
            int HKPassCnt = PersonList.Where(t => t.HKPass > 0).Count();
            if (HKPassCnt > 0)
            {
                sheet1.GetRow(startRow).GetCell("F").SetCellValue((double)PersonList.Sum(t => t.HKPass) / HKPassCnt);
                sheet1.GetRow(startRow).GetCell("G").SetCellValue(HKPassCnt);
            }
            else
            {
                sheet1.GetRow(startRow).Hide();
            }
            startRow++;
            int BirthCertCnt = PersonList.Where(t => t.BirthCert > 0).Count();
            if (BirthCertCnt > 0)
            {
                sheet1.GetRow(startRow).GetCell("F").SetCellValue((double)PersonList.Sum(t => t.BirthCert) / BirthCertCnt);
                sheet1.GetRow(startRow).GetCell("G").SetCellValue(BirthCertCnt);
            }
            else
            {
                sheet1.GetRow(startRow).Hide();
            }
            startRow++;
            //？？？？----是每人收一份邮费，还是每一张账单收一份邮费呢？？？？----
            //邮费是DHL，费用10欧元，2次，一共收客人20----------------------
            //有时候，不用特别急，就不用DHL，用挂号信(Einschreiben)，那就是5欧元每次，2次，10欧元
            if (PersonList[0].PostFee == 20)
            {
                sheet1.GetRow(startRow).GetCell(0).SetCellValue("Porto DHL Express");
                sheet1.GetRow(startRow).GetCell("F").SetCellValue(10);
            }
            else if (PersonList[0].PostFee == 10)
            {
                sheet1.GetRow(startRow).GetCell(0).SetCellValue("Einschreiben");
                sheet1.GetRow(startRow).GetCell("F").SetCellValue(5);
            }
            startRow++;
            //-------------------------------------------
            int VisumExpressCnt = PersonList.Where(t => t.IsUrgent).Count();
            if (VisumExpressCnt == 0)
                sheet1.GetRow(startRow).Hide();
            else
                sheet1.GetRow(startRow).GetCell("G").SetCellValue(VisumExpressCnt);
            startRow++;
            //-------------------------------------------
            //Sevice Charge
            sheet1.GetRow(startRow).GetCell("F").SetCellValue((double)PersonList.Sum(t => t.ServiceFee) / PersonList.Count);
            sheet1.GetRow(startRow).GetCell("G").SetCellValue(PersonList.Count);
            //Force excel to recalculate all the formula while open
            sheet1.ForceFormulaRecalculation = true;
            string FullFileName = AppDomain.CurrentDomain.BaseDirectory + "Upload\\Excel\\" + dic["内部订单号"].ToString() + ".xls";
            file = new FileStream(FullFileName, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            DownloadFileAsAttachment(FullFileName);

        }

        private void NetherlandVisa()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("部门名称", ddlDept.SelectedItem.Text);
            dic.Add("部门地址", txtDepAddress.Text);
            dic.Add("预订日期", DateTime.Parse(txtBookingDate.Text).ToString("dd.MM.yyyy"));
            dic.Add("内部订单号", txtInnerReferenceID.Text);
            dic.Add("客户全名", txtCustName.Text);
            dic.Add("电话", txtCustTel.Text);
            dic.Add("电邮", txtCustEmail.Text);
            dic.Add("所属公司", txtParentCompany.Text);
            dic.Add("客户地址", txtCustAddress.Text);
            dic.Add("付款方式", ddlPayMethod.Text);
            dic.Add("付款日期", txtPaymentDate.Text);
            dic.Add("签证名单", BindPerson(false));

            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Visa_Template_Maa.xls", FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
            HSSFSheet sheet1 = hssfworkbook.GetSheet("Sheet1");
            //row,cell都是从0开始计数
            sheet1.GetRow(6).GetCell(0).SetCellValue("Fujian Int. Travel Tang (FITT) • " + dic["部门地址"].ToString());
            sheet1.GetRow(8).GetCell("F").SetCellValue(dic["部门名称"].ToString());
            sheet1.GetRow(8).GetCell("H").SetCellValue(dic["预订日期"].ToString());
            if (dic["所属公司"].ToString().Trim() == "")
                sheet1.GetRow(9).GetCell(0).SetCellValue(dic["客户全名"].ToString() + "\n" + dic["客户地址"].ToString());
            else
                sheet1.GetRow(9).GetCell(0).SetCellValue(dic["客户全名"].ToString() + "\n"
                    + dic["所属公司"].ToString() + "\n" + dic["客户地址"].ToString());

            sheet1.GetRow(13).GetCell(0).SetCellValue("T   " + dic["电话"].ToString() + "\nE   " + dic["电邮"].ToString());
            sheet1.GetRow(15).GetCell(0).SetCellValue("Invoice No. " + dic["内部订单号"].ToString());

            var PersonList = dic["签证名单"] as List<BillVisaPerson>;

            sheet1.GetRow(18).GetCell(1).SetCellValue(PersonList.Count);

            int startRow = 21;
            //各人价格小计固定在34~38行
            for (int i = 0; i < PersonList.Count; i++)
            {
                //Einmalige Einreisen,Zweimalige Einreisen,Mehrmalige Einreisen 6 Monate
                if (PersonList[i].VisaType == "一次")
                    sheet1.GetRow(startRow + i).GetCell("C").SetCellValue("One Entry (3 Months)");
                else if (PersonList[i].VisaType == "两次")
                    sheet1.GetRow(startRow + i).GetCell("C").SetCellValue("Two Entries (6 Months)");
                else
                    sheet1.GetRow(startRow + i).GetCell("C").SetCellValue("Multipel Entries (6 Months)");

                sheet1.GetRow(startRow + i).GetCell("F").SetCellValue((double)PersonList[i].EmbassyFee);
            }
            for (int i = PersonList.Count; i < 5; i++)
            {
                sheet1.GetRow(startRow + i).Hide();

            }
            startRow += 5;
            //Fees of Visa Center Den Haag
            sheet1.GetRow(startRow).GetCell("F").SetCellValue((double)PersonList.Sum(t => t.VisaCenterFee) / PersonList.Count);
            sheet1.GetRow(startRow).GetCell("G").SetCellValue(PersonList.Count);
            startRow +=2;

            //Sevice Charge
            sheet1.GetRow(startRow).GetCell("F").SetCellValue((double)PersonList.Sum(t => t.ServiceFee) / PersonList.Count);
            sheet1.GetRow(startRow).GetCell("G").SetCellValue(PersonList.Count);

            //付款方式
            sheet1.GetRow(43).GetCell("C").SetCellValue(dic["付款方式"].ToString());

            //Force excel to recalculate all the formula while open
            sheet1.ForceFormulaRecalculation = true;
            string FullFileName = AppDomain.CurrentDomain.BaseDirectory + "Upload\\Excel\\" + dic["内部订单号"].ToString() + ".xls";
            file = new FileStream(FullFileName, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            DownloadFileAsAttachment(FullFileName);
        }

        //click Excel Button
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            //德国签证由28改成51,52,53
            if (txtInnerReferenceID.Text.StartsWith("28") || txtInnerReferenceID.Text.StartsWith("5"))
                GermanVisa();
            else
                NetherlandVisa();
        }

        //Click Delete Button
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                svr.SoftDeleteById(typeof(BillVisa), "ID", hidID.Value);
                this.ShowDeleteOK();
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
            Response.Redirect("frmVisa.aspx");
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }
        //cancel
        protected void btnCANX_Click(object sender, EventArgs e)
        {
            if (btnCANX.Text == GetREMes("lblCancel"))
            {
                svr.SetStatusInactive(typeof(BillVisa), "id", hidID.Value);
                svr.AddAuditLog("status: Active=> Inactive", base.LoginUserID, long.Parse(hidID.Value));
                btnCANX.Text = GetREMes("lblRestore");
                this.ShowMessage("取消订单成功");
            }
            else
            {
                svr.SetStatusActive(typeof(BillVisa), "id", hidID.Value);
                svr.AddAuditLog("status: Inactive=> Active", base.LoginUserID, long.Parse(hidID.Value));
                btnCANX.Text = GetREMes("lblCancel");
                this.ShowMessage("恢复订单成功");
            }
        }

        //ddlDept.OnSelectedIndexChanged and txtBookingDate_TextChanged
        protected void txtBookingDate_TextChanged(object sender, EventArgs e)
        {
            if (hidID.Value == "0") //新增
                txtInnerReferenceID.Text = svr.GetNextInnerReferenceID(ddlDept.SelectedItem.Text, txtBookingDate.Text);
            
            BindInfoByDep();
        }

        #region Person

        protected void btnAddPerson_Click(object sender, EventArgs e)
        {
            gvData.DataSource = BindPerson(true);
            gvData.DataBind();
        }

        private List<BillVisaPerson> BindPerson(bool AddNew)
        {
            List<BillVisaPerson> lis = new List<BillVisaPerson>();
            List<string> OwnerList = new List<string>();
            BillVisaPerson person;
            TextBox txtVisaName, txtCountry, txtPassport, txtPostFee, txtServiceFee, txtBirthCert, txtHKPass;
            TextBox txtEntryDate, txtApplyDate, txtApproveDate, txtEmbassySerialNum, txtEmbassyFee, txtVisaCenterFee;
            DropDownList ddlVisaType, ddlUrgent;

            //保存现有的gridview 数据
            for (int i = 0; i < gvData.Rows.Count; i++)
            {
                if (gvData.Rows[i].Visible == false) continue;

                txtVisaName = (TextBox)gvData.Rows[i].FindControl("txtVisaName");
                OwnerList.Add(txtVisaName.Text);
                txtCountry = (TextBox)gvData.Rows[i].FindControl("txtCountry");
                txtPassport = (TextBox)gvData.Rows[i].FindControl("txtPassport");
                txtEntryDate = (TextBox)gvData.Rows[i].FindControl("txtEntryDate");
                txtApplyDate = (TextBox)gvData.Rows[i].FindControl("txtApplyDate");
                txtApproveDate = (TextBox)gvData.Rows[i].FindControl("txtApproveDate");
                txtEmbassySerialNum = (TextBox)gvData.Rows[i].FindControl("txtEmbassySerialNum");
                txtEmbassyFee = (TextBox)gvData.Rows[i].FindControl("txtEmbassyFee");
                txtPostFee = (TextBox)gvData.Rows[i].FindControl("txtPostFee");
                txtServiceFee = (TextBox)gvData.Rows[i].FindControl("txtServiceFee");
                txtBirthCert = (TextBox)gvData.Rows[i].FindControl("txtBirthCert");
                txtHKPass = (TextBox)gvData.Rows[i].FindControl("txtHKPass");
                txtVisaCenterFee = (TextBox)gvData.Rows[i].FindControl("txtVisaCenterFee");

                ddlVisaType = (DropDownList)gvData.Rows[i].FindControl("ddlVisaType");
                ddlUrgent = (DropDownList)gvData.Rows[i].FindControl("ddlUrgent");
                person = new BillVisaPerson();
                person.VisaName = txtVisaName.Text.Trim();
                person.Country = txtCountry.Text.Trim();
                person.Passport = txtPassport.Text.Trim();
                person.VisaType = ddlVisaType.Text;
                person.IsUrgent = bool.Parse(ddlUrgent.Text);
                if (!string.IsNullOrEmpty(txtEntryDate.Text))
                    person.EntryDate = DateTime.Parse(txtEntryDate.Text);
                if (!string.IsNullOrEmpty(txtApplyDate.Text))
                    person.ApplyDate = DateTime.Parse(txtApplyDate.Text);
                if (!string.IsNullOrEmpty(txtApproveDate.Text))
                    person.ApproveDate = DateTime.Parse(txtApproveDate.Text);
                person.EmbassySerialNum = txtEmbassySerialNum.Text.Trim();
                person.EmbassyFee = decimal.Parse(txtEmbassyFee.Text);
                person.PostFee = decimal.Parse(txtPostFee.Text==""?"0":txtPostFee.Text);
                person.ServiceFee = decimal.Parse(txtServiceFee.Text);
                person.BirthCert = decimal.Parse(txtBirthCert.Text==""?"0":txtBirthCert.Text);
                person.HKPass = decimal.Parse(txtHKPass.Text == "" ? "0" : txtHKPass.Text);
                person.VisaCenterFee = decimal.Parse(txtVisaCenterFee.Text==""?"0": txtVisaCenterFee.Text);
                lis.Add(person);
            }
            //新增一行
            if (AddNew)
            {
                person = new BillVisaPerson();
                person.PostFee = 20;
                person.ServiceFee = 20;

                lis.Add(person);
            }
            return lis;
        }
        //签证人员表
        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtEntryDate, txtApplyDate, txtApproveDate;
                txtEntryDate = (TextBox)e.Row.FindControl("txtEntryDate");
                txtApplyDate = (TextBox)e.Row.FindControl("txtApplyDate");
                txtApproveDate = (TextBox)e.Row.FindControl("txtApproveDate");
                var person = e.Row.DataItem as BillVisaPerson;
                if (person.EntryDate != null)
                    txtEntryDate.Text = ((DateTime)person.EntryDate).ToString("yyyy-MM-dd");
                if (person.ApplyDate != null)
                    txtApplyDate.Text = ((DateTime)person.ApplyDate).ToString("yyyy-MM-dd");
                if (person.ApproveDate != null)
                    txtApproveDate.Text = ((DateTime)person.ApproveDate).ToString("yyyy-MM-dd");

            }
        }
        #endregion

    }
}