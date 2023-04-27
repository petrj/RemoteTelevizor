using LoggerService;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteTelevizor
{
    public class SocketService
    {
        private ILoggingService _loggingService;
        private SocketConnection _connection;

        public SocketService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void SetConnection(SocketConnection connection)
        {
            _connection = connection;
        }
    }
}
