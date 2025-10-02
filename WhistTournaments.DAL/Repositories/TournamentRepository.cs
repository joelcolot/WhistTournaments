using Microsoft.Data.SqlClient;
using WhistTournaments.DL.Entities;
using WhistTournaments.DL.Enums;

namespace WhistTournaments.DAL.Repositories
{
    public  class TournamentRepository
    {
        private string _connectionstring = "server=10.2.28.135;database=WhistDB;uid=sa;pwd=test1234=;trustServerCertificate=true";

        public bool Add(Tournament tournament)
        {
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO TOURNAMENT 
                                        (NAME, TYPE, REG_ENDDATE, STARTDATE, NB_PLAYERS, NB_GAMES, ONGOING)
                                        VALUES
                                        (@name, @type, @reg_enddate, @startdate, @nbplayers, (@nbplayers/2)-1, @ongoing);";

                command.Parameters.AddWithValue("@name", tournament.Name);
                command.Parameters.AddWithValue("@type", tournament.Type);
                command.Parameters.AddWithValue("@reg_enddate", tournament.RegistrationEndDate);
                command.Parameters.AddWithValue("@startdate", tournament.StartDate);
                command.Parameters.AddWithValue("@nbplayers", tournament.NbPlayers);
                command.Parameters.AddWithValue("@nbgames", tournament.NbGames);
                command.Parameters.AddWithValue("@ongoing", tournament.OnGoing);

                connection.Open();

                return (command.ExecuteNonQuery()==1 ? true : false);
            }
        }

        public Tournament? GetById(int id)
        {
            Tournament tournament = new();

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM TOURNAMENT
                                        WHERE ID = @id;";

                command.Parameters.AddWithValue("id", id);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapTournament(reader);
                    }
                }

                return null;
            }
        }

        public List<Tournament> GetAll()
        {
            List<Tournament> tournaments = [];

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM TOURNAMENT";

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tournaments.Add(MapTournament(reader));
                }

                connection.Close();
            }

            return tournaments;
        }

        public void SubscribeToTournament(int tournament_id, int user_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"UPDATE TOP (1) GAME
                                        SET PLAYER_1 = CASE WHEN PLAYER_1 IS NULL THEN @userid ELSE PLAYER_1 END,
                                            PLAYER_2 = CASE WHEN PLAYER_1 IS NOT NULL AND PLAYER_2 IS NULL THEN @userid ELSE PLAYER_2 END,
                                            PLAYER_3 = CASE WHEN PLAYER_1 IS NOT NULL AND PLAYER_2 IS NOT NULL AND PLAYER_3 IS NULL THEN @userid ELSE PLAYER_3 END,
                                            PLAYER_4 = CASE WHEN PLAYER_1 IS NOT NULL AND PLAYER_2 IS NOT NULL AND PLAYER_3 IS NOT NULL AND PLAYER_4 IS NULL THEN @userid ELSE PLAYER_4 END
                                        WHERE TOURNAMENT_ID = @tournamentid
                                        AND (PLAYER_1 IS NULL OR PLAYER_2 IS NULL OR PLAYER_3 IS NULL OR PLAYER_4 IS NULL)
                                        AND STEP > 3;";

                command.Parameters.AddWithValue("tournamentid", tournament_id);
                command.Parameters.AddWithValue("userid", user_id);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();

            }
        }

        public Tournament MapTournament(SqlDataReader reader)
        {
            return new Tournament()
            {
                Id = (int)reader["ID"],
                Name = (string)reader["NAME"],
                Type = (Type_Tournament)reader["TYPE"],
                RegistrationEndDate = (DateTime)reader["REG_ENDDATE"],
                StartDate = (DateTime)reader["STARTDATE"],
                NbPlayers = (int)reader["NB_PLAYERS"],
                NbGames = (int)reader["NB_GAMES"],
                OnGoing = (bool)reader["ONGOING"],
            };
        }

        public bool UpdateTournament(Tournament tournament, int id) 
        {
            using(SqlConnection connection = new SqlConnection(_connectionstring))
            using(SqlCommand command = connection.CreateCommand())
            {
                command.CommandText=@"UPDATE TOURNAMENT 
                                      SET [NAME]=@name, [TYPE]=@type, REG_ENDDATE=@reg_enddate, 
                                        STARTDATE=@startdate, NB_PLAYERS=@nbplayers, NB_SUBSCRIBED_PLAYERS=@nbsubscribed,
                                        NB_GAMES=(@nbplayers/2)-1, ONGOING=@ongoing 
                                      WHERE [ID]=@id";

                command.Parameters.AddWithValue("@name",tournament.Name);
                command.Parameters.AddWithValue("@type",tournament.Type);
                command.Parameters.AddWithValue("@reg_enddate",tournament.RegistrationEndDate);
                command.Parameters.AddWithValue("@startdate",tournament.StartDate);
                command.Parameters.AddWithValue("@nbplayers",tournament.NbPlayers);
                command.Parameters.AddWithValue("@nbsubscribed",tournament.NbSubscribedPlayers);
                command.Parameters.AddWithValue("@ongoing",tournament.OnGoing);
                command.Parameters.AddWithValue("@id",id);
                connection.Open();

                return (command.ExecuteNonQuery()==1 ? true : false);
            }
        }
    }
}
