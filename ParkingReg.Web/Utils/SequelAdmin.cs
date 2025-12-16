using MySql.Data;
using MySql.Data.MySqlClient;
using ParkingReg.Web.Models;
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
        public void AddWhiteListedMail(WhiteListEmailModel WLEM)
        {
            Open();
            string query = "INSERT INTO WhitelistMails (Email, Address, Name) VALUES (@mail, @adress, @name);";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@mail", WLEM.Email);
                cmd.Parameters.AddWithValue("@adress", WLEM.Address);
                cmd.Parameters.AddWithValue("@name", WLEM.Name);
                cmd.ExecuteNonQuery();
            }
            Dispose();
        }

    }
}
