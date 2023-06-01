using LoggerService;
using RemoteTelevizor.Models;
using RemoteTelevizor.Services;
using RemoteTelevizor.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Java.Util.Jar.Attributes;

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

            MessagingCenter.Subscribe<RemoteDeviceConnection>(this, BaseViewModel.MSG_EditRemoteDevice, async (device) =>
            {
                await EditRemoteDevice(device);
            });

            MessagingCenter.Subscribe<string>(this, BaseViewModel.MSG_AddRemoteDevice, async (sender) =>
            {
                await AddRemoteDevice();
            });

            BindingContext = _viewModel = new ListPageViewModel(loggingService, _appData, _dialogService);
        }

        public RemoteDeviceConnection Connection
        {
            get
            {
                return _viewModel.SelectedItem;
            }
            set
            {
                _viewModel.SelectedItem = value;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.RefreshCommand.Execute(this);

            Device.BeginInvokeOnMainThread(
                delegate
                {
                    // workaround for selecting item in ListView
                    var item = _viewModel.SelectedItem;
                    _viewModel.SelectedItem = null;
                    _viewModel.SelectedItem = item;
                });
        }

        private async Task EditRemoteDevice(RemoteDeviceConnection remoteDeviceConnection)
        {
            if (remoteDeviceConnection == null)
            {
                return;
            }

            var remoteDeviceConnectionPage = new RemoteDeviceConnectionPage(_loggingService);

            remoteDeviceConnectionPage.Connection = RemoteDeviceConnection.CloneFrom(remoteDeviceConnection);

            remoteDeviceConnectionPage.Disappearing += async delegate
            {
                _loggingService.Info("RemoteDeviceConnectionPage Disappearing");

                if (remoteDeviceConnectionPage.Confirmed)
                {
                    remoteDeviceConnection.UpdateFrom(remoteDeviceConnectionPage.Connection);
                    _appData.SaveConnections();

                    MessagingCenter.Send(remoteDeviceConnection, BaseViewModel.MSG_SelectRemoteDevice);
                }
            };

            await Device.InvokeOnMainThreadAsync(async () => await Navigation.PushModalAsync(remoteDeviceConnectionPage));
        }

        private async Task AddRemoteDevice()
        {
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
                    _appData.Connections.Add(remoteDeviceConnectionPage.Connection);
                    _appData.SaveConnections();
                    _viewModel.SelectedItem = remoteDeviceConnectionPage.Connection;
                    _viewModel.RefreshCommand.Execute(null);
                    MessagingCenter.Send(remoteDeviceConnectionPage.Connection, BaseViewModel.MSG_SelectRemoteDevice);
                }
            };

            await Device.InvokeOnMainThreadAsync(async () => await Navigation.PushModalAsync(remoteDeviceConnectionPage));
        }

        private async void OnButtonAdd(object sender, EventArgs e)
        {
            await AddRemoteDevice();
        }
    }
}
