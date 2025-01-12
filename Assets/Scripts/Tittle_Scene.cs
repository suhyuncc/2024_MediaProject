using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Scene : MonoBehaviour
{
    [SerializeField]
    private Player_stat player_Stat;
    [SerializeField]
    private GameObject _savedataPanel;

    private string SAVE_DATA_DIRECTORY;  // 저장할 폴더 경로

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBtn()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Resources/Save/";

        // 해당 경로가 존재하지 않는다면
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY); // 폴더 생성(경로 생성)
            //defualt 파일 생성
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
        else
        {
            _savedataPanel.SetActive(true);
        }
    }

    public void FinBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
