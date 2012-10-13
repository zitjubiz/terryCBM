
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using Terry.CRM.Entity;
using System.Data;
using System.Data.Common;
using System.Data.Linq;

namespace Terry.CRM.Service
{
    public class ActionService : BaseService
    {


        public IList<vw_CRMAction> SearchByCriteria(int CurrentPage, int PageSize, out int RecordCount, 
            string Filter, string OrderBy)
        {
            if (OrderBy == "") OrderBy = "ACTID";
            var qry = from t in vw_CRMActions
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
        public IList<vw_CRMAction> SearchByCriteria(int CurrentPage, int PageSize, out int RecordCount, 
            int ACTypeID, int CustID,string OrderBy)
        {
            if (OrderBy == "") OrderBy = "ACTID";
            var qry = from t in vw_CRMActions
                      where t.ACTTypeID == ACTypeID && t.CustID ==CustID
                      select t;
            qry = qry.OrderBy(OrderBy);

            RecordCount = qry.Count();

            if (CurrentPage == -1 || PageSize == -1)
                return qry.ToList();
            else
            {
                return qry.Skip(CurrentPage * PageSize).Take(PageSize).ToList();
            }

        }
        public vw_CRMAction LoadById(object Id)
        {
            long lngID = long.Parse((string)Id);
            var qry = from t in vw_CRMActions
                      where t.ACTID == lngID
                      select t;
            return qry.SingleOrDefault();
        }
        public CRMAction Save(CRMAction entity, IList<CRMUser> UserList)
        {
            if (this.dataCtx.Connection != null) 
                if(this.dataCtx.Connection.State== ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMActions
                          where t.ACTID == entity.ACTID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMActions.InsertOnSubmit(entity);

                this.dataCtx.SubmitChanges();

                //delete Action relationship with User
                var qryDel = from t in CRMActionUsers
                             where t.ACTID == entity.ACTID
                             select t;
                foreach (var item in qryDel.ToList())
                {
                    this.CRMActionUsers.DeleteOnSubmit(item);
                }
                //add new
                foreach (var user in UserList)
                {
                    var p = new CRMActionUser();
                    p.ACTID = entity.ACTID;
                    p.ACTUser = user.UserID;
                    this.CRMActionUsers.InsertOnSubmit(p);
                }

                this.dataCtx.SubmitChanges();

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
        public void DeleteById(object Id)
        {
            long lngID = long.Parse((string)Id);
            var qry = from t in CRMActions
                      where t.ACTID == lngID
                      select t;
            var obj = qry.SingleOrDefault();
            CRMActions.DeleteOnSubmit(obj);
            this.dataCtx.SubmitChanges();
        }
        public IList<CRMActionUser> GetJoinUsers(long lngID)
        {
            var qry = from t in CRMActionUsers
                      where t.ACTID == lngID
                      select t;
            return qry.ToList();
        }

        public IList<CRMActionComment> GetActionComments(long lngID)
        {
            var qry = from t in CRMActionComments
                      where t.ACTID == lngID
                      select t;
            return qry.ToList();
        }
    }
}

