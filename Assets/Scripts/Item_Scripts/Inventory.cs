using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemSlot
{
    public Item item;
    public int quantity;

    public ItemSlot(Item newItem, int newQuantity)
    {
        item = newItem;
        quantity = newQuantity;
    }
}

[CreateAssetMenu(fileName = "NewInventory", menuName = "Inventory/New Inventory")]
public class Inventory : ScriptableObject
{
    public List<ItemSlot> itemSlots = new List<ItemSlot>();

    // Add an item to the inventory
    public void AddItem(Item item, int amount)
    {
        var slot = itemSlots.Find(i => i.item == item);
        if (slot.item != null)
        {
            // Increase the quantity if item already exists
            int index = itemSlots.IndexOf(slot);
            itemSlots[index] = new ItemSlot(slot.item, slot.quantity + amount);
        }
        else
        {
            // Add a new item if it doesn't exist
            itemSlots.Add(new ItemSlot(item, amount));
        }
    }

    // Remove an item from the inventory
    public void RemoveItem(Item item, int amount)
    {
        var slot = itemSlots.Find(i => i.item == item);
        if (slot.item != null && slot.quantity >= amount)
        {
            int index = itemSlots.IndexOf(slot);
            int newQuantity = slot.quantity - amount;

            if (newQuantity > 0)
            {
                itemSlots[index] = new ItemSlot(slot.item, newQuantity);
            }
            else
            {
                // Remove the item slot if the quantity drops to 0
                itemSlots.RemoveAt(index);
            }
        }
    }

    public void PrintInventory()
    {
        if (itemSlots.Count == 0)
        {
            Debug.Log("Inventory is empty.");
            return;
        }

        Debug.Log("Inventory Items:");
        foreach (var slot in itemSlots)
        {
            Debug.Log($"{slot.item.itemName}: {slot.quantity}");
        }
    }
}