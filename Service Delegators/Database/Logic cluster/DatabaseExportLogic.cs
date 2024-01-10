using System.Net.Mail;
using Data_Mapping_Containers.Dtos;
using Independent_Modules;
using Newtonsoft.Json;

namespace Service_Delegators;

public interface IDatabaseExportLogic
{
    void ExportSnapshot();
}

public class DatabaseExportLogic : IDatabaseExportLogic
{
    private readonly AppSettings appSettings;
    private readonly IMailingModule mailingModule;
    private readonly Snapshot snapshot;

    public DatabaseExportLogic(
        AppSettings appSettings,
        Snapshot snapshot)
    {
        this.appSettings = appSettings;
        this.snapshot = snapshot;
        mailingModule = new MailingModule(appSettings.AvelraanEmail, appSettings.AvelraanEmailPass);
    }

    public void ExportSnapshot()
    {
        var snapshotJson = JsonConvert.SerializeObject(snapshot);

        SendEmail($"Avelraan SNAPSHOT export {DateTime.Now.ToShortDateString()}", snapshotJson, new List<Attachment>());
    }

    #region private methods
    private void SendEmail(string subject, string message, List<Attachment> attachments)
    {
        foreach (var email in appSettings.AdminData.Recipients)
        {
            mailingModule.SendEmail(email, subject, message, attachments);
            Thread.Sleep(3000);
        }
    }
    #endregion
}
