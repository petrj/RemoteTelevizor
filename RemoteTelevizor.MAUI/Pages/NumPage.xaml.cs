using LoggerService;
using Microsoft.Maui.Layouts;
using RemoteTelevizor.Models;
using RemoteTelevizor.Services;
using RemoteTelevizor.ViewModels;

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
                ResizeGrid(0.5, 0.5, 0.9, 0.9);
            }
        }

        public void ResizeGrid(double posX, double posY, double width, double height)
        {
            _loggingService.Info($"ResizeGrid");

            if (_viewModel == null)
                return;

            Device.BeginInvokeOnMainThread(() =>
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

                AbsoluteLayout.SetLayoutFlags(NumGrid, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(NumGrid, new Rect(posX, posY, width, height));
            });
        }
    }
}