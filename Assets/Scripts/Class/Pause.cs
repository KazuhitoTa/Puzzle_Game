using System.Collections;
using System.Collections.Generic;
using kurukuru;
using UnityEngine;

public class Pause :MonoBehaviour
{
    [SerializeField]List<GameObject> pauseObjectsList=new List<GameObject>();

    [SerializeField]GameManager gameManager;
    public void PauseStart()
    {
        foreach (var item in pauseObjectsList)
        {
            item.SetActive(true);
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

    public void SetPause()
    {
        gameManager.ChangeGameState(GameManager.GameState.Pause);
    }

    public void EndPause()
    {
        PauseEnd();
        gameManager.ChangeGameState(GameManager.GameState.Play);
    }
}
