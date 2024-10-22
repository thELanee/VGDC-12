using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public static bool gamePaused = false;
    public static int curPage = 1;
    public GameObject PauseMenu;
    public GameObject page1;
    public GameObject page2;
    public GameObject page3;
    public GameObject tab2;
    public GameObject tab3;
    public GameObject text2;
    public GameObject text3;

    public static Animator mAnimator1;
    public static Animator mAnimator2;
    public static Animator mAnimator3;
    public static Animator tAnimator2;
    public static Animator tAnimator3;

    void Start()
    {
        mAnimator1 = page1.GetComponent<Animator>();
        mAnimator2 = page2.GetComponent<Animator>();
        mAnimator3 = page3.GetComponent<Animator>();
        tAnimator2 = tab2.GetComponent<Animator>();
        tAnimator3 = tab3.GetComponent<Animator>();
    }
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                StartCoroutine(Resume2());
                curPage = 1;
            }
            else
            {
                Pause();
                mAnimator1.SetTrigger("Open");
            }
        }
    }

    public void Resume()
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

    IEnumerator Resume2()
    {
        switch (curPage)
        {
            case 1: 
                mAnimator1.SetTrigger("Close");
                yield return new WaitForSecondsRealtime(0.6f);
                page1.transform.SetAsLastSibling();
                yield return new WaitForSecondsRealtime(0.6f);
                break;
            case 2:
                mAnimator1.SetTrigger("Close");
                mAnimator2.SetTrigger("Close");
                tAnimator2.SetTrigger("Close");
                yield return new WaitForSecondsRealtime(0.6f);
                tab2.transform.SetAsLastSibling();
                page2.transform.SetAsLastSibling();
                page1.transform.SetAsLastSibling();
                yield return new WaitForSecondsRealtime(0.6f);
                break;
            case 3:
                mAnimator1.SetTrigger("Close");
                mAnimator2.SetTrigger("Close");
                mAnimator3.SetTrigger("Close");
                tAnimator2.SetTrigger("Close");
                tAnimator3.SetTrigger("Close");
                text2.SetActive(false);
                text3.SetActive(false);
                yield return new WaitForSecondsRealtime(0.6f);
                tab3.transform.SetAsLastSibling();
                tab2.transform.SetAsLastSibling();
                page3.transform.SetAsLastSibling();
                page2.transform.SetAsLastSibling();
                page1.transform.SetAsLastSibling();
                yield return new WaitForSecondsRealtime(0.6f);
                text2.SetActive(true);
                text3.SetActive(true);
                break;
        }
        
        Resume();
    }
}
