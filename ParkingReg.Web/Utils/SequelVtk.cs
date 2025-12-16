using MySql.Data;
using MySql.Data.MySqlClient;
using ParkingReg.Web.Models;
using ParkingReg.Web.Utils;
namespace ParkingReg.Utils
{

    // Klasse for migrering av database
    public class SequelVtk : SequelBase
    {

        public SequelVtk(string dbIP, string dbname) : base(dbIP, dbname) { }
        public SequelVtk() : base() { }

        public void GenerateVtk(int emailid, string BaseUrl)
        {
            Open();
            bool SuccessfulInsert = false;
            string tkn = null;
            InvalidateVtks(emailid);
            while (!SuccessfulInsert)
            {
                tkn = TokenGen.Generate(30);

                string query = "INSERT INTO Vtk (EmailId, Token) VALUES (@mail, @tkn);";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@mail", emailid);
                    cmd.Parameters.AddWithValue("@tkn", tkn);
                    cmd.ExecuteNonQuery();
                }
                SuccessfulInsert = true;
            }
            SequelWhitelistEmail seqwle = new SequelWhitelistEmail();
            string email = seqwle.GetWhitelistEmail(emailid);
            seqwle.Dispose();
            EmailHandler.SendEmail(email, "Parkering thing", $"please klikk link {BaseUrl}/Parking?t={tkn}");

        }
        public void InvalidateVtks(int emailid)
        {
            string query = "UPDATE Vtk SET Valid = False WHERE Emailid = @mail;";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@mail", emailid);
                cmd.ExecuteNonQuery();
            }
        }
        public Vtk GetVtkByToken(string token)
        {
            Open();
            string query = "SELECT Id, EmailId, Token, Valid FROM Vtk WHERE Token = @tkn AND Valid = True;";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@tkn", token);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Vtk
                        {
                            Id = reader.GetInt32("Id"),
                            EmailId = reader.GetInt32("EmailId"),
                            Token = reader.GetString("Token"),
                            Valid = reader.GetBoolean("Valid")
                        };
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
