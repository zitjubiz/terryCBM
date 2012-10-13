
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
using System.Configuration;

namespace Terry.CRM.Service
{
    public class CustomerService : BaseService
    {
        public long GetCustIDByName(string CustName, string CustTel)
        {
            var qry = from t in vw_CRMCustomers
                      where t.CustName == CustName && t.CustTel == CustTel
                      select t.CustID;
            return qry.SingleOrDefault();
        }

        //����������ID�п����ظ�
        public long GetCustIDByName(string CustName)
        {
            var qry = from t in vw_CRMCustomers
                      where t.CustName == CustName
                      select t.CustID;
            var lst = qry.ToList();
            if (lst.Count > 0)
                return lst[0];
            else
                return 0;
        }
        //������ֲ�����,����һ��cust
        public long GetCustIDByName(string CustName, string CustAddress, string CustTel,
           string CustEmail, long Creator, long Owner, bool CreateIfNotExist)
        {
            var qry = from t in vw_CRMCustomers
                      where t.CustName == CustName && t.IsActive==true
                      select t;
            var lst = qry.ToList();
            if (lst.Count > 0)
            {
                return lst[0].CustID;
            }
            else
            {
                if (CreateIfNotExist)
                {
                    var qry1 = from t in CRMCustomers
                              where t.CustName == CustName
                              select t;
                    
                    if (qry1.ToList().Count > 0)
                    {
                        var InactiveCust = qry1.SingleOrDefault();
                        InactiveCust.IsActive = true;
                        return Save(InactiveCust, null).CustID;
                    }

                    CRMCustomer entity = new CRMCustomer();
                    if (CustName.Contains("MRS"))
                        entity.Gender = "F";
                    else
                        entity.Gender = "M";
                    entity.CustName = CustName;
                    entity.CustFullName = CustName;
                    entity.CustAddress = CustAddress;
                    entity.CustEmail = CustEmail;
                    entity.CustTel = CustTel;
                    entity.CustTypeID = 3; // ��ͨ����
                    entity.CustRelationID = 6; //�����ö��
                    entity.CommissionFactor = 1;
                    entity.IsActive = true;
                    entity.Status = "A";
                    entity.CustCDate = DateTime.Now;
                    entity.CustCUserID = Creator;
                    entity.CustOwnerID = Owner;
                    entity.SYSID = 1;
                    entity.CustModifyDate = DateTime.Now;
                    entity.CustModifyUserID = Creator;
                    entity = Save(entity, null);
                    return entity.CustID;

                }
                else
                    return 0;
            }
        }
        //get Product relative customer,����Ʒʹ������ҿͻ�
        public long[] GetProdRelativeCustomer(long ProdID)
        {
            //ȡ�øò�Ʒ�Լ��������Ĳ�ƷID
            var prods = GetSubProds(ProdID);

            var qry = from t in CRMCustomerProds
                      .Where(t => prods.Contains(t.ProdID))
                      select t.CustID;
            return qry.ToArray();
        }

