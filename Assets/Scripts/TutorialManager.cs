using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public List<GameObject> tutorialObjList=new();
    int nowNum=0;
    // Start is called before the first frame update
    void Start()
    {
        
        foreach (var item in tutorialObjList)
        {
            item.SetActive(false);
        }
        tutorialObjList[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
        {
             
            Touch touch;
            if (Input.touchCount > 0)
                touch = Input.GetTouch(0);
            else
                touch = new Touch();

            if (touch.phase == TouchPhase.Began)
            {
                if(nowNum==5)
                {
                    nowNum=0;
                    tutorialObjList[5].SetActive(false);
                }
                
                
                else nowNum++;
                tutorialObjList[nowNum].SetActive(true);
                if(nowNum!=0) tutorialObjList[nowNum-1].SetActive(false);
            }
        }
    }
}
