using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

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

            _cancel = false;
            btnStop.IsEnabled = true;

            lsbRelevantLinks.Items.Clear();
            lsbAllLinks.Items.Clear();
            lsbVisitedSites.Items.Clear();
            lsbFoundDinos.Items.Clear();

            lsbRelevantLinks.Items.Add(txtUri.Text);

            while (lsbRelevantLinks.Items.Count > 0 && !_cancel) {
                await RunCrawler(lsbRelevantLinks.Items.GetItemAt(0).ToString());
            }

            _cancel = false;
            btnStop.IsEnabled = false;
        }

        private async Task RunCrawler(string uriAsString) {
            var uri = new Uri(uriAsString);

            lsbRelevantLinks.Items.RemoveAt(0);

            string htmlResult;
            try {
                Console.WriteLine("Try '{0}'", uriAsString);
                htmlResult = await _webClient.DownloadStringTaskAsync(uri);
            } catch (WebException webEx) {
                Console.WriteLine("Can´t crawl '{0}' because of {1}", uriAsString, webEx.Message);
                return;
            }

            _visitedSites.Add(uri.ToString());
            lsbVisitedSites.Items.Add(uri.ToString());
            lblVisitedSitesCounter.Content = lsbVisitedSites.Items.Count;
            txtHtmlResult.Text = htmlResult;

            IList<string> allLinks = _linkFinder.GetAllLinks(htmlResult);
            lsbAllLinks.Items.Clear();
            foreach (string linkText in allLinks) {
                lsbAllLinks.Items.Add(linkText);
            }
            lblAllLinksCounter.Content = lsbAllLinks.Items.Count;

            IList<string> filterdLinks = _linkFinder.GetFilteredLinksToVisit(allLinks.ToList(), _visitedSites, uriAsString);
            foreach (string filterdLink in filterdLinks) {
                lsbRelevantLinks.Items.Add(filterdLink);
            }
            lblRelevantLinksCounter.Content = lsbRelevantLinks.Items.Count;

            IList<string> foundDinos = _linkFinder.ExtractDinos(uri, allLinks);
            foreach (string foundDino in foundDinos) {
                lsbFoundDinos.Items.Add(foundDino);
            }
            lblFoundDinosCounter.Content = lsbFoundDinos.Items.Count;
        }

        private void btnSetDevart_Click(object sender, RoutedEventArgs e) {
            txtUri.Text = "http://www.devart.com/";
        }

        private void btnSetStammtisch_Click(object sender, RoutedEventArgs e) {
            txtUri.Text = "http://stammtisch.azurewebsites.net/";
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
