
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
    public class AuditLogService : BaseService
    {


        public IList<CRMAuditLog> SearchByCriteria(int CurrentPage, int PageSize, out int RecordCount, string Filter, string OrderBy)
        {
            if (OrderBy == "") OrderBy = "LogId";
            var qry = from t in CRMAuditLogs
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

        public CRMAuditLog Insert(CRMAuditLog entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                this.CRMAuditLogs.InsertOnSubmit(entity);

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

    }
}

