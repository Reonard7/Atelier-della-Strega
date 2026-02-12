using Events;
using GameData.Scripts.Items;
using Interaction;
using UnityEngine;

public class TreasureInteractable : MonoBehaviour, IInteractable
{
     
    //[SerializeField] private Animator animator;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }
    
    public void Interact(GameObject caller)
    {
        animator.SetTrigger("open");
    }
}
