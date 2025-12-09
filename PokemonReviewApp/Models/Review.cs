namespace PokemonReviewApp.Models
{
    public class Review : AuditEntityBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }

        public int ReviewerId { get; set; }
        public Reviewer Reviewer { get; set; }

        public int PokemonId { get; set; }
        public Pokemon Pokemon { get; set; }


        //navigation properties, pokemon pokemon olduğu icin bir reviewin sadece bir pokemona ait olabilecegini belirtiyoruz
        //ICollection yapsaydık o zaman bir reviewin birden fazla pokemona ait olabilecegini dusunurdu entity framework
    }
}
