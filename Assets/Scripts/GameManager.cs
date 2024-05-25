using InnerDriveStudios.DiceCreator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum stage
{
    RedRoom,
    Cafe,
    RedSwim,
    Dark_Robby,
    B2,
    Light_Robby,
    Casino,
    Party,
    Store
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

    [Header("COUNT")]
    public int Siren_count;

    [Header("STAGE")]
    public stage Current_stage;
    private stage First_stage;
    private Camera _CurrentCam;//현재 카메라

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        First_stage = stage.RedRoom;

        Current_stage = First_stage;

        _CurrentCam = _mainCam;

        Siren_count = 0;

        //주사위 오브젝트 숨기기
        for (int i = 0; i < _dices.Length; i++)
        {
            _dices[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        Background_change();
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

    public void Dialogue_Start()
    {
        Dialogue_Manage.Instance.GetEventName("Example");
    }

    private void Background_change()
    {
        

        //이 아래는 추후 switch로 재작성
        //RedRoom
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _buttons[0].SetActive(true);
            Dialogue_Manage.Instance.Change_back(4);
        }
        //Cafe
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            _buttons[1].SetActive(true);
            Dialogue_Manage.Instance.Change_back(6);
        }
        //RedSwim
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _buttons[2].SetActive(true);
            Dialogue_Manage.Instance.Change_back(7);
            CSVParsingD.instance.Setcsv(_csvFiles[0]);
            Dialogue_Start();
        }
        //Dark_Robby
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _buttons[3].SetActive(true);
            Dialogue_Manage.Instance.Change_back(8);
        }
        //B2
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _buttons[4].SetActive(true);
            Dialogue_Manage.Instance.Change_back(9);
        }
        //Light_Robby
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _buttons[5].SetActive(true);
            Dialogue_Manage.Instance.Change_back(1);
        }
        //Casino
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            _buttons[6].SetActive(true);
            Dialogue_Manage.Instance.Change_back(12);
        }
        //Party
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            _buttons[7].SetActive(true);
            Dialogue_Manage.Instance.Change_back(13);
        }
        //Store
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            _buttons[8].SetActive(true);
            Dialogue_Manage.Instance.Change_back(14);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Cam_switch(_CurrentCam);
        }
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
            }
        }
    }

    private void Stage_start(int _stage)
    {
        switch (_stage)
        {
            case (int)stage.RedRoom:
                _buttons[0].SetActive(true);                    // 버튼켜기
                Dialogue_Manage.Instance.Change_back(4);        // 배경바꾸기
                CSVParsingD.instance.Setcsv(_csvFiles[0]);      // 처음 시작하는 대사
                Dialogue_Start();                               // 대사시작
                break;

            case (int)stage.Cafe:
                _buttons[1].SetActive(true);                    
                Dialogue_Manage.Instance.Change_back(6);        
                CSVParsingD.instance.Setcsv(_csvFiles[0]);      
                Dialogue_Start();
                break;

            case (int)stage.RedSwim:
                _buttons[2].SetActive(true);
                Dialogue_Manage.Instance.Change_back(7);
                CSVParsingD.instance.Setcsv(_csvFiles[0]);
                Dialogue_Start();
                break;

            case (int)stage.Dark_Robby:
                _buttons[3].SetActive(true);
                Dialogue_Manage.Instance.Change_back(8);
                CSVParsingD.instance.Setcsv(_csvFiles[0]);
                Dialogue_Start();
                break;

            case (int)stage.B2:
                _buttons[4].SetActive(true);
                Dialogue_Manage.Instance.Change_back(9);
                CSVParsingD.instance.Setcsv(_csvFiles[0]);
                Dialogue_Start();
                break;

            case (int)stage.Light_Robby:
                _buttons[5].SetActive(true);
                Dialogue_Manage.Instance.Change_back(1);
                CSVParsingD.instance.Setcsv(_csvFiles[0]);
                Dialogue_Start();
                break;

            case (int)stage.Casino:
                _buttons[6].SetActive(true);
                Dialogue_Manage.Instance.Change_back(12);
                CSVParsingD.instance.Setcsv(_csvFiles[0]);
                Dialogue_Start();
                break;

            case (int)stage.Party:
                _buttons[7].SetActive(true);
                Dialogue_Manage.Instance.Change_back(13);
                CSVParsingD.instance.Setcsv(_csvFiles[0]);
                Dialogue_Start();
                break;

            case (int)stage.Store:
                _buttons[8].SetActive(true);
                Dialogue_Manage.Instance.Change_back(14);
                CSVParsingD.instance.Setcsv(_csvFiles[0]);
                Dialogue_Start();
                break;
        }
    }

    // deadend 리셋 (SoftReset버튼으로 실행)
    public void SoftReset()
    {
        Siren_count = 0;
        Button_reset();
        Stage_start((int)Current_stage);
    }

    // 처음부터 (HardReset버튼으로 실행)
    public void HardReset()
    {
        Siren_count = 0;
        Button_reset();
        Stage_start((int)First_stage);
    }

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
        Siren_count++;
    }
}
