using Microsoft.Data.SqlClient;
using WhistTournaments.DL.Entities;
using WhistTournaments.DL.Enums;

namespace WhistTournaments.DAL.Repositories
{
    public  class TournamentRepository
    {
        //private string _connectionstring = "server=10.2.28.135;database=WhistDB;uid=sa;pwd=test1234=;trustServerCertificate=true";
        private string _connectionstring = "Server=(localdb)\\MSSQLLocalDB;Database=WhistDB;Trusted_Connection=True;";


        public bool Add(Tournament tournament)
        {
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO TOURNAMENT 
                                        (NAME, TYPE, REG_ENDDATE, STARTDATE, NB_PLAYERS ,NB_GAMES, ONGOING)
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
                command.CommandText = @"SELECT t.*,
                                       (
                                            SELECT COUNT(*) AS cnt
                                            FROM subscription tu
                                            WHERE tu.tournament_id = t.id
                                        ) registeredCount
                                        FROM TOURNAMENT t WHERE t.id = @id";

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
                command.CommandText = @"SELECT t.*, 
                                        (
                                            SELECT COUNT(*) AS cnt
                                            FROM subscription tu
                                            WHERE tu.tournament_id = t.id
                                        ) registeredCount
                                        FROM tournament t";

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

                command.CommandText = @"INSERT INTO SUBSCRIPTION (TOURNAMENT_ID, USER_ID)
                                        VALUES (@tournamentid, @userid);";
                                        

                command.Parameters.AddWithValue("tournamentid", tournament_id);
                command.Parameters.AddWithValue("userid", user_id);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();

            }
        }

        public void UnsubscribeToTournament(int tournament_id, int user_id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {

                command.CommandText = @"DELETE FROM SUBSCRIPTION
                                        WHERE TOURNAMENT_ID = @tournamentid AND USER_ID = @userid;";

                command.Parameters.AddWithValue("tournamentid", tournament_id);
                command.Parameters.AddWithValue("userid", user_id);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public bool ExistByUserIdinTournament(int tournamentid, int? userid)
        {

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                                    SELECT CAST(
                                    CASE WHEN EXISTS (SELECT 1 FROM SUBSCRIPTION WHERE TOURNAMENT_ID = @tournamentid AND
                                    USER_ID = @userid) THEN 1 ELSE 0 END
                                    AS BIT) AS IsExisting;";

                command.Parameters.AddWithValue("tournamentid", tournamentid);
                command.Parameters.AddWithValue("userid", userid);

                connection.Open();

                return (bool)command.ExecuteScalar();
            }
        }

        public int GetCountSubscribedPlayersByTournamentId(int tournamentid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT COUNT(*) FROM SUBSCRIPTION tu WHERE TOURNAMENT_ID = @id;";

                command.Parameters.AddWithValue("tournamentid", tournamentid);

                connection.Open();

                return (int)command.ExecuteScalar();
            }
        }

        public List<int> GetAllSubscribedPlayersByTournamentId(int tournamentid)
        {
            List<int> players = new List<int>();

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT TOP 16 USER_ID FROM SUBSCRIPTION
                                        WHERE TOURNAMENT_ID = @id;";

                command.Parameters.AddWithValue("id", tournamentid);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        players.Add((int)reader["USER_ID"]);

                    }

                    return players;
                }
            }
        }

        public void InitiateGame(int tournamentid, int step)
        {
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO GAME (
                                       TOURNAMENT_ID, STEP, PLAYER_1, PLAYER_2, PLAYER_3, PLAYER_4, 
                                       SCORE_1, SCORE_2, SCORE_3, SCORE_4,
                                       RANKING_1, RANKING_2, RANKING_3, RANKING_4)
                                       VALUES(@tournamentid, @step, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);";

                command.Parameters.AddWithValue("tournamentid", tournamentid);
                command.Parameters.AddWithValue("step", step);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();

            }
        }

        public void InitiateGame(int tournamentid, int step, int player1, int player2, int player3, int player4)
        {
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO GAME (
                                       TOURNAMENT_ID, STEP, PLAYER_1, PLAYER_2, PLAYER_3, PLAYER_4, 
                                       SCORE_1, SCORE_2, SCORE_3, SCORE_4,
                                       RANKING_1, RANKING_2, RANKING_3, RANKING_4)
                                       VALUES(@tournamentid, @step, @player1, @player2, @player3, @player4, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);";

                command.Parameters.AddWithValue("tournamentid", tournamentid);
                command.Parameters.AddWithValue("step", step);
                command.Parameters.AddWithValue("player1", player1);
                command.Parameters.AddWithValue("player2", player2);
                command.Parameters.AddWithValue("player3", player3);
                command.Parameters.AddWithValue("player4", player4);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();

            }
        }

        public void SetOnGoingTournament(int tournamentid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"UPDATE TOURNAMENT SET ONGOING = 1
                                        WHERE ID = @tournamentid;";

                command.Parameters.AddWithValue("tournamentid", tournamentid);

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
                RegisteredPlayers = (int)reader["registeredCount"],
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
                command.Parameters.AddWithValue("@ongoing",tournament.OnGoing);
                command.Parameters.AddWithValue("@id",id);
                connection.Open();

                return (command.ExecuteNonQuery()==1 ? true : false);
            }
        }
    }
}
