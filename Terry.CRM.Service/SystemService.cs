 
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
    public class SystemService: BaseService
    {
				
        public IList<CRMSystem> SearchByCriteria(int CurrentPage, int PageSize, out int RecordCount, string Filter,string OrderBy)
        {
            if (OrderBy == "") OrderBy = "SYSID";

            var qry = from t in CRMSystems                      
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
        public CRMSystem LoadById(object Id)
        {
            long lngID = long.Parse((string)Id);
            var qry = from t in CRMSystems
                      where t.SYSID == lngID
                      select t;
            return qry.SingleOrDefault();
        }
        public CRMSystem Save(CRMSystem entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMSystems
                          where t.SYSID == entity.SYSID 
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMSystems.InsertOnSubmit(entity);

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
            var qry = from t in CRMSystems
                      where t.SYSID == lngID
                      select t;
			var obj = qry.SingleOrDefault();
            CRMSystems.DeleteOnSubmit(obj);
            this.dataCtx.SubmitChanges();
        }		
        #region DropdownList Data


        #endregion

    }
}

		