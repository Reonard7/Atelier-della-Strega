using UnityEngine;

namespace GameData.Scripts.Items
{
    [CreateAssetMenu(fileName = "Ingredient", menuName = "GameData/Ingredient")]
    public class Ingredient : ScriptableObject, IGrimoireData
    {
        [Header("General")]
        public string id;
        public string displayName;

        [TextArea(5, 5)]
        public string description;

        public Sprite icon;
        public GameObject prefab;

        [Header("Ingredient")]
        public Potion[] craftablePotions;

        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;
    }
}
