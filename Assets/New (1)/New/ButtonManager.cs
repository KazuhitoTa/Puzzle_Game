using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using UnityEngine.SceneManagement;

namespace kurukuru
{
    public class ButtonManager : MonoBehaviour
    {
        [SerializeField] GameObject stagePopup;
        [SerializeField] MapData map;
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] List<GameObject> gameObjects =new();

        public static int stageNumber = 0;
        public static int selectNumber = 0;
        private RectTransform clickedButtonRect; // �����ꂽ�{�^���̈ʒu
        private RectTransform myRectTransform;
        private RectTransform[] buttonsRectTransform; // �{�^����RectTransform�̔z��
        private Button[] buttons; // �{�^���̔z��
        private string fileName; // �t�@�C����
        private string filePath; // �t�@�C���p�X

        void Awake()
        {
            fileName = "SaveData.csv";
            filePath = System.IO.Path.Combine(Application.persistentDataPath, fileName);
            // �t�@�C�������݂��Ȃ��ꍇ�A�V�����t�@�C�����쐬
            if (!File.Exists(filePath))
            {
                // �����f�[�^���쐬
                string[] initialData = { "StageData,0", "SelectData,0" };
                File.WriteAllLines(filePath, initialData);
            }
        }
        void Start()
        {   
            stagePopup.SetActive(false);
            int stage;
            if (int.TryParse(ReadSpecificValueFromCSV(0, 1), out stage))
            {
                stage = stage + 1;
            }
            // Empty�I�u�W�F�N�g�̎q���ł���{�^�����擾
            buttons = GetComponentsInChildren<Button>();
            buttonsRectTransform = new RectTransform[buttons.Length];

            // �{�^���̐������J��Ԃ�
            for (int i = 0; i < buttons.Length; i++)
            {
                // �{�^���Ƀe�L�X�g�R���|�[�l���g���A�^�b�`����Ă���ꍇ
                TextMeshProUGUI buttonText = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
                Image[] images = buttons[i].GetComponentsInChildren<Image>(); // Image�̎擾
                buttonsRectTransform[i] = buttons[i].transform as RectTransform;
                if (buttonText != null)
                {
                    int buttonNumber = i + 1;
                    // �{�^���̃e�L�X�g�ɔԍ���U��
                    buttonText.text = (i + 1).ToString();

                    buttons[i].onClick.AddListener(() => OnButtonClick(buttonNumber));

                }
                if (i < stage)
                {
                    buttons[i].interactable = true;
                    foreach (Image image in images)
                    {
                        if (image != null && image.CompareTag("Invisible"))
                        {
                            image.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    buttons[i].onClick.RemoveAllListeners(); // ���ׂẴN���b�N���X�i�[���폜
                }
            }

            myRectTransform = this.gameObject.transform as RectTransform;
            int select;
            if (int.TryParse(ReadSpecificValueFromCSV(1, 1), out select))
            {

                if(select==0)select=1;
                clickedButtonRect = buttonsRectTransform[select-1];
                
                if (myRectTransform != null)
                {
                    myRectTransform.anchoredPosition = new Vector2(myRectTransform.anchoredPosition.x, -clickedButtonRect.anchoredPosition.y - 200);
                    //Debug.Log("�R���e���g" + myRectTransform.position.y);
                    //myRectTransform.position = new Vector3(myRectTransform.position.x, -clickedButtonRect.anchoredPosition.y + 200, myRectTransform.position.z);
                    //Debug.Log(myRectTransform.position.y);
                }
            }
            else
            {
                myRectTransform.anchoredPosition = new Vector2(0,0);
            }
        }

        // �{�^�����N���b�N���ꂽ���̏���
        void OnButtonClick(int buttonNumber)
        {
            clickedButtonRect = buttonsRectTransform[buttonNumber - 1];
            //Debug.Log(clickedButtonRect.anchoredPosition);
            if (clickedButtonRect != null)
            {
                //Debug.Log("�{�^��"+clickedButtonRect.position.y);
            }
            stageNumber = buttonNumber;
            selectNumber = buttonNumber;
            SaveToCSV(1,1,selectNumber);
            Debug.Log(buttonNumber);
            PopUp();
            
        }



        // SaveData.csv�̎w�肳�ꂽ�ʒu��CSV�t�@�C�����̒l���擾����֐�
        // ReadSpecificValueFromCSV(int �c���W, int �����W)�ōs��
        string ReadSpecificValueFromCSV(int roadColPlace, int roadRowPlace)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);   // �s���Ƃɓǂݍ���
                                                                // ��̍��W���z��̒����������ǂ������m�F
                if (roadColPlace < lines.Length)
                {
                    string[] elements = lines[roadColPlace].Split(','); // �s�̍��W�ɑΉ�����s���擾���J���}�ŕ������Ĕz��elements�Ɋi�[
                                                                        // �s�̍��W���z��̒����������ǂ������m�F
                    if (roadRowPlace < elements.Length)
                    {
                        return elements[roadRowPlace];  //string�Œl��Ԃ�
                    }
                    else
                    {
                        Debug.LogError("�w�肳�ꂽ�s���͈͊O�ł�");
                    }
                }
                else
                {
                    Debug.LogError("�w�肳�ꂽ�񂪔͈͊O�ł�");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("CSV�t�@�C���̓ǂݍ��ݒ��ɃG���[���������܂���: " + e.Message);
            }
            return null;    // �Ԃ��l���Ȃ��ꍇ�Anull��Ԃ�
        }  

        void SaveToCSV(int writeColPlace, int writeRowPlace, int writeValue)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);   // 列ごとに読み込み
                if (writeColPlace < lines.Length)               // 列の座標が配列の長さ未満かどうかを確認
                {
                    string[] elements = lines[writeColPlace].Split(',');  // 行の座標に対応する行を取得しカンマで分割して配列elementsに格納
                    if (writeRowPlace < elements.Length)                  // 行の座標が配列の長さ未満かどうかを確認
                    {
                        elements[writeRowPlace] = writeValue.ToString();      // 文字列に変換し、elements内の指定された列の座標の要素を変更
                        lines[writeColPlace] = string.Join(",", elements);    // elementsを再びカンマで連結（しないといけないらしい）
                        File.WriteAllLines(filePath, lines);                // 更新されたlinesを書き込み
                    }
                    else
                    {
                        Debug.LogError("指定された行が範囲外です");
                    }
                }
                else
                {
                    Debug.LogError("指定された列が範囲外です");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("書き込み失敗" + e.Message);
            }
        }

        void PopUp()
        {
            text.text=map.Maps[stageNumber-1].information;
            int index = stageNumber - 1;
            for (int i = 0; i < gameObjects.Count; i++) {
                gameObjects[i].SetActive(i == index);
            }

            stagePopup.SetActive(true);
        } 

        public void back()
        {
            stagePopup.SetActive(false);
        }
        public void GameStart()
        {
            SceneManager.LoadScene("kurukuruGame");
        }
    }
}

