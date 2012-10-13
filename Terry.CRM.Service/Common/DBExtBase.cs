using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace Terry.CRM.Service
{
    class DBExtBase
    {
        public static SqlParameter[] getParameterList(DataTable tbParameter)
        {
            if (tbParameter == null || tbParameter.Rows.Count <= 0)
            {
                string strErrMessage = "Parameter Error";
                throw new Exception(strErrMessage);
            }
            int paraCount = tbParameter.Columns.Count;
            DataRow row = tbParameter.Rows[0];
            SqlParameter[] paramList = new SqlParameter[paraCount];
            for (int i = 0; i < paraCount; i++)
            {
                DataColumn col = tbParameter.Columns[i];
                paramList[i] = new SqlParameter();
                paramList[i].ParameterName = col.ColumnName;
                if (!string.IsNullOrEmpty(row[col].ToString()))
                    paramList[i].Value = row[col];
                else
                    paramList[i].Value = DBNull.Value;

            }
            return paramList;
        }

        public static SqlParameter[] getParameterList(string[] strParameterName, object[] strParameterValue)
        {
            if (strParameterName == null || strParameterValue == null || strParameterName.Length <= 0 || strParameterValue.Length <= 0)
            {
                string strErrMessage = "Parameter Error";
                throw new Exception(strErrMessage);
            }
            int length = strParameterValue.Length;
            SqlParameter[] paramList = new SqlParameter[length];
            for (int i = 0; i < strParameterValue.Length; i++)
            {
                paramList[i] = new SqlParameter();
                paramList[i].Direction = ParameterDirection.InputOutput;
                paramList[i].ParameterName = strParameterName[i].Trim();
                paramList[i].Value = strParameterValue[i].ToString().Trim();
            }
            return paramList;
        }
        public static void ExeFillTblBySP(DataContext ctx, DataTable table, string strStoredProcName, DataTable tbParameter)
        {
            DbConnection conn = ctx.Connection;
            SqlCommand cmd = (SqlCommand)conn.CreateCommand();
            if (ctx.Transaction != null)
                cmd.Transaction = (SqlTransaction)ctx.Transaction;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            SqlParameter[] paramList = null;
            if (tbParameter != null)
                paramList = getParameterList(tbParameter);
            try
            {
                cmd.CommandText = strStoredProcName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.CommandTimeout = 40;
                if (paramList != null)
                {
                    cmd.Parameters.AddRange(paramList);
                }
                ada.Fill(table);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ExeFillDsBySP(DataContext ctx, DataSet ds, string tblName, string strStoredProcName, DataTable tbParameter)
        {
            DbConnection conn = ctx.Connection;
            SqlCommand cmd = (SqlCommand)conn.CreateCommand();
            if (ctx.Transaction != null)
                cmd.Transaction = (SqlTransaction)ctx.Transaction;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            SqlParameter[] paramList = getParameterList(tbParameter);
            try
            {
                cmd.CommandText = strStoredProcName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                if (paramList != null)
                {
                    cmd.Parameters.AddRange(paramList);
                }
                ada.Fill(ds, tblName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ExeNonQueryBySp(DataContext ctx, string strStoredProcName, DataTable tbParameter)
        {
            DbConnection conn = ctx.Connection;
            SqlCommand cmd = (SqlCommand)conn.CreateCommand();
            if (ctx.Transaction != null)
                cmd.Transaction = (SqlTransaction)ctx.Transaction;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            SqlParameter[] paramList = getParameterList(tbParameter);
            try
            {
                cmd.CommandText = strStoredProcName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                if (paramList != null)
                {
                    cmd.Parameters.AddRange(paramList);
                }

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void ExeFillDsBySP(DataContext ctx, DataSet ds, string tblName, string strStoredProcName)
        {
            DataTable tbParameter = null;
            ExeFillDsBySP(ctx, ds, tblName, strStoredProcName, tbParameter);
        }

        public static void ExeFillTblBySP(DataContext ctx, DataTable table, string strStoredProcName)
        {
            DataTable tbParameter = null;
            ExeFillTblBySP(ctx, table, strStoredProcName, tbParameter);
        }

        public static void ExeNonQueryBySp(DataContext ctx, string strStoredProcName)
        {
            DataTable tbParameter = null;
            ExeNonQueryBySp(ctx, strStoredProcName, tbParameter);
        }
        public static void ExeBySP(DataContext ctx, DataTable table, string strStoredProcName, SqlParameter[] paramList)
        {
            DbConnection conn = ctx.Connection;
            SqlCommand cmd = (SqlCommand)conn.CreateCommand();
            if (ctx.Transaction != null)
                cmd.Transaction = (SqlTransaction)ctx.Transaction;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            try
            {
                cmd.CommandText = strStoredProcName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                if (paramList != null)
                {
                    cmd.Parameters.AddRange(paramList);
                }
                ada.Fill(table);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ExeFillDsBySqlText(DataContext ctx, DataSet ds, string tblName, string strSql)
        {
            DbConnection conn = ctx.Connection;
            SqlCommand cmd = (SqlCommand)conn.CreateCommand();
            if (ctx.Transaction != null)
                cmd.Transaction = (SqlTransaction)ctx.Transaction;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            try
            {
                cmd.CommandText = strSql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                ada.Fill(ds, tblName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ExeFillTblBySqlText(DataContext ctx, string strSql)
        {
            DbConnection conn = ctx.Connection;
            SqlCommand cmd = (SqlCommand)conn.CreateCommand();

            if (ctx.Transaction != null)
                cmd.Transaction = (SqlTransaction)ctx.Transaction;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            try
            {
                DataTable tb = new DataTable();
                cmd.CommandText = strSql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                ada.Fill(tb);
                return tb;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ExeNonQueryBySqlText(DataContext ctx, string strSql)
        {
            DbConnection conn = ctx.Connection;
            SqlCommand cmd = (SqlCommand)conn.CreateCommand();
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            if (ctx.Transaction != null)
                cmd.Transaction = (SqlTransaction)ctx.Transaction;

            try
            {
                cmd.CommandText = strSql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ExeScalarBySqlText(DataContext ctx, string strSql)
        {
            DbConnection conn = ctx.Connection;
            SqlCommand cmd = (SqlCommand)conn.CreateCommand();
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            if (ctx.Transaction != null)
                cmd.Transaction = (SqlTransaction)ctx.Transaction;

            try
            {
                cmd.CommandText = strSql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
