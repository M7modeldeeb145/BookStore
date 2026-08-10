using FluentValidation;
using BookStore.Application.Dtos;

namespace BookStore.Application.Validators
{
    public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
    {
        public CreateBookDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(250).WithMessage("Title must not exceed 250 characters");

            RuleFor(x => x.TotalCopies)
                .GreaterThanOrEqualTo(0).WithMessage("TotalCopies must be zero or greater");
        }
    }
}
