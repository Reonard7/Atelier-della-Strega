using UnityEngine;

public class MageWalking : MonoBehaviour
{
    [SerializeField] private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (animator == null) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("isWalking", true);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetBool("isWalking", false);
        }
    }
}
