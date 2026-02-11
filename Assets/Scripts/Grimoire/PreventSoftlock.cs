using UnityEngine;

public class PreventSoftlock : MonoBehaviour
{
    private bool retreatDiscovered = false;

    private void OnEnable()
    {
        GrimoireEvents.OnRetreatDiscovered += OnRetreatDiscovered;
    }

    private void OnRetreatDiscovered()
    {
        this.gameObject.SetActive(false);
    }
}
