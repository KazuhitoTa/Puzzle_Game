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
        private List<List<int>> gridState = new List<List<int>>();
        private List<List<Vector3>> tilePosList = new List<List<Vector3>>();
        private List<List<Panel>> panelList = new List<List<Panel>>();

        [SerializeField] private List<GameObject> tilePrefabList;

        int mapSizeX = 6;   // グリッドサイズ(X)
        int mapSizeY = 6;   // グリッドサイズ(Y)
        int goalPosX = 0;   // ゴールの位置(X)
        int goalPosY = 0;   // ゴールの位置(Y)
        bool tmp = false;
        
        private string path="Assets/StageDate/test.csv";
        
        // パネルの構造体
        public struct Panel
        {
            public string col;
            public int num;

            //public Panel(string c, int n)
            //{
            //    col = c;
            //    num = n;
            //}
        };

        // Start is called before the first frame update
        void Start()
        {
            PanelLoading(path);
            GridInit();
        }

        // Update is called once per frame
        void Update()
        {    
            if (Input.GetMouseButtonDown(0)||Input.touchCount > 0)
            {
                GameObject temp=GetClickObj();
                //GridStateCheck(temp);
                GridStateChange(temp);
                float rotationSpeed = -90.0f; // 回転速度（度/秒）
                float rotationAmount = rotationSpeed * Time.deltaTime;
                temp.transform.Rotate(0,0,rotationSpeed);

                bool tmp = CheckPazzleCorrect();

                // パズルの正誤判定
                if (CheckPazzleCorrect())       SceneManager.LoadScene("StageSelect");
                //else if (!CheckPazzleCorrect()) Debug.Log("Incorrect...");
            }

            if (Input.GetKey(KeyCode.Return) && tmp) {
                tmp = false;
                /*
                Debug.Log("\n" + 
                    gridState[4][0] + " " + gridState[4][1] + " " + gridState[4][2] + " " + gridState[4][3] + " " + gridState[4][4] + "\n" +
                    gridState[3][0] + " " + gridState[3][1] + " " + gridState[3][2] + " " + gridState[3][3] + " " + gridState[3][4] + "\n" +
                    gridState[2][0] + " " + gridState[2][1] + " " + gridState[2][2] + " " + gridState[2][3] + " " + gridState[2][4] + "\n" +
                    gridState[1][0] + " " + gridState[1][1] + " " + gridState[1][2] + " " + gridState[1][3] + " " + gridState[1][4] + "\n" +
                    gridState[0][0] + " " + gridState[0][1] + " " + gridState[0][2] + " " + gridState[0][3] + " " + gridState[0][4]
                    );
                */
            }
            if (!Input.GetKey(KeyCode.Return) && !tmp) tmp = true;
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

                // 
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
                // デバッグ
                /*
                for (int i = 0; i < panelList.Count; i++)
                {
                    for (int j = 0; j < panelList[0].Count; j++)
                    {
                        Debug.Log(panelList[i][j].col + ", " + panelList[i][j].num);
                    }
                }
                */
            }
            catch (Exception e)
            {
                Debug.LogError("CSV読み込みエラー: " + e.Message);
            }
        }


        void GridInit()
        {
            //2次元配列の初期化
            for (int i = 0; i < mapSizeX; i++)
            {
                grid.Add(new List<GameObject>());
                gridState.Add(new List<int>());
                tilePosList.Add(new List<Vector3>());
                for (int h = 0; h < mapSizeY; h++)
                {
                    grid[i].Add(null);
                    gridState[i].Add(0);
                    tilePosList[i].Add(new Vector3(0, 0, 0));
                }
            }

            // -----------------------------------
            // 本来はココで csvファイル を読み込む
            // -----------------------------------
            gridState[4][0] = 4; gridState[4][1] = 2; gridState[4][2] = 2; gridState[4][3] = 2; gridState[4][4] = 8;
            gridState[3][0] = 1; gridState[3][1] = 4; gridState[3][2] = 2; gridState[3][3] = 2; gridState[3][4] = 5;
            gridState[2][0] = 3; gridState[2][1] = 6; gridState[2][2] = 0; gridState[2][3] = 0; gridState[2][4] = 1;
            gridState[1][0] = 0; gridState[1][1] = 0; gridState[1][2] = 4; gridState[1][3] = 2; gridState[1][4] = 6;
            gridState[0][0] = 7; gridState[0][1] = 2; gridState[0][2] = 6; gridState[0][3] = 0; gridState[0][4] = 0;
            // ゴール位置の取得
            for (int y = 0; y < mapSizeY; y++)
            {
                for (int x = 0; x < mapSizeX; x++)
                {
                    if (gridState[y][x] == 8)
                    {
                        goalPosX = x;
                        goalPosY = y;
                        //Debug.Log(goalPosY + ", " + goalPosX);
                    }
                }
            }
            // ダミーパネルの生成 & パネルのランダム回転
            SetGridRandom();

            for (int row = 0; row < mapSizeX; row++)
            {
                for (int col = 0; col <mapSizeY; col++)
                {
                    Vector3 posTemp=new Vector3(-2+col, -2.5f+row, 0);
                    tilePosList[row][col] = posTemp;
                    // スタート 左下
                    if (row==0&&col==0)
                    {
                        gridState[row][col]=7;
                        GameObject tile = Instantiate(tilePrefabList[7], posTemp, tilePrefabList[7].transform.rotation);
                        grid[row][col]=tile;
                    }
                    // ゴール 右上
                    else if(row==mapSizeX-1&&col==mapSizeY-1)
                    {
                        gridState[row][col]=8;
                        GameObject tile = Instantiate(tilePrefabList[8], posTemp, tilePrefabList[8].transform.rotation);
                        grid[row][col]=tile;
                    }
                    // その他
                    else
                    {
                        GameObject tile = Instantiate(tilePrefabList[gridState[row][col]], posTemp, tilePrefabList[gridState[row][col]].transform.rotation);
                        grid[row][col] = tile;
                    }
                    
                }
            }
        }

        public void GridStateCheck(GameObject tempObject)
        {
            for (int row = 0; row < mapSizeX; row++)
            {
                for (int col = 0; col <mapSizeY; col++)
                {
                    if(tempObject==grid[row][col])
                    {
                        //Debug.Log(row);
                        //Debug.Log(col);
                    }     
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
                        if (gridState[row][col] == 1)       gridState[row][col] = 2;
                        else if (gridState[row][col] == 2)  gridState[row][col] = 1;
                        else if (gridState[row][col] < 6)   gridState[row][col]++;
                        else if (gridState[row][col] == 6)  gridState[row][col] = 3;
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
            for (int a = 0; a < mapSizeY; a++)
            {
                for (int b = 0; b < mapSizeX; b++)
                {
                    switch (gridState[a][b])
                    {
                        case 0:
                            gridState[a][b] = UnityEngine.Random.Range(1, 7);
                            break;
                        case 1:
                        case 2:
                            gridState[a][b] = UnityEngine.Random.Range(1, 3);
                            break;
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                            gridState[a][b] = UnityEngine.Random.Range(3, 7);
                            break;
                    }
                }
            }
        }

        // パズルの正誤判定
        bool CheckPazzleCorrect()
        {
            int y = goalPosY;
            int by = y;
            int x = goalPosX;
            int bx = x;
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
                // gridState毎の処理
                switch (gridState[y][x])
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
                    case 7: // スタート
                        return true;
                    case 8: // ゴール
                        bx = x;
                        x--;
                        break;
                    default:
                        fin = true;
                        break;
                }
            }
            //Debug.Log(y + ", " + x);
            //Debug.Log("Status : " + gridState[y][x]);
            return false;
        }
    }

}
