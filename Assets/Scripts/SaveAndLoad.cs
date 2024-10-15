using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveAndLoad : MonoBehaviour
{
    public Transform player;

    // Call this method to save the player's position
    public void SavePlayerPosition()
    {
        PlayerPrefs.SetFloat("PlayerX", player.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.position.z);
        PlayerPrefs.Save();
    }

    // Call this method to load the player's position
    public void LoadPlayerPosition()
    {
        if (PlayerPrefs.HasKey("PlayerX"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            float z = PlayerPrefs.GetFloat("PlayerZ");

            player.position = new Vector3(x, y, z);

            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            Debug.Log("No saved position found!");
        }
    }
}


