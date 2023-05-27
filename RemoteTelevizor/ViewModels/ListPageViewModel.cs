using Android.InputMethodServices;
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
using Xamarin.Forms;
using static Android.Content.ClipData;

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

        private async Task LongPress(object item)
        {
            SelectedItem = item as RemoteDeviceConnection;

            MessagingCenter.Send(SelectedItem, BaseViewModel.MSG_SelectRemoteDevice);
            await ShowMenu(SelectedItem);
        }

        private async Task ShortPress(object item)
        {
            SelectedItem = item as RemoteDeviceConnection;

            MessagingCenter.Send(SelectedItem, BaseViewModel.MSG_SelectRemoteDevice);
            MessagingCenter.Send(string.Empty, BaseViewModel.MSG_HideFlyoutPage);
        }

        private async Task ShowMenu(object item)
        {
            SelectedItem = item as RemoteDeviceConnection;

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
            if (await _dialogService.Confirm($"Are you sure to delete selected remote device?"))
            {
                try
                {
                    await _semaphoreSlim.WaitAsync();

                    Device.BeginInvokeOnMainThread(delegate { SelectedItem = null; });

                    //RemoteDevices.Remove(remoteDeviceConnection);
                    _appData.Connections.Remove(remoteDeviceConnection);
                }
                finally
                {
                    _semaphoreSlim.Release();

                    await Refresh();

                    if (RemoteDevices.Count > 0)
                    {
                        Device.BeginInvokeOnMainThread(delegate { SelectedItem = RemoteDevices[0]; });
                    }

                    MessagingCenter.Send(SelectedItem, BaseViewModel.MSG_SelectRemoteDevice);
                }
            }
        }

        private async Task Refresh()
        {
            try
            {
                await _semaphoreSlim.WaitAsync();

                IsBusy = true;

                RemoteDevices.Clear();

                foreach (var device in _appData.Connections)
                {
                    RemoteDevices.Add(device);
                }
            }
            finally
            {
                IsBusy = false;

                _semaphoreSlim.Release();
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
