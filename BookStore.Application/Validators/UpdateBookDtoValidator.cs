using FluentValidation;
using BookStore.Application.Dtos;

namespace BookStore.Application.Validators
{
    public class UpdateBookDtoValidator : AbstractValidator<UpdateBookDto>
    {
        public UpdateBookDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than zero");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(250).WithMessage("Title must not exceed 250 characters");

            RuleFor(x => x.TotalCopies)
                .GreaterThanOrEqualTo(0).WithMessage("TotalCopies must be zero or greater");
        }
    }
}
