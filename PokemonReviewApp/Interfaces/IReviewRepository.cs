using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsForAPokemon(int pokeId);
        bool ReviewExists(int reviewId);
        ICollection<Review> GetReviewsIncludingDeleted();
        Review GetReviewIncludingDeleted(int id);


        //bir reviewer'dan gelen bütün review'ları incelemek için method yok.
        //ICollection<Reviewer> GetReviewsFromAReviewer(int reviewerId);

        bool CreateReview(Review review, int userId);
        bool UpdateReview(Review review, int userId);
        bool SoftDeleteReview(int reviewId, int userId);
        bool RestoreReview(Review review);

        bool DeleteReviews(List<Review> reviews);

        bool Save();




    }
}
