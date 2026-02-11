using UnityEngine;

public class BrazerCollider : MonoBehaviour
{
    private bool inPlace = false;
    private bool alreadyLit = false;

    private void OnEnable()
    {
        SpellEvents.OnFirebreathUsed += OnFirebreathUsed;
    }
    private void OnDisable()
    {
        SpellEvents.OnFirebreathUsed -= OnFirebreathUsed;
    }

    private void OnFirebreathUsed()
    {
        if (inPlace && !alreadyLit)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            alreadyLit = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inPlace = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inPlace = false;
        }
    }
}
