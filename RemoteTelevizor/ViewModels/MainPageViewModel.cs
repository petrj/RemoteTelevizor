using Android.Content.Res;
using Android.InputMethodServices;
using LoggerService;
using RemoteTelevizor.Models;
using RemoteTelevizor.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RemoteTelevizor.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private ILoggingService _loggingService;
        private SocketService _socketService;
        private RemoteDeviceConnection _currentConnection;

        public MainPageViewModel(ILoggingService loggingService)
        {
            _loggingService = loggingService;
            _socketService = new SocketService(_loggingService);
        }

        public void SetConnection(RemoteDeviceConnection connection)
        {
            _currentConnection = connection;
            _socketService.SetConnection(connection);
        }

        public void SendKey(string keyCode)
        {
            var msg = new RemoteAccessMessage()
            {
                securityKey = _currentConnection.SecurityKey,
                command = "keyDown",
                commandArg1 = keyCode
            };


            var encryptedMessage = CryptographyService.EncryptString(_currentConnection.SecurityKey, msg.ToString());

            _socketService.SendMessage(encryptedMessage);
        }


    }
}
