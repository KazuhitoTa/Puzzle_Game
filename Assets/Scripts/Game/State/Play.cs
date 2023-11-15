using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using kurukuru;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    [SerializeField]GameObject startOChanPrefab;
    [SerializeField]private List<GameObject> startOChan=new List<GameObject>(){null,null,null};

    private List<bool> moveEndFlags=new List<bool>(){false,false,false};
    
    [SerializeField]GameObject gameCanvas;
    [SerializeField]List<GameObject> playerGameObjects=new List<GameObject>(); 
    [SerializeField] List<Animator> animator=new List<Animator>();
    [SerializeField]MapData mapData;
    [SerializeField] AudioSource audioSource;
    [SerializeField]public AudioClip sound1;
    [SerializeField]List<Color> leadyTimeBarColor=new List<Color>();
    private List<List<GameObject>> grid = new List<List<GameObject>>();
    private List<List<Panel>> gridState = new List<List<Panel>>();          
    private List<List<Vector3>> tilePosList = new List<List<Vector3>>();
    private List<List<Panel>> panelList = new List<List<Panel>>();

    [SerializeField] private List<PanelPrefab> panelPrefabList=new List<PanelPrefab>();

    [SerializeField]private List<GameObject> buttonTempList=new List<GameObject>();
    [SerializeField]private List<GameObject> buttonTempList2=new List<GameObject>();

    [SerializeField]private GameObject hpBar;
    [SerializeField]private Image playerHpBarImage;
    [SerializeField]private Image enemyHpBarImage;
    [SerializeField] private float playerMaxHp; 
    private float playerNowHp;
    [SerializeField] private float enemyMaxHp; 
    private float enemyNowHp;

    private List<bool> clearFlag=new List<bool>();

    [SerializeField] GameObject puzzleBoard;
    private List<bool> flowmeterCheck=new List<bool>();

    [SerializeField]List<GameObject> clearObjects=new List<GameObject>();

    int mapSizeX = 5;   // グリッドサイズ(X)           
    int mapSizeY = 5;   // グリッドサイズ(Y)

    private GameObject clickObject;     // クリックオブジェクト
    private float rotateSpeed = 600.0f; // 回転速度
    private bool isRotate = false;      // 回転しているかをチェックする
    private float startAngleZ = 0.0f;   // 回転開始時のZ角度
    private float deltaAngleZ = 0.0f;   // 回転したZ角度

    //private int score = 0;  // パズルを解いて得たスコア
    private bool allColorCorrect = false;
    //int separateCount = 0;  // パズル間の管理

    [SerializeField]GameManager gameManager;

    List<TextAsset> csvFiles=new List<TextAsset>();
    List<TextAsset> csvFilesHoge=new List<TextAsset>();

    [SerializeField]GameObject invPanelPrefab;

    float tileScaleX;
    float tileScaleY;

    int buttonNum=-1;

    float startTime=0;
    float countTime;

    bool firstBool=false;
    [SerializeField] GameObject enemyPrefab;

    [SerializeField] Image leadyTimeBar;
    [SerializeField] GameObject untObj;
    List<GameObject> cloud=new List<GameObject>();
    List<GameObject> unt=new List<GameObject>();

    [SerializeField]GameObject oChan;

    private List<List<Vector2>> correctRoot = new List<List<Vector2>>();    // kaneki
    Vector2 invalid = new Vector2(-1, -1);  // kaneki

    private bool timeStop=false;


    Vector2[] goalPos = // ゴールの位置          
    {
        new Vector2(-1, -1),    // RED
        new Vector2(-1, -1),    // GREEN
        new Vector2(-1, -1),    // BLUE
    };

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

    public void PlayStart()
    {
        csvFiles = new List<TextAsset>( Resources.LoadAll<TextAsset>(GetStageFilePath()));
        PanelLoading();
        //PanelLoading("StageData3/s_hard09");

        // 各色クリアルートの確保
        for (int i = 0; i < 3; i++) correctRoot.Add(new List<Vector2>());

        GridInit();
        enemyNowHp=enemyMaxHp;
        playerNowHp=playerMaxHp;
        
        var temp=(GameObject)Instantiate(mapData.Maps[kurukuru.ButtonManager.stageNumber-1].EnemyPrefab);
        temp.transform.SetParent(gameCanvas.transform, false);
        foreach (var item in playerGameObjects)
        {
            var tmp=(GameObject)Instantiate(item);
            tmp.transform.SetParent(gameCanvas.transform, false);
            animator.Add(tmp.GetComponent<Animator>());
        }
        // clearObjects.Add(temp);
        Debug.Log(mapData.Maps[ButtonManager.stageNumber].Time);
        
    }

    public void PlayInit()
    {
        foreach (var item in animator)
        {
            item.enabled=true;
        }
    }


    public void PlayUpdate()
    {
        //oChan.transform.position=Vector3.MoveTowards(oChan.transform.position,test(), 1.0f * Time.deltaTime);

        if(0<countTime&&!timeStop)
        {
            countTime=countTime-Time.deltaTime;
            leadyTimeBar.fillAmount=countTime/mapData.Maps[ButtonManager.stageNumber-1].rt;
            //Debug.Log(countTime);
        }

        if(!timeStop)
        {
            playerNowHp -= Time.deltaTime * mapData.Maps[ButtonManager.stageNumber-1].Time; // 10の速度でHPを減少させる
            
        }
            playerNowHp = Mathf.Max(0, playerNowHp); // HPが0未満にならないように制約をかける

            // HPバーを更新
            playerHpBarImage.fillAmount = playerNowHp / 100f; // HPが0から100の範囲にある場合

        

        enemyHpBarImage.fillAmount=Mathf.Lerp(enemyHpBarImage.fillAmount,enemyNowHp/enemyMaxHp,Time.deltaTime*10f);

        if(enemyNowHp<=0)
        {
            PlayEnd();
            foreach (var item in animator)
            {
                item.SetTrigger("Win");
            }
            gameManager.ChangeGameState(GameManager.GameState.GameClear);
        }
        
        if(playerNowHp<=0)
        {
            foreach (var item in animator)
            {
                item.SetTrigger("Lose");
            }
            gameManager.ChangeGameState(GameManager.GameState.GameOver);
        }
        
        

        if ((Input.touchCount > 0 || Input.GetMouseButtonDown(0)) && GetClickObj() && !isRotate)
        {
             
            Touch touch;
            if (Input.touchCount > 0)
                touch = Input.GetTouch(0);
            else
                touch = new Touch();

            if (touch.phase == TouchPhase.Began)
            {
                audioSource.PlayOneShot(sound1);
                isRotate = true;
                clickObject = GetClickObj();
                startAngleZ = clickObject.transform.localEulerAngles.z;
                //Debug.Log("startAngleZ : " + startAngleZ);
            }
        }


        if (isRotate)
        {
            // 回転計算
            if (startAngleZ == 0) startAngleZ = 360;
            clickObject.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime * -1);
            deltaAngleZ = startAngleZ - clickObject.transform.localEulerAngles.z;
            // 90度ずつ回転させる
            if (Math.Abs(deltaAngleZ) >= 90)
            {
                isRotate = false;
                clickObject.transform.localEulerAngles = new Vector3(0, 0, startAngleZ - 90);

                // gridState を変える
                GridStateChange(clickObject);

                string clickColor = GetColor(clickObject);
                //Debug.Log(clickColor);

                // パズルの正誤判定
                clearFlag[GetNumber(clickColor)] = CheckPazzleCorrect(clickColor);

                //Debug.Log(clearFlag[0]);

                // 全ての clearFlag が true ならクリア
                if (clearFlag[0] && clearFlag[1] && clearFlag[2])
                {
                    allColorCorrect = true;
                    //clearUI.SetActive(true);
                    //Debug.Log("clear");
                }
            }
        }

        if (allColorCorrect)
        {
            //敵の行動一時停止
            
            
            //アニメーション終了後にパズルの時間停止
            //時間制限停止
            timeStop=true;
            //ピースの操作停止
            foreach (var items in grid)
            {
                foreach (var item in items)
                {
                    var temp=item.GetComponent<BoxCollider2D>();
                    if(temp!=null)temp.enabled=false;
                }
            }

            foreach (var item in cloud)
            {
                Destroy(item);
            }
            foreach (var item in unt)
            {
                Destroy(item);
            }
            animator[0].SetTrigger("Atk");

            GetCorrectRoot();
            //アニメーション再生
            MoveOchan(correctRoot);
            allColorCorrect=false;
        }
        if(moveEndFlags[0]&&moveEndFlags[1]&&moveEndFlags[2])
        {
            ResetPazzle(); 
            PanelLoading();
            GridInit(); 
            moveEndFlags[0]=false;
            moveEndFlags[1]=false;
            moveEndFlags[2]=false;
            timeStop=false;
        }
        
    }

    public void PlayEnd()
    {
        foreach (var item in clearObjects)
        {
            item.SetActive(false);
        }
        
        leadyTimeBar.gameObject.SetActive(false);
    }

    string GetStageFilePath()
    {
        return "StageData/StageData"+kurukuru.ButtonManager.stageNumber.ToString();
    }

    void PanelLoading()
    {
        if (csvFiles != null && csvFiles.Count > 0)
        {
            if(csvFilesHoge.Count==0)
            {
                csvFilesHoge.AddRange(csvFiles);
            }
            // ランダムにファイルを選択
            int randomIndex = UnityEngine.Random.Range(0, csvFilesHoge.Count);
            TextAsset randomCSV = csvFilesHoge[randomIndex];

            //ta
            csvFilesHoge.RemoveAt(randomIndex);


            //Debug.Log("csvFiles.Length : " + csvFiles.Length);
            //Debug.Log("randomIndex : " + randomIndex);
            int listCounter = 0;    // 行をカウント
            //Debug.Log(randomCSV);
            if (randomCSV != null)
            {
                // CSV ファイルの中身を取得
                string csvText = randomCSV.text;

                // 例: CSV テキストを改行で分割して行ごとに処理
                string[] lines = csvText.Split('\n');

                panelList.Clear();

                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))    // 行が空でない場合に処理を行う
                    {
                        
                        string[] elements = line.Split(',');
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
                                if (int.TryParse(element, out tempNumber))
                                {
                                    panelList[listCounter].Add(new Panel { col = tempColor, num = tempNumber });
                                    //Debug.Log(listCounter + "行目" + elementCounter / 2 + "個目");
                                    //Debug.Log("読み込み：　" + tempColor + ", " + tempNumber);
                                }
                                else
                                {
                                    //Debug.Log("読み込み失敗：　" + listCounter + "," + elementCounter / 2 + "番目");
                                }
                            }
                            elementCounter++;
                        }
                        listCounter++;
                    }
                }
            }

        }
        else
        {
            Debug.LogError("CSV ファイルが見つかりませんでした。");
        }
        mapSizeX = panelList[0].Count;
        mapSizeY = panelList.Count;
    }
    
    // Self Select File
    void PanelLoading(string CSVName)
    {
        try
        {
            // フォルダ内のCSVファイルのパスを取得
            string csvPath = "Assets/Resources/StageData/" + CSVName + ".csv";
            List<string[]> data = new List<string[]>();
            // ファイルの全体を読み込み、各行をリストに格納
            using (StreamReader reader = new StreamReader(csvPath))
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
                        if (int.TryParse(element, out tempNumber))
                        {
                            panelList[listCounter].Add(new Panel { col = tempColor, num = tempNumber });
                            //Debug.Log("読み込み：　" + tempColor + ", " + tempNumber);
                        }
                        else
                        {
                            Debug.Log("読み込み失敗：　" + listCounter + "," + elementCounter / 2 + "番目");
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
        mapSizeX = panelList[0].Count;
        mapSizeY = panelList.Count;
    }


    void GridInit()
    {   
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);

        // 流量計生成用の仮リスト
        List<Vector2> tmpRedList = new List<Vector2>();
        List<Vector2> tmpGreenList = new List<Vector2>();
        List<Vector2> tmpBlueList = new List<Vector2>();
        bool redFlowMeterFlg=false;
        bool greenFlowMeterFlg=false;
        bool blueFlowMeterFlg=false;

        //配列の初期化
        for(int i=0;i<3;i++)
        {
            flowmeterCheck.Add(true);

            clearFlag.Add(false);
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

                // 流量計になり得る場所をリストに保管
                if ((gridState[r][c].num == 1) || (gridState[r][c].num == 2))
                {
                    if      (gridState[r][c].col == "red")     tmpRedList.Add((new Vector2(c, r)));
                    else if (gridState[r][c].col == "green") tmpGreenList.Add((new Vector2(c, r)));
                    else if (gridState[r][c].col == "blue")   tmpBlueList.Add((new Vector2(c, r)));
                }
                // ゴール位置の記憶
                else if (gridState[r][c].num == 11||gridState[r][c].num == 12||gridState[r][c].num == 13||gridState[r][c].num == 14)
                {
                    if      (gridState[r][c].col == "red")    goalPos[(int)COLOR.RED]   = new Vector2(c, r);
                    else if (gridState[r][c].col == "green")  goalPos[(int)COLOR.GREEN] = new Vector2(c, r);
                    else if (gridState[r][c].col == "blue")   goalPos[(int)COLOR.BLUE]  = new Vector2(c, r);
                }
                // 流量計を書いているかどうか確認
                else if ((gridState[r][c].num == 15) || (gridState[r][c].num == 16))
                {
                    if      (gridState[r][c].col == "red")
                    {
                        redFlowMeterFlg=true;
                        flowmeterCheck[0]=false;
                    }
                    
                    else if (gridState[r][c].col == "green")
                    {
                        greenFlowMeterFlg=true;
                        flowmeterCheck[1]=false;
                    }
                    else if (gridState[r][c].col == "blue")
                    {
                        blueFlowMeterFlg=true;
                        flowmeterCheck[2]=false;
                    }
                    
                }

            }
        }

        // 流量計に進化させる
        // RED
        if (tmpRedList.Any()&&!redFlowMeterFlg)
        {
            Vector2 redFlowMeter = tmpRedList[UnityEngine.Random.Range(0, tmpRedList.Count)];
            var tmp = gridState[(int)redFlowMeter.y][(int)redFlowMeter.x];
            tmp.num += 14;
            gridState[(int)redFlowMeter.y][(int)redFlowMeter.x] = tmp;
            flowmeterCheck[(int)COLOR.RED] = false;

        }
        // GREEN
        if (tmpGreenList.Any()&&!greenFlowMeterFlg)
        {
            Vector2 greenFlowMeter = tmpGreenList[UnityEngine.Random.Range(0, tmpGreenList.Count)];
            var tmp = gridState[(int)greenFlowMeter.y][(int)greenFlowMeter.x];
            tmp.num += 14;
            gridState[(int)greenFlowMeter.y][(int)greenFlowMeter.x] = tmp;
            flowmeterCheck[(int)COLOR.GREEN] = false;
        }
        // BLUE
        if (tmpBlueList.Any()&&!blueFlowMeterFlg)
        {
            Vector2 blueFlowMeter = tmpBlueList[UnityEngine.Random.Range(0, tmpBlueList.Count)];
            var tmp = gridState[(int)blueFlowMeter.y][(int)blueFlowMeter.x];
            tmp.num += 14;
            gridState[(int)blueFlowMeter.y][(int)blueFlowMeter.x] = tmp;
            flowmeterCheck[(int)COLOR.BLUE] = false;
        }




        // ゴール位置の Debug 表示
        // Debug.Log("goalPos[RED]   : " + goalPos[0]);
        // Debug.Log("goalPos[GREEN] : " + goalPos[1]);
        // Debug.Log("goalPos[BLUE]  : " + goalPos[2]);
        // clearFlag のセット

        if (goalPos[(int)COLOR.RED]   == invalid) clearFlag[(int)COLOR.RED] = true;
        else clearFlag[(int)COLOR.RED] = false;
        if (goalPos[(int)COLOR.GREEN] == invalid) clearFlag[(int)COLOR.GREEN] = true;
        else clearFlag[(int)COLOR.GREEN] = false;
        if (goalPos[(int)COLOR.BLUE]  == invalid) clearFlag[(int)COLOR.BLUE] = true;
        else clearFlag[(int)COLOR.BLUE] = false;

        // ダミーパネルの生成 & パネルのランダム回転
        while (true)
        {
            // ランダム化
            SetGridRandom();

            // 始めからゴールされていないようにする
            if ( !(
                (!clearFlag[(int)COLOR.RED]   && CheckPazzleCorrect("red"))   ||
                (!clearFlag[(int)COLOR.GREEN] && CheckPazzleCorrect("green")) ||
                (!clearFlag[(int)COLOR.BLUE]  && CheckPazzleCorrect("blue"))    ))
                break;
        }
        
        for (int row = 0; row < mapSizeX; row++)
        {
            for (int col = 0; col < mapSizeY; col++)
            {
                //マスの数によってTileの大きさ変更
               tileScaleX=(5f/(float)mapSizeX);
               tileScaleY=(5f/(float)mapSizeY);

                //生成位置
                Vector3 posTemp=new Vector3((-2f-((1f-tileScaleX)/2))+tileScaleX*col, -2.8f-((1f-tileScaleY)/2)+tileScaleY*row, 0);
                
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
                //スタートならその位置におーちゃん生成
                if(gridState[row][col].num == 7||gridState[row][col].num == 8||gridState[row][col].num == 9||gridState[row][col].num == 10)
                {
                    if(gridState[row][col].col=="red")
                    {
                        startOChan[0]=Instantiate(startOChanPrefab, posTemp, Quaternion.identity);
                        startOChan[0].transform.localScale = new Vector3(tileScaleX,tileScaleY,1);
                    }
                    else if(gridState[row][col].col=="green")
                    {
                        startOChan[1]=Instantiate(startOChanPrefab, posTemp, Quaternion.identity);
                        startOChan[1].transform.localScale = new Vector3(tileScaleX,tileScaleY,1);
                    }
                    else if(gridState[row][col].col=="blue")
                    {
                        startOChan[2]=Instantiate(startOChanPrefab, posTemp, Quaternion.identity);
                        startOChan[2].transform.localScale = new Vector3(tileScaleX,tileScaleY,1);
                    }
                    
                }

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

        //Debug.Log("checkColor : " + checkColor);
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
                //Debug.Log("[ Color is Not Much ]");
                //Debug.Log("x:" + x + " y:" + y);
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
                    else fin=true; 
                    break;
                case 8:
                    if (y > by&&(check||flowmeterCheck[GetNumber(checkColor)]))
                    {
                        return true;
                    }
                    else if (y > by)
                    {
                        return false;
                    }
                    else fin=true;  
                    break;
                case 9:
                    if (x > bx&&(check||flowmeterCheck[GetNumber(checkColor)]))
                    {
                        return true;
                    }
                    else if(x > bx)
                    {
                        return false;
                    }
                    else fin=true;  
                    break;
                case 10:
                    if (x < bx&&(check||flowmeterCheck[GetNumber(checkColor)]))
                    {
                        return true;
                    }
                    else if (x < bx)
                    {
                        return false;
                    }
                    else fin=true; 
                    break;
                // ゴール
                case 11:
                    if ((by == y) && (bx == x))
                    {
                        by = y;
                        y++;
                    }
                    else return false;
                    break;
                case 12:
                if ((by == y) && (bx == x))
                    {
                        by = y;
                        y--;
                    }
                    else return false;
                    break;
                case 13:
                if ((by == y) && (bx == x))
                    {
                        bx = x;
                        x--;
                    }
                    else return false;
                    break;
                case 14:
                if ((by == y) && (bx == x))
                    {
                        bx = x;
                        x++;
                    }
                    else return false;
                    break;
                // 流量計
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

    // パズルをクリアした時にリセット
    public void ResetPazzle()
    {   
        // Add score
        //score += 100;
        playerNowHp+=20;
        if(playerNowHp>100)playerNowHp=100;
       
        

        // Clear Lists
        for (int i = 0; i < mapSizeX; i++)
        {
            // grid
            grid[i].Clear();
            // gridState
            gridState[i].Clear();
            // tilePosList
            tilePosList[i].Clear();
            // panelList
            panelList[i].Clear();
        }

        foreach (var item in cloud)
        {
            Destroy(item);
        }
           
        cloud.Clear();

        
        foreach (var item in unt)
        {
            Destroy(item);
        }
        unt.Clear();
        // grid
        grid.Clear();
        // gridState
        gridState.Clear();
        // tilePosList
        tilePosList.Clear();
        // paneList
        panelList.Clear();
        //correctRoot
        for (int i = 0; i < correctRoot.Count; i++)
        {
            correctRoot[i].Clear();
        }
        
        //flowmeterCheck
        flowmeterCheck.Clear();

        // Clear clearFlag
        clearFlag.Clear();

        // Clear Objects
        foreach (Transform child in puzzleBoard.transform)
        {
            Destroy(child.gameObject);
        }
        clickObject = null;

        // Clear goalPos
        goalPos[(int)COLOR.RED] = new Vector2(-1, -1);
        goalPos[(int)COLOR.GREEN] = new Vector2(-1, -1);
        goalPos[(int)COLOR.BLUE] = new Vector2(-1, -1);

        // Clear Correct Data
        allColorCorrect = false;
        //separateCount = 0;
         StopAllCoroutines();
        foreach (var item in startOChan)
        {
            Destroy(item);
        }
        startOChan[0]=null;
        startOChan[1]=null;
        startOChan[2]=null;
    }

    public void DecTime(float timeTemp)
    {
        Damaged();
        playerNowHp-=timeTemp;
    }

    public void InvPanel(int count)
    {
        Debug.LogError(count<=0);

        Damaged();
        List<(int,int)> pos=new List<(int, int)>();
        
        
        invPanelPrefab.transform.localScale = new Vector3(tileScaleX, tileScaleY, 1);
        
        
        pos =PosRand(count);
        
        foreach (var item in pos)
        {
            cloud.Add(Instantiate(invPanelPrefab, tilePosList[item.Item1][item.Item2], Quaternion.identity));
        }

        StartCoroutine(DestroyTileAfterDelay(cloud, 5));
    }

    public void UntPanel(int count)
    {

        Debug.LogError(count<=0);

        Damaged();
        List<(int,int)> pos=new List<(int, int)>();

        pos =PosRand(count);


        untObj.transform.localScale = new Vector3(tileScaleX, tileScaleY, 1);
        List<BoxCollider2D> bind=new List<BoxCollider2D>();

        foreach (var item in pos)
        {
            var tmpCollider=grid[item.Item1][item.Item2].GetComponent<BoxCollider2D>();
            bind.Add(tmpCollider);
            if(tmpCollider!=null)tmpCollider.enabled=false;
        }

        foreach (var item in pos)
        {
            unt.Add(Instantiate(untObj, tilePosList[item.Item1][item.Item2], Quaternion.identity));
        }
        
        StartCoroutine(EnableTileAfterDelay(bind, 5,unt));
    }

    List<(int,int)> PosRand(int count)
    {
        List<(int,int)> temp=new List<(int, int)>();

        while (temp.Count<count)
        {
            (int,int) tempNum=(0,0);
            tempNum.Item1=UnityEngine.Random.Range(0, mapSizeX);
            tempNum.Item2=UnityEngine.Random.Range(0, mapSizeY);
            
            if (!temp.Contains(tempNum) && gridState[tempNum.Item1][tempNum.Item2].num < 7)
            {
                Debug.Log(gridState[tempNum.Item1][tempNum.Item2].num);
                temp.Add(tempNum);
            }

        }
        return temp;
    }


    IEnumerator DestroyTileAfterDelay(List<GameObject> tile, float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var item in tile)
        {
            if(item!=null)Destroy(item);
        }
        
    }

    IEnumerator EnableTileAfterDelay(List<BoxCollider2D> collider2D, float delay,List<GameObject> tile)
    {
        yield return new WaitForSeconds(delay);
        foreach (var item in collider2D)
        {
            if(item!=null)item.enabled = true;
        }
        foreach (var item in tile)
        {
            if(item!=null)Destroy(item);
        }
        
    }

    public void ActReady(int color,float time)
    {
        //startTime=Time.time;
        countTime=time;
        startTime=countTime;
        leadyTimeBar.color=leadyTimeBarColor[color];
    }

    public void button1()
    {
        buttonNum=0;
        for (int i = 0; i < buttonTempList.Count; i++)
        {
            buttonTempList[i].SetActive(i == buttonNum);
        }
    }

    public void button2()
    {
        buttonNum=1;
        for (int i = 0; i < buttonTempList.Count; i++)
        {
            buttonTempList[i].SetActive(i == buttonNum);
        }
    }

    public void button3()
    {
        buttonNum=2;
        for (int i = 0; i < buttonTempList.Count; i++)
        {
            buttonTempList[i].SetActive(i == buttonNum);
        }
    }

    public int NowButtonNum()
    {
        return buttonNum;
    }

    public void EnemyInit(int hp)
    {
        enemyMaxHp=hp;
        enemyNowHp=enemyMaxHp;
    }

    public void EnemyDamage()
    {
        enemyNowHp-=10;
        if(enemyNowHp<0)enemyNowHp=0;
    }

    public void Grd()
    {
        animator[1].SetTrigger("Grd");
    }

    public void SetPause()
    {
        foreach (var item in animator)
        {
            item.enabled=false;
        }
        gameManager.ChangeGameState(GameManager.GameState.Pause);
    }
    
    void OValPos(List<Vector2> movePosList)
    {
        int temp=movePosList.Count-1;
        Vector3 posTemp=tilePosList[(int)movePosList[temp].x][(int)movePosList[temp].y];
        if((oChan.transform.position-posTemp).magnitude>=0.1f)
        {
           
        }
    }


    public void Damaged()
    {
        foreach (var item in animator)
        {
            item.SetTrigger("Damaged");
        }
    }

    // 正解ルートを保存
    void GetCorrectRoot()
    {

        int y, by, x, bx;
        bool fin = false;
        
        for (int i = 0; i < 3; i++)
        {
            // 色が無い場合スキップする
            if (goalPos[i] == invalid)
            {
                Debug.Log("goalPos[" + i + "] : invalid");
                //correctRoot[i].Add(new Vector2(-1, -1));
                continue;
            }
            // 経路を記憶
            else
            {
                Debug.Log("goalPos[" + i + "] : valid!!");
                y = (int)goalPos[i].y;
                by = y;
                x = (int)goalPos[i].x;
                bx = x;
                fin = false;

                int tmp = 0;

                while (!fin)
                {
                    tmp++;
                    if (tmp >= 100)
                    {
                        Debug.LogError("---------- 無限周回編突入 ----------");
                        Debug.Log("x, y = " + x + ", " + y);
                        Debug.Log("correctRoot[" + i + "] = " + correctRoot[i].Count);

                        return;
                    }

                    //Debug.Log("gridState here = " + gridState[y][x].num);
                    // 現在地をリストに追加
                    correctRoot[i].Add(new Vector2(x, y));
                    // gridState毎の処理
                    switch (gridState[y][x].num)
                    {
                        case 1:
                        case 15:
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
                            break;
                        case 2:
                        case 16:
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
                            break;
                        // スタート
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            fin = true;
                            //Debug.LogError("fin = " + fin);
                            break;
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
                        // その他
                        default:
                            Debug.Log("ルート記憶内容が不適です");
                            fin = true;
                            break;
                    }
                }
            }
        }
    }

    public void MoveOchan(List<List<Vector2>> List)
    {
        var red=List[0];
        var green=List[1];
        var blue=List[2];

        List<Vector3> redPosList=new List<Vector3>();
        List<Vector3> greenPosList=new List<Vector3>();
        List<Vector3> bluePosList=new List<Vector3>();

        foreach (var item in red)
        {
            redPosList.Add(tilePosList[(int)item.y][(int)item.x]);
        }

        foreach (var item in green)
        {
            greenPosList.Add(tilePosList[(int)item.y][(int)item.x]);
        }

        foreach (var item in blue)
        {
            bluePosList.Add(tilePosList[(int)item.y][(int)item.x]);
        }
        redPosList.Reverse();
        greenPosList.Reverse();
        bluePosList.Reverse();
        

        Debug.Log("赤"+redPosList.Count);
        Debug.Log("緑"+greenPosList.Count);
        Debug.Log("青"+bluePosList.Count);

        StartCoroutine(MovePlayer(redPosList,0));
        StartCoroutine(MovePlayer(greenPosList,1));
        StartCoroutine(MovePlayer(bluePosList,2));
        
    
    }

    IEnumerator MovePlayer(List<Vector3> positions,int num)
    {
        float speed = positions.Count*5.0f; // 移動速度

        foreach (var position in positions)
        {
            while (Vector3.Distance(startOChan[num].transform.position, position) > 0.01f)
            {
                startOChan[num].transform.position = Vector3.MoveTowards(startOChan[num].transform.position, position, speed * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(0);
        }
        moveEndFlags[num]=true;
    }

    public bool enemyStop()
    {
        return timeStop;
    }


}
