using System.Collections;
using System.Collections.Generic;
using kurukuru;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]List<GameObject> TutorialObjList=new List<GameObject>();

    [SerializeField]List<Color> color=new List<Color>();
    private GameManager gameManager;

    [SerializeField]AudioClip audioClip;
    [SerializeField]AudioSource audioSource;
   
    int nowNum=0;
    
    public void TutorialStart()
    {
        Debug.Log("start" + TutorialObjList.Count);
        foreach (var item in TutorialObjList)
        {
            item.SetActive(false);
        }
        TutorialObjList[0].SetActive(true);

        gameManager=GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    
    public void TutorialUpdate()
    {
        
        Debug.Log("update" + TutorialObjList.Count);
        if ((Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
        {
            Touch touch;
            if (Input.touchCount > 0)
                touch = Input.GetTouch(0);
            else
                touch = new Touch();

            if (touch.phase == TouchPhase.Began)
            {
                if(nowNum>=15)
                {
                    foreach (var item in TutorialObjList)
                    {
                        item.SetActive(false);
                    }
                    gameManager.ChangeGameState(GameManager.GameState.Ready);
                }
                else 
                {
                    audioSource.PlayOneShot(audioClip);
                    nowNum++;
                    TutorialObjList[nowNum].SetActive(true);
                }
                
                if(nowNum!=0) TutorialObjList[nowNum-1].SetActive(false);
            }
        }
    }
}
