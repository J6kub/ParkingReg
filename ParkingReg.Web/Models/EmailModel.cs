using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace ParkingReg.Web.Models
{
    public class EmailModel
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailModel(MySqlDataReader reader)
        {
            Email = reader.GetString("Email");
            Subject = reader.GetString("Subject");
            Body = reader.GetString("Body");
        }
    }
}
