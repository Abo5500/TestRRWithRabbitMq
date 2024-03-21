using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Email
{
    public interface IEmailContainsFileChecker
    {
        Task<EmailFileInfo> GetFileInfoAsync(string senderEmail, string fileExtension);
    }
}
