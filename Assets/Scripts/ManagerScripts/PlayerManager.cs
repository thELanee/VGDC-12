using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Inventory playerInventory;  // Use this reference for the player's inventory

    public static event Action<int, int> OnHealthChanged; // Sends currentHealth and maxHealth

    private static PlayerManager instance;

    public static PlayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("PlayerManager is not initialized.");
            }
            return instance;
        }
    }

    private void Awake()
    {
        // Ensure this instance persists across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void Start()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("PlayerInventory is not assigned in PlayerManager. Please assign it in the Inspector.");
            return;
        }

        // Initialize health or other player states as needed
        if (playerHealth != null)
        {
            playerHealth.currentHealth = playerHealth.maxHealth; // Initialize health
            BroadcastHealthChange();
        }
    }
    // Method to take damage and reduce player health
    public void TakeDamage(int damage)
    {
        playerHealth.currentHealth -= damage;
        if (playerHealth.currentHealth < 0)
            playerHealth.currentHealth = 0;

        BroadcastHealthChange();
    }

    // Method to heal the player
    public void Heal(int amount)
    {
        playerHealth.currentHealth += amount;
        if (playerHealth.currentHealth > playerHealth.maxHealth)
            playerHealth.currentHealth = playerHealth.maxHealth;

        BroadcastHealthChange();
    }

    // Method to add item to the player's inventory
    public void AddItemToInventory(Item item, int amount)
    {
        if (playerInventory != null)
        {
            playerInventory.AddItem(item, amount);
            playerInventory.PrintInventory();
        }
        else
        {
            Debug.LogWarning("PlayerInventory is not assigned in PlayerManager.");
        }
    }

    // Method to remove item from the player's inventory
    public void RemoveItemFromInventory(Item item, int amount)
    {
        if (playerInventory != null)
        {
            playerInventory.RemoveItem(item, amount);
        }
        else
        {
            Debug.LogWarning("PlayerInventory is not assigned in PlayerManager.");
        }
    }

    // Broadcasts the health status change
    private void BroadcastHealthChange()
    {
        OnHealthChanged?.Invoke(playerHealth.currentHealth, playerHealth.maxHealth);
        Debug.Log($"Health changed: {playerHealth.currentHealth}/{playerHealth.maxHealth}");
    }
}
