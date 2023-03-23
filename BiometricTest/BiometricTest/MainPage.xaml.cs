using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Xamarin.Forms;

namespace BiometricTest
{
    public partial class MainPage : ContentPage
    {
        CancellationTokenSource _cancel;
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var authType = await Plugin.Fingerprint.CrossFingerprint.Current.GetAuthenticationTypeAsync();
            // authType will give following
            if(authType==AuthenticationType.Face || authType==AuthenticationType.Fingerprint || authType==AuthenticationType.None)
            lblAuthenticationType.Text = "Auth Type: " + authType;
            await AuthenticateAsync("Authendicate with Touch ID");
            //authType == AuthenticationType.Face  - This only applicable in IOS.
            // Android Only Face Will not work , It is the combination of PIN/Password/Pattern + Face so , app will auto ask for PIN/Password/Pattern

        }
        private async Task AuthenticateAsync(string reason, string cancel = null, string fallback = null, string tooFast = null)
        {
            _cancel = false ? new CancellationTokenSource(TimeSpan.FromSeconds(10)) : new CancellationTokenSource();

            var dialogConfig = new AuthenticationRequestConfiguration("My App", reason)
            {
                CancelTitle = cancel,
                FallbackTitle = fallback,
                AllowAlternativeAuthentication = true,   // this will allow us for PIN/PATTERN/Password
                ConfirmationRequired = false
            };

            dialogConfig.HelpTexts.MovedTooFast = tooFast;
            var result = await Plugin.Fingerprint.CrossFingerprint.Current.AuthenticateAsync(dialogConfig, _cancel.Token);
            if (result.Authenticated)
            {
                await DisplayAlert("Success", "Authenticated", "OK");
            }
            else
            {
                await DisplayAlert("Failure", "Not Authenticated", "OK");
            }
        }
        public MainPage()
        {
            InitializeComponent();
        }
    }
}
