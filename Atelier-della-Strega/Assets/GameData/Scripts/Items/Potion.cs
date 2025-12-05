using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "GameData/Potion")]
public class Potion : ScriptableObject
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
}
