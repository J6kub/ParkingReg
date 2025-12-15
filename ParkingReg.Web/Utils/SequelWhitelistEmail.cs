using MySql.Data;
using MySql.Data.MySqlClient;
namespace ParkingReg.Utils
{

    // Klasse for migrering av database
    public class SequelWhitelistEmail : SequelBase
    {
        
        public SequelWhitelistEmail(string dbIP, string dbname) : base(dbIP, dbname) { }
        public SequelWhitelistEmail() : base() { }

        public bool CheckWhitelistEmail(string email)
        {
            Open();
            string query = "SELECT COUNT(*) FROM WhitelistMails WHERE Email = @mail;";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@mail", email);
                var count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
        public int GetWhitelistEmailId(string email)
        {
            Open();
            string query = "SELECT Id FROM WhitelistMails WHERE Email = @mail;";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@mail", email);
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    return -1;
                }
            }
        }
        public string GetWhitelistEmail(int id)
        {
            Open();
            string query = "SELECT Email FROM WhitelistMails WHERE id = @mail;";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@mail", id);
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return result.ToString();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
