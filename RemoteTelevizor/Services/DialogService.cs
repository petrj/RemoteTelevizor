using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RemoteTelevizor.Services
{
    public class DialogService
    {
        public DialogService(Page page = null)
        {
            DialogPage = page;
        }

        public Page DialogPage { get; set; }

        public async Task<bool> Confirm(string message, string title = "Confirmation", string accept = "Yes", string cancel = "No")
        {
            var dp = DialogPage == null ? Application.Current.MainPage : DialogPage;
            var result = await dp.DisplayAlert(title, message, accept, cancel);

            return result;
        }

        public async Task Information(string message, string title = "Information")
        {
            var dp = DialogPage == null ? Application.Current.MainPage : DialogPage;
            await dp.DisplayAlert(title, message, "OK");
        }

        public async Task<string> Select(List<string> options, string title = "Select", string cancel = "Back")
        {
            var dp = DialogPage == null ? Application.Current.MainPage : DialogPage;
            return await dp.DisplayActionSheet(title, cancel, null, options.ToArray());
        }

        public async Task<string> GetText(string message, string title, string defaultValue = "")
        {
            return await DialogPage.DisplayPromptAsync(title, message, initialValue: defaultValue, keyboard: Xamarin.Forms.Keyboard.Text);
        }
    }
}
