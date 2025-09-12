

using LoggerService;
using Microsoft.Maui.Layouts;
using RemoteTelevizor.Models;
using RemoteTelevizor.Services;
using RemoteTelevizor.ViewModels;

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
                RefreshGUI();
            }
        }

        private void RefreshGUI()
        {
            _loggingService.Info($"RefreshGUI");

            Device.BeginInvokeOnMainThread(() =>
            {
                if (_viewModel.Portrait)
                {
                    ResizeArrows(0.5, 0.3, 0.6, 0.6);
                    ResizeButtonFrame(FrameVolume, 0.9, 0.95, 0.2, 0.4);
                    ResizeButtonFrame(FrameBack, 0.1, 0.05, 0.15, 0.15);
                } else
                {
                    ResizeArrows(0.5, 0.5, 0.8, 0.8);
                    ResizeButtonFrame(FrameVolume, 0.95, 0.5, 0.1, 0.6);
                    ResizeButtonFrame(FrameBack, 0.1, 0.2, 0.15, 0.15);
                }
            });
        }

        private void ResizeArrows(double posX, double posY, double width, double height)
        {
            if (!_viewModel.Portrait && FrameRemote.Width > 0)
            {
                // landscape
                width = width * (FrameRemote.Height) / (FrameRemote.Width);
            }

            if (_viewModel.Portrait && FrameRemote.Height > 0)
            {
                // portrait
                height = height * (FrameRemote.Width) / (FrameRemote.Height);
            }

            AbsoluteLayout.SetLayoutFlags(AbsoluteLayoutArrows, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(AbsoluteLayoutArrows, new Rect(posX, posY, width, height));
        }

        public static void ResizeButtonFrame(Frame frame, double posX, double posY, double width, double height)
        {
            AbsoluteLayout.SetLayoutFlags(frame, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(frame, new Rect(posX, posY, width, height));
        }
    }
}
