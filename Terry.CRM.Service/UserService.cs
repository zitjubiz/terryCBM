
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
using System.Security.Cryptography;

namespace Terry.CRM.Service
{
    public class UserService : BaseService
    {

        public IList<vw_CRMUser> SearchByCriteria(int CurrentPage, int PageSize, out int RecordCount, string Filter, string OrderBy)
        {
            if (OrderBy == "") OrderBy = "UserID";

            var qry = from t in vw_CRMUsers
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
        
        public vw_CRMUser LoadById(object Id)
        {
            long lngID = long.Parse((string)Id);
            var qry = from t in vw_CRMUsers
                      where t.UserID == lngID
                      select t;
            return qry.SingleOrDefault();
        }

        public vw_CRMUser LoadByUserName(string UserName)
        {
            var qry = from t in vw_CRMUsers
                      where t.UserName == UserName
                      select t; 
            return qry.FirstOrDefault();
        }

        //---------------保存CRMActionComments---------------------------			
        public CRMUser Save(CRMUser entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMUsers
                          where t.UserID == entity.UserID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMUsers.InsertOnSubmit(entity);

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
   
        public CRMUser Save(CRMUser entity, IList<CRMRole> RoleList)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMUsers
                          where t.UserID == entity.UserID
                          select t;
                var obj = qry.SingleOrDefault();

                //encrypt password,如果没输入password，不更改数据库password
                if (string.IsNullOrEmpty(entity.Password))
                {
                    if (obj != null)
                    {
                        entity.Password = obj.Password;
                    }

                }
                else
                {
                    entity.Password = this.Encypt(entity.UserName, entity.Password);
                }

                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMUsers.InsertOnSubmit(entity);

                this.dataCtx.SubmitChanges();

                //delete Role relationship with User
                var qryDel = from t in CRMUserRoles
                             where t.UserID == entity.UserID
                             select t;
                foreach (var item in qryDel.ToList())
                {
                    this.CRMUserRoles.DeleteOnSubmit(item);
                }
                //add new
                foreach (var role in RoleList)
                {
                    var p = new CRMUserRole();
                    p.UserID = entity.UserID;
                    p.RoleID = role.RoleID;
                    this.CRMUserRoles.InsertOnSubmit(p);
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
            var qry = from t in CRMUsers
                      where t.UserID == lngID
                      select t;
            var obj = qry.SingleOrDefault();
            CRMUsers.DeleteOnSubmit(obj);
            this.dataCtx.SubmitChanges();
        }
        //soft delete user and rename login_name to prevent duplicate
        public void SoftDeleteById(string Id)
        {
            string sql = "update CRMUser set username='_'+username, IsActive=0 where UserId=" + Id;

            DBExtBase.ExeNonQueryBySqlText(this.dataCtx, sql);

        }
        public int GetActiveUserCount()
        {
            return int.Parse(
                DBExtBase.ExeScalarBySqlText(this.dataCtx, 
                "select count(*) from CRMUser where IsActive=1").ToString());
        }
        /// <summary>
        /// 校验用户名,密码
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public vw_CRMUser Validate(string UserName, string Password,ref int ErrCode)
        {
            vw_CRMUser UserInDb = this.LoadByUserName(UserName);
            if (null == UserInDb)
            {
                ErrCode = 1; //NotExistUser
                return null;
            }
            else
            {
                if (UserInDb.Password == Encypt(UserName, Password) && UserInDb.IsActive == true)
                {
                    ErrCode = 0;
                    return UserInDb;
                }
                else
                {
                    ErrCode = 2; //Wrong Password
                    return UserInDb;
                }
            }

        }
        /// <summary>
        /// 加密密码
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PlainText"></param>
        /// <returns></returns>
        public string Encypt(string UserID, string PlainTextPwd)
        {
            HashAlgorithm Al = HashAlgorithm.Create("SHA1");
            string HashStr = String.Empty;

            Byte[] bytes = Encoding.UTF8.GetBytes(UserID.ToUpper() + PlainTextPwd);
            Byte[] buffer = Al.ComputeHash(bytes);
            int Len = buffer.Length;
            for (int i = 0; i < Len; i++)
            {
                string HexStr = Convert.ToString(buffer[i], 16);
                int Pos = HexStr.Length - 2;
                if (Pos < 0)
                    Pos = 0;
                HashStr += ("0" + HexStr).Substring(Pos);

            }
            return HashStr;
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="EncyptedPwd"></param>
        /// <returns></returns>
        private string Decypt(string UserID, string EncyptedPwd)
        {
            return string.Empty;
        }

        public IList<CRMUserRole> GetRole(long UserID)
        {
            var qry = from t in CRMUserRoles
                      where t.UserID == UserID
                      select t;
            return qry.ToList();
        }

        public DataTable GetCustomerByUserId(string UserID)
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMCustomer where CustOwnerID="+ UserID);
        }
        //public DataTable GetUsersByGrade(int Grade)
        //{
        //    //return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMUser");
        //}
        public void TransferCustomer(string FromUser, string ToUser, bool DelFromUser)
        {
            string sql = "Update CRMCustomer set CustOwnerId=" + ToUser + " where CustOwnerID=" + FromUser;
            if(DelFromUser)
                sql += " Update CRMUser set isActive=0 wher UserID="+ FromUser;
            DBExtBase.ExeNonQueryBySqlText(this.dataCtx,sql);
        }
        public void TransferCustomer(string CustomerLists, string ToUser)
        {
            string sql = "Update CRMCustomer set CustOwnerId=" + ToUser + " where CustID in (" + CustomerLists +")";

            DBExtBase.ExeNonQueryBySqlText(this.dataCtx, sql);
        }
        public void InsertLoginHistory(CRMLoginHistory entity)
        {
            CRMLoginHistories.InsertOnSubmit(entity);
            this.dataCtx.SubmitChanges();
        }

        public IList<vw_CRMLoginHistory> GetLoginHistory(DateTime Begin, DateTime End, params int[] UserID)
        {
            var qry = from t in vw_CRMLoginHistories
                      where t.LoginAt > Begin && t.LoginAt < End
                      select t;

            if (UserID != null && UserID.Length > 0)
                qry = qry.Where(t => t.UserId == UserID[0]).OrderByDescending(t=>t.LoginAt);
            else
                qry =qry.OrderByDescending(t=>t.LoginAt);

            return qry.Take(500).ToList<vw_CRMLoginHistory>();

        }
    }
}

