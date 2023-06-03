using System.Net.Mail;
using Independent_Modules;
using Persistance_Manager;

namespace Service_Delegators;

internal class DatabaseExportLogic
{
    private readonly IDatabaseManager dbm;
    private readonly IMailingModule mailingModule;

    private readonly List<string> listOfRecipients = new()
    {
        "iiancu85@gmail.com"
    };

    private DatabaseExportLogic() { }
    internal DatabaseExportLogic(IDatabaseManager databaseManager)
    {
        dbm = databaseManager;
        mailingModule = new MailingModule(dbm.Info.AvelraanEmail, dbm.Info.AvelraanPassword);
    }

    internal void ExportLogs(int days)
    {
        var logsLocation = dbm.Info.LogPath;
        var attachments = GetLogFilesAsAttachments(logsLocation, days);

        if (attachments.Count == 0) return;

        SendEmails("Logs exported", attachments);
    }

    public void ExportDatabase()
    {
        var attachments = SetExportDatabaseAttachments();

        if (attachments.Count == 0) return;

        SendEmails("Database exported", attachments);
    }

    #region private methods
    private void SendEmails(string subject, List<Attachment> attachments)
    {
        var message = $"Export triggered on: {DateTime.Now.ToShortDateString()}";

        foreach (var email in listOfRecipients)
        {
            mailingModule.SendEmail(email, subject, message, attachments);
            Thread.Sleep(1000);
        }
    }

    private List<Attachment> SetExportDatabaseAttachments()
    {
        var listOfAttachments = new List<Attachment>();

        // database
        var databaseAttachment = new Attachment(dbm.Info.DbPath);
        listOfAttachments.Add(databaseAttachment);

        // players
        var paths = Directory.GetFiles(dbm.Info.DbPlayersPath);
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
