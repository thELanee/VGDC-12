using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// To do: have a way to lock player from moving while engaged in dialogue, probably just a bool in the player script
// Make it so that we can change the portrait art for certain dialogue (also maybe the font style as well)

public class Interactable : MonoBehaviour
{
    public GameObject optionButtonPrefab;
    public Transform optionButtonContainer;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public DialogueNode startNode;             // The dialogue we want to start from whenever the player interacts with the NPC
    protected DialogueNode currentDialogueNode;
    public float wordSpeed;
    public bool playerIsClose;
    protected bool isTyping;
    protected bool expectingResponse;

    protected virtual void Start()
    {

    }

    // Update is called once per frame, used to open the dialogue panel or go to next line of text when the conditions are met. 
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerIsClose)
        {
            // To play the next line, must have dialogue panel open, must have stopped typing and must not be expecting a response
            if (dialoguePanel.activeInHierarchy && !isTyping && !expectingResponse)
            {
                NextLine();
            }
            else if (!dialoguePanel.activeInHierarchy)
            {
                Time.timeScale = 0;
                dialoguePanel.SetActive(true);
                currentDialogueNode = startNode;
                StartCoroutine(Typing());
            }
        }
    }

    public virtual void closeDialogue()
    {
        Time.timeScale = 1;
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
        ClearDialogueOptions();
        if (optionButtonContainer != null)
        {
            foreach (Transform child in optionButtonContainer)
            {
                Destroy(child.gameObject); // Destroy each button in the container
            }
        }
    }

    protected virtual IEnumerator Typing()
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in currentDialogueNode.dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(wordSpeed);
        }
        isTyping = false;

        // Check if there are options to display after typing
        if (optionButtonContainer != null)
        {
            if (currentDialogueNode.options.Count > 0)
            {
                expectingResponse = true;
                DisplayDialogueOptions(currentDialogueNode.options);
            }
            else
            {
                optionButtonContainer.gameObject.SetActive(false); // Hide options if none
            }
        }

    }

    // Controls what dialogue node we go to next or if we exit dialogue
    protected virtual void NextLine()
    {
        // We still have more dialogue, and it does not require player input to reach
        if (!currentDialogueNode.pause && (currentDialogueNode.options.Count == 0) && currentDialogueNode.nextNode != null)
        {
            currentDialogueNode = currentDialogueNode.nextNode;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        // We want to pause the dialogue here, it will continue from the next line if you reinteract with the NPC
        else if (currentDialogueNode.pause && currentDialogueNode.nextNode != null)
        {
            startNode = currentDialogueNode.nextNode;
            closeDialogue();
        }
        // There is no dialogue after this
        else
        {
            closeDialogue();
        }
        // We want the dialogue to start from here when you talk to them again in the same scene
        if (currentDialogueNode.bookmark)
        {
            startNode = currentDialogueNode;
        }
    }

    protected void DisplayDialogueOptions(List<DialogueOption> options)
    {
        if (optionButtonContainer != null)
        {
            ClearDialogueOptions(); // Clear existing buttons
            optionButtonContainer.gameObject.SetActive(true);
            foreach (DialogueOption option in options)
            {
                // Instantiate a button for each dialogue option using the static prefab from DialogueManager
                GameObject optionButton = Instantiate(optionButtonPrefab, optionButtonContainer);
                TextMeshProUGUI buttonText = optionButton.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = option.optionText;
                // Add a listener to handle the button click
                Button button = optionButton.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.AddListener(() => OnOptionSelected(option));
                }
            }
        }
    }
    public void OnOptionSelected(DialogueOption selectedOption)
    {
        if (optionButtonContainer != null)
        {
            // Move to the selected option's next node
            currentDialogueNode = selectedOption.nextNode;
            optionButtonContainer.gameObject.SetActive(false);
            ClearDialogueOptions();
            expectingResponse = false;
            StartCoroutine(Typing());
        }
    }

    protected void ClearDialogueOptions()
    {
        // Destroy all existing buttons in the option container
        if (optionButtonContainer != null)
        {
            foreach (Transform child in optionButtonContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }

    protected DialogueNode CreateDialogueNode(string dialogueText, bool p = false, bool b = false, List<DialogueOption> options = null, DialogueNode nn = null)
    {
        DialogueNode node = ScriptableObject.CreateInstance<DialogueNode>();
        node.dialogue = dialogueText;
        node.pause = p;
        node.bookmark = b;
        node.options ??= new List<DialogueOption>();;
        node.nextNode ??= nn;
        return node;
    }
}
