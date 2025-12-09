using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Diagnostics.Metrics;

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
        .Include(o => o.Country)
        .ToList();
        }
        public Owner GetOwner(int ownerId)
        {
            return _context.Owners
        .Include(o => o.Country)
        .FirstOrDefault(o => o.Id == ownerId);
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

        public bool CreateOwner(Owner owner, int userId)
        {

            owner.CreatedUserId = userId;
            owner.CreatedDateTime = DateTime.Now;

            owner.UpdatedUserId = userId;
            owner.UpdatedDateTime = DateTime.Now;

            owner.IsDeleted = false;

            _context.Add(owner);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner, int userId)
        {
            owner.UpdatedUserId = userId;
            owner.UpdatedDateTime = DateTime.Now;
            _context.Update(owner);
            return Save();
        }


        public bool SoftDeleteOwner(int ownerId, int userId)
        {
            var entity = _context.Owners
                  .FirstOrDefault(c => c.Id == ownerId);

            if (entity == null)
                return false;

            entity.IsDeleted = true;
            entity.DeletedUserId = userId;
            entity.DeletedDateTime = DateTime.Now;
            entity.UpdatedUserId = userId;
            entity.UpdatedDateTime = DateTime.Now;

            return Save();
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
            owner.DeletedUserId = null;
            owner.DeletedDateTime = null;
            _context.Owners.Update(owner);
        }

        public ICollection<Owner> GetDeletedOwners()
        {
            return _context.Owners
       .IgnoreQueryFilters()
       .Include(o => o.Country)
       .Where(o => o.IsDeleted == true)
       .ToList();
        }

        public ICollection<Country> GetCountriesIncludingDeleted()
        {
            return _context.Countries
               .IgnoreQueryFilters()
               .ToList();
        }
    }
}
