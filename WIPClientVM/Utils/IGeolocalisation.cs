using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIPClient.Utils
{
    public interface IGeolocalisation
    {
        public static Location GetPerestroika()
            => new Location(48.58432, 7.73750);
        public static Location GetNowhere()
            => new Location(48.58432, 7.74000);
        public Task<Geolocalisation> VerifyPermission(bool? forcedPermissionAccepted = null);
        public void StartService();
        public Task<Location> GetLocationAsync(
            Location? forcedLocation = null,
            bool? forcedLocationPermission = null);

        public void StopService();
    }
}
