using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;


namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ReviewerRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool CreateReviewer(Reviewer reviewer, int userId)
        {
            reviewer.CreatedUserId = userId;
            reviewer.CreatedDateTime = DateTime.Now;
            reviewer.UpdatedUserId = userId;
            reviewer.UpdatedDateTime = DateTime.Now;
            reviewer.IsDeleted = false;

            _context.Add(reviewer);
            return Save();

        }


        public bool SoftDeleteReviewer(int reviewerId, int userId)
        {
            var reviewer = _context.Reviewers
                            .IgnoreQueryFilters()
                            .FirstOrDefault(r => r.Id == reviewerId);

            if (reviewer == null)
                return false;

            reviewer.IsDeleted = true;
            reviewer.DeletedUserId = userId;
            reviewer.DeletedDateTime = DateTime.Now;
            reviewer.UpdatedUserId = userId;
            reviewer.UpdatedDateTime = DateTime.Now;

            _context.Update(reviewer);
            return Save();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _context.Reviewers
                           .Include(r => r.Reviews)
                           .FirstOrDefault(r => r.Id == reviewerId && !r.IsDeleted);
        }

        public Reviewer GetReviewerIncludingDeleted(int id)
        {
            return _context.Reviewers
                           .IgnoreQueryFilters()
                           .Include(r => r.Reviews)
                           .FirstOrDefault(r => r.Id == id);
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers
                .Include(r => r.Reviews)
                .Where(r => !r.IsDeleted)
                .ToList();
        }

        public ICollection<Reviewer> GetReviewersIncludingDeleted()
        {
            return _context.Reviewers
                            .IgnoreQueryFilters()
                            .Include(r => r.Reviews)
                            .ToList();
        }

        public ICollection<Review> GetReviewsByAReviewer(int reviewerId)
        {
            return _context.Reviews
                .Where(r => r.Reviewer.Id == reviewerId)
                .ToList();
        }

        public bool RestoreReviewer(Reviewer reviewer)
        {
            reviewer.IsDeleted = false;
            reviewer.DeletedUserId = null;
            reviewer.DeletedDateTime = null;

            _context.Update(reviewer);
            return Save();
        }
        public bool ReviewerExists(int reviewerId)
        {
            return _context.Reviewers.Any(r => r.Id == reviewerId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;

        }

        public bool UpdateReviewer(Reviewer reviewer, int userId)
        {
            var existing = _context.Reviewers
                .IgnoreQueryFilters()
                .FirstOrDefault(r => r.Id == reviewer.Id);

            if (existing == null)
                return false;

            existing.FirstName = reviewer.FirstName;
            existing.LastName = reviewer.LastName;

            existing.UpdatedUserId = userId;
            existing.UpdatedDateTime = DateTime.Now;

            _context.Update(existing);
            return Save();
        }

    }
}
