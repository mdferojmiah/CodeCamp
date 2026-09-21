using FluentValidation;
using learning_validation_mediatr.Dtos;

namespace learning_validation_mediatr.Validators;

public class CreateProductRequestValidator: AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("[Fluentvalidation] Name can not be empty!")
            .MaximumLength(10).WithMessage("[Fluentvalidation] name can not exceed 10 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("[Fluentvalidation] Price can not be zero.");
    }
}