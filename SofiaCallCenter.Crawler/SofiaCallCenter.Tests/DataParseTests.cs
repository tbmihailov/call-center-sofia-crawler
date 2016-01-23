using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SofiaCallCenter.Crawler.Helpers;
using HtmlAgilityPack;
using System.IO;
using SofiaCallCenter.Crawler;

namespace SofiaCallCenter.Tests
{
    [TestClass]
    public class DataParseTests
    {
        [TestMethod]
        public void Test_SingleSignal_Parsing()
        {

            string fileName = @"TestDocuments\SofiaCallCenter_Signal_3996.html";
            FileInfo fileInfo = new FileInfo(fileName);
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            TextReader reader = new StreamReader(fileName);
            htmlDoc.Load(reader);

            int signalId = 3996;

            var signal = SofiaCallCenterCrawlerHelpers.ParseSignalFromHtmlDocument(signalId, htmlDoc);

            Assert.AreEqual("Клони на тротоара", signal.Title);
        }
    }
}
