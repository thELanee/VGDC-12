using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public string SceneName;

    void Start()
    {
        // Subscribe to the sceneLoaded event to load NPCs when a new scene is fully loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("Subscribed to OnSceneLoaded");
    }

    private void OnDestroy()
    {
        // Unsubscribe from sceneLoaded to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Load the new scene asynchronously
        Debug.Log("Attempting to load scene: " + SceneName);
        SceneManager.LoadScene(SceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the newly loaded scene is the one specified in Transition
        if (scene.name == SceneName)
        {
            // Call GameManager to load NPCs for the newly loaded scene
            Debug.Log("Scene loaded successfully: " + SceneName);
            GameManager.Instance.LoadCharactersForScene(scene.name); // Pass the loaded scene's name
        }
    }
}