        //���г������ҿͻ�
        public long[] GetCategoryRelativeCustomer(int catID)
        {
            //category��Ӧ�Ĳ�Ʒcodes
            DataTable dt = DBExtBase.ExeFillTblBySqlText(this.dataCtx, @"select prodID from crmCategoryProd  cp 
                        inner join crmproduct p on left(p.code,len(cp.prodcode)) = cp.prodcode
                        where catid=" + catID.ToString());
            long[] prodIDs = new long[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                prodIDs[i] = Convert.ToInt32(dt.Rows[i][0]);
            }

            var qry = from t in CRMCustomerProds
                   .Where(t => prodIDs.Contains(t.ProdID))
                      select t.CustID;
            return qry.ToArray();

        }

        public IList<vw_CRMCustomer2> SearchByCriteria(int CurrentPage, int PageSize,
            out int RecordCount, string Filter, string OrderBy, long[] CustListByRole,
            long[] CustListByProd, long[] custListByCategory, long LoginUserID)
        {

            if (OrderBy == "") OrderBy = "CustID";

            var qry = from t in vw_CRMCustomer2s
                      .Where(Filter)
                      .OrderBy(OrderBy)
                      select t;
            if (CustListByRole != null)
            {
                qry = qry.Where(t => CustListByRole.Contains(t.CustID) || t.CustOwnerID == LoginUserID);
            }

            if (CustListByProd != null)
                qry = qry.Where(t => CustListByProd.Contains(t.CustID));

            if (custListByCategory != null)
                qry = qry.Where(t => custListByCategory.Contains(t.CustID));

            RecordCount = qry.Count();

            if (CurrentPage == -1 || PageSize == -1)
                return qry.ToList();
            else
            {
                return qry.Skip(CurrentPage * PageSize).Take(PageSize).ToList();
            }


        }

        public vw_CRMCustomer LoadById(object Id)
        {
            long lngID = long.Parse((string)Id);
            var qry = from t in vw_CRMCustomers
                      where t.CustID == lngID
                      select t;
            return qry.SingleOrDefault();
        }

        //���ڲ�ֱ��update�ɼ�¼,���ǰѾɼ�¼copy��history��,��update customer to status=Pending
        //Boss ����֮�󣬰�customer's status = Active
        public CRMCustomer Save(CRMCustomer entity, IList<CRMCustomerProd> LstCP)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMCustomers
                          where t.CustID == entity.CustID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                {
                    //�Ѿɼ�¼copy��history��,ɾ���ɵ�CRMCustomerProds
                    CopyToHistory(obj);
                    //�Ա��¾ɼ�¼,����auditlog
                    this.AddAuditLog(obj, entity);
                    //��ֵ
                    this.CopyEntity(obj, entity);

                }
                else
                    this.CRMCustomers.InsertOnSubmit(entity);

                //����ͻ���Ϣ,�õ����µ�entity.CustID
                this.dataCtx.SubmitChanges();

                ////�����Զ����ɿͻ�����,����ƴ������ĸ
                //entity.CustCode = entity.CustID.ToString();
                if (LstCP != null)
                {
                    foreach (var cp in LstCP)
                    {
                        //����������ͻ���Ϣ��custid=0��Ҫ��ֵ
                        if (cp.CustID == 0)
                            cp.CustID = entity.CustID;

                        this.CRMCustomerProds.InsertOnSubmit(cp);

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

        //�ѵ�ǰ��¼copy����ʷ��
        private void CopyToHistory(CRMCustomer obj)
        {
            long NextVersion;

            var qry = CRMCustomerProdHistorys.Where(t => t.CustID == obj.CustID);

            if (qry.Count() > 0)
                NextVersion = qry.Max(t => t.Version) + 1;
            else
                NextVersion = 1;

            CRMCustomerHistory h = new CRMCustomerHistory();
            h.Version = NextVersion;
            h.BirthDay = obj.BirthDay;
            h.BuddyList = obj.BuddyList;
            h.Budget = obj.Budget;
            h.CommissionFactor = obj.CommissionFactor;
            h.CRMCustomer = obj;
            h.CustAddress = obj.CustAddress;
            h.CustBackground = obj.CustBackground;
            h.CustCity = obj.CustCity;
            h.CustCode = obj.CustCode == null ? obj.CustID.ToString() : obj.CustCode;
            h.CustCountryID = obj.CustCountryID;
            h.CustDistinct = obj.CustDistinct;
            h.CustEmpNumID = obj.CustEmpNumID;
            h.CustFax = obj.CustFax;
            h.CustFromID = obj.CustFromID;
            h.CustFullName = obj.CustFullName;
            h.CustID = obj.CustID;
            h.CustIndustryID = obj.CustIndustryID;
            h.CustInfo = obj.CustInfo;
            h.CustMaterial = obj.CustMaterial;
            h.CustName = obj.CustName;
            h.CustOwnerID = obj.CustOwnerID;
            h.CustPayMethod = obj.CustPayMethod;
            h.CustPort = obj.CustPort;
            h.CustProvince = obj.CustProvince;
            h.CustPurchaseChannels = obj.CustPurchaseChannels;
            h.CustRelationID = obj.CustRelationID;
            h.CustRisk = obj.CustRisk;
            h.CustSaleChannels = obj.CustSaleChannels;
            h.CustStatusID = obj.CustStatusID;
            h.CustTel = obj.CustTel;
            h.CustTypeID = obj.CustTypeID;
            h.CustWeb = obj.CustWeb;
            h.FavoriteDest = obj.FavoriteDest;
            h.FavoriteProd = obj.FavoriteProd;
            h.Gender = obj.Gender;
            h.ParentCompany = obj.ParentCompany;
            h.Passport = obj.Passport;
            h.PassportExpiryDate = obj.PassportExpiryDate;
            h.Position = obj.Position;
            h.PreferPlace = obj.PreferPlace;
            h.PreferPrice = obj.PreferPrice;
            h.RequireVisa = obj.RequireVisa;
            h.Status = obj.Status;
            h.TravelDay = obj.TravelDay;
            h.UseOwnMoney = obj.UseOwnMoney;
            h.SYSID = obj.SYSID;

            this.CRMCustomerHistorys.InsertOnSubmit(h);

            var oldList = CRMCustomerProds.Where(t => t.CustID == obj.CustID).ToList();
            foreach (var item in oldList)
            {
                CRMCustomerProdHistory cph = new CRMCustomerProdHistory();
                cph.Brand = item.Brand;
                cph.CustID = item.CustID;
                cph.ProdID = item.ProdID;
                cph.Remark = item.Remark;
                cph.Usage = item.Usage;
                cph.Version = NextVersion;
                this.CRMCustomerProdHistorys.InsertOnSubmit(cph);
            }
            //ɾ���ɵ�Customer Products
            this.CRMCustomerProds.DeleteAllOnSubmit(oldList);

        }

        //���бȽϿͻ�������
        private void AddAuditLog(CRMCustomer oldCust, CRMCustomer newCust)
        {
            #region ID to Name using DataTable Select Method

            DataTable dtCustType = base.GetCustType();
            DataTable dtCustRelation = base.GetCustRelation();
            //DataTable dtCustIndustry = base.GetCustIndustry();
            DataTable dtCustEmpNum = base.GetCustEmpNum();
            DataTable dtCountry = base.GetCountry();
            DataTable dtUser = base.GetUser();

            string oldCustType = "", newCustType;
            string oldCustRelation, newCustRelation;
            //string oldCustIndustry, newCustIndustry;
            string oldCustEmpNum, newCustEmpNum;
            string oldCountry, newCountry;
            string oldOwner="", newOwner="";

            if (oldCust.CustOwnerID == 0)
                oldOwner = "";
            else
            {
                var drs =dtUser.Select("UserID=" + oldCust.CustOwnerID);
                if (drs.Length > 0)
                    oldOwner = drs[0]["UserFullName"].ToString();
                else
                    oldOwner = "";
            }
            if(newCust.CustOwnerID!=0)
            {
                var drs = dtUser.Select("UserID=" + newCust.CustOwnerID);
                newOwner = drs[0]["UserFullName"].ToString();
            }

            //-------------------------------------
            oldCustType = dtCustType.Select("CustTypeID=" + oldCust.CustTypeID)[0]["CustType"].ToString();
            newCustType = dtCustType.Select("CustTypeID=" + newCust.CustTypeID)[0]["CustType"].ToString();
            //-------------------------------------
            if (oldCust.CustRelationID == null)
                oldCustRelation = "";
            else
            {
                DataRow[] drs = dtCustRelation.Select("RelationID=" + oldCust.CustRelationID);
                if (drs.Length > 0)
                    oldCustRelation = drs[0]["Relation"].ToString();
                else
                    oldCustRelation = "";
            }
            newCustRelation = dtCustRelation.Select("RelationID=" + newCust.CustRelationID)[0]["Relation"].ToString();
            //-------------------------------------
            if (oldCust.CustCountryID == null)
                oldCountry = "";
            else
            {
                DataRow[] drs = dtCountry.Select("CountryID=" + oldCust.CustCountryID);
                if (drs.Length > 0)
                    oldCountry = drs[0]["Country"].ToString();
                else
                    oldCountry = "";
            }
            if (newCust.CustCountryID == null)
                newCountry = "";
            else
                newCountry = dtCountry.Select("CountryID=" + newCust.CustCountryID)[0]["Country"].ToString();
            //-------------------------------------
            if (oldCust.CustEmpNumID == null)
                oldCustEmpNum = "";
            else
            {
                DataRow[] drs = dtCustEmpNum.Select("ID=" + oldCust.CustEmpNumID);
                if (drs.Length > 0)
                    oldCustEmpNum = drs[0]["EmpNum"].ToString();
                else
                    oldCustEmpNum = "";
            }
            if (newCust.CustEmpNumID == null)
                newCustEmpNum = "";
            else
                newCustEmpNum = dtCustEmpNum.Select("ID=" + newCust.CustEmpNumID)[0]["EmpNum"].ToString();
            //-------------------------------------
            #endregion

            CRMAuditLog Log = new CRMAuditLog();
            Log.Action = "SaveCustomer"; //������ʾʱ��ת�ɶ����� SaveCustomer,SaveDeal,SaveUser
            Log.ActionAt = DateTime.Now;
            Log.ActionBy = newCust.CustModifyUserID;
            Log.PKId = newCust.CustID;
            string ActionLog = "";

            if (oldCust.CustOwnerID != newCust.CustOwnerID)
                ActionLog += "����������Ա:" + oldOwner + "=>" + newOwner + " ;";

            if (oldCust.IsActive != newCust.IsActive)
                ActionLog += "IsActive:" + oldCust.IsActive + "=>" + newCust.IsActive + " ;";
            if (oldCust.CustCode != newCust.CustCode)
                ActionLog += "�ͻ�����:" + oldCust.CustCode + "=>" + newCust.CustCode + " ;";
            if (oldCust.CustName != newCust.CustName)
                ActionLog += "�ͻ����:" + oldCust.CustName + "=>" + newCust.CustName + " ;";
            if (oldCust.CustFullName != newCust.CustFullName)
                ActionLog += "�ͻ�ȫ��:" + oldCust.CustCode + "=>" + newCust.CustCode + " ;";

            if (oldCust.CustTypeID != newCust.CustTypeID)
                ActionLog += "�ͻ�����:" + oldCustType + "=>" + newCustType + " ;";

            if (oldCust.CustRelationID != newCust.CustRelationID)
                ActionLog += "�ͻ�����:" + oldCustRelation + "=>" + newCustRelation + " ;";

            if (oldCust.CustPayMethod != newCust.CustPayMethod)
                ActionLog += "���ʽ:" + oldCust.CustPayMethod + "=>" + newCust.CustPayMethod + " ;";
            if (oldCust.CustEmail != newCust.CustEmail)
                ActionLog += "����:" + oldCust.CustEmail + "=>" + newCust.CustEmail + " ;";
            if (oldCust.CustProvince != newCust.CustProvince)
                ActionLog += "ʡ��:" + oldCust.CustProvince + "=>" + newCust.CustProvince + " ;";
            if (oldCust.CustAddress != newCust.CustAddress)
                ActionLog += "�ͻ���ַ:" + oldCust.CustAddress + "=>" + newCust.CustAddress + " ;";
            if (oldCust.CustTel != newCust.CustTel)
                ActionLog += "�绰:" + oldCust.CustTel + "=>" + newCust.CustTel + " ;";
            if (oldCust.CustFax != newCust.CustFax)
                ActionLog += "����:" + oldCust.CustFax + "=>" + newCust.CustFax + " ;";
            if (oldCust.CustWeb != newCust.CustWeb)
                ActionLog += "��ַ:" + oldCust.CustWeb + "=>" + newCust.CustWeb + " ;";

            //ticket
            if (ConfigurationManager.AppSettings["Industry"].ToLower() == "ticket")
            {
                if (oldCust.Gender != newCust.Gender)
                    ActionLog += "�Ա�:" + oldCust.Gender + "=>" + newCust.Gender + " ;";
                if (oldCust.BirthDay != newCust.BirthDay)
                    ActionLog += "����:" + oldCust.BirthDay + "=>" + newCust.BirthDay + " ;";

                if (oldCust.CustCountryID != newCust.CustCountryID)
                    ActionLog += "����:" + oldCountry + "=>" + newCountry + " ;";

                if (oldCust.Passport != newCust.Passport)
                    ActionLog += "����:" + oldCust.Passport + "=>" + newCust.Passport + " ;";
                if (oldCust.PassportExpiryDate != newCust.PassportExpiryDate)
                    ActionLog += "������Ч��:" + oldCust.PassportExpiryDate + "=>" + newCust.PassportExpiryDate + " ;";
                if (oldCust.RequireVisa != newCust.RequireVisa)
                    ActionLog += "ÿ�ζ�ǩ֤:" + oldCust.RequireVisa + "=>" + newCust.RequireVisa + " ;";
                if (oldCust.ParentCompany != newCust.ParentCompany)
                    ActionLog += "������˾:" + oldCust.ParentCompany + "=>" + newCust.ParentCompany + " ;";
                if (oldCust.Position != newCust.Position)
                    ActionLog += "ְλ:" + oldCust.Position + "=>" + newCust.Position + " ;";
                if (oldCust.UseOwnMoney != newCust.UseOwnMoney)
                    ActionLog += "���Լ�Ǯ:" + oldCust.UseOwnMoney + "=>" + newCust.UseOwnMoney + " ;";

                if (oldCust.CustEmpNumID != newCust.CustEmpNumID)
                    ActionLog += "��˾��ģ:" + oldCustEmpNum + "=>" + newCustEmpNum + " ;";

                if (oldCust.FavoriteProd != newCust.FavoriteProd)
                    ActionLog += "ϲ�ú��չ�˾:" + oldCust.FavoriteProd + "=>" + newCust.FavoriteProd + " ;";
                if (oldCust.PreferPrice != newCust.PreferPrice)
                    ActionLog += "��λϲ��:" + oldCust.PreferPrice + "=>" + newCust.PreferPrice + " ;";
                if (oldCust.PreferPlace != newCust.PreferPlace)
                    ActionLog += "��ɵص�:" + oldCust.PreferPlace + "=>" + newCust.PreferPlace + " ;";
                if (oldCust.FavoriteDest != newCust.FavoriteDest)
                    ActionLog += "��ȥ����:" + oldCust.FavoriteDest + "=>" + newCust.FavoriteDest + " ;";
                if (oldCust.Budget != newCust.Budget)
                    ActionLog += "����Ԥ��:" + oldCust.Budget + "=>" + newCust.Budget + " ;";
                if (oldCust.TravelDay != newCust.TravelDay)
                    ActionLog += "ÿ�����ʱ��:" + oldCust.TravelDay + "=>" + newCust.TravelDay + " ;";

                if (IsNull(oldCust.BuddyList) != newCust.BuddyList)
                    ActionLog += "��������:" + GetBuddyNameList(oldCust.BuddyList) +
                        "=>" + GetBuddyNameList(newCust.BuddyList) + " ;";
            }
            //chemical
            if (ConfigurationManager.AppSettings["Industry"].ToLower() == "chemical")
            {
                if (oldCust.CustPort != newCust.CustPort)
                    ActionLog += "�ӻ��ۿ�:" + oldCust.CustPort + "=>" + newCust.CustPort + " ;";
                if (oldCust.CustPurchaseChannels != newCust.CustPurchaseChannels)
                    ActionLog += "��������:" + oldCust.CustPurchaseChannels + "=>" + newCust.CustPurchaseChannels + " ;";
                if (oldCust.CustSaleChannels != newCust.CustSaleChannels)
                    ActionLog += "����;��:" + oldCust.CustSaleChannels + "=>" + newCust.CustSaleChannels + " ;";
                if (oldCust.CommissionFactor != newCust.CommissionFactor)
                    ActionLog += "�ͻ�ϵ��:" + oldCust.CommissionFactor + "=>" + newCust.CommissionFactor + " ;";
                if (oldCust.CustMaterial != newCust.CustMaterial)
                    ActionLog += "ʹ���ҹ�˾��Ʒ�����:" + oldCust.CustMaterial + "=>" + newCust.CustMaterial + " ;";
                if (oldCust.CustProduct != newCust.CustProduct)
                    ActionLog += "�ͻ�������Ʒ:" + oldCust.CustProduct + "=>" + newCust.CustProduct + " ;";
                if (oldCust.CustInfo != newCust.CustInfo)
                    ActionLog += "�ɹ�ϰ��:" + oldCust.CustInfo + "=>" + newCust.CustInfo + " ;";
            }
            Log.ActionLog = ActionLog;

            this.CRMAuditLogs.InsertOnSubmit(Log);
        }

        public void AddAuditLog(long CustID, long ActionBy, string LogInfo)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();

            CRMAuditLog Log = new CRMAuditLog();
            Log.Action = "SaveCustomer"; //������ʾʱ��ת�ɶ����� SaveCustomer,SaveDeal,SaveUser
            Log.ActionAt = DateTime.Now;
            Log.ActionBy = ActionBy;
            Log.PKId = CustID;
            Log.ActionLog = LogInfo;
            this.CRMAuditLogs.InsertOnSubmit(Log);
            this.dataCtx.SubmitChanges();
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State != ConnectionState.Closed)
                    this.dataCtx.Connection.Close();

        }
        public DataTable GetAuditLog(long custID, string CreateDate, string CreateUser, string CreateUserID)
        {
            string sql = @"select g.ActionLog, 
            g.ActionBy, g.ActionAt,u.UserName from CRMAuditLog g inner join CRMUser u 
            on g.ActionBy=u.UserID where PKId=" + custID.ToString() + " and Action='SaveCustomer' order by ActionAt";
            DataTable dt = DBExtBase.ExeFillTblBySqlText(this.dataCtx, sql);
            DataRow dr = dt.NewRow();
            dr["ActionLog"] = "�½��ͻ�����";
            dr["ActionBy"] = CreateUserID;
            dr["ActionAt"] = CreateDate;
            dr["UserName"] = CreateUser;
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }

        public void ApprovePendingCustomer(CRMCustomer entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMCustomers
                          where t.CustID == entity.CustID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                {
                    obj.Status = "A";

                }

                //����ͻ���Ϣ,�õ����µ�entity.CustID
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

        public void RejectPendingCustomer(CRMCustomer entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                long MaxVersion = CRMCustomerHistorys.Where(t => t.CustID == entity.CustID).Max(t => t.Version);
                var qry = from t in CRMCustomerHistorys
                          where t.CustID == entity.CustID && t.Version == MaxVersion
                          select t;
                var OldCust = qry.SingleOrDefault();
                if (OldCust != null)
                {
                    //����ʷ��ɼ�¼copy��Customer��
                    this.CopyEntity(entity, OldCust);

                }

                //����ͻ���Ϣ,�õ����µ�entity.CustID
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

        public CRMCustomerDeal SaveDeal(CRMCustomerDeal entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMCustomerDeals
                          where t.ID == entity.ID
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                {
                    entity.CreateDate = obj.CreateDate;
                    entity.CreateUserID = obj.CreateUserID;
                    this.CopyEntity(obj, entity);
                }

                else
                {
                    entity.CreateDate = DateTime.Now;
                    entity.CreateUserID = entity.ModifyUserID;
                    this.CRMCustomerDeals.InsertOnSubmit(entity);
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

        //public void DeleteById(object Id)
        //{
        //    long lngID = long.Parse((string)Id);
        //    var qry = from t in CRMCustomers
        //              where t.CustID == lngID
        //              select t;
        //    var obj = qry.SingleOrDefault();
        //    CRMCustomers.DeleteOnSubmit(obj);
        //    this.dataCtx.SubmitChanges();
        //}

        public CRMCustomerProd GetProdUsageBrand(string CustID, string ProdID)
        {
            var qryCP = from t in CRMCustomerProds
                        where t.CustID == long.Parse(CustID) && t.ProdID == long.Parse(ProdID)
                        select t;
            return qryCP.SingleOrDefault();
        }

        public string GetBuddyNameList(string BuddyIDList)
        {
            if (string.IsNullOrEmpty(BuddyIDList))
                return "";
            string[] arrBuddy = BuddyIDList.Split('|');
            string seperator = ", ";
            string strBuddyNameList = "";
            foreach (string buddy in arrBuddy)
            {
                var obj = LoadById(buddy);
                if (obj != null)
                {
                    string buddyName = obj.CustName;
                    strBuddyNameList += buddyName + seperator;
                }
            }
            return strBuddyNameList.Substring(0, strBuddyNameList.Length - 1);
        }
        /// <summary>
        /// Ҫ����ContractNum,������AmadeusԤ���Ź���
        /// </summary>
        public void BuddyAnalysis()
        {
            string sql = @"select distinct d.custid,ContractNum from crmCustomerDeal d
                            where ContractNum in (
                            select ContractNum from crmCustomerDeal 
                            group by ContractNum having count(custID)>1 )";
            DataTable dt = DBExtBase.ExeFillTblBySqlText(dataCtx, sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string custID = dt.Rows[i]["custid"].ToString();
                string ContractNum = dt.Rows[i]["ContractNum"].ToString();
                string oldBuddyList = DBExtBase.ExeScalarBySqlText(dataCtx, "select buddylist from CRMCustomer where custID=" + custID).ToString();
                string[] arrOldBuddy = oldBuddyList.Split('|');
                //ͬһ�ŵ���������
                DataRow[] arr = dt.Select("ContractNum='" + ContractNum + "' and custid<>" + custID);
                string buddylist = "";
                for (int j = 0; j < arr.Length; j++)
                {
                    if (arrOldBuddy.Contains(arr[j]["custid"]) == false)
                        buddylist += arr[j]["custid"] + "|";
                }
                if (buddylist.Length > 0)
                {
                    buddylist = buddylist.Substring(0, buddylist.Length - 1);
                    DBExtBase.ExeNonQueryBySqlText(dataCtx, "update CRMCustomer set BuddyList='" + buddylist + "' where custID=" + custID);
                }
            }


        }

        /// <summary>
        /// �Ƿ������Ŀͻ�
        /// </summary>
        /// <param name="LoginUserID"></param>
        /// <param name="CustOwnerID"></param>
        /// <returns></returns>
        public bool IsSubordinateCustomer(long LoginUserID, string CustOwnerID)
        {
            String sql = "select count(*) from dbo.GetSubordinateUser(" + LoginUserID + ") where userid=" + CustOwnerID;
            if ((int)DBExtBase.ExeScalarBySqlText(dataCtx, sql) > 0)
                return true;
            else
                return false;
        }
    }
}

