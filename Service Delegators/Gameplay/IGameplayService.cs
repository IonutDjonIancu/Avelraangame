using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IGameplayService
{
    Party CreateParty(Position position, bool isSinglePlayerOnly = false);
    Party JoinParty(string partyId, CharacterIdentity charIdentity, bool isSinglePlayerOnly = false);
    Party LeaveParty(string partyId, CharacterIdentity charIdentity);

}