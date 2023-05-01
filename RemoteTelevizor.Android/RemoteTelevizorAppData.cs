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
        public void SaveConnections(ObservableCollection<RemoteDeviceConnection> connections)
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

        public ObservableCollection<RemoteDeviceConnection> LoadConnections()
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