using Microsoft.Phone.Net.NetworkInformation;

namespace SismoNica_WP8._0.Common
{
    public class NetworkStatusHelper
    {
        public bool ConnectionStatus { get; private set; }

        public NetworkStatusHelper()
        {
            ConnectionStatus = DeviceNetworkInformation.IsNetworkAvailable;
            WatchNetwork();
        }

        public void WatchNetwork()
        {
            DeviceNetworkInformation.NetworkAvailabilityChanged += ChangeDetected;
        }

        public void UnWatchNetwork()
        {
            DeviceNetworkInformation.NetworkAvailabilityChanged -= ChangeDetected;
        }

        private void ChangeDetected(object sender, NetworkNotificationEventArgs e)
        {
            ConnectionStatus = (e.NotificationType == NetworkNotificationType.InterfaceConnected);
        }
    }
}