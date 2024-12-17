using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform originalRootTransform;
    [SerializeField] private Transform unitRagdollPrefab;
    
    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDie += HealthSystem_OnDie;
    }
    
    private void HealthSystem_OnDie(object sender, System.EventArgs e)
    {
        Transform unitRagdollTransform = Instantiate(unitRagdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = unitRagdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootTransform);
    }
}
