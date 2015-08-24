using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace DinoCrawlerWebClient {
    public class DinoExtractTests {

        private LinkFinder _linkFinder;

        [SetUp]
        public void TestSetup() {
            _linkFinder = new LinkFinder();
        }

        
        [TestCase("http://www.devart.com/images/14082015/1-39.png", Result = 1)]
        [TestCase("https://www.devart.com/images/14082015/1-39.png", Result = 1)]
        [TestCase("https://www.devart.com/images/14082015/1-11.png", Result = 1)]
        public int TestSearchForDinosAndFindOne(string htmlContent) {
            Console.WriteLine(@"Input: '{0}'", htmlContent);
            var result = _linkFinder.ExtractDinos(new Uri("http://www.devart.com/"), new List<string> {htmlContent});
            return (result.Count);
        }

        [TestCase("http://www.devart.com/images/14082015/-39.png", Result = 0)]
        [TestCase("https://www.devart.com/images/14082015/139.png", Result = 0)]
        [TestCase("https://www.devart.com/images/14082015/1-1.png", Result = 0)]
        [TestCase("https://www.devart.com/images/14082015/1-11png", Result = 0)]
        [TestCase("https://www.devart.com/images/14082015/1-11.ppng", Result = 0)]
        [TestCase("https://www.devart.com/images/14082015/1-11.pg", Result = 0)]
        [TestCase("https://www.devart.com/images/14082015/1-11.jpg", Result = 0)]
        [TestCase("https://www.devart.com/images/14082016/1-11.jpg", Result = 0)]
        [TestCase("https://www.devart.com/images/1408201/1-11.jpg", Result = 0)]
        [TestCase("https://www.devart.com/images14082015/1-11.jpg", Result = 0)]
        [TestCase("https://www.devart.com/image/14082015/1-11.jpg", Result = 0)]
        [TestCase("https://www.devart.com/image/114082015/1-11.jpg", Result = 0)]
        public int TestSearchForDinosAndFindNothing(string htmlContent) {
            Console.WriteLine(@"Input: '{0}'", htmlContent);
            var result = _linkFinder.ExtractDinos(new Uri("http://www.devart.com/"), new List<string> { htmlContent });
            return (result.Count);
        }
    }
}