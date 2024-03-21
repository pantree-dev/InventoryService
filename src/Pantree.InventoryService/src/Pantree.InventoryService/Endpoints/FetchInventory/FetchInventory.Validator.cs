using FastEndpoints;
using FluentValidation;

namespace Pantree.InventoryService.Endpoints.FetchInventory;

public sealed class FetchInventoryValidator : Validator<FetchInventoryRequest> {

    public FetchInventoryValidator() {
        RuleFor(x => x.Limit)
            .InclusiveBetween(10, 100)
            .WithMessage("Page size cannot be more than 100 entries and no less than 10.");
    }
}