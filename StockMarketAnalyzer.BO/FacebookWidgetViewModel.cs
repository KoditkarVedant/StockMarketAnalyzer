using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.BO
{
    public class FacebookWidgetViewModel
    {
        public bool IsAdminUser { get; set; }
        public string Ticker { get; set; }
        public string Name { get; set; }
        public string FacebookURL { get; set; }
    }
}
