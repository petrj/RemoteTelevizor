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

namespace RemoteTelevizor.Droid
{
    public class RemoteTelevizorAppData : CustomSharedPreferencesObject, IAppData
    {
        private ObservableCollection<RemoteDeviceConnection> _connections = null;

        public string LastConnectionIP
        {
            get
            {
                try
                {
                    return GetPersistingSettingValue<string>("LastConnectionIP");
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
                    SavePersistingSettingValue<string>("LastConnectionIP", value);
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
                SaveConnections(value);
                _connections = value;
            }
        }

        private void SaveConnections(ObservableCollection<RemoteDeviceConnection> connections)
        {
            try
            {
                var devicesString = JsonConvert.SerializeObject(connections);

                SavePersistingSettingValue<string>("RemoteDeviceConnections", devicesString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public RemoteDeviceConnection GetConnectionByIP(string ip)
        {
            foreach (var connection in Connections)
            {
                if (connection.IP == ip)
                {
                    return connection;
                }
            }

            return null;
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