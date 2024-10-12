using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct ChestItem
{
    public Item item; // The item to be given
    public int quantity; // The quantity of the item
}

public class Chest : Interactable
{
    private bool empty;
    public bool refillable = false;
    public List<ChestItem> itemPool;

    public int chestID;

    void Start()
    {
        if (!refillable)
        {
            empty = GameManager.Instance.IsChestEmpty(chestID, SceneManager.GetActiveScene().name);
        }
        else
        {
            empty = false;
        }
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerIsClose)
        {
            // To play the next line, must have dialogue panel open, must have stopped typing and must not be expecting a response
            if (DialogueManager.Instance.dialoguePanel.activeSelf && !DialogueManager.Instance.isTyping && !DialogueManager.Instance.expectingResponse)
            {
                DialogueManager.Instance.NextLine();
            }
            else if (!DialogueManager.Instance.dialoguePanel.activeSelf && !empty)
            {
                Time.timeScale = 0;
                DialogueManager.Instance.ShowDialogue();
                GiveRandomItem();
            }
        }
    }

    private void GiveRandomItem()
    {
        // Check if the item pool is not empty
        if (itemPool.Count > 0)
        {
            // Get a random item from the item pool
            ChestItem randomChestItem = itemPool[Random.Range(0, itemPool.Count)];

            if (randomChestItem.quantity > 1)
            {
                currentDialogueNode = CreateDialogueNode("You found " + randomChestItem.item.itemName + " x" + randomChestItem.quantity + "!");
            }
            else
            {
                currentDialogueNode = CreateDialogueNode("You found " + randomChestItem.item.itemName + "!");
            }

            // Use the PlayerManager to add the item to the player's inventory
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.AddItemToInventory(randomChestItem.item, randomChestItem.quantity);
            }
            else
            {
                Debug.LogWarning("PlayerManager instance not found!");
            }

            DialogueManager.Instance.TypingS(currentDialogueNode);
            empty = true;

            if (!refillable)
            {
                GameManager.Instance.SetChestEmpty(chestID, SceneManager.GetActiveScene().name);
            }
        }
    }
}
