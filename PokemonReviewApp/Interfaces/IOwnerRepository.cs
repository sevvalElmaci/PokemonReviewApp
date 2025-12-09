using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        //Fetch of all owners

        Owner GetOwner(int ownerId);
        //Find an owner with his id's
        ICollection<Owner> GetOwnerOfAPokemon(int pokeId);
        //Get owner list of a certain pokeId
        ICollection<Pokemon> GetPokemonByOwner(int ownerId);
        //Get pokemon list of a certain ownerId
        bool OwnerExists(int ownerId);  
        bool CreateOwner(Owner owner, int userId);
        bool UpdateOwner(Owner owner, int userId);
        bool SoftDeleteOwner(int ownerId, int userId);
        Owner GetOwnerIncludingDeleted(int id);
        void RestoreOwner(Owner owner);
        ICollection<Owner> GetDeletedOwners();
        ICollection<Country> GetCountriesIncludingDeleted();

        bool Save();

    }
}
