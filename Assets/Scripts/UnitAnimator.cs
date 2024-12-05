using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    public const string IS_WALKING = "IsWalking";
    public const string SWORD_SLASH = "SwordSlash";
    public const string SHOOT = "Shoot";

    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;
    [SerializeField] private Transform rifleTransform;
    [SerializeField] private Transform swordTransform;

    private void Start()
    {
        EquipRifle();

        if (TryGetComponent(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }

        if (TryGetComponent(out SwordAction swordAction))
        {
            swordAction.OnSwordSlashStarted += SwordAction_OnSwordSlashStarted;
            swordAction.OnSwordSlashCompleted += SwordAction_OnSwordSlashCompleted;
        }
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger(SHOOT);

        Transform bulletProjectileTransfrom = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransfrom.GetComponent<BulletProjectile>();

        Vector3 targetShootPosition = e.targetUnit.GetWorldPosition();

        //Adjust Target Shoot Position so we don't shoot at target unit's feet
        targetShootPosition.y = shootPointTransform.position.y;
               
        bulletProjectile.SetTargetPosition(targetShootPosition);
    }

    private void MoveAction_OnStopMoving(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_WALKING, false);
    }

    private void MoveAction_OnStartMoving(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_WALKING, true);
    }

    private void SwordAction_OnSwordSlashCompleted(object sender, System.EventArgs e)
    {
        EquipRifle();
    }

    private void SwordAction_OnSwordSlashStarted(object sender, System.EventArgs e)
    {
        EquipSword();
        animator.SetTrigger(SWORD_SLASH);
    }

    private void EquipRifle()
    {
        rifleTransform.gameObject.SetActive(true);
        swordTransform.gameObject.SetActive(false);
    }

    private void EquipSword()
    {
        rifleTransform.gameObject.SetActive(false);
        swordTransform.gameObject.SetActive(true);
    }
}
