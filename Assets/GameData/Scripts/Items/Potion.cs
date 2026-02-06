using UnityEngine;

namespace GameData.Scripts.Items
{
    [CreateAssetMenu(fileName = "Potion", menuName = "GameData/Potion")]
    public class Potion : ScriptableObject, IGrimoireData
    {
        [Header("General")]
        public string id;
        public string displayName;

        [TextArea(5, 5)]
        public string description;

        public Sprite icon;
        public GameObject prefab;

        [Header("Potion")]
        public Ingredient[] potionRecipe;

        public string Id => id;
        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;
    }
}
