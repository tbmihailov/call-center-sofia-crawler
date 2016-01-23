using CsvHelper;
using SofiaCallCenter.Crawler.Helpers;
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

            List<Signal> crawledSignals = SofiaCallCenterCrawlerHelpers.CrawlSignalsFromCallSofiaBg(signalFrom, signalTo, 10);

            SignalsCsvHelper.WriteSignalsToCsvFile(outputFile, crawledSignals);
        }

      
    }
}
