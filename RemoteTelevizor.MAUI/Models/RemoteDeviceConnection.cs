using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace RemoteTelevizor.Models
{
    public class RemoteDeviceConnection : BaseNotifableObject
    {
        public string Name { get; set; }

        public string IP { get; set; }
        public int Port { get; set; }
        public string SecurityKey { get; set; }

        public override string ToString()
        {
            return $"{Name} ({IP}:{Port})";
        }

        public string IpAndPort
        {
            get
            {
                return $"{IP}:{Port}";
            }
        }

        public static RemoteDeviceConnection CloneFrom(RemoteDeviceConnection remoteDeviceConnection)
        {
            var newRemoteDeviceConnection = new RemoteDeviceConnection();
            newRemoteDeviceConnection.Name = remoteDeviceConnection.Name;
            newRemoteDeviceConnection.IP = remoteDeviceConnection.IP;
            newRemoteDeviceConnection.Port = remoteDeviceConnection.Port;
            newRemoteDeviceConnection.SecurityKey = remoteDeviceConnection.SecurityKey;

            return newRemoteDeviceConnection;
        }

        public void UpdateFrom(RemoteDeviceConnection remoteDeviceConnection)
        {
            Name = remoteDeviceConnection.Name;
            IP = remoteDeviceConnection.IP;
            Port = remoteDeviceConnection.Port;
            SecurityKey = remoteDeviceConnection.SecurityKey;
        }

        public void NotifyPropertyChange()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(IP));
            OnPropertyChanged(nameof(Port));
            OnPropertyChanged(nameof(SecurityKey));
        }
    }
}
