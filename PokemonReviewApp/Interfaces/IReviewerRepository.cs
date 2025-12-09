using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        ICollection<Reviewer> GetReviewersIncludingDeleted();
        ICollection<Review> GetReviewsByAReviewer(int reviewerId);

        Reviewer GetReviewer(int reviewerId);
        Reviewer GetReviewerIncludingDeleted(int id);         

        bool ReviewerExists(int reviewerId);

        bool CreateReviewer(Reviewer reviewer, int userId);
        bool UpdateReviewer(Reviewer reviewer, int userId);
        bool SoftDeleteReviewer(int reviewerId, int userId);
        bool RestoreReviewer(Reviewer reviewer);

        bool Save();

    }
}
