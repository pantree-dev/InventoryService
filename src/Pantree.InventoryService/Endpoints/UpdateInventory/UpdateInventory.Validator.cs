using FastEndpoints;
using FluentValidation;

namespace Pantree.InventoryService.Endpoints.UpdateInventory;

public sealed class UpdateInventoryValidator : Validator<UpdateInventoryRequest> {

    public UpdateInventoryValidator() {
        RuleFor(x => x.Amount)
            .NotEqual(0)
            .WithMessage("You must add or subtract at least 1 from your inventory.");
    }
}