using LoggerService;
using RemoteTelevizor.Models;
using RemoteTelevizor.Services;
using RemoteTelevizor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RemoteTelevizor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpecialPage : ContentPage
    {
        private ILoggingService _loggingService;
        private RemoteDeviceViewModel _viewModel;
        private IAppData _appData;
        private Size _lastAllocatedSize = new Size(-1, -1);

        public SpecialPage(ILoggingService loggingService, IAppData appData, DialogService dialogService)
        {
            InitializeComponent();

            _loggingService = loggingService;
            _appData = appData;

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
            _loggingService.Info($"OnSizeAllocated: {width}/{height}");

            base.OnSizeAllocated(width, height);

            if (_viewModel == null)
                return;

            if (_lastAllocatedSize.Width == width &&
                _lastAllocatedSize.Height == height)
            {
                // no size changed
                return;
            }

            _lastAllocatedSize.Width = width;
            _lastAllocatedSize.Height = height;

            _viewModel.SetViewAbsoluteLayoutBySize(RemoteStackLayout);
        }


    }
}