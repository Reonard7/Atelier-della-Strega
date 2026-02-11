using UnityEngine;

public class BrazerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Firebreath"))
        {
            Debug.Log("true");
            this.GetComponentInChildren<GameObject>().SetActive(true);
        }
    }
}
