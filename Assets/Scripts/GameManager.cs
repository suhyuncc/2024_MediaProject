using InnerDriveStudios.DiceCreator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum stage
{
    Prolouge,
    RedRoom,
    Endless_Hallway,
    Cafe,
    RedSwim,
    Swim_to_Cafe,
    To_lobby,
    Dark_Robby,
    B2,
    Light_Robby,
    Party_before,
    Sweet_room,
    Casino,
    Party,
    Store,
    Ending
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Core Objects")]
    [SerializeField]
    private Player_stat P_stat;
    [SerializeField]
    private GameObject[] _dices; //주사위들
    [SerializeField]
    private TextAsset[] _csvFiles;

    [Header("UI")]
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private Camera _mainCam;
    [SerializeField]
    private Camera _diceCam;
    [SerializeField]
    private GameObject _dicePanel;
    [SerializeField]
    private GameObject[] _buttons;
    private GameObject _current_buttons;

    [Header("Siren")]
    public int Siren_count;
    public bool Siren_On;
    [SerializeField]
    private TextAsset[] _sirenCSV; //수영장 세이렌 이벤트

    [Header("Owner")]
    public int Owner_count;
    [SerializeField]
    private TextAsset[] _ownerCSV; //연회장 축사 이벤트

    [Header("STAGE")]
    public stage Current_stage;
    private stage First_stage;
    private Camera _CurrentCam;//현재 카메라

    private int _currentItemIndex; // 현재 화면에서 얻은 아이템의 수

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        First_stage = stage.RedRoom;

        _CurrentCam = _mainCam;

        Siren_count = 0;

        //주사위 오브젝트 숨기기
        for (int i = 0; i < _dices.Length; i++)
        {
            _dices[i].SetActive(false);
        }

        // 스탯 조정
        _dicePanel.GetComponent<Dice_panel>().Player_set = true;
        Dice_On("2d6");

        // 스테이지 시작을 강제(실제 빌드때 사용 X)
        //Stage_start(9);
    }

    public void Dice_On(string dice_name)
    {
        Cam_switch(_CurrentCam);
        Dice_setting(dice_name);
    }

    public void Dice_Off()
    {
        Cam_switch(_CurrentCam);
    }

    private void Cam_switch(Camera cam)
    {

        if (cam.Equals(_mainCam))
        {
            _mainCam.gameObject.SetActive(false);
            _diceCam.gameObject.SetActive(true);
            _dicePanel.SetActive(true);
            _CurrentCam = _diceCam;
            _canvas.worldCamera = _CurrentCam;
        }
        else if (cam.Equals(_diceCam))
        {
            _mainCam.gameObject.SetActive(true);
            _diceCam.gameObject.SetActive(false);
            _dicePanel.SetActive(false);
            _CurrentCam = _mainCam;
            _canvas.worldCamera = _CurrentCam;
        }
        
    }

    //다른 버튼에서 다이얼로그 실행시키기 위한 함수
    public void Dialogue_Start()
    {
        Dialogue_Manage.Instance.GetEventName("Example");
    }

    //켜저있는 버튼 끄기
    private void Button_off()
    {
        for(int i = 0; i < _buttons.Length; i++)
        {
            if (_buttons[i].activeSelf)
            {
                _buttons[i].SetActive(false);
            }
        }
    }

    //켜저있는 주사위 끄기
    private void Dice_off()
    {
        for (int i = 0; i < _dices.Length; i++)
        {
            if (_dices[i].activeSelf)
            {
                _dices[i].SetActive(false);
            }
        }
    }

    //모든 스테이지의 버튼 count초기화
    private void Button_reset()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            for(int j = 0; j < _buttons[i].transform.childCount; j++)
            {
                _buttons[i].transform.GetChild(j).GetComponent<Event_button>().CountReset();
                _buttons[i].transform.GetChild(j).gameObject.SetActive(true);
            }
        }
    }

    public void Stage_Btn_On()
    {
        for(int i = 0; i < _current_buttons.transform.childCount; i++)
        {
            _current_buttons.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    //세이렌 이벤트
    public void Siren_event()
    {
        if(Siren_count == 3)
        {
            bool is_flug = false;
            //귀마개 판단
            for (int i = 0; i < P_stat.Item_list.Length; i++)
            {
                if (P_stat.Item_list[i] == 6)
                {
                    is_flug = true;
                }
            }

            if(is_flug)
            {
                CSVParsingD.instance.Setcsv(_sirenCSV[Siren_count]);
            }
            else
            {
                CSVParsingD.instance.Setcsv(_sirenCSV[Siren_count - 1]);
            }
        }
        else
        {
            CSVParsingD.instance.Setcsv(_sirenCSV[Siren_count - 1]);
        }
        
        Dialogue_Start();
    }

    public void Owner_Event()
    {
        bool is_coin = false;
        //동전 판단
        for (int i = 0; i < P_stat.Item_list.Length; i++)
        {
            if (P_stat.Item_list[i] == 12)
            {
                is_coin = true;
            }
        }
        //추후 스위트룸 판단 추가 예정
        if (is_coin)
        {
            CSVParsingD.instance.Setcsv(_ownerCSV[0]);
        }
        else
        {
            CSVParsingD.instance.Setcsv(_ownerCSV[1]);
        }

        Dialogue_Start();
    }

    //스테이지 시작시 작동하는 핵심 함수
    public void Stage_start(int _stage)
    {
        _currentItemIndex = 0;
        switch (_stage)
        {
            //0
            case (int)stage.Prolouge:
                Current_stage = stage.Prolouge;
                Button_off();
                Dialogue_Manage.Instance.Change_back(0);        // 배경바꾸기
                CSVParsingD.instance.Setcsv(_csvFiles[0]);      // 처음 시작하는 대사
                Dialogue_Start();                               // 대사시작
                break;
            //1
            case (int)stage.RedRoom:
                Current_stage = stage.RedRoom;
                _current_buttons = _buttons[0];
                Button_off();
                _current_buttons.SetActive(true);                    // 버튼켜기
                Dialogue_Manage.Instance.Change_back(4);        // 배경바꾸기
                CSVParsingD.instance.Setcsv(_csvFiles[1]);      // 처음 시작하는 대사
                Dialogue_Start();                               // 대사시작
                break;
            //2
            case (int)stage.Endless_Hallway:
                Current_stage = stage.Endless_Hallway;
                Button_off();
                Dialogue_Manage.Instance.Change_back(5);        // 배경바꾸기
                CSVParsingD.instance.Setcsv(_csvFiles[2]);      // 처음 시작하는 대사
                Dialogue_Start();                               // 대사시작
                break;
            //3
            case (int)stage.Cafe:
                Current_stage = stage.Cafe;
                _current_buttons = _buttons[1];
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(6);        
                CSVParsingD.instance.Setcsv(_csvFiles[3]);      
                Dialogue_Start();
                break;
            //4
            case (int)stage.RedSwim:
                Current_stage = stage.RedSwim;
                _current_buttons = _buttons[2];
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(0);
                CSVParsingD.instance.Setcsv(_csvFiles[4]);
                Dialogue_Start();
                break;
            //5
            case (int)stage.Swim_to_Cafe:
                Current_stage = stage.Swim_to_Cafe;
                Dialogue_Manage.Instance.Change_back(6);
                CSVParsingD.instance.Setcsv(_csvFiles[5]);
                Dialogue_Start();
                break;

            //6
            case (int)stage.To_lobby:
                Current_stage = stage.To_lobby;
                Dialogue_Manage.Instance.Change_back(5);
                CSVParsingD.instance.Setcsv(_csvFiles[6]);
                Dialogue_Start();
                break;

            //7
            case (int)stage.Dark_Robby:
                Current_stage = stage.Dark_Robby;
                _current_buttons = _buttons[3];
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(8);
                CSVParsingD.instance.Setcsv(_csvFiles[7]);
                Dialogue_Start();
                break;

            //8
            case (int)stage.B2:
                Current_stage = stage.B2;
                _current_buttons = _buttons[4];
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(9);
                CSVParsingD.instance.Setcsv(_csvFiles[8]);
                Dialogue_Start();
                break;

            //9
            case (int)stage.Light_Robby:
                Current_stage = stage.Light_Robby;
                _current_buttons = _buttons[5];
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(1);
                Dialogue_Manage.Instance.Dialouge_off(); //여긴 시작 대사가 없기 때문에 강제로 패널을 닫음
                break;

            //10
            case (int)stage.Party_before:
                Current_stage = stage.Party_before;
                Button_off();
                Dialogue_Manage.Instance.Change_back(15);
                CSVParsingD.instance.Setcsv(_csvFiles[9]);
                Dialogue_Start();
                break;
            
            //11
            case (int)stage.Sweet_room:
                Current_stage = stage.Sweet_room;
                _current_buttons = _buttons[6];
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(10);
                CSVParsingD.instance.Setcsv(_csvFiles[10]);
                Dialogue_Start();
                break;

            //12
            case (int)stage.Casino:
                P_stat.Coin_num = 15; //카지노 칩 획득 (15개)
                Current_stage = stage.Casino;
                _current_buttons = _buttons[6];
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(12);
                CSVParsingD.instance.Setcsv(_csvFiles[11]);
                Dialogue_Start();
                break;

            //13
            case (int)stage.Party:
                Current_stage = stage.Party;
                _current_buttons = _buttons[7];
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(15);
                CSVParsingD.instance.Setcsv(_csvFiles[12]);
                Dialogue_Start();
                break;

            //14
            case (int)stage.Store:
                Current_stage = stage.Store;
                _buttons[8].SetActive(true);
                Dialogue_Manage.Instance.Change_back(14);
                CSVParsingD.instance.Setcsv(_csvFiles[0]);
                Dialogue_Start();
                break;

            //15
            case (int)stage.Ending:
                Current_stage = stage.Ending;
                Button_off();
                Dialogue_Manage.Instance.Change_back(1);
                CSVParsingD.instance.Setcsv(_csvFiles[14]);
                Dialogue_Start();
                break;
        }
    }

    // deadend 리셋 (SoftReset버튼으로 실행)
    public void SoftReset()
    {
        Siren_count = 0;
        Owner_count = 0;
        Button_reset();
        Remove_Item(_currentItemIndex);
        Stage_start((int)Current_stage);
    }

    // 처음부터 (HardReset버튼으로 실행)
    public void HardReset()
    {
        Siren_count = 0;
        Owner_count = 0;
        Button_reset();
        Remove_Item(P_stat.C_Item_Index);
        P_stat.C_san = P_stat.Max_san;
        Stage_start((int)First_stage);
    }

    // 엔딩
    public void ending()
    {
        Siren_count = 0;
        Owner_count = 0;
        Button_reset();
        SceneManager.LoadScene("Title_Scene");
    }

    //주사위 세팅
    public void Dice_setting(string dice)
    {
        Dice_off();

        switch (dice)
        {
            case "1d4":
                _dices[0].SetActive(true);
                break;

            case "1d6":
                _dices[1].SetActive(true);
                break;

            case "2d6":
                _dices[1].SetActive(true);
                _dices[2].SetActive(true);
                break;
            case "3d6":
                _dices[1].SetActive(true);
                _dices[2].SetActive(true);
                _dices[3].SetActive(true);
                break;
            case "1d100":
                _dices[4].SetActive(true);
                _dices[5].SetActive(true);
                break;
        }
    }

    // 세이렌 카운트 증가(수영장 버튼들이 실행)
    public void Increase_siren()
    {
        //3이상이면 진행하지 않음
        if (Siren_count < 3)
        {
            Siren_On = true;
        }
        Siren_count++; 
    }

    // 호텔주인 카운트 증가(연회장 버튼들이 실행)
    public void Increase_owner()
    {
        Owner_count++;
    }

    // 아이템 추가
    public void Add_Item(int item)
    {
        P_stat.Item_list[P_stat.C_Item_Index] = item;
        P_stat.C_Item_Index++;
        _currentItemIndex++;
    }

    //아이템 리셋
    public void Remove_Item(int item)
    {
        for(int i = 0; i < item; i++)
        {
            P_stat.C_Item_Index--;
            P_stat.Item_list[P_stat.C_Item_Index] = 0;
        }
    }
}
