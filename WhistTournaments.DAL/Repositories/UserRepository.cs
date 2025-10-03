using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhistTournaments.DL.Entities;
using WhistTournaments.DL.Enums;

namespace WhistTournaments.DAL.Repositories
{
    public class UserRepository
    {
        //private string _connectionstring = "server=10.2.28.135;database=WhistDB;uid=sa;pwd=test1234=;trustServerCertificate=true";
        private string _connectionstring = "Server=(localdb)\\MSSQLLocalDB;Database=WhistDB;Trusted_Connection=True;";

        public bool AddUser(User user) 
        {
            using(SqlConnection connection = new SqlConnection(_connectionstring))
            using(SqlCommand command = connection.CreateCommand()) 
            {
                command.CommandText="INSERT INTO [USER] ([NAME], [FIRST_NAME], [USER_NAME], [PASSWORD], [EMAIL], [GENDER], [ROLE]) VALUES (@firstname, @lastname, @username, @password, @email, @gender, 1)";
                command.Parameters.AddWithValue("@firstname", user.FirstName);
                command.Parameters.AddWithValue("@lastname", user.LastName);
                command.Parameters.AddWithValue("@username", user.UserName);
                command.Parameters.AddWithValue("@password", user.Password);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@gender", user.Gender);
                connection.Open();
                return command.ExecuteNonQuery()==1 ? true : false;
            }
        }
        public string? GetUsernameById(int? id)
        {
            using(SqlConnection connection = new SqlConnection(_connectionstring))
            using(SqlCommand command = connection.CreateCommand())
            {
                if (id != null)
                {
                    command.CommandText = "SELECT [USER_NAME] FROM [USER] WHERE [ID]=@id;";
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    string username = (string)command.ExecuteScalar();
                    return username;
                }

                return null;

            }
        }

        public User GetUserById(int id)
        {
            User user = new User();
            using(SqlConnection connection = new SqlConnection(_connectionstring))
            using(SqlCommand command = connection.CreateCommand())
            {
                command.CommandText="SELECT * FROM [USER] WHERE [ID]=@id;";
                command.Parameters.AddWithValue("@id",id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read()) 
                {
                    user.UserName=(string)reader["user_name"];
                    user.Email=(string)reader["Email"];
                    user.Gender=(Gender)reader["gender"];
                    user.Role=(Role)reader["role"];
                }
                return user;
            }
        }

        public User? GetUserByUsername(string username)
        {
            User? user = new User();
            using(SqlConnection connection = new SqlConnection(_connectionstring))
            using(SqlCommand command = connection.CreateCommand())
            {
                command.CommandText="SELECT * FROM [USER] WHERE [USER_NAME]=@username;";
                command.Parameters.AddWithValue("@username",username);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    user.Id = (int)reader["Id"];
                    user.UserName=(string)reader["user_name"];
                    user.FirstName=(string)reader["first_name"];
                    user.LastName=(string)reader["name"];
                    user.Email=(string)reader["Email"];
                    user.Gender=(Gender)reader["gender"];
                    user.Role=(Role)reader["role"];
                    user.Password=(string)reader["Password"];
                }
                else
                {
                    return null;
                }
                return user;
            }
        }
    }
}
