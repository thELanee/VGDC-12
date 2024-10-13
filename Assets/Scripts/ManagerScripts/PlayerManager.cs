using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerResource playerHealth;
    public PlayerResource playerSan;
    public PlayerResource playerHunger;
    public Inventory playerInventory;

    public static event Action<float, float> OnHealthChanged; // Sends currentHealth and maxHealth

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
            playerHealth.current = playerHealth.max; // Initialize health
            BroadcastHealthChange();
        }
        if (playerSan != null)
        {
            playerSan.current = playerSan.max; // Initialize health
        }
        if (playerHunger != null)
        {
            playerHunger.current = playerHunger.max; // Initialize health
            StartCoroutine(DecreaseHungerOverTime()); // Start the hunger decrease coroutine
        }
    }

    // Coroutine to decrease hunger over time
    private System.Collections.IEnumerator DecreaseHungerOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); // Change this to however often you want to decrease hunger

            if (playerHunger.current > 0)
            {
                playerHunger.current -= 1; // Change the amount to decrease hunger per tick
                if (playerHunger.current < 0) playerHunger.current = 0;

                Debug.Log($"Hunger decreased: {playerHunger.current}/{playerHunger.max}");
            }
        }
    }
    // Method to take damage and reduce player health
    public void TakeDamage(int damage)
    {
        playerHealth.current -= damage;
        if (playerHealth.current < 0)
            playerHealth.current = 0;

        BroadcastHealthChange();
    }

    // Method to heal the player
    public void Heal(int amount)
    {
        playerHealth.current += amount;
        if (playerHealth.current > playerHealth.max)
            playerHealth.current = playerHealth.max;

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
        OnHealthChanged?.Invoke(playerHealth.current, playerHealth.max);
        Debug.Log($"Health changed: {playerHealth.current}/{playerHealth.max}");
    }
}