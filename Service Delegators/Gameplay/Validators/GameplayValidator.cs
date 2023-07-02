#pragma warning disable CA1822 // Mark members as static

using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class GameplayValidator : ValidatorBase
{
    private readonly Snapshot snapshot;

    internal GameplayValidator(Snapshot snapshot)
        : base(snapshot)
    {
        this.snapshot = snapshot;
    }

    internal void ValidatePartyOnCreate(Position position)
    {
        ValidatePosition(position);
    }

    internal void ValidatePartyBeforeJoin(string partyId, CharacterIdentity charIdentity)
    {
        ValidateCharacterPlayerCombination(charIdentity);
        ValidateIfCharacterIsInAnotherParty(charIdentity);

        var character = snapshot.Players.Find(p => p.Identity.Id == charIdentity.PlayerId)!.Characters.Find(c => c.Identity.Id == charIdentity.Id)!;

        var party = snapshot.Parties.Find(p => p.Identity.Id == partyId);
        if (party != null && party!.Characters.Count > 0)
        {
            if (character.Position.Location != party!.Position.Location) throw new Exception($"Unable to join party. You must first travel to their location, {party.Position.Location}, in order to join.");
        }
    }

    internal void ValidatePartyOnLeave(string partyId, CharacterIdentity charIdentity)
    {
        if (!snapshot.Parties.Exists(p => p.Identity.Id == partyId)) throw new Exception("Party with specified id does not exist.");

        ValidateCharacterPlayerCombination(charIdentity);

        var party = snapshot.Parties.FirstOrDefault(p => p.Identity.Id == partyId)!;
        
        if (!party.Characters.Select(s => s.Id).Contains(charIdentity.Id)) throw new Exception("This party does not have this character.");
        if (!string.IsNullOrEmpty(party.Status.QuestId)) throw new Exception("Cannot abandon party during adventuring.");
    }

    internal void ValidateParty(string partyId)
    {
        var party = snapshot.Parties.Find(s => s.Identity.Id == partyId) ?? throw new Exception("No party found.");

        ValidatePosition(party.Position);
    }

    internal void ValidateQuestOnBegin(string partyId, string questName)
    {
        ValidateParty(partyId);

        var quest = QuestsLore.All.Find(s => s.Name == questName) ?? throw new Exception("No such quest found.");
        var party = snapshot.Parties.Find(s => s.Identity.Id == partyId)!;

        if (quest.Position.Region != party.Position.Region) throw new Exception("Party and quest have a different region.");
        if (quest.Position.Subregion != party.Position.Subregion) throw new Exception("Party and quest have a different subregion.");
        if (quest.Position.Land != party.Position.Land) throw new Exception("Party and quest have a different land.");
        if (quest.Position.Location != party.Position.Location) throw new Exception("Party and quest have a different location.");

        if (!quest.IsRepeatable)
        {
            var listOfFinishedQuests = new List<string>();

            foreach (var charIdentity in party.Characters)
            {
                var character = snapshot.Players.Find(s => s.Identity.Id == charIdentity.PlayerId)!.Characters.Find(p => p.Identity.Id == charIdentity.Id)!;

                character.Status.QuestsFinished.ForEach(s => listOfFinishedQuests.Add(s));
            }

            if (listOfFinishedQuests.Contains(questName)) throw new Exception("One of the characters has already completed this quest.");
        }
    }

    #region private methods
    private void ValidateIfCharacterIsInAnotherParty(CharacterIdentity charIdentity)
    {
        var isCharInAnotherParty = snapshot.Parties
            .SelectMany(p => p.Characters)
            .Select(c => c.Id)
            .Contains(charIdentity.Id);
        if (isCharInAnotherParty) throw new Exception("Character is already in a party.");
    }

    private static void ValidatePosition(Position position)
    {
        ValidateObject(position, "Position is null.");

        var fullName = $"{position.Region}_{position.Subregion}_{position.Land}_{position.Location}";

        if (!GameplayLore.MapLocations.All.Select(s => s.FullName).ToList().Contains(fullName)) throw new Exception("Position data is wrong or incomplete.");
    }
    #endregion
}

#pragma warning restore CA1822 // Mark members as static