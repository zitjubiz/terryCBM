using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.Linq;
using Terry.CRM.Entity;
using System.Data;
using System.Reflection;
using System.Data.Common;
using System.Data.SqlClient;

namespace Terry.CRM.Service
{

    public partial class BaseService
    {
        private DataContext _dataCtx;
        private string OnlyDisplayActiveRecords = " where isActive=1";

        public void handler(DataContext dataContext)
        {
            this._dataCtx = dataContext;
        }

        public DataContext dataCtx
        {
            get
            {
                if (_dataCtx == null)
                    _dataCtx = new DataContext(ConfigurationManager.ConnectionStrings["Terry.CRM.Entity.Properties.Settings.CRMConnectionString"].ToString().Trim());
                return _dataCtx;
            }
        }

        protected void CopyEntity(Object DestObj, Object SrcObj)
        {
            Type type = SrcObj.GetType();
            PropertyInfo[] p = type.GetProperties();
            for (int i = 0; i < p.Length; i++)
            {
                if (p[i].GetCustomAttributes(false)[0] is System.Data.Linq.Mapping.ColumnAttribute)
                {
                    System.Data.Linq.Mapping.ColumnAttribute att = (System.Data.Linq.Mapping.ColumnAttribute)(p[i].GetCustomAttributes(false)[0]);
                    //如果是主键,就不去更新copy
                    if (att.IsPrimaryKey == false && att.IsDbGenerated == false)
                        p[i].SetValue(DestObj, p[i].GetValue(SrcObj, null), null);
                }
            }
        }

        protected string IsNull(object obj)
        {
            if (obj != null)
                return obj.ToString();
            else
                return "";
        }

