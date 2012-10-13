using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Terry.CRM.Service;
using Terry.CRM.Entity;

namespace Terry.CRM.Web.Service
{
    /// <summary>
    /// Customer 的摘要说明
    /// </summary>
    public class Customer : IHttpHandler
    {
        private CustomerService svr = new CustomerService();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Cache.SetNoStore();
            //context.Response.Write("Hello World");
            string query = context.Request["query"];
            string LoginUserCompany = context.Request["com"];
            //----get auto complete data-------------

            string styleNumOrName = query;
            int RecordCount =0;
            string Filter = "CustTel.Contains(\"" + query + "\") or CustName.Contains(\""+  query + "\")";
            IList<vw_CRMCustomer2> CustList = svr.SearchByCriteria(-1, -1, out RecordCount, Filter, "CustName", null, null, null, 0);
            //----------------------------------------

            string JSON = string.Empty;
            JSON = "{ query:'" + query + "',suggestions:[";
            if (CustList.Count > 0)
            {
                foreach (vw_CRMCustomer2 item in CustList)
                {
                    JSON += "'" + item.CustFullName + "__" + item.CustTel
                         + "__" + item.CustEmail + "__" + item.CustAddress + "__" + item.ParentCompany + "',";
                }
                //del last commar
                JSON = JSON.Substring(0, JSON.Length - 1);

            }

            JSON += "],data:[";
            if (CustList.Count > 0)
            {
                foreach (vw_CRMCustomer2 item in CustList)
                {
                    JSON += "'" + item.CustID  + "',";
                }
                //del last commar
                JSON = JSON.Substring(0, JSON.Length - 1);
            }
            JSON += "]}";
            context.Response.Write(JSON);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}