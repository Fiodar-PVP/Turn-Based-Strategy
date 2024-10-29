using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private ParticleSystem bulletHitVFX;

    private Vector3 targetPosition;

    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float moveSpeed = 200f;

        float distanceBeforeMoving = Vector3.Distance(targetPosition, transform.position);

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(targetPosition, transform.position);

        //Check if we overshoot the target
        if(distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;

            trailRenderer.transform.parent = null;
            
            Instantiate(bulletHitVFX, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
