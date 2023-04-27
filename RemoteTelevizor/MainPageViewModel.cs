using Android.InputMethodServices;
using LoggerService;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteTelevizor
{
    public class MainPageViewModel
    {
        private ILoggingService _loggingService;
        private SocketService _socketService;

        public MainPageViewModel(ILoggingService loggingService)
        {
            _loggingService = loggingService;
            _socketService = new SocketService(_loggingService);
        }

        public void SetConnection(SocketConnection connection)
        {
            _socketService.SetConnection(connection);
        }
    }
}
