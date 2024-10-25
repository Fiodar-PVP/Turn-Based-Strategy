using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    public const string IS_WALKING = "IsWalking";
    [SerializeField] private Unit unit;
    [SerializeField] private Animator animator;

    private void Update()
    {
        animator.SetBool(IS_WALKING, unit.GetMoveAction().IsWalking);
    }
}
