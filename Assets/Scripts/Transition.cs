using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public string SceneName;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Swap to temp scene.");
        SceneManager.LoadScene("TransitionSample");
    }
}
