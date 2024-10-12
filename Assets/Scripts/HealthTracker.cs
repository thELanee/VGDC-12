using UnityEngine;

public class PlayerHealthTracker : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerManager.OnHealthChanged += LogHealthChange;
    }

    private void OnDisable()
    {
        PlayerManager.OnHealthChanged -= LogHealthChange;
    }

    private void LogHealthChange(int currentHealth, int maxHealth)
    {
        Debug.Log($"Health updated: {currentHealth}/{maxHealth}");
    }
}
