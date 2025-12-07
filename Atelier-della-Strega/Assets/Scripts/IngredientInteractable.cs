using UnityEngine;

public class IngredientInteractable : Interactable
{
    public Ingredient ingredient;

    public override void Interact(GameObject caller)
    {
        Debug.Log("Interacted with ingredient: " + ingredient.displayName);
    }
}
