 
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
    public class GTDService: BaseService
    {

        public IList<CRMCalendar> SearchByCriteria(int CurrentPage, int PageSize, out int RecordCount, string Filter, string OrderBy)
        {
            if (OrderBy == "") OrderBy = "ID";
            var qry = from t in CRMCalendars                      
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
        /// <summary>
        /// 日历每日的单元格的的预约/任务对应的html
        /// </summary>
        /// <param name="day">yyyyMMdd</param>
        /// <param name="user">Terry</param>
        /// <returns></returns>
        public string GetDayTagHtml(string day, string user)
        {
            var qry = from t in CRMCalendars
                      where t.TaskDate.Date== DateTime.ParseExact(day,"yyyyMMdd",null)
                      && t.UserName == user 
                      orderby t.TaskDate
                      select t;
            var tasks = qry.ToList();
            string html = string.Empty;
            string symbol=string.Empty;
            string cssclass = "task";
            foreach (var task in tasks)
            {
                if (task.Status == 1)
                {
                    cssclass = "taskApproved";
                    symbol = "√";
                }
                else if (task.Status == 2)
                {
                    cssclass = "taskReject";
                    symbol = "×";
                }
                string Hour = "&nbsp;&nbsp;&nbsp;&nbsp;";
                if(task.TaskDate.ToString("%H")!="0")
                    Hour = task.TaskDate.ToString("%H") + "点 ";

                html += "<div align=left><a class='"+ cssclass + "' href='#' onclick=\"EditTask("+task.ID.ToString()+"," 
                    + task.TaskDate.ToString("yyyyMMdd") + ",'"
                    + task.UserName + "');\">" + Hour + task.Task + symbol + "</a></div>";
            }
            return html;
        }
        public CRMCalendar LoadById(object Id)
        {
            long lngID = long.Parse((string)Id);
            var qry = from t in CRMCalendars
                      where t.ID == lngID
                      select t;
            return qry.SingleOrDefault();
        }
        public CRMCalendar Save(CRMCalendar entity)
        {
            if (this.dataCtx.Connection != null)
                if (this.dataCtx.Connection.State == ConnectionState.Closed)
                    this.dataCtx.Connection.Open();
            DbTransaction tran = this.dataCtx.Connection.BeginTransaction();
            dataCtx.Transaction = tran;

            try
            {
                var qry = from t in CRMCalendars
                          where t.ID == entity.ID 
                          select t;
                var obj = qry.SingleOrDefault();
                if (obj != null)
                    this.CopyEntity(obj, entity);
                else
                    this.CRMCalendars.InsertOnSubmit(entity);

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
            var qry = from t in CRMCalendars
                      where t.ID == lngID
                      select t;
			var obj = qry.SingleOrDefault();
            CRMCalendars.DeleteOnSubmit(obj);
            this.dataCtx.SubmitChanges();
        }		


    }
}

		