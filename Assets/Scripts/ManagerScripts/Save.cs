using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
[System.Serializable]
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public Dictionary<string, Dictionary<string, string>> sceneStartNodes;
    public List<Condition> characterFlags;
    // Scene_Name => NPC_Name => [([Conditions], (dialogue, (coords)))]    
    private Dictionary<string, Dictionary<string, List<RenderingInstructions>>> renderingConditions;

    private static string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance alive across scenes
            LoadRenderingConditionsFromFile();
            characterFlags = new List<Condition>();
            saveFilePath = Application.persistentDataPath + "/savedata.dat";

        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    public void AddStartNode(string npc, string scene, string nodeId)
    {
        if (sceneStartNodes == null)
        {
            sceneStartNodes = new Dictionary<string, Dictionary<string, string>>();
        }
        if (!sceneStartNodes.ContainsKey(scene))
        {
            // If the scene doesn't exist, create a new dictionary for NPCs in this scene
            sceneStartNodes[scene] = new Dictionary<string, string>();
        }
        if (sceneStartNodes[scene].ContainsKey(npc))
        {
            sceneStartNodes[scene][npc] = nodeId; // Update existing start node id
        }
        else
        {
            sceneStartNodes[scene].Add(npc, nodeId); // Add new NPC and start node id
        }
    }

    public void SaveStartNode()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = new FileStream(saveFilePath, FileMode.Create);
        formatter.Serialize(file, sceneStartNodes);
        file.Close();
    }

    public void LoadStartNodes()
    {
        if (File.Exists(saveFilePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(saveFilePath, FileMode.Open);
            try
            {
                sceneStartNodes = (Dictionary<string, Dictionary<string, string>>)formatter.Deserialize(file);
            }
            catch (Exception ex)
            {
                //Debug.LogError("Failed to load start nodes: " + ex.Message);
            }
            finally
            {
                file.Close();
            }
        }
        else
        {
            //Debug.LogWarning("Save file does not exist: " + saveFilePath);
        }
    }

    public string GetStartNode(string npc, string scene)
    {
        if (sceneStartNodes != null && sceneStartNodes.ContainsKey(scene) && sceneStartNodes[scene].ContainsKey(npc))
        {
            return sceneStartNodes[scene][npc]; // return nodeId
        }
        return null;                            // Could not find starting node
    }

    public void AddCharacterFlags(List<string> flags)
    {
        foreach (string flag in flags)
        {
            // Check if the flag already exists in characterFlags
            Condition existingCondition = characterFlags.Find(c => c.flag.Equals(flag));

            if (existingCondition != null)
            {
                // If it exists, set the triggered property to true
                existingCondition.triggered = true;
            }
            else
            {
                // If it doesn't exist, create a new Condition and add it to characterFlags
                Condition newCondition = new Condition
                {
                    flag = flag,
                    triggered = true
                };
                characterFlags.Add(newCondition);
            }
        }
    }

    public void SaveCharacterFlags()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = new FileStream(saveFilePath, FileMode.Create);
        formatter.Serialize(file, characterFlags);
        file.Close();
    }

    public bool CheckConditions(List<Condition> flags)
    {
        // Iterate through the provided conditions to check
        foreach (Condition flag in flags)
        {
            // Check if the flag already exists in characterFlags
            Condition existingCondition = characterFlags.Find(c => c.flag.Equals(flag.flag));

            // If the condition does not exist, add it as false and continue
            if (existingCondition == null)
            {
                // Create a new Condition with triggered as false
                existingCondition = new Condition
                {
                    flag = flag.flag,
                    triggered = false
                };
                characterFlags.Add(existingCondition);
            }

            // Check if the triggered state matches
            if (existingCondition.triggered != flag.triggered)
            {
                return false; // Return false if their states differ
            }
        }
        // All conditions matched with the same triggered state
        return true;
    }

    public void LoadCharacterFlags()
    {
        if (File.Exists(saveFilePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(saveFilePath, FileMode.Open);

            try
            {
                // Deserialize the data into the characterFlags dictionary
                characterFlags = (List<Condition>)formatter.Deserialize(file);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to load character flags: " + ex.Message);
            }
            finally
            {
                file.Close();
            }
        }
        else
        {
            Debug.LogWarning("Save file does not exist: " + saveFilePath);
            characterFlags = new List<Condition>(); // Initialize if file does not exist
        }
    }

    public List<Condition> GetCharacterFlags()
    {
        return characterFlags; // Return an empty list if no flags found for the NPC
    }
    //(Scene_Name) => <NPC_NAME, [([conditions],(nodeId, coords))]>, Each npc has a list of (one or more conditions) 
    // and for each of those condition sets, a dialogue node id and position they will be instantiated with if the conditions are met
    public void LoadRenderingConditionsFromFile()
    {
        // Load the JSON file from the Resources folder
        TextAsset jsonFile = Resources.Load<TextAsset>("renderingConditions");

        // Check if the file was loaded successfully
        if (jsonFile != null)
        {
            // Get the JSON string from the loaded TextAsset
            string json = jsonFile.text;

            // Deserialize the JSON into RenderingConditionData
            RenderingConditionData data = JsonUtility.FromJson<RenderingConditionData>(json);

            // Initialize the renderingConditions dictionary
            renderingConditions = new Dictionary<string, Dictionary<string, List<RenderingInstructions>>>();

            // Iterate through the entries in the data
            foreach (var entry in data.entries)
            {
                // Log each entry being processed
                //Debug.Log($"Processing entry for Scene: {entry.sceneName}, Character: {entry.characterName}");

                // Ensure the outer dictionary has the scene name
                if (!renderingConditions.ContainsKey(entry.sceneName))
                {
                    renderingConditions[entry.sceneName] = new Dictionary<string, List<RenderingInstructions>>();
                }

                // Access the inner dictionary for the specific character name
                var characterDict = renderingConditions[entry.sceneName];

                // Check if the character already exists in the inner dictionary
                if (!characterDict.ContainsKey(entry.characterName))
                {
                    characterDict[entry.characterName] = new List<RenderingInstructions>();
                }

                // Log the instructions before creating the RenderingInstructions object
                //Debug.Log($"Entry Instructions: NodeId: {entry.instructions.nodeId}, RenderingCoords: {entry.instructions.renderingCoords}, Facing Direction: {entry.instructions.facingDirection}");

                // Check if conditions are null
                if (entry.instructions.conditions == null)
                {
                    //Debug.LogWarning($"Conditions for Scene: {entry.sceneName}, Character: {entry.characterName} are null.");
                }
                else
                {
                    //Debug.Log($"Loaded {entry.instructions.conditions.Count} conditions for Scene: {entry.sceneName}, Character: {entry.characterName}.");
                }

                // Create the RenderingInstructions object directly from the data
                var renderingInstructions = new RenderingInstructions
                {
                    conditions = entry.instructions.conditions, // Accessing conditions correctly
                    nodeId = entry.instructions.nodeId, // Assuming there's only one nodeId
                    renderingCoords = entry.instructions.renderingCoords, // Assuming this is a CustomVector2
                    facingDirection = entry.instructions.facingDirection // New property for facing direction
                };

                // Add the RenderingInstructions object to the list for that character
                characterDict[entry.characterName].Add(renderingInstructions);
            }

            // Log the total loaded conditions after the loop
            //Debug.Log($"Successfully loaded rendering conditions. Total scenes: {renderingConditions.Count}");
        }
        else
        {
           // Debug.LogError("Failed to load renderingConditions.json from Resources.");
        }
    }

    public Dictionary<string, List<RenderingInstructions>> GetRenderConditions(string sceneName)
    {
        var result = new Dictionary<string, List<RenderingInstructions>>();

    //    Debug.Log($"Getting rendering conditions for scene: {sceneName}");

        if (renderingConditions.TryGetValue(sceneName, out var sceneEntry))
        {
         //   Debug.Log($"Found {sceneEntry.Count} characters in scene: {sceneName}");

            foreach (var characterEntry in sceneEntry)
            {
                string characterName = characterEntry.Key;
               // Debug.Log($"Processing character: {characterName}");

                // Initialize the list for this character if it doesn't exist
                if (!result.ContainsKey(characterName))
                {
                    result[characterName] = new List<RenderingInstructions>();
                }

                foreach (var renderingInstructions in characterEntry.Value)
                {
                    // Add the RenderingInstructions directly to the list for this character
                    result[characterName].Add(renderingInstructions);
                    //Debug.Log("Added Instruction " + renderingInstructions.ToString());
                }
            }
        }
        else
        {
            //Debug.LogWarning($"No rendering conditions found for scene: {sceneName}");
        }

        //Debug.Log($"Finished getting rendering conditions for scene: {sceneName}. Total characters processed: {result.Count}");

        return result;
    }
}

