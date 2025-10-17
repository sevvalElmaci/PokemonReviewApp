namespace PokemonReviewApp.Dto
{
    public class ReviewerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

    public class ReviewerDtoCreate
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

