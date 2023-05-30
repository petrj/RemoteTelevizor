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

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (_viewModel == null)
                return;

            if (_viewModel.LastAllocatedSizeChanged(width, height))
            {
                RefreshGUI();
            }
        }

        public void RefreshGUI()
        {
            _loggingService.Info($"RefreshGUI");

            if (_viewModel == null)
                return;

            // button up

            var width = 0.5;
            var height = 0.5;

            if (_viewModel.Portrait)
            {
                height = width * _viewModel.Ratio;
            } else
            {
                width = height * _viewModel.Ratio;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                AbsoluteLayout.SetLayoutFlags(AbsoluteLayoutArrows, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(AbsoluteLayoutArrows, new Rectangle(0.5, 0.5, width, height));
            });
        }

    }
}
