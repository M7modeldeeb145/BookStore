using AutoMapper;
using BookStore.Application.Dtos;
using BookStore.Domain.Entities;

namespace BookStore.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>();
        }
    }
}
