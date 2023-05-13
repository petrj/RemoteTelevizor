using LoggerService;
using RemoteTelevizor.Models;
using RemoteTelevizor.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Android.Icu.Text.ListFormatter;
using static Android.Provider.MediaStore.Audio;

namespace RemoteTelevizor
{
    public partial class MainPage : ContentPage
    {
        private ILoggingService _loggingService;
        private MainPageViewModel _viewModel;
        private IAppData _appData;
        private Size _lastAllocatedSize = new Size(-1, -1);
        private bool _portrait = false;

        public MainPage(ILoggingService loggingService, IAppData appData)
        {
            InitializeComponent();

            _loggingService = loggingService;
            _appData = appData;

            BindingContext = _viewModel = new MainPageViewModel(loggingService);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            NavigationPage.SetHasNavigationBar(this, false);

            _viewModel.SetConnection(new RemoteDeviceConnection()
            {
                IP = "192.168.1.163",
                Port = 49151,
                Name = "Android TV Box",
                SecurityKey = "OnlineTelevizor"
            });
        }

        private async void OnButtonDown(object sender, EventArgs e)
        {
            //await ButtonDownFrame.ScaleTo(2, 100);
            //await ButtonDownFrame.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.DpadDown.ToString());
        }

        private async void OnButtonUp(object sender, EventArgs e)
        {
            //await ButtonUpFrame.ScaleTo(2, 100);
            //await ButtonUpFrame.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.DpadUp.ToString());
        }

        private async void OnButtonLeft(object sender, EventArgs e)
        {
            //await ButtonLeftFrame.ScaleTo(2, 100);
            //await ButtonLeftFrame.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.DpadLeft.ToString());
        }

        private async void OnButtonRight(object sender, EventArgs e)
        {
            //await ButtonRightFrame.ScaleTo(2, 100);
            //await ButtonRightFrame.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.DpadRight.ToString());
        }

        private async void OnButtonOK(object sender, EventArgs e)
        {
            await ButtonOKFrame.ScaleTo(2, 100);
            await ButtonOKFrame.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.Enter.ToString());
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            _loggingService.Info($"OnSizeAllocated: {width}/{height}");

            if (_viewModel == null)
                return;

            base.OnSizeAllocated(width, height);

            if (_lastAllocatedSize.Width == width &&
                _lastAllocatedSize.Height == height)
            {
                // no size changed
                return;
            }

            var ratio = 0.0;

            if (width > height)
            {
                _portrait = false;
                ratio = height / width;
            }
            else
            {
                _portrait = true;
                ratio = width / height;
            }

            _lastAllocatedSize.Width = width;
            _lastAllocatedSize.Height = height;

            AbsoluteLayout.SetLayoutFlags(RemoteStackLayout, AbsoluteLayoutFlags.All);

            if (_portrait)
            {
                AbsoluteLayout.SetLayoutBounds(RemoteStackLayout, new Rectangle(0, 0, 1, 1));
            } else
            {
                AbsoluteLayout.SetLayoutBounds(RemoteStackLayout, new Rectangle(0.5, 0.5, ratio, 1));
            }
        }

    }
}
