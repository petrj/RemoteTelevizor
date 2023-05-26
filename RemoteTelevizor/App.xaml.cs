using LoggerService;
using RemoteTelevizor.Models;
using RemoteTelevizor;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RemoteTelevizor
{
    public partial class App : Application
    {
        private ListPage _listPage;
        private MainTabbedPage _tabbedPage;
        private FlyoutPage _flyoutPage;

        public App(ILoggingService loggingService, IAppData appData)
        {
            InitializeComponent();

            _flyoutPage = new FlyoutPage()
            {
                Title = "Remote Televizor"
            };

            _listPage = new ListPage(loggingService, appData);
            _tabbedPage = new MainTabbedPage(loggingService, appData);
            _listPage.ParentPage = this;

            _flyoutPage.Flyout = _listPage;
            _flyoutPage.Detail = _tabbedPage;

            // setting last connection
            Connection = appData.GetConnectionByIP(appData.LastConnectionIP);

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
