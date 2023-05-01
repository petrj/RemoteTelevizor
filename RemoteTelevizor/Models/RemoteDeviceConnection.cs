using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RemoteTelevizor.Models
{
    public class RemoteDeviceConnection
    {
        public string Name { get; set; }

        public string IP { get; set; }
        public int Port { get; set; }
        public string SecurityKey { get; set; }
    }
}
