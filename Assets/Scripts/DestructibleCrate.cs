using System;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    public static event EventHandler OnAnyCrateDestruction;

    [SerializeField] private Transform destroyedCratePrefab;

    private GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public void Damage()
    {
        Transform destroyedCrateTranform = Instantiate(destroyedCratePrefab, transform.position, transform.rotation);

        float explosionForce = 150f;
        float explosionRadius = 10f;
        ApplyExplosionToRagdoll(destroyedCrateTranform, explosionForce, transform.position, explosionRadius);

        Destroy(gameObject);
        OnAnyCrateDestruction?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition() => gridPosition;

    private void ApplyExplosionToRagdoll(Transform rootTransform, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        foreach (Transform child in rootTransform)
        {
            if (child.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }

            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRadius);
        }
    }
}
