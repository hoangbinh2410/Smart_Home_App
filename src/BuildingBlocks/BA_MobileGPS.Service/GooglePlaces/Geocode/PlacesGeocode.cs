using Newtonsoft.Json;

using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class PlacesGeocode : IPlacesGeocode
    {
        private readonly PlacesHttpProvider httpProvider;
        private readonly PlacesConfig authConfig;

        private const string GooglePlacesUrl = "https://maps.googleapis.com/maps/api/geocode/json";

        public PlacesGeocode()
        {
            this.authConfig = new PlacesConfig();
            this.httpProvider = new PlacesHttpProvider(authConfig);
        }

        /// <summary>
        /// Creates post body content for an geocode request
        /// </summary>
        /// <param name="input"> The search params. </param>
        public async Task<Geocode> GetGeocode(string input)
        {
            string content = "address=" + input;
            string responseData = await httpProvider.FetchPostContentAsync(GooglePlacesUrl, content).ConfigureAwait(false);

            var geocode = JsonConvert.DeserializeObject<Geocode>(responseData);

            return geocode;
        }

        public async Task<Geocode> GetAddressesForPosition(string lat, string lng)
        {
            string content = string.Format("latlng={0},{1}", lat.Replace(',', '.'), lng.Replace(',', '.'));
            string responseData = await httpProvider.FetchPostContentAsync(GooglePlacesUrl, content).ConfigureAwait(false);

            var geocode = JsonConvert.DeserializeObject<Geocode>(responseData);

            return geocode;
        }
    }
}