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

    private string SAVE_DATA_DIRECTORY;  // ������ ���� ���

    private Save_Data _saveData;
    private Save_Data _loadData = new Save_Data();
    private int save_num = 0;

    //�г��� ������ ��
    private void OnEnable()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Resources/Save/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY)) // �ش� ��ΰ� �������� �ʴ´ٸ�
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY); // ���� ����(��� ����)

        for (int i = 0; i < _btnTexts.Length; i++)
        {
            string file_num = $"{i + 1}";
            if(File.Exists(SAVE_DATA_DIRECTORY + "/SaveFile" + file_num + ".txt"))
            {
                // ��ü �о����
                string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + "/SaveFile" + file_num + ".txt");
                _loadData = JsonUtility.FromJson<Save_Data>(loadJson);

                _btnTexts[i].alignment = TextAnchor.MiddleLeft;
                _btnTexts[i].text = Show_Data(_loadData);
            }
            else
            {
                _btnTexts[i].alignment = TextAnchor.MiddleCenter;
                _btnTexts[i].text = $"���� ����{i+1}";
            }
        }
    }

    private string Show_Data(Save_Data stat)
    {
        string Data;
        Data = $"���ŷ� {stat.C_san} / {stat.Max_san} \n" +
            $"�ٷ�: {stat.P_str}\t\t���: {stat.P_luk}\t\t����: {stat.P_int}\t\t��ø: {stat.P_dex}";
        return Data;
    }

    public void Press_btn(int btn_num)
    {
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();

        //Ÿ��Ʋ���� �۵�
        if (scene.name == "Title_Scene")
        {
            save_num = btn_num;
            if (File.Exists(SAVE_DATA_DIRECTORY + "/SaveFile" + btn_num + ".txt"))
            {
                //�������� �ε�
                Load_Data();
            }
            else
            {
                //defualt ���� ����
                player_Stat.P_int = 0;
                player_Stat.P_luk = 0;
                player_Stat.P_dex = 0;
                player_Stat.P_str = 0;
                player_Stat.Max_san = 90;
                player_Stat.C_san = 90;
                player_Stat.Coin_num = 0;
                player_Stat.Current_stage_num = 0;
                player_Stat.C_Item_Index = 0;

                SceneManager.LoadScene("Test_Scene");
            }
            
        }
        //�ΰ��ӿ��� �۵�
        else
        {
            save_num = btn_num;
            if (File.Exists(SAVE_DATA_DIRECTORY + "/SaveFile" + btn_num + ".txt"))
            {
                //�����ðڽ��ϱ�? â ����
                _isData_Panel.SetActive(true);
            }
            else
            {
                //�����Ͻðڽ��ϱ�? â ����
                _isSave_Panel.SetActive(true);
            }
            
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

        _saveData.Is_menual = player_Stat.Is_menual;
        _saveData.Is_minimap = player_Stat.Is_minimap;

        for (int i = 0; i < player_Stat.Item_list.Length; i++)
        {
            _saveData.Item_list.Add(player_Stat.Item_list[i]);
        }

        string json = JsonUtility.ToJson(player_Stat); // ���̽�ȭ
        File.WriteAllText(SAVE_DATA_DIRECTORY + "/SaveFile" + save_num + ".txt", json);

        SceneManager.LoadScene("Title_Scene");
    }

    //�������� �ε�
    public void Load_Data()
    {
        // ��ü �о����
        string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + "/SaveFile" + save_num + ".txt");
        _loadData = JsonUtility.FromJson<Save_Data>(loadJson);

        player_Stat.P_int = _loadData.P_int;
        player_Stat.P_luk = _loadData.P_luk;
        player_Stat.P_dex = _loadData.P_dex;
        player_Stat.P_str = _loadData.P_str;
        player_Stat.Max_san = _loadData.Max_san;
        player_Stat.C_san = _loadData.C_san;
        player_Stat.Coin_num = _loadData.Coin_num;
        player_Stat.Current_stage_num = _loadData.Current_stage_num;
        player_Stat.C_Item_Index = _loadData.C_Item_Index;

        player_Stat.Is_menual = _loadData.Is_menual;
        player_Stat.Is_minimap = _loadData.Is_minimap;

        for (int i = 0; i < player_Stat.Item_list.Length; i++)
        {
            player_Stat.Item_list[i] = _loadData.Item_list[i];
        }

        SceneManager.LoadScene("Test_Scene");
    }
}