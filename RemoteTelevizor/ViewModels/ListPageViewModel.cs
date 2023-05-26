using Android.InputMethodServices;
using LoggerService;
using RemoteTelevizor.Models;
using RemoteTelevizor.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
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
        private IAppData _appData;
        private DialogService _dialogService;

        public ObservableCollection<RemoteDeviceConnection> RemoteDevices { get; set; } = new ObservableCollection<RemoteDeviceConnection>();

        public Command RefreshCommand { get; set; }
        public Command AddRemoteCommand { get; set; }
        public Command MenuCommand { get; set; }

        public Command LongPressCommand { get; set; }
        public Command ShortPressCommand { get; set; }

        public ListPageViewModel(ILoggingService loggingService, IAppData appData, DialogService dialogService)
        {
            _loggingService = loggingService;
            _appData = appData;
            _dialogService = dialogService;

            RefreshCommand = new Command(async () => await Refresh());

            MenuCommand = new Command(async () => await ShowMenu());
            LongPressCommand = new Command(LongPress);
            ShortPressCommand = new Command(ShortPress);
        }

        private void LongPress(object item)
        {
        }

        private void ShortPress(object item)
        {
        }

        private async Task ShowMenu()
        {
            var actions = new List<string>();
            actions.Add("Edit");
            actions.Add("Delete");

            var selectedvalue = await _dialogService.Select(actions, _selectedItem.Name);
        }

        private async Task Refresh()
        {
            IsBusy = true;


            try
            {
                RemoteDevices.Clear();

                foreach (var device in _appData.Connections)
                {
                    RemoteDevices.Add(device);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        public bool EditButtonVisible
        {
            get
            {
                return _selectedItem != null;
            }
        }

        public bool DeleteButtonVisible
        {
            get
            {
                return _selectedItem != null;
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

                OnPropertyChanged(nameof(EditButtonVisible));
                OnPropertyChanged(nameof(DeleteButtonVisible));
            }
        }
    }
}
