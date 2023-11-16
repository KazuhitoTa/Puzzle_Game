using System.Collections;
using System.Collections.Generic;
using kurukuru;
using Unity.VisualScripting;
using UnityEngine;

public class Ready : MonoBehaviour
{
    [SerializeField]List<GameObject> readyCount=new();
    [SerializeField]GameObject readyObj;
    [SerializeField] GameManager gameManager;
    private int nowNum=0;
    public void ReadyStart()
    {
        readyObj.SetActive(true);
        readyCount[0].SetActive(true);
    }

    public void ReadyUpdate()
    {
        
    }

    public void ChangeNum()
    {
        if(nowNum==0)
        {
            readyCount[0].SetActive(false);
            readyCount[1].SetActive(true);
            nowNum=1;
        }
        else if(nowNum==1)
        {
            readyCount[1].SetActive(false);
            readyCount[2].SetActive(true);
            nowNum=2;
        }
        else
        {
            gameManager.ChangeGameState(GameManager.GameState.Play);
            readyCount[2].SetActive(false);
            readyObj.SetActive(false);
            nowNum=0;
        }
    }
}