        #region DropdownList Data
        public DataTable GetAction()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMAction" + OnlyDisplayActiveRecords);
        }
        public DataTable GetActionType()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMActionType");
        }
        public DataTable GetCustomer()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMCustomer" + OnlyDisplayActiveRecords);
        }
        public DataTable GetCustNameTel(enumRoleGrade LoginUserRoleGrade, long LoginUserID, long LoginUserRoleID)
        {
            //add search criteria
            string Filter = string.Empty;

            //Chemical: sales只能看到自己所属的客户，产品经理可以看到有使用其所属产品的客户，老板可以看到所有
            //Ticket: sales能看到所有的客户
            switch (LoginUserRoleGrade)
            {
                case enumRoleGrade.Sales://sales
                    Filter = "and CustOwnerID=" + LoginUserID;

                    break;
                case enumRoleGrade.ProdManager: //Prod Manager can see its prods and it's customer as sales
                    Filter = "and CustId in (select CustId from vw_CRMRoleCustomer where RoleId=" + LoginUserRoleID.ToString() + ")";
                    break;
                case enumRoleGrade.DepManager: //Dep Manager can see its department customers
                    Filter = "and CustId in (select CustId from vw_CRMRoleDepCustomer where RoleId=" + LoginUserRoleID.ToString() + ")";
                    break;
                case enumRoleGrade.Boss: //Boss
                    Filter = "and 1=1";
                    break;
                default:
                    Filter = "and 1=0";
                    break;
            }
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, @"select Cast(CustID as varchar(10))+'|'+CustName as CustIDName,
            CustName + ' '+isnull(CustTel,'') as CustNameTel from CRMCustomer" + OnlyDisplayActiveRecords + Filter + " order by CustNameTel");



        }
        public DataTable GetAuditLog()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMAuditLog");
        }
        public DataTable GetContact()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMContact" + OnlyDisplayActiveRecords);
        }
        public DataTable GetContactType()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMContactType");
        }
        public DataTable GetCustType()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMCustomerType");
        }
        public DataTable GetCustStatus()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMCustomerStatus");
        }
        public DataTable GetCustRelation()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMCustomerRelation order by RelationID");
        }
        public DataTable GetCustIndustry()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMCustomerIndustry");
        }
        public DataTable GetCustFrom()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMCustomerFrom");
        }
        public DataTable GetCustEmpNum()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMCustomerEmpNum");
        }
        public DataTable GetDepartment()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMDepartment");
        }
        public DataTable GetProvince()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMProvince");
        }
        public DataTable GetRegion()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select distinct Region from CRMProvince");
        }
        public DataTable GetSystem()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMSystem");
        }
        public DataTable GetUser()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from vw_CRMUser where username<>'admin' and isActive=1 order by DepID,username");
        }
        public DataTable GetUserByDep()
        {
            string sql = @"SELECT   UserName, CAST(UserID AS varchar(10)) AS userid, DepName
            FROM      vw_CRMUser
            WHERE   (UserName <> 'admin') AND (IsActive = 1)
            UNION
            SELECT   ' ' + DepName AS username, 'optgroup' AS Userid, DepName
            FROM      CRMDepartment
            ORDER BY DepName, UserName";
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, sql);
        }
        public DataTable GetEmailGroup(long? OwnerID)
        {
            if (OwnerID == null)
                return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select distinct GroupName from CRMEmailGroup");
            else
                return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select distinct GroupName from CRMEmailGroup where OwnerID=" + OwnerID.ToString());
        }
        public DataTable GetCountry()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select Country+'-'+ Remark as Country,CountryID from CRMCountry");
        }

        public DataTable GetRole()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMRole");
        }
        public DataTable GetRoleBelowGrade(int grade)
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMRole where RoleGrade<=" + grade.ToString());
        }

        public DataTable GetProduct()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMProduct");
        }
        public DataTable GetProductDDL()
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select *, replicate('-',len(code)-3)+product as Proddl  from CRMProduct where len(code)<=6 order by code");
        }
        #endregion

        public object LoadById(Type type, string key, string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            int iValue = 0;
            int.TryParse(value, out iValue);

            string sql = "select top 1 * from " + type.Name;

            sql += " where " + key + "=" + iValue;

            DataTable dt = DBExtBase.ExeFillTblBySqlText(this.dataCtx, sql);
            //------------------------------
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                var Entity = Activator.CreateInstance(type);
                DataRow dr = dt.Rows[0];
                foreach (DataColumn dc in dt.Columns)
                {

                    PropertyInfo pi = Entity.GetType().GetProperty(dc.ColumnName);
                    if (pi != null)
                    {
                        if (dr[dc.ColumnName] != DBNull.Value)
                        {
                            object ColumnValue = dr[dc.ColumnName];

                            //类型“System.Decimal”的对象无法转换为类型“System.String”
                            if (pi.PropertyType == typeof(string))
                                ColumnValue = dr[dc.ColumnName].ToString();

                            //类型“System.String”的对象无法转换为类型“System.Char”
                            //set char(1) to string data type in dbml file
                            if (pi.PropertyType == typeof(Char))
                                ColumnValue = ColumnValue.ToString().ToCharArray()[0];

                            pi.SetValue(Entity, ColumnValue, null);
                        }
                        else
                            pi.SetValue(Entity, null, null);

                    }
                }
                return Entity;

            }
        }

        public void DeleteById(Type type, string key, string value)
        {
            string sql = "delete from " + type.Name;
            sql += " where " + key + "=" + value;

            DBExtBase.ExeNonQueryBySqlText(this.dataCtx, sql);
        }

        public void SoftDeleteById(Type type, string key, string value)
        {
            string sql = "update " + type.Name;
            sql += " set IsActive=0 where " + key + "=" + value;

            DBExtBase.ExeNonQueryBySqlText(this.dataCtx, sql);

        }
        public void SetStatusInactive(Type type, string key, string value)
        {
            string sql = "update " + type.Name;
            sql += " set status='I' where " + key + "=" + value;

            DBExtBase.ExeNonQueryBySqlText(this.dataCtx, sql);

        }
        public void SetStatusActive(Type type, string key, string value)
        {
            string sql = "update " + type.Name;
            sql += " set status='A' where " + key + "=" + value;

            DBExtBase.ExeNonQueryBySqlText(this.dataCtx, sql);

        }
        public DataTable SearchByCriteria(string TableName, string ColumnName, string Filter, string OrderBy)
        {
            string sql = "select " + ColumnName + " from " + TableName;

            if (string.IsNullOrEmpty(Filter) == false)
                sql += " where 1=1 " + Filter; //.Replace("'", "''").Replace("\"", "'");

            if (string.IsNullOrEmpty(OrderBy) == false)
                sql += " order by " + OrderBy;
            DataTable dt = DBExtBase.ExeFillTblBySqlText(this.dataCtx, sql);

            return dt;

        }
        public DataTable SearchByCriteria(string TableName, out int RecordCount, string Filter, string OrderBy)
        {
            string sql = "select * from " + TableName;

            if (string.IsNullOrEmpty(Filter) == false)
                sql += " where 1=1 " + Filter; //.Replace("'", "''").Replace("\"", "'");

            if (string.IsNullOrEmpty(OrderBy) == false)
                sql += " order by " + OrderBy;
            DataTable dt = DBExtBase.ExeFillTblBySqlText(this.dataCtx, sql);

            RecordCount = dt.Rows.Count;

            return dt;

        }
        public DataTable SearchByCriteria(Type type, int CurrentPage, int PageSize,
            out int RecordCount, string Filter, string OrderBy)
        {
            return SearchByCriteria(type.Name, CurrentPage, PageSize, out RecordCount, Filter, OrderBy);
        }
        /// <summary>
        /// 因为用ROW_NUMBER() Over(Order By XXX) As RowNum,所以OrderBy参数必须有值
        /// </summary>
        public DataTable SearchByCriteria(string TableName, int CurrentPage, int PageSize,
            out int RecordCount, string Filter, string OrderBy)
        {
            DataTable dt = new DataTable();
            int PageCnt = 0, RCnt = 0;
            SqlParameter[] parms = new SqlParameter[] {
					new SqlParameter("@Columns", "*"),
					new SqlParameter("@TableName", TableName),
					new SqlParameter("@Condition", Filter),
					new SqlParameter("@OrderBy", OrderBy),
					new SqlParameter("@PageNum", CurrentPage+1),
					new SqlParameter("@PageSize", PageSize),
					new SqlParameter("@PageCount", PageCnt),
					new SqlParameter("@RecordCount", RCnt)
            };
            parms[6].Direction = ParameterDirection.Output;
            parms[7].Direction = ParameterDirection.Output;
            DBExtBase.ExeBySP(this.dataCtx, dt, "usp_GetPageData", parms);
            RecordCount = (int)parms[7].Value;

            return dt;

        }

        public DataTable GetTopN(Type type, int N, string Filter, string OrderBy)
        {
            string sql = "select top " + N + " * from " + type.Name;

            if (string.IsNullOrEmpty(Filter) == false)
                sql += " where " + Filter.Replace("'", "''").Replace("\"", "'");

            if (string.IsNullOrEmpty(OrderBy) == false)
                sql += " order by " + OrderBy;
            DataTable dt = DBExtBase.ExeFillTblBySqlText(this.dataCtx, sql);

            return dt;

        }
        public IList<CRMRoleProvince> GetRoleRelativeProvince(long RoleID)
        {
            var qry = from t in CRMRoleProvinces
                      where t.RoleID == RoleID
                      select t;
            return qry.ToList();
        }
        public IList<CRMRoleProd> GetRoleRelativeProducts(long RoleID)
        {
            var qry = from t in CRMRoleProds
                      where t.RoleID == RoleID
                      select t;
            return qry.ToList();
        }
        public IList<CRMRoleDep> GetRoleRelativeDeps(long RoleID)
        {
            var qry = from t in CRMRoleDeps
                      where t.RoleID == RoleID
                      select t;
            return qry.ToList();
        }
        public IList<CRMCategoryProd> GetCatProducts(long CatID)
        {
            var qry = from t in CRMCategoryProds
                      where t.CatID == CatID
                      select t;
            return qry.ToList();
        }
        //get Role relative customer
        public long[] GetRoleRelativeCustomer(long RoleID)
        {
            var qry = from t in vw_CRMRoleCustomers
                      where t.RoleID == RoleID
                      select t.CustID;
            return qry.ToArray();
        }

        public IList<CRMRoleModule> GetRoleAccessRight(long RoleID)
        {
            var qry = from t in CRMRoleModules
                      where t.RoleID == RoleID
                      select t;
            return qry.ToList();
        }

        //找到Role对某个模块有无权限
        public CRMRoleModule GetRoleAccessRight(long RoleID, enumModule Module)
        {
            var qry = from t in CRMRoleModules
                      where t.RoleID == RoleID && t.ModuleID == (long)Module
                      select t;
            return qry.FirstOrDefault();
        }

        //找到该产品下属的产品ID
        public long[] GetSubProds(long ParentProdID)
        {
            //StartsWith 只支持客户端取值的，所以要查2次
            string code = (from p in CRMProducts
                           where p.ProdID == ParentProdID
                           select p).First().Code;

            var qry = from t in CRMProducts
                      where t.Code.StartsWith(code)
                      select t.ProdID;

            return qry.ToArray();
        }
        public DataTable GetEmailGroup(string GroupName, long OwnerID)
        {
            return DBExtBase.ExeFillTblBySqlText(this.dataCtx, "select * from CRMEmailGroup where GroupName='" + GroupName + "' and OwnerID="+ OwnerID.ToString());
        }
        public bool SaveEmailGroup(ArrayList Name, ArrayList Email, string GroupName, long OwnerID)
        {
            int GroupNameCount = (int)DBExtBase.ExeScalarBySqlText(this.dataCtx,
                "select count(*) from CRMEmailGroup where GroupName='" + GroupName + "'");
            if (GroupNameCount > 0)
                return false;
            else
            {
                string sql = "";
                for (int i = 0; i < Name.Count; i++)
                {
                    sql += "insert into CRMEmailGroup ([GroupName],[PersonName],[Email],[OwnerID]) VALUES('"
                        + GroupName + "','" + Name[i] + "','" + Email[i] + "'," + OwnerID +");";
                }
                DBExtBase.ExeNonQueryBySqlText(this.dataCtx, sql);
                return true;
            }
        }


    }
}
