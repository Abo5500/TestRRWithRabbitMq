using Application.Interfaces.PriceItems;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class PriceItemRepository : IPriceItemRepository
    {
        private readonly ApplicationDbContext _context;

        public PriceItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddPriceItemsAsync(List<PriceItem> priceItems)
        {
            await _context.PriceItems.AddRangeAsync(priceItems);
            return await _context.SaveChangesAsync();
        }
    }
}
