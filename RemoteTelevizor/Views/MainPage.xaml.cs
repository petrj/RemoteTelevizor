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

namespace RemoteTelevizor
{
    public partial class MainPage : ContentPage
    {
        private ILoggingService _loggingService;
        MainPageViewModel _viewModel;

        public MainPage(ILoggingService loggingService)
        {
            InitializeComponent();
            _loggingService = loggingService;

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
                SecurityKey = "OnlineTelevizor"
            });
        }

        private async void OnButtonList(object sender, EventArgs e)
        {
            await ButtonListFrame.ScaleTo(2, 100);
            await ButtonListFrame.ScaleTo(1, 100);

            var listPage = new ListPage(_loggingService);
            await Navigation.PushAsync(listPage);
        }

        private async void OnButtonDown(object sender, EventArgs e)
        {
            await ButtonDownFrame.ScaleTo(2, 100);
            await ButtonDownFrame.ScaleTo(1, 100);

            _viewModel.SendKey(Android.Views.Keycode.DpadDown.ToString());
        }

        private async void OnButtonUp(object sender, EventArgs e)
        {
            await ButtonUpFrame.ScaleTo(2, 100);
            await ButtonUpFrame.ScaleTo(1, 100);

            _viewModel.SendKey(Android.Views.Keycode.DpadUp.ToString());
        }

        private async void OnButtonLeft(object sender, EventArgs e)
        {
            await ButtonLeftFrame.ScaleTo(2, 100);
            await ButtonLeftFrame.ScaleTo(1, 100);

            _viewModel.SendKey(Android.Views.Keycode.DpadLeft.ToString());
        }

        private async void OnButtonRight(object sender, EventArgs e)
        {
            await ButtonRightFrame.ScaleTo(2, 100);
            await ButtonRightFrame.ScaleTo(1, 100);

            _viewModel.SendKey(Android.Views.Keycode.DpadRight.ToString());
        }

        private async void OnButtonOK(object sender, EventArgs e)
        {
            await ButtonOKFrame.ScaleTo(2, 100);
            await ButtonOKFrame.ScaleTo(1, 100);

            _viewModel.SendKey(Android.Views.Keycode.Enter.ToString());
        }

    }
}
