using System.Text.Json;

namespace ParkingReg.Web.Utils
{
    public class CarInfo
    {
        public string Make { get; set; }
        public string Color { get; set; }
        public string Year { get; set; }

        public override string ToString() => $"{Color} {Make} {Year}";
    }

    public class FetchCarInfo
    {
        private readonly HttpClient _client;

        public FetchCarInfo(HttpClient client = null)
        {
            _client = client ?? new HttpClient();
        }

        public async Task<string> GetCarInfoAsync(string regnr)
        {
            if (string.IsNullOrWhiteSpace(regnr))
                throw new ArgumentException("Registration number cannot be empty.", nameof(regnr));

            string url = $"https://kjoretoyoppslag.atlas.vegvesen.no/ws/no/vegvesen/kjoretoy/kjoretoyoppslag/v2/oppslag/raw/{regnr}";

            HttpResponseMessage response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var carInfo = new CarInfo
            {
                Make = root
                    .GetProperty("kjoretoy")
                    .GetProperty("godkjenning")
                    .GetProperty("tekniskGodkjenning")
                    .GetProperty("tekniskeData")
                    .GetProperty("generelt")
                    .GetProperty("handelsbetegnelse")[0]
                    .GetString(),

                Color = root
                    .GetProperty("kjoretoy")
                    .GetProperty("godkjenning")
                    .GetProperty("tekniskGodkjenning")
                    .GetProperty("tekniskeData")
                    .GetProperty("karosseriOgLasteplan")
                    .GetProperty("rFarge")[0]
                    .GetProperty("kodeNavn")
                    .GetString(),

                Year = root
                    .GetProperty("kjoretoy")
                    .GetProperty("godkjenning")
                    .GetProperty("tekniskGodkjenning")
                    .GetProperty("gyldigFraDato")
                    .GetString()
                    .Split('-')[0]
            };

            return carInfo.ToString();
        }
    }
}
