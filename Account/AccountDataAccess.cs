using System;
using Microsoft;
using System.Collections.Generic;
using System.Data;

using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace BankingV1._8.Account
{
    class AccountDataAccess
    {
        // store values in DB
        public bool Store(SqlParameter[] parameters)
        {
            SqlConnection connection = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BankingConnection"].ConnectionString;
                connection = new SqlConnection(connectionString);

                /*
                SqlCommand cmd = new SqlCommand("insert into [account]" +
                    "(AccountNumber,AccountAlias,AccountType,Balance,UserID,Interest,CreditLimit,DepositLimit,CreatedAt)" +
                   " values(@number,@alias,@type,@balance,@userID,@interest,@creditLimit,@depositLimit,@createdAt)", connection);
                */
                SqlCommand cmd = new SqlCommand("sp_insertaccount", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);
                connection.Open();
                // Execute the command
                cmd.ExecuteNonQuery();//INSERT, DELETE, UPDATE, and SET
                
                return true;
            }
            catch(ConfigurationErrorsException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return false;
        }
        public DataSet GetAllAccountsFromDB()
        {
            DataSet ds = new DataSet();
            SqlConnection connection = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BankingConnection"].ConnectionString;
                connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("select * from tbl_Account", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                connection.Open();
                adapter.Fill(ds);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return ds;

        }


        public DataSet SearchAccountByUser(SqlParameter parameter)
        {
            DataSet ds = new DataSet();
            SqlConnection connection =null ;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BankingConnection"].ConnectionString;
                connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("select * from tbl_Account where userId = @userID", connection);
                cmd.Parameters.Add(parameter);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                connection.Open();
                adapter.Fill(ds);
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            
            return ds;
        }
        public DataSet SearchAccountByNumber(SqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            SqlConnection connection = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BankingConnection"].ConnectionString;
                connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("select * from tbl_Account where userId = @userID and AccountID=@accountID", connection);
                cmd.Parameters.AddRange(parameters);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                connection.Open();
                adapter.Fill(ds);
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
           
            return ds;
        }

        public bool Update(SqlParameter[] parameters)
        {
            SqlConnection connection = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BankingConnection"].ConnectionString;
                connection = new SqlConnection(connectionString);

            /*
                SqlCommand cmd = new SqlCommand("UPDATE account set " +
                    "AccountNumber=@number,AccountAlias=@alias,Balance=@balance,Interest=@interest," +
                    "CreditLimit=@creditLimit,DepositLimit=@depositLimit where UserID=@userID and AccountID=@accountID", connection);
            */
                SqlCommand cmd = new SqlCommand("sp_updateaccount", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);
                connection.Open();
                int res=cmd.ExecuteNonQuery();//INSERT, DELETE, UPDATE
                if (res > 0)
                    return true;
                else
                    return false;
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return false;
            
        }

        public bool Destroy(SqlParameter[] parameters)
        {
            SqlConnection connection = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BankingConnection"].ConnectionString;
                connection = new SqlConnection(connectionString);

                //SqlCommand cmd = new SqlCommand("DELETE from account where UserID=@userID and AccountID=@accountID", connection);
                SqlCommand cmd = new SqlCommand("sp_deleteaccount", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);
                connection.Open();
                int res = cmd.ExecuteNonQuery();//INSERT, DELETE, UPDATE, and SET
                if (res > 0)
                    return true;
                else
                    return false;
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return false;
        }

    }
}
