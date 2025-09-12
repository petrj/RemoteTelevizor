using LoggerService;
using RemoteTelevizor.Models;
using RemoteTelevizor.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace RemoteTelevizor.ViewModels
{
    public class ListPageViewModel : BaseViewModel
    {
        private ILoggingService _loggingService;
        private RemoteDeviceConnection _selectedItem;
        private IAppData _appData;
        private DialogService _dialogService;
        private static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

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

            MenuCommand = new Command<object>(async (o) => await ShowMenu(o));

            LongPressCommand = new Command<object>(async (o) => await LongPress(o));
            ShortPressCommand = new Command<object>(async (o) => await ShortPress(o));
        }

        private void NotifySelectgedRemoteDeviceChange()
        {
            if (SelectedItem == null)
            {
                MessagingCenter.Send(String.Empty, BaseViewModel.MSG_SelectNoRemoteDevice);
            } else
            {
                MessagingCenter.Send(SelectedItem, BaseViewModel.MSG_SelectRemoteDevice);
            }
        }

        private async Task LongPress(object item)
        {
            SelectedItem = item as RemoteDeviceConnection;
            NotifySelectgedRemoteDeviceChange();

            await ShowMenu(SelectedItem);
        }

        private async Task ShortPress(object item)
        {
            SelectedItem = item as RemoteDeviceConnection;

            NotifySelectgedRemoteDeviceChange();
            MessagingCenter.Send(string.Empty, BaseViewModel.MSG_HideFlyoutPage);
        }

        private async Task ShowMenu(object item)
        {
            SelectedItem = item as RemoteDeviceConnection;

            NotifySelectgedRemoteDeviceChange();

            if (SelectedItem == null)
                return;

            var actions = new List<string>();
            actions.Add("Edit");
            actions.Add("Delete");

            var selectedvalue = await _dialogService.Select(actions, SelectedItem.Name);

            if (selectedvalue == "Edit")
            {
                MessagingCenter.Send(SelectedItem, BaseViewModel.MSG_EditRemoteDevice);
            } else
            if (selectedvalue == "Delete")
            {
                await Delete(SelectedItem);
            }
        }

        private async Task Delete(RemoteDeviceConnection remoteDeviceConnection)
        {
            if (await _dialogService.Confirm($"Are you sure to delete selected device {remoteDeviceConnection.Name}?"))
            {
                try
                {
                    await _semaphoreSlim.WaitAsync();

                    _appData.Connections.Remove(remoteDeviceConnection);
                    _appData.SaveConnections();
                }
                finally
                {
                    _semaphoreSlim.Release();

                    await Refresh();

                    NotifySelectgedRemoteDeviceChange();

                    if (RemoteDevices.Count==0)
                    {
                        MessagingCenter.Send(string.Empty, BaseViewModel.MSG_HideFlyoutPage);
                    }
                }
            }
        }

        public async Task Refresh()
        {
            try
            {
                await _semaphoreSlim.WaitAsync();

                string selectedDevice = SelectedItem == null ? null : SelectedItem.IpAndPort;
                SelectedItem = null;
                RemoteDeviceConnection first = null;

                IsBusy = true;

                RemoteDevices.Clear();

                foreach (var device in _appData.Connections)
                {
                    if (first == null)
                    {
                        first = device;
                    }
                    RemoteDevices.Add(device);

                    if (device.IpAndPort == selectedDevice)
                    {
                        SelectedItem = device;
                    }
                }

                if (SelectedItem == null)
                {
                    SelectedItem = first;
                }
            }
            finally
            {
                _semaphoreSlim.Release();

                IsBusy = false;

                OnPropertyChanged(nameof(RemoteDevices));
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
                if (SelectedItem != null)
                {
                    SelectedItem.NotifyPropertyChange();
                }

                OnPropertyChanged(nameof(EditButtonVisible));
                OnPropertyChanged(nameof(DeleteButtonVisible));
            }
        }
    }
}
