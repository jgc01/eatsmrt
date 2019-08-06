using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace EatIn.Models
{
    public class EmailUtility
    {
        public static void SendConfirmation(String toEmail, String emailContent, String subject = "New Account", bool htmlOrNot = false)
        {
            // Command line argument must the the SMTP host.
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential("team.eatsmrt@gmail.com", "vnbmnekpszpecbxw");

            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            MailAddress from = new MailAddress("team.eatsmrt@gmail.com");
            MailAddress to = new MailAddress(toEmail);
            MailMessage message = new MailMessage(from, to);
            message.Subject = subject;
            message.Body = emailContent;

            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = htmlOrNot;
            client.Send(message);

            message.Dispose();
            
        }
    }
}
