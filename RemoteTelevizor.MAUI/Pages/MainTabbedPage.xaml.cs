using LoggerService;
using RemoteTelevizor.Models;
using RemoteTelevizor.Services;

namespace RemoteTelevizor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        private BasicPage _basicPage;
        private NumPage _numPage;
        private SpecialPage _specialPage;

        public MainTabbedPage(ILoggingService loggingService, IAppData appData)
        {
            InitializeComponent();

            var dialogService = new DialogService(this);

            _basicPage = new BasicPage(loggingService, appData, dialogService) { Title = "Basic"};
            _numPage = new NumPage(loggingService, appData, dialogService) { Title = "Numeric"};
            _specialPage = new SpecialPage(loggingService, appData, dialogService) { Title = "Special" };

            Children.Add(_basicPage);
            Children.Add(_numPage);
            Children.Add(_specialPage);
        }

        public RemoteDeviceConnection Connection
        {
            get
            {
                return _basicPage.Connection;
            }
            set
            {
                _basicPage.Connection = value;
                _numPage.Connection = value;
                _specialPage.Connection = value;
            }
        }

        private void ToolbarItemMenu_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send("", BaseViewModel.MSG_ShowOrHideFlyoutPage);
        }
    }
}