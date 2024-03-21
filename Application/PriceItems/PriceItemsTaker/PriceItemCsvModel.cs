using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PriceItems.PriceItemsTaker
{
    public struct PriceItemCsvModel
    {
        public string Vendor { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Count { get; set; }
    }
}
