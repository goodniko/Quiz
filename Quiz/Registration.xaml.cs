using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
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
    public sealed partial class Registration : Page
    {
        public Registration()
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

        private async void btnRegist_Click(object sender, RoutedEventArgs e)
        {
            Classes.User[] com;

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

            foreach (var item in com)
            {
                if (item.login == txtLogin.Text && item.pass ==txtPass.Password)
                {
                    MessageDialog md = new MessageDialog("Пользователья с таким именем уже существует!");
                    await md.ShowAsync();
                }
                return;
            }

            string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);


            string newText = text.Replace("</users>", "\n<user>"
                  + "\n<login>"
                  + txtLogin.Text
                  + "</login>"
                  + "\n<pass>"
                  + txtPass.Password
                  + "</pass>"
                  + "\n</user>"
                  + "\n</users>");


            StorageFile sampleFile2 = await storageFolder.CreateFileAsync("Users.xml", CreationCollisionOption.OpenIfExists);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile2, newText);

            System.Diagnostics.Debug.WriteLine(newText);

            MessageDialog md1 = new MessageDialog("Пользователь успешно добавлен!");
            await md1.ShowAsync();
            Frame.GoBack();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
