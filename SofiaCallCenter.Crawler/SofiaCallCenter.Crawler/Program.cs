using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SofiaCallCenter.Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            int signalFrom = 1;
            int signalTo = 2400;

            if (args.Length >= 1)
            {
                signalFrom = int.Parse(args[0]);
            }
            
            if (args.Length >= 2)
            {
                signalTo = int.Parse(args[1]);
            }

            string outputFile = string.Format("CCS_Signals-{0}-{1}.csv",signalFrom, signalTo);
            if (args.Length >= 3)
            {
                outputFile = args[2];
            }
            
            List<Signal> crawledSignals = CrawlSignalsFromCallSofiaBg(signalFrom, signalTo);

            using(StreamWriter sw = new StreamWriter(outputFile)){
                CsvWriter writer = new CsvWriter(sw);
                foreach (var signal in crawledSignals)
                {
                    writer.WriteRecord<Signal>(signal);
                }
            }


            //test
            //int testSignalNumber = 2286;
            //Signal testSignal = CrawlSingleSignalFromCallSofiaBg(testSignalNumber);
        }

        private static List<Signal> CrawlSignalsFromCallSofiaBg(int fromSignalId, int toSignalId)
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

        private static Signal CrawlSingleSignalFromCallSofiaBg(int signalId)
        {
            string signalUrl = string.Format("http://call.sofia.bg/bg/Signal/Details?id={0}", signalId);

            //download page html content
            WebClient webClient = new WebClient();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(webClient.OpenRead(signalUrl), Encoding.UTF8);

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
