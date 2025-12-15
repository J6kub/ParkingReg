using System.Text.RegularExpressions;

namespace ParkingReg.Web.Utils
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Parses a string like "2D_4HR" and returns DateTime offset from now.
        /// </summary>
        public static DateTime AddFromString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input cannot be empty");

            // Match pattern: optional days + optional hours
            var pattern = @"(?:(\d+)D)?_?(?:(\d+)HR)?";
            var match = Regex.Match(input.ToUpper(), pattern);

            if (!match.Success)
                throw new FormatException("Invalid format. Expected e.g. '2D_4HR'");

            int days = 0;
            int hours = 0;

            if (!string.IsNullOrEmpty(match.Groups[1].Value))
                days = int.Parse(match.Groups[1].Value);

            if (!string.IsNullOrEmpty(match.Groups[2].Value))
                hours = int.Parse(match.Groups[2].Value);

            return DateTime.Now.AddDays(days).AddHours(hours);
        }
    }
}
