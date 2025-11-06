using FluentValidation;
using MillionApi.Domain.AggregateModel;

namespace MillionApi.Application.Validators;

public class GetPropertiesRequestValidator : AbstractValidator<GetPropertiesRequest>
{
	private const int MaxStringLength = 250;

	private const int MinPageNumber = 1;

	private const int MinPageSize = 1;

	private const int MaxPageSize = 100;

	public GetPropertiesRequestValidator()
	{
		When((GetPropertiesRequest x) => !string.IsNullOrEmpty(x.Name), delegate
		{
			RuleFor((GetPropertiesRequest x) => x.Name).MaximumLength(250).WithMessage($"Name must not exceed {250} characters.").Matches("^[a-zA-Z0-9\\s]*$")
				.WithMessage("Name must contain only alphanumeric characters and spaces.");
		});
		When((GetPropertiesRequest x) => !string.IsNullOrEmpty(x.Address), delegate
		{
			RuleFor((GetPropertiesRequest x) => x.Address).MaximumLength(250).WithMessage($"Address must not exceed {250} characters.").Matches("^[a-zA-Z0-9\\s]*$")
				.WithMessage("Address must contain only alphanumeric characters and spaces.");
		});
		When((GetPropertiesRequest x) => !string.IsNullOrEmpty(x.MinPrice), delegate
		{
			RuleFor((GetPropertiesRequest x) => x.MinPrice).Must(BeAValidPositiveDecimal).WithMessage("MinPrice must be a valid positive decimal number.");
		});
		When((GetPropertiesRequest x) => !string.IsNullOrEmpty(x.MaxPrice), delegate
		{
			RuleFor((GetPropertiesRequest x) => x.MaxPrice).Must(BeAValidPositiveDecimal).WithMessage("MaxPrice must be a valid positive decimal number.");
		});
		When((GetPropertiesRequest x) => !string.IsNullOrEmpty(x.MinPrice) && !string.IsNullOrEmpty(x.MaxPrice), delegate
		{
			RuleFor((GetPropertiesRequest x) => x).Must((GetPropertiesRequest x) => ValidatePriceRange(x.MinPrice, x.MaxPrice)).WithMessage("MinPrice must be less than or equal to MaxPrice.");
		});
		RuleFor((GetPropertiesRequest x) => x.PageNumber).GreaterThanOrEqualTo(1).WithMessage($"PageNumber must be greater than or equal to {1}.");
		RuleFor((GetPropertiesRequest x) => x.PageSize).GreaterThanOrEqualTo(1).WithMessage($"PageSize must be greater than or equal to {1}.").LessThanOrEqualTo(100)
			.WithMessage($"PageSize must be less than or equal to {100}.");
	}

	private bool BeAValidPositiveDecimal(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return false;
		}
		if (!decimal.TryParse(value, out var result))
		{
			return false;
		}
		return result >= 0m;
	}

	private bool ValidatePriceRange(string minPrice, string maxPrice)
	{
		if (decimal.TryParse(minPrice, out var result) && decimal.TryParse(maxPrice, out var result2))
		{
			return result <= result2;
		}
		return true;
	}
}
