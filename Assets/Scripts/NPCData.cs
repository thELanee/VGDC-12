using System.Collections.Generic;

// Kinda a placeholder file for now, I'll give this more thought when I know for sure what we want to have NPCs keep track of and once we have saving and loading down

[System.Serializable]
public class NPCData {
    public string npcId;  // Unique identifier for the NPC
    public int talkCount;  // Number of times the player has talked to this NPC
    public List<string> choicesMade; // List of choices made by the player

    public NPCData(string id) {
        npcId = id;
        talkCount = 0; // Initialize talk count to 0
        choicesMade = new List<string>(); // Initialize the list of choices
    }
}