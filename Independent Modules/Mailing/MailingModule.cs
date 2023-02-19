using System.Net.Mail;
using System.Net;

namespace Independent_Modules;

public class MailingModule : IMailingModule
{
    private readonly string from;
    private readonly string googlePass;

    public MailingModule(string senderEmail, string senderPassword)
    {
        from = senderEmail;
        googlePass = senderPassword;
    }

    public void SendEmail(string sendTo, string subject, string messageBody)
    {
        Gmail(sendTo, subject, messageBody, new List<Attachment>());
    }

    public void SendEmail(string sendTo, string subject, string messageBody, List<Attachment>? attachments)
    {
        Gmail(sendTo, subject, messageBody, attachments);
    }

    private void Gmail(string sendTo, string subject, string messageBody, List<Attachment>? attachments)
    {
        MailMessage message = new();
        SmtpClient smtp = new();

        message.From = new MailAddress(from);
        message.To.Add(new MailAddress(sendTo));
        message.Subject = subject;
        message.IsBodyHtml = true; //to make message body as html  
        message.Body = messageBody;

        if (attachments?.Count > 0)
        {
            foreach (var item in attachments)
            {
                message.Attachments.Add(item);
            }
        }

        smtp.Port = 587;
        smtp.Host = "smtp.gmail.com"; //for gmail host  
        smtp.EnableSsl = true;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(from, googlePass);
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtp.Send(message);
    }
}
