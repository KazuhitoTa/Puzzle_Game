using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gomi : MonoBehaviour
{
    
    [SerializeField]List<GameObject> objList=new List<GameObject>();

    [SerializeField]List<Color> color=new List<Color>();
   
    int nowNum=0;
    
    public void Start()
    {
        Debug.Log(objList.Count);
        //tutorialObjList[0].SetActive(true);
        objList.Add(null);
    }

    public  void Update()
    {
        Debug.Log(objList.Count);
    }

}
