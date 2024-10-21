using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PageFlipping : MonoBehaviour
{
    public GameObject page;
    int curPage;
    public GameObject text3;
    public GameObject tab3;

    void Start()
    {
    }
    public void open()
    {
        curPage = PauseScript.curPage;
        int.TryParse(page.transform.name.Substring(1), out PauseScript.curPage);
        switch (PauseScript.curPage)
        {
            case 2:
                if(curPage == 1)
                {
                    PauseScript.mAnimator2.SetTrigger("Open");
                    StartCoroutine(Wait(page));
                }else if(curPage == 2)
                {
                    PauseScript.curPage = 2;
                }
                break;
            case 3:
                if(curPage == 1)
                {
                    PauseScript.mAnimator2.SetTrigger("Open");
                    PauseScript.mAnimator3.SetTrigger("Open");
                    PauseScript.tAnimator3.SetTrigger("Open");
                    StartCoroutine(Wait(page));
                }else if(curPage == 2)
                {
                    PauseScript.mAnimator3.SetTrigger("Open");
                    PauseScript.tAnimator3.SetTrigger("Open");
                    StartCoroutine(Wait(page));
                }else if(curPage == 3)
                {
                    PauseScript.curPage = 3;
                }
                break;
        }
        
        
    }

    IEnumerator Wait(GameObject page)
    {
        if(PauseScript.curPage == 3)
        {
            text3.SetActive(false);
        }
        yield return new WaitForSecondsRealtime(0.6f);
        tab3.transform.SetAsLastSibling();
        page.transform.SetAsLastSibling();
        yield return new WaitForSecondsRealtime(0.6f);
        if (PauseScript.curPage == 3)
        {
            text3.SetActive(true);
        }
    }
}
