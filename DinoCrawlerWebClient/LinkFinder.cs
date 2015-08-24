using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace DinoCrawlerWebClient {

    public class LinkFinder {

        private static readonly Regex LinkParser = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", 
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex DinoHtmlImgSrcParser = new Regex(@"<img\s*src\s*=\s*\S/images/14082015/1-\d{2}.png\S\s*/>", 
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex DinoImgSrcParser = new Regex(@"/images/14082015/1-\d{2}.png",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly IList<string> LinkTypesToIgnoreForVisit = new List<string> {
            ".zip", ".txt", ".exe", ".rdf", ".xml", ".js", ".ico", ".png", ".jpg", ".jpeg", ".bmp",
            ".7z", ".7zip", ".css"

        };

        public IList<string> GetAllLinks(string htmlContent) {
            var tokens = htmlContent
                .Trim()
                .Replace("\r", "")
                .Replace("\n", "")
                .Split(' ');

            List<string> list = new List<string>();

            foreach (string token in tokens) {
                foreach (Match link in LinkParser.Matches(token)) {
                    list.Add(link.Value);
                }
            }
            
            return list;
        }

        public IList<string> GetFilteredLinksToVisit(IList<string> linksToFilter, IList<string> visitedLinks, string rootUri, bool onlyRootLinks = true) {
            foreach (string visitedLink in visitedLinks) {
                linksToFilter.Remove(visitedLink);
            }

            var rootUriWithoutHttpPrefix = rootUri.Replace("http://www", "");
            rootUriWithoutHttpPrefix = rootUriWithoutHttpPrefix.Replace("https://www", "");

            IList<string> listToRemove = new List<string>();
            foreach (string link in linksToFilter) {
                if (onlyRootLinks) {
                    if (!link.Contains(rootUriWithoutHttpPrefix)) {
                        listToRemove.Add(link);
                    }
                }

                foreach (string ignoreMask in LinkTypesToIgnoreForVisit) {
                    if (!listToRemove.Contains(link) &&
                        link.EndsWith(ignoreMask)) {
                        listToRemove.Add(link);
                    }
                }
            }

            foreach (string s in listToRemove) {
                linksToFilter.Remove(s);
            }

            return linksToFilter.Distinct().ToList();
        }


        public IList<string> ExtractDinos(string htmlContent) {
            htmlContent = htmlContent
                .Trim()
                .Replace("\r", "")
                .Replace("\n", "");

            List<string> list = new List<string>();
            
            foreach (Match dinoMatch in DinoHtmlImgSrcParser.Matches(htmlContent)) {
                list.Add(dinoMatch.Value);
            }

            return list;
        }

        public Uri GetDinoUriFromHtmlImgSrc(string htmlImgSrc) {
            var uriToBuild = "https://www.devart.com" + DinoImgSrcParser.Match(htmlImgSrc).Value;

            try {
                return new Uri(uriToBuild);
            } catch (Exception e) {
                Console.WriteLine("Can´t convert '{0}' to a valid Uri: {1}", uriToBuild, e.Message);
                return null;
            }
            
        }
    }
}
