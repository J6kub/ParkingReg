using MySql.Data.MySqlClient;
using ParkingReg.Utils;
using ParkingReg.Web.Models;

namespace ParkingReg.Web.Utils
{
    public static class EmailHandler
    {
        public static void SendEmail(string to, string subject, string body)
        {
            // Placeholder for email sending logic
            // This could be implemented using SMTP, SendGrid, or any other email service
            Console.WriteLine("-------------------------------");
            Console.WriteLine($"Sending email to: {to}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Body: {body}");
            Console.WriteLine("-------------------------------");

            SequelBase seq = new SequelBase();
            MySqlConnection conn = seq.GetConnection();
            conn.Open();
            string sql = "INSERT INTO Emails (Email, Subject, Body) VALUES (@Email, @Subject, @Body)";
            using (var cmd = new MySqlCommand(sql,conn))
            {
                cmd.Parameters.AddWithValue("@Email", to);
                cmd.Parameters.AddWithValue("@Subject", subject);
                cmd.Parameters.AddWithValue("@Body", body);
                cmd.ExecuteNonQuery();

            }
            seq.Dispose();
        }
        public static List<EmailModel> GetEmails()
        {
            List<EmailModel> emails = new List<EmailModel>();

            SequelBase seq = new SequelBase();
            MySqlConnection conn = seq.GetConnection();
            conn.Open();
            string sql = @"
                SELECT Email, Subject, Body
                FROM Emails
                ORDER BY Id DESC
                LIMIT 10;
            "; 

            using (var cmd = new MySqlCommand(sql, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    emails.Add(new EmailModel(reader));
                }
            }

            conn.Close();
            return emails;
        }
    }
}
