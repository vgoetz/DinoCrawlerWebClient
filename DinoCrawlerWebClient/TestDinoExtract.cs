using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace DinoCrawlerWebClient {
    public class TestDinoExtract {

        private LinkFinder _linkFinder;

        [SetUp]
        public void TestSetup() {
            _linkFinder = new LinkFinder();
        }

        
        [TestCase("<img src='/images/14082015/1-39.png' />", Result = 1)]
        [TestCase("<img src=\"/images/14082015/1-39.png\" />", Result = 1)]
        [TestCase("<img src = '/images/14082015/1-39.png' />", Result = 1)]

        [TestCase("<img src='/images/14082015/1-01.png' />", Result = 1)]
        [TestCase("<img src='/images/14082015/1-11.png' />", Result = 1)]
        [TestCase("<img src='/images/14082015/1-55.png' />", Result = 1)]

        [TestCase("website and look something like this &mdash; <img src='/images/14082015/1-39.png' />" +
                  "<sup><a href='#note2' class='quietLink'>**</a>", Result = 1)]
        [TestCase("website and <img src='/images/14082015/1-3.png' />look something like this &mdash; <img src='/images/14082015/1-39.png' /><sup><a href='#note2' class='quietLink'>**</a>", Result = 1)]
        [TestCase("website and look something like this &mdas<img src='/images/14082015/1-39.jpg' />h; <img src='/images/14082015/1-39.png' /><sup><a href='#note2' class='quietLink'>**</a>", Result = 1)]
        [TestCase("website and look something like this &mdas<img src='/images/14082015/1-39.jpg' />h; <img src='/images/14082015/1-14.png' /><sup><a href='#note2' class='quietLink'>**</a>", Result = 1)]

        [TestCase("</h2>\n<p>\ndbForge Fusion for MySQL automates debugging by providing step-by-step code execution,\nbreakpoints, watches, a call <img src=\"/images/14082015/1-61.png\" style=\"border: 1px dashed #777;\" /> stack, and a variables evaluation mechanism. The tool\ncan help you work with MySQL stored routines and triggers and keeps MySQL server's\nlogic of procedure execution.\n</p>\n<p>", Result = 1)]
        [TestCase("watches, a call <img src=\"/images/14082015/1-61.png\" style=\"border: 1px dashed #777;\" /> stack, and a variables evaluation mechanism. The tool\ncan help you work with MySQL stored routines and triggers and keeps MySQL server's\nlogic of procedure execution.\n</p>\n<p>", Result = 1)]
        [TestCase("watches, a call <img src=\"/images/14082015/1-61.png\" style=\"border: 1px dashed #777;\" /> stack, and a variables evalu", Result = 1)]
        [TestCase("watche<img src=\"/images/14082015/1-61.png\" style=\"border: 1px dashed #777;\" /> ", Result = 1)]
        [TestCase("watche<img src=\"/images/14082015/1-61.png\" /> ", Result = 1)]
        public int TestSearchForDinosAndFindOne(string htmlContent) {
            Console.WriteLine(@"Input: '{0}'", htmlContent);
            var result = _linkFinder.ExtractDinos(htmlContent);
            return (result.Count);
        }


        [TestCase("<img src'/images/14082015/1-39.png' />", Result = 0)]
        [TestCase("<img src=/images/14082015/1-39.png' />", Result = 0)]
        [TestCase("<img src='images/14082015/1-39.png' />", Result = 0)]
        [TestCase("<img src='/mages/14082015/1-39.png' />", Result = 0)]
        [TestCase("<img src='/mages14082015/1-39.png' />",  Result = 0)]
        [TestCase("<img src='/images/14082015/-39.png' />", Result = 0)]
        [TestCase("<img src='/images/14082015/139.png' />", Result = 0)]
        [TestCase("<img src='/images/14082015/1-9.png' />", Result = 0)]
        [TestCase("<img src='/images/14082015/1-39png' />", Result = 0)]
        [TestCase("<img src='/images/14082015/1-39.pn' />", Result = 0)]
        [TestCase("<img src='/images/14082015/1-39.png />", Result = 0)]

        [TestCase("<img src='/images/14082015/1-1.png' />", Result =   0)]
        [TestCase("<img src='/images/14082015/1-111.png' />", Result = 0)]
        [TestCase("<img src='/images/1408201/1-39.png' />", Result =   0)]
        [TestCase("<img src='/images/14082016/1-39.png' />", Result =  0)]
        public int TestSearchForDinosAndFindNothing(string htmlContent) {
            Console.WriteLine(@"Input: '{0}'", htmlContent);
            var result = _linkFinder.ExtractDinos(htmlContent);
            return (result.Count);
        }
    }
}