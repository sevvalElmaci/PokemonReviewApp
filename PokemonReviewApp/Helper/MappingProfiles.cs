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
            CreateMap<PokeFood, PokeFoodDtoCreate>().ReverseMap();
            CreateMap<PokeFood, PokeFoodDto>()
           .ForMember(dest => dest.PokemonName, opt => opt.MapFrom(src => src.Pokemon.Name))
           .ForMember(dest => dest.FoodName, opt => opt.MapFrom(src => src.Food.Name))
           .ReverseMap();
            CreateMap<Property, PropertyDto>().ReverseMap();
            CreateMap<Property, PropertyDtoUpdate>().ReverseMap();
            CreateMap<PokeProperty, PokePropertyDto>()
           .ForMember(dest => dest.PokemonName, opt => opt.MapFrom(src => src.Pokemon.Name))
           .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(src => src.Property.Name));


            CreateMap<PokeProperty, PokePropertyDto>().ReverseMap();
            CreateMap<PokeProperty, PokePropertyDtoUpdate>().ReverseMap();
            CreateMap<PokeProperty, PokePropertyDtoCreate>().ReverseMap();
            CreateMap<User, UserDto>()
    .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
    .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId));


            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserCreateDto, User>().ReverseMap();
            CreateMap<UserUpdateDto, User>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<RoleCreateDto, Role>().ReverseMap();
            CreateMap<Permission, PermissionDto>().ReverseMap();
            CreateMap<PermissionCreateDto, Permission>().ReverseMap();















        }
    }
}
