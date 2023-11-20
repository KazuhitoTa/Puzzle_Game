using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Count : MonoBehaviour
{
    [SerializeField] Ready ready;
    public void ReadyCount()
    {
        ready.ChangeNum();
    }
}
