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
        }
    }
}
