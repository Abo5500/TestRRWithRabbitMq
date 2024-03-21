using Application.Options;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PriceItems.PriceItemsTaker
{
    public class PriceItemCsvMapping : ClassMap<PriceItemCsvModel>
    {
        public PriceItemCsvMapping(SupplierOptions supplierOptions)
        {
            Map(m => m.Vendor).Name(supplierOptions.Vendor);
            Map(m => m.Number).Name(supplierOptions.Number);
            Map(m => m.Description).Name(supplierOptions.Description);
            Map(m => m.Price).Name(supplierOptions.Price);
            Map(m => m.Count).Name(supplierOptions.Count);
        }
    }
}
