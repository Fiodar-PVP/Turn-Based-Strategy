using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    private Action OnGrenadeBehaviorComplete;
    private Vector3 targetPosition;

    private void Update()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float movespeed = 15f;
        transform.position += moveDirection * movespeed * Time.deltaTime;

        float reachedTargetPosition = 0.2f;
        if(Vector3.Distance(targetPosition, transform.position) < reachedTargetPosition)
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

            Destroy(gameObject);

            OnGrenadeBehaviorComplete();
        }
    }

    public void Setup(GridPosition targetGridPosition, Action OnGrenadeBehaviorComplete)
    {
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        this.OnGrenadeBehaviorComplete = OnGrenadeBehaviorComplete;
    }
}
