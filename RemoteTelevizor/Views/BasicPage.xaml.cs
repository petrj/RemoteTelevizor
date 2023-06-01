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
using static Java.Util.Jar.Attributes;

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


            MessagingCenter.Subscribe<string>(this, BaseViewModel.MSG_AnimeButton, async (name) =>
            {
                await BaseViewModel.Anime<Image>(name, this);
            });

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
                ResizeArrows(0.5,0.3,0.6,0.6);
            }
        }

        public void ResizeArrows(double posX, double posY, double width, double height)
        {
            _loggingService.Info($"RefreshGUI");

            if (_viewModel == null)
                return;

            Device.BeginInvokeOnMainThread(() =>
            {
                if (FrameRemote.Width> FrameRemote.Height && FrameRemote.Width > 0)
                {
                    // landscape
                    width = width * (FrameRemote.Height) / (FrameRemote.Width);
                }

                if (FrameRemote.Height > FrameRemote.Width && FrameRemote.Height > 0)
                {
                    // portrait
                    height = height * (FrameRemote.Width) / (FrameRemote.Height);
                }

                AbsoluteLayout.SetLayoutFlags(AbsoluteLayoutArrows, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(AbsoluteLayoutArrows, new Rectangle(posX, posY, width, height));
            });
        }

    }
}
