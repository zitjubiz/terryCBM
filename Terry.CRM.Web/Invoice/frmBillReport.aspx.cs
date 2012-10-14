using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Terry.CRM.Entity;
using Terry.CRM.Service;
using Terry.CRM.Web.CommonUtil;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;

namespace Terry.CRM.Web.Invoice
{
    /// <summary>
    /// NPOI CreateRow 没有边框, GetRow只能拿到模板已经定义的行,未定义的为null
    /// </summary>
    public partial class frmBillReport : BasePage
    {
        VisaService Visa = new VisaService();
        BillingService Ticket = new BillingService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Authentication(enumModule.Report);

                ddlYear.Text = DateTime.Now.Year.ToString();
                ddlMonth.Text = DateTime.Now.Month.ToString();

                ddlYear1.Text = DateTime.Now.Year.ToString();
                ddlMonth1.Text = DateTime.Now.Month.ToString();
                ddlDept2.BindDropDownListAndSelect(Ticket.GetDepartment(), "DepName", "DepId");
                ddlYear2.Text = DateTime.Now.Year.ToString();
                ddlMonth2.Text = DateTime.Now.Month.ToString();
            }

        }

        //月份送签流水表
        private void Visum(int Year, int Month)
        {
            DataTable dt = Visa.GetVisum(Year, Month);

            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Visum_Template.xls", FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
            HSSFSheet sheet1 = hssfworkbook.GetSheet("Sheet1");
            //row,cell都是从0开始计数
            //第1行title,不是数据       

            HSSFCellStyle DateTimecellStyle = hssfworkbook.CreateCellStyle();
            //- 细边缘
            DateTimecellStyle.BorderBottom = HSSFCellStyle.BORDER_THIN;
            DateTimecellStyle.BorderLeft = HSSFCellStyle.BORDER_THIN;
            DateTimecellStyle.BorderRight = HSSFCellStyle.BORDER_THIN;
            DateTimecellStyle.BorderTop = HSSFCellStyle.BORDER_THIN;
            //根据excel 单元格的自定义格式 http://www.cnblogs.com/405464904/archive/2011/11/09/2242608.html
            HSSFDataFormat format = hssfworkbook.CreateDataFormat();
            DateTimecellStyle.DataFormat = format.GetFormat("yyyy/m/d");

            HSSFCellStyle cellStyle = hssfworkbook.CreateCellStyle();
            //- 细边缘
            cellStyle.BorderBottom = HSSFCellStyle.BORDER_THIN;
            cellStyle.BorderLeft = HSSFCellStyle.BORDER_THIN;
            cellStyle.BorderRight = HSSFCellStyle.BORDER_THIN;
            cellStyle.BorderTop = HSSFCellStyle.BORDER_THIN;
 
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    HSSFCell cell = sheet1.CreateRow(i + 1).CreateCell(j);


                    if (j == 8 || j == 11)
                    {
                        cell.CellStyle = cellStyle;
                        cell.SetCellValue(double.Parse(dr[j].ToString()));
                    }
                    else if (j == 4 || j == 5 || j == 6 || j == 13 || j == 15)
                    {
                        cell.CellStyle = DateTimecellStyle;
                        if (dr[j].ToString() != "")
                            cell.SetCellValue(DateTime.Parse(dr[j].ToString()));
                    }
                    else
                    {
                        cell.CellStyle = cellStyle;
                        cell.SetCellValue(dr[j].ToString());
                    }

                    
                }

            }
            //Excel文件在被打开的时候自动将焦点定位在单元格
            sheet1.GetRow(0).GetCell(0).SetAsActiveCell();

            //Force excel to recalculate all the formula while open
            sheet1.ForceFormulaRecalculation = true;
            string FullFileName = AppDomain.CurrentDomain.BaseDirectory + "Upload\\Excel\\Visum_" + Year.ToString() + Month.ToString("00") + ".xls";
            file = new FileStream(FullFileName, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            DownloadFileAsAttachment(FullFileName);

        }

        //德国签证会计总账（28开头,51,52,53）
        private void VisumAccount(int Year, int Month)
        {
            DataTable dt = Visa.GetVisumAccount(Year, Month,"28");
            DataTable dt51 = Visa.GetVisumAccount(Year, Month,"51");
            DataTable dt52 = Visa.GetVisumAccount(Year, Month,"52");
            DataTable dt53 = Visa.GetVisumAccount(Year, Month,"53");
            DataTable dt55 = Visa.GetVisumAccount(Year, Month, "55");
            dt.Merge(dt51);
            dt.Merge(dt52);
            dt.Merge(dt53);
            dt.Merge(dt55);

            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Visum_Account_Template.xls", FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
            HSSFSheet sheet1 = hssfworkbook.GetSheet("Sheet1");
            //row,cell都是从0开始计数
            //第1,2行title,不是数据
            sheet1.Items(0,0).SetCellValue("Fitt 2012 - Rechnungsausgangsbuch D´dorf");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                int j = (i * 2) + 2;//每条数据占2行
                sheet1.Items(j,"C").SetCellValue(dr["InnerReferenceID"].ToString());
                if (!string.IsNullOrEmpty(dr["ApplyDate"].ToString()))
                {
                    DateTime ApplyDate = (DateTime)dr["ApplyDate"];
                    sheet1.Items(j,"D").SetCellValue(ApplyDate);
                }

                //如果签证账单内容里填写了公司信息，直接显示公司的名字来替代客人的名字.
                //当然，在签证账单里没有填写公司信息的时候，还是使用客人的姓名填写到会计汇总表"客户信息"这栏
                if (string.IsNullOrEmpty(dr["ParentCompany"].ToString()))
                    sheet1.Items(j,"E").SetCellValue(dr["VisaName"].ToString()); //customer name
                else
                    sheet1.Items(j,"E").SetCellValue(dr["ParentCompany"].ToString()); //company name

                sheet1.Items(j,"F").SetCellValue("Netto Betrag");
                sheet1.Items(j + 1,"F").SetCellValue("Service Charge inkl. 19% Mwst");
                sheet1.Items(j,"G").SetCellValue(double.Parse(dr["TotalAmount"].ToString()) - double.Parse(dr["ServiceFee"].ToString()));
                sheet1.Items(j + 1,"G").SetCellValue(double.Parse(dr["ServiceFee"].ToString()));
                if (dr["Status"].ToString() == "A")
                {
                    sheet1.Items(j,"H").SetCellValue(double.Parse(dr["TotalAmount"].ToString()));
                    if (!string.IsNullOrEmpty(dr["PayDate"].ToString()))
                    {
                        DateTime PayDate = (DateTime)dr["PayDate"];
                        sheet1.Items(j,"I").SetCellValue(PayDate);
                    }

                    string PayMethod = "";
                    switch (dr["PayMethod"].ToString())
                    {
                        case "By PickUp(Cash)":
                            PayMethod = "kasse-dus";
                            break;
                        case "Paid(Cash)":
                            PayMethod = "kasse-str";
                            break;
                        case "Bank":
                            PayMethod = "bank";
                            break;
                        default:
                            break;
                    }
                    sheet1.Items(j,"J").SetCellValue(PayMethod);
                }
                else
                {
                    sheet1.Items(j,"H").SetCellValue("");
                    sheet1.Items(j,"I").SetCellValue("CANX");
                    sheet1.Items(j,"J").SetCellValue("");

                    HSSFCellStyle style = hssfworkbook.CreateCellStyle();
                    style.FillPattern = HSSFCellStyle.SOLID_FOREGROUND;
                    style.FillForegroundColor = HSSFColor.ORANGE.index;
                    //- 细边缘
                    style.BorderBottom = HSSFCellStyle.BORDER_THIN;
                    style.BorderLeft = HSSFCellStyle.BORDER_THIN;
                    style.BorderRight = HSSFCellStyle.BORDER_THIN;
                    style.BorderTop = HSSFCellStyle.BORDER_THIN;

                    sheet1.Items(j,"H").CellStyle = style;
                    sheet1.Items(j,"I").CellStyle = style;
                    sheet1.Items(j,"J").CellStyle = style;

                }

                if (!string.IsNullOrEmpty(dr["BookingDate"].ToString()))
                {
                    DateTime BookingDate = (DateTime)dr["BookingDate"];
                    sheet1.Items(j,"N").SetCellValue(BookingDate);
                }
                sheet1.Items(j,"O").SetCellValue(dr["remark"].ToString());

            }

            //Excel文件在被打开的时候自动将焦点定位在单元格
            sheet1.GetRow(0).GetCell(0).SetAsActiveCell();

            //Force excel to recalculate all the formula while open
            sheet1.ForceFormulaRecalculation = true;
            string FullFileName = AppDomain.CurrentDomain.BaseDirectory + "Upload\\Excel\\Visum_Account" + Year.ToString() + Month.ToString("00") + ".xls";
            file = new FileStream(FullFileName, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            DownloadFileAsAttachment(FullFileName);

        }

        //荷兰签证会计总账（68开头）
        private void VisumAccountNL(int Year, int Month)
        {
            DataTable dt = Visa.GetVisumAccount(Year, Month, "68");

            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Visum_Account_Template_nl.xls", FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
            HSSFSheet sheet1 = hssfworkbook.GetSheet("Sheet1");
            //row,cell都是从0开始计数
            //第1,2行title,不是数据
            sheet1.GetRow(0).GetCell(0).SetCellValue("Fitt 2012 - accounting ledger Maastricht");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                int j = (i * 2) + 2;//每条数据占2行
                sheet1.GetRow(j).GetCell("C").SetCellValue(dr["InnerReferenceID"].ToString());
                if (!string.IsNullOrEmpty(dr["BookingDate"].ToString()))
                {
                    DateTime BookingDate = (DateTime)dr["BookingDate"];
                    sheet1.GetRow(j).GetCell("D").SetCellValue(BookingDate);
                }
                //如果签证账单内容里填写了公司信息，直接显示公司的名字来替代客人的名字.
                //当然，在签证账单里没有填写公司信息的时候，还是使用客人的姓名填写到会计汇总表"客户信息"这栏
                if(string.IsNullOrEmpty(dr["ParentCompany"].ToString()))
                    sheet1.GetRow(j).GetCell("E").SetCellValue(dr["VisaName"].ToString()); //customer name
                else
                    sheet1.GetRow(j).GetCell("E").SetCellValue(dr["ParentCompany"].ToString()); //company name
                double TotalAmount, VisaCenterFee, FittServiceFee;
                TotalAmount = double.Parse(dr[3].ToString());
                VisaCenterFee = double.Parse(dr[4].ToString());
                FittServiceFee = double.Parse(dr[5].ToString());

                sheet1.GetRow(j).GetCell("F").SetCellValue(TotalAmount); 
                //Embassy Fees, 0%VAT
                sheet1.GetRow(j).GetCell("G").SetCellValue(TotalAmount - VisaCenterFee - FittServiceFee);
                
                //Fees of Visa Center (NET)	
                sheet1.GetRow(j).GetCell("H").SetCellValue(VisaCenterFee/1.19); //total amount

                //Fees of Visa Center (VAT) 19%	
                sheet1.GetRow(j).GetCell("I").SetCellValue(VisaCenterFee -VisaCenterFee / 1.19); 
                
                //Fitt Service Charge (NET)	
                sheet1.GetRow(j).GetCell("J").SetCellValue(FittServiceFee / 1.19); //total amount

                //Fitt Service Charge(VAT) 19%
                sheet1.GetRow(j).GetCell("K").SetCellValue(FittServiceFee - FittServiceFee / 1.19);


                if (dr["Status"].ToString() == "A")
                {
                    if (!string.IsNullOrEmpty(dr["PayDate"].ToString()))
                    {
                        DateTime PayDate = (DateTime)dr["PayDate"];
                        sheet1.GetRow(j).GetCell("L").SetCellValue(PayDate);
                    }
                    sheet1.GetRow(j).GetCell("M").SetCellValue(dr["PayMethod"].ToString());
                }
                else
                {
                    sheet1.GetRow(j).GetCell("L").SetCellValue("CANX");

                    HSSFCellStyle style = hssfworkbook.CreateCellStyle();
                    style.FillPattern = HSSFCellStyle.SOLID_FOREGROUND;
                    style.FillForegroundColor = HSSFColor.ORANGE.index;
                    //- 细边缘
                    style.BorderBottom = HSSFCellStyle.BORDER_THIN;
                    style.BorderLeft = HSSFCellStyle.BORDER_THIN;
                    style.BorderRight = HSSFCellStyle.BORDER_THIN;
                    style.BorderTop = HSSFCellStyle.BORDER_THIN;

                    sheet1.GetRow(j).GetCell("L").CellStyle = style;
                }
                sheet1.GetRow(j).GetCell("N").SetCellValue(dr["remark"].ToString());

            }
            for (int i = dt.Rows.Count*2 + 2; i < 184; i++)
            {
                sheet1.GetRow(i).Hide();

            }

            //Excel文件在被打开的时候自动将焦点定位在单元格
            sheet1.GetRow(0).GetCell(0).SetAsActiveCell();

            //Force excel to recalculate all the formula while open
            sheet1.ForceFormulaRecalculation = true;
            string FullFileName = AppDomain.CurrentDomain.BaseDirectory + "Upload\\Excel\\Visum_Account" + Year.ToString() + Month.ToString("00") + ".xls";
            file = new FileStream(FullFileName, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            DownloadFileAsAttachment(FullFileName);
        }
        //机票会计总账(DepCode=订单号开头2个数字21,22,23,25)
        private void TicketAccount(int Year, int Month, int DepCode)
        {

            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Ticket_Account_Template.xls", FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
            HSSFSheet sheet1 = hssfworkbook.GetSheet("Sheet1");
            DataTable dt;
            int[] arrDep = { 21, 22, 23, 25 };
            int startRow = 0;
            if (DepCode == 0)
            {
                foreach (int iDep in arrDep)
                {
                    dt = Ticket.GetTicketAccountingReport(Year, Month, iDep);
                    FillExcelAccount(sheet1, dt, startRow);
                    startRow += dt.Rows.Count + 1;

                }

            }
            else
            {
                dt = Ticket.GetTicketAccountingReport(Year, Month, DepCode);
                FillExcelAccount(sheet1, dt, startRow);
            }

            //Excel文件在被打开的时候自动将焦点定位在单元格
            sheet1.GetRow(0).GetCell(0).SetAsActiveCell();

            //Force excel to recalculate all the formula while open
            sheet1.ForceFormulaRecalculation = true;
            string FullFileName = AppDomain.CurrentDomain.BaseDirectory + "Upload\\Excel\\Ticket_Account_" + Year.ToString() + Month.ToString("00") + ".xls";
            file = new FileStream(FullFileName, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            DownloadFileAsAttachment(FullFileName);

        }
        private void FillExcelAccount(HSSFSheet sheet1, DataTable dt, int startRow)
        {
            //row,cell都是从0开始计数
            //第1行title,不是数据       
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    HSSFCell cell = sheet1.Items((startRow + i + 1), j);
                    if (j == 4)
                        cell.SetCellValue(double.Parse(dr[j].ToString()));
                    else if (j == 1)
                    {
                        DateTime dtBooking;
                        if (DateTime.TryParse(dr[j].ToString(), out dtBooking))
                            cell.SetCellValue(dtBooking);
                        else
                            cell.SetCellValue(dr[j].ToString());
                    }
                    else
                        cell.SetCellValue(dr[j].ToString());
                }

            }

        }

        //每日出票记录(不按部门分组)
        private void DailyIssue(int Year, int Month, int DepCode)
        {
            int MaxDay = 30;
            if (Month == 1 || Month == 3 || Month == 5 || Month == 7 || Month == 8 || Month == 10 || Month == 12)
                MaxDay = 31;
            if (Month == 2)  //要考虑闰年,2月是29天
            {
                if ((Year % 400 == 0) || (Year % 100 != 0) && (Year % 4 == 0))
                    MaxDay = 29;
                else
                    MaxDay = 28;
            }

            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Invoice\\DailyIssue.xls", FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
            HSSFSheet sheet1 = hssfworkbook.GetSheet("Sheet1");
            //row,cell都是从0开始计数
            //第1行title,不是数据    
            int dayIssueCnt = 0;
            DataTable dt;

            if (DepCode == 0)
                dt = Ticket.GetIssue(Year, Month);
            else
                dt = Ticket.GetIssue(Year, Month, DepCode);

            for (int day = 1; day <= MaxDay; day++)
            {
                HSSFCell cell = sheet1.CreateRow(dayIssueCnt + 1).CreateCell(0);
                HSSFCellStyle cellStyle = hssfworkbook.CreateCellStyle();
                //根据excel 单元格的自定义格式
                cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("d-mmm");
                cell.CellStyle = cellStyle;
                cell.SetCellValue(new DateTime(Year, Month, day));

                DataRow[] drs = dt.Select("IssueDate='" + new DateTime(Year, Month, day).ToString() + "'");

                for (int i = 0; i < drs.Length; i++)
                {
                    DataRow dr = drs[i];
                    for (int j = 2; j < dt.Columns.Count; j++)
                    {
                        if (j == 4)
                            sheet1.CreateRow(dayIssueCnt + 2 + i).CreateCell(j - 2).SetCellValue(double.Parse(dr[j].ToString()));
                        else
                            sheet1.CreateRow(dayIssueCnt + 2 + i).CreateCell(j - 2).SetCellValue(dr[j].ToString());
                    }

                }
                dayIssueCnt += drs.Length + 4;

            }

            //Excel文件在被打开的时候自动将焦点定位在单元格
            sheet1.GetRow(0).GetCell(0).SetAsActiveCell();

            //Force excel to recalculate all the formula while open
            sheet1.ForceFormulaRecalculation = true;
            string FullFileName = AppDomain.CurrentDomain.BaseDirectory + "Upload\\Excel\\DailyIssue_" + Year.ToString() + Month.ToString("00") + ".xls";
            file = new FileStream(FullFileName, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            DownloadFileAsAttachment(FullFileName);
        }

        //每月机票明细
        private void MonthlyDetail(int Year, int Month, int DepCode)
        {

            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Invoice\\Ticket_Monthly_Template.xls", FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
            HSSFSheet sheet1 = hssfworkbook.GetSheetAt(0);

            DataTable dt;
            int[] arrDep = { 21, 22, 23, 24, 25 };
            int startRow = 0;
            if (DepCode == 0)
            {
                foreach (int iDep in arrDep)
                {
                    dt = Ticket.GetTicketReportDetail(Year, Month, iDep);
                    FillExcelMonthly(sheet1, dt, startRow);
                    startRow += dt.Rows.Count + 1;
                    //set break line.............
                    if (dt.Rows.Count > 0)
                    {
                        HSSFCellStyle style = hssfworkbook.CreateCellStyle();
                        style.FillPattern = HSSFCellStyle.SOLID_FOREGROUND;
                        style.FillForegroundColor = HSSFColor.BRIGHT_GREEN.index;
                        for (int j = 0; j < 10; j++)
                        {
                            sheet1.Items((startRow), j).CellStyle = style;
                        }
                    }
                }

            }
            else
            {
                dt = Ticket.GetTicketReportDetail(Year, Month, DepCode);
                FillExcelMonthly(sheet1, dt, startRow);
            }

            //Excel文件在被打开的时候自动将焦点定位在单元格
            sheet1.GetRow(0).GetCell(0).SetAsActiveCell();

            //Force excel to recalculate all the formula while open
            sheet1.ForceFormulaRecalculation = true;
            string FullFileName = AppDomain.CurrentDomain.BaseDirectory + "Upload\\Excel\\Ticket_Monthly_" + Year.ToString() + Month.ToString("00") + ".xls";
            file = new FileStream(FullFileName, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            DownloadFileAsAttachment(FullFileName);
        }
        private void FillExcelMonthly(HSSFSheet sheet1, DataTable dt, int startRow)
        {
            //row,cell都是从0开始计数
            //第1行title,不是数据       
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                //最后一列bookingDate 不显示
                for (int j = 0; j < dt.Columns.Count - 1; j++)
                {

                    HSSFCell cell = sheet1.Items((startRow + i + 1), j);
                    //HSSFCellStyle cellStyle = hssfworkbook.CreateCellStyle();
                    if (j == 2 || j == 3 || j == 5)
                    {
                        if (!string.IsNullOrEmpty(dr[j].ToString()))
                            cell.SetCellValue(double.Parse(dr[j].ToString()));
                    }
                    else
                        cell.SetCellValue(dr[j].ToString());

                    //cell.CellStyle = cellStyle;
                }

            }

        }

        /// <summary>
        /// 列出sales 每个月的业绩(以收到钱为准),并和上一个月比较
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Month">每2个月的后面那个月</param>
        private void SalesByPerson(int Year, int Month)
        {
            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Invoice\\SalesByPerson_Template.xls", FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
            HSSFSheet sheet1 = hssfworkbook.GetSheetAt(0);
            
            int Cnt = Ticket.GetTicketCountByMonth(Year, Month);
            int PrevCnt;
            DataTable dt, dtPrevYear=new DataTable();
            //现在Month参数没有用到,一次把每年12个月的数值都取出来
            dt = Ticket.GetMonthlyProfit(Year);
            //如果是1月份,要把去年12月份的数据取出来
            if (Month == 1)
            {
                dtPrevYear = Ticket.GetMonthlyProfit(Year - 1);
                PrevCnt = Ticket.GetTicketCountByMonth(Year-1, 12);
            }
            else
                PrevCnt = Ticket.GetTicketCountByMonth(Year, Month-1);

            FillExcelPersonProfit(sheet1, dt, dtPrevYear,Month, ddlDept2.Text,Cnt,PrevCnt);


            //Force excel to recalculate all the formula while open
            sheet1.ForceFormulaRecalculation = true;
            string FullFileName = AppDomain.CurrentDomain.BaseDirectory + "Upload\\Excel\\SalesByPerson_" + Year.ToString() +"_"+ Month.ToString("00") + ".xls";
            file = new FileStream(FullFileName, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            DownloadFileAsAttachment(FullFileName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet1"></param>
        /// <param name="dt"></param>
        /// <param name="dtPrevYear"></param>
        /// <param name="CurrentMonth"></param>
        /// <param name="DepId"></param>
        private void FillExcelPersonProfit(HSSFSheet sheet1, DataTable dt, DataTable dtPrevYear,
            int CurrentMonth, string DepId,int Cnt, int PrevCnt)
        {
            switch (CurrentMonth)
            {
                case 1:
                    sheet1.Items(0, 1).SetCellValue("Dec");
                    sheet1.Items(0, 2).SetCellValue("Jan");
                    sheet1.Items(0, 3).SetCellValue("Total(Dec+Jan)");
                    break;
                case 2:
                    sheet1.Items(0, 1).SetCellValue("Jan");
                    sheet1.Items(0, 2).SetCellValue("Feb");
                    sheet1.Items(0, 3).SetCellValue("Total(Jan+Feb)");
                    break;
                case 3:
                    sheet1.Items(0, 1).SetCellValue("Feb");
                    sheet1.Items(0, 2).SetCellValue("Mar");
                    sheet1.Items(0, 3).SetCellValue("Total(Feb+Mar)");
                    break;
                case 4:
                    sheet1.Items(0, 1).SetCellValue("Mar");
                    sheet1.Items(0, 2).SetCellValue("Apr");
                    sheet1.Items(0, 3).SetCellValue("Total(Mar+Apr)");
                    break;
                case 5:
                    sheet1.Items(0, 1).SetCellValue("Apr");
                    sheet1.Items(0, 2).SetCellValue("May");
                    sheet1.Items(0, 3).SetCellValue("Total(Apr+May)");
                    break;
                case 6:
                    sheet1.Items(0, 1).SetCellValue("May");
                    sheet1.Items(0, 2).SetCellValue("Jun");
                    sheet1.Items(0, 3).SetCellValue("Total(May+Jun)");
                    break;
                case 7:
                    sheet1.Items(0, 1).SetCellValue("Jun");
                    sheet1.Items(0, 2).SetCellValue("Jul");
                    sheet1.Items(0, 3).SetCellValue("Total(Jun+Jul)");
                    break;
                case 8:
                    sheet1.Items(0, 1).SetCellValue("Jul");
                    sheet1.Items(0, 2).SetCellValue("Aug");
                    sheet1.Items(0, 3).SetCellValue("Total(Jul+Aug)");
                    break;
                case 9:
                    sheet1.Items(0, 1).SetCellValue("Aug");
                    sheet1.Items(0, 2).SetCellValue("Sep");
                    sheet1.Items(0, 3).SetCellValue("Total(Aug+Sep)");
                    break;
                case 10:
                    sheet1.Items(0, 1).SetCellValue("Sep");
                    sheet1.Items(0, 2).SetCellValue("Oct");
                    sheet1.Items(0, 3).SetCellValue("Total(Sep+Oct)");
                    break;
                case 11:
                    sheet1.Items(0, 1).SetCellValue("Oct");
                    sheet1.Items(0, 2).SetCellValue("Nov");
                    sheet1.Items(0, 3).SetCellValue("Total(Oct+Nov)");
                    break;
                case 12:
                    sheet1.Items(0, 1).SetCellValue("Nov");
                    sheet1.Items(0, 2).SetCellValue("Dec");
                    sheet1.Items(0, 3).SetCellValue("Total(Nov+Dec)");
                    break;
                default:
                    break;
            }
            //是否按部门过滤数据
            DataRow[] DRS =new DataRow[dt.Rows.Count];
            DataRow[] DRSPrevYear = new DataRow[dtPrevYear.Rows.Count];
            if (DepId != "")
            {
                DRS = dt.Select("DepId=" + DepId);
                //只有1月份的时候，才有去年的数值
                if(dtPrevYear.Rows.Count>0)
                    DRSPrevYear = dtPrevYear.Select("DepId=" + DepId);
            }
            else
            {
                dt.Rows.CopyTo(DRS, 0);
                dtPrevYear.Rows.CopyTo(DRSPrevYear, 0);
            }

            //row,cell都是从0开始计数
            //第1行title,不是数据
            for (int i = 0; i < DRS.Length; i++)  //每个员工
            {
                DataRow dr = DRS[i];
                HSSFCell cell = sheet1.Items((i + 1), 0);
                cell.SetCellValue(dr[1].ToString()); //员工名字
                //dr[5~16]的值是该员工1-12月的profit 1=>5,3=>7,5=>9,7=>11,9=>13,11=>15
                double monthProfit1, monthProfit2;
                if (CurrentMonth == 1)
                {
                    monthProfit1 = double.Parse(DRSPrevYear[i][12+4].ToString());
                    monthProfit2 = double.Parse(dr[CurrentMonth + 4].ToString());               
                }
                else
                {
                    monthProfit1 = double.Parse(dr[CurrentMonth + 3].ToString());
                    monthProfit2 = double.Parse(dr[CurrentMonth + 4].ToString());
                }

                sheet1.Items((i + 1), 1).SetCellValue(monthProfit1);
                sheet1.Items((i + 1), 2).SetCellValue(monthProfit2);
            }
            //模板员工行数现在是23行,假如超过就要再加
            for (int i = DRS.Length+1; i < 23; i++)
            {
                sheet1.GetRow(i).Hide();
            }
            //28B,28C是出票数量
            sheet1.Items(27, 1).SetCellValue(PrevCnt);
            sheet1.Items(27, 2).SetCellValue(Cnt);
        }

        protected void btnVisumAccount_Click(object sender, EventArgs e)
        {
            VisumAccount(int.Parse(ddlYear.Text), int.Parse(ddlMonth.Text));
        }
        protected void btnVisumAccountNL_Click(object sender, EventArgs e)
        {
            VisumAccountNL(int.Parse(ddlYear.Text), int.Parse(ddlMonth.Text));
        }

        protected void btnVisum_Click(object sender, EventArgs e)
        {
            Visum(int.Parse(ddlYear.Text), int.Parse(ddlMonth.Text));
        }

        protected void btnTicketAccount_Click(object sender, EventArgs e)
        {
            int DepCode;
            DepCode = int.Parse(ddlDept.Text);
            TicketAccount(int.Parse(ddlYear1.Text), int.Parse(ddlMonth1.Text), DepCode);
        }

        protected void btnDailyIssue_Click(object sender, EventArgs e)
        {
            int DepCode;
            DepCode = int.Parse(ddlDept.Text);
            DailyIssue(int.Parse(ddlYear1.Text), int.Parse(ddlMonth1.Text), DepCode);
        }

        protected void btnMonthReport_Click(object sender, EventArgs e)
        {
            int DepCode;
            DepCode = int.Parse(ddlDept.Text);
            MonthlyDetail(int.Parse(ddlYear1.Text), int.Parse(ddlMonth1.Text), DepCode);
        }

        protected void btnSalesByPerson_Click(object sender, EventArgs e)
        {
            SalesByPerson(int.Parse(ddlYear2.Text), int.Parse(ddlMonth2.Text));
        }



    }
}