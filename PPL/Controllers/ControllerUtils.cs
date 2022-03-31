using Npgsql;
using System.Data;
using PPL.Models;

namespace PPL.Controllers
{
    public class ControllerUtils
    {
        public static Int64 authenticateUser(string auth_token, string db_conn_string)
        {
            string query = @"
                SELECT * FROM auth_tokens where token=@token
            ";

            DataTable table = new DataTable();
            string sqlDataSource = db_conn_string;
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@token", auth_token);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            if (table.Rows.Count == 0)
                return -1;

            return table.Rows[0].Field<Int64>("id_user");
        }

        public static User getUser(Int64 id_user, string db_conn_string)
        {
            string query = @"
                SELECT * FROM users WHERE id_user=@id_user
            ";

            DataTable table = new DataTable();
            string sqlDataSource = db_conn_string;
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id_user", id_user);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }


            return new User
            {
                Id = id_user,
                Email = table.Rows[0].Field<string>("email"),
                Username = table.Rows[0].Field<string>("username"),
                Role = table.Rows[0].Field<string>("role")
            };
        }
    }
}
