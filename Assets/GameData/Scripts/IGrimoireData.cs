using UnityEngine;

public interface IGrimoireData
{
    string Id { get; }
    string DisplayName { get; }
    string Description { get; }
    Sprite Icon { get; }
}

