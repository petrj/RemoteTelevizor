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
        private Size _lastAllocatedSize = new Size(-1, -1);

        private RemoteDeviceConnection _currentConnection;
        private RemoteAccessService _remoteAccessService;

        public Command OnButtonPressedCommand { get; set; }

        public RemoteDeviceViewModel(ILoggingService loggingService, DialogService dialogService)
        {
            _loggingService = loggingService;
            _remoteAccessService = new RemoteAccessService(loggingService);
            _dialogService = dialogService;

            OnButtonPressedCommand = new Command<object>(async (parameter) => await OnButtonPressed(parameter));
        }

        public bool LastAllocatedSizeChanged(double width, double height)
        {
            _loggingService.Info($"AllocatedSizeChanged: {width}/{height}");

            if (_lastAllocatedSize.Width == width &&
                _lastAllocatedSize.Height == height)
            {
                // no size changed
                return false;
            }

            _lastAllocatedSize.Width = width;
            _lastAllocatedSize.Height = height;

            return true;
        }

        public Size LastAllocatedSize
        {
            get
            {
                return _lastAllocatedSize;
            }
        }

        public bool Portrait
        {
            get
            {
                return _lastAllocatedSize.Height> _lastAllocatedSize.Width;
            }
        }

        public double Ratio
        {
            get
            {
                if (Portrait)
                {
                    if (_lastAllocatedSize.Height != 0)
                    {
                        return _lastAllocatedSize.Width / _lastAllocatedSize.Height;
                    }
                } else
                {
                    if (_lastAllocatedSize.Width != 0)
                    {
                        return _lastAllocatedSize.Height / _lastAllocatedSize.Width;
                    }
                }

                return 1;
            }
        }

        private async Task OnButtonPressed(object parameter)
        {
            if (parameter == null)
                return;

            switch (parameter.ToString())
            {
                case "Info":
                    MessagingCenter.Send("ImageInfoButton", BaseViewModel.MSG_AnimeButton);
                    await SendKey(Android.Views.Keycode.Guide.ToString());
                    break;
                case "Back":
                    MessagingCenter.Send("ImageBack", BaseViewModel.MSG_AnimeButton);
                    await SendKey(Android.Views.Keycode.Back.ToString());
                    break;
                case "OK":
                    MessagingCenter.Send("ImageOK", BaseViewModel.MSG_AnimeButton);
                    await SendKey(Android.Views.Keycode.Enter.ToString());
                    break;
            }
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

        public void SetViewAbsoluteLayoutBySize(View view)
        {

            if (Portrait)
            {
                AbsoluteLayout.SetLayoutBounds(view, new Rectangle(0, 0, 1, 1));
            } else
            {
                AbsoluteLayout.SetLayoutBounds(view, new Rectangle(0.5, 0.5, Ratio, 1));
            }

            AbsoluteLayout.SetLayoutFlags(view, AbsoluteLayoutFlags.All);
        }
    }
}
