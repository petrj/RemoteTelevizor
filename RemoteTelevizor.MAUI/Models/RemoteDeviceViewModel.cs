using CommunityToolkit.Mvvm.Messaging;
using LoggerService;
using RemoteAccessService;
using RemoteTelevizor.MAUI;
using RemoteTelevizor.Models;
using RemoteTelevizor.Services;

namespace RemoteTelevizor.ViewModels
{
    public class RemoteDeviceViewModel : BaseViewModel
    {
        private ILoggingService _loggingService;
        private DialogService _dialogService;
        private Size _lastAllocatedSize = new Size(-1, -1);

        private RemoteDeviceConnection _currentConnection;
        private RemoteAccessService.RemoteAccessService _remoteAccessService;

        public Command OnButtonPressedCommand { get; set; }
        public Command AddNewRemoteCommand { get; set; }

        public RemoteDeviceViewModel(ILoggingService loggingService, DialogService dialogService)
        {
            _loggingService = loggingService;
            _remoteAccessService = new RemoteAccessService.RemoteAccessService(loggingService);
            _dialogService = dialogService;

            OnButtonPressedCommand = new Command<object>(async (parameter) => await OnButtonPressed(parameter));
            AddNewRemoteCommand = new Command(async () => await AddNewRemote());
        }

        public bool AddNewDeviceButtonVisible
        {
            get
            {
                return _currentConnection == null;
            }
        }

        private async Task AddNewRemote()
        {
            MessagingCenter.Send(String.Empty, BaseViewModel.MSG_AddRemoteDevice);
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

        public bool Portrait
        {
            get
            {
                return _lastAllocatedSize.Height > _lastAllocatedSize.Width;
            }
        }

        private async Task OnButtonPressed(object parameter)
        {
            if (parameter == null)
                return;

            switch (parameter.ToString())
            {
                case "Up":
                    MessagingCenter.Send("ImageUp", BaseViewModel.MSG_AnimeButton);
                    await SendKey("DpadUp");
                    break;
                case "Down":
                    MessagingCenter.Send("ImageDown", BaseViewModel.MSG_AnimeButton);
                    await SendKey("DpadDown");
                    break;
                case "Left":
                    MessagingCenter.Send("ImageLeft", BaseViewModel.MSG_AnimeButton);
                    await SendKey("DpadLeft");
                    break;
                case "Right":
                    MessagingCenter.Send("ImageRight", BaseViewModel.MSG_AnimeButton);
                    await SendKey("DpadRight");
                    break;
                case "OK":
                    MessagingCenter.Send("ImageOK", BaseViewModel.MSG_AnimeButton);
                    await SendKey("Enter");
                    break;


                case "VolUp":
                    MessagingCenter.Send("FrameVolume", BaseViewModel.MSG_AnimeFrame);
                    await SendKey("VolumeUp");
                    break;

                case "VolDown":
                    MessagingCenter.Send("FrameVolume", BaseViewModel.MSG_AnimeFrame);
                    await SendKey("VolDown");
                    break;

                case "Back":
                    MessagingCenter.Send("FrameBack", BaseViewModel.MSG_AnimeFrame);
                    await SendKey("Back");
                    break;

                case "Info":
                    MessagingCenter.Send("FrameInfo", BaseViewModel.MSG_AnimeFrame);
                    await SendKey("Guide");
                    break;

                case "PageUp":
                    MessagingCenter.Send("FramePageUpDown", BaseViewModel.MSG_AnimeFrame);
                    await SendKey("PageUp");
                    break;

                case "PageDown":
                    MessagingCenter.Send("FramePageUpDown", BaseViewModel.MSG_AnimeFrame);
                    await SendKey("PageDown");
                    break;

                case "Home":
                    MessagingCenter.Send("FrameHomeEnd", BaseViewModel.MSG_AnimeFrame);
                    await SendKey("Home");
                    break;
                case "End":
                    MessagingCenter.Send("FrameHomeEnd", BaseViewModel.MSG_AnimeFrame);
                    await SendKey("End");
                    break;

                case "PlayPause":
                    MessagingCenter.Send("FramePlayPause", BaseViewModel.MSG_AnimeFrame);
                    await SendKey("PlayPause");
                    break;
                case "Stop":
                    MessagingCenter.Send("FrameStop", BaseViewModel.MSG_AnimeFrame);
                    await SendKey("Stop");
                    break;

                case "Red":
                    MessagingCenter.Send("FrameRed", BaseViewModel.MSG_AnimeFrame);
                    await SendKey("Red");
                    break;
                case "Yellow":
                    MessagingCenter.Send("FrameYellow", BaseViewModel.MSG_AnimeFrame);
                    await SendKey("Yellow");
                    break;
                case "Green":
                    MessagingCenter.Send("FrameGreen", BaseViewModel.MSG_AnimeFrame);
                    await SendKey("Green");
                    break;
                case "Blue":
                    MessagingCenter.Send("FrameBlue", BaseViewModel.MSG_AnimeFrame);
                    await SendKey("Blue");
                    break;

                case "Txt":
                    MessagingCenter.Send("FrameTxt", BaseViewModel.MSG_AnimeFrame);
                    await SendText();
                    break;
            }

            // num keys 0 .. 9
            for (var i=0; i<=9;i++)
            {
                var num = i.ToString();
                if (num == parameter.ToString())
                {
                    MessagingCenter.Send("Frame" + num, BaseViewModel.MSG_AnimeFrame);
                    await SendKey("Num" + num);
                    break;
                }
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
            _currentConnection = connection;

            if (connection != null)
            {
                _remoteAccessService.SetConnection(connection.IP, connection.Port, connection.SecurityKey);
            }

            OnPropertyChanged(nameof(DeviceName));
            OnPropertyChanged(nameof(AddNewDeviceButtonVisible));
        }

        public string DeviceName
        {
            get
            {
                if (_currentConnection == null)
                {
                    return "No remote device";
                }

                return _currentConnection.Name;
            }
        }

        public async Task SendKey(string keyCode)
        {
            if (_currentConnection == null)
            {
                MessagingCenter.Send("No device selected", BaseViewModel.MSG_ToastMessage);
                return;
            }

            var msg = new RemoteAccessMessage()
            {
                command = "keyDown",
                commandArg1 = keyCode,
                sender = DeviceFriendlyName
            };

            _loggingService.Info($"Sending key: {keyCode}");

            var res = await _remoteAccessService.SendMessage(msg);

            if (res == null)
            {
                MessagingCenter.Send("No response", BaseViewModel.MSG_ToastMessage);
                return;
            }

            if ((res.command != "responseStatus") || (res.commandArg1 != "OK"))
            {
                MessagingCenter.Send("Invalid response", BaseViewModel.MSG_ToastMessage);
                return;
            }
        }

        private async Task SendText()
        {
            if (_currentConnection == null)
            {
                MessagingCenter.Send("No device selected", BaseViewModel.MSG_ToastMessage);
                return;
            }

            var txt = await _dialogService.GetText("", "Send text");

            if (txt == null)
            {
                return;
            }

            var msg = new RemoteAccessMessage()
            {
                command = "sendText",
                commandArg1 = txt,
                sender = DeviceFriendlyName
            };

            _loggingService.Info($"Sending text: {txt}");

            var res = await _remoteAccessService.SendMessage(msg);

            if (res == null)
            {
                MessagingCenter.Send("Timeout", BaseViewModel.MSG_ToastMessage);
                return;
            }

            if (res.command != "responseStatus" || res.commandArg1 != "OK")
            {
                MessagingCenter.Send("Error", BaseViewModel.MSG_ToastMessage);
            }
        }
    }
}
