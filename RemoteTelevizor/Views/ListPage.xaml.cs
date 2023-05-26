using LoggerService;
using RemoteTelevizor.Models;
using RemoteTelevizor.Services;
using RemoteTelevizor.ViewModels;
using System;
using Xamarin.Forms;

namespace RemoteTelevizor
{
    public partial class ListPage : ContentPage
    {
        private ILoggingService _loggingService;
        private ListPageViewModel _viewModel;
        private IAppData _appData;
        private DialogService _dialogService;

        public App ParentPage { get; set; }

        public ListPage(ILoggingService loggingService, IAppData appData)
        {
            InitializeComponent();
            _loggingService = loggingService;
            _appData = appData;

            _dialogService = new DialogService(this);

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

        private async void Item_Tapped(object sender, ItemTappedEventArgs e)
        {
            ParentPage.Connection = e.Item as RemoteDeviceConnection;
            ParentPage.AssociatedFlyoutPage.IsPresented = false;
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
                    _appData.Connections = _viewModel.RemoteDevices;
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
                _appData.Connections = _viewModel.RemoteDevices;

                _viewModel.SelectedItem = null;
                _viewModel.RefreshCommand.Execute(this);
            }
        }

        private async void OnButtonShowMenu(object sender, EventArgs e)
        {
            _viewModel.MenuCommand.Execute(null);
        }

        private async void OnButtonAdd(object sender, EventArgs e)
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
                    _viewModel.RefreshCommand.Execute(null);
                }
            };

            Device.BeginInvokeOnMainThread(async () => await Navigation.PushAsync(remoteDeviceConnectionPage));
        }
    }
}
