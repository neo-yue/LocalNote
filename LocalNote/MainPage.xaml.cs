using LocalNote.Command;
using LocalNote.Dialog;
using LocalNote.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LocalNote
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ViewModels.LocalNoteViewModel LNViewModel { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            this.LNViewModel = new ViewModels.LocalNoteViewModel();

            //bind mainpage to  LNViewModel
            LNViewModel.MainPage = this;
        }

        //Get and assign value to textbox
        public string NoteContent
        {
            get { return ContentBox.Text; }

            set { ContentBox.Text = value; }

        }

        //Clean the textbox
        public void CleanTextbox()
        {
            ContentBox.Text = "";
        }

        //Lock textbox
        public void textLock()
        {
            ContentBox.IsReadOnly = true;
        }

        //Unlock textbox
        public void textUnlock()
        {
            ContentBox.IsReadOnly = false;
        }

        //Returns the editability of the textbox
        public bool textboxStatus()
        {

            return ContentBox.IsReadOnly;
        }

        private async void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutDialog ad = new AboutDialog();
            ContentDialogResult result = await ad.ShowAsync();


         }

        //private void AboutButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Frame.Navigate(typeof(AboutPage));
        //}
    }
}
