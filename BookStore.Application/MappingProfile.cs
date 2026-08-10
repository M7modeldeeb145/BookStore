using AutoMapper;
using BookStore.Application.Dtos;
using BookStore.Domain.Entities;

namespace BookStore.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>();
        }
    }
}
