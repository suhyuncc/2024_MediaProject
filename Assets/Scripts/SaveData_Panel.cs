using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveData_Panel : MonoBehaviour
{
    [SerializeField]
    private Player_stat player_Stat;
    [SerializeField]
    private Text[] _btnTexts;
    [SerializeField]
    private GameObject _isData_Panel;
    [SerializeField]
    private GameObject _isSave_Panel;

    private string SAVE_DATA_DIRECTORY;  // 저장할 폴더 경로

    private Save_Data _saveData;
    private Save_Data _loadData = new Save_Data();
    private int save_num = 0;

    //패널이 켜졌을 때
    private void OnEnable()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Resources/Save/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY)) // 해당 경로가 존재하지 않는다면
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY); // 폴더 생성(경로 생성)

        for (int i = 0; i < _btnTexts.Length; i++)
        {
            string file_num = $"{i + 1}";
            if(File.Exists(SAVE_DATA_DIRECTORY + "/SaveFile" + file_num + ".txt"))
            {
                // 전체 읽어오기
                string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + "/SaveFile" + file_num + ".txt");
                _loadData = JsonUtility.FromJson<Save_Data>(loadJson);

                _btnTexts[i].alignment = TextAnchor.MiddleLeft;
                _btnTexts[i].text = Show_Data(_loadData);
            }
            else
            {
                _btnTexts[i].alignment = TextAnchor.MiddleCenter;
                _btnTexts[i].text = $"저장 슬롯{i+1}";
            }
        }
    }

    private string Show_Data(Save_Data stat)
    {
        string Data;
        Data = $"정신력 {stat.C_san} / {stat.Max_san} \n" +
            $"근력: {stat.P_str}\t\t행운: {stat.P_luk}\t\t지능: {stat.P_int}\t\t민첩: {stat.P_dex}";
        return Data;
    }

    public void Press_btn(int btn_num)
    {
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();

        //타이틀에서 작동
        if (scene.name == "Title_Scene")
        {
            if (File.Exists(SAVE_DATA_DIRECTORY + "/SaveFile" + btn_num + ".txt"))
            {
                //기존파일 로드
            }
            else
            {
                //defualt 파일 생성
            }
            SceneManager.LoadScene("Test_Scene");
        }
        //인게임에서 작동
        else
        {
            if (File.Exists(SAVE_DATA_DIRECTORY + "/SaveFile" + btn_num + ".txt"))
            {
                //덮으시겠습니까? 창 오픈
                _isData_Panel.SetActive(true);
            }
            else
            {
                //저장하시겠습니까? 창 오픈
                _isSave_Panel.SetActive(true);
            }
            save_num = btn_num;
        }
    }

    public void Save_Data()
    {
        _saveData = new Save_Data();

        _saveData.P_int = player_Stat.P_int;
        _saveData.P_luk = player_Stat.P_luk;
        _saveData.P_dex = player_Stat.P_dex;
        _saveData.P_str = player_Stat.P_str;
        _saveData.Max_san = player_Stat.Max_san;
        _saveData.C_san = player_Stat.C_san;
        _saveData.Coin_num = player_Stat.Coin_num;
        _saveData.Current_stage_num = player_Stat.Current_stage_num;
        _saveData.C_Item_Index = player_Stat.C_Item_Index;

        for(int i = 0; i < player_Stat.Item_list.Length; i++)
        {
            _saveData.Item_list.Add(player_Stat.Item_list[i]);
        }

        string json = JsonUtility.ToJson(player_Stat); // 제이슨화
        File.WriteAllText(SAVE_DATA_DIRECTORY + "/SaveFile" + save_num + ".txt", json);
    }
}
