using System;
using UnityEngine;

public static class InteractionEvents
{
    public static Action<Ingredient> OnIngredientPickup;
    public static Action OnIngredientDiscard;
}
