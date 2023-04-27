using LoggerService;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RemoteTelevizor
{
    public partial class App : Application
    {
        public App(ILoggingService loggingService)
        {
            InitializeComponent();

            MainPage = new MainPage(loggingService);
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
