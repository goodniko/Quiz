using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class Records : Page
    {
        List<Classes.Record> com;
        List<Classes.Record> sortMas;

        public Records()
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

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage();
                    httpClient.BaseAddress = new Uri("https://api.parse.com/1/classes/Record");
                    request.Method = new HttpMethod("GET");
                    request.Headers.Add("X-Parse-Application-Id", "q8r9XYoXbEw1kXsWIXbyCNQm9ylcWfAS0fqlWWNx");
                    request.Headers.Add("X-Parse-REST-API-Key", "2nke5LSbXQmTiA9FaVytlBihjOOz4fPcoxjEHlee");

                    var response = await httpClient.SendAsync(request);
                    var text = await response.Content.ReadAsStringAsync();

                    System.Diagnostics.Debug.WriteLine(text);

                    JObject results = JObject.Parse(text);
                    com = new List<Classes.Record>();
                    foreach (var result in results["results"])
                    {
                        com.Add(new Classes.Record() { name = (string)result["Name"], score = (string)result["Score"], mode = (string)result["Mode"], date = (string)result["createdAt"] });
                    }

                    System.Diagnostics.Debug.WriteLine(com[0].date);
                    System.Diagnostics.Debug.WriteLine(com[0].name);


                }
                await System.Threading.Tasks.Task.Delay(2500);
                sortMas = com.OrderByDescending(a => Convert.ToInt32(a.score)).ToList();

                string date;
                if (DateTime.Now.Month < 10)
                    date = "0" + DateTime.Now.Month.ToString();
                else
                    date = DateTime.Now.Month.ToString();

                listBox.ItemsSource = sortMas;
                listBoxChooseUnsver.ItemsSource = sortMas.OrderByDescending(a=>Convert.ToInt32(a.score)).Where(a => a.mode == "4btn");
                listBoxMyUnsver.ItemsSource = sortMas.OrderByDescending(a=>Convert.ToInt32(a.score)).Where(a => a.mode == "MyUnsver");
                listBoxBestInMounth.ItemsSource = sortMas.OrderByDescending(a=>Convert.ToInt32(a.score)).Where(a => a.date.Split('/')[0] == date);
            }
            catch
            {
                MessageDialog dm = new MessageDialog("Пока что нет рекордов!");
            }
        }
    }
}
