using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.PriceItems
{
    public interface IPriceItemRepository
    {
        public Task<int> AddPriceItemsAsync(List<PriceItem> priceItems);
    }
}
