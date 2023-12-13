using System.Collections;
using System.Collections.Generic;
using kurukuru;
using UnityEngine;
using UnityEngine.UI;

public class Pause :MonoBehaviour
{
    [SerializeField]List<GameObject> pauseObjectsList=new List<GameObject>();

    [SerializeField]GameManager gameManager;

    [SerializeField]List<Button> buttons=new List<Button>();
    public void PauseStart()
    {
        foreach (var item in pauseObjectsList)
        {
            item.SetActive(true);
        }

        foreach (var item in buttons)
        {
            item.enabled=false;
        }
    }

    public void PauseUpdate()
    {
        
    }

    public void PauseEnd()
    {
        foreach (var item in pauseObjectsList)
        {
            item.SetActive(false);
        }
        
    }

    

    public void EndPause()
    {
        PauseEnd();
        gameManager.ChangeGameState(GameManager.GameState.Play);
    }
}
