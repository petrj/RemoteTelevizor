using LoggerService;
using RemoteAccess;
using RemoteTelevizor.Models;
using RemoteTelevizor.Services;
using RemoteTelevizor.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RemoteTelevizor
{
    public partial class NavPage : ContentPage
    {
        private ILoggingService _loggingService;
        private RemoteDeviceViewModel _viewModel;
        private IAppData _appData;
        private Size _lastAllocatedSize = new Size(-1, -1);
        private DialogService _dialogService;

        public NavPage(ILoggingService loggingService, IAppData appData, DialogService dialogService)
        {
            InitializeComponent();

            _loggingService = loggingService;
            _appData = appData;
            _dialogService = dialogService;

            BindingContext = _viewModel = new RemoteDeviceViewModel(loggingService, dialogService);
        }

        public RemoteDeviceConnection Connection
        {
            get
            {
                return _viewModel.Connection;
            }
            set
            {
                _viewModel.SetConnection(value);
            }
        }

        private async void OnButtonDown(object sender, EventArgs e)
        {
            await ButtonDownFrame.ScaleTo(2, 100);
            await ButtonDownFrame.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.DpadDown.ToString());
        }

        private async void OnButtonUp(object sender, EventArgs e)
        {
            await ButtonUpFrame.ScaleTo(2, 100);
            await ButtonUpFrame.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.DpadUp.ToString());
        }

        private async void OnButtonLeft(object sender, EventArgs e)
        {
            await ButtonLeftFrame.ScaleTo(2, 100);
            await ButtonLeftFrame.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.DpadLeft.ToString());
        }

        private async void OnButtonRight(object sender, EventArgs e)
        {
            await ButtonRightFrame.ScaleTo(2, 100);
            await ButtonRightFrame.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.DpadRight.ToString());
        }

        private async void OnButtonOK(object sender, EventArgs e)
        {
            await ButtonOKFrame.ScaleTo(2, 100);
            await ButtonOKFrame.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.Enter.ToString());
        }

        private async void OnButtonVolumeUp(object sender, EventArgs e)
        {
            await VolumeUpLabel.ScaleTo(2, 100);
            await VolumeUpLabel.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.VolumeUp.ToString());
        }

        private async void OnButtonVolumeDown(object sender, EventArgs e)
        {
            await VolumeDownLabel.ScaleTo(2, 100);
            await VolumeDownLabel.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.VolumeDown.ToString());
        }

        private async void OnButtonEscape(object sender, EventArgs e)
        {
            await ImageBack.ScaleTo(2, 100);
            await ImageBack.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.Escape.ToString());
        }

        private async void OnButtonMenu(object sender, EventArgs e)
        {
            await ImageMenu.ScaleTo(2, 100);
            await ImageMenu.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.Menu.ToString());
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            _loggingService.Info($"OnSizeAllocated: {width}/{height}");

            base.OnSizeAllocated(width, height);

            if (_viewModel == null)
                return;

            if (_lastAllocatedSize.Width == width &&
                _lastAllocatedSize.Height == height)
            {
                // no size changed
                return;
            }

            _lastAllocatedSize.Width = width;
            _lastAllocatedSize.Height = height;

            RemoteDeviceViewModel.SetViewAbsoluteLayoutBySize(RemoteStackLayout, width, height);
        }

    }
}
