using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // Make this class serializable for Unity Inspector
public class Item : ScriptableObject
{
    public string itemName;
    public int itemID;
    public Sprite itemIcon; 
    public string description;
    public ItemType itemType;


    // This method will automatically assign an itemID based on the number of items in the "Items" folder
    // Static method to assign itemID
    public static void SetItemID(Item item)
    {
        // Load all items in the "Items" folder
        Object[] allItems = Resources.LoadAll("Items", typeof(Item));
        item.itemID = allItems.Length;
    }
}
public enum ItemType
{
    Weapon,
    Consumable,
    KeyItem,
}