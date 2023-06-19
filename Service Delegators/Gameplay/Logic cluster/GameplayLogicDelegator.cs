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

    internal Party CreateParty()
    {
        return partyLogic.CreateParty();
    }

    internal Party JoinParty(string partyId, CharacterIdentity charIdentity)
    {
        return partyLogic.JoinParty(partyId, charIdentity);
    }

    internal Party LeaveParty(string partyId, CharacterIdentity charIdentity)
    {
        return partyLogic.LeaveParty(partyId, charIdentity);    
    }
}
