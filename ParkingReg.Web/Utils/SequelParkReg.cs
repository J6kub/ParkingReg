using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using ParkingReg.Web.Models;
using ParkingReg.Web.Utils;
namespace ParkingReg.Utils
{

    // Klasse for migrering av database
    public class SequelParkReg : SequelBase
    {
        
        public SequelParkReg(string dbIP, string dbname) : base(dbIP, dbname) { }
        public SequelParkReg() : base() { }

        // Register Parking

        // Extend

        // Get registrerable

        public bool HasValidParking(string regnr)
        {
            Open();
            string sql = "SELECT * FROM Parkings WHERE Active = true AND Regnr = @regnr LIMIT 1";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@regnr", regnr);

                using (var reader = cmd.ExecuteReader())
                {
                    // If a row exists, the parking is valid
                    return reader.Read();
                }
            }
        }
        public void SubmitParking(ParkReq PR)
        {

            string sql = "INSERT INTO Parkings (Regnr, EmailId, FromDate, ToDate, Active) VALUES (@regnr, @emailId, @fromDate, @toDate, true)";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                DateTime fromDate = DateTime.Now;
                DateTime toDate = DateTimeHelper.AddFromString(PR.Duration);
                cmd.Parameters.AddWithValue("@regnr", PR.Regnr);
                cmd.Parameters.AddWithValue("@emailId", PR.EmailId);
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);
                cmd.ExecuteNonQuery();
            }
        }

        public Parking GetParking(string regnr)
        {
            string sql = "SELECT * FROM Parkings WHERE Active = true AND Regnr = @regnr LIMIT 1";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@regnr", regnr);

                using (var reader = cmd.ExecuteReader())
                {
                    // If a row exists, the parking is valid
                    if (reader.Read())
                    {
                        return new Parking(reader);
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
