using Newtonsoft.Json;
using RemoteTelevizor.Models;
using System.Collections.ObjectModel;
using System.Threading.Channels;

namespace RemoteTelevizor
{
    public class RemoteTelevizorAppData : IAppData
    {
        private ObservableCollection<RemoteDeviceConnection> _connections = null;

        private string ConfigPath
        {
            get
            {
                if (!Path.Exists(ConfigDir))
                {
                    Directory.CreateDirectory(ConfigDir);
                }
                return Path.Combine(ConfigDir, "config.json");
            }
        }

        private string ConfigDir
        {
            get
            {
                // ProgramData
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),"RemoteTelevizor");
            }
        }

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
                if (_connections == null)
                {
                    _connections = LoadConnections();
                }
                if (_connections == null)
                {
                    _connections = new ObservableCollection<RemoteDeviceConnection>();
                }

                return _connections;
            }
            set
            {
                _connections = value;
                SaveConnections();
            }
        }

        private ObservableCollection<RemoteDeviceConnection> LoadConnections()
        {
            try
            {
                var devicesString = File.ReadAllText(ConfigPath);
                if (string.IsNullOrEmpty(devicesString))
                {
                    return new ObservableCollection<RemoteDeviceConnection>();
                }

                return JsonConvert.DeserializeObject<ObservableCollection<RemoteDeviceConnection>>(devicesString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ObservableCollection<RemoteDeviceConnection>();
            }
        }

        public void SaveConnections()
        {
            try
            {
                var devicesString = JsonConvert.SerializeObject(_connections);

                File.WriteAllText(ConfigPath, devicesString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

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