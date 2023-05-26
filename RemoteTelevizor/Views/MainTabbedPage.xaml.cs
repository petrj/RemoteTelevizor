using LoggerService;
using RemoteTelevizor.Models;
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
    public partial class MainTabbedPage : TabbedPage
    {
        private NavPage _navPage;
        private NumPage _numPage;
        private MultiMediaPage _multiMediaPage;

        public MainTabbedPage(ILoggingService loggingService, IAppData appData)
        {
            InitializeComponent();

            _navPage = new NavPage(loggingService, appData) { Title = "Navigation"};
            _numPage = new NumPage(loggingService, appData) { Title = "Numeric"};
            _multiMediaPage = new MultiMediaPage(loggingService, appData) { Title = "Multimedia" };

            Children.Add(_navPage);
            Children.Add(_numPage);
            Children.Add(_multiMediaPage);
        }

        public RemoteDeviceConnection Connection
        {
            get
            {
                return _navPage.Connection;
            }
            set
            {
                _navPage.Connection = value;
                _numPage.Connection = value;
                _multiMediaPage.Connection = value;
            }
        }
    }
}