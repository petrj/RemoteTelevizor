using Newtonsoft.Json;
using RemoteTelevizor.Models;
using System.Collections.ObjectModel;

namespace RemoteTelevizor
{
    public class RemoteTelevizorAppData : IAppData
    {
        private ObservableCollection<RemoteDeviceConnection> _connections = null;

        public string LastConnectionIpAndPort
        {
            get
            {
                return null;
            }
            set
            {

            }
        }

        public ObservableCollection<RemoteDeviceConnection> Connections
        {
            get
            {
                return new ObservableCollection<RemoteDeviceConnection>();
            }
            set
            {

            }
        }

        public void SaveConnections()
        {

        }

        public RemoteDeviceConnection GetConnectionByIPAndPort(string ipAndPort)
        {
            return null;
        }

        public RemoteDeviceConnection GetConnectionByIPAndPort(string ip, int port)
        {
            return null;
        }
    }
}