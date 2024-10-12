using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Inventory inventory; // Reference to the Inventory component

    public void AddItem(Item item, int amount)
    {
        inventory.AddItem(item, amount);
        Debug.Log($"Added {amount} of {item.itemName} to inventory.");
    }
}
