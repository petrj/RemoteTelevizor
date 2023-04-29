using Android.InputMethodServices;
using LoggerService;
using RemoteTelevizor.Models;
using RemoteTelevizor.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RemoteTelevizor.ViewModels
{
    public class RemoteDeviceConnectionViewModel : BaseViewModel
    {
        private ILoggingService _loggingService;
        private RemoteDeviceConnection _connection = new RemoteDeviceConnection();

        public RemoteDeviceConnectionViewModel(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public RemoteDeviceConnection Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                _connection = value;

                OnPropertyChanged(nameof(Connection));
                OnPropertyChanged(nameof(IP));
                OnPropertyChanged(nameof(Port));
                OnPropertyChanged(nameof(SecurityKey));
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Name
        {
            get
            {
                return _connection.Name;
            }
            set
            {
                _connection.Name = value;

                OnPropertyChanged(nameof(Name));
            }
        }

        public string IP
        {
            get
            {
                return _connection.IP;
            }
            set
            {
                _connection.IP = value;

                OnPropertyChanged(nameof(IP));
            }
        }

        public int Port
        {
            get
            {
                return _connection.Port;
            }
            set
            {
                _connection.Port = value;

                OnPropertyChanged(nameof(Port));
            }
        }

        public string SecurityKey
        {
            get
            {
                return _connection.SecurityKey;
            }
            set
            {
                _connection.SecurityKey = value;

                OnPropertyChanged(nameof(SecurityKey));
            }
        }
    }
}
