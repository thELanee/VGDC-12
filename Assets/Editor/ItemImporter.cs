using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Dynamic;
using System;

public class ItemImporter : EditorWindow
{
    public string csvFilePath = "Assets/Resources/Items - Sheet1.csv"; // Path to your CSV file
    public string itemFolderPath = "Assets/Resources/Items"; // Folder to save the items

    [MenuItem("Tools/Import Items")]
    public static void ShowWindow()
    {
        GetWindow<ItemImporter>("Import Items");
    }

    private void OnGUI()
    {
        GUILayout.Label("Item Importer", EditorStyles.boldLabel);

        // csvFilePath = EditorGUILayout.TextField("CSV File Path", csvFilePath);
        //itemFolderPath = EditorGUILayout.TextField("Item Folder Path", itemFolderPath);

        if (GUILayout.Button("Import Items"))
        {
            ImportItems();
        }
    }

    // Call this method to import items
    public void ImportItems()
    {
        // Read the CSV file
        string[] csvLines = File.ReadAllLines(csvFilePath);

        for (int i = 1; i < csvLines.Length; i++) // Start at 1 to skip header
        {
            string[] lineData = csvLines[i].Split(',');

            if (lineData.Length < 4) continue; // Skip lines with insufficient data

            Item newItem = null;
            ItemType parsedItemType;

            // Parse item type
            if (Enum.TryParse(lineData[3].Trim(), true, out parsedItemType))
            {
                // Check if the item already exists
                string itemName = lineData[0].Trim();
                string itemPath = $"{itemFolderPath}/{itemName}.asset";
                newItem = AssetDatabase.LoadAssetAtPath<Item>(itemPath);

                if (newItem == null) // Item does not exist, create a new one
                {
                    switch (parsedItemType)
                    {
                        case ItemType.Weapon:
                            newItem = CreateInstance<Weapon>();
                            ((Weapon)newItem).attackStat = int.Parse(lineData[4].Trim());
                            break;

                        case ItemType.Consumable:
                            newItem = CreateInstance<Consumable>();
                            ((Consumable)newItem).healthRecovered = int.Parse(lineData[5].Trim());
                            ((Consumable)newItem).sanRecovered = int.Parse(lineData[6].Trim());
                            break;

                        case ItemType.KeyItem:
                            break;

                        default:
                            Debug.LogWarning($"Unsupported item type '{parsedItemType}' for item '{itemName}'.");
                            continue; // Skip unsupported types
                    }

                    // Set common properties
                    newItem.itemName = itemName;
                    newItem.itemID = int.Parse(lineData[1].Trim());
                    newItem.description = lineData[2].Trim();
                    newItem.itemType = parsedItemType;

                    // Save the new item as a ScriptableObject
                    AssetDatabase.CreateAsset(newItem, itemPath);
                }
            }
            else
            {
                Debug.LogWarning($"Invalid item type '{lineData[3].Trim()}' for item '{lineData[0].Trim()}'.");
            }
        }

        // Save changes to the asset database
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
