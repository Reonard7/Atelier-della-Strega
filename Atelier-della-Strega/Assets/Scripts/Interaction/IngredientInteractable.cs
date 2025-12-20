using UnityEngine;

public class IngredientInteractable : Interactable
{
    public Ingredient ingredient;

    public override void Interact(GameObject caller)
    {
        InteractionEvents.OnIngredientPickup?.Invoke(ingredient);
    }
}
