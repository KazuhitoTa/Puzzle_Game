using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{

    //妥協
    public static int stageNumber;

    void Update()
    {
        if(Input.GetKey(KeyCode.Return))
        {
            stageNumber++;
            Debug.Log(stageNumber);
        }
        
    }

    public void GoGame()
    {
        SceneManager.LoadScene("kurukuruGame");
    }
}
