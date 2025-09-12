using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RemoteTelevizor.Models
{
    public interface IAppData
    {
        ObservableCollection<RemoteDeviceConnection> Connections { get; set; }
        void SaveConnections();
        string LastConnectionIpAndPort { get; set; }
        RemoteDeviceConnection GetConnectionByIPAndPort(string ip, int port);
        RemoteDeviceConnection GetConnectionByIPAndPort(string ipAndPort);
    }
}
