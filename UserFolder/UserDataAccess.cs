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
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BankingConnection"].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("sp_insertuser", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                //SqlCommand cmd = new SqlCommand("insert into [tbl_user](Email,Password,FirstName,LastName) values(@email,@password,@firstname,@lastname)", connection);
                cmd.Parameters.AddRange(parameters);
                connection.Open();
                // Execute the command
                cmd.ExecuteNonQuery();//INSERT, DELETE, UPDATE  
                connection.Close();
                return true;
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                //Error number 2627 in the THROW statement is outside the valid range. Specify an error number in the valid range of 50000 to 2147483647.
                if (ex.Number == 35100)
                {
                    Console.WriteLine("Someone already has this email address. Try again, please.");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                //2601
                Console.WriteLine(ex.Data.Keys);
                Console.WriteLine(ex.Message);
            }

            return false;
        }
        public User Search(SqlParameter[] parameters)
        {
            try
            {
                DataSet ds = new DataSet();
                string connectionString = ConfigurationManager.ConnectionStrings["BankingConnection"].ConnectionString;
                // Create and initialize the connection using connection string
                SqlConnection connection = new SqlConnection(connectionString);
                //Define Command Type
                SqlCommand cmd = new SqlCommand("select * from [tbl_user] " +
                    "where email = @email and password = @password", connection);
                cmd.Parameters.AddRange(parameters);
                // Execute the command
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                connection.Open();
                //Get the data in disconnected mode
                adapter.Fill(ds);
                connection.Close();

                DataRow user = ds.Tables[0].Rows[0];
                BankMenu.userID = Convert.ToInt32(user["userID"]);
                User u = new User(Convert.ToInt32(user["userID"]),user["email"].ToString(), user["password"].ToString(), user["firstName"].ToString(), user["lastName"].ToString());
                Console.WriteLine(u.FirstName + " id=" + u.UserID); 
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
            return null;
        }
        //its not neccesary
        public User SearchByID(SqlParameter parameter)
        {
            try
            {
                DataSet ds = new DataSet();
                //string connectionString = "Data Source=ASPLAPLTM057;Initial Catalog=banking;User ID=sa;Password=aspire123;Encrypt=True;TrustServerCertificate=True;";
                string connectionString = ConfigurationManager.ConnectionStrings["BankingConnection"].ConnectionString;
                // Create and initialize the connection using connection string
                SqlConnection connection = new SqlConnection(connectionString);
                //Define Command Type
                SqlCommand cmd = new SqlCommand("select * from [tbl_user] " +
                    "where UserID=@userID", connection);
                cmd.Parameters.Add(parameter);
                // Execute the command
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                connection.Open();
                //Get the data in disconnected mode
                adapter.Fill(ds);
                connection.Close();

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
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }


    }
}
