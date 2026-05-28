using FluentValidation;
using Fourth.TradersTask.API.Constants;
using Fourth.TradersTask.Application.Models;

namespace Fourth.TradersTask.API.Validators;

/// <summary>
/// Validator for pagination parameters.
/// </summary>
public class QueryParametersValidator : AbstractValidator<GetCustomersQueryParameters>
{
    /// <summary>
    /// Constructor for QueryParametersValidator, sets up validation rules for pagination parameters.
    /// </summary>
    public QueryParametersValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage(ApiConstants.InvalidPageNumberMessage);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, ApiConstants.MaxPageSize)
            .WithMessage(ApiConstants.InvalidPageSizeMessage);
    }
}