using BankingV1._8.Menu;
using BankingV1._8.UserFolder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BankingV1._8.Account.Log
{
    class OperationDataAccess
    {
        public bool Store(SqlParameter[] parameters)
        {
            SqlConnection connection = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BankingConnection"].ConnectionString;
                connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("sp_insertOperation", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);
                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
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
        public Operation Search(SqlParameter[] parameters)
        {
            SqlConnection connection = null;
            try
            {
                DataSet ds = new DataSet();
                string connectionString = ConfigurationManager.ConnectionStrings["BankingConnection"].ConnectionString;
                connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("select * from tbl_Operation " +
                    "where email = @email and password = @password", connection);
                cmd.Parameters.AddRange(parameters);
                // Execute the command
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                connection.Open();
                //Get the data in disconnected mode
                adapter.Fill(ds);                
            }catch (IndexOutOfRangeException)
            {
                //User not found
                return null;
            }catch (Exception)
            {
                Console.WriteLine("Error");
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
                // Create and initialize the connection using connection string
                connection = new SqlConnection(connectionString);
                //Define Command Type
                SqlCommand cmd = new SqlCommand("select * from [tbl_User] " +
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
                //User not found
                return null;
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
            }
            finally
            {
                connection.Close();
            }
            return null;
        }


    }
}
