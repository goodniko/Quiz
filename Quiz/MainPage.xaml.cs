using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Quiz
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Classes.User[] com;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            hideStatusBar();
        }

        async void hideStatusBar()
        {
            await Windows.UI.ViewManagement.StatusBar.GetForCurrentView().HideAsync();
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
            else
                Application.Current.Exit();
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

    


        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile sampleFile2 = await folder.CreateFileAsync("Users.xml", CreationCollisionOption.FailIfExists);

                string text = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"
                            + "\n<users>"
                            + "\n</users>";
                await Windows.Storage.FileIO.WriteTextAsync(sampleFile2, text.ToString());
            }
            catch
            {

            }


                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("Users.xml");
                string users_text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);

                XDocument xDoc = XDocument.Parse(users_text);

                com = xDoc.Root.Elements("user").Select(p =>
                {
                    return new Classes.User()
                    {
                        login = p.Element("login").Value,
                        pass = p.Element("pass").Value
                    };
                }).ToArray();
           
        }

        private void btnRegist_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Registration));
        }

        private async void btnAouth_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog md;

            if (string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrWhiteSpace(txtPass.Password))
            {
                md = new MessageDialog("Пожалуйста заполните все поля!");
                await md.ShowAsync();
                return;
            }

            for (int i = 0; i < com.Length; i++)
            {
                if (string.Equals(txtLogin.Text, com[i].login, StringComparison.CurrentCultureIgnoreCase) && string.Equals(txtPass.Password, com[i].pass, StringComparison.CurrentCultureIgnoreCase))
                {
                    ApplicationData.Current.LocalSettings.Values["UserName"] = com[i].login;
                    Frame.Navigate(typeof(Menu));
                    return;
                }
            }

            md = new MessageDialog("Неправильный логин или пароль!");
            await md.ShowAsync();
        }
    }
}
