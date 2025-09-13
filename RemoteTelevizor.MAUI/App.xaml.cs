using LoggerService;
using RemoteTelevizor.Models;
using RemoteTelevizor.ViewModels;

namespace RemoteTelevizor.MAUI
{
    public partial class App : Application
    {
        private ListPage _listPage;
        private MainTabbedPage _tabbedPage;
        private FlyoutPage _flyoutPage;
        private IAppData _appData;

        public App(IAppData appData)
        {
            InitializeComponent();

            Current.UserAppTheme = AppTheme.Dark;

            var loggingService = new DummyLoggingService();

            _listPage = new ListPage(loggingService, appData);
            _tabbedPage = new MainTabbedPage(loggingService, appData);
            _appData = appData;

            MessagingCenter.Subscribe<RemoteDeviceConnection>(this, RemoteDeviceViewModel.MSG_SelectRemoteDevice, (device) =>
            {
                Connection = device;
            });

            MessagingCenter.Subscribe<string>(this, RemoteDeviceViewModel.MSG_SelectNoRemoteDevice, (device) =>
            {
                Connection = null;
            });

            MessagingCenter.Subscribe<string>(this, RemoteDeviceViewModel.MSG_HideFlyoutPage, (msg) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        _flyoutPage.IsPresented = false;
                    } catch (Exception ex)
                    {
                        loggingService.Error($"Error in MSG_HideFlyoutPage: {ex}");
                    }
                });
            });

            MessagingCenter.Subscribe<string>(this, RemoteDeviceViewModel.MSG_ShowOrHideFlyoutPage, (msg) =>
            {
                try
                {
                    _flyoutPage.IsPresented = !_flyoutPage.IsPresented;
                }
                catch (Exception ex)
                {
                    loggingService.Error($"Error in MSG_ShowOrHideFlyoutPage: {ex}");
                }
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
    }
}