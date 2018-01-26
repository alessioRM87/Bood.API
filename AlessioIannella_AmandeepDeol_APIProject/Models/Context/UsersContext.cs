using AlessioIannella_AmandeepDeol.API.Models.Requests;
using AlessioIannella_AmandeepDeol_APIProject.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlessioIannella_AmandeepDeol.API.Models.Context
{
    public class UsersContext
    {
        public string ConnectionString { get; set; }

        public UsersContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public async Task<List<User>> GetAllUsers()
        {
            List<User> list = new List<User>();

            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM DBUser", connection);

            var reader = cmd.ExecuteReader();

            while (await reader.ReadAsync())
            {
                User user = new User()
                {
                    UserID = Convert.ToInt32(reader["userID"]),
                    Username = reader["username"].ToString(),
                    Email = reader["email"].ToString(),
                    Password = "",
                    First_Name = reader["first_name"].ToString(),
                    Last_Name = reader["last_name"].ToString(),
                };

                list.Add(user);
            }

            reader.Close();

            await connection.CloseAsync();

            return list;
        }

        public async Task<User> GetUserByID(int userID)
        {
            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM DBUser WHERE userID = @userID", connection);
            cmd.Parameters.AddWithValue("@userID", userID);

            var reader = cmd.ExecuteReader();

            User user = null;

            while (await reader.ReadAsync())
            {
                user = new User()
                {
                    UserID = Convert.ToInt32(reader["userID"]),
                    Username = reader["username"].ToString(),
                    Email = reader["email"].ToString(),
                    Password = "",
                    First_Name = reader["first_name"].ToString(),
                    Last_Name = reader["last_name"].ToString(),
                };
            }

            reader.Close();

            await connection.CloseAsync();

            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM DBUser WHERE email = @email", connection);
            cmd.Parameters.AddWithValue("@email", email);

            var reader = cmd.ExecuteReader();

            User user = null;

            while (await reader.ReadAsync())
            {
                user = new User()
                {
                    UserID = Convert.ToInt32(reader["userID"]),
                    Username = reader["username"].ToString(),
                    Email = reader["email"].ToString(),
                    Password = "",
                    First_Name = reader["first_name"].ToString(),
                    Last_Name = reader["last_name"].ToString(),
                };
            }

            reader.Close();

            await connection.CloseAsync();

            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM DBUser WHERE username = @username", connection);
            cmd.Parameters.AddWithValue("@username", username);

            var reader = cmd.ExecuteReader();

            User user = null;

            while (await reader.ReadAsync())
            {
                user = new User()
                {
                    UserID = Convert.ToInt32(reader["userID"]),
                    Username = reader["username"].ToString(),
                    Email = reader["email"].ToString(),
                    Password = "",
                    First_Name = reader["first_name"].ToString(),
                    Last_Name = reader["last_name"].ToString(),
                };
            }

            reader.Close();

            await connection.CloseAsync();

            return user;
        }

        public async Task<int> SaveUser(User user)
        {
            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand myCommand = new MySqlCommand("INSERT INTO DBUser (username, email, password, first_name, last_name) VALUES (@username, @email, @password, @first_name, @last_name)", connection);
            myCommand.Parameters.AddWithValue("@username", user.Username);
            myCommand.Parameters.AddWithValue("@email", user.Email);
            myCommand.Parameters.AddWithValue("@password", user.Password);
            myCommand.Parameters.AddWithValue("@first_name", user.First_Name);
            myCommand.Parameters.AddWithValue("@last_name", user.Last_Name);

            var result = await myCommand.ExecuteScalarAsync();

            int userID = -1;

            if (result == null)
            {
                userID = Convert.ToInt32(myCommand.LastInsertedId);
            }
            else
            {
                userID = Convert.ToInt32(result);
            }

            await connection.CloseAsync();

            return userID;

        }

        public async Task<int> UpdateUser(int userID, User user)
        {
            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand myCommand = new MySqlCommand("UPDATE DBUser SET username = @username, email = email, first_name = @first_name, last_name = @last_name WHERE userID = @userID", connection);
            myCommand.Parameters.AddWithValue("@username", user.Username);
            myCommand.Parameters.AddWithValue("@email", user.Email);
            myCommand.Parameters.AddWithValue("@first_name", user.First_Name);
            myCommand.Parameters.AddWithValue("@last_name", user.Last_Name);
            myCommand.Parameters.AddWithValue("@userID", userID);

            await myCommand.ExecuteNonQueryAsync();

            await connection.CloseAsync();

            return userID;
        }

        public async void DeleteUser(int userID)
        {
            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand myCommand = new MySqlCommand("DELETE FROM DBUser WHERE userID = @userID", connection);
            myCommand.Parameters.AddWithValue("@userID", userID);

            await myCommand.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }

        public async Task<User> Login(Login login)
        {
            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM DBUser WHERE username = @username AND password = @password", connection);
            cmd.Parameters.AddWithValue("@username", login.Username);
            cmd.Parameters.AddWithValue("@password", login.Password);

            var reader = cmd.ExecuteReader();

            User user = null;

            while (await reader.ReadAsync())
            {
                user = new User()
                {
                    UserID = Convert.ToInt32(reader["userID"]),
                    Username = reader["username"].ToString(),
                    Email = reader["email"].ToString(),
                    Password = "",
                    First_Name = reader["first_name"].ToString(),
                    Last_Name = reader["last_name"].ToString(),
                };
            }

            reader.Close();

            await connection.CloseAsync();

            return user;
        }
    }
}
