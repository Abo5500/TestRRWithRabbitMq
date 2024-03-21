using Application.Common.Models;
using Application.Interfaces.Email;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Email
{
    public class EmailImapChecker : IEmailContainsFileChecker
    {
        private readonly EmailOptions _emailOptions;

        public EmailImapChecker(IOptions<EmailOptions> emailOptions)
        {
            _emailOptions = emailOptions.Value;
        }
        public async Task<EmailFileInfo> GetFileInfoAsync(string emailSender, string fileExtension)
        {
            using var client = new ImapClient();
            await client.ConnectAsync(_emailOptions.Host, 993, MailKit.Security.SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(_emailOptions.Email, _emailOptions.AppPassword);
            await client.Inbox.OpenAsync(MailKit.FolderAccess.ReadOnly);
            var uids = await client.Inbox.SearchAsync(SearchQuery.FromContains(emailSender));
            for (int i = uids.Count - 1; i > 0; i--)
            {
                var message = await client.Inbox.GetMessageAsync(uids[i]);
                var attachment = message.Attachments.FirstOrDefault(a => a.ContentDisposition.FileName.EndsWith($"{fileExtension}"));
                if (attachment is not null)
                {
                    var mimePart = attachment as MimePart;
                    if(mimePart is not null)
                    {
                        return new EmailFileInfo()
                        {
                            MessageId = uids[i].Id,
                            FileName = mimePart.FileName,
                            EmailSender = emailSender
                        };
                    }
                }
            }
            return null;
        }
    }
}
