using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace DAL
{
    /// <summary>
    /// Include all the database operation here
    /// </summary>
    /// 
    public class DataAccessLayer
    {
        private ConnectionStringSettings connSetting;
        
        public DataAccessLayer()
        {
            connSetting = ConfigurationManager.ConnectionStrings["MISConnectionString"];
        }
        
        public int OperateOnTable(string theCommandText)
        {
            using (SqlConnection con1 = new SqlConnection(connSetting.ConnectionString))
            {
                con1.Open();

                SqlCommand com = new SqlCommand(theCommandText, con1);
                SqlTransaction transaction = con1.BeginTransaction();
                com.Transaction = transaction;

                try
                {
                    com.ExecuteNonQuery();
                    transaction.Commit();
                }

                catch (Exception e)
                {
                    try
                    {
                        transaction.Rollback();
                        return (0);
                    }
                    catch (SqlException ex)
                    {
                        if (transaction.Connection != null)
                        {
                            //ex.Message;
                        }
                    }
                }

                finally
                {
                    con1.Close();
                }
            }

            return (1);
        }
        
        public int ExecuteStoredProcedure(string nameStoredProcedure, string[] fieldsName, string[] fields)
        {
            using (SqlConnection conn = new SqlConnection(connSetting.ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = nameStoredProcedure;

                try
                {
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;

                    for (int i = 0; i < fields.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@" + fieldsName[i], fields[i]);

                    }
                    cmd.ExecuteNonQuery();
                    transaction.Commit();

                }

                catch (Exception e)
                {
                    try
                    {
                        transaction.Rollback();
                        return (0);
                    }
                    catch (SqlException ex)
                    {
                        if (transaction.Connection != null)
                        {
                            //ex.Message;
                        }
                    }
                }

                finally
                {
                }
            }

            return (1);
        }
        
        public int ExecuteStoredProcedureWithoutFieldsName(string nameStoredProcedure)
        {
            using (SqlConnection conn = new SqlConnection(connSetting.ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = nameStoredProcedure;

                try
                {
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                }

                catch (Exception e)
                {
                    try
                    {
                        transaction.Rollback();
                        return (0);
                    }
                    catch (SqlException ex)
                    {
                        if (transaction.Connection != null)
                        {
                            //ex.Message;
                        }
                    }
                }

                finally
                {
                }
            }
            
            return (1);
        }
        
        public int ReturnExistingRowCountFor(string sql)
        {
            int rowCount = 0;

            using (SqlConnection conn = new SqlConnection(connSetting.ConnectionString))
            {
                conn.Open();

                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand(sql, conn);
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                    rowCount = Int32.Parse(myReader["count"].ToString());

                conn.Close();
            }

            return (rowCount);
        }
        
        public int ReturnIntValueFor(string sql)
        {
            int value = 0;

            using (SqlConnection conn = new SqlConnection(connSetting.ConnectionString))
            {
                conn.Open();

                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand(sql, conn);
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                    value = Int32.Parse(myReader["value"].ToString());

                conn.Close();
            }

            return (value);
        }
        
        public string ReturnStringValueFor(string sql)
        {
            string value = "";

            using (SqlConnection conn = new SqlConnection(connSetting.ConnectionString))
            {
                conn.Open();

                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand(sql, conn);
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                    value = myReader["value"].ToString();

                conn.Close();
            }

            return (value);
        }
        
        public double ReturnDoubleValueFor(string sql)
        {
            double value = 0;

            using (SqlConnection conn = new SqlConnection(connSetting.ConnectionString))
            {
                conn.Open();

                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand(sql, conn);
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                    value = Double.Parse(myReader["value"].ToString());

                conn.Close();
            }

            return (value);
        }
        
        public long ReturnLongValueFor(string sql)
        {
            long value = 0;

            using (SqlConnection conn = new SqlConnection(connSetting.ConnectionString))
            {
                conn.Open();

                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand(sql, conn);
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                    value = long.Parse(myReader["value"].ToString());

                conn.Close();
            }

            return (value);
        }
        
        public DataTable FillDataSource(string sql)
        {
            using (SqlConnection con = new SqlConnection(connSetting.ConnectionString))
            {
                SqlDataAdapter dap = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                return dt;
            }
        }
    }
}
