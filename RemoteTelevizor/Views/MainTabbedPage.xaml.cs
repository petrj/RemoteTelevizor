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

            Children.Add(new NavPage(loggingService, appData) { Title = "Navigation" });
            Children.Add(new NumPage(loggingService, appData) { Title = "Numeric" });
            Children.Add(new MultiMediaPage(loggingService, appData) { Title = "Multimedia" });
        }
    }
}