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
using System.Collections;

namespace Terry.CRM.Web.Invoice
{
    public partial class frmTicketEdit : BasePage
    {
        private BillingService svr = new BillingService();
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

            //现在不用批量上传银行对账记录,所以录入订单和录入银行对账在同一界面,
            //要根据enumModule.BankStatement的权限来决定gridview里的对账记录是否可写.
            var rights = svr.GetRoleAccessRight(LoginUserRoleID, enumModule.BankStatement);
            if (rights == null || (!rights.New && !rights.Edit))
            {
                for (int i = 0; i < gvData.Rows.Count; i++)
                {
                    TextBox txt = gvData.Rows[i].FindControl("txtBankStatement") as TextBox;
                    if (txt != null)
                        txt.ReadOnly = true;
                }

            }
        }

        //绑定数据
        private void BindData()
        {
            //TODO: bind Dropdownlist,change accordingly
            txtCustOwnerID.BindDropDownListAndSelect(svr.GetUserByDep(), "UserName", "UserID");


            //bind entity
            var entity = (vw_BillTicket)svr.LoadById(typeof(vw_BillTicket), "ID", hidID.Value);
            if (entity != null)
            {
                if (entity.Status == 'A')
                    btnCANX.Text = GetREMes("lblCancel");
                else
                    btnCANX.Text = GetREMes("lblRestore");

                hidID.Value = entity.ID.ToString();
                hidDepId.Value = entity.DepId.ToString();

                ddlDept.SelectedByText(entity.DepName);
                //根据德国/荷兰,决定有些选项是显示德文还是英文
                BindInfoByDep(ddlDept.SelectedItem.Text);

                txtDepAddress.Text = entity.DepAddress;
                txtInnerReferenceID.Text = entity.InnerReferenceID;
                if (txtInnerReferenceID.Text.EndsWith("GS"))
                    ddlBillType.Text = "1";
                else if(txtInnerReferenceID.Text.EndsWith("REB"))
                    ddlBillType.Text = "2";

                HideFieldsWhenRefund();

                txtBookingDate.Text = entity.BookingDate.ToString(DateFormatString);

                hidCustID.Text = entity.CustID.ToString();
                txtCustName.Text = IsNull(entity.CustName);
                txtCustTel.Text = IsNull(entity.CustTel);
                txtCustFax.Text = IsNull(entity.CustFax);
                txtCustEmail.Text = IsNull(entity.CustEmail);
                txtCustAddress.Text = IsNull(entity.CustAddress);
                txtParentCompany.Text = IsNull(entity.ParentCompany);

                txtAirline.Items.Insert(0, entity.ProdProvider);
                txtAirline.Text = entity.ProdProvider;
                txtBankAccount.Text = entity.BankAccount;

                txtAccessory.Text = entity.Accessory;
                txtMaxLuggage.Text = entity.MaxLuggage;

                txtCancellationFee.Items.Insert(0, entity.CancellationFee);
                txtCancellationFee.Text = entity.CancellationFee;
                txtChangeFee.Items.Insert(0, entity.ChangeFee);
                txtChangeFee.Text = entity.ChangeFee;

                txtPaymentDate.Text = entity.PaymentDate;
                ddlDestination.Text = entity.DestinationRegion;

                txtCustCDate.Text = IsNull(entity.CreateDate);
                txtCustModifyDate.Text = IsNull(entity.ModifyDate);
                txtCustCUserID.Value = entity.CreateUserID.ToString();
                txtCustModifyUserID.Value = entity.ModifyUserID.ToString();
                txtCustCUserName.Text = entity.CreateUserName;
                txtCustModifyUserName.Text = entity.ModifyUserName;
                txtCustOwnerID.Text = entity.CustOwnerID.ToString();

                lblTotalAmount.Text = IsNull(entity.TotalAmount);
                //取得乘客名单
                gvData.DataSource = svr.getTicketPerson(entity.ID);
                gvData.DataBind();
                //行程的乘客名单是一样的
                rptTour.DataSource = gvData.DataSource;
                rptTour.DataBind();
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
                        txtCustFax.Text = IsNull(Customer.CustFax);
                        txtCustEmail.Text = IsNull(Customer.CustEmail);
                        txtCustAddress.Text = IsNull(Customer.CustAddress);
                        txtParentCompany.Text = IsNull(Customer.ParentCompany);
                    }
                }
            }



        }

        //获取输入数据
        private BillTicket GetSaveEntity()
        {
            var entity = new BillTicket();
            if (string.IsNullOrEmpty(hidID.Value.Trim()) == false)
                entity.ID = int.Parse(hidID.Value.Trim());
            //hardcode according ddldep text
            entity.DepName = ddlDept.SelectedItem.Text;

            //??还是根据内部订单号前2位决定部门??
            //if (string.IsNullOrEmpty(hidDepId.Value.Trim()) == false)
            //    entity.DepId = int.Parse(hidDepId.Value.Trim());
            //else
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

            entity.ParentCompany = txtParentCompany.Text;

            if (string.IsNullOrEmpty(txtCustName.Text.Trim()) == false)
                entity.CustName = txtCustName.Text.Trim();

            entity.CustAddress = txtCustAddress.Text.Trim();

            entity.CustTel = txtCustTel.Text.Trim();
            entity.CustFax = txtCustFax.Text.Trim();
            entity.CustEmail = txtCustEmail.Text.Trim();

            if (hidCustID.Text != "")
                entity.CustID = long.Parse(hidCustID.Text);
            else
                entity.CustID = CustSvr.GetCustIDByName(entity.CustName,
                    entity.CustAddress, entity.CustTel, entity.CustEmail,
                    base.LoginUserID, int.Parse(txtCustOwnerID.Text.Trim()), true);

            
            entity.ProdProvider = txtAirline.Text.Trim();
            entity.BankAccount = txtBankAccount.Text.Trim();

            entity.Currency = "EUR";
            entity.Accessory = txtAccessory.Text;
            entity.MaxLuggage = txtMaxLuggage.Text;
            entity.CancellationFee = txtCancellationFee.Text;
            entity.ChangeFee = txtChangeFee.Text;
            entity.PaymentDate = txtPaymentDate.Text;
            entity.DestinationRegion = ddlDestination.Text;

            if (string.IsNullOrEmpty(txtCustCDate.Text.Trim()) == false)
                entity.CreateDate = DateTime.Parse(txtCustCDate.Text.Trim());
            if (string.IsNullOrEmpty(txtCustCUserID.Value.Trim()) == false)
                entity.CreateUserID = int.Parse(txtCustCUserID.Value.Trim());
            if (string.IsNullOrEmpty(txtCustOwnerID.Text.Trim()) == false)
                entity.CustOwnerID = int.Parse(txtCustOwnerID.Text.Trim());


            entity.ModifyDate = DateTime.Now;
            entity.ModifyUserID = base.LoginUserID;

            //还没想好Status字段用途
            entity.Status = 'A';


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
        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //新增时,检查内部订单号是否已存在
            if (hidID.Value == "0" || hidID.Value == "")
            {
                DataTable dt = svr.GetTopN(typeof(BillTicket), 1, "InnerReferenceID=\"" + txtInnerReferenceID.Text + "\"", "");
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
                log4netHelper.Error("", ex);
            }

        }

        private void Save()
        {
            //get entity
            var entity = GetSaveEntity();
            entity = svr.Save(entity, BindPerson(false), GetAllPersonTours());
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

        //click Excel Button
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("部门名称", ddlDept.SelectedItem.Text);
            dic.Add("部门地址", txtDepAddress.Text);
            dic.Add("预订日期", DateTime.Parse(txtBookingDate.Text).ToString("dd.MM.yyyy"));
            dic.Add("内部订单号", txtInnerReferenceID.Text);
            dic.Add("客户全名", txtCustName.Text);
            dic.Add("电话", txtCustTel.Text);
            dic.Add("传真", txtCustFax.Text);
            dic.Add("电邮", txtCustEmail.Text);
            dic.Add("银行账户", txtBankAccount.Text);
            dic.Add("航空公司", txtAirline.Text);

            dic.Add("客户地址", txtCustAddress.Text);
            dic.Add("最大行李重量", txtMaxLuggage.Text);
            dic.Add("含税/火车票", txtAccessory.Text);
            dic.Add("取消费用", txtCancellationFee.Text);
            dic.Add("改签费用", txtChangeFee.Text);
            dic.Add("付款日期", txtPaymentDate.Text);
            dic.Add("所属公司", txtParentCompany.Text);
            dic.Add("乘客名单", BindPerson(false));
            dic.Add("行程信息", GetAllPersonTours());
            var PersonList = dic["乘客名单"] as List<BillTicketPerson>;
            var TourList = dic["行程信息"] as List<BillTicketTour>;

            bool IsNL = false; //是否荷兰,账单用英文还是德文显示
            string TemplateFile="";
            if (ddlDept.SelectedItem.Text == "Maastricht")
            { 
                IsNL = true;
                if (PersonList.Count > 5)
                    TemplateFile = AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Ticket_Template_maa_2page.xls";
                else
                    TemplateFile = AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Ticket_Template_maa.xls";
            }
            if (ddlDept.SelectedItem.Text == "Arnhem")
            {
                IsNL = true;
                if (PersonList.Count > 5)
                    TemplateFile = AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Ticket_Template_arn_2page.xls";
                else
                    TemplateFile = AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Ticket_Template_arn.xls";
            }

            if (IsNL == false)
            {
                if (PersonList.Count > 5)
                {
                    if (ddlDept.SelectedItem.Text == "Düsseldorf")
                        TemplateFile = AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Ticket_Template_2page.xls";
                    else if (ddlDept.SelectedItem.Text == "Stuttgart")
                        TemplateFile = AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Ticket_Template_stug_2page.xls";
                    else if (ddlDept.SelectedItem.Text == "Köln")
                        TemplateFile = AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Ticket_Template_koln_2page.xls";
                    else if (ddlDept.SelectedItem.Text == "Nürnberg")
                        TemplateFile = AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Ticket_Template_nurn_2page.xls";
                    else
                        TemplateFile = AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Ticket_Template_2page.xls";
                }
                else
                {
                    if (ddlDept.SelectedItem.Text == "Düsseldorf")
                        TemplateFile = AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Ticket_Template.xls";
                    else if (ddlDept.SelectedItem.Text == "Stuttgart")
                        TemplateFile = AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Ticket_Template_stug.xls";
                    else if (ddlDept.SelectedItem.Text == "Köln")
                        TemplateFile = AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Ticket_Template_koln.xls";
                    else if (ddlDept.SelectedItem.Text == "Nürnberg")
                        TemplateFile = AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Ticket_Template_nurn.xls";
                    else
                        TemplateFile = AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Ticket_Template.xls";
                }
            }
            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            FileStream file = new FileStream(TemplateFile, FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
            HSSFSheet sheet1 = hssfworkbook.GetSheet("Sheet1");


            //row,cell都是从0开始计数
            if (IsNL)
                sheet1.GetRow(6).GetCell(0).SetCellValue("FITT Tours B.V.  •" + dic["部门地址"].ToString());
            else
                sheet1.GetRow(6).GetCell(0).SetCellValue("Fujian Int. Travel Tang (FITT) • "
                    + dic["部门地址"].ToString());
            sheet1.GetRow(8).GetCell("F").SetCellValue(dic["部门名称"].ToString());
            sheet1.GetRow(8).GetCell("H").SetCellValue(dic["预订日期"].ToString());
            if (dic["所属公司"].ToString().Trim() == "")
                sheet1.GetRow(9).GetCell(0).SetCellValue(dic["客户全名"].ToString() + "\n"
                    + dic["客户地址"].ToString());
            else
                sheet1.GetRow(9).GetCell(0).SetCellValue(dic["客户全名"].ToString() + "\n"
                    + dic["所属公司"].ToString() + "\n" + dic["客户地址"].ToString());
            string TelFaxEmail = "";
            if (dic["电话"].ToString() != "")
                TelFaxEmail += "T   " + dic["电话"].ToString() + "\n";
            if (dic["传真"].ToString() != "")
                TelFaxEmail += "F   " + dic["传真"].ToString() + "\n";
            if (dic["电邮"].ToString() != "")
                TelFaxEmail += "E   " + dic["电邮"].ToString();
            sheet1.GetRow(13).GetCell(0).SetCellValue(TelFaxEmail);

            //退票时，显示客户的银行的账户，而不是自己的。
            if (dic["内部订单号"].ToString().EndsWith("GS"))
                sheet1.GetRow(11).GetCell("E").SetCellValue(dic["银行账户"].ToString());

            if (IsNL)
            {
                string strNo = dic["内部订单号"].ToString().EndsWith("GS") ? "Credit Note No." : "Booking Confirmation / Invoice No.";
                sheet1.GetRow(15).GetCell(0).SetCellValue(strNo + dic["内部订单号"].ToString());
            }
            else
            {
                string strNo = dic["内部订单号"].ToString().EndsWith("GS") ? "Gutschrift Nr." : "Rechnung Nr.";
                sheet1.GetRow(15).GetCell(0).SetCellValue(strNo + dic["内部订单号"].ToString());
            }
            sheet1.GetRow(18).GetCell(1).SetCellValue(dic["航空公司"].ToString());


            int startRow = 21;
            int TourRowCount = 0;


            //各人价格小计固定在34~38行(1页的模板),或34~68行(2页的模板)
            for (int i = 0; i < PersonList.Count; i++)
            {
                if (PersonList[i].IsShowOnInvoice)
                {
                    sheet1.Items((33 + i), 1).SetCellValue(PersonList[i].OwnerName);
                    sheet1.Items((33 + i), "F").SetCellValue((double)PersonList[i].Price);
                }
                else
                {
                    //隐藏的乘客金额加到第一个乘客金额上
                    double originalValue = sheet1.Items(33, "F").NumericCellValue;
                    sheet1.Items(33, "F").SetCellValue(originalValue + (double)PersonList[i].Price);
                }
                //显示在发票上,而且显示明细
                if (PersonList[i].IsShowOnInvoice && string.IsNullOrEmpty(PersonList[i].MergeWith))
                {
                    ArrayList MergeList = new ArrayList();
                    MergeList.Add(PersonList[i].OwnerName);
                    //是否后几个人和它合并显示, 碰到空值停止循环
                    for (int k = i + 1; k < PersonList.Count; k++)
                    {
                        if (string.IsNullOrEmpty(PersonList[k].MergeWith))
                            break;
                        else
                            if (PersonList[k].IsShowOnInvoice && PersonList[k].MergeWith == PersonList[k - 1].OwnerName)
                                MergeList.Add(PersonList[k].OwnerName);
                    }
                    //如果账单所有人的行程都一样,明细上面就不用显示人名了
                    if (MergeList.Count == PersonList.Count)
                    {
                        //显示不合并显示的乘客名,2012-3-10 现在不显示了
                        //sheet1.GetRow(startRow).GetCell(0).SetCellValue(PersonList[i].OwnerName);
                    }
                    else
                    {
                        //显示合并显示的乘客名
                        for (int m = 0; m < MergeList.Count; m++)
                        {
                            sheet1.Items(startRow, (m * 2)).SetCellValue(MergeList[m].ToString());
                        }
                    }

                    var PersonTourList = TourList.Where(t => t.OwnerName.Equals(PersonList[i].OwnerName)).ToList();
                    int Cnt = PersonTourList.Count();
                    for (int j = 0; j < Cnt; j++)
                    {
                        sheet1.Items(startRow + 1 + j, 0).SetCellValue(PersonTourList[j].FlightNum);
                        sheet1.Items(startRow + 1 + j, 1).SetCellValue(PersonTourList[j].FlightDate);
                        sheet1.Items(startRow + 1 + j, 2).SetCellValue(PersonTourList[j].FlightFrom);
                        sheet1.Items(startRow + 1 + j, 4).SetCellValue(PersonTourList[j].FlightTo);
                        sheet1.Items(startRow + 1 + j, 6).SetCellValue(PersonTourList[j].FlightStartTime);
                        sheet1.Items(startRow + 1 + j, 7).SetCellValue(PersonTourList[j].FlightEndTime);

                    }
                    TourRowCount += Cnt + 2; //名字1行,明细cnt行,间隔行1行
                    startRow += Cnt + 2;
                }

            }
            int seq = 1;
            int MaxPersonCnt = 5;
            if (PersonList.Count > 5)
                MaxPersonCnt = 35;
            for (int i = 0; i < MaxPersonCnt; i++)
            {
                if (i >= PersonList.Count || !PersonList[i].IsShowOnInvoice)
                    if (sheet1.GetRow(33 + i) != null)
                        sheet1.GetRow(33 + i).Hide(); //隐藏=zeroheight
                    else
                    {
                        sheet1.Items(33 + i, 0).SetCellValue(seq);
                        seq++;
                    }
            }
            int TwoPageAdd = 0;
            if (PersonList.Count > 5)
                TwoPageAdd = 30;
            //改票单显示Umbuchungskosten、Rebooking Charge
            if (dic["内部订单号"].ToString().EndsWith("REB"))
            {
                sheet1.Items(40 + TwoPageAdd, "C").SetCellValue(IsNL ? "Rebooking Charge" : "Umbuchungskosten");
                sheet1.Items(41 + TwoPageAdd, "C").SetCellValue("");
                sheet1.Items(43 + TwoPageAdd, "C").SetCellValue("");
                sheet1.Items(44 + TwoPageAdd, "C").SetCellValue("");
            }
            else if (dic["内部订单号"].ToString().EndsWith("GS"))
            {
                sheet1.Items(40 + TwoPageAdd, "C").SetCellValue(IsNL ? "Refund" : "Rückerstattung");
                sheet1.Items(41 + TwoPageAdd, "C").SetCellValue("");
                sheet1.Items(43 + TwoPageAdd, "C").SetCellValue("");
                sheet1.Items(44 + TwoPageAdd, "C").SetCellValue("");
                dic["付款日期"] = "";
                sheet1.Items(15, "D").SetCellValue(""); // 不显示（请注明付款发票号码）
            }
            else
            {
                sheet1.Items(40 + TwoPageAdd, "C").SetCellValue(dic["含税/火车票"].ToString());
                sheet1.Items(41 + TwoPageAdd, "C").SetCellValue(dic["最大行李重量"].ToString());
                sheet1.Items(43 + TwoPageAdd, "C").SetCellValue(dic["取消费用"].ToString());
                sheet1.Items(44 + TwoPageAdd, "C").SetCellValue(dic["改签费用"].ToString());
            }

            if (IsNL)
                sheet1.Items(46 + TwoPageAdd, "D").SetCellValue(dic["付款日期"].ToString());
            else
                sheet1.Items(46 + TwoPageAdd, "C").SetCellValue(dic["付款日期"].ToString());

            //Force excel to recalculate all the formula while open
            sheet1.ForceFormulaRecalculation = true;
            string FullFileName = AppDomain.CurrentDomain.BaseDirectory +
                "Upload\\Excel\\" + dic["内部订单号"].ToString() + "-" +
                DelFileNameIllegalChar(dic["客户全名"].ToString()) + "-" +
                dic["航空公司"].ToString().Substring(0, 2) + ".xls";

            if(dic["内部订单号"].ToString().Length>10)
                FullFileName = AppDomain.CurrentDomain.BaseDirectory +
                "Upload\\Excel\\" + dic["内部订单号"].ToString().Substring(0,10) + "-" +
                DelFileNameIllegalChar(dic["客户全名"].ToString()) + "-" +
                dic["航空公司"].ToString().Substring(0, 2) + dic["内部订单号"].ToString().Substring(10) + ".xls";
            

            file = new FileStream(FullFileName, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            DownloadFileAsAttachment(FullFileName);
        }

        private string DelFileNameIllegalChar(string FileName)
        {
            FileName = FileName.Replace("/", "");
            FileName = FileName.Replace("\\", "");
            FileName = FileName.Replace(":", "");
            FileName = FileName.Replace("*", "");
            FileName = FileName.Replace("?", "");
            FileName = FileName.Replace("\"", "");
            FileName = FileName.Replace("<", "");
            FileName = FileName.Replace(">", "");
            FileName = FileName.Replace("|", "");
            //去掉Mr、Mrs
            FileName = FileName.Replace(" MR", "");
            FileName = FileName.Replace(" MRS", "");
            return FileName;
        }

        //Click Delete Button
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                svr.SoftDeleteById(typeof(BillTicket), "ID", hidID.Value);
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
            Response.Redirect("frmTicket.aspx");
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmTicketEdit.aspx?id=0");
        }

        //cancel按钮,2012-04-06 取消、改签机票转到一个新页面，copy原来的数据，重新填写售价、成本（都是负数）
        protected void btnCANX_Click(object sender, EventArgs e)
        {
            if (btnCANX.Text == GetREMes("lblCancel"))
            {
                svr.SetStatusInactive(typeof(BillTicket), "id", hidID.Value);
                svr.AddAuditLog("status: Active=> Inactive", base.LoginUserID, long.Parse(hidID.Value));
                this.ShowMessage("订单作废");
                btnCANX.Text = GetREMes("lblRestore");
            }
            else
            {
                svr.SetStatusActive(typeof(BillTicket), "id", hidID.Value);
                svr.AddAuditLog("status: Inactive=> Active", base.LoginUserID, long.Parse(hidID.Value));
                this.ShowMessage("恢复订单成功");
                btnCANX.Text = GetREMes("lblCancel");
            }
        }

        //ddlDept.OnSelectedIndexChanged and txtBookingDate_TextChanged,ddlbilltype.OnSelectedIndexChanged
        protected void txtBookingDate_TextChanged(object sender, EventArgs e)
        {
            BindInfoByDep(ddlDept.SelectedItem.Text);
            HideFieldsWhenRefund();
            if (hidID.Value == "0") //新增
                txtInnerReferenceID.Text = svr.GetNextInnerReferenceID(
                    ddlDept.SelectedItem.Text, txtBookingDate.Text, ddlBillType.SelectedValue);
        }

        private void HideFieldsWhenRefund()
        {
            //退票改票,含税/火车票“最大行李重量”“取消费用”“改签费用” 都不显示
            if (ddlBillType.SelectedValue != "0")
            {
                txtMaxLuggage.Attributes["usage"] = "";
                lblMaxLuggage.Visible = false;
                txtMaxLuggage.Visible = false;
                lblAccessory.Visible = false;
                txtAccessory.Visible = false;
                lblCancellationFee.Visible = false;
                txtCancellationFee.Visible = false;
                lblChangeFee.Visible = false;
                txtChangeFee.Visible = false;

            }
            else
            {
                txtMaxLuggage.Attributes["usage"] = "notempty";
                lblMaxLuggage.Visible = true;
                txtMaxLuggage.Visible = true;
                lblAccessory.Visible = true;
                txtAccessory.Visible = true;
                lblCancellationFee.Visible = true;
                txtCancellationFee.Visible = true;
                lblChangeFee.Visible = true;
                txtChangeFee.Visible = true;
            }
            //退票的时候才要显示客户银行账户来填写
            if (ddlBillType.SelectedValue == "1") //退票
            {
                txtBankAccount.Attributes["usage"] = "notempty";
                txtBankAccount.Visible = true;
                lblBankAccount.Visible = true;
            }
            else
            {
                txtBankAccount.Attributes["usage"] = "";
                txtBankAccount.Visible = false;
                lblBankAccount.Visible = false;
            }

        }
        private void BindInfoByDep(string DepName)
        {
            if (DepName == "Maastricht" || DepName == "Arnhem")
            {
                txtAccessory.Items.Clear();
                txtAccessory.Items.Add("Flights including airport taxes including Rail & Fly (OW)");
                txtAccessory.Items.Add("Flight Airport taxes included, Rail & Fly for adults");
                txtAccessory.Items.Add("Flights including airport taxes and Rail & Fly");
                txtAccessory.Items.Add("Flights including airport taxes");

                txtCancellationFee.Items.Clear();
                txtCancellationFee.Items.Add("250.00 p. P. (before departure)");
                txtCancellationFee.Items.Add("300.00 p. P. (before departure)");
                txtCancellationFee.Items.Add("100.00 p. P. (before departure)");
                txtCancellationFee.Items.Add("not permitted");

                txtChangeFee.Items.Clear();
                txtChangeFee.Items.Add("150.00 p. P. (depending on availability)");
                txtChangeFee.Items.Add("130.00 p. P. (depending on availability)");
                txtChangeFee.Items.Add("110.00 p. P. (depending on availability)");
                txtChangeFee.Items.Add("not permitted");

            }
            else
            {
                txtAccessory.Items.Clear();
                txtAccessory.Items.Add("Flug inkl. Flughafensteuern inkl. Rail&Fly(OW)");
                txtAccessory.Items.Add("Flug inkl. Flughafensteuern，Rail&Fly fuer Erw");
                txtAccessory.Items.Add("Flug inkl. Flughafensteuern und Rail&Fly");
                txtAccessory.Items.Add("Flug inkl. Flughafensteuern");

                txtCancellationFee.Items.Clear();
                txtCancellationFee.Items.Add("250.00 p. P. (vor dem Abflug)");
                txtCancellationFee.Items.Add("300.00 p. P. (vor dem Abflug)");
                txtCancellationFee.Items.Add("100.00 p. P. (vor dem Abflug)");
                txtCancellationFee.Items.Add("nicht gestattet");

                txtChangeFee.Items.Clear();
                txtChangeFee.Items.Add("150.00 p. P. (je nach Verfügbarkeit)");
                txtChangeFee.Items.Add("130.00 p. P. (je nach Verfügbarkeit)");
                txtChangeFee.Items.Add("110.00 p. P. (je nach Verfügbarkeit)");
                txtChangeFee.Items.Add("nicht gestattet");

            }
        }

        #region Person

        protected void btnAddPerson_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "btnAddTenPerson")
                gvData.DataSource = BindPerson(true, 10);
            else
                gvData.DataSource = BindPerson(true, 1);
            gvData.DataBind();
        }
        private List<BillTicketPerson> BindPerson(bool AddNew)
        {
            return BindPerson(AddNew, 1);
        }
        private List<BillTicketPerson> BindPerson(bool AddNew, int AddCount)
        {
            List<BillTicketPerson> lis = new List<BillTicketPerson>();
            List<string> OwnerList = new List<string>();
            BillTicketPerson person;
            TextBox txtOwnerName, txtBankStatement;
            CheckBox chkShowOnInvoice, chkPayPartly;
            DropDownList ddlMergeWith;
            Label lblPrice;
            //保存现有的gridview 数据
            for (int i = 0; i < gvData.Rows.Count; i++)
            {
                if (gvData.Rows[i].Visible == false) continue;

                txtOwnerName = (TextBox)gvData.Rows[i].FindControl("txtOwnerName");
                OwnerList.Add(txtOwnerName.Text);
                txtBankStatement = (TextBox)gvData.Rows[i].FindControl("txtBankStatement");
                chkShowOnInvoice = (CheckBox)gvData.Rows[i].FindControl("chkShowOnInvoice");
                chkPayPartly = (CheckBox)gvData.Rows[i].FindControl("chkPayPartly");
                ddlMergeWith = (DropDownList)gvData.Rows[i].FindControl("ddlMergeWith");
                lblPrice = (Label)gvData.Rows[i].FindControl("lblPrice");
                person = new BillTicketPerson();
                person.OwnerName = txtOwnerName.Text.Trim();
                person.BankStatement = txtBankStatement.Text.Trim();
                person.IsShowOnInvoice = chkShowOnInvoice.Checked;
                person.PayNotEnough = chkPayPartly.Checked;
                person.Price = decimal.Parse(lblPrice.Text);
                if (ddlMergeWith.SelectedValue == "Prev" && i > 0)
                    person.MergeWith = OwnerList[i - 1];
                lis.Add(person);
            }
            //新增一行
            if (AddNew)
            {
                for (int i = 0; i < AddCount; i++)
                {
                    person = new BillTicketPerson();
                    person.IsShowOnInvoice = true;
                    person.PayNotEnough = false;
                    lis.Add(person);
                }

            }
            return lis;
        }

        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var Person = (BillTicketPerson)e.Row.DataItem;
                DropDownList ddlMergeWith = (DropDownList)e.Row.FindControl("ddlMergeWith");
                if (string.IsNullOrEmpty(Person.MergeWith))
                    ddlMergeWith.SelectedIndex = 0;
                else
                    ddlMergeWith.SelectedIndex = 1;


            }
        }
        #endregion

        #region Tour
        protected void btnAddTour_Click(object sender, EventArgs e)
        {
            //gvTour.DataSource = bindItem(true);
            //gvTour.DataBind();
        }
        protected void rptTour_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            GridView gvTour = e.Item.FindControl("gvTour") as GridView;
            Button btnAddTour = e.Item.FindControl("btnAddTour") as Button;
            //rptTour.ItemCommand +=new RepeaterCommandEventHandler(rptTour_ItemCommand);

            BillTicketPerson p = e.Item.DataItem as BillTicketPerson;

            var TourList = svr.getTicketTour(int.Parse(hidID.Value), p.OwnerName);
            //当账单人数多于10人，空白记录设为1条，小于10人时，设为4条
            int BlankTourCnt = 4;
            if (gvData.Rows.Count > 10)
                BlankTourCnt = 1;

            if (TourList.Count == 0)
                for (int i = 0; i < BlankTourCnt; i++)
                {
                    TourList.Add(new BillTicketTour());
                }

            gvTour.DataSource = TourList;
            gvTour.DataBind();
        }

        protected void rptTour_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                GridView gvTour = e.Item.FindControl("gvTour") as GridView;
                TextBox txtOwnerName = e.Item.FindControl("txtOwnerName") as TextBox;
                gvTour.DataSource = BindTour(txtOwnerName.Text, gvTour, true);
                gvTour.DataBind();
                //rptTour.DataSource = svr.getTicketPerson(int.Parse(hidID.Value));
                //rptTour.DataBind();
            }
        }

        private IList<BillTicketTour> BindTour(string OwnerName, GridView gvTour, bool AddNew)
        {
            List<BillTicketTour> lis = new List<BillTicketTour>();

            BillTicketTour tour;
            TextBox txtFlightNum, txtFlightDate, txtFlightFrom,
                    txtFlightTo, txtFlightStartTime, txtFlightEndTime,
                    txtFlightTicketNum, txtOuterReferenceID, txtPrice, txtCost;
            //保存现有的gridview 数据
            for (int i = 0; i < gvTour.Rows.Count; i++)
            {
                if (gvTour.Rows[i].Visible == false) continue;

                txtFlightNum = (TextBox)gvTour.Rows[i].FindControl("txtFlightNum");
                txtFlightDate = (TextBox)gvTour.Rows[i].FindControl("txtFlightDate");
                txtFlightFrom = (TextBox)gvTour.Rows[i].FindControl("txtFlightFrom");
                txtFlightTo = (TextBox)gvTour.Rows[i].FindControl("txtFlightTo");
                txtFlightStartTime = (TextBox)gvTour.Rows[i].FindControl("txtFlightStartTime");
                txtFlightEndTime = (TextBox)gvTour.Rows[i].FindControl("txtFlightEndTime");
                txtFlightTicketNum = (TextBox)gvTour.Rows[i].FindControl("txtFlightTicketNum");
                txtOuterReferenceID = (TextBox)gvTour.Rows[i].FindControl("txtOuterReferenceID");
                txtPrice = (TextBox)gvTour.Rows[i].FindControl("txtPrice");
                txtCost = (TextBox)gvTour.Rows[i].FindControl("txtCost");

                tour = new BillTicketTour();
                tour.OwnerName = OwnerName;
                tour.FlightNum = txtFlightNum.Text.Trim();
                tour.FlightDate = txtFlightDate.Text.Trim();
                tour.FlightFrom = txtFlightFrom.Text.Trim();
                tour.FlightTo = txtFlightTo.Text.Trim();
                tour.FlightStartTime = txtFlightStartTime.Text.Trim();
                tour.FlightEndTime = txtFlightEndTime.Text.Trim();
                tour.FlightTicketNum = txtFlightTicketNum.Text.Trim();
                tour.OuterReferenceID = txtOuterReferenceID.Text.Trim();
                tour.Price = decimal.Parse(txtPrice.Text.Trim());
                if (txtCost.Text != "")
                    tour.Cost = decimal.Parse(txtCost.Text.Trim());
                lis.Add(tour);
            }
            //新增一行
            if (AddNew)
            {
                tour = new BillTicketTour();
                tour.OwnerName = OwnerName;
                lis.Add(tour);
            }
            return lis;
        }

        private IList<BillTicketTour> GetAllPersonTours()
        {
            List<BillTicketTour> lis = new List<BillTicketTour>();
            for (int i = 0; i < rptTour.Items.Count; i++)
            {
                GridView gvTour = rptTour.Items[i].FindControl("gvTour") as GridView;
                TextBox txtOwnerName = rptTour.Items[i].FindControl("txtOwnerName") as TextBox;
                lis.AddRange(BindTour(txtOwnerName.Text, gvTour, false));
            }
            return lis;
        }
        #endregion
    }
}