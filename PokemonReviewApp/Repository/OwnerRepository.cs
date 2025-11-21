using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public OwnerRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public ICollection<Owner> GetOwners()
        {
            return _context.Owners
                .ToList();
        }
        public Owner GetOwner(int ownerId)
        {
            return _context.Owners
                .Where(o => o.Id == ownerId)
                .FirstOrDefault();
        }
        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _context.PokemonOwners
                .Where(p => p.OwnerId == ownerId)
                .Select(p => p.Pokemon)
                .ToList();
        }
        public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
        {
            return _context.PokemonOwners
                .Where(p => p.Pokemon.Id == pokeId)
                .Select(o => o.Owner)
                .ToList();
        }
        public bool OwnerExists(int ownerId)
        {
            return _context.Owners
                .Any(o => o.Id == ownerId);
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            return Save();
        }

        public void SoftDeleteOwner(Owner owner)
        {
            owner.IsDeleted = true;
            owner.DeletedDateTime = DateTime.Now;
            _context.Owners.Update(owner);
        }

        public Owner GetOwnerIncludingDeleted(int id)
        {
            return _context.Owners
                            .IgnoreQueryFilters()
                            .Include(o => o.Country)
                            .FirstOrDefault(o => o.Id == id);
        }

        public void RestoreOwner(Owner owner)
        {
            owner.IsDeleted = false;
            owner.DeletedDateTime = null;
            _context.Owners.Update(owner);
        }

        public ICollection<Owner> GetDeletedOwners()
        {
            return _context.Owners
                .IgnoreQueryFilters()
                .Where(o => o.IsDeleted)
                .OrderBy(o => o.Id)
                .ToList();
        }
    }
}
