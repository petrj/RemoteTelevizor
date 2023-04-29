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
    public partial class ListPage : ContentPage
    {
        private ILoggingService _loggingService;
        ListPageViewModel _viewModel;

        public ListPage(ILoggingService loggingService)
        {
            InitializeComponent();
            _loggingService = loggingService;

            BindingContext = _viewModel = new ListPageViewModel(loggingService);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.RefreshCommand.Execute(this);
        }

        private async void Item_Tapped(object sender, ItemTappedEventArgs e)
        {
            var selectedConnection = e.Item as RemoteDeviceConnection;
        }

        private async void OnButtonAdd(object sender, EventArgs e)
        {
            await ButtonAddFrame.ScaleTo(2, 100);
            await ButtonAddFrame.ScaleTo(1, 100);

            var remoteDeviceConnectionPage = new RemoteDeviceConnectionPage(_loggingService);

            remoteDeviceConnectionPage.Connection = new RemoteDeviceConnection()
            {
                 Name = "New remote device",
                 IP = "192.168.1.42",
                 Port = 49151,
                 SecurityKey = "OnlineTelevizor"
            };

            remoteDeviceConnectionPage.Disappearing += delegate
            {
                _loggingService.Info("RemoteDeviceConnectionPage Disappearing");
            };

            Device.BeginInvokeOnMainThread(async () => await Navigation.PushAsync(remoteDeviceConnectionPage));
        }
    }
}
