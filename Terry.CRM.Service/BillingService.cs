using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using Terry.CRM.Entity;
using System.Data.Common;
using System.Data;
using System.Data.Linq;

namespace Terry.CRM.Service
{
    public class BillingService : BaseService
    {
        public IList<BillTicket> SearchByCriteria(int CurrentPage, int PageSize, out int RecordCount, string Filter, string OrderBy)
        {
            if (OrderBy == "") OrderBy = "ID";
            var qry = from t in BillTickets
                      .Where(Filter)
                      .OrderBy(OrderBy)
                      select t;

            RecordCount = qry.Count();

            if (CurrentPage == -1 || PageSize == -1)
                return qry.ToList();
            else
            {
                return qry.Skip(CurrentPage * PageSize).Take(PageSize).ToList();
            }


        }

        public BillTicket LoadById(object Id)
        {
            long lngID = long.Parse((string)Id);
            var qry = from t in BillTickets
                      where t.ID == lngID
                      select t;
            return qry.SingleOrDefault();
        }

        //没有银行对账记录,或者没有全部付款,都返回false
        public bool IsPaid(object Id)
        {
            string sql;
            sql = @"select count(ID) from BillTicketPerson 
            where TicketID=" + Id.ToString() + " and ( isnull(BankStatement,'')='' or PayNotEnough=1) ";
            int cnt = (int)DBExtBase.ExeScalarBySqlText(this.dataCtx, sql);
            if (cnt > 0)
                return false;
            else
                return true;

        }

        //每月出票数量
        public int GetTicketCountByMonth(int Year, int Month)
        {
            var TicketCnt= DBExtBase.ExeScalarBySqlText(this.dataCtx, @"
            SELECT   COUNT(FlightTicketNum) AS TicketCnt FROM vw_BillTicketReportDetail
            WHERE   (YEAR(BookingDate) = " + Year.ToString() + ") AND (MONTH(BookingDate) = "
                                           + Month.ToString()+")");
            return (int)TicketCnt;
        }
        //每月出票记录,按天分开
        public DataTable GetIssue(int Year, int Month, params int[] DepCode)
        {
            string sql;
            sql = @"select * from BillDailyIssue where year(IssueDate)=" + Year.ToString()
                + " and month(IssueDate)=" + Month.ToString();
            if (DepCode.Length > 0)
                sql += " and left(InnerReferenceID,2)=" + DepCode[0].ToString();
            
            sql += " order by IssueDate,InnerReferenceID";
            DataTable dt = DBExtBase.ExeFillTblBySqlText(this.dataCtx, sql);
            return dt;
        }
        //每月会计记录,现在不按bookingDate，按账单号分月
        public DataTable GetTicketAccountingReport(int Year, int Month, int DepCode)
        {
            string sql;
            sql = @"select * from vw_BillTicketAccount where SUBSTRING(InnerReferenceID, 4, 2)='" + Year.ToString().Substring(2)
                + "' and SUBSTRING(InnerReferenceID, 6, 2)='" + Month.ToString("00") +
                "' and left(InnerReferenceID,2)=" + DepCode.ToString() +
                " order by BookingDate,InnerReferenceID";
            DataTable dt = DBExtBase.ExeFillTblBySqlText(this.dataCtx, sql);
            return dt;
        }
        //每月明细,现在不按bookingDate，按账单号分月
        public DataTable GetTicketReportDetail(int Year, int Month, int DepCode)
        {
            string sql;
            sql = @"select * from vw_BillTicketReportDetail where SUBSTRING(InnerReferenceID, 4, 2)='" + Year.ToString().Substring(2)
                + "' and SUBSTRING(InnerReferenceID, 6, 2)='" + Month.ToString("00") +
                "' and left(InnerReferenceID,2)=" + DepCode.ToString() +
                " order by BookingDate,InnerReferenceID";
            //sql = @"select * from vw_BillTicketReportDetail where year(BookingDate)=" + Year.ToString()
            //    + " and month(BookingDate)=" + Month.ToString() +
            //    " and left(InnerReferenceID,2)=" + DepCode.ToString() +
            //    " order by BookingDate,InnerReferenceID";
            DataTable dt = DBExtBase.ExeFillTblBySqlText(this.dataCtx, sql);
            return dt;
        }
        //每个员工的业绩
        public DataTable GetMonthlyProfit(int Year)
        {
            DataTable dt=DBExtBase.ExeFillTblBySqlText(this.dataCtx, "exec usp_GetMonthlyProfit "+ Year.ToString());
            return dt;
        }
        
        public void SaveIssue(IList<BillDailyIssue> IssueList)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                foreach (var entity in IssueList)
                {
                    //查询是否存在db
                    var qry = from t in BillDailyIssues
                              where t.IssueDate == entity.IssueDate &&
                              t.FlightTicketNum == entity.FlightTicketNum
                              select t;
                    var obj = qry.SingleOrDefault();
                    if (obj != null)
                    {
                        this.CopyEntity(obj, entity);
                    }
                    else
                        this.BillDailyIssues.InsertOnSubmit(entity);
                }


                this.dataCtx.SubmitChanges();
                //update billticketperson & billtickettour
                foreach (var entity in IssueList)
                {
                    var qry = from t in BillTickets
                              where t.InnerReferenceID == entity.InnerReferenceID
                              select t.ID;
                    var TicketID = qry.SingleOrDefault();
                    if (TicketID != null)
                    {
                        var person = BillTicketPersons
                            .Where(t => t.OwnerName == entity.OwnerName &&
                                t.TicketID == TicketID).FirstOrDefault();
                        if (person != null)
                            person.BankStatement = entity.BankStatement;

                        var tour = BillTicketTours
                            .Where(t => t.OwnerName == entity.OwnerName &&
                                t.TicketID == TicketID).FirstOrDefault();
                        if (tour != null)
                        {
                            tour.FlightTicketNum = entity.FlightTicketNum;
                            tour.OuterReferenceID = entity.OuterReferenceID;
                            tour.Cost = entity.Cost;
                        }
                    }
                }
                this.dataCtx.SubmitChanges();


                tran.Commit();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally
            {
                dataCtx.Connection.Close();
            }
        }

