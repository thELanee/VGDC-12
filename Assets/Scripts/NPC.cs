using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// To do: have a way to lock player from moving while engaged in dialogue, probably just a bool in the player script
// Make it so that we can change the portrait art for certain dialogue (also maybe the font style as well)

public class NPC : Interactable
{
    private DialogueManager dialogueManager;
    public string npcName;
    public Sprite npcPortrait;
    public TextMeshProUGUI npcNameText;
    public Image npcImageUI;


    protected override void Start()
    {
        // Assuming the DialogueManager is on the same GameObject or you can find it through the scene.
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame, used to open the dialogue panel or go to next line of text when the conditions are met. 
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerIsClose)
        {
            // To play the next line, must have dialogue panel open, must have stopped typing and must not be expecting a response
            if (dialoguePanel.activeInHierarchy && !isTyping && !expectingResponse)
            {
                base.NextLine();
            }
            else if (!dialoguePanel.activeInHierarchy)
            {
                string savedNodeName = PlayerPrefs.GetString(npcName + "_currentNode", string.Empty);
                Time.timeScale = 0;
                npcImageUI.sprite = npcPortrait;
                npcNameText.text = npcName;
                dialoguePanel.SetActive(true);
                npcImageUI.gameObject.SetActive(true);
                if (!string.IsNullOrEmpty(savedNodeName))
                {
                    currentDialogueNode = dialogueManager.GetDialogueNodeById(savedNodeName);
                }
                else
                {
                    currentDialogueNode = startNode;
                }
                StartCoroutine(base.Typing());
            }
        }
    }

    public override void closeDialogue()
    {
        PlayerPrefs.SetString(npcName + "_currentNode", startNode.nodeId);
        PlayerPrefs.Save();
        npcImageUI.gameObject.SetActive(false);
        npcImageUI.sprite = null;
        npcNameText.text = null;
        base.closeDialogue();
    }
}
