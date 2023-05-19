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
        public MainTabbedPage(ILoggingService loggingService, IAppData appData)
        {
            InitializeComponent();

            var lastConnection = appData.GetConnectionByIP(appData.LastConnectionIP);

            Children.Add(new NavPage(loggingService, appData) { Title = "Navigation", Connection = lastConnection });
            Children.Add(new NumPage(loggingService, appData) { Title = "Numeric", Connection = lastConnection });
            Children.Add(new MultiMediaPage(loggingService, appData) { Title = "Multimedia", Connection = lastConnection });
        }
    }
}