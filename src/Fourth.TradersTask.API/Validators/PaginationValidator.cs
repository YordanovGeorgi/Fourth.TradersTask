using FluentValidation;
using Fourth.TradersTask.Application.Models;
using Fourth.TradersTask.API.Constants;

namespace Fourth.TradersTask.API.Validators;

/// <summary>
/// Validator for pagination parameters.
/// </summary>
public class PaginationParamsValidator : AbstractValidator<PaginationParams>
{
    public PaginationParamsValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage(ApiConstants.InvalidPageNumberMessage);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, ApiConstants.MaxPageSize)
            .WithMessage(ApiConstants.InvalidPageSizeMessage);
    }
}

/// <summary>
/// Validator for customer ID.
/// </summary>
public class CustomerIdValidator : AbstractValidator<string>
{
    public CustomerIdValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithMessage(ApiConstants.EmptyCustomerIdMessage)
            .NotNull()
            .WithMessage(ApiConstants.EmptyCustomerIdMessage);
    }
}
