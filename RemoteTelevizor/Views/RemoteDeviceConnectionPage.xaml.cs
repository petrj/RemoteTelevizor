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
    public partial class RemoteDeviceConnectionPage : ContentPage
    {
        private ILoggingService _loggingService;
        RemoteDeviceConnectionViewModel _viewModel;
        DialogService _dialogService;

        public RemoteDeviceConnectionPage(ILoggingService loggingService)
        {
            InitializeComponent();
            _loggingService = loggingService;

            _dialogService = new DialogService(this);

            NavigationPage.SetHasBackButton(this,false);

            BindingContext = _viewModel = new RemoteDeviceConnectionViewModel(loggingService);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
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
            await Navigation.PopAsync();
        }
    }
}
