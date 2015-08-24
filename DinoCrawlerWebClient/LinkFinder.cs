using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DinoCrawlerWebClient {

    public class LinkFinder {

        private static readonly string DinoImageFolderAndPrefixPattern = "/images/14082015/1-";
        private static readonly string DinoImageTypePattern = ".png";

        private static readonly Regex LinkParser = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", 
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly IList<string> LinkTypesToIgnoreForVisit = new List<string> {
            ".zip", ".txt", ".exe", ".rdf", ".xml", ".js", ".ico", ".png", ".jpg", ".jpeg", ".bmp", ".7z", ".7zip"
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
            rootUriWithoutHttpPrefix.Replace("https://www", "");

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

        public IList<string> ExtractDinos(Uri originalUri, IList<string> links) {
            var foundDinos = new List<string>();

            foreach (string link in links) {
                if (link.EndsWith(DinoImageTypePattern) &&
                    link.Contains(DinoImageFolderAndPrefixPattern) &&
                    link.Length >= 8) {

                    string numberOfImageFileAsString = 
                        link.Substring(link.Length - (DinoImageTypePattern.Length + 2), 2);

                    int numberOfImageFile;
                    int.TryParse(numberOfImageFileAsString, out numberOfImageFile);

                    string dinoImageFolderAndPrefixSubstring = 
                        link.Substring(link.Length - (DinoImageTypePattern.Length + 2 + DinoImageFolderAndPrefixPattern.Length), DinoImageFolderAndPrefixPattern.Length);

                    if (numberOfImageFile > 0 &&
                        numberOfImageFile < 100 &&
                        dinoImageFolderAndPrefixSubstring == DinoImageFolderAndPrefixPattern) {
                        
                        foundDinos.Add(link);
                        Console.WriteLine("YEHAAAA!!! Dino found: {0}", link);
                    }
                }
            }

            return foundDinos;
        }
    }
}
