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
        //private string _connectionstring = "server=10.2.28.135;database=WhistDB;uid=sa;pwd=test1234=;trustServerCertificate=true";
        private readonly string _connectionstring = "Server=(localdb)\\MSSQLLocalDB;Database=WhistDB;Trusted_Connection=True;";


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

        public void CreateGamesNewTournament(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                                    INSERT INTO GAME 
                                    (TOURNAMENT_ID, STEP, PLAYER_1, PLAYER_2, PLAYER_3, PLAYER_4, SCORE_1,  SCORE_2, SCORE_3, SCORE_4, RANKING_1, RANKING_2, RANKING_3, RANKING_4)
                                    VALUES
                                    (@id, 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
                                    (@id, 6, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
                                    (@id, 5, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
                                    (@id, 4, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
                                    (@id, 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
                                    (@id, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
                                    (@id, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);";

                command.Parameters.AddWithValue("id", id);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();

            }
        }

        public bool ExistsMatchsByTournamentId(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                                    SELECT CAST(
                                    CASE WHEN EXISTS (SELECT 1 FROM GAME WHERE TOURNAMENT_ID = @id) THEN 1 ELSE 0 END
                                    AS BIT) AS IsExisting;";

                connection.Open();

                return (bool)command.ExecuteScalar();

            }
        }

        public int GetStepFromTournamentId(int tournamentid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT
                                            CASE WHEN NOT EXISTS (
                                                    SELECT 1 
                                                    FROM GAME 
                                                    WHERE TOURNAMENT_ID = @tournamentId
                                                ) THEN 0

                                                WHEN EXISTS (
                                                    SELECT 1 
                                                    FROM GAME 
                                                    WHERE TOURNAMENT_ID = @tournamentId 
                                                      AND STEP IN (7, 6, 5, 4)
                                                      AND (
                                                          SCORE_1 IS NULL OR 
                                                          SCORE_2 IS NULL OR 
                                                          SCORE_3 IS NULL OR 
                                                          SCORE_4 IS NULL
                                                      )
                                                ) THEN 1

                                                WHEN EXISTS (
                                                    SELECT 1 
                                                    FROM GAME 
                                                    WHERE TOURNAMENT_ID = @tournamentId 
                                                      AND STEP IN (3, 2)
                                                      AND (
                                                          SCORE_1 IS NULL OR 
                                                          SCORE_2 IS NULL OR 
                                                          SCORE_3 IS NULL OR 
                                                          SCORE_4 IS NULL
                                                      )
                                                ) THEN 2

                                                WHEN EXISTS (
                                                    SELECT 1 
                                                    FROM GAME 
                                                    WHERE TOURNAMENT_ID = @tournamentId 
                                                      AND STEP = 1
                                                      AND (
                                                          SCORE_1 IS NULL OR 
                                                          SCORE_2 IS NULL OR 
                                                          SCORE_3 IS NULL OR 
                                                          SCORE_4 IS NULL
                                                      )
                                                ) THEN 3

                                                ELSE 4
                                            END AS Step;";

                command.Parameters.AddWithValue("tournamentid", tournamentid);

                connection.Open();

                return (int)command.ExecuteScalar();
            }
        }

        public List<Game> GetGamesTour1(int tournamentid)
        {
            List<Game> games = [];

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM GAME
                                        WHERE TOURNAMENT_ID = @tournamentid AND STEP IN (7,6,5,4);";

                command.Parameters.AddWithValue("tournamentid", tournamentid);

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

        public List<Game> GetGamesSemiFinals(int tournamentid)
        {
            List<Game> games = [];

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM GAME
                                        WHERE TOURNAMENT_ID = @tournamentid AND STEP IN (3,2);";

                command.Parameters.AddWithValue("tournamentid", tournamentid);

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

        public Game GetGameFinal(int tournamentid)
        {
            Game game = new();

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM GAME
                                        WHERE TOURNAMENT_ID = @tournamentid AND STEP = 1";

                command.Parameters.AddWithValue("tournamentid", tournamentid);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    game = MapGame(reader);
                }

                connection.Close();
            }

            return game;
        }

        public Game MapGame(SqlDataReader reader)
        {
            return new Game()
            {
                Id = (int)reader["ID"],
                TournamentId = (int)reader["TOURNAMENT_ID"],
                Step = (int)reader["STEP"],
                Player1 = reader["PLAYER_1"] == DBNull.Value ? null : (int)reader["PLAYER_1"],
                Player2 = reader["PLAYER_2"] == DBNull.Value ? null : (int)reader["PLAYER_2"],
                Player3 = reader["PLAYER_3"] == DBNull.Value ? null : (int)reader["PLAYER_3"],
                Player4 = reader["PLAYER_4"] == DBNull.Value ? null : (int)reader["PLAYER_4"],
                Score1 = reader["SCORE_1"] == DBNull.Value ? null : (int)reader["SCORE_1"],
                Score2 = reader["SCORE_2"] == DBNull.Value ? null : (int)reader["SCORE_2"],
                Score3 = reader["SCORE_3"] == DBNull.Value ? null : (int)reader["SCORE_3"],
                Score4 = reader["SCORE_4"] == DBNull.Value ? null : (int)reader["SCORE_4"],
                Ranking1 = reader["RANKING_1"] == DBNull.Value ? null : (int)reader["RANKING_1"],
                Ranking2 = reader["RANKING_2"] == DBNull.Value ? null : (int)reader["RANKING_2"],
                Ranking3 = reader["RANKING_3"] == DBNull.Value ? null : (int)reader["RANKING_3"],
                Ranking4 = reader["RANKING_4"] == DBNull.Value ? null : (int)reader["RANKING_4"],
            };
        }


    }
}
