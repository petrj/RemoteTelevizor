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
        public App(ILoggingService loggingService, IAppData appData)
        {
            InitializeComponent();

            var flyoutPage = new FlyoutPage()
            {
                Title = "Remote Televizor"
            };
            flyoutPage.Flyout = new ListPage(loggingService, appData);
            flyoutPage.Detail = new MainTabbedPage(loggingService, appData);

            MainPage = new NavigationPage(flyoutPage);
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
