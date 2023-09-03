using System.Net.Mail;
using Data_Mapping_Containers.Dtos;
using Independent_Modules;

namespace Service_Delegators;

public interface IDatabaseExportLogic
{
    void ExportDatabase();
    void ExportLogs(int days);
}

public class DatabaseExportLogic : IDatabaseExportLogic
{
    private readonly AppSettings appSettings;
    private readonly IMailingModule mailingModule;

    public DatabaseExportLogic(AppSettings appSettings)
    {
        this.appSettings = appSettings;
        mailingModule = new MailingModule(appSettings.AvelraanEmail, appSettings.AvelraanEmailPass);
    }

    public void ExportDatabase()
    {
        var attachments = SetExportDatabaseAttachments();

        if (attachments.Count == 0) return;

        SendEmails("Avelraan DATABASE export", attachments);
    }

    public void ExportLogs(int days)
    {
        var logsLocation = appSettings.LogPath;
        var attachments = GetLogFilesAsAttachments(logsLocation, days);

        if (attachments.Count == 0) return;

        SendEmails("Avelraan LOGS export", attachments);
    }


    #region private methods
    private void SendEmails(string subject, List<Attachment> attachments)
    {
        var message = $"Export date: {DateTime.Now.ToShortDateString()}";

        foreach (var email in appSettings.AdminData.Recipients)
        {
            mailingModule.SendEmail(email, subject, message, attachments);
            Thread.Sleep(3000);
        }
    }

    private List<Attachment> SetExportDatabaseAttachments()
    {
        var listOfAttachments = new List<Attachment>();

        // players
        var paths = Directory.GetFiles(appSettings.DbPlayersPath);
        foreach (var path in paths)
        {
            var playerAttachment = new Attachment(path);
            listOfAttachments.Add(playerAttachment);
        }

        return listOfAttachments;
    }

    private static List<Attachment> GetLogFilesAsAttachments(string logsPath, int days)
    {
        var listOfAttachments = new List<Attachment>();
        var listOfPaths = Directory.GetFiles(logsPath).Reverse().ToList();
        var counter = 1;

        foreach (var path in listOfPaths)
        {
            if (counter > days || counter >= 7)
            {
                return listOfAttachments;
            }

            var attachment = new Attachment(path);
            listOfAttachments.Add(attachment);
            counter++;
        }

        return listOfAttachments;
    }
    #endregion
}
