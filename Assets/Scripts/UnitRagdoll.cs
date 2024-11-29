using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;

    public void Setup(Transform originalRootBone)
    {
        MatchAllChildTransform(originalRootBone, ragdollRootBone);

        float explosionForce = 300f;
        float explosionRadius = 10f;
        Vector3 explosionOffset = new Vector3(Random.Range(1f, -1f), 0, Random.Range(1f, -1f));
        ApplyExplosionToRagdoll(ragdollRootBone, explosionForce, transform.position + explosionOffset, explosionRadius);
    }

    private void MatchAllChildTransform(Transform originalRootBone, Transform cloneRootBone)
    {
        foreach(Transform child in originalRootBone)
        {
            Transform clone = cloneRootBone.Find(child.name);
            if (clone != null)
            {
                clone.transform.position = child.position;
                clone.transform.rotation = child.rotation;

                MatchAllChildTransform(child, clone);
            }
        }
    }

    private void ApplyExplosionToRagdoll(Transform rootBone, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        foreach (Transform child in rootBone)
        {
            if(child.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }

            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRadius);
        }
    }
}
