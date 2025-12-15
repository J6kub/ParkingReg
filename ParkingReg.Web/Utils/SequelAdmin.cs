using MySql.Data;
using MySql.Data.MySqlClient;
namespace ParkingReg.Utils
{

    // Klasse for migrering av database
    public class SequelAdmin : SequelBase
    {

        public SequelAdmin(string dbIP, string dbname) : base(dbIP, dbname) { }
        public SequelAdmin() : base() { }

        public void AddWhiteListedMail(string mail)
        {
            Open();
            string query = "INSERT INTO WhitelistMails (Email) VALUES (@mail);";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@mail", mail);
                cmd.ExecuteNonQuery();
            }
        }

    }
}
