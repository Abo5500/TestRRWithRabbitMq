using Application.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models
{
    public class EmailFileInfo
    {
        public uint MessageId { get; set; }
        public string EmailSender { get; set; }
        public string FileName { get; set; }
        public SupplierOptions SupplierOptions { get; set; }
    }
}
