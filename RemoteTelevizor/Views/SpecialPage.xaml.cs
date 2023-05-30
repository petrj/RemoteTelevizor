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

        public SpecialPage(ILoggingService loggingService, IAppData appData, DialogService dialogService)
        {
            InitializeComponent();

            _loggingService = loggingService;

            MessagingCenter.Subscribe<string>(this, RemoteDeviceViewModel.MSG_AnimeFrame, (name) =>
            {
                Task.Run(async () => { await BaseViewModel.Anime<Frame>(name, this); });
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
    }
}