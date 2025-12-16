using MySql.Data;
using MySql.Data.MySqlClient;
using ParkingReg.Web.Models;
using System.Collections.Generic;
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
        public List<ParkingExtended> GetActiveParkings()
        {
            conn.Open();
            List<ParkingExtended> Parkings = new List<ParkingExtended>();
            string sql = @"SELECT 
                p.*,
                w.Email,
                w.Name
            FROM 
                Parkings p
            LEFT JOIN 
                WhitelistMails w
            ON 
                p.EmailId = w.Id
            WHERE 
                p.Active = 1;";

            using (var cmd = new MySqlCommand(sql, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Parkings.Add(new ParkingExtended(reader));
                }
            }
            return Parkings;
        }
        public ParkingExtended LookUpParking(string regnr)
        {
            conn.Open();
            string sql = @"SELECT 
                p.*,
                w.Email,
                w.Name
            FROM 
                Parkings p
            LEFT JOIN 
                WhitelistMails w
            ON 
                p.EmailId = w.Id
            WHERE 
                p.Regnr = @Regnr
            ORDER BY 
                p.FromDate DESC
            LIMIT 1;";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Regnr", regnr);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ParkingExtended(reader);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            
        }

    }
}
