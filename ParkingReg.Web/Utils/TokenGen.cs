using System;
using System.Security.Cryptography;

namespace ParkingReg.Web.Utils
{
    
    public static class TokenGen
    {
        public static string Generate(int length = 24)
        {
            // base64 expands 3 bytes → 4 chars
            var byteCount = (int)Math.Ceiling(length * 3 / 4.0);
            var bytes = RandomNumberGenerator.GetBytes(byteCount);

            return Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=')
                .Substring(0, length);
        }
    }
}
