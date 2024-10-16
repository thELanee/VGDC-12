using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public static bool gamePaused = false;

    public GameObject PauseMenu;
    public GameObject page1;
    private Animator mAnimator;

    void Start()
    {
        mAnimator = page1.GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

            if (mAnimator != null)
            {
                if (gamePaused)
                {
                    Debug.Log("a");
                    mAnimator.SetTrigger("Open");
                }
                else
                {
                    mAnimator.SetTrigger("Closed");
                }
            }
        }
    }

    void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    void Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }
}
