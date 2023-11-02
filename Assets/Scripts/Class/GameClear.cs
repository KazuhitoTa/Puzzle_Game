using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour
{
    [SerializeField]List<GameObject> gameObjects=new List<GameObject>();
    public void GameClearStart()
    {
        Debug.Log("start");
        foreach (var item in gameObjects)
        {
            item.SetActive(true);
        }
    }

    public void GameClearUpdate()
    {
        
    }

    public void GameClearEnd()
    {
        foreach (var item in gameObjects)
        {
            item.SetActive(false);
        }
    }
}
