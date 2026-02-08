using UnityEngine;

public class MageController : MonoBehaviour
{
    [SerializeField] private Transform downstairsPoint;

    void Update()
    {
        // DEBUG: premi T per testare il teletrasporto
        if (Input.GetKeyDown(KeyCode.T))
        {
            TeleportDownstairs();
        }
    }

    public void TeleportDownstairs()
    {
        transform.position = downstairsPoint.position;
        Debug.Log("Maga teletrasportata al piano di sotto");
    }
}
