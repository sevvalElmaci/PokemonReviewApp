namespace PokemonReviewApp.Models
{
    public class PokemonLog
    {
        public int Id { get; set; }
        public int PokemonId { get; set; }

        public string? ActionType { get; set; }       // CREATE, UPDATE, DELETE
        public string? ChangedField { get; set; }     // "Name", "OwnerId"
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

        public string? Name { get; set; }             // OPTIONAL: only for readability
        public int? OwnerId { get; set; }             // OPTIONAL: snapshot of owner at moment

        public int? CreatedUserId { get; set; }
        public DateTime? CreatedDateTime { get; set; }

        public int? UpdatedUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public int? DeletedUserId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }

}
