using LoggerService;
using RemoteTelevizor.Models;
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
        private Size _lastAllocatedSize = new Size(-1, -1);

        public NumPage(ILoggingService loggingService, IAppData appData)
        {
            InitializeComponent();

            _loggingService = loggingService;
            _appData = appData;

            BindingContext = _viewModel = new RemoteDeviceViewModel(loggingService);
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

            RemoteDeviceViewModel.SetViewAbsoluteLayoutBySize(RemoteStackLayout, width, height);
        }

        private async void OnButton0(object sender, EventArgs e)
        {
            await Image0.ScaleTo(2, 100);
            await Image0.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.Num0.ToString());
        }

        private async void OnButton1(object sender, EventArgs e)
        {
            await Image0.ScaleTo(2, 100);
            await Image0.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.Num1.ToString());
        }

        private async void OnButton2(object sender, EventArgs e)
        {
            await Image0.ScaleTo(2, 100);
            await Image0.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.Num2.ToString());
        }

        private async void OnButton3(object sender, EventArgs e)
        {
            await Image0.ScaleTo(2, 100);
            await Image0.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.Num3.ToString());
        }


        private async void OnButton4(object sender, EventArgs e)
        {
            await Image0.ScaleTo(2, 100);
            await Image0.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.Num4.ToString());
        }

        private async void OnButton5(object sender, EventArgs e)
        {
            await Image0.ScaleTo(2, 100);
            await Image0.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.Num5.ToString());
        }

        private async void OnButton6(object sender, EventArgs e)
        {
            await Image0.ScaleTo(2, 100);
            await Image0.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.Num6.ToString());
        }

        private async void OnButton7(object sender, EventArgs e)
        {
            await Image0.ScaleTo(2, 100);
            await Image0.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.Num7.ToString());
        }


        private async void OnButton8(object sender, EventArgs e)
        {
            await Image0.ScaleTo(2, 100);
            await Image0.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.Num8.ToString());
        }

        private async void OnButton9(object sender, EventArgs e)
        {
            await Image0.ScaleTo(2, 100);
            await Image0.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.Num9.ToString());
        }
    }
}