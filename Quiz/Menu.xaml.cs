using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Quiz
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Menu : Page
    {
        List<Classes.Items> items;
        bool playerAdverPanelStatus = false;
        string category = null;

        public Menu()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void goBack_click(object sender, TappedRoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtPlayerName.Text = ApplicationData.Current.LocalSettings.Values["UserName"].ToString();
            comboBox.SelectedItem = "sda";
        }

        private void btnOnePlayer_Click(object sender, TappedRoutedEventArgs e)
        {
            if (playerAdverPanelStatus == false)
            {
                advencedPanelForOnePlayer.Visibility = Windows.UI.Xaml.Visibility.Visible;
                playerAdverPanelStatus = true;
            }
            else
            {
                advencedPanelForOnePlayer.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                playerAdverPanelStatus = false;
            }
        }


        private async void btnChooseUnsver_Click(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(comboBox.SelectedItem.ToString()))
                {
                    Frame.Navigate(typeof(OnePlayer), category);
                }
            }
            catch
            {
                MessageDialog md = new MessageDialog("Пожалуйста, выберите категорию!");
                md.ShowAsync();
            }
        }

        private void btnRecord_Click(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Records));
        }

        private async void btnYourUnsver_Click(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(comboBox.SelectedItem.ToString()))
                {
                    Frame.Navigate(typeof(OnePlayer2), category);
                }
            }
            catch
            {
                MessageDialog md = new MessageDialog("Пожалуйста, выберите категорию!");
                md.ShowAsync();
            }
        }

        private async void btnTwoPlayer_Click(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(comboBox.SelectedItem.ToString()))
                {
                    Frame.Navigate(typeof(TwoPlayer), category);
                }
            }
            catch
            {
                MessageDialog md = new MessageDialog("Пожалуйста, выберите категорию!");
                md.ShowAsync();
            }
        }

        private  void cmbBoxTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(((ComboBoxItem)comboBox.SelectedItem).Content.ToString())
            {
                case "Спорт": category = "Sport"; break;
                case "Животные": category = "Animals"; break;
            }
        }

    }
}
