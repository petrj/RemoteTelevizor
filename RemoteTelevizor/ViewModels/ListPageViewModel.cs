﻿using Android.InputMethodServices;
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
    public class ListPageViewModel : BaseViewModel
    {
        private ILoggingService _loggingService;
        private RemoteDeviceConnection _selectedItem;

        public ObservableCollection<RemoteDeviceConnection> RemoteDevices { get; set; } = new ObservableCollection<RemoteDeviceConnection>();

        public Command RefreshCommand { get; set; }

        public ListPageViewModel(ILoggingService loggingService)
        {
            _loggingService = loggingService;

            RefreshCommand = new Command(async () => await Refresh());
        }

        private async Task Refresh()
        {
            IsBusy = true;

            try
            {

                RemoteDevices.Clear();

                RemoteDevices.Add(new RemoteDeviceConnection()
                {
                    Name = "Evolveo TV Box",
                    IP = "10.0.0.18",
                    Port = 49151,
                    SecurityKey = "OnlineTelevizor"
                });
                RemoteDevices.Add(new RemoteDeviceConnection()
                {
                    Name = "Lenovo Tablet",
                    IP = "10.0.0.231",
                    Port = 49151,
                    SecurityKey = "OnlineTelevizor"
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        public RemoteDeviceConnection SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                 _selectedItem = value;

                OnPropertyChanged(nameof(SelectedItem));
            }
        }
    }
}
