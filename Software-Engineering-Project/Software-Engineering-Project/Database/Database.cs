using Npgsql;

namespace Software_Engineering_Project.Database
{
    public class Database
    {
        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection("Server=localhost;Port=5432;" +
                 "Database=software-engineering-database;User Id=postgres;Password=123;");
        }

        public static NpgsqlDataReader ExecuteQuery(string query, NpgsqlConnection con)
        {
            NpgsqlConnection conn = con;
            NpgsqlCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = query;
            NpgsqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        public static int ExecuteUpdate(string query, NpgsqlConnection con)
        {
            NpgsqlConnection conn = con;
            NpgsqlCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = query;
            int result = cmd.ExecuteNonQuery();
            return result;
        }
    }
}
