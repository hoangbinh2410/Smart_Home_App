using BA_MobileGPS.Utilities.Constant;

namespace BA_MobileGPS.Service
{
    /// <summary>
    /// The auth config.
    /// </summary>
    public class PlacesConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlacesConfig"/> class.
        /// </summary>
        /// <param name="apiKey"> The google web-service api key. </param>
        public PlacesConfig()
        {
            this.ApiKey = Config.GoogleMapKeyWeb;
        }

        /// <summary>
        /// Gets or sets the api key of your Firebase app.
        /// </summary>
        public string ApiKey
        {
            get;
            set;
        }
    }
}