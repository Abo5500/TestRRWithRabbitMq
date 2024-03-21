using Application.Interfaces.PriceItems;
using Application.Options;
using CsvHelper.Configuration;
using CsvHelper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PriceItems.PriceItemsTaker
{
    public class PriceItemTaker : IPriceItemTaker
    {
        public async Task<List<PriceItem>> GetPriceItemsPartFromScvAsync(string path, int index, int chunkSize, SupplierOptions supplierOptions)
        {
            List<PriceItem> priceItems = new();
            using StreamReader streamReader = new(path);

            var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture);
            bool isRecordBad = false;
            config.BadDataFound = context =>
            {
                isRecordBad = true;
            };
            config.MissingFieldFound = null;
            config.Delimiter = ";";
            config.LineBreakInQuotedFieldIsBadData = true;
            config.Mode = CsvMode.NoEscape;
            config.Quote = '\"';
            if (index != 0)
            {
                config.ShouldSkipRecord = args => args.Row.Parser.RawRow < index && args.Row.Parser.RawRow > 1;
            }

            config.HasHeaderRecord = true;
            using CsvReader csvReader = new(streamReader, config);
            csvReader.Context.RegisterClassMap(new PriceItemCsvMapping(supplierOptions));
            //csvReader.ReadHeader();
            while (await csvReader.ReadAsync())
            {
                try
                {
                    var priceItemModel = csvReader.GetRecord<PriceItemCsvModel>();
                    if (isRecordBad)
                    {
                        isRecordBad = false;
                        continue;
                    }
                    PriceItem priceItem = new()
                    {
                        Vendor = priceItemModel.Vendor,
                        Number = priceItemModel.Number,
                        Description = SetCorrectDescription(priceItemModel.Description),
                        Price = decimal.Parse(priceItemModel.Price),
                        Count = SetCorrectCount(priceItemModel.Count),
                        SearchNumber = new string(priceItemModel.Number.Where(char.IsLetterOrDigit).ToArray()).ToUpper(),
                        SearchVendor = new string(priceItemModel.Number.Where(char.IsLetterOrDigit).ToArray()).ToUpper()
                    };
                    priceItems.Add(priceItem);
                    if (priceItems.Count == chunkSize)
                    {
                        csvReader.Dispose();
                        streamReader.Dispose();
                        return priceItems;
                    }
                }
                catch
                {
                    continue;
                }
            }
            return priceItems;
        }

        private static int SetCorrectCount(string count)
        {
            if (count.Contains('>') || count.Contains('<'))
            {
                return int.Parse(count.Remove(0, 1));
            }
            if (count.Contains('-'))
            {
                return int.Parse(count[(count.IndexOf('-') + 1)..]);
            }
            return int.Parse(count);
        }
        private static string SetCorrectDescription(string description)
        {
            if (description?.Length > 512)
            {
                //проверить, 511 или 512 надо
                return description[512..];
            }
            return description;
        }
    }
}
