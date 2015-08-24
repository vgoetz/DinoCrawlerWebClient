using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace DinoCrawlerWebClient {
    public class LinkFilterTests {

        private LinkFinder _linkFinder;

        [SetUp]
        public void TestSetup() {
            _linkFinder = new LinkFinder();
        }

        [TestCase("http://www.devart.com/", "http://www.devart.com/huhu", "http://www.devart.com/haha", Result = 2)]
        [TestCase("http://www.devart.com/", "http://www.devart.com/huhu", "http://www.de.de/bliblu.html", Result = 1)]
        public int TestLinkFilter_AcceptOnlyRootUri(string rootLink, string firstLink, string secondLink) {
            Console.WriteLine(@"Input: '{0}' '{1}'", firstLink, secondLink);
            IList<string> allLinks = new List<string> { firstLink, secondLink };
            var filterdLinks = _linkFinder.GetFilteredLinksToVisit(allLinks, new List<string>(), rootLink);
            return (filterdLinks.Count);
        }

        [TestCase("http://www.de.de/bla.html", "http://www.de.de/bla.html", Result = 1)]
        [TestCase("http://www.de.de/bla.html", "http://www.de.de/bliblu.html", Result = 2)]
        public int TestLinkFilter_DropDuplicates(string firstLink, string secondLink) {
            Console.WriteLine(@"Input: '{0}' '{1}'", firstLink, secondLink);
            IList<string> allLinks = new List<string> { firstLink, secondLink };
            var filterdLinks = _linkFinder.GetFilteredLinksToVisit(allLinks, new List<string>(), firstLink, false);
            return (filterdLinks.Count);
        }

        [TestCase("http://www.de.de", Result = 1)]
        [TestCase("http://www.de.de/bla", Result = 1)]
        [TestCase("http://www.de.de/bla/", Result = 1)]
        [TestCase("http://www.de.de/bla.htm", Result = 1)]
        [TestCase("http://www.de.de/bla.html", Result = 1)]
        [TestCase("http://www.de.de/bla.php", Result = 1)]
        public int TestLinkFilter_KeepRelevantLinks(string htmlContent) {
            Console.WriteLine(@"Input: '{0}'", htmlContent);
            IList<string> allLinks = _linkFinder.GetAllLinks(htmlContent);
            var filterdLinks = _linkFinder.GetFilteredLinksToVisit(allLinks, new List<string>(), htmlContent, false);
            return (filterdLinks.Count);
        }

        [TestCase("http://www.de.de/bla.zip", Result = 0)]
        [TestCase("http://www.de.de/bla.exe", Result = 0)]
        public int TestLinkFilter_IgnoreFiles(string htmlContent) {
            Console.WriteLine(@"Input: '{0}'", htmlContent);
            IList<string> allLinks = _linkFinder.GetAllLinks(htmlContent);
            var filterdLinks = _linkFinder.GetFilteredLinksToVisit(allLinks, new List<string>(), htmlContent, false);
            return (filterdLinks.Count);
        }

        [TestCase("http://www.de.de", "http://www.de.de", Result = 0)]
        [TestCase("http://www.de.xy", "http://www.de.de", Result = 1)]
        public int TestLinkFilter_IgnoreAlreadyVisitedSites(string htmlContent, string visitedSite) {
            Console.WriteLine(@"Input: '{0}'", htmlContent);
            Console.WriteLine(@"Visited site: '{0}'", visitedSite);
            IList<string> allLinks = _linkFinder.GetAllLinks(htmlContent);
            var filterdLinks = _linkFinder.GetFilteredLinksToVisit(allLinks, new List<string> { visitedSite }, htmlContent, false);
            return (filterdLinks.Count);
        }
    }
}
