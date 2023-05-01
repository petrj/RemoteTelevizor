using LoggerService;
using RemoteTelevizor.Models;
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

            MainPage = new NavigationPage(new MainPage(loggingService, appData));
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
