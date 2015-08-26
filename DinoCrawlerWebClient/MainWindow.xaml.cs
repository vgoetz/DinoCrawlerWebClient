using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DinoCrawlerWebClient {
    
    public partial class MainWindow {

        private readonly WebClient _webClient = new WebClient();
        private readonly LinkFinder _linkFinder = new LinkFinder();
        private readonly IList<string> _visitedSites = new List<string>();
        private bool _cancel;

        public MainWindow() {
            InitializeComponent();
        }

        private async void vtnCrawl_Click(object sender, RoutedEventArgs e) {

            _visitedSites.Clear();
            _cancel = false;
            btnStop.IsEnabled = true;

            lsbRelevantLinks.Items.Clear();
            lsbAllLinks.Items.Clear();
            lsbVisitedSites.Items.Clear();
            lsbFoundDinos.Items.Clear();

            lsbRelevantLinks.Items.Add(txtUri.Text);

            while (lsbRelevantLinks.Items.Count > 0 && !_cancel) {
                var nextUri = lsbRelevantLinks.Items.GetItemAt(0).ToString();
                await RunCrawler(nextUri);
            }

            _cancel = false;
            btnStop.IsEnabled = false;
        }

        private async Task RunCrawler(string uriAsString) {
            lsbRelevantLinks.Items.RemoveAt(0);

            Uri uri;
            try {
                uriAsString = uriAsString.Replace("http:www.", "http://www.");
                uriAsString = uriAsString.Replace("https:www.", "https://www.");
                uri = new Uri(uriAsString);
            } catch (Exception e) {
                Console.WriteLine("Can´t convert '{0}' to a valid Uri: {1}", uriAsString, e.Message);
                return;
            }

            

            if (_visitedSites.Contains(uriAsString)) {
                return;
            }

            string htmlResult;
            try {
                Console.WriteLine("Try '{0}'", uriAsString);
                htmlResult = await _webClient.DownloadStringTaskAsync(uri);
            } catch (WebException webEx) {
                Console.WriteLine("Can´t crawl '{0}' because of {1}", uriAsString, webEx.Message);
                return;
            } catch (Exception) {
                return;
            }

            _visitedSites.Add(uri.ToString());
            lsbVisitedSites.Items.Add(uri.ToString());
            lblVisitedSitesCounter.Content = lsbVisitedSites.Items.Count;
            txtHtmlResult.Text = htmlResult;

            IList<string> allLinks = _linkFinder.GetAllLinks(htmlResult, uri.AbsoluteUri.Replace(uri.PathAndQuery, ""));
            lsbAllLinks.Items.Clear();
            foreach (string linkText in allLinks) {
                lsbAllLinks.Items.Add(linkText);
            }
            lblAllLinksCounter.Content = lsbAllLinks.Items.Count;

            IList<string> filterdLinks = _linkFinder.GetFilteredLinksToVisit(allLinks.ToList(), _visitedSites);
            foreach (string filterdLink in filterdLinks) {
                lsbRelevantLinks.Items.Add(filterdLink);
            }
            lblRelevantLinksCounter.Content = lsbRelevantLinks.Items.Count;

            IList<string> foundDinos = _linkFinder.ExtractDinos(htmlResult);
            foreach (string foundDino in foundDinos) {
                if (lsbFoundDinos.Items.Contains(foundDino)) {
                    continue;
                }

                lsbFoundDinos.Items.Add(foundDino);

                try {
                    var imageUri = _linkFinder.GetDinoUriFromHtmlImgSrc(foundDino);
                    imgDinoPreview.Source = new BitmapImage(imageUri);
                } catch (Exception e) {
                    Console.WriteLine("Can´t create BitmapImage from src-string'{0}': {1}", foundDino, e.Message);
                    return;
                }
            }
            lblFoundDinosCounter.Content = lsbFoundDinos.Items.Count;
        }

        private void btnSetDevart_Click(object sender, RoutedEventArgs e) {
            txtUri.Text = "http://www.devart.com/";
        }

        private void btnSetStammtisch_Click(object sender, RoutedEventArgs e) {
            txtUri.Text = "http://stammtisch.azurewebsites.net";
        }

        private void button_Click(object sender, RoutedEventArgs e) {
            txtUri.Text = "https://www.devart.com/news/2015/dino-hunt.html";
        }

        private void btnSetHeise_Click(object sender, RoutedEventArgs e) {
            txtUri.Text = "http://www.heise.de/";
        }



        private void btnStop_Click(object sender, RoutedEventArgs e) {
            _cancel = true;
            btnStop.IsEnabled = false;
        }


    }
}
