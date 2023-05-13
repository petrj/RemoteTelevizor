using Android.Content.Res;
using Android.InputMethodServices;
using LoggerService;
using RemoteAccess;
using RemoteTelevizor.Models;
using RemoteTelevizor.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RemoteTelevizor.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private ILoggingService _loggingService;
        private RemoteDeviceConnection _currentConnection;
        private RemoteAccessService _remoteAccessService;

        public MainPageViewModel(ILoggingService loggingService)
        {
            _loggingService = loggingService;
            _remoteAccessService = new RemoteAccessService(loggingService);
        }

        public void SetConnection(RemoteDeviceConnection connection)
        {
            _currentConnection = connection;
            _remoteAccessService.SetConnection(connection.IP, connection.Port, connection.SecurityKey);

            OnPropertyChanged(nameof(DeviceName));
            OnPropertyChanged(nameof(IPPort));
        }

        public string DeviceName
        {
            get
            {
                if (_currentConnection == null)
                {
                    return "No device selected";
                }

                return _currentConnection.Name;
            }
        }

        public string IPPort
        {
            get
            {
                if (_currentConnection == null)
                {
                    return "";
                }

                return _currentConnection.IP + ":" + _currentConnection.Port.ToString();
            }
        }

        public async Task SendKey(string keyCode)
        {
            var msg = new RemoteAccessMessage()
            {
                command = "keyDown",
                commandArg1 = keyCode
            };

            await Task.Run(() => { _remoteAccessService.SendMessage(msg); });
        }
    }
}
