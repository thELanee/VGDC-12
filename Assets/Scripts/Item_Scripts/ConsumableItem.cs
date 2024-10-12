using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumable", menuName = "Inventory/Consumable")]
public class Consumable : Item
{
    public int healthRecovered; // Unique to consumables
    public int sanRecovered;   // Additional consumable-specific property
}
