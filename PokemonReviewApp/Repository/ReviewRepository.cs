using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ReviewRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ================
        // GETTERS
        // ================

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public Review GetReview(int id)
        {
            return _context.Reviews.FirstOrDefault(r => r.Id == id);
        }

        public ICollection<Review> GetReviewsIncludingDeleted()
        {
            return _context.Reviews
                .IgnoreQueryFilters()
                .Include(r => r.Reviewer)
                .Include(r => r.Pokemon)
                .ToList();
        }


        public Review GetReviewIncludingDeleted(int id)
        {
            return _context.Reviews
     .IgnoreQueryFilters()
     .Include(r => r.Reviewer)
     .Include(r => r.Pokemon)
     .FirstOrDefault(r => r.Id == id);
        }

        public ICollection<Review> GetReviewsForAPokemon(int pokeId)
        {
            return _context.Reviews
                .Where(r => r.PokemonId == pokeId)
                .ToList();
        }

        public bool ReviewExists(int id)
        {
            return _context.Reviews.Any(r => r.Id == id);
        }

        // ================
        // CREATE
        // ================

        public bool CreateReview(Review review, int userId)
        {
            review.CreatedUserId = userId;
            review.CreatedDateTime = DateTime.UtcNow;
            review.UpdatedUserId = userId;
            review.UpdatedDateTime = DateTime.UtcNow;
            review.IsDeleted = false;

            _context.Add(review);
            return Save();
        }

        // ================
        // UPDATE
        // ================

        public bool UpdateReview(Review review, int userId)
        {
            review.UpdatedUserId = userId;
            review.UpdatedDateTime = DateTime.UtcNow;

            _context.Update(review);
            return Save();
        }

        // ================
        // SOFT DELETE
        // ================

        public bool SoftDeleteReview(int reviewId, int userId)
        {
            var review = _context.Reviews
                .IgnoreQueryFilters()
                .FirstOrDefault(r => r.Id == reviewId);

            if (review == null)
                return false;

            review.IsDeleted = true;
            review.DeletedUserId = userId;
            review.DeletedDateTime = DateTime.UtcNow;
            review.UpdatedUserId = userId;
            review.UpdatedDateTime = DateTime.UtcNow;

            _context.Update(review);
            return Save();
        }

        // ================
        // RESTORE
        // ================

        public bool RestoreReview(Review review)
        {
            review.IsDeleted = false;
            review.DeletedUserId = null;
            review.DeletedDateTime = null;

            _context.Update(review);
            return Save();
        }

        // ================
        // DELETE MULTIPLE (unused)
        // ================

        public bool DeleteReviews(List<Review> reviews)
        {
            _context.RemoveRange(reviews);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
