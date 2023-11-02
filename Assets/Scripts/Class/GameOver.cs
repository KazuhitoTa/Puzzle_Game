using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField]List<GameObject> gameObjects=new List<GameObject>();
    public void GameOverStart()
    {
        foreach (var item in gameObjects)
        {
            item.SetActive(true);
        }
    }

    public void GameOverUpdate()
    {
        
    }
}
