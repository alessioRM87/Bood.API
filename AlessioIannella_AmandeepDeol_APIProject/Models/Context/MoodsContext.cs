using AlessioIannella_AmandeepDeol.API.Models.Requests;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlessioIannella_AmandeepDeol.API.Models.Context
{
    public class MoodsContext
    {
        public string ConnectionString { get; set; }

        public MoodsContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public async Task<List<Mood>> GetAllMoods()
        {
            List<Mood> list = new List<Mood>();

            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Mood", connection);

            var reader = cmd.ExecuteReader();

            while (await reader.ReadAsync())
            {
                Mood mood = new Mood()
                {
                    MoodID = Convert.ToInt32(reader["moodID"]),
                    Name = reader["name"].ToString(),
                };

                list.Add(mood);
            }

            reader.Close();

            await connection.CloseAsync();

            return list;
        }

        public async Task<Mood> GetMoodByID(int moodID)
        {
            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Mood WHERE moodID = @moodID", connection);
            cmd.Parameters.AddWithValue("@moodID", moodID);

            var reader = cmd.ExecuteReader();

            Mood mood = null;

            while (await reader.ReadAsync())
            {
                mood = new Mood()
                {
                    MoodID = Convert.ToInt32(reader["moodID"]),
                    Name = reader["name"].ToString(),
                };
            }

            reader.Close();

            await connection.CloseAsync();

            return mood;
        }

        public async Task<Mood> GetMoodByName(string name)
        {
            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Mood WHERE name = @name", connection);
            cmd.Parameters.AddWithValue("@name", name);

            var reader = cmd.ExecuteReader();

            Mood mood = null;

            while (await reader.ReadAsync())
            {
                mood = new Mood()
                {
                    MoodID = Convert.ToInt32(reader["moodID"]),
                    Name = reader["name"].ToString(),
                };
            }

            reader.Close();

            await connection.CloseAsync();

            return mood;
        }

        public async Task<int> SaveMood(Mood mood)
        {
            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand myCommand = new MySqlCommand("INSERT INTO Mood (name) VALUES (@name)", connection);
            myCommand.Parameters.AddWithValue("@name", mood.Name);
            
            var result = await myCommand.ExecuteScalarAsync();

            int moodID = -1;

            if (result == null)
            {
                moodID = Convert.ToInt32(myCommand.LastInsertedId);
            }
            else
            {
                moodID = Convert.ToInt32(result);
            }

            await connection.CloseAsync();

            return moodID;
        }

        public async Task<int> UpdateMood(int moodID, Mood mood)
        {
            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand myCommand = new MySqlCommand("UPDATE Mood SET name = @name WHERE moodID = @moodID", connection);
            myCommand.Parameters.AddWithValue("@name", mood.Name);
            myCommand.Parameters.AddWithValue("@moodID", moodID);

            await myCommand.ExecuteScalarAsync();

            await connection.CloseAsync();

            return moodID;
        }

        public async void DeleteMood(int moodID)
        {
            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand myCommand = new MySqlCommand("DELETE FROM Mood WHERE moodID = @moodID", connection);
            myCommand.Parameters.AddWithValue("@moodID", moodID);

            await myCommand.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }
    }
}