        public BillTicket Save(BillTicket entity, IList<BillTicketPerson> PersonList, IList<BillTicketTour> TourList)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;
            string AuditLog = "";
            try
            {
                //由BillTicketTour的Price合计算出总价
                entity.TotalAmount = TourList.Sum(t => t.Price);
                //查询是否存在db
                var qry = from t in BillTickets
                          where t.ID == entity.ID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                {
                    //对比新旧记录,增加auditlog
                    this.AddAuditLog(obj, entity);
                    this.CopyEntity(obj, entity);
                }
                else
                    this.BillTickets.InsertOnSubmit(entity);

                this.dataCtx.SubmitChanges();
                //-------------- Delete & Insert TicketPerson Again -------------
                var OldPersonList = this.BillTicketPersons.Where(t => t.TicketID == entity.ID).ToList();
                if (CompareListObject(OldPersonList, PersonList) == false)
                    AuditLog = "修改乘客信息"; 
                this.BillTicketPersons.DeleteAllOnSubmit(OldPersonList);

                if (PersonList != null)
                {
                    foreach (var person in PersonList)
                    {
                        //如果是新增信息，id=0，要赋值
                        if (person.TicketID == 0)
                            person.TicketID = entity.ID;

                        //PersonAmount
                        decimal PersonAmount = TourList.Where(p => p.OwnerName.Equals(person.OwnerName)).Sum(t => t.Price);
                        person.Price = PersonAmount;
                        if (person.OwnerName.Trim() != "")
                            this.BillTicketPersons.InsertOnSubmit(person);

                    }
                }
                this.dataCtx.SubmitChanges();
                //-------------- Delete & Insert TourList Again -------------
                var OldBillTicketTours = this.BillTicketTours.Where(t => t.TicketID == entity.ID).ToList();
                if (CompareListObject(OldBillTicketTours, TourList) == false)
                    AuditLog += " 修改行程信息";

                this.BillTicketTours.DeleteAllOnSubmit(OldBillTicketTours);

                if (TourList != null)
                {
                    foreach (var tour in TourList)
                    {
                        //如果是新增信息，id=0，要赋值
                        if (tour.TicketID == 0)
                            tour.TicketID = entity.ID;
                        //如果填写了机票号，或者金额不为0，保存行程记录
                        if (tour.FlightNum.Trim() != "" || tour.Price!=0)
                            this.BillTicketTours.InsertOnSubmit(tour);

                    }
                }
                if(AuditLog!="")
                    this.AddAuditLog(AuditLog, entity.ModifyUserID, entity.ID);

                this.dataCtx.SubmitChanges();
                //------------Sync to CRMCustomerDeal-------------
                Sync2CRMCustomerDeal(entity.InnerReferenceID);

                tran.Commit();

                return entity;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally
            {
                dataCtx.Connection.Close();
            }
        }
        //if same return true
        private bool CompareListObject(IList<BillTicketPerson> Old, IList<BillTicketPerson> New)
        {
            if (Old == null && New == null) return true;
            if (Old == null && New != null) return false;
            if (Old != null && New == null) return false;
            if (Old != null && New != null)
            {
                if (Old.Count != New.Count)
                    return false;
                else
                {
                    for (int i = 0; i < Old.Count; i++)
                    {
                        if (Old[i] != New[i])
                            return false;
                    }
                    return true;
                }
            }
            return true;
        }
        private bool CompareListObject(IList<BillTicketTour> Old, IList<BillTicketTour> New)
        {
            if (Old == null && New == null) return true;
            if (Old == null && New != null) return false;
            if (Old != null && New == null) return false;
            if (Old != null && New != null)
            {
                if (Old.Count != New.Count)
                    return false;
                else
                {
                    for (int i = 0; i < Old.Count; i++)
                    {
                        if (Old[i] != New[i])
                            return false;
                    }
                    return true;
                }
            }
            return true;
        }
        /// <summary>
        /// 同步成交数据到CRMCustomerDeal表
        /// (依赖Customer)
        /// 在使用LINQ中更新数据或删除数据,总是出现找不到行或行已更改，最后发现每行数据中如果有值为NULL的时候，就会出现这个错误
        /// 原来是LinqToSql的数据实体对象在进行更新时会进行字段检查.
        /// </summary>
        /// <param name="deal"></param>
        private void Sync2CRMCustomerDeal(string ContractNum)
        {

            //-------------- Delete & Insert CRMCustomerDeals Again -------------
            var OldDealList = this.CRMCustomerDeals.Where(t => t.ContractNum == ContractNum).ToList();
            this.CRMCustomerDeals.DeleteAllOnSubmit(OldDealList);
            this.dataCtx.SubmitChanges();

            var DealList = vw_BillDeals.Where(t => t.ContractNum.Equals(ContractNum) && t.Price > 0).ToList();
            foreach (var deal in DealList)
            {
                CRMCustomerDeal entity = new CRMCustomerDeal();
                entity.ContractNum = deal.ContractNum;
                entity.CreateDate = deal.CreateDate;
                entity.CreateUserID = deal.CreateUserID;
                entity.Currency = deal.Currency;
                entity.CustID = (long)deal.CustID;
                entity.DealDate = deal.DealDate;
                entity.DealOwner = deal.DealOwner;
                entity.ModifyDate = deal.ModifyDate;
                entity.ModifyUserID = deal.ModifyUserID;
                entity.ProdID = deal.ProdID;
                entity.ProdSerialNum = deal.ProdSerialNum;
                entity.Qty = deal.Qty;
                entity.ReferenceNum = deal.ReferenceNum;
                entity.TotalAmount = deal.Price;
                entity.Unit = deal.Unit;
                entity.UnitPrice = deal.Price;
                entity.Status = 0;
                entity.Remark = "";
                entity.Brand = "";
                entity.PayTerm = "";
                entity.Shipment = "";
                entity.StockCategory = "";
                entity.UnitPriceDesc = deal.Price.ToString();
                entity.QtyDesc = deal.Qty.ToString();
                this.CRMCustomerDeals.InsertOnSubmit(entity);
            }
            this.dataCtx.SubmitChanges();
        }

        private void AddAuditLog(BillTicket oldEntity, BillTicket newEntity)
        {
            string oldOwner="", newOwner="";
            DataTable dtUser = base.GetUser();
            DataRow[] drs = dtUser.Select("UserID=" + oldEntity.CustOwnerID);
            if (drs.Length > 0)
            {
                oldOwner = drs[0]["UserFullName"].ToString();
            }
            drs = dtUser.Select("UserID=" + newEntity.CustOwnerID);
            if (drs.Length > 0)
            {
                newOwner = drs[0]["UserFullName"].ToString();
            }


            CRMAuditLog Log = new CRMAuditLog();
            Log.Action = "SaveTicket"; //界面显示时再转成多语言 SaveCustomer,SaveDeal,SaveUser
            Log.ActionAt = DateTime.Now;
            Log.ActionBy = newEntity.ModifyUserID;
            Log.PKId = newEntity.ID;
            string ActionLog = "";

            if (oldEntity.DepName != newEntity.DepName)
                ActionLog += "部门:" + oldEntity.DepName + "=>" + newEntity.DepName + " ;";
            if (oldEntity.DepAddress != newEntity.DepAddress)
                ActionLog += "公司地址:" + oldEntity.DepAddress + "=>" + newEntity.DepAddress + " ;";
            if (oldEntity.InnerReferenceID != newEntity.InnerReferenceID)
                ActionLog += "内部预订号:" + oldEntity.InnerReferenceID + "=>" + newEntity.InnerReferenceID + " ;";
            if (oldEntity.BookingDate != newEntity.BookingDate)
                ActionLog += "预订日期:" + oldEntity.BookingDate + "=>" + newEntity.BookingDate + " ;";

            if (oldEntity.CustID != newEntity.CustID)
                ActionLog += "CustID:" + oldEntity.CustID + "=>" + newEntity.CustID + " ;";

            if (oldEntity.CustName != newEntity.CustName)
                ActionLog += "客户姓名:" + oldEntity.CustName + "=>" + newEntity.CustName + " ;";

            if (oldEntity.CustEmail != newEntity.CustEmail)
                ActionLog += "电邮:" + oldEntity.CustEmail + "=>" + newEntity.CustEmail + " ;";

            if (oldEntity.CustAddress != newEntity.CustAddress)
                ActionLog += "客户地址:" + oldEntity.CustAddress + "=>" + newEntity.CustAddress + " ;";

            if (oldEntity.CustTel != newEntity.CustTel)
                ActionLog += "电话:" + oldEntity.CustTel + "=>" + newEntity.CustTel + " ;";
            if (oldEntity.CustOwnerID != newEntity.CustOwnerID)
                ActionLog += "所属销售:" + oldOwner + "=>" + newOwner + " ;";
            //if (oldEntity.BankAccount != newEntity.BankAccount)
            //    ActionLog += "BankAccount:" + oldEntity.BankAccount + "=>" + newEntity.BankAccount + " ;";
            //if (oldEntity.Currency != newEntity.Currency)
            //    ActionLog += "Currency:" + oldEntity.Currency + "=>" + newEntity.Currency + " ;";
            if (oldEntity.TotalAmount != newEntity.TotalAmount)
                ActionLog += "总价:" + oldEntity.TotalAmount + "=>" + newEntity.TotalAmount + " ;";
            if (oldEntity.ProdProvider != newEntity.ProdProvider)
                ActionLog += "航空公司:" + oldEntity.ProdProvider + "=>" + newEntity.ProdProvider + " ;";
            if (oldEntity.Accessory != newEntity.Accessory)
                ActionLog += "Leistung:" + oldEntity.Accessory + "=>" + newEntity.Accessory + " ;";
            if (oldEntity.MaxLuggage != newEntity.MaxLuggage)
                ActionLog += "行李重量:" + oldEntity.MaxLuggage + "=>" + newEntity.MaxLuggage + " ;";
            if (oldEntity.CancellationFee != newEntity.CancellationFee)
                ActionLog += "取消费用:" + oldEntity.CancellationFee + "=>" + newEntity.CancellationFee + " ;";
            if (oldEntity.ChangeFee != newEntity.ChangeFee)
                ActionLog += "改签费用:" + oldEntity.ChangeFee + "=>" + newEntity.ChangeFee + " ;";
            if (oldEntity.PaymentDate != newEntity.PaymentDate)
                ActionLog += "付款日期:" + oldEntity.PaymentDate + "=>" + newEntity.PaymentDate + " ;";
            if (oldEntity.DestinationRegion != newEntity.DestinationRegion)
                ActionLog += "目的国:" + oldEntity.DestinationRegion + "=>" + newEntity.DestinationRegion + " ;";
            if (oldEntity.Status != newEntity.Status)
                ActionLog += "Status:" + oldEntity.Status + "=>" + newEntity.Status + " ;";

            Log.ActionLog = ActionLog;

            this.CRMAuditLogs.InsertOnSubmit(Log);
        }
        //如果该方法在save方法调用,已经有Transaction了,就不要打开关闭db连接
        public void AddAuditLog(string ActionLog, long ModifyUserID, long PKID)
        {
            if (this.dataCtx.Transaction == null)
            {
                if (this.dataCtx.Connection != null)
                    if (this.dataCtx.Connection.State == ConnectionState.Closed)
                        this.dataCtx.Connection.Open();
            }

            CRMAuditLog Log = new CRMAuditLog();
            Log.Action = "SaveTicket"; //界面显示时再转成多语言 SaveCustomer,SaveDeal,SaveUser
            Log.ActionAt = DateTime.Now;
            Log.ActionBy = ModifyUserID;
            Log.PKId = PKID;
            Log.ActionLog = ActionLog;
            this.CRMAuditLogs.InsertOnSubmit(Log);

            if (this.dataCtx.Transaction == null)
            {
                this.dataCtx.SubmitChanges();
                if (this.dataCtx.Connection != null)
                    if (this.dataCtx.Connection.State != ConnectionState.Closed)
                        this.dataCtx.Connection.Close();
            }
        }

