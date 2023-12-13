using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleManager : MonoBehaviour
{
    public GameObject Pathykuru;
    public int clickcount;
 
    void Update()
    {
        Vector3 mousepos = Input.mousePosition;//マウスポインターの位置取得
        mousepos.z = 10.0f;//マウスの座標を固定
        Vector3 objpos = Camera.main.ScreenToWorldPoint(mousepos);//マウスの座標をカメラサイズに変更

        if(Input.GetMouseButton(0))clickcount++;//右クリックしたら+1
        else clickcount=0;
        if(clickcount==1)
        {
            GameObject particl=Instantiate(Pathykuru,objpos,Quaternion.identity);//マウスのクリックした座標にパーティクルを出す
            Destroy(particl,1.0f);//1秒後に破壊

        }
    }
}
