using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    // Singleton instance of GameManager
    public static GameManager Instance { get; private set; }
    public GameObject saveManagerPrefab; // Reference to the SaveManager prefab
    public DialogueManager dialogueManager;
    // Dictionary to keep track of emptied chests (using unique identifiers)
    private Dictionary<string, Dictionary<int, bool>> chestStates = new Dictionary<string, Dictionary<int, bool>>();

    public Pathfinding Pathfinder { get; private set; }
    public MyTilemapProvider TileProvider { get; private set; } // Assume you have some tile provider

    private void Awake()
    {
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
        }

        // Ensure DialogueManager is available
        dialogueManager = FindObjectOfType<DialogueManager>();
        Pathfinder = new Pathfinding();

        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to sceneLoaded event
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to avoid memory leaks
    }

    BoundsInt GetTileMapDimensions(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;
        int width = bounds.size.x;
        int height = bounds.size.y;
        // Output the size
        Debug.Log("Tilemap Width: " + width);
        Debug.Log("Tilemap Height: " + height);
        return bounds;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Call LoadCharactersForScene when a new scene is loaded
        LoadCharactersForScene(scene.name); // Load characters for the new scene
        Tilemap groundTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        Tilemap wallTilemap = GameObject.Find("Wall").GetComponent<Tilemap>();
        if (groundTilemap != null && wallTilemap != null)
        {
            Debug.Log("Ground and Wall tilemaps loaded successfully.");
            // Get dimensions of the tilemaps
            BoundsInt groundBounds = GetTileMapDimensions(groundTilemap);
            BoundsInt wallBounds = GetTileMapDimensions(wallTilemap);
            Debug.Log("Tilemap Bounds: " + groundBounds);
            TileProvider = new MyTilemapProvider(groundBounds, groundTilemap, wallTilemap);
        }
        else
        {
            Debug.LogWarning("Tilemaps not found in the scene.");
        }
    }
    public void LoadCharactersForScene(string sceneName)
    {
        // Access the SaveManager instance
        SaveManager saveManager = SaveManager.Instance;
        if (saveManager != null)
        {
            // Get rendering conditions for the current scene
            var renderConditions = saveManager.GetRenderConditions(sceneName);
            foreach (var kvp in renderConditions)
            {
                string npcName = kvp.Key; // Get the NPC name
                var renderingInstructionsList = kvp.Value; // Get the list of RenderingInstructions
                foreach (var renderingInstructions in renderingInstructionsList)
                {
                    var conditions = renderingInstructions.conditions; // List<Condition>

                    // Check if the character can be instantiated based on conditions
                    if (saveManager.CheckConditions(conditions))
                    {
                        // Instantiate the character with RenderingInstructions
                        InstantiateCharacter(npcName, renderingInstructions, sceneName);
                        break; // Only instantiate once per NPC
                    }
                }
            }
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
                    npcInstance.transform.rotation = Quaternion.Euler(0, 0, 180);  // Default Face down 
                    break;
            }

            // Move the NPC GameObject to the target scene
            Scene targetScene = SceneManager.GetSceneByName(targetSceneName);
            if (targetScene.isLoaded)
            {
                SceneManager.MoveGameObjectToScene(npcInstance, targetScene);
            }
        }
    }

    // Check if a chest has been emptied
    public bool IsChestEmpty(int chestID, string sceneName)
    {
        // Check if the scene exists in the chestStates dictionary
        if (chestStates.TryGetValue(sceneName, out Dictionary<int, bool> sceneChests))
        {
            // Check if the specific chest exists and return its state (true = empty)
            return sceneChests.TryGetValue(chestID, out bool isEmpty) && isEmpty;
        }
        return false; // If scene or chest is not found, assume the chest is not empty
    }

    // Set a chest as empty
    public void SetChestEmpty(int chestID, string sceneName)
    {
        // Check if the scene exists in the chestStates dictionary
        if (!chestStates.ContainsKey(sceneName))
        {
            chestStates[sceneName] = new Dictionary<int, bool>(); // Add the scene if not present
        }

        // Mark the specific chest as empty in the scene's dictionary
        chestStates[sceneName][chestID] = true;
    }
}
