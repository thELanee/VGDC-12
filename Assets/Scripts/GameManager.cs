using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance of GameManager
    public static GameManager Instance { get; private set; }

    public GameObject saveManagerPrefab; // Reference to the SaveManager prefab
    public DialogueManager dialogueManager;

    private void Awake()
    {
        // Implement the singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep GameManager across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager instances
            return;
        }

        // Check if SaveManager is already instantiated
        if (SaveManager.Instance == null)
        {
            Instantiate(saveManagerPrefab); // Instantiate SaveManager
            Debug.Log("SaveManager instantiated by GameManager.");
        }

        // Ensure DialogueManager is available
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found in the scene. Please ensure it is instantiated.");
        }

        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to sceneLoaded event
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to avoid memory leaks
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Call LoadCharactersForScene when a new scene is loaded
        Debug.Log($"Scene {scene.name} loaded, attempting to load NPCs.");
        LoadCharactersForScene(scene.name); // Load characters for the new scene
    }
    public void LoadCharactersForScene(string sceneName)
    {
        // Access the SaveManager instance
        SaveManager saveManager = SaveManager.Instance;
        if (saveManager != null)
        {
            // Get rendering conditions for the current scene
            var renderConditions = saveManager.GetRenderConditions(sceneName);

            // Log the rendering conditions for the current scene
            Debug.Log($"Loaded rendering conditions for scene: {sceneName}. Total characters: {renderConditions.Count}");

            foreach (var kvp in renderConditions)
            {
                string npcName = kvp.Key; // Get the NPC name
                var renderingInstructionsList = kvp.Value; // Get the list of RenderingInstructions

                Debug.Log($"Checking NPC: {npcName}. Total rendering instructions: {renderingInstructionsList.Count}");

                foreach (var renderingInstructions in renderingInstructionsList)
                {
                    var conditions = renderingInstructions.conditions; // List<Condition>
                    Debug.Log($"NPC: {npcName} - Checking {conditions.Count} conditions.");

                    // Check if the character can be instantiated based on conditions
                    if (saveManager.CheckConditions(conditions))
                    {
                        Debug.Log($"NPC: {npcName} meets the conditions. Instantiating character.");
                        // Instantiate the character with RenderingInstructions
                        InstantiateCharacter(npcName, renderingInstructions, sceneName);
                        break; // Only instantiate once per NPC
                    }
                    else
                    {
                        Debug.Log($"NPC: {npcName} does not meet the conditions.");

                    }
                }
            }
        }
        else
        {
            Debug.LogError("SaveManager instance is null. Cannot load characters for the scene.");
        }
    }

    private void InstantiateCharacter(string npcName, RenderingInstructions renderingDetails, string targetSceneName)
    {
        GameObject npcPrefab = Resources.Load<GameObject>($"NPCs/{npcName}");

        if (npcPrefab != null)
        {
            // Instantiate the NPC prefab
            GameObject npcInstance = Instantiate(npcPrefab);

            // Set the NPC's position based on rendering coordinates
            Vector3 npcPosition = new Vector3(renderingDetails.renderingCoords.x, renderingDetails.renderingCoords.y, 0);
            npcInstance.transform.position = npcPosition;

            // Set the NPC's layer (2 in this case)
            npcInstance.layer = 2;

            // Get the NPC component and set the starting dialogue node
            var npcComponent = npcInstance.GetComponent<NPC>();
            if (npcComponent != null)
            {
                npcComponent.startNode = dialogueManager.GetDialogueNodeById(renderingDetails.nodeId);
            }

            // Set the NPC's facing direction based on rendering details (using Direction enum)
            switch (renderingDetails.facingDirection)  // Assuming facingDirection is of type Direction
            {
                case Direction.Up:
                    npcInstance.transform.rotation = Quaternion.Euler(0, 0, 0);  // Face up
                    break;
                case Direction.Down:
                    npcInstance.transform.rotation = Quaternion.Euler(0, 0, 180);  // Face down
                    break;
                case Direction.Left:
                    npcInstance.transform.rotation = Quaternion.Euler(0, 0, 90);  // Face left
                    break;
                case Direction.Right:
                    npcInstance.transform.rotation = Quaternion.Euler(0, 0, -90);  // Face right
                    break;
                default:
                    Debug.LogWarning($"Unknown facing direction '{renderingDetails.facingDirection}' for NPC {npcName}. Defaulting to 'up'.");
                    npcInstance.transform.rotation = Quaternion.Euler(0, 0, 180);  // Default Face down 
                    break;
            }

            // Move the NPC GameObject to the target scene
            UnityEngine.SceneManagement.Scene targetScene = SceneManager.GetSceneByName(targetSceneName);
            if (targetScene.isLoaded)
            {
                SceneManager.MoveGameObjectToScene(npcInstance, targetScene);
                Debug.Log($"NPC {npcName} instantiated and moved to scene: {targetSceneName}");
            }
            else
            {
                Debug.LogWarning($"Scene {targetSceneName} is not loaded. Cannot move NPC to the target scene.");
            }

            Debug.Log($"Instantiated {npcName} at position {npcPosition} with nodeId {renderingDetails.nodeId}, facing {renderingDetails.facingDirection}");
        }
        else
        {
            Debug.LogWarning($"NPC prefab '{npcName}' not found in Resources/NPCs.");
        }
    }
}
