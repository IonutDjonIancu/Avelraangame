using System.Net.Mail;

namespace Independent_Modules;

public interface IMailingModule
{
    void SendEmail(string sendTo, string subject, string messageBody);
    void SendEmail(string sendTo, string subject, string messageBody, List<Attachment> attachments);
}