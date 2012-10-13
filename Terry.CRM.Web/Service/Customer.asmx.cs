using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Terry.CRM.Entity;
using Terry.CRM.Service;

namespace Terry.CRM.Web.Service
{
    /// <summary>
    /// CustomerASMX 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://www.2simplework.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class CustomerASMX : System.Web.Services.WebService
    {
        private CustomerService svr = new CustomerService();

        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}
        [WebMethod]
        public string GetCustInfo(string query)
        {
            int RecordCount = 0;
            string Filter = "CustTel.Contains(\"" + query + "\") or CustName.Contains(\"" + query + "\")";
            //只取前10个
            IList<vw_CRMCustomer2> CustList = svr.SearchByCriteria(0,10, out RecordCount, Filter, "CustName", null, null, null, 0);
            //----------------------------------------
            if (CustList.Count > 0)
            {
                vw_CRMCustomer2 cust = CustList[0];

                string CustInfo = string.Empty;
                CustInfo = cust.CustID + "$$" + cust.CustName + "$$" + cust.CustType + "$$" + cust.FavoriteProd
                    + "$$" + cust.PreferPrice + "$$" + cust.PreferPlace;

                string DealInfo = "$$$$";

                Filter = " and CustID=" + cust.CustID.ToString();
                string OrderBy = "DealDate Desc";

                var dtDeal = svr.SearchByCriteria(typeof(vw_CRMCustomerDeal), 0, 1,
                    out RecordCount, Filter, OrderBy);
                if (dtDeal.Rows.Count > 0)
                    DealInfo = dtDeal.Rows[0]["ContractNum"] + "$$" + dtDeal.Rows[0]["TotalAmount"] + "$$" + dtDeal.Rows[0]["DealOwnerName"];
                return CustInfo + "$$" + DealInfo;
            }
            else
                return "$$ $$ $$ $$ $$ $$ $$ $$ ";
            

        }
    }
}
