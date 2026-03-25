using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    
    private const string Is_Walking = "IsWalking";
    [SerializeField] private PlayerMovement player;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    private void Update()
    {
        animator.SetBool(Is_Walking, player.IsWalking());
    }
}
 