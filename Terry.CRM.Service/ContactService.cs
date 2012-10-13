
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
    public class ContactService : BaseService
    {


        public IList<vw_CRMContact> SearchByCriteria(int CurrentPage, int PageSize, out int RecordCount, string Filter, string OrderBy)
        {
            Filter = Filter.Replace("'", "\'");
            if (OrderBy == "") OrderBy = "ContactID";
            var qry = from t in vw_CRMContacts
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
        public vw_CRMContact LoadById(object Id)
        {
            long lngID = long.Parse((string)Id);
            var qry = from t in vw_CRMContacts
                      where t.ContactID == lngID
                      select t;
            return qry.FirstOrDefault();
        }

        public vw_CRMContact LoadById(string CustId, int ContactTypeID)
        {
            var qry = from t in vw_CRMContacts
                      where t.ContactCustID == long.Parse(CustId)
                      && t.ContactTypeID == ContactTypeID
                      && t.IsActive == true
                      select t;
            return qry.FirstOrDefault();
        }

        //联络人信息要保存历史记录
        public CRMContact Save(CRMContact entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMContacts
                          where t.ContactID == entity.ContactID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                {
                    //this.CopyEntity(obj, entity);
                    obj.IsActive = false;   //把原来的联络人信息的IsActive=false
                }
                this.CRMContacts.InsertOnSubmit(entity);

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
            var qry = from t in CRMContacts
                      where t.ContactID == lngID
                      select t;
            var obj = qry.SingleOrDefault();
            CRMContacts.DeleteOnSubmit(obj);
            this.dataCtx.SubmitChanges();
        }

    }
}

