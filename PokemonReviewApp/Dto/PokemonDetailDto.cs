namespace PokemonReviewApp.Dto
{
    public class PokemonDetailDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string CreatedBy { get; set; }
        public string OwnerName { get; set; }
        public string CategoryName { get; set; }
        public string CategoryCreatedBy { get; set; }
        public List<string> Owners { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Foods { get; set; }
        public decimal Rating { get; set; }
    }

}
