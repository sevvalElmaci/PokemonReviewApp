using AutoMapper;
using PokemonReviewApp.Models;
using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDto>().ReverseMap();
            CreateMap<Pokemon, PokemonDtoCreate>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryDtoCreate>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, CountryDtoCreate>().ReverseMap();
            CreateMap<Owner, OwnerDto>().ReverseMap();
            CreateMap<Owner, OwnerDtoCreate>().ReverseMap();
            CreateMap<Review, ReviewDto>().ReverseMap();
            CreateMap<Review, ReviewDtoCreate>().ReverseMap();
            CreateMap<Reviewer, ReviewerDto>().ReverseMap();
            CreateMap<Reviewer, ReviewerDtoCreate>().ReverseMap();
            CreateMap<Food, FoodDto>().ReverseMap();
            CreateMap<Food, FoodDtoCreate>().ReverseMap();
        }
    }
}
