using System;
using GameData.Scripts.Items;
using UnityEngine;

namespace Events
{
    public static class InteractionEvents
    {
        public static Action<Ingredient> OnIngredientPickup;
        public static Action OnIngredientDiscard;
    }
}
