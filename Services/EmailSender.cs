using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Nemesys.Services
{
    //Adapted from https://stackoverflow.com/questions/65070487/how-to-configure-email-sending-in-asp-net-core-for-identity-library
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("identity.nemesys@gmail.com", "Cud453M6VC3Nj5E")
            };

            MailMessage message = new MailMessage("identity.nemesys@gmail.com", email, subject, htmlMessage);
            message.IsBodyHtml = true;

            return client.SendMailAsync(message);
        }
    }
}