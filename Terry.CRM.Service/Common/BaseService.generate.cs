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

namespace Terry.CRM.Service
{
    public partial class BaseService
    {
        //Linq Entity
        public Table<CRMAnnouce> CRMAnnouces
        {
            get { return this.dataCtx.GetTable<CRMAnnouce>(); }
        }
        //---------------保存CRMAnnouce---------------------------			
        public CRMAnnouce Save(CRMAnnouce entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMAnnouces
                          where t.ID == entity.ID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMAnnouces.InsertOnSubmit(entity);

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
        //Linq Entity
        public Table<CRMActionComment> CRMActionComments
        {
            get { return this.dataCtx.GetTable<CRMActionComment>(); }
        }
        //---------------保存CRMActionComments---------------------------			
        public CRMActionComment Save(CRMActionComment entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMActionComments
                          where t.ID == entity.ID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMActionComments.InsertOnSubmit(entity);

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
        //Linq Entity
        public Table<CRMCustomerType> CRMCustomerTypes
        {
            get { return this.dataCtx.GetTable<CRMCustomerType>(); }
        }
        //---------------保存CRMCustomerType---------------------------			
        public CRMCustomerType Save(CRMCustomerType entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMCustomerTypes
                          where t.CustTypeID == entity.CustTypeID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMCustomerTypes.InsertOnSubmit(entity);

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

        //Linq Entity
        public Table<CRMCustomerIndustry> CRMCustomerIndustrys
        {
            get { return this.dataCtx.GetTable<CRMCustomerIndustry>(); }
        }
        //---------------保存CRMCustomerIndustry---------------------------			
        public CRMCustomerIndustry Save(CRMCustomerIndustry entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMCustomerIndustrys
                          where t.IndustryID == entity.IndustryID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMCustomerIndustrys.InsertOnSubmit(entity);

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

        //Linq Entity
        public Table<CRMCustomerRelation> CRMCustomerRelations
        {
            get { return this.dataCtx.GetTable<CRMCustomerRelation>(); }
        }
        //---------------保存CRMCustomerRelation---------------------------			
        public CRMCustomerRelation Save(CRMCustomerRelation entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMCustomerRelations
                          where t.RelationID == entity.RelationID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMCustomerRelations.InsertOnSubmit(entity);

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

        //Linq Entity
        public Table<CRMCustomerEmpNum> CRMCustomerEmpNums
        {
            get { return this.dataCtx.GetTable<CRMCustomerEmpNum>(); }
        }
        //---------------保存CRMCustomerEmpNum---------------------------			
        public CRMCustomerEmpNum Save(CRMCustomerEmpNum entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMCustomerEmpNums
                          where t.ID == entity.ID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMCustomerEmpNums.InsertOnSubmit(entity);

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

        //Linq Entity
        public Table<CRMCustomerFrom> CRMCustomerFroms
        {
            get { return this.dataCtx.GetTable<CRMCustomerFrom>(); }
        }
        //---------------保存CRMCustomerFrom---------------------------			
        public CRMCustomerFrom Save(CRMCustomerFrom entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMCustomerFroms
                          where t.ID == entity.ID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMCustomerFroms.InsertOnSubmit(entity);

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

        //Linq Entity
        public Table<CRMCustomerStatus> CRMCustomerStatuss
        {
            get { return this.dataCtx.GetTable<CRMCustomerStatus>(); }
        }
        //---------------保存CRMCustomerStatus---------------------------			
        public CRMCustomerStatus Save(CRMCustomerStatus entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMCustomerStatuss
                          where t.ID == entity.ID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMCustomerStatuss.InsertOnSubmit(entity);

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

        //Linq Entity
        public Table<CRMCountry> CRMCountrys
        {
            get { return this.dataCtx.GetTable<CRMCountry>(); }
        }
        //---------------保存CRMCountry---------------------------			
        public CRMCountry Save(CRMCountry entity)
        {
            if (this.dataCtx.Connection != null) this.dataCtx.Connection.Open(); if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open(); DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMCountrys
                          where t.CountryID == entity.CountryID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMCountrys.InsertOnSubmit(entity);

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

        //Linq Entity
        public Table<CRMProvince> CRMProvinces
        {
            get { return this.dataCtx.GetTable<CRMProvince>(); }
        }
        //---------------保存CRMCountry---------------------------			
        public CRMProvince Save(CRMProvince entity)
        {
            if (this.dataCtx.Connection != null) this.dataCtx.Connection.Open(); if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open(); DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMProvinces
                          where t.ProvinceID == entity.ProvinceID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMProvinces.InsertOnSubmit(entity);

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
        //Linq Entity
        public Table<CRMContactType> CRMContactTypes
        {
            get { return this.dataCtx.GetTable<CRMContactType>(); }
        }
        //---------------保存CRMContactType---------------------------			
        public CRMContactType Save(CRMContactType entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMContactTypes
                          where t.ID == entity.ID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMContactTypes.InsertOnSubmit(entity);

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

        //Linq Entity
        public Table<CRMCategory> CRMCategorys
        {
            get { return this.dataCtx.GetTable<CRMCategory>(); }
        }
        //Linq Entity
        public Table<CRMCategoryProd> CRMCategoryProds
        {
            get { return this.dataCtx.GetTable<CRMCategoryProd>(); }
        }
        //---------------保存CRMCategory---------------------------			
        public CRMCategory Save(CRMCategory entity, IList<CRMProduct> prodList)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMCategorys
                          where t.CatID == entity.CatID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMCategorys.InsertOnSubmit(entity);

                this.dataCtx.SubmitChanges();

                //delete  relationship with product
                var qryDel = from t in CRMCategoryProds
                             where t.CatID == entity.CatID
                             select t;
                foreach (var item in qryDel.ToList())
                {
                    this.CRMCategoryProds.DeleteOnSubmit(item);
                }
                //add new
                foreach (var prod in prodList)
                {
                        var p = new CRMCategoryProd();
                        p.CatID = entity.CatID;
                        p.ProdCode = prod.Code;
                        this.CRMCategoryProds.InsertOnSubmit(p);
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

        //Linq Entity
        public Table<CRMActionUser> CRMActionUsers
        {
            get { return this.dataCtx.GetTable<CRMActionUser>(); }
        }
        //---------------保存CRMActionUser---------------------------			
        public CRMActionUser Save(CRMActionUser entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMActionUsers
                          where t.ID == entity.ID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMActionUsers.InsertOnSubmit(entity);

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

        //Linq Entity
        public Table<CRMRole> CRMRoles
        {
            get { return this.dataCtx.GetTable<CRMRole>(); }
        }
        //---------------保存CRMRole---------------------------			
        public CRMRole Save(CRMRole entity, IList<CRMProduct> prodList,IList<CRMRoleModule> ModuleRights,
            IList<CRMDepartment> depList, IList<CRMProvince> ProvinceList)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMRoles
                          where t.RoleID == entity.RoleID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMRoles.InsertOnSubmit(entity);

                this.dataCtx.SubmitChanges();
                //delete Role relationship with product
                var qryDel = from t in CRMRoleProds
                             where t.RoleID == entity.RoleID
                             select t;
                foreach (var item in qryDel.ToList())
                {
                    this.CRMRoleProds.DeleteOnSubmit(item);
                }
                //add new
                foreach (var prod in prodList)
                {
                    //insert sub product
                    var subList = GetSubProds(prod.ProdID);
                    foreach (var sub in subList)
                    {
                        var p = new CRMRoleProd();
                        p.RoleID = entity.RoleID;
                        p.ProdID = sub;
                        this.CRMRoleProds.InsertOnSubmit(p);
                    }
                }

                this.dataCtx.SubmitChanges();

                //delete Role relationship with module
                var qryDelM = from t in CRMRoleModules
                             where t.RoleID == entity.RoleID
                             select t;
                foreach (var item in qryDelM.ToList())
                {
                    this.CRMRoleModules.DeleteOnSubmit(item);
                }
                //add new
                foreach (var Module in ModuleRights)
                {
                    if (Module!=null)
                    { 
                        var p = new CRMRoleModule();
                        p.RoleID = entity.RoleID;
                        p.ModuleID = Module.ModuleID;
                        p.ReadOnly = Module.ReadOnly;
                        p.New = Module.New;
                        p.Edit = Module.Edit;
                        p.Del = Module.Del;
                        this.CRMRoleModules.InsertOnSubmit(p);                    
                    }

                }
                this.dataCtx.SubmitChanges();

                //delete Role relationship with Department
                var qryDelDep = from t in CRMRoleDeps
                              where t.RoleID == entity.RoleID
                              select t;
                foreach (var item in qryDelDep.ToList())
                {
                    this.CRMRoleDeps.DeleteOnSubmit(item);
                }
                //add new 
                foreach (var dep in depList)
                {
                    if (dep != null)
                    {
                        var p = new CRMRoleDep();
                        p.RoleID = entity.RoleID;
                        p.DepID = dep.DepID;
                        this.CRMRoleDeps.InsertOnSubmit(p);
                    }

                }
                this.dataCtx.SubmitChanges();

                //delete Role relationship with province
                var qryDelProvince = from t in CRMRoleProvinces
                             where t.RoleID == entity.RoleID
                             select t;
                foreach (var item in qryDelProvince.ToList())
                {
                    this.CRMRoleProvinces.DeleteOnSubmit(item);
                }
                //add new
                foreach (var prov in ProvinceList)
                {
                    if (prov != null)
                    {
                        var p = new CRMRoleProvince();
                        p.RoleID = entity.RoleID;
                        p.ProvinceID = prov.ProvinceID;
                        this.CRMRoleProvinces.InsertOnSubmit(p);
                    }
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

        //Linq Entity
        public Table<CRMUserRole> CRMUserRoles
        {
            get { return this.dataCtx.GetTable<CRMUserRole>(); }
        }
        //---------------保存CRMUserRole---------------------------			
        public CRMUserRole Save(CRMUserRole entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMUserRoles
                          where t.ID == entity.ID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMUserRoles.InsertOnSubmit(entity);

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

        //Linq Entity
        public Table<CRMProduct> CRMProducts
        {
            get { return this.dataCtx.GetTable<CRMProduct>(); }
        }
        //---------------保存CRMProduct---------------------------			
        public CRMProduct Save(CRMProduct entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMProducts
                          where t.ProdID == entity.ProdID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMProducts.InsertOnSubmit(entity);

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

        //Linq Entity
        public Table<CRMDepartment> CRMDepartments
        {
            get { return this.dataCtx.GetTable<CRMDepartment>(); }
        }
        //---------------保存CRMDepartment---------------------------			
        public CRMDepartment Save(CRMDepartment entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMDepartments
                          where t.DepID == entity.DepID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMDepartments.InsertOnSubmit(entity);

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
        //Linq Entity
        public Table<CRMRoleProvince> CRMRoleProvinces
        {
            get { return this.dataCtx.GetTable<CRMRoleProvince>(); }
        }
        //---------------保存CRMRoleProvince---------------------------			
        public CRMRoleProvince Save(CRMRoleProvince entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMRoleProvinces
                          where t.ID == entity.ID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMRoleProvinces.InsertOnSubmit(entity);

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

        //Linq Entity
        public Table<CRMRoleDep> CRMRoleDeps
        {
            get { return this.dataCtx.GetTable<CRMRoleDep>(); }
        }
        //---------------保存CRMRoleDep---------------------------			
        public CRMRoleDep Save(CRMRoleDep entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMRoleDeps
                          where t.ID == entity.ID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMRoleDeps.InsertOnSubmit(entity);

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
        //Linq Entity
        public Table<CRMRoleProd> CRMRoleProds
        {
            get { return this.dataCtx.GetTable<CRMRoleProd>(); }
        }
        //---------------保存CRMRoleProd---------------------------			
        public CRMRoleProd Save(CRMRoleProd entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMRoleProds
                          where t.ID == entity.ID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMRoleProds.InsertOnSubmit(entity);

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

        //Linq Entity
        public Table<CRMRoleModule> CRMRoleModules
        {
            get { return this.dataCtx.GetTable<CRMRoleModule>(); }
        }
        //---------------保存CCRMRoleModule---------------------------			
        public CRMRoleModule Save(CRMRoleModule entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMRoleModules
                          where t.ID == entity.ID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMRoleModules.InsertOnSubmit(entity);

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

        public Table<vw_CRMRoleCustomer> vw_CRMRoleCustomers
        {
            get { return this.dataCtx.GetTable<vw_CRMRoleCustomer>(); }
        }
        //Linq Entity
        public Table<vw_CRMAction> vw_CRMActions
        {
            get { return this.dataCtx.GetTable<vw_CRMAction>(); }
        }
        public Table<CRMAction> CRMActions
        {
            get { return this.dataCtx.GetTable<CRMAction>(); }
        }
        //Linq Entity
        public Table<CRMAuditLog> CRMAuditLogs
        {
            get { return this.dataCtx.GetTable<CRMAuditLog>(); }
        }
        //Linq Entity
        public Table<vw_CRMContact> vw_CRMContacts
        {
            get { return this.dataCtx.GetTable<vw_CRMContact>(); }
        }
        public Table<CRMContact> CRMContacts
        {
            get { return this.dataCtx.GetTable<CRMContact>(); }
        }
        //Linq Entity
        public Table<vw_CRMCustomer> vw_CRMCustomers
        {
            get { return this.dataCtx.GetTable<vw_CRMCustomer>(); }
        }
        public Table<vw_CRMCustomer2> vw_CRMCustomer2s
        {
            get { return this.dataCtx.GetTable<vw_CRMCustomer2>(); }
        }
        public Table<CRMCustomer> CRMCustomers
        {
            get { return this.dataCtx.GetTable<CRMCustomer>(); }
        }

        public Table<CRMCustomerProd> CRMCustomerProds
        {
            get { return this.dataCtx.GetTable<CRMCustomerProd>(); }
        }
        public Table<CRMCustomerHistory> CRMCustomerHistorys
        {
            get { return this.dataCtx.GetTable<CRMCustomerHistory>(); }
        }

        public Table<CRMCustomerProdHistory> CRMCustomerProdHistorys
        {
            get { return this.dataCtx.GetTable<CRMCustomerProdHistory>(); }
        }
        public Table<CRMCustomerDeal> CRMCustomerDeals
        {
            get { return this.dataCtx.GetTable<CRMCustomerDeal>(); }
        }

        //Linq Entity
        public Table<CRMCalendar> CRMCalendars
        {
            get { return this.dataCtx.GetTable<CRMCalendar>(); }
        }
        //Linq Entity
        public Table<CRMSystem> CRMSystems
        {
            get { return this.dataCtx.GetTable<CRMSystem>(); }
        }
        //Linq Entity
        public Table<vw_CRMUser> vw_CRMUsers
        {
            get { return this.dataCtx.GetTable<vw_CRMUser>(); }
        }
        public Table<CRMUser> CRMUsers
        {
            get { return this.dataCtx.GetTable<CRMUser>(); }
        }

        public Table<CRMLoginHistory> CRMLoginHistories
        {
            get { return this.dataCtx.GetTable<CRMLoginHistory>(); }
        }
        public Table<vw_CRMLoginHistory> vw_CRMLoginHistories
        {
            get { return this.dataCtx.GetTable<vw_CRMLoginHistory>(); }
        }

        public Table<BillDailyIssue> BillDailyIssues
        {
            get { return this.dataCtx.GetTable<BillDailyIssue>(); }
        }
        public Table<BillTicket> BillTickets
        { 
            get {return this.dataCtx.GetTable<BillTicket>();}
        }
        public Table<BillTicketPerson> BillTicketPersons
        { 
            get {return this.dataCtx.GetTable<BillTicketPerson>();}
        }
         public Table<BillTicketTour> BillTicketTours
        {
            get { return this.dataCtx.GetTable<BillTicketTour>(); }
        }
         public Table<BillVisa> BillVisas
         {
             get { return this.dataCtx.GetTable<BillVisa>(); }
         }
         public Table<BillVisaPerson> BillVisaPersons
         {
             get { return this.dataCtx.GetTable<BillVisaPerson>(); }
         }

         public Table<vw_BillDeal> vw_BillDeals
         {
             get { return this.dataCtx.GetTable<vw_BillDeal>(); }
         }
         public Table<vw_BillTicket> vw_BillTickets
         {
             get { return this.dataCtx.GetTable<vw_BillTicket>(); }
         }
        
    }
}
