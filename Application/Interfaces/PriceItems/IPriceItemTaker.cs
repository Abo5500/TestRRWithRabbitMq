using Application.Options;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.PriceItems
{
    public interface IPriceItemTaker
    {
        public Task<List<PriceItem>> GetPriceItemsPartFromScvAsync(string filePath, int index, int chunkSize, SupplierOptions supplierOptions);
    }
}
