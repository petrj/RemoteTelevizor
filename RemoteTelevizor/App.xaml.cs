using LoggerService;
using RemoteTelevizor.Models;
using RemoteTelevizor;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RemoteTelevizor.ViewModels;

namespace RemoteTelevizor
{
    public partial class App : Application
    {
        private ListPage _listPage;
        private MainTabbedPage _tabbedPage;
        private FlyoutPage _flyoutPage;
        private IAppData _appData;

        public App(ILoggingService loggingService, IAppData appData)
        {
            InitializeComponent();

            _listPage = new ListPage(loggingService, appData);
            _tabbedPage = new MainTabbedPage(loggingService, appData);
            _appData = appData;

            MessagingCenter.Subscribe<RemoteDeviceConnection>(this, RemoteDeviceViewModel.MSG_SelectRemoteDevice, (device) =>
            {
                Connection = device;
            });

            MessagingCenter.Subscribe<string>(this, RemoteDeviceViewModel.MSG_HideFlyoutPage, (msg) =>
            {
                AssociatedFlyoutPage.IsPresented = false;
            });

            _flyoutPage = new FlyoutPage()
            {
                Title = "Remote Televizor"
            };

            _flyoutPage.Flyout = _listPage;
            _flyoutPage.Detail = _tabbedPage;

            // setting previous/first connection
            Connection = appData.GetConnectionByIPAndPort(appData.LastConnectionIpAndPort);

            if (Connection == null)
            {
                if (appData.Connections.Count > 0)
                {
                    Connection = appData.Connections[0];
                }
            }

            MainPage = new NavigationPage(_flyoutPage);
        }

        public FlyoutPage AssociatedFlyoutPage
        {
            get
            {
                return _flyoutPage;
            }
        }

        public RemoteDeviceConnection Connection
        {
            get
            {
                return _tabbedPage.Connection;
            }
            set
            {
                _listPage.Connection = value;
                _tabbedPage.Connection = value;

                if (value != null)
                {
                    if (_appData.LastConnectionIpAndPort != value.IpAndPort)
                    {
                        _appData.LastConnectionIpAndPort = value.IpAndPort;
                    }
                }
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
