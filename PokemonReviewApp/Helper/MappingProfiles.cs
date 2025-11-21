using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // Pokemon
        CreateMap<Pokemon, PokemonDto>().ReverseMap();
        CreateMap<Pokemon, PokemonDtoCreate>().ReverseMap();

        // Category
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Category, CategoryDtoCreate>().ReverseMap();

        // Country
        CreateMap<Country, CountryDto>().ReverseMap();
        CreateMap<Country, CountryDtoCreate>().ReverseMap();

        // Owner
        CreateMap<Owner, OwnerDto>().ReverseMap();
        CreateMap<Owner, OwnerDtoCreate>().ReverseMap();

        // Review
        CreateMap<Review, ReviewDto>().ReverseMap();
        CreateMap<Review, ReviewDtoCreate>().ReverseMap();

        // Reviewer
        CreateMap<Reviewer, ReviewerDto>().ReverseMap();
        CreateMap<Reviewer, ReviewerDtoCreate>().ReverseMap();

        // Food
        CreateMap<Food, FoodDto>().ReverseMap();
        CreateMap<Food, FoodDtoCreate>().ReverseMap();

        // PokeFood
        CreateMap<PokeFood, PokeFoodDto>()
           .ForMember(dest => dest.PokemonName, opt => opt.MapFrom(src => src.Pokemon.Name))
           .ForMember(dest => dest.FoodName, opt => opt.MapFrom(src => src.Food.Name));
        CreateMap<PokeFood, PokeFoodDtoCreate>().ReverseMap();

        // Property
        CreateMap<Property, PropertyDto>().ReverseMap();
        CreateMap<Property, PropertyDtoUpdate>().ReverseMap();

        // PokeProperty
        CreateMap<PokeProperty, PokePropertyDto>()
           .ForMember(dest => dest.PokemonName, opt => opt.MapFrom(src => src.Pokemon.Name))
           .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(src => src.Property.Name));
        CreateMap<PokeProperty, PokePropertyDtoCreate>().ReverseMap();
        CreateMap<PokeProperty, PokePropertyDtoUpdate>().ReverseMap();

        // User
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ReverseMap();

        CreateMap<UserCreateDto, User>().ReverseMap();
        CreateMap<UserUpdateDto, User>().ReverseMap();

        // Role
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<RoleCreateDto, Role>().ReverseMap();

        // Permission
        CreateMap<Permission, PermissionDto>().ReverseMap();
        CreateMap<PermissionCreateDto, Permission>().ReverseMap();
    }
}
