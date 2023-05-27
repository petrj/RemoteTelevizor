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
        string LastConnectionIP { get; set; }
        RemoteDeviceConnection GetConnectionByIP(string ip);
    }
}
