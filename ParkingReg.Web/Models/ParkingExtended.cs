using MySql.Data.MySqlClient;

namespace ParkingReg.Web.Models
{
    public class ParkingExtended : Parking
    {
        public string Email { get; set; }
        public string Name { get; set; }

        public ParkingExtended(MySqlDataReader reader)
        {
            Id = reader.GetInt32("Id");
            Regnr = reader.GetString("Regnr");
            EmailId = reader.GetInt32("EmailId");
            FromDate = reader.GetDateTime("FromDate");
            ToDate = reader.GetDateTime("ToDate");
            Active = reader.GetBoolean("Active");

            Email = reader.GetString("Email");
            Name = reader["Name"] as string;
        }
    }
}
