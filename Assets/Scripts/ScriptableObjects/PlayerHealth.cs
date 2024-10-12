using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealth", menuName = "ScriptableObjects/PlayerHealth", order = 1)]
public class PlayerHealth : ScriptableObject
{
    public int maxHealth = 100;
    public int currentHealth;
}