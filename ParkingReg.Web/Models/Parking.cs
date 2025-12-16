using MySql.Data.MySqlClient;

namespace ParkingReg.Web.Models
{
    public class Parking
    {
        public int Id { get; set; }
        public string Regnr { get; set; }
        public int EmailId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool Active { get; set; }

        public Parking() { }
        // Constructor that maps a MySqlDataReader to this object
        public Parking(MySqlDataReader reader)
        {
            Id = reader.GetInt32("Id");
            Regnr = reader.GetString("Regnr");
            EmailId = reader.GetInt32("EmailId");
            FromDate = reader.GetDateTime("FromDate");
            ToDate = reader.GetDateTime("ToDate");
            Active = reader.GetBoolean("Active");
        }
    }

}
