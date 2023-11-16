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
        public static int stageNumber = 0;
        private static RectTransform clickedButtonRect; // �����ꂽ�{�^���̈ʒu
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
                string initialData = "StageData,0";
                File.WriteAllText(filePath, initialData);
            }
        }
        void Start()
        {
           
            
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
             clickedButtonRect=buttonsRectTransform[0];
            if (myRectTransform != null)
            {
                myRectTransform.position = new Vector3(myRectTransform.position.x, -clickedButtonRect.anchoredPosition.y + 200,myRectTransform.position.z);
                //Debug.Log("�R���e���g" + myRectTransform.position.y);
                //myRectTransform.position = new Vector3(myRectTransform.position.x, -clickedButtonRect.anchoredPosition.y + 200, myRectTransform.position.z);
                //Debug.Log(myRectTransform.position.y);
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
            SceneManager.LoadScene("kurukuruGame");
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
    }
}

