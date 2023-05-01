using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RemoteTelevizor.Models
{
    public interface IAppData
    {
        void SaveConnections(ObservableCollection<RemoteDeviceConnection> connections);
        ObservableCollection<RemoteDeviceConnection> LoadConnections();
    }
}
