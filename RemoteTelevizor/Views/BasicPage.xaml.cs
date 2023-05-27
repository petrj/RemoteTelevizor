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
    public partial class BasicPage : ContentPage
    {
        private ILoggingService _loggingService;
        private RemoteDeviceViewModel _viewModel;

        public BasicPage(ILoggingService loggingService, IAppData appData, DialogService dialogService)
        {
            InitializeComponent();

            _loggingService = loggingService;

            MessagingCenter.Subscribe<string>(this, RemoteDeviceViewModel.MSG_AnimeButton, (buttonName) =>
            {
                Task.Run(async () => { await AnimeButton(buttonName); });
            });

            BindingContext = _viewModel = new RemoteDeviceViewModel(loggingService, dialogService);
        }

        private async Task AnimeButton(string buttonName)
        {
            var img = this.FindByName<Image>(buttonName);
            if (img != null)
            {
                await img.ScaleTo(2, 100);
                await img.ScaleTo(1, 100);
            }
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

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (_viewModel == null)
                return;

            if (_viewModel.LastAllocatedSizeChanged(width, height))
            {
                _viewModel.SetViewAbsoluteLayoutBySize(RemoteStackLayout);
                RefreshGUI();
            }
        }

        public void RefreshGUI()
        {
            _loggingService.Info($"RefreshGUI");

            if (_viewModel == null)
                return;

            // button up

            var width = 0.75;
            var height = 0.75;

            if (_viewModel.Portrait)
            {
                height = width * _viewModel.Ratio;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                AbsoluteLayout.SetLayoutFlags(AbsoluteLayoutArrows, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(AbsoluteLayoutArrows, new Rectangle(0.5, 0.5, width, height));
            });
        }

    }
}
