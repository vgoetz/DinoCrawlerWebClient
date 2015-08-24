using System;
using NUnit.Framework;

namespace DinoCrawlerWebClient {
    public class LinkExtractTests {

        private LinkFinder _linkFinder;

        [SetUp]
        public void TestSetup() {
            _linkFinder = new LinkFinder();
        }

        [TestCase("", Result = 0)]
        [TestCase("aaaa", Result = 0)]
        [TestCase("aa aa", Result = 0)]
        [TestCase("abcdefghijklmonpq", Result = 0)]
        [TestCase("34lk5h3k2t98uedfkjn32iuh4", Result = 0)]
        [TestCase("www", Result = 0)]
        [TestCase("@@@weewwebhh", Result = 0)]
        [TestCase("http", Result = 0)]
        [TestCase("http:", Result = 0)]
        [TestCase("http:/", Result = 0)]
        [TestCase("http://a", Result = 0)]
        [TestCase("http://a.", Result = 0)]
        [TestCase("http://ww.", Result = 0)]
        [TestCase("http://www.", Result = 0)]
        [TestCase("http://www..", Result = 0)]
        [TestCase("http://www. .", Result = 0)]
        [TestCase("http://www ..", Result = 0)]
        [TestCase("http:// www.d.", Result = 0)]
        [TestCase("http: // www.d.", Result = 0)]
        [TestCase("http : // www.d.", Result = 0)]
        [TestCase("htt p : // www.d.", Result = 0)]
        [TestCase("htt p :// www.d.", Result = 0)]
        [TestCase("http: // www.d.", Result = 0)]
        [TestCase("http:/ / www.d.", Result = 0)]
        [TestCase("http://www. d.", Result = 0)]
        [TestCase("http://www. de", Result = 0)]
        [TestCase("http://www. de.d", Result = 0)]
        [TestCase("http://www. de.de", Result = 0)]
        public int TestSearchForLinksWithoutMatches(string htmlContent) {
            Console.WriteLine(@"Input: '{0}'", htmlContent);
            var result = _linkFinder.GetAllLinks(htmlContent);
            return (result.Count);
        }

        [TestCase("http://www.de", Result = 1)]
        [TestCase("http://www.de.", Result = 1)]
        [TestCase("http://www.de.d", Result = 1)]
        [TestCase("http://www.de.de", Result = 1)]
        [TestCase("http://www.de.de/", Result = 1)]
        [TestCase("http://www.de.de/b", Result = 1)]
        [TestCase("http://www.de.de/bla", Result = 1)]
        [TestCase("http://www.de.de/bla/", Result = 1)]
        [TestCase("http://www.de.de/bla/www", Result = 1)]
        [TestCase("http://www.de.de/bla/www.", Result = 1)]
        [TestCase("http://www.de.de/bla/www.de", Result = 1)]
        [TestCase("http://www.de.de/bla/www.dehttp", Result = 1)]
        [TestCase("http://www.de.de/bla/www.dehttp.de", Result = 1)]
        [TestCase("http://www.de.de/bla/www.dehttp.de.com", Result = 1)]
        [TestCase("http://www.de.de/bla/www.dehttp://", Result = 1)]
        [TestCase("http://www.de.de/bla/www.dehttp://www", Result = 1)]
        [TestCase("http://www.de.de/bla/www.dehttp://www.", Result = 1)]
        [TestCase("a http://www.de.de/bla/www.dehttp://www.de b", Result = 1)]
        [TestCase("a http://www.de.de/bla/www.dehttp://www.de bcd", Result = 1)]
        [TestCase("123 www .de a http://www.de.de/bla/www.dehttp://www.de bcd <>", Result = 1)]
        public int TestSearchForLinksWith1Match(string htmlContent) {
            Console.WriteLine(@"Input: '{0}'", htmlContent);
            var result = _linkFinder.GetAllLinks(htmlContent);
            return (result.Count);
        }

        [TestCase("a http://w\nww.de.de/bla/www.dehttp://www.de bcd", Result = 1)]
        [TestCase("a http://w\n\nww.de.de/bla/www.dehttp://www.de bcd", Result = 1)]
        [TestCase("a http://w\n\rww.de.de/bla/www.dehttp://www.de bcd", Result = 1)]
        [TestCase("a http://w\r\nww.de.de/bla/www.dehttp://www.de bcd", Result = 1)]
        [TestCase("a http://w\r\rww.de.de/bla/www.dehttp://www.de bcd", Result = 1)]
        [TestCase("123 www .de a http://www.de.de/bla/www.dehttp://www.de bcd <>", Result = 1)]
        public int TestSearchHavingLinebreaksForLinksWith1Match(string htmlContent) {
            Console.WriteLine(@"Input: '{0}'", htmlContent);
            var result = _linkFinder.GetAllLinks(htmlContent);
            return (result.Count);
        }

        [TestCase("http://www.de.de/bla http://www.de", Result = 2)]
        [TestCase("http://www.de.de/bla/ http://www.de", Result = 2)]
        [TestCase("http://www.de.de/bla/ http://www.de.de", Result = 2)]
        [TestCase(" http://www.de.de/bla/ http://www.de.de ", Result = 2)]
        [TestCase("aaa http://www.de.de/bla/ http://www.de.de ", Result = 2)]
        [TestCase("aaa http://www.de.de/bla/ asdf asdfasd http://www.de.de ", Result = 2)]
        [TestCase("aaa http://www.de.de/bla/ asdf asdfasd asdfasdf334534 http://www.de.de ", Result = 2)]
        [TestCase("aaa http://www.de.de/bla/ asdf asdfasd asdfasdf334534 http://www.de.de ", Result = 2)]
        [TestCase("aaa http://www.de.de/bla/ asdf asdfasd // asdfasdf334534 http://www.de.de ", Result = 2)]
        [TestCase("aaa http://www.de.de/bla/ asdf asdfasd :// asdfasdf334534 http://www.de.de ", Result = 2)]
        [TestCase("aaa http://www.de.de/bla/ asdf asdfasd ://: http asdfasdf334534 http://www.de.de ", Result = 2)]
        [TestCase("aaa http://www.de.de/bla/ asdf asdfasd \n http asdfasdf334534 http://www.de.de ", Result = 2)]
        [TestCase("aaa http://www.de.de/bla/ asdf asdfasd \n\r http asdfasdf334534 http://www.de.de ", Result = 2)]
        [TestCase("aaa http://www.de.de/bla/ asdf asdfasd  http asdfasdf334534 http:\n//www.de.de ", Result = 2)]
        [TestCase("aaa http://www.de.de/bla/ asdf asdfasd  http asdfasdf334534 http:\n\r//www.de.de ", Result = 2)]
        [TestCase("aaa http://www.de.de/bla/ asdf asdfasd  http asdfasdf334534 http:\r//www.de.de ", Result = 2)]
        public int TestSearchForLinksWith2Matches(string htmlContent) {
            Console.WriteLine(@"Input: '{0}'", htmlContent);
            var result = _linkFinder.GetAllLinks(htmlContent);
            return (result.Count);
        }
    }
}