using UnityEngine;

public class TestHealth : MonoBehaviour
{
    public PlayerManager healthManager; // Reference to the HealthManager
    private void Start()
    {
        // Find the HealthManager in the scene
        healthManager = FindObjectOfType<PlayerManager>();

        if (healthManager == null)
        {
            Debug.LogError("No HealthManager found in the scene!");
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // Press D to take damage
        {
            healthManager.TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.E)) // Press H to heal
        {
            healthManager.Heal(10);
        }
    }
}