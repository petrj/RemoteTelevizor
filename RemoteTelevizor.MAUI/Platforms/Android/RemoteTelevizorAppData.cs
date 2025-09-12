using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Org.Json;
using RemoteTelevizor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace RemoteTelevizor
{
    public class RemoteTelevizorAppData : CustomSharedPreferencesObject, IAppData
    {
        private ObservableCollection<RemoteDeviceConnection> _connections = null;

        public string LastConnectionIpAndPort
        {
            get
            {
                try
                {
                    return GetPersistingSettingValue<string>("LastConnectionIpAndPort");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
            set
            {
                try
                {
                    SavePersistingSettingValue<string>("LastConnectionIpAndPort", value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
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

                return _connections;
            }
            set
            {
                _connections = value;
                SaveConnections();
            }
        }

        public void SaveConnections()
        {
            try
            {
                var devicesString = JsonConvert.SerializeObject(_connections);

                SavePersistingSettingValue<string>("RemoteDeviceConnections", devicesString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public RemoteDeviceConnection GetConnectionByIPAndPort(string ipAndPort)
        {
            foreach (var connection in Connections)
            {
                if (connection.IpAndPort == ipAndPort)
                {
                    return connection;
                }
            }

            return null;
        }

        public RemoteDeviceConnection GetConnectionByIPAndPort(string ip, int port)
        {
            return GetConnectionByIPAndPort($"{ip}:{port}");
        }

        private ObservableCollection<RemoteDeviceConnection> LoadConnections()
        {
            try
            {
                var devicesString = GetPersistingSettingValue<string>("RemoteDeviceConnections");
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
    }
}