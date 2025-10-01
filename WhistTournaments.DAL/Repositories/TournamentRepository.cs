using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
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
                                        (NAME, TYPE, REG_ENDDATE, STARTDATE, NB_PLAYERS, NB_SUBSCRIBED_PLAYERS,NB_GAMES, ONGOING)
                                        VALUES
                                        (@name, @type, @reg_enddate, @startdate, @nbplayers, 0, (@nbplayers/2)-1, @ongoing);";

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

        public Tournament MapTournament(SqlDataReader reader)
        {
            return new Tournament()
            {
                Id = (int)reader["ID"],
                Name = (string)reader["NAME"],
                Type = (Type_Tournament)reader["TYPE"],
                RegistrationEndDate = (DateTime)reader["REG_ENDDATE"],
                StartDate = (DateTime)reader["STARTDATE"],
                NbSubscribedPlayers = (int)reader["NB_SUBSCRIBED_PLAYERS"],
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
                                        STARTDATE=startdate, NB_PLAYERS=nbplayers, NB_SUBSCRIBED_PLAYERS=nbgames,
                                        NB_GAMES=(nbplayers/2)-1, ONGOING=@ongoing 
                                      WHERE [ID]=22";

                command.Parameters.AddWithValue("@name",tournament.Name);
                command.Parameters.AddWithValue("@type",tournament.Type);
                command.Parameters.AddWithValue("@reg_enddate",tournament.RegistrationEndDate);
                command.Parameters.AddWithValue("@startdate",tournament.StartDate);
                command.Parameters.AddWithValue("@nbplayers",tournament.NbPlayers);
                command.Parameters.AddWithValue("@nbgames",tournament.NbGames);
                command.Parameters.AddWithValue("@ongoing",tournament.OnGoing);

                connection.Open();

                return (command.ExecuteNonQuery()==1 ? true : false);
            }
        }
    }
}
