using BankingV1._8.Menu;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._8.UserFolder
{
    class UserDataAccess
    {
        public bool Store(SqlParameter[] parameters)
        {
            SqlConnection connection = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BankingConnection"].ConnectionString;
                connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("sp_insertuser", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);
                connection.Open();
                // Execute the command
                int res = cmd.ExecuteNonQuery();

                if (res == 0)
                {
                    Console.WriteLine("Someone already has this email address. Try again, please.\n");
                }
                else if (res == -1)
                {
                    Console.WriteLine("There was a problem. Try again, please.\n");
                }
                else if (res == 1)
                    return true;

                return false;
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                connection.Close();
            }

            return false;
        }
        public User Search(SqlParameter[] parameters)
        {
            SqlConnection connection = null;
            try
            {
                DataSet ds = new DataSet();
                string connectionString = ConfigurationManager.ConnectionStrings["BankingConnection"].ConnectionString;
                connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("select * from [tbl_user] " +
                    "where email = @email and password = @password", connection);
                cmd.Parameters.AddRange(parameters);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                connection.Open();
                //Get the data in disconnected mode
                adapter.Fill(ds);

                DataRow user = ds.Tables[0].Rows[0];
                //BankMenu.userID = Convert.ToInt32(user["userID"]);
                User u = new User(Convert.ToInt32(user["userID"]),user["email"].ToString(), user["password"].ToString(), user["firstName"].ToString(), user["lastName"].ToString());
                UserBO.user = u;
                return u;
                
            }catch (IndexOutOfRangeException)
            {
                //User not found
                return null;
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
            return null;
        }
        //its not neccesary
        public User SearchByID(SqlParameter parameter)
        {
            SqlConnection connection = null;
            try
            {
                DataSet ds = new DataSet();
                string connectionString = ConfigurationManager.ConnectionStrings["BankingConnection"].ConnectionString;
                connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("select * from [tbl_user] " +
                    "where UserID=@userID", connection);
                cmd.Parameters.Add(parameter);
                // Execute the command
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                connection.Open();
                //Get the data in disconnected mode
                adapter.Fill(ds);

                DataRow user = ds.Tables[0].Rows[0];
                User u = new User(Convert.ToInt32(user["userID"]), user["email"].ToString(), user["password"].ToString(), user["firstName"].ToString(), user["lastName"].ToString());
                Console.WriteLine(u.FirstName + " id=" + u.UserID);
                return u;

            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                connection.Close();
            }
            return null;
        }


    }
}
