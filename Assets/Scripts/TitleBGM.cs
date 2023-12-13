using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class TitleBGM : MonoBehaviour
{
    [SerializeField] AudioSource BGMAudioSource;
    [SerializeField] AudioSource SEAudioSource;
    [SerializeField] List<AudioClip> BGMAudioClips = new List<AudioClip>();


    public static float BGMVolume;
    public static float SEVolume;

    private bool hasPlayedFirstClip = false;

    private static bool startCheck=false;
    private string fileName;
    private string filePath;

    private void Start()
    {
        fileName="SaveData.csv";
        filePath = System.IO.Path.Combine(Application.persistentDataPath, fileName);
        BGMVolume=float.Parse(ReadSpecificValueFromCSV(2, 1));
        SEVolume=float.Parse(ReadSpecificValueFromCSV(3, 1));
        
        BGMAudioSource.volume=BGMVolume;
        SEAudioSource.volume=SEVolume;
        if (BGMAudioClips.Count >= 2 && BGMAudioSource != null)
        {
            // 最初のクリップを再生
            PlayFirstClip();
        }
        else
        {
            Debug.LogError("Audio clips or AudioSource not properly set!");
        }
    }

    private void Update()
    {
        // 最初のクリップが再生された後、それ以降は2番目のクリップをループ再生する
        if (!BGMAudioSource.isPlaying&&hasPlayedFirstClip)
        {
            BGMAudioSource.clip = BGMAudioClips[1]; // 2番目のクリップを設定
            BGMAudioSource.loop = true; // ループ再生を有効にする
            BGMAudioSource.Play(); // 2番目のクリップを再生
        }
    }

    private void PlayFirstClip()
    {
        BGMAudioSource.clip = BGMAudioClips[0]; // 1番目のクリップを設定
        BGMAudioSource.Play(); // 1番目のクリップを再生
        hasPlayedFirstClip=true;
    }
    public void SEPlay()
    {
        SEAudioSource.Play();
    }

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
}
