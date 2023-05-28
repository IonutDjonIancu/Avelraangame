using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IGameplayService
{
    Party CreateParty();
    Party JoinParty(string partyId, CharacterIdentity charIdentity);
}