using LoggerService;
using RemoteTelevizor.Models;
using RemoteTelevizor.Services;
using RemoteTelevizor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteTelevizor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpecialPage : ContentPage
    {
        private ILoggingService _loggingService;
        private RemoteDeviceViewModel _viewModel;

        public SpecialPage(ILoggingService loggingService, IAppData appData, DialogService dialogService)
        {
            InitializeComponent();

            _loggingService = loggingService;

            MessagingCenter.Subscribe<string>(this, RemoteDeviceViewModel.MSG_AnimeFrame, async (name) =>
            {
                await BaseViewModel.Anime<Frame>(name, this);
            });

            BindingContext = _viewModel = new RemoteDeviceViewModel(loggingService, dialogService);
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
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (_viewModel.Portrait)
                    {
                        BasicPage.ResizeButtonFrame(FramePageUpDown, 0.1, 0.5, 0.2, 0.4);
                        BasicPage.ResizeButtonFrame(FrameHomeEnd, 0.9, 0.5, 0.2, 0.4);
                    }
                    else
                    {
                        BasicPage.ResizeButtonFrame(FramePageUpDown, 0.05, 0.3, 0.1, 0.6);
                        BasicPage.ResizeButtonFrame(FrameHomeEnd, 0.9, 0.3, 0.1, 0.6);
                    }
                });
            }
        }
    }
}