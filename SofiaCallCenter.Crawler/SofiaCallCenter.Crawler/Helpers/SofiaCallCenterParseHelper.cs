using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SofiaCallCenter.Crawler.Helpers
{
    public class SofiaCallCenterParseHelper
    {
        public static Signal ParseSingleSignalData(int signalId, HtmlAgilityPack.HtmlDocument doc)
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

            var signalLocationText = doc.DocumentNode.SelectSingleNode("//fieldset//label[@for='LOCATION']").ParentNode.ParentNode.InnerText;
            decimal lat = 0;
            decimal lon = 0;
            ExtractLatLon(signalLocationText, ref lat, ref lon);

            Signal signal = new Signal();
            signal.Id = signalId;
            signal.DocNumberInfo = signalDocNumberInfo;
            signal.Title = signalTitle;
            signal.Description = signalDescription;
            signal.Status = signalStatus;
            signal.CategoryFull = signalCategoryFull;
            signal.LocationLat = lat;
            signal.LocationLon = lon;

            string[] categoryLevels = signalCategoryFull.Split('/');
            signal.CategoryLevel1 = categoryLevels[0];
            signal.CategoryLevel2 = categoryLevels.Length > 1 ? categoryLevels[1] : string.Empty;
            return signal;
        }

        private static void ExtractLatLon(string signalLocationText, ref decimal lat, ref decimal lon)
        {
            var match = Regex.Match(signalLocationText, @"\[([\d\.]+)\,([\d\.]+)\]");
            if (match.Success && match.Groups.Count > 2)
            {
                lat = Decimal.Parse(match.Groups[1].Value);
                lon = Decimal.Parse(match.Groups[2].Value);
            }
        }
    }
}
