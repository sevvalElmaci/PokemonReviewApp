using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsForAPokemon(int pokeId);
        bool ReviewExists(int reviewId);


        //bir reviewer'dan gelen bütün review'ları incelemek için method yok.
        //ICollection<Reviewer> GetReviewsFromAReviewer(int reviewerId);

        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool DeleteReviews(List<Review> reviews);

        bool Save();




    }
}
