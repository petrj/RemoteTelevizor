using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace RemoteTelevizor.Models
{
    public class BaseViewModel : BaseNotifableObject
    {
        public const string MSG_ToastMessage = "ToastMessage";
        public const string MSG_HideFlyoutPage = "HideFlyoutPage";
        public const string MSG_SelectRemoteDevice = "SelectRemoteDevice";
        public const string MSG_SelectNoRemoteDevice = "SelectNoRemoteDevice";

        public const string MSG_AnimeButton = "AnimeButton";

        public const string MSG_EditRemoteDevice = "EditRemoteDevice";

        private bool _isBusy = false;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public static string DeviceFriendlyName
        {
            get
            {
                return $"{Xamarin.Essentials.DeviceInfo.Manufacturer} {Xamarin.Essentials.DeviceInfo.Model}";
            }
        }
    }
}
