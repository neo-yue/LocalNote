using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LocalNote
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        Package package;
        PackageId packageID;
        PackageVersion packageVersion;
        String versionID;
        String appName;
        String auther;

        public AboutPage()
        {
            this.InitializeComponent();

            package = Package.Current;
            packageID = package.Id;
            packageVersion = packageID.Version;
            versionID = string.Format("{0}.{1}.{2}.{3}", packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
            appName = package.DisplayName;
            auther = package.PublisherDisplayName;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            SystemNavigationManager.GetForCurrentView().BackRequested += About_BackRequested;


        }

        private async void About_BackRequested(object sender, BackRequestedEventArgs e)
        {
            try
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                }

                //The operation has been processed and will no longer be processed
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                ContentDialog error = new ContentDialog
                {
                    Title = "Error",
                    Content = "Can't go back, please try again later",
                    PrimaryButtonText = "OK"
                };
                await error.ShowAsync();
            }
        }
    }
}
