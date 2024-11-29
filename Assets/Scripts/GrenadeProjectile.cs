using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static EventHandler OnAnyGrenadeExploded;

    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private ParticleSystem grenadeExplosionVFX;
    [SerializeField] private AnimationCurve arcYAnimationCurve;

    private Action OnGrenadeBehaviorComplete;
    private Vector3 targetPosition;
    private float totalDistance;
    private Vector3 positionXZ;

    private void Update()
    {
        Vector3 moveDirection = (targetPosition - positionXZ).normalized;
        float movespeed = 15f;
        positionXZ += moveDirection * movespeed * Time.deltaTime;

        float distance = Vector3.Distance(targetPosition, positionXZ);
        float distanceNormalized = 1 - distance / totalDistance;

        float maxHeight = totalDistance / 4f;
        float pozitionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, pozitionY, positionXZ.z);

        float reachedTargetPosition = 0.2f;
        if(Vector3.Distance(targetPosition, positionXZ) < reachedTargetPosition)
        {
            float damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);
            foreach (Collider collider in colliderArray)    
            {
                if(collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    int grenadeDamage = 30;
                    targetUnit.Damage(grenadeDamage);
                }
            }

            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);

            Instantiate(grenadeExplosionVFX, targetPosition + Vector3.up * 1f, Quaternion.identity);

            trailRenderer.transform.parent = null;
            
            Destroy(gameObject);

            OnGrenadeBehaviorComplete();
        }
    }

    public void Setup(GridPosition targetGridPosition, Action OnGrenadeBehaviorComplete)
    {
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        this.OnGrenadeBehaviorComplete = OnGrenadeBehaviorComplete;

        positionXZ = transform.position;
        positionXZ.y = 0;

        totalDistance = Vector3.Distance(targetPosition, positionXZ);
    }
}
