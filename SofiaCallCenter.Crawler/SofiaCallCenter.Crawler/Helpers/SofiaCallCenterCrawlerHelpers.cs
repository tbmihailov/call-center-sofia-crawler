using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SofiaCallCenter.Crawler
{
    public class SofiaCallCenterCrawlerHelpers
    {
        public static List<Signal> CrawlSignalsFromCallSofiaBg(int fromSignalId, int toSignalId, int timeBetweenHttpRequestsInMiliseconds)
        {
            List<Signal> signals = new List<Signal>();
            for (int signalId = fromSignalId; signalId <= toSignalId; signalId++)
            {
                try
                {
                    Console.Write("Downloading signal [{0}]...", signalId);
                    Signal signal = CrawlSingleSignalFromCallSofiaBg(signalId);
                    signals.Add(signal);
                    Console.WriteLine("DONE!");
                    Thread.Sleep(timeBetweenHttpRequestsInMiliseconds);
                }
                catch (SignalHtmlStructureNotDetectedException se)
                {
                    Console.WriteLine("NOT FOUND!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("FAILED! - Exception :" + e.Message);
                }
            }

            return signals;
        }

        public static Signal CrawlSingleSignalFromCallSofiaBg(int signalId)
        {
            string signalUrl = string.Format("http://call.sofia.bg/bg/Signal/Details?id={0}", signalId);

            //download page html content
            WebClient webClient = new WebClient();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(webClient.OpenRead(signalUrl), Encoding.UTF8);

            Signal signal = ParseSignalFromHtmlDocument(signalId, doc);

            return signal;
        }

        public static Signal ParseSignalFromHtmlDocument(int signalId, HtmlAgilityPack.HtmlDocument doc)
        {
            if ((doc.DocumentNode.SelectNodes("//p[@id='statusIndicator']") == null)
                            || (doc.DocumentNode.SelectNodes("//p[@id='statusIndicator']").Count == 0))
            {
                throw new SignalHtmlStructureNotDetectedException(signalId);
            }

            //get sections from html
            var docNodes = doc.DocumentNode.SelectNodes("//section[@class]");

            //get number and category
            var nodeSectionWithCategory = docNodes.FirstOrDefault();
            var signalStatus = nodeSectionWithCategory.SelectSingleNode("//p[@id='statusIndicator']").InnerText;
            var signalDocNumberInfo = nodeSectionWithCategory.SelectSingleNode("//h3").InnerText;
            var signalCategoryFull = nodeSectionWithCategory.SelectSingleNode("//h4").InnerText;

            //get content
            var nodeSectionContent = docNodes[1];
            var signalTitle = nodeSectionWithCategory.SelectSingleNode("//div/blockquote").InnerText;
            var signalDescription = nodeSectionWithCategory.SelectSingleNode("//fieldset/div[@class='display-field']").InnerText;

            Signal signal = new Signal();
            signal.Id = signalId;
            signal.DocNumberInfo = signalDocNumberInfo;
            signal.Title = signalTitle;
            signal.Description = signalDescription;
            signal.Status = signalStatus;
            signal.CategoryFull = signalCategoryFull;

            string[] categoryLevels = signalCategoryFull.Split('/');
            signal.CategoryLevel1 = categoryLevels[0];
            signal.CategoryLevel2 = categoryLevels.Length > 1 ? categoryLevels[1] : string.Empty;
            return signal;
        }
    }
}
