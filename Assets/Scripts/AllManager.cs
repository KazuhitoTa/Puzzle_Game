using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AllManager : MonoBehaviour
{
    public static AllManager instance;

    //private StageState stageState;

    enum StageState
    {
        Title,
        StageSelect,
        Puzzle 
    }

    void Awake()
    {
        CheckInstance();
    }

   void Start()
   {
        DontDestroyOnLoad(gameObject);
        //stageState=StageState.Title;
   }

    void CheckInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    } 

    public void OnClick()
    {
        SceneManager.LoadScene("StageSelect");
        //stageState=StageState.StageSelect;
    }
}  
