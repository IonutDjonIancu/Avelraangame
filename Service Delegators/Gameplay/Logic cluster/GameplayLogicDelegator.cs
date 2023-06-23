using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class GameplayLogicDelegator
{
    private readonly GameplayPartyLogic partyLogic;

    private GameplayLogicDelegator() { }
    internal GameplayLogicDelegator(IDatabaseService databaseService)
    {
        partyLogic = new GameplayPartyLogic(databaseService);
    }

    internal Party CreateParty(bool isSinglePlayerOnly)
    {
        return partyLogic.CreateParty(isSinglePlayerOnly);
    }

    internal Party JoinParty(string partyId, bool isSinglePlayerOnly, CharacterIdentity charIdentity)
    {
        return partyLogic.JoinParty(partyId, isSinglePlayerOnly, charIdentity);
    }

    internal Party LeaveParty(string partyId, CharacterIdentity charIdentity)
    {
        return partyLogic.LeaveParty(partyId, charIdentity);    
    }
}
