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
    public partial class NumPage : ContentPage
    {
        private ILoggingService _loggingService;
        private RemoteDeviceViewModel _viewModel;
        private IAppData _appData;

        public NumPage(ILoggingService loggingService, IAppData appData, DialogService dialogService)
        {
            InitializeComponent();

            _loggingService = loggingService;
            _appData = appData;

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
                ResizeGrid(0.5, 0.5, 0.8, 0.8);
            }
        }

        public void ResizeGrid(double posX, double posY, double width, double height)
        {
            _loggingService.Info($"RefreshGUI");

            if (_viewModel == null)
                return;

            Device.BeginInvokeOnMainThread(() =>
            {
                if (FrameRemote.Width > FrameRemote.Height && FrameRemote.Width > 0)
                {
                    // landscape
                    width = width * (FrameRemote.Height) / (FrameRemote.Width);
                }

                if (FrameRemote.Height > FrameRemote.Width && FrameRemote.Height > 0)
                {
                    // portrait
                    height = height * (FrameRemote.Width) / (FrameRemote.Height);
                }

                AbsoluteLayout.SetLayoutFlags(NumGrid, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(NumGrid, new Rectangle(posX, posY, width, height));
            });
        }
    }
}