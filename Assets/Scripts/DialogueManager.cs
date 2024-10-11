using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    public GameObject dialoguePanel; // Reference to the dialogue panel GameObject
    public TextMeshProUGUI dialogueText; // Reference to the dialogue text component
    public Transform optionButtonContainer; // Reference to the container for option buttons
    public GameObject optionButtonPrefab;
    public bool expectingResponse;
    public TextMeshProUGUI npcNameText;
    public Image npcImageUI;
    //public List<DialogueNode> allDialogueNodes; // Store all dialogue nodes
    //private Dictionary<string, DialogueNode> dialogueNodeDictionary; // Dictionary for quick access

    public DialogueNode currentDialogueNode; // Current dialogue node being processed
    public bool isTyping;
    public float wordSpeed;
    public DialogueNode startNode;

    public string npcName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        //allDialogueNodes = new List<DialogueNode>(Resources.LoadAll<DialogueNode>($"Dialogue/"));
        // Populate the dictionary for fast access
        //dialogueNodeDictionary = new Dictionary<string, DialogueNode>();
        //foreach (var node in allDialogueNodes)
        //{
        //    dialogueNodeDictionary[node.nodeId] = node;
        //}
        // Initially hide the dialogue panel
        dialoguePanel.SetActive(false);
    }

    // Call this method to find a node by its ID
    public DialogueNode GetDialogueNodeById(string id)
    {
        // Attempt to load the DialogueNode from the Resources folder
        DialogueNode node = Resources.Load<DialogueNode>($"Dialogue/{id}");

        // Return the loaded node or null if not found
        if (node != null)
        {
            return node;
        }

        Debug.LogWarning($"DialogueNode with ID '{id}' not found in Resources/DialogueNodes.");
        return null; // Return null if not found
    }
    public void unsetNPCImage()
    {
        npcImageUI.gameObject.SetActive(false);
        npcImageUI.sprite = null;
        npcNameText.text = null;
    }
    public void setNPCImage(string npcName, Sprite npcPortrait)
    {
        npcImageUI.sprite = npcPortrait;
        npcNameText.text = npcName;
    }
    public void TypingS(DialogueNode currentDialogueNode)
    {
        this.currentDialogueNode = currentDialogueNode;
        StartCoroutine(Typing(currentDialogueNode));
    }
    public IEnumerator Typing(DialogueNode currentDialogueNode)
    {
        isTyping = true;
        dialogueText.text = ""; // Clear current text

        // Check flags for conditions and update current node accordingly
        if (currentDialogueNode.flags.Count > 0)
        {
            foreach (FlagConnector flag in currentDialogueNode.flags)
            {
                if (SaveManager.Instance.CheckConditions(flag.flags))
                {
                    currentDialogueNode = flag.nextNode; // Move to the node linked to the met condition
                }
            }
        }

        // Display the dialogue text with typing effect
        foreach (char letter in currentDialogueNode.dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(wordSpeed);
        }
        isTyping = false;

        // Display options if available
        if (currentDialogueNode.options.Count > 0)
        {
            expectingResponse = true;
            DisplayDialogueOptions(currentDialogueNode.options); // Show options
        }
        else
        {
            optionButtonContainer.gameObject.SetActive(false); // Hide options if none
        }

        // Activate any flags associated with this dialogue node
        if (currentDialogueNode.flags_activated.Count > 0)
        {
            SaveManager.Instance.AddCharacterFlags(currentDialogueNode.flags_activated);
        }
    }
    // Show the dialogue panel
    public void ShowDialogue()
    {
        dialoguePanel.SetActive(true);
    }

    // Hide the dialogue panel
    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        ClearDialogueOptions(); // Clear options when hiding
    }

    // Display dialogue options in the options container
    public void DisplayDialogueOptions(List<DialogueOption> options)
    {
        ClearDialogueOptions(); // Clear existing options

        optionButtonContainer.gameObject.SetActive(true); // Show the options container

        foreach (DialogueOption option in options)
        {
            // Instantiate a button for each dialogue option using the prefab
            GameObject optionButton = Instantiate(optionButtonPrefab, optionButtonContainer);
            TextMeshProUGUI buttonText = optionButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = option.optionText;

            // Add a listener to handle the button click
            Button button = optionButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnOptionSelected(option));
                Debug.Log("Button added for option: " + option.optionText); // Debug log
            }
            else
            {
                Debug.LogError("Button component is missing from the option button prefab.");
            }
        }
    }
    // Clear dialogue options from the options container
    public void ClearDialogueOptions()
    {
        // Destroy all existing buttons in the option container
        foreach (Transform child in optionButtonContainer)
        {
            Destroy(child.gameObject);
        }
        optionButtonContainer.gameObject.SetActive(false); // Hide options container after clearing
    }

    // Handle option selection
    public void OnOptionSelected(DialogueOption selectedOption)
    {
        // Proceed to the next node based on the selected option
        if (selectedOption.nextNode != null)
        {
            expectingResponse = false;
            currentDialogueNode = selectedOption.nextNode; // Update the current dialogue node
            ClearDialogueOptions(); // Clear existing options
            optionButtonContainer.gameObject.SetActive(false);
            TypingS(currentDialogueNode); // Start typing effect for the next dialogue node
        }
        else
        {
            Debug.LogWarning("Selected option does not have a next node.");
        }
    }
    public void NextLine()
    {
        // Continue to the next node if available and not expecting player input
        if (!currentDialogueNode.pause && (currentDialogueNode.options.Count == 0) && currentDialogueNode.nextNode != null)
        {
            currentDialogueNode = currentDialogueNode.nextNode; // Move to the next node
            Debug.Log("Switch to next node: " + currentDialogueNode.dialogue);
            dialogueText.text = ""; // Clear current text
            TypingS(currentDialogueNode); // Start typing effect
        }
        // If pausing, exit dialogue and save the next node
        else if (currentDialogueNode.pause && currentDialogueNode.nextNode != null)
        {
            startNode = currentDialogueNode.nextNode; // Save next node
            closeDialogue(); // Exit dialogue
        }
        // If there’s no more dialogue, close the panel
        else
        {
            closeDialogue(); // Close dialogue
        }

        // Save the current node if it is a bookmark
        if (currentDialogueNode.bookmark)
        {
            startNode = currentDialogueNode; // Bookmark the current node
        }
    }

    public void eraseNodes()
    {
        this.startNode = null;
        this.currentDialogueNode = null;
    }
    public void closeDialogue()
    {
        Time.timeScale = 1; // Resume game time
        if (npcName != null)
        {
            if (startNode != null)
            {
                SaveManager.Instance.AddStartNode(npcName, SceneManager.GetActiveScene().name, startNode.nodeId);
            }
            npcImageUI.gameObject.SetActive(false);
            npcImageUI.sprite = null;
            npcNameText.text = null;
            npcName = null;
        }
        dialogueText.text = ""; // Clear dialogue text
        Instance.HideDialogue(); // Hide the dialogue panel
        Instance.ClearDialogueOptions(); // Clear existing options
    }
    private string CleanFileName(string fileName)
    {
        char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
        foreach (char c in invalidChars)
        {
            fileName = fileName.Replace(c.ToString(), "");
        }
        return fileName;
    }
}
