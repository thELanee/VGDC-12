using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrabbableItem : Interactable
{
    public Item item; // This is the item to be picked up
    public int amount = 1; // Quantity of the item to be added
    public int chestID; // Keep track of whether this item has been claimed or not

    private void Start()
    {
        // Check if this item has already been picked up
        if (GameManager.Instance.IsChestEmpty(chestID, SceneManager.GetActiveScene().name))
        {
            Destroy(gameObject); // Destroy the item if it has already been claimed
        }
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerIsClose)
        {
            // To play the next line, must have dialogue panel open, must have stopped typing, and must not be expecting a response
            if (DialogueManager.Instance.dialoguePanel.activeSelf && !DialogueManager.Instance.isTyping && !DialogueManager.Instance.expectingResponse)
            {
                DialogueManager.Instance.NextLine();
            }
            else if (!DialogueManager.Instance.dialoguePanel.activeSelf)
            {
                Time.timeScale = 0;
                DialogueManager.Instance.ShowDialogue();

                // Use PlayerManager to add the item to the player's inventory
                if (PlayerManager.Instance != null)
                {
                    PlayerManager.Instance.AddItemToInventory(item, amount);
                    Debug.Log($"Added {amount} of {item.itemName} to inventory.");
                }
                else
                {
                    Debug.LogWarning("PlayerManager instance not found!");
                }

                currentDialogueNode = CreateDialogueNode("You found " + item.itemName + "!");
                GameManager.Instance.SetChestEmpty(chestID, SceneManager.GetActiveScene().name);
                DialogueManager.Instance.Typing(currentDialogueNode);

                if (currentDialogueNode.nextNode == null)
                {
                    StartCoroutine(HandleDialogueAndDestroy());
                }
            }
        }
    }

    private IEnumerator HandleDialogueAndDestroy()
    {
        // Wait for the Typing coroutine to finish typing
        yield return StartCoroutine(DialogueManager.Instance.Typing(currentDialogueNode));

        // Wait for dialogue to be closed by the player
        while (DialogueManager.Instance.dialoguePanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DialogueManager.Instance.closeDialogue(); 
                break;
            }
            yield return null; // Wait for the next frame
        }
        Destroy(gameObject); // Destroy the item after it has been picked up and dialogue is finished
    }
}