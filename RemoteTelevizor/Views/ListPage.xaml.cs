using LoggerService;
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
    public partial class ListPage : ContentPage
    {
        private ILoggingService _loggingService;
        private ListPageViewModel _viewModel;
        private IAppData _appData;
        private DialogService _dialogService;

        public ListPage(ILoggingService loggingService, IAppData appData)
        {
            InitializeComponent();
            _loggingService = loggingService;
            _appData = appData;

            _dialogService = new DialogService(this);

            BindingContext = _viewModel = new ListPageViewModel(loggingService, _appData);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.SelectedItem = null;
            _viewModel.RefreshCommand.Execute(this);
        }

        private async void Item_Tapped(object sender, ItemTappedEventArgs e)
        {
            var selectedConnection = e.Item as RemoteDeviceConnection;
        }

        private async void OnButtonEdit(object sender, EventArgs e)
        {
            var remoteDeviceConnectionPage = new RemoteDeviceConnectionPage(_loggingService);

            remoteDeviceConnectionPage.Connection = _viewModel.SelectedItem;

            remoteDeviceConnectionPage.Disappearing += delegate
            {
                _loggingService.Info("RemoteDeviceConnectionPage Disappearing");

                if (remoteDeviceConnectionPage.Confirmed)
                {
                    _appData.SaveConnections(_viewModel.RemoteDevices);
                } else
                {
                    _viewModel.SelectedItem = null;
                    _viewModel.RefreshCommand.Execute(this);
                }
            };

            Device.BeginInvokeOnMainThread(async () => await Navigation.PushAsync(remoteDeviceConnectionPage));
        }

        private async void OnButtonDelete(object sender, EventArgs e)
        {
            if (await _dialogService.Confirm($"Are you sure to delete selected remote device?"))
            {
                _viewModel.RemoteDevices.Remove(_viewModel.SelectedItem);
                _appData.SaveConnections(_viewModel.RemoteDevices);

                _viewModel.SelectedItem = null;
                _viewModel.RefreshCommand.Execute(this);
            }
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

                if (remoteDeviceConnectionPage.Confirmed)
                {
                    var devices = _appData.LoadConnections();
                    devices.Add(remoteDeviceConnectionPage.Connection);
                    _appData.SaveConnections(devices);
                }
            };

            Device.BeginInvokeOnMainThread(async () => await Navigation.PushAsync(remoteDeviceConnectionPage));
        }
    }
}
