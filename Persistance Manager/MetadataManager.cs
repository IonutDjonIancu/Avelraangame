#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null reference return.

using Data_Mapping_Containers.Dtos;
using Independent_Modules;
using Persistance_Manager.Validators;
using System.Net.Mail;

namespace Persistance_Manager;

public class MetadataManager
{
    private const string djonEmail = "iiancu85@gmail.com";

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
    public Player GetPlayerById(string id)
    {
        var player = dbm.Snapshot.Players.Find(p => p.Identity.Id == id);

        validator.ValidateObject(player, "Player is null");

        return player;
    }

    public Player GetPlayerByName(string name)
    {
        var player = dbm.Snapshot.Players.Find(p => p.Identity.Name == name);

        validator.ValidateObject(player, "Player is null");

        return player;
    }

    public bool IsPlayerBanned(string playerName)
    {
        return dbm.info.Banned.Contains(playerName);
    }

    public bool IsPlayerAdmin(string playerName)
    {
        return dbm.info.Admins.Contains(playerName);
    }
    #endregion

    #region characters
    public Character GetCharacterById(string characterId, string playerId)
    {
        var player = GetPlayerById(playerId);
        var character = player.Characters.Find(c => c.Identity.Id == characterId);
        
        validator.ValidateObject(character, "Character is null");

        return character;
    }

    public bool DoesCharacterExist(string characterId, string playerId)
    {
        var player = GetPlayerById(playerId);
        return player.Characters.Exists(c => c.Identity.Id == characterId);
    }
    #endregion

    #region database
    public bool ExportLogs(int days, string playerId)
    {
        var player = GetPlayerById(playerId);

        validator.PlayerShouldBeAdmin(player.Identity.Name);
        validator.DaysLimit(days);

        var subject = $"Logs exported on - {DateTime.Now.ToShortDateString()}";
        var logsLocation = dbm.info.LogPath;
        var attachments = GetLogFilesAsAttachments(logsLocation, days);

        if (attachments.Count > 0)
        {
            mailingModule.SendEmail(djonEmail, subject, "See logs attached.", attachments);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ExportDatabase(string playerId)
    {
        var player = GetPlayerById(playerId);

        validator.PlayerShouldBeAdmin(player.Identity.Name);

        var subject = $"Database exported on: {DateTime.Now.ToShortDateString()}";

        var listOfAttachments = SetExportDatabaseAttachments();

        if (listOfAttachments.Count > 0)
        {
            mailingModule.SendEmail(djonEmail, subject, "Database json attached.", listOfAttachments);
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region private methods
    private List<Attachment> SetExportDatabaseAttachments()
    {
        var listOfAttachments = new List<Attachment>();

        // database
        var databaseAttachment = new Attachment(dbm.info.DbPath);
        listOfAttachments.Add(databaseAttachment);

        // players
        var paths = Directory.GetFiles(dbm.info.DbPlayersPath);
        foreach (var path in paths)
        {
            var playerAttachment = new Attachment(path);
            listOfAttachments.Add(playerAttachment);
        }

        return listOfAttachments;
    }

    private List<Attachment> GetLogFilesAsAttachments(string logsPath, int days)
    {
        validator.ValidateString(logsPath);

        var listOfAttachments = new List<Attachment>();
        var listOfPaths = Directory.GetFiles(logsPath).Reverse().ToList();
        var counter = 1;

        foreach(var path in listOfPaths)
        {
            if (counter > days)
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

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8603 // Possible null reference return.