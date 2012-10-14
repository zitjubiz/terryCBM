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
    public class VisaService : BaseService
    {
        public IList<BillVisa> SearchByCriteria(int CurrentPage, int PageSize, out int RecordCount, string Filter, string OrderBy)
        {
            if (OrderBy == "") OrderBy = "ID";
            var qry = from t in BillVisas
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

        public BillVisa LoadById(object Id)
        {
            long lngID = long.Parse((string)Id);
            var qry = from t in BillVisas
                      where t.ID == lngID
                      select t;
            return qry.SingleOrDefault();
        }

        public BillVisa Save(BillVisa entity, IList<BillVisaPerson> PersonList)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                //由BillVisaPerson的Price合计算出总价
                decimal totalAmount = 0;
                foreach (var person in PersonList)
                {
                    totalAmount += person.EmbassyFee + person.PostFee + person.ServiceFee
                        + person.BirthCert + person.HKPass + person.VisaCenterFee;
                }
                entity.TotalAmount = totalAmount;
                //查询是否存在db
                var qry = from t in BillVisas
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
                    this.BillVisas.InsertOnSubmit(entity);

                this.dataCtx.SubmitChanges();
                //-------------- Delete & Insert TicketPerson Again -------------
                this.BillVisaPersons.DeleteAllOnSubmit(this.BillVisaPersons.Where(t => t.VisaID == entity.ID).ToList());

                if (PersonList != null)
                {
                    foreach (var person in PersonList)
                    {
                        //如果是新增信息，id=0，要赋值
                        if (person.VisaID == 0)
                            person.VisaID = entity.ID;

                        //PersonAmount

                        if (person.VisaName.Trim() != "")
                            this.BillVisaPersons.InsertOnSubmit(person);

                    }
                }
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

        /// <summary>
        /// 同步成交数据到CRMCustomerDeal表
        /// (依赖Customer)
        /// </summary>
        /// <param name="deal"></param>
        private void Sync2CRMCustomerDeal(string ContractNum)
        {

            //-------------- Delete & Insert CRMCustomerDeals Again -------------
            this.CRMCustomerDeals.DeleteAllOnSubmit(this.CRMCustomerDeals.Where(t => t.ContractNum == ContractNum).ToList());
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
                entity.UnitPrice =  deal.Price;
                entity.Status = 0;
                entity.Remark = "";
                this.CRMCustomerDeals.InsertOnSubmit(entity);
            }
            this.dataCtx.SubmitChanges();
        }

        private void AddAuditLog(BillVisa oldEntity, BillVisa newEntity)
        {
            string oldOwner, newOwner;
            DataTable dtUser = base.GetUser();
            oldOwner = dtUser.Select("UserID=" + oldEntity.CustOwnerID)[0]["UserFullName"].ToString();
            newOwner = dtUser.Select("UserID=" + newEntity.CustOwnerID)[0]["UserFullName"].ToString();


            CRMAuditLog Log = new CRMAuditLog();
            Log.Action = "SaveVisa"; //界面显示时再转成多语言 SaveCustomer,SaveDeal,SaveUser
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

            if (oldEntity.Status != newEntity.Status)
                ActionLog += "Status:" + oldEntity.Status + "=>" + newEntity.Status + " ;";

            Log.ActionLog = ActionLog;

            this.CRMAuditLogs.InsertOnSubmit(Log);
        }

        public void AddAuditLog(string ActionLog, long ModifyUserID, long PKID)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();

            CRMAuditLog Log = new CRMAuditLog();
            Log.Action = "SaveVisa"; //界面显示时再转成多语言 SaveCustomer,SaveDeal,SaveUser
            Log.ActionAt = DateTime.Now;
            Log.ActionBy = ModifyUserID;
            Log.PKId = PKID;
            Log.ActionLog = ActionLog;
            this.CRMAuditLogs.InsertOnSubmit(Log);
            this.dataCtx.SubmitChanges();
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State != ConnectionState.Closed)
                    this.dataCtx.Connection.Close();
        }

        public DataTable GetAuditLog(long PKID, string CreateDate, string CreateUser, string CreateUserID)
        {
            string sql;

            sql = @"select g.ActionLog, 
                g.ActionBy, g.ActionAt,u.UserName from CRMAuditLog g inner join CRMUser u 
                on g.ActionBy=u.UserID where PKId=" + PKID.ToString() + " and Action='SaveVisa' order by ActionAt";
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
        public string GetNextInnerReferenceID(string DepName, string BookingDate)
        {
            string InnerReferenceID = "28";
            if (DepName.Contains("Maastricht"))
                InnerReferenceID = "68";
            else if (DepName.Contains("Arnhem"))
                InnerReferenceID = "68";
            else if (DepName.Contains("Düsseldorf"))
                InnerReferenceID = "51";
            else if (DepName.Contains("Stuttgart"))
                InnerReferenceID = "52";
            else if (DepName.Contains("Köln"))
                InnerReferenceID = "53";
            else if (DepName.Contains("Nürnberg"))
                InnerReferenceID = "55";

            DateTime dtBookingDate = DateTime.Parse(BookingDate);

            InnerReferenceID += "-" + dtBookingDate.Year.ToString().Substring(2) + dtBookingDate.Month.ToString("00");

            string CurrentMax = DBExtBase.ExeScalarBySqlText(this.dataCtx,
                @"select max(InnerReferenceID) from BillVisa 
                where InnerReferenceID like '" + InnerReferenceID + "%'").ToString();
            if (string.IsNullOrEmpty(CurrentMax))
                return InnerReferenceID + "001";
            else
                return InnerReferenceID + (int.Parse(CurrentMax.Substring(CurrentMax.Length - 3)) + 1).ToString("000");
        }

        //取得该签证单上所有人
        public IList<BillVisaPerson> getVisaPerson(long VisaID)
        {
            var qry = from t in BillVisaPersons
                      where t.VisaID == VisaID
                      select t;
            return qry.ToList();
        }

        public DataTable GetVisum(int Year, int Month)
        {
            string sql;
            sql = @"select * from vw_BillVisum where SUBSTRING(InnerReferenceID, 4, 2)='" + Year.ToString().Substring(2)
                + "' and SUBSTRING(InnerReferenceID, 6, 2)='" + Month.ToString("00") + "'";
            DataTable dt = DBExtBase.ExeFillTblBySqlText(this.dataCtx, sql);
            return dt;
        }
        //按bookingdate，还是ApplyDate呢？--按账单号！
        public DataTable GetVisumAccount(int Year, int Month,string DepPrefix="")
        {
            string sql;
            sql = @"select * from vw_BillVisumAccount where  SUBSTRING(InnerReferenceID, 4, 2)='" + Year.ToString().Substring(2)
                + "' and SUBSTRING(InnerReferenceID, 6, 2)='" + Month.ToString("00") + "'";
            if (DepPrefix != "")
            {
                sql += " and left(InnerReferenceID,2)='" + DepPrefix + "'";
            }
            DataTable dt = DBExtBase.ExeFillTblBySqlText(this.dataCtx, sql);
            return dt;
        }
    }
}
