using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class GameplayLogicDelegator
{
    private readonly GameplayPartyLogic partyLogic;
    private readonly GameplayQuestsLogic questLogic;

    private GameplayLogicDelegator() { }
    internal GameplayLogicDelegator(
        IDatabaseService databaseService,
        IDiceRollService diceRollService)
    {
        partyLogic = new GameplayPartyLogic(databaseService);
        questLogic = new GameplayQuestsLogic(databaseService, diceRollService);
    }

    internal Party CreateParty(Position position, bool isSinglePlayerOnly = false)
    {
        return partyLogic.CreateParty(position, isSinglePlayerOnly);
    }

    internal Party JoinParty(string partyId, CharacterIdentity charIdentity, bool isSinglePlayerOnly = false)
    {
        return partyLogic.JoinParty(partyId, charIdentity, isSinglePlayerOnly);
    }

    internal Party LeaveParty(string partyId, CharacterIdentity charIdentity)
    {
        return partyLogic.LeaveParty(partyId, charIdentity);    
    }

    internal List<Quest> GetQuestsAtPartyLocation(string partyId)
    {
        return questLogic.GetQuestsAtPartyLocation(partyId);
    }

    internal Quest BeginQuestForParty(string partyId, string questName)
    {
        return questLogic.BeginQuestForParty(partyId, questName);
    }
}
