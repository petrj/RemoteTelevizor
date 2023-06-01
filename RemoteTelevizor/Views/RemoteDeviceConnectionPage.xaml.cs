using Android.Locations;
using LoggerService;
using RemoteTelevizor.Models;
using RemoteTelevizor.Services;
using RemoteTelevizor.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RemoteTelevizor
{
    public partial class RemoteDeviceConnectionPage : ContentPage
    {
        private ILoggingService _loggingService;
        private RemoteDeviceConnectionViewModel _viewModel;
        private DialogService _dialogService;

        public bool Confirmed { get; set; } = false;

        public RemoteDeviceConnectionPage(ILoggingService loggingService)
        {
            InitializeComponent();
            _loggingService = loggingService;

            _dialogService = new DialogService(this);

            NavigationPage.SetHasBackButton(this,false);

            BindingContext = _viewModel = new RemoteDeviceConnectionViewModel(loggingService);
        }

        public RemoteDeviceConnection Connection
        {
            get
            {
                return _viewModel.Connection;
            }
            set
            {
                _viewModel.Connection = value;
            }
        }

        private async void OnButtonAdd(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Connection.Name))
            {
                await _dialogService.Information("Name cannot be empty", "Error");
                return;
            }

            IPAddress ipAddress;
            if (!IPAddress.TryParse(Connection.IP, out ipAddress))
            {
                await _dialogService.Information("Invalid IP address", "Error");
                return;
            }

            int port;
            if (Connection.Port<=0 || Connection.Port>65535)
            {
                await _dialogService.Information("Invalid port", "Error");
                return;
            }

            if (string.IsNullOrEmpty(Connection.SecurityKey))
            {
                await _dialogService.Information("SecurityKey cannot be empty", "Error");
                return;
            }

            Confirmed = true;
            await Device.InvokeOnMainThreadAsync(async () => await Navigation.PopModalAsync());
        }
    }
}
