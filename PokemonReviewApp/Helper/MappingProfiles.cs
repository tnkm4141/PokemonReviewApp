using AutoMapper;
using PokemonReviewApp.Dtos.CategoryDtos;
using PokemonReviewApp.Dtos.CountryDtos;
using PokemonReviewApp.Dtos.OwnerDtos;
using PokemonReviewApp.Dtos.PokemonDtos;
using PokemonReviewApp.Dtos.ReviewDtos;
using PokemonReviewApp.Dtos.ReviewerDtos;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {

            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<Category,CategoryCreateDto>().ReverseMap();
            CreateMap<Category,CategoryUpdateDto>().ReverseMap();
            CreateMap<Category,CategoryDeleteDto>().ReverseMap();

            CreateMap<Pokemon,PokemonDto>().ReverseMap();
            CreateMap<Pokemon,PokemonCreateDto>().ReverseMap();
            CreateMap<Pokemon,PokemonUpdateDto>().ReverseMap();
            CreateMap<Pokemon,PokemonDeleteDto>().ReverseMap();
            
            CreateMap<Country,CountryDto>().ReverseMap();
            CreateMap<Country,CountryCreateDto>().ReverseMap();
            CreateMap<Country,CountryUpdateDto>().ReverseMap();
            CreateMap<Country,CountryDeleteDto>().ReverseMap();
            
            CreateMap<Owner,OwnerDto>().ReverseMap();
            CreateMap<Owner,OwnerCreateDto>().ReverseMap();
            CreateMap<Owner,OwnerUpdateDto>().ReverseMap();
            CreateMap<Owner,OwnerDeleteDto>().ReverseMap();
           
            CreateMap<Review, ReviewDto>().ReverseMap();
            CreateMap<Review, ReviewCreateDto>().ReverseMap();
            CreateMap<Review, ReviewUpdateDto>().ReverseMap();
            CreateMap<Review, ReviewDeleteDto>().ReverseMap();

            CreateMap<Reviewer, ReviewerDto>().ReverseMap();
            CreateMap<Reviewer, ReviewerCreateDto>().ReverseMap();
            CreateMap<Reviewer, ReviewerUpdateDto>().ReverseMap();
            CreateMap<Reviewer, ReviewerDeleteDto>().ReverseMap();
            


        }

    }
}
