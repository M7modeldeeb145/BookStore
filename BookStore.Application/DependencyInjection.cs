using BookStore.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using BookStore.Application.Dtos;
using BookStore.Application.Validators;
using BookStore.Application.IServices;

namespace BookStore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Application services
            services.AddScoped<IBookService, BookService>();
            // Register AutoMapper profiles from this assembly
            services.AddAutoMapper(typeof(MappingProfile));
            // Register FluentValidation validator(s)
            services.AddScoped<FluentValidation.IValidator<CreateBookDto>, CreateBookDtoValidator>();
            services.AddScoped<FluentValidation.IValidator<UpdateBookDto>, UpdateBookDtoValidator>();
            return services;
        }
    }
}
