using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RemoteTelevizor.Models
{
    public class BaseViewModel : BaseNotifableObject
    {
        public const string MSG_ToastMessage = "ToastMessage";
        public const string MSG_HideFlyoutPage = "HideFlyoutPage";
        public const string MSG_SelectRemoteDevice = "SelectRemoteDevice";
        public const string MSG_SelectNoRemoteDevice = "SelectNoRemoteDevice";

        public const string MSG_AnimeButton = "AnimeButton";
        public const string MSG_AnimeFrame= "AnimeFrame";

        public const string MSG_EditRemoteDevice = "EditRemoteDevice";
        public const string MSG_AddRemoteDevice = "AddRemoteDevice";

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

        public static async Task Anime<T>(string name, ContentPage page) where T : VisualElement
        {
            var visualElement = page.FindByName<T>(name);
            if (visualElement != null)
            {
                await Device.InvokeOnMainThreadAsync(async () =>
                {
                    await visualElement.ScaleTo(1.5, 100);
                    await visualElement.ScaleTo(1, 100);
                });
            }
        }
    }
}
