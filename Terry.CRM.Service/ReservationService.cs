using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Terry.CRM.Entity;

namespace Terry.CRM.Service
{
    public struct LogInfo
    {
        public string op;
        public DateTime opDate;
        public DateTime eventDate;
        public string client;
        public string employee;
        public string tel;
        public string logInfo;
    }
    public class ReservationService:BaseService
    {
        public System.Data.Linq.Table<vw_Reservation> vw_Reservations
        {
            get
            {
                return this.dataCtx.GetTable<vw_Reservation>();
            }

        }

        public System.Data.Linq.Table<tblReservation> Reservations
        {
            get 
            {
                return this.dataCtx.GetTable<tblReservation>();
            }
        }
        public System.Data.Linq.Table<tblReservationTime> ReservationTimes
        {
            get
            {
                return this.dataCtx.GetTable<tblReservationTime>();
            }
        }

        public DateTime getReservationDateByID(long ReservationID)
        {
            object dt =DBExtBase.ExeScalarBySqlText(this.dataCtx,  "select eventstart from tblReservation where ReservationID=" + ReservationID.ToString());
            if (dt == null)
                return DateTime.Now;
            else
                return (DateTime)dt;
        }
        public DateTime getReservationDateByMobile(string Mobile)
        {
            object dt = DBExtBase.ExeScalarBySqlText(this.dataCtx, "select eventstart from vw_Reservation where eventStart >= getdate()-1 and  MobilePhone='" + Mobile.Replace("'","''") + "'");
            if (dt == null)
                return DateTime.Now;
            else
                return (DateTime)dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public  vw_Reservation loadReservation(long ID)
        {
            var qry = from t in vw_Reservations
                      where t.ReservationID == ID
                      select t;
                return qry.SingleOrDefault();
        }
        public string loadReservationSMS(long ID)
        {
            vw_Reservation r = loadReservation(ID);
            string msg = string.Empty;
            if (r != null)
            { 
                msg ="<<公司名称>>提提您: 預約了" +  r.Product + "於" + r.EventStart  + "於" + r.OfficeName;
                if (r.CanChange==true)
                    msg += "分店,是次服務,之指定人员為" + r.UserName +",如有更改請致電. 謝!(預約編號:" + r.ReservationID.ToString()+")";
                else
                    msg += "分店,如有更改請致電. 謝!(預約編號:" + r.ReservationID.ToString() + ")";
            }
            return msg;
        }
        public void sendSMS(long ID)
        {
            DBExtBase.ExeNonQueryBySqlText(this.dataCtx, "update tblReservation set SendSMS=1 where reservationID=" + ID.ToString());
        }

        public void setInput(long ID)
        {
            DBExtBase.ExeNonQueryBySqlText(this.dataCtx, "update tblReservation set isInputDeal=1 where reservationID=" + ID.ToString());
        }

        public tblReservation loadReservationByKey(long ID)
        {
            var qry = from t in Reservations
                      where t.ReservationID == ID
                      select t;
            return qry.SingleOrDefault();
        }

        public List<vw_Reservation> loadReservationByID(long ID)
        {
            var qry = from t in vw_Reservations
                      where t.ReservationID == ID
                      select t;
            return qry.ToList();
        }
        public List<vw_Reservation> loadReservationByMobile(string Mobile)
        {
            var qry = from t in vw_Reservations
                      where t.CustomerTel.Contains(Mobile) 
                      && t.EventStart > DateTime.Now.AddDays(-7) 
                      orderby t.EventStart descending
                      select t;
            return qry.ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public tblReservation saveReservation(bool isNewAdd, tblReservation entity)
        {
            if (dataCtx.Connection != null) dataCtx.Connection.Open();
            DbTransaction tran = dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                if (isNewAdd == true)
                {
                    this.Reservations.InsertOnSubmit(entity);

                    this.dataCtx.SubmitChanges();
                    tran.Commit();
                    return entity;
                }
                else
                {
                    var qry = from t in Reservations
                              where t.ReservationID == entity.ReservationID
                              select t;
                    var obj = qry.SingleOrDefault();
                    if (obj != null)
                        this.CopyEntity(obj, entity);

                    this.dataCtx.SubmitChanges();
                    tran.Commit();
                    return obj;
                }
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

        public void deleteReservation(int ID)
        {
            DBExtBase.ExeNonQueryBySqlText(this.dataCtx, "delete from tblReservation where ReservationID=" + ID.ToString());
        }

        public tblReservationTime saveReservationTime(tblReservationTime entity)
        {
            if (dataCtx.Connection != null) dataCtx.Connection.Open();
            DbTransaction tran = dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {

                var qry = from t in ReservationTimes
                          where t.employeeNum == entity.employeeNum
                          && t.workdate == entity.workdate
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                {
                    //pk identity can't update
                    //this.CopyEntity(obj, entity);
                    obj.checkinTime = entity.checkinTime;
                    obj.checkoutTime = entity.checkoutTime;
                    obj.isLeave = entity.isLeave;
                    obj.AnnualLeave = entity.AnnualLeave;
                    obj.SickLeave = entity.SickLeave;
                    obj.OtherLeave = entity.OtherLeave;
                    obj.workOffice = entity.workOffice;
                }
                else
                    this.ReservationTimes.InsertOnSubmit(entity);

                this.dataCtx.SubmitChanges();
                tran.Commit();
                return obj;

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
        //log
        public void WriteLog(LogInfo entity)
        {
            string sql = "INSERT INTO [tblLog] ([Operator],[OpDate],[EventDate],[Client],[Employee],[Tel],[LogInfo]) VALUES(";
            sql += "N'" + entity.op + "','" + entity.opDate.ToString() + "','" + entity.eventDate.ToString();
            sql += "',N'" + entity.client + "',N'" + entity.employee + "','" + entity.tel + "',N'" + entity.logInfo.Replace("'","''") + "')";
            DBExtBase.ExeNonQueryBySqlText(this.dataCtx, sql);
        }
        public DataTable ReadLog(DateTime eventDate)
        {
            DataTable dt = new DataTable();
            dt =DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from tblLog where eventDate between '" + eventDate.ToString() + "' and '"+ eventDate.AddDays(1).ToString()+"'");
            return dt;
        }
        public DataTable getReservationLeave(string Date)
        {
            DataTable dt = new DataTable();
            dt=DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from vw_ReservationLeave where workdate='" + Date+"'");
            return dt;
        }
        public DataTable LoadAllEmployeeForReservation()
        {
            DataTable dt = new DataTable();
            dt=DBExtBase.ExeFillTblBySqlText(this.dataCtx,  "select employeeNum,employeeName from tblEmployee ");
            return dt;
        }
        public bool checkConflict(long reservationID,string EmpNum, DateTime EventStart, DateTime EventEnd,ref string ErrMsg)
        {
            string sql = string.Empty;
            DataTable table = new DataTable();

            sql = "select eventStart,eventEnd from vw_Reservation where employeeNum='" + EmpNum 
                +"' and eventStart between '" + EventStart.Date.ToShortDateString() 
                + "' and '"+ EventStart.Date.AddDays(1).ToShortDateString() 
                +"'  and NOT(eventStart>='" + EventEnd + "' or eventEnd<='" + EventStart
                + "')  and reservationID<>" + reservationID;
            table =DBExtBase.ExeFillTblBySqlText(this.dataCtx, sql);
            if (table.Rows.Count > 0)
            {
                ErrMsg = "預約時間有衝突,該員工這個時間段"
                    + ((DateTime)table.Rows[0]["eventStart"]).ToShortTimeString()+"---"
                    + ((DateTime)table.Rows[0]["eventEnd"]).ToShortTimeString() +
                    "已被預約.";
                return false;
            }
            return true;
        }


        public DataTable GetEmployeesByOfficeAndDay(string workOffice, string workDate)
        {
            DataTable dt = new DataTable();
            dt = DBExtBase.ExeFillTblBySqlText(this.dataCtx, 
                "select employeeNum from tblReservationTime where workOffice='"+ workOffice+"' and workDate='"+ workDate +"'");
            return dt;
        }

        public string getBusinessExcludeHours(string strEmployeeNum, DateTime workDate)
        {
            DataTable table = new DataTable();
            string sql = "SELECT * FROM tblReservationTime";
            sql += " where employeeNum='" + strEmployeeNum + "'";
            sql += " and workdate ='" + workDate.ToShortDateString() + "'";

            table = DBExtBase.ExeFillTblBySqlText(this.dataCtx, sql);

            string BusinessExcludeHours = "";
            if (table.Rows.Count > 0)
            { //如果休息,整日都变成灰色
                if ((bool)table.Rows[0]["isLeave"] == true)
                    BusinessExcludeHours = "11:00-23:00";
                else
                    BusinessExcludeHours = "11:00-" + table.Rows[0]["checkinTime"] + "," + table.Rows[0]["checkoutTime"] + "-23:00";

            }
            return BusinessExcludeHours;
        }

        public DataTable getReservation(string strEmployeeNum, DateTime start, int days)
        {
            DataTable table = new DataTable();
            string sql = "SELECT * FROM vw_Reservation";
            sql += " WHERE NOT ((eventEnd <= '" + start.ToShortDateString() + @"') 
                OR (eventStart >= '" + start.AddDays(days).ToShortDateString() + "'))";
            sql += " and employeeNum='" + strEmployeeNum + "'";

            table =DBExtBase.ExeFillTblBySqlText(this.dataCtx, sql);
            return table;

        }
    }
}
