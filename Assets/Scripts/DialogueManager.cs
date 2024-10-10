using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public List<DialogueNode> allDialogueNodes; // Store all dialogue nodes
    private Dictionary<string, DialogueNode> dialogueNodeDictionary; // Dictionary for quick access

    void Start()
    {
        string characterName = transform.parent.name;
        string sanitizedCharacterName = CleanFileName(characterName);
        allDialogueNodes = new List<DialogueNode>(Resources.LoadAll<DialogueNode>($"Dialogue/{sanitizedCharacterName}"));
        // Populate the dictionary for fast access
        dialogueNodeDictionary = new Dictionary<string, DialogueNode>();
        foreach (var node in allDialogueNodes)
        {
            dialogueNodeDictionary[node.nodeId] = node;
        }
    }

    // Call this method to find a node by its ID
    public DialogueNode GetDialogueNodeById(string id)
    {
        // Try to get the node from the dictionary
        if (dialogueNodeDictionary.TryGetValue(id, out DialogueNode node))
        {
            return node;
        }
        return null; // Return null if not found
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

    internal DialogueNode GetDialogueNodeByID(string savedNodeName)
    {
        throw new NotImplementedException();
    }
}