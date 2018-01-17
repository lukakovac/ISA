using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ISA.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SmtpKey, subject, message, email);
        }

        private Task Execute(string apiKey, string subject, string message, string email)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("tim.isa.ftn@gmail.com"));
            mimeMessage.To.Add(new MailboxAddress(email));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart() { Text = message };

            var client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, false);

            client.Authenticate("tim.isa.ftn@gmail.com", "execom123");
            return client.SendAsync(mimeMessage);
        }
    }
}
