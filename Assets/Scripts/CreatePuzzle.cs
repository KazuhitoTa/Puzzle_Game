using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePuzzle : MonoBehaviour
{
   //Mapの生成
   private List<List<GameObject>> grid = new List<List<GameObject>>();
   private List<List<int>> gridState=new List<List<int>>();
   [SerializeField]private List<GameObject> piecePrefab=new List<GameObject>();

   [SerializeField]private GameObject[] MapData;

   public GameObject tilePrefab; // タイルのプレハブをInspectorから割り当てる

   public GameObject catchGameObject;
   private List<List<Vector3>> tilePos=new List<List<Vector3>>();
   [SerializeField]private List<Vector3> piecePos=new List<Vector3>();

   [SerializeField]private List<panelType> panelList=new List<panelType>();


    void Start()
    {
       
        int mapSizeX=5;
        int mapSizeY=5;


        //2次元配列の初期化
        for (int i = 0; i < mapSizeX; i++)
        {
            grid.Add(new List<GameObject>());
            gridState.Add(new List<int>());
            tilePos.Add(new List<Vector3>());
            for (int h = 0; h < mapSizeY; h++)
            {
                gridState[i].Add(0);
                grid[i].Add(null);
                tilePos[i].Add(Vector3.zero);
            }
        }


        //Tileの生成と初期化
        for (int row = 0; row < mapSizeX; row++)
        {
            for (int col = 0; col <mapSizeY; col++)
            {
                Vector3 posTemp=new Vector3(-2+col, -2.5f+row, 0);
                GameObject tile = Instantiate(tilePrefab, posTemp, Quaternion.identity);
                grid[row][col]=tile;
                gridState[row][col]=0;
                tilePos[row][col]=posTemp;
                
            }
        }

        // for (int i = 1; i <= 4; i++)
        // {
        //     Instantiate(piecePrefab[i], piecePos[i - 1], piecePrefab[i].transform.rotation);
        // }
        
        

    }

    void Update()
    {
        int mapSizeX=5;
        int mapSizeY=5;
        int clickedObjectNumber=0;
        if (Input.GetMouseButtonDown(0)||Input.touchCount > 0)
        {
            catchGameObject=GetClickObj();
        }

        //gridをクリックしたときにそのgridの状態をわかるようにする

        
        // マウスの位置を取得
        Vector3 mousePosition = Input.mousePosition;

        // マウスの位置をワールド座標に変換
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

        //つかんだオブジェクトをマウスの位置に追従
        if(catchGameObject!=null)
        {
            for (int row = 0; row < mapSizeX; row++)
            {
                for (int col = 0; col <mapSizeY; col++)
                {
                    if(catchGameObject.transform.position==tilePos[row][col])gridState[row][col]=0;
                }
            }
            switch (catchGameObject.tag)
            {
                case "redHeight":
                    clickedObjectNumber = 1;
                    break;
                case "redWeght":
                    clickedObjectNumber = 2;
                    break;
                case "redL":
                    clickedObjectNumber = 3;
                    break;
                case "redL90":
                    clickedObjectNumber = 4;
                    break;
                case "redL180":
                    clickedObjectNumber = 5;
                    break;
                case "redL270":
                    clickedObjectNumber = 6;
                    break;
            }

            Debug.Log(clickedObjectNumber);         
            
            catchGameObject.transform.position=worldMousePosition;
        }

        if(Input.GetMouseButtonUp(0))
        {
            Vector3 nearestVector=Vector3.zero;
            if(catchGameObject!=null)
            {
                nearestVector = FindNearestVector(tilePos, catchGameObject.transform.position);
                if(Vector3.Distance(catchGameObject.transform.position,nearestVector)<=1.0f)
                {
                    (int,int) numberTemp=(0,0);
                    //nearestVectorとtilePosの中身を比較して同じ番号のものを探す
                    for (int row = 0; row < mapSizeX; row++)
                    {
                        for (int col = 0; col <mapSizeY; col++)
                        {
                            if(nearestVector==tilePos[row][col])
                            {
                                numberTemp.Item1=row;
                                numberTemp.Item2=col;
                            }
                        }
                    }

                    //tilePosと同じ番号のpieceStateを探してそのデータをピースの番号に書き換える
                    Debug.Log(clickedObjectNumber);
                    gridState[numberTemp.Item1][numberTemp.Item2]=clickedObjectNumber;
                    

                    catchGameObject.transform.position=nearestVector;
                }
                
            }
            
            catchGameObject=null;
        }

        

        
    }

    /// <summary>
    /// クリックしたオブジェクトを保持する
    /// </summary>
    /// <returns></returns>
    GameObject GetClickObj()
    {
        GameObject clickedObject = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
    
        if (hit.collider != null) 
        {
            clickedObject = hit.collider.gameObject;
        }
        
        return clickedObject;
        
    }

    /// <summary>
    /// Vector3の２次元Listの要素からVector3と一番近いものを見つけてListの要素に合わせる
    /// </summary>
    /// <param name="list">合わせるVector3のList</param>
    /// <param name="target">合わせたいVector3</param>
    /// <returns></returns>
    private Vector3 FindNearestVector(List<List<Vector3>> list, Vector3 target)
    {
        Vector3 nearestVector = list[0][0]; // 2次元リストの最初の要素を初期化値として設定
        float closestDistance = Vector3.Distance(target, nearestVector);

        // 2次元リスト内の各要素との距離を比較して最も近いVector3を見つける
        foreach (var sublist in list)
        {
            foreach (var vector in sublist)
            {
                float distance = Vector3.Distance(target, vector);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestVector = vector;
                }
            }
        }

        return nearestVector;
    }

    [System.Serializable]
    public class panelType
    {
        public List<GameObject> panelTypeList =new List<GameObject>();
    }

    

}
    

    


