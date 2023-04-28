﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RemoteTelevizor.Models
{
    public class SocketConnection
    {
        public string Name { get; set; }

        public string IP { get; set; }
        public int Port { get; set; }
        public string SecurityKey { get; set; }

        public IPAddress IPAddress
        {
            get
            {
                return IPAddress.Parse(IP);
            }
        }
    }
}