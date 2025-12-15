namespace ParkingReg.Web.Models
{
    public class ParkReq
    {
        public string Regnr { get; set; }
        public string Token { get; set; }
        public string Duration { get; set; }
        public int EmailId { get; set; } 

        }
}
