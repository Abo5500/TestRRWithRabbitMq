using Application.Common.Models;
using Application.Interfaces.Email;
using MailKit.Net.Imap;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Email
{
    public class EmailImapLoader : IEmailFileLoader
    {
        private readonly EmailOptions _emailOptions;

        public EmailImapLoader(IOptions<EmailOptions> emailOptions)
        {
            _emailOptions = emailOptions.Value;
        }
        public async Task LoadFileAsync(EmailFileInfo fileInfo, string outputPath)
        {
            using var client = new ImapClient();
            await client.ConnectAsync(_emailOptions.Host, 993, MailKit.Security.SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(_emailOptions.Email, _emailOptions.AppPassword);

            await client.Inbox.OpenAsync(MailKit.FolderAccess.ReadOnly);

            var message = await client.Inbox.GetMessageAsync(new MailKit.UniqueId(fileInfo.MessageId));
            var attachment = message.Attachments.FirstOrDefault(a => a.ContentDisposition.FileName == fileInfo.FileName);

            if (attachment is not null)
            {
                using var output = File.Create(outputPath);

                await ((MimePart)attachment).Content.DecodeToAsync(output);
            }
        }
    }
}
