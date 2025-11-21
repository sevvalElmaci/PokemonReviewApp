namespace PokemonReviewApp.Models
{
    public abstract class AuditEntityBase
    {
        public int? CreatedUserId { get; set; }           // JWT gelince otomatik dolacak
        public DateTime CreatedDateTime { get; set; } 

        public int? UpdatedUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public bool IsDeleted { get; set; } = false;
        public int? DeletedUserId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