        public DataTable GetAuditLog(long PKID, string CreateDate, string CreateUser, string CreateUserID)
        {
            string sql;

            sql = @"select g.ActionLog, 
                g.ActionBy, g.ActionAt,u.UserName from CRMAuditLog g inner join CRMUser u 
                on g.ActionBy=u.UserID where PKId=" + PKID.ToString() + " and Action='SaveTicket' order by ActionAt";
            DataTable dt = DBExtBase.ExeFillTblBySqlText(this.dataCtx, sql);
            DataRow dr = dt.NewRow();
            dr["ActionLog"] = "新建Invoice";
            dr["ActionBy"] = CreateUserID;
            dr["ActionAt"] = CreateDate;
            dr["UserName"] = CreateUser;
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }

        //根据部门,预订日期生成下一个内部订单号
        //账单号写法：分类(两位)-年(两位)+月(两位)+序列号(三位)，如：21-1205095，杜塞机票2012年5月第95份	
        //21-	Airticket DUS, 杜塞机票
        //22-	Airticket STR, 斯图机票
        //23-	Airticket CGN, 科隆机票
        //25-   纽伦堡公司，机票账单号

        //28-	Visum, 德国签证,杜塞51 斯图52 科隆53 纽伦堡55
        //61-	Airticket Maastricht
        //62-	Airticket Arnhem

        public string GetNextInnerReferenceID(string DepName, string BookingDate,string BillType)
        {
            string InnerReferenceID = "";
            if (DepName.Contains("Düsseldorf"))
                InnerReferenceID = "21";
            else if (DepName.Contains("Stuttgart"))
                InnerReferenceID = "22";
            else if (DepName.Contains("Köln"))
                InnerReferenceID = "23";
            else if (DepName.Contains("代理"))
                InnerReferenceID = "24";
            else if (DepName.Contains("Nürnberg"))
                InnerReferenceID = "25";
            else if (DepName.Contains("Maastricht"))
                InnerReferenceID = "61";
            else if(DepName.Contains("Arnhem"))
                InnerReferenceID = "62";


            DateTime dtBookingDate = DateTime.Parse(BookingDate);

            InnerReferenceID += "-" + dtBookingDate.Year.ToString().Substring(2) + dtBookingDate.Month.ToString("00");

            string CurrentMax = DBExtBase.ExeScalarBySqlText(this.dataCtx,
                @"select max(InnerReferenceID) from BillTicket 
                where InnerReferenceID like '" + InnerReferenceID + "%'").ToString();
            if (string.IsNullOrEmpty(CurrentMax))
                InnerReferenceID += "001";
            else
                InnerReferenceID += (int.Parse(CurrentMax.Substring(7,3)) + 1).ToString("000"); //21-1204XXX-REB/GS

            if (BillType == "1")
                return InnerReferenceID + "-GS";
            else if (BillType == "2")
                return InnerReferenceID + "-REB";
            else 
                return InnerReferenceID;
        }

        //取得该预订单上所有乘客
        public IList<BillTicketPerson> getTicketPerson(int TicketID)
        {
            var qry = from t in BillTicketPersons
                      where t.TicketID == TicketID
                      select t;
            return qry.ToList();
        }

        //取得该预订单上某个乘客的行程
        public IList<BillTicketTour> getTicketTour(int TicketID, string Person)
        {
            var qry = from t in BillTicketTours
                      where t.TicketID == TicketID && t.OwnerName == Person
                      select t;
            return qry.ToList();
        }
    }
}
