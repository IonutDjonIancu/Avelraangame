using System.Net.Mail;
using System.Numerics;
using System.Text;
using Data_Mapping_Containers.Dtos;
using Independent_Modules;
using Newtonsoft.Json;

namespace Service_Delegators;

public interface IDatabaseExportLogic
{
    void ExportPlayers();
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
        mailingModule = new MailingModule(Environment.GetEnvironmentVariable("AvelraanEmail")!, Environment.GetEnvironmentVariable("AvelraanGmailPassword")!);
    }

    public void ExportPlayers()
    {
        var playersJson = JsonConvert.SerializeObject(snapshot.Players);
        byte[] bytes = Encoding.UTF8.GetBytes(playersJson);
        using (MemoryStream stream = new (bytes))
        {
            var attachments = new List<Attachment>
            {
                new (stream, "players.txt", "text/plain")
            };

            SendEmail($"Avelraan PLAYERS export @ {DateTime.Now.ToShortDateString()}", "Players data are attached as a text file.", attachments);
        }
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
