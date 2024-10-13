using UnityEngine;

public class GrabbableItem : MonoBehaviour
{
    public Item item; // This is the item to be picked up
    public int amount = 1; // Quantity of the item to be added

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collides with the item
        if (other.CompareTag("Player"))
        {
            // Access the player's inventory and add the item
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.AddItem(item, amount);
                Debug.Log($"Picked up {amount} of {item.itemName}");
                Destroy(gameObject); // Destroy the item GameObject after it's picked up
            }
        }
    }
}