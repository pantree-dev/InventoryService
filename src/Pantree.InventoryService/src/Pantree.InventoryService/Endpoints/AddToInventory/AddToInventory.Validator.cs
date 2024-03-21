using FastEndpoints;
using FluentValidation;

namespace Pantree.InventoryService.Endpoints.AddToInventory;

public sealed class AddToInventoryValidator : Validator<AddToInventoryRequest> {

    public AddToInventoryValidator() {
        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(1)
            .WithMessage("You must add at least 1 item to your inventory.");
    }
}