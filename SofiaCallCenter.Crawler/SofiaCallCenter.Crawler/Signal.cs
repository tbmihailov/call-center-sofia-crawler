using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SofiaCallCenter.Crawler
{
    class Signal
    {
        public int Id { get; set; }

        public string DocNumberInfo { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public string CategoryFull { get; set; }

        public string CategoryLevel1 { get; set; }

        public string CategoryLevel2 { get; set; }
    }
}
