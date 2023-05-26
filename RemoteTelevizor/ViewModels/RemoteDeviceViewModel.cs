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
using static Android.Icu.Text.CaseMap;

namespace RemoteTelevizor.ViewModels
{
    public class RemoteDeviceViewModel : BaseViewModel
    {
        private ILoggingService _loggingService;
        private DialogService _dialogService;

        private RemoteDeviceConnection _currentConnection;
        private RemoteAccessService _remoteAccessService;

        public RemoteDeviceViewModel(ILoggingService loggingService, DialogService dialogService)
        {
            _loggingService = loggingService;
            _remoteAccessService = new RemoteAccessService(loggingService);
            _dialogService = dialogService;
        }

        public RemoteDeviceConnection Connection
        {
            get
            {
                return _currentConnection;
            }
        }

        public void SetConnection(RemoteDeviceConnection connection)
        {
            if (connection == null)
            {
                return;
            }

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
            if (_currentConnection == null)
            {
                return;
            }

            var msg = new RemoteAccessMessage()
            {
                command = "keyDown",
                commandArg1 = keyCode
            };

            await Task.Run(() => { _remoteAccessService.SendMessage(msg); });
        }

        public static void SetViewAbsoluteLayoutBySize(View view, double width, double height)
        {
            var ratio = 0.0;

            if (width > height)
            {
                ratio = height / width;
                AbsoluteLayout.SetLayoutBounds(view, new Rectangle(0.5, 0.5, ratio, 1));
            }
            else
            {
                AbsoluteLayout.SetLayoutBounds(view, new Rectangle(0, 0, 1, 1));
                ratio = width / height;
            }

            AbsoluteLayout.SetLayoutFlags(view, AbsoluteLayoutFlags.All);
        }
    }
}
