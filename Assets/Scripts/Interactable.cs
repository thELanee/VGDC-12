using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public DialogueNode startNode; // The dialogue we want to start from whenever the player interacts with the NPC
    protected DialogueNode currentDialogueNode;
    public bool playerIsClose;
    protected virtual void Start()
    {
        // Optional: Initialization code here if needed
    }

    protected virtual void Update()
    {
        // Check for player interaction input
        if (Input.GetKeyDown(KeyCode.Space) && playerIsClose)
        {
            // If the dialogue panel is active and not currently typing or expecting a response, move to the next line
            if (DialogueManager.Instance.dialoguePanel.activeSelf && !DialogueManager.Instance.isTyping && !DialogueManager.Instance.expectingResponse)
            {
                DialogueManager.Instance.NextLine();
                this.currentDialogueNode = DialogueManager.Instance.currentDialogueNode;
                this.startNode = DialogueManager.Instance.startNode;
            }
            else if (!DialogueManager.Instance.dialoguePanel.activeSelf)
            {
                Time.timeScale = 0; // Pause game time
                DialogueManager.Instance.startNode = startNode;
                DialogueManager.Instance.ShowDialogue();
                DialogueManager.Instance.TypingS(currentDialogueNode); // Start typing coroutine
            }
        }
    }

    protected DialogueNode CreateDialogueNode(string dialogueText, bool p = false, bool b = false, List<DialogueOption> options = null, DialogueNode nn = null)
    {
        DialogueNode node = ScriptableObject.CreateInstance<DialogueNode>();
        node.dialogue = dialogueText;
        node.pause = p;
        node.bookmark = b;
        node.options ??= new List<DialogueOption>();
        node.nextNode ??= nn;
        return node;
    }
}