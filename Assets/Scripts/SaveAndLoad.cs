using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndLoad : MonoBehaviour
{
    public GameObject player;  // Reference to the player GameObject

    // Call this method to save the player's position (triggered by Save button)
    public void SavePlayerPosition()
    {
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.transform.position.z);
        PlayerPrefs.Save();  // Save the changes immediately
        Debug.Log("Player position saved!");
    }

    // Call this method to load the player's position
    public void LoadPlayerPosition()
    {
        // Check if there is a saved position
        if (PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY") && PlayerPrefs.HasKey("PlayerZ"))
        {
            // Load the scene asynchronously to ensure the scene is fully loaded
            SceneManager.LoadSceneAsync("SampleScene").completed += OnSceneLoaded;
        }
        else
        {
            Debug.Log("No saved position found!");
        }
    }

    // Callback method after the scene is loaded
    void OnSceneLoaded(AsyncOperation op)
    {
        // Get the saved position from PlayerPrefs
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        float z = PlayerPrefs.GetFloat("PlayerZ");

        // Access the player's Transform and set the position
        player.transform.position = new Vector3(x, y, z);
    }

    // Automatically assign the player GameObject if not assigned in the Inspector
    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");  // Find the GameObject tagged as 'Player'
            if (playerObj != null)
            {
                player = playerObj;  // Assign the player GameObject
            }
            else
            {
                Debug.LogError("Player object with tag 'Player' not found!");
            }
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
