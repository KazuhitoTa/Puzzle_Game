using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace kurukuru
{
    public class GameManager : MonoBehaviour
    {
        private List<List<GameObject>> grid = new List<List<GameObject>>();
        private List<List<Panel>> gridState = new List<List<Panel>>();          
        private List<List<Vector3>> tilePosList = new List<List<Vector3>>();
        private List<List<Panel>> panelList = new List<List<Panel>>();

        [SerializeField] private List<PanelPrefab> panelPrefabList=new List<PanelPrefab>();

        [SerializeField]GameObject clearUI;

        private List<bool> clearFlag=new List<bool>();

        [SerializeField] GameObject puzzleBoard;
        private List<bool> flowmeterCheck=new List<bool>();

        int mapSizeX = 5;   // グリッドサイズ(X)           
        int mapSizeY = 5;   // グリッドサイズ(Y)          
        Vector2[] goalPos = // ゴールの位置          
        {
            new Vector2(-1, -1),    // RED
            new Vector2(-1, -1),    // GREEN
            new Vector2(-1, -1),    // BLUE
        };
        
        
        //private string path = "Assets/StageDate/test02.csv";    // 1 Colors
        private string path = "Assets/StageDate/test.csv";      // 2 Colors

        // パネルの構造体
        public struct Panel
        {
            public string col;
            public int num;
        };

        // パネルPrefabの構造体
        [System.Serializable]
        public struct PanelPrefab
        {
            public List<GameObject> colorList;
        };

        // 色の列挙         
        enum COLOR
        {
            RED = 0,
            GREEN = 1,
            BLUE = 2,
        }

        void Start()
        {
            PanelLoading(path);
            GridInit();
        }

        void Update()
        {    
            if (Input.GetMouseButtonDown(0)||Input.touchCount > 0)
            {
                GameObject temp=GetClickObj();
    
                if(temp!=null)
                {
                    GridStateChange(temp);
                    float rotationSpeed = -90.0f; 
                    float rotationAmount = rotationSpeed * Time.deltaTime;
                    temp.transform.Rotate(0,0,rotationSpeed);

                    string clickColor = GetColor(temp);
                    Debug.Log(clickColor);

                    // パズルの正誤判定
                    clearFlag[GetNumber(clickColor)]=CheckPazzleCorrect(clickColor);

                    Debug.Log(clearFlag[0]);

                    if(clearFlag[0]&&clearFlag[1]&&clearFlag[2])
                    {
                        foreach (Transform child in puzzleBoard.transform)
                        {
                            Destroy(child.gameObject);
                        }
                        clearUI.SetActive(true);
                        Debug.Log("clear");
                    }
                }
            }

        }

        void PanelLoading(string filePath)
        {
            try
            {
                List<string[]> data = new List<string[]>();

                // ファイルの全体を読み込み、各行をリストに格納
                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] elements = line.Split(','); // カンマで要素を分割
                        data.Add(elements);
                    }
                }

                int listCounter = 0;    // 行をカウント
                

                foreach (string[] elements in data)
                {
                    panelList.Add(new List<Panel>());
                    int elementCounter = 0; //要素数をカウント（奇数番目、偶数番目を識別）
                    string tempColor = "";  // 色
                    int tempNumber = 0;     // 数字

                    foreach (string element in elements)
                    {

                        if (elementCounter % 2 == 0) // 偶数番目（csvの奇数番目）は色
                        {
                            tempColor = element;
                        }
                        else // 奇数番目（csvの偶数番目）は数字
                        {
                            if(int.TryParse(element, out tempNumber))
                            {
                                panelList[listCounter].Add(new Panel { col = tempColor, num = tempNumber });
                                //Debug.Log("読み込み：　" + tempColor + ", " + tempNumber);
                            }
                            else
                            {
                                Debug.Log("読み込み失敗：　" + listCounter + "," + elementCounter/2 + "番目");
                            }
                        }
                        elementCounter++;
                    }
                    listCounter++;
                    //Debug.Log("------"); // 行の終わりに区切りを表示
                }
            }
            catch (Exception e)
            {
                Debug.LogError("CSV読み込みエラー: " + e.Message);
            }

            mapSizeX=panelList[0].Count;
            mapSizeY=panelList.Count;
        }


        void GridInit()
        {   
            //配列の初期化
            for(int i=0;i<3;i++)
            {
                flowmeterCheck.Add(true);
            }

            //2次元配列の初期化
            for (int i = 0; i < mapSizeX; i++)
            {
                grid.Add(new List<GameObject>());
                gridState.Add(new List<Panel>());           
                tilePosList.Add(new List<Vector3>());
                for (int h = 0; h < mapSizeY; h++)
                {
                    grid[i].Add(null);
                    gridState[i].Add(new Panel { col = "", num = 0 });          
                    tilePosList[i].Add(new Vector3(0, 0, 0));
                }
            }

            // マップ生成            
            for (int r = 0; r < mapSizeY; r++)
            {
                for (int c = 0; c < mapSizeX; c++)
                {
                    // panelList の内容を gridState に引き継ぎ
                    gridState[r][c] = panelList[r][c];
                    
                    //流量計があるか確認
                    if(gridState[r][c].num==15||gridState[r][c].num==16)
                    {
                        if(gridState[r][c].col=="red")flowmeterCheck[0]=false;
                        else if (gridState[r][c].col == "green")flowmeterCheck[1]=false;
                        else if (gridState[r][c].col == "blue")flowmeterCheck[2]=false;
                    }

                    //Debug.Log("gridState[" + r + "][" + c + "].col = " + gridState[r][c].col);

                    // ゴール位置の記憶
                    if (gridState[r][c].num == 11||gridState[r][c].num == 12||gridState[r][c].num == 13||gridState[r][c].num == 14)
                    {
                        if      (gridState[r][c].col == "red")    goalPos[(int)COLOR.RED]   = new Vector2(c, r);
                        else if (gridState[r][c].col == "green")  goalPos[(int)COLOR.GREEN] = new Vector2(c, r);
                        else if (gridState[r][c].col == "blue")   goalPos[(int)COLOR.BLUE]  = new Vector2(c, r);
                    }
                }
            }


            // ゴール位置の Debug 表示
            Vector2 invalid = new Vector2(-1, -1);
            if (goalPos[(int)COLOR.RED]   == invalid) clearFlag.Add(true);
            else clearFlag.Add(false);
            if (goalPos[(int)COLOR.GREEN] == invalid) clearFlag.Add(true);
            else clearFlag.Add(false);
            if (goalPos[(int)COLOR.BLUE]  == invalid) clearFlag.Add(true);
            else clearFlag.Add(false);

            // ダミーパネルの生成 & パネルのランダム回転
            SetGridRandom();
            
            for (int row = 0; row < mapSizeX; row++)
            {
                for (int col = 0; col < mapSizeY; col++)
                {
                    //マスの数によってTileの大きさ変更
                    float tileScaleX=(5f/(float)mapSizeX);
                    float tileScaleY=(5f/(float)mapSizeY);

                    //生成位置
                    Vector3 posTemp=new Vector3((-2f-((1f-tileScaleX)/2))+tileScaleX*col, -2f-((1f-tileScaleY)/2)+tileScaleY*row, 0);
                    
                    //生成する座標をtilePosListに保存
                    tilePosList[row][col] = posTemp;

                    // パネルの番号を仮置き         
                    int colorNumTemp=GetNumber(gridState[row][col].col);
                    
                    //パネルの種類を仮置き
                    int  panelGenreTemp=gridState[row][col].num;

                    //Tileを生成してする
                    GameObject tile = Instantiate(panelPrefabList[colorNumTemp].colorList[panelGenreTemp], posTemp, panelPrefabList[colorNumTemp].colorList[panelGenreTemp].transform.rotation);
                    
                    //生成したTileの大きさを変更
                    tile.transform.localScale = new Vector3(tileScaleX,tileScaleY,1);

                    //puzzleBoardの子供オブジェクトにする
                    tile.transform.parent=puzzleBoard.transform;
                    
                    //生成したTileをgridに保存
                    grid[row][col] = tile;

                }
            }
        }

        public void GridStateChange(GameObject tempObject)
        {
            for (int row = 0; row < mapSizeX; row++)
            {
                for (int col = 0; col <mapSizeY; col++)
                {
                    if (tilePosList[row][col] == tempObject.transform.position)
                    {
                        var tmp = gridState[row][col];  // 仮変数
                        // タップしたパネルの種類毎の gridState 変更処理         
                        // 1. 直線(縦)の場合
                        if (gridState[row][col].num == 1)
                        {
                            tmp.num = 2;
                            gridState[row][col] = tmp;
                        }
                        // 2. 直線(横)の場合
                        else if (gridState[row][col].num == 2)
                        {
                            tmp.num = 1;
                            gridState[row][col] = tmp;
                        }
                        // 3. L字の場合
                        else if (gridState[row][col].num < 6)
                        {
                            tmp.num++;
                            gridState[row][col] = tmp;
                        }
                        // 3+. L字回転リセット
                        else if (gridState[row][col].num == 6)
                        {
                            tmp.num = 3;
                            gridState[row][col] = tmp;
                        }
                    }
                    //Debug.Log(gridState[row][col]);
                }
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

        // ダミーパネルの生成 & パネルのランダム回転           
        void SetGridRandom()
        {
            for (int r = 0; r < mapSizeY; r++)
            {
                for (int c = 0; c < mapSizeX; c++)
                {
                    var tmp = gridState[r][c];  // 値の入れ替え用の仮変数
                    // 分岐処理 (パネルのランダム変更)
                    switch (gridState[r][c].num)
                    {
                        // 1. 空白
                        case 0:
                            // clearFlag が false の色のみ取得
                            List<string> strings = new List<string>();
                            if (!clearFlag[(int)COLOR.RED])   strings.Add("red");
                            if (!clearFlag[(int)COLOR.GREEN]) strings.Add("green");
                            if (!clearFlag[(int)COLOR.BLUE])  strings.Add("blue");
                            // 全てのフラグが立っている場合
                            if (strings.Count <= 0)
                            {
                                Debug.Log("All clearFlag is True");
                                break;
                            }
                            // ランダム化
                            tmp.col = strings[UnityEngine.Random.Range(0, strings.Count)];
                            tmp.num = UnityEngine.Random.Range(1, 7);
                            break;
                        // 2. 縦 or 横
                        case 1:
                        case 2:
                            // ランダム化
                            tmp.num = UnityEngine.Random.Range(1, 3);
                            break;
                        // 3. L字
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                            // ランダム化
                            tmp.num = UnityEngine.Random.Range(3, 7);
                            break;
                    }
                    gridState[r][c] = tmp;  // 値入れ替え
                }
            }
        }

        // パズルの正誤判定
        bool CheckPazzleCorrect(string checkColor)
        {
            bool check=false;

            Debug.Log("checkColor : " + checkColor);
            // 判定色の読み込み
            int tmpcol = 0;
            if (checkColor == "red") tmpcol = (int)COLOR.RED;
            else if (checkColor == "green") tmpcol = (int)COLOR.GREEN;
            else if (checkColor == "blue") tmpcol = (int)COLOR.BLUE;
            else Debug.Log("checkColor Error");
            // 座標変数
            int y = (int)goalPos[tmpcol].y;
            int by = y;
            int x = (int)goalPos[tmpcol].x;
            int bx = x;
            // ループステート
            bool fin = false;

            while(!fin)
            {
                // 枠外に出ている場合終了
                if((y >= mapSizeY) || (y < 0) || (x >= mapSizeX) || (x < 0))
                {
                    //Debug.Log("[Out of Range] " + y + ", " + x);
                    //Debug.Log("Status : " + gridState[y][x]);
                    return false;
                }
                // 次のパネルの色が違う場合終了
                else if (gridState[y][x].col != checkColor)
                {
                    Debug.Log("[ Color is Not Much ]");
                    Debug.Log("x:" + x + " y:" + y);
                    return false;
                }
                // gridState毎の処理
                switch (gridState[y][x].num)
                {
                    case 0:
                        fin = true;
                        break;
                    case 1:
                        if (y > by)
                        {
                            by = y;
                            y++;
                        }
                        else if (y < by)
                        {
                            by = y;
                            y--;
                        }
                        else fin = true;
                        break;
                    case 2:
                        if (x > bx)
                        {
                            bx = x;
                            x++;
                        }
                        else if (x < bx)
                        {
                            bx = x;
                            x--;
                        }
                        else fin = true;
                        break;
                    case 3:
                        if (y < by)
                        {
                            by = y;
                            x++;
                        }
                        else if (x < bx)
                        {
                            bx = x;
                            y++;
                        }
                        else fin = true;
                        break;
                    case 4:
                        if (x < bx)
                        {
                            bx = x;
                            y--;
                        }
                        else if (y > by)
                        {
                            by = y;
                            x++;
                        }
                        else fin = true;
                        break;
                    case 5:
                        if (y > by)
                        {
                            by = y;
                            x--;
                        }
                        else if (x > bx)
                        {
                            bx = x;
                            y--;
                        }
                        else fin = true;
                        break;
                    case 6:
                        if (x > bx)
                        {
                            bx = x;
                            y++;
                        }
                        else if (y < by)
                        {
                            by = y;
                            x--;
                        }
                        else fin = true;
                        break;
                    // スタート
                    case 7:
                        if (y < by&&(check||flowmeterCheck[GetNumber(checkColor)]))
                        {
                            return true;
                        }
                        else if (y < by)
                        {
                            return false;
                        }
                        else break;
                    case 8:
                        if (y > by&&(check||flowmeterCheck[GetNumber(checkColor)]))
                        {
                            return true;
                        }
                        else if (y > by)
                        {
                            return false;
                        }
                        else break;
                    case 9:
                        if (x > bx&&(check||flowmeterCheck[GetNumber(checkColor)]))
                        {
                            return true;
                        }
                        else if(x > bx)
                        {
                            return false;
                        }
                        else break;
                    case 10:
                        if (x < bx&&(check||flowmeterCheck[GetNumber(checkColor)]))
                        {
                            return true;
                        }
                        else if (x < bx)
                        {
                            return false;
                        }
                        else break;
                    // ゴール
                    case 11:
                        by = y;
                        y++;
                        break;
                    case 12:
                        by = y;
                        y--;
                        break;
                    case 13:
                        bx = x;
                        x--;
                        break;
                    case 14:
                        bx = x;
                        x++;
                        break;
                    case 15:
                        if (y > by)
                        {
                            by = y;
                            y++;
                            check=true;
                        }
                        else if (y < by)
                        {
                            by = y;
                            y--;
                            check=true;
                        }
                        else fin = true;
                        break;
                    case 16:
                        if (x > bx)
                        {
                            bx = x;
                            x++;
                            check=true;
                        }
                        else if (x < bx)
                        {
                            bx = x;
                            x--;
                            check=true;
                        }
                        else fin = true;
                        break;
                    
                    default:
                        fin = true;
                        break;
                }
            }
            
            return false;
        }

        // クリックされたパネルの色を判別
        string GetColor(GameObject obj)
        {
            for (int i = 0; i < grid.Count; i++)
            {
                int j = grid[i].IndexOf(obj);
                if (j >= 0)
                {
                    Panel panelTemp = gridState[i][j];
                    return panelTemp.col;
                }
            }
            return "Color not found";
        }

        int GetNumber(string color)
        {
            if(color=="red")return 0;
            else if(color=="green")return 1;
            else if(color=="blue")return 2;
            else
            {
                Debug.LogError("Color not found");
                return 0;
            }
        }
    }

}
