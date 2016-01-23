using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofiaCallCenter.Crawler.Helpers
{
    public class SignalsCsvHelper
    {
        public static void WriteSignalsToCsvFile(string outputFile, List<Signal> crawledSignals)
        {
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                CsvWriter writer = new CsvWriter(sw,
                       new CsvConfiguration()
                       {
                           Encoding = new UTF8Encoding(),
                           Delimiter = ",",
                           //QuoteNoFields = true,
                           HasHeaderRecord = false
                       });
                foreach (var signal in crawledSignals)
                {
                    writer.WriteRecord<Signal>(signal);
                }
            }
        }

        public static List<Signal> ReadSignalsFromCsvFile(string outputFile)
        {
            List<Signal> signals = new List<Signal>();
            using (StreamReader sw = new StreamReader(outputFile))
            {
                CsvReader reader = new CsvReader(sw,
                       new CsvConfiguration()
                       {
                           Encoding = new UTF8Encoding(),
                           Delimiter = ",",
                           //QuoteNoFields = true,
                           HasHeaderRecord = false
                       });
                var loadedSignals = reader.GetRecords<Signal>();
                signals = new List<Signal>(loadedSignals);
            }

            return signals;
        }
    }
}
