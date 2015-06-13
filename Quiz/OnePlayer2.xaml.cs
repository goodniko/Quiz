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
    public sealed partial class OnePlayer2 : Page
    {
        List<Classes.Items> items;
        int randNumb;
        int countTrueUnsver = 0;
        string category;

        public OnePlayer2()
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
            category = e.Parameter.ToString();

        }

        private void Gen_Quest()
        {
            Random rand = new Random();
            randNumb = rand.Next(0, items.Count);

            txtQuestion.Text = items[randNumb].question;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage();
                httpClient.BaseAddress = new Uri("https://api.parse.com/1/classes/" + category);
                request.Method = new HttpMethod("GET");
                request.Headers.Add("X-Parse-Application-Id", "q8r9XYoXbEw1kXsWIXbyCNQm9ylcWfAS0fqlWWNx");
                request.Headers.Add("X-Parse-REST-API-Key", "2nke5LSbXQmTiA9FaVytlBihjOOz4fPcoxjEHlee");

                var response = await httpClient.SendAsync(request);
                var text = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine(text);

                JObject results = JObject.Parse(text);
                items = new List<Classes.Items>();
                foreach (var result in results["results"])
                {
                    items.Add(new Classes.Items() { question = (string)result["Question"], trueUnsver = (string)result["trueUnsver"] });
                }

                Gen_Quest();
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.Equals(txtBox.Text, items[randNumb].trueUnsver, StringComparison.CurrentCultureIgnoreCase))
            {
                MessageDialog md1 = new MessageDialog("Это был правильный ответ!");
                await md1.ShowAsync();

                txtBox.Text = "";
                countTrueUnsver++;
                txtScore.Text = countTrueUnsver.ToString();
                Gen_Quest();
            }
            else
            {
                MessageDialog md1 = new MessageDialog("Вы отетили не верно! Правильный ответ: " + items[randNumb].trueUnsver);
                await md1.ShowAsync();

                MessageDialog md = new MessageDialog("Игра окночена! Сохранить результат в рекордах?");
                UICommand cancelBtn = new UICommand("Нет");
                cancelBtn.Invoked = cancelBtn_CLick;
                UICommand yesBtn = new UICommand("Да");
                yesBtn.Invoked = yesBtn_CLick;

                md.Commands.Add(cancelBtn);
                md.Commands.Add(yesBtn);

                await md.ShowAsync();
            }
        }

        private async void yesBtn_CLick(IUICommand command)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            httpClient.BaseAddress = new Uri("https://api.parse.com/1/classes/Record");
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string data = "{\"Name\":\"" + ApplicationData.Current.LocalSettings.Values["UserName"].ToString() + "\",\"Score\":" + countTrueUnsver + ",\"Mode\":\"MyUnsver\"}";

            request.Headers.Add("X-Parse-Application-Id", "q8r9XYoXbEw1kXsWIXbyCNQm9ylcWfAS0fqlWWNx");
            request.Headers.Add("X-Parse-REST-API-Key", "2nke5LSbXQmTiA9FaVytlBihjOOz4fPcoxjEHlee");

            request.Method = new HttpMethod("POST");

            // var response = await httpClient.PostAsync(request, new StringContent(data, System.Text.Encoding.UTF8, "application/json"));
            request.Content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            await httpClient.SendAsync(request);
            //Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            //Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("Records.xml");
            //string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);


            //string newText = text.Replace("</records>", "\n<record>"
            //      + "\n<name>"
            //      + ApplicationData.Current.LocalSettings.Values["UserName"].ToString()
            //      + "</name>"
            //      + "\n<score>"
            //      + countTrueUnsver
            //      + "</score>"
            //      + "\n</record>"
            //      + "\n</records>");


            //StorageFile sampleFile2 = await storageFolder.CreateFileAsync("Records.xml", CreationCollisionOption.OpenIfExists);
            //await Windows.Storage.FileIO.WriteTextAsync(sampleFile2, newText);

            //System.Diagnostics.Debug.WriteLine(newText);

            MessageDialog md = new MessageDialog("Рекорд успешно сохранен!");
            await md.ShowAsync();
            Frame.GoBack();
        }

        private void cancelBtn_CLick(IUICommand command)
        {
            Frame.GoBack();
        }
    }
}
