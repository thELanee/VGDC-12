using UnityEngine;
using System.Collections.Generic;


/*
 *  Ideally how this should work is that each character in a given scene will have a starting dialogue node that assigned to them, this node 
 *  is connected to another node by the programmer to the next instance of dialogue (an instance being what you click through to refresh and repopulate the dialogue box with text).
 *  In some cases, the dialogue will require a player response, in which case, the NPC logic should detect that the list of dialogue options has been populated and a display will
 *  show up containing your dialogue options. These dialogue options also have a set next node, so you can have branching dialogue paths that can also converge back to linear dialogue.
 */

[CreateAssetMenu(fileName = "NewDialogueNode", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    public List<DialogueOption> options; // This should be set only if the next dialogue IS affected by player choice

    public List<FlagConnector> flags;    // If this is set, we have dialogue that are determined by flags, if those flags are triggered, we skip the dialogue here and go directly to the triggered node.
                                         // NOTE: We will select the first node that passes the condition checks so make sure that the dialogue that requires the most conditions is placed first in the list
    public DialogueNode nextNode;        // This should be set only if the next dialogue IS NOT affected by player choice
    public bool pause;                   // This should be set to true only if we want the dialogue to exit after displaying 
                                         // the text on this node
    public bool bookmark;                // This should be set to true if you want the dialogue to start from here on the next interaction
                                         // WARNING: DO NOT HAVE A BOOKMARK NODE BEFORE A PAUSE NODE OR ELSE DIALOGUE WILL NOT CONTINUE PAST THE PAUSE NODE
    public string dialogue;              // Where the character's dialogue is stored

    public string nodeId;                // How we keep track of and retrieve dialogue nodes

    public List<string> flags_activated; // Seeing this dialogue option triggers the flags listed
    private void OnEnable()
    {
        // Automatically set nodeId to the name of the file (the name of this ScriptableObject)
        nodeId = name; 
    }
}

[System.Serializable]
public class DialogueOption
{
    public string optionText;           // The text for the option
    public DialogueNode nextNode;       // The next dialogue node if this option is chosen
}

// A flag dialogue node is a class containing a nextNode and a list of flags that, if they are all true, will cause that next node to become current node. Each dialogue node will have a list of these and we will check if it has something in next line. We now
// just have start node as a way to save the last start point of a given scene and npc's dialogue 
[System.Serializable]
public class FlagConnector
{
    public List<Condition> flags;       // The text for the option
    public DialogueNode nextNode;       // The next dialogue node if this option is chosen
}

[System.Serializable]
public class Condition
{
    public string flag;
    public bool triggered;
}