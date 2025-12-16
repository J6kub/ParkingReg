using MySql.Data.MySqlClient;

namespace ParkingReg.Web.Models
{
    public class WhiteListEmailModel
    {
        public string Email { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? Id { get; set; }

        public WhiteListEmailModel() { }
        public WhiteListEmailModel(MySqlDataReader reader) {
            Email = reader["Email"] as string;
            Address = reader["Address"] as string;
            Name = reader["Name"] as string;
            Id = reader["Id"] as int?;
            Active = reader["Active"] as bool?;
        }
    }
}
