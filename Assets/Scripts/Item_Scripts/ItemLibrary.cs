using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemLibrary", menuName = "Inventory/ItemLibrary")]
public class ItemLibrary : ScriptableObject
{
    public List<Item> allItems;

    // Find an item by its name or ID
    public Item GetItemByName(string itemName)
    {
        return allItems.Find(item => item.itemName == itemName);
    }
    public Item GetItemByID(int itemID)
    {
        return allItems.Find(item => item.itemID == itemID);
    }
    private void OnEnable()
    {
        LoadAllItems();
    }
    private void LoadAllItems()
    {
        // Clear the list before loading to prevent duplicates
        allItems.Clear();

        // Load all items in the "Items" folder
        Item[] loadedItems = Resources.LoadAll<Item>("Items");

        // Add the loaded items to the allItems list
        allItems.AddRange(loadedItems);
    }
}

