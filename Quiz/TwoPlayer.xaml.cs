using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class TwoPlayer : Page
    {
        List<Classes.Items> items;
        int randNumb;
        int countTrueUnsverP1 = 0;
        int countTrueUnsverP2 = 0;
        bool P1Unsverd;
        bool P2Unsverd;
        int touch = 0;
        string category;

        public TwoPlayer()
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

            txtQuestionP1.Text = items[randNumb].question;
            bttUnsver1P1Text.Text = items[randNumb].unsver1;
            bttUnsver2P1Text.Text = items[randNumb].unsver2;
            bttUnsver3P1Text.Text = items[randNumb].unsver3;
            bttUnsver4P1Text.Text = items[randNumb].unsver4;

            txtQuestionP2.Text = items[randNumb].question;
            bttUnsver1P2Text.Text = items[randNumb].unsver1;
            bttUnsver2P2Text.Text = items[randNumb].unsver2;
            bttUnsver3P2Text.Text = items[randNumb].unsver3;
            bttUnsver4P2Text.Text = items[randNumb].unsver4;
        }

        private void btnUnsverP1(object sender, RoutedEventArgs e)
        {

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
                    items.Add(new Classes.Items() { question = (string)result["Question"], unsver1 = (string)result["unsver1"], unsver2 = (string)result["unsver2"], unsver3 = (string)result["unsver3"], unsver4 = (string)result["unsver4"], trueUnsver = (string)result["trueUnsver"] });
                    //item.Add(new Item() { driveId = (string)result["parentReference"]["driveId"], id = (string)result["parentReference"]["id"], path = (string)result["parentReference"]["path"] });
                }

                Gen_Quest();
            }
        }

        private async void NextQuest()
        {
            touch = 0;

            if (P1Unsverd == false && P2Unsverd == false)
            {
                MessageDialog md = new MessageDialog("Игра окончена! Никто не ответил верно! Правильный ответ: " + items[randNumb].trueUnsver + Environment.NewLine + "Результаты игры:" + Environment.NewLine + "Игрок 1: " + countTrueUnsverP1 + Environment.NewLine + "Игрок 2: " + countTrueUnsverP2);
                await md.ShowAsync();
                Frame.GoBack();
            }

            P1Unsverd = false;
            P2Unsverd = false;

            bttUnsver1P1.IsEnabled = true;
            bttUnsver2P1.IsEnabled = true;
            bttUnsver3P1.IsEnabled = true;
            bttUnsver4P1.IsEnabled = true;

            bttUnsver1P2.IsEnabled = true;
            bttUnsver2P2.IsEnabled = true;
            bttUnsver3P2.IsEnabled = true;
            bttUnsver4P2.IsEnabled = true;

            txtScoreP1.Text = countTrueUnsverP1.ToString();
            txtScoreP2.Text = countTrueUnsverP2.ToString();

            Gen_Quest();
        }

        //private void btnUnsverP1Text_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    string unsver = (sender as TextBlock).Text.ToString();

        //    if (unsver == items[randNumb].trueUnsver)
        //    {
        //        countTrueUnsverP1++;
        //        P1Unsverd = true;
        //    }

        //    bttUnsver1P1.IsEnabled = false;
        //    bttUnsver2P1.IsEnabled = false;
        //    bttUnsver3P1.IsEnabled = false;
        //    bttUnsver4P1.IsEnabled = false;

        //    touch++;

        //    if (touch == 2)
        //    {
        //        NextQuest();
        //    }
        //}

        private void btnUnsverP2Text_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string unsver = (sender as TextBlock).Text.ToString();

            if (unsver == items[randNumb].trueUnsver)
            {
                countTrueUnsverP2++;
                P2Unsverd = true;
            }

            bttUnsver1P2.IsEnabled = false;
            bttUnsver2P2.IsEnabled = false;
            bttUnsver3P2.IsEnabled = false;
            bttUnsver4P2.IsEnabled = false;

            touch++;

            if (touch == 2)
            {
                NextQuest();
            }
        }

        //private void btnUnsverP1_Click(object sender, RoutedEventArgs e)
        //{
        //    string unsver = (sender as Button).Content.ToString();

        //    if (unsver == items[randNumb].trueUnsver)
        //    {
        //        countTrueUnsverP1++;
        //        P2Unsverd = true;
        //    }

        //    bttUnsver1P1.IsEnabled = false;
        //    bttUnsver2P1.IsEnabled = false;
        //    bttUnsver3P1.IsEnabled = false;
        //    bttUnsver4P1.IsEnabled = false;

        //    touch++;

        //    if (touch == 2)
        //    {
        //        NextQuest();
        //    }
        //}

        private void btnUnsverP1Text_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string unsver = (sender as TextBlock).Text.ToString();
            if (unsver == items[randNumb].trueUnsver)
            {
                countTrueUnsverP1++;
                P1Unsverd = true;
            }

            bttUnsver1P1.IsEnabled = false;
            bttUnsver2P1.IsEnabled = false;
            bttUnsver3P1.IsEnabled = false;
            bttUnsver4P1.IsEnabled = false;

            touch++;

            if (touch == 2)
            {
                NextQuest();
            }
        }
    }
}
