using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    private bool empty;
    public List<Item> itemPool; 
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerIsClose)
        {
            // To play the next line, must have dialogue panel open, must have stopped typing and must not be expecting a response
            if (DialogueManager.Instance.dialoguePanel.activeSelf && !DialogueManager.Instance.isTyping && !DialogueManager.Instance.expectingResponse)
            {
                DialogueManager.Instance.NextLine();
            }
            else if (!DialogueManager.Instance.dialoguePanel.activeSelf)
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
        empty = true;
        if (itemPool.Count > 0)
        {
            // Get a random item from the item pool
            Item randomItem = itemPool[Random.Range(0, itemPool.Count)];

            // Display the item name in the dialogue
            currentDialogueNode = CreateDialogueNode("You found " + randomItem.itemName + "!");

            DialogueManager.Instance.TypingS(currentDialogueNode);
        }
    }
}
