using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SofiaCallCenter.Crawler
{
    public class SignalHtmlStructureNotDetectedException : Exception
    {
        public SignalHtmlStructureNotDetectedException(int signalId) : base(string.Format("Signal structure not detected for [{0}]", signalId)) { }
    }
}
