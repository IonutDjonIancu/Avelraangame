using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IGameplayService
{
    Party CreateParty(bool isSinglePlayerOnly);
    Party JoinParty(string partyId, bool isSinglePlayerOnly, CharacterIdentity charIdentity);
    Party LeaveParty(string partyId, CharacterIdentity charIdentity);

    
}