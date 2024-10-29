using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    public const string IS_WALKING = "IsWalking";
    public const string SHOOT = "Shoot";

    [SerializeField] private Animator animator;

    private void Start()
    {
        if(TryGetComponent(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if(TryGetComponent(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
    }

    private void ShootAction_OnShoot(object sender, System.EventArgs e)
    {
        animator.SetTrigger(SHOOT);
    }

    private void MoveAction_OnStopMoving(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_WALKING, false);
    }

    private void MoveAction_OnStartMoving(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_WALKING, true);
    }
}
