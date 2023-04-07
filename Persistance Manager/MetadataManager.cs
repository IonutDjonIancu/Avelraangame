#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Data_Mapping_Containers.Dtos;
using Independent_Modules;
using Persistance_Manager.Validators;
using System.Net.Mail;

namespace Persistance_Manager;

public class MetadataManager
{
    private readonly IMailingModule mailingModule;
    private readonly DatabaseManagerValidator validator;
    private readonly DatabaseManager dbm;

    private MetadataManager() { }

    public MetadataManager(DatabaseManager databaseManager)
    {
        dbm = databaseManager;
        validator = new DatabaseManagerValidator(databaseManager);
        mailingModule = new MailingModule(dbm.info.AvelraanEmail, dbm.info.AvelraanPassword);
    }

    #region players
    public Player? GetPlayerById(string id)
    {
        return dbm.Snapshot.Players.Find(p => p.Identity.Id == id);
    }

    public Player? GetPlayerByName(string name)
    {
        return dbm.Snapshot.Players.Find(p => p.Identity.Name == name);
    }

    public bool IsPlayerBanned(string playerEmail)
    {
        return dbm.info.Banned.Contains(playerEmail);
    }

    public bool IsPlayerAdmin(string playerName)
    {
        return dbm.info.Admins.Contains(playerName);
    }
    #endregion

    #region characters
    public Character? GetCharacterById(string characterId, string playerId)
    {
        var player = GetPlayerById(playerId);

        return player.Characters.Find(c => c.Identity.Id == characterId);
    }
    #endregion

    #region database
    public bool ExportLogs(LogsExport export)
    {
        validator.ValidateObject(export);
        validator.EmailShouldBeAdmin(export.ReceiverEmail);
        validator.DaysLimit(export.HowManyDays);

        var subject = $"Logs exported on - {DateTime.Now.ToShortDateString()}";
        var logsLocation = dbm.info.LogPath;
        var attachments = GetLogFilesAsAttachments(logsLocation, export.HowManyDays);

        if (attachments.Count > 0)
        {
            mailingModule.SendEmail(export.ReceiverEmail, subject, "See logs attached.", attachments);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ExportDatabase(string recipient)
    {
        validator.EmailShouldBeAdmin(recipient);

        var subject = $"Database exported on: {DateTime.Now.ToShortDateString()}";

        var listOfAttachments = new List<Attachment>()
        {
            new Attachment(dbm.info.DbPath)
        };

        if (listOfAttachments.Count > 0)
        {
            mailingModule.SendEmail(recipient, subject, "Database json attached.", listOfAttachments);
            return true;
        }
        else
        {
            return false;
        }
    }

    
    #endregion

    #region private methods
    private List<Attachment> GetLogFilesAsAttachments(string logsPath, int days)
    {
        validator.ValidateString(logsPath);

        var listOfAttachments = new List<Attachment>();

        for (int i = 0; i < days; i++)
        {
            var timeSpan = new TimeSpan(i, 0, 0, 0);
            var dateString = DateTime.Now.Subtract(timeSpan).ToShortDateString().Split('/');

            var logPath = $"{logsPath}Logs{dateString[2]}{dateString[0]}{dateString[1]}.txt";

            listOfAttachments.Add(new Attachment(logPath));

            validator.ValidateNumber(listOfAttachments.Count);
        }

        return listOfAttachments;
    }
    #endregion
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.