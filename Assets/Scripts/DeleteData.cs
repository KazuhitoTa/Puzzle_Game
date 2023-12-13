using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class DeleteData : MonoBehaviour
{
    public GameObject checkImg;

    private string fileName;
    private string filePath;
    string[] initialData = { "StageData,0", "SelectData,0", "BGM,1", "SE,1", "tutorial,false" };
    
    void Start()
    {
        fileName = "SaveData.csv";
        filePath = System.IO.Path.Combine(Application.persistentDataPath, fileName);
    }
    public void CheckUp()
    {
        checkImg.SetActive(true);
    }

    public void CheckExit()
    {
        checkImg.SetActive(false);
    }

    public void ReCreateData()
    {
        File.WriteAllLines(filePath, initialData);
        SceneManager.LoadScene("SplashScreen");
    }
}
