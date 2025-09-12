using LoggerService;
using RemoteTelevizor.Models;

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

            var loggingService = new DummyLoggingService();

            _listPage = new ListPage(loggingService, appData);
            _tabbedPage = new MainTabbedPage(loggingService, appData);
            _appData = appData;

            _flyoutPage = new FlyoutPage()
            {
                Title = "Remote Televizor"
            };

            _flyoutPage.Flyout = _listPage;
            _flyoutPage.Detail = _tabbedPage;

            MainPage = new NavigationPage(_flyoutPage);
        }


    }
}