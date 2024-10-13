using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// To do: have a way to lock player from moving while engaged in dialogue, probably just a bool in the player script
// Make it so that we can change the portrait art for certain dialogue (also maybe the font style as well)

public class NPC : Interactable
{
    public string npcName;
    public Sprite npcPortrait;


    // Update is called once per frame, used to open the dialogue panel or go to next line of text when the conditions are met. 
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerIsClose)
        {
            // To play the next line, must have dialogue panel open, must have stopped typing and must not be expecting a response
            if (DialogueManager.Instance.dialoguePanel.activeSelf && !DialogueManager.Instance.isTyping && !DialogueManager.Instance.expectingResponse)
            {
                DialogueManager.Instance.NextLine();
                this.currentDialogueNode = DialogueManager.Instance.currentDialogueNode;
                this.startNode = DialogueManager.Instance.startNode;
            }
            else if (!DialogueManager.Instance.dialoguePanel.activeSelf)
            {
                DialogueManager.Instance.npcName = this.npcName;
                string savedNodeId = SaveManager.Instance.GetStartNode(npcName, SceneManager.GetActiveScene().name);
                Time.timeScale = 0;
                DialogueManager.Instance.setNPCImage(npcName, npcPortrait);
                DialogueManager.Instance.ShowDialogue();
                DialogueManager.Instance.npcImageUI.gameObject.SetActive(true);
                if (!string.IsNullOrEmpty(savedNodeId))
                {
                    currentDialogueNode = DialogueManager.Instance.GetDialogueNodeById(savedNodeId);
                }
                else
                {
                    currentDialogueNode = startNode;
                }
                DialogueManager.Instance.startNode = startNode;
                DialogueManager.Instance.TypingS(currentDialogueNode);
            }
        }
    }
}


// Unable to account for scene instances: What if we want to have our NPC reappear in the scene after certain conditions are met. How do we get them to have different dialogue. 
// Potential solution: Have a unique SceneManager script attached to empty game object in scene, can have a public check conditions function called upon transitioning to a scene, check NPC flags, if met render them at a set location with set start node
// Sounds like a pain, how do we not have to make a unique SceneManager script for each scene, 
// On Load, check save file npc flags (current scene -> check all character names -> check all bools), if conditions met (how do we know conditions) => have a game file organized like Scene Name => NPC_NAME => Conditions [(flag, bool)]
// (get conditions using names and compare) then for each set of conditions see if the list of bools match=>  
// (how do we know what to do when they are met) => automatically assume that we want to render the character with start node and location, so under each condition in the game file, we will have a nodeId for their start node and a coordinate in the scene
// Three functions: getRenderConditions(Scene_Name) => <NPC_NAME, [([conditions],(nodeId, coords))]>, getCharacterFlags(Scene Name, [NPC_NAME]) => <NPC_Name, [Flags]> , conditionallyRender(<NPC_NAME, conditions>, <NPC_NAME, [([conditions],(nodeId, coords)]>
// Flags class => string flag, bool active;                         List of tuples with a list of conditions tied to outcomes in another tuple