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
    public class GameRepository
    {
        private string _connectionstring = "server=10.2.28.135;database=WhistDB;uid=sa;pwd=test1234=;trustServerCertificate=true";

        public List<Game> GetAllGamesByTournamentId(int id)
        {

            List<Game> games = [];

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM GAME
                                        WHERE TOURNAMENT_ID = @id;";

                command.Parameters.AddWithValue("id", id);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    games.Add(MapGame(reader));
                }

                connection.Close();
            }

            return games;

        }

        public Game MapGame(SqlDataReader reader)
        {
            return new Game()
            {
                Id = (int)reader["ID"],
                TournamentId = (int)reader["TOURNAMENT_ID"],
                Step = (int)reader["STEP"],
                Player1 = (int)reader["PLAYER_1"],
                Player2 = (int)reader["PLAYER_2"],
                Player3 = (int)reader["PLAYER_3"],
                Player4 = (int)reader["PLAYER_4"],
                Score1 = (int)reader["SCORE_1"],
                Score2 = (int)reader["SCORE_2"],
                Score3 = (int)reader["SCORE_3"],
                Score4 = (int)reader["SCORE_4"],
            };
        }
    }
}
