﻿using BugTracker.Models;
using BugTracker.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BugTracker.Services
{
    public class BTEmailService : IEmailSender // can use my own IBTEmailSender
    {
        private readonly MailSettings _mailSettings;

        public BTEmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendContactEmailAsync(string emailFrom, string name, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(_mailSettings.Mail));
            email.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = $"<b>{name}</b> has sent you an email and can be reached at: <b>{emailFrom}</b><br/><br/>{htmlMessage}";

            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }

        public async Task SendEmailAsync(string emailTo, string subject, string htmlMessage)
        {
            MimeMessage email = new MimeMessage();

            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(emailTo));
            email.Subject = subject;

            var builder = new BodyBuilder()
            {
                HtmlBody = htmlMessage
            };

            email.Body = builder.ToMessageBody();

            try
            {
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

                await smtp.SendAsync(email);

                smtp.Disconnect(true);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
