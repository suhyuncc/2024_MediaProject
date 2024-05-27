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
    private GameObject[] _dices; //�ֻ�����
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
    private TextAsset[] _sirenCSV; //������ ���̷� �̺�Ʈ

    [Header("Owner")]
    public int Owner_count;
    [SerializeField]
    private TextAsset[] _ownerCSV; //��ȸ�� ��� �̺�Ʈ

    [Header("STAGE")]
    public stage Current_stage;
    private stage First_stage;
    private Camera _CurrentCam;//���� ī�޶�

    private int _currentItemIndex; // ���� ȭ�鿡�� ���� �������� ��

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        First_stage = stage.RedRoom;

        _CurrentCam = _mainCam;

        Siren_count = 0;

        //�ֻ��� ������Ʈ �����
        for (int i = 0; i < _dices.Length; i++)
        {
            _dices[i].SetActive(false);
        }

        // ���� ����
        _dicePanel.GetComponent<Dice_panel>().Player_set = true;
        Dice_On("2d6");

        // �������� ������ ����(���� ���嶧 ��� X)
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

    //�ٸ� ��ư���� ���̾�α� �����Ű�� ���� �Լ�
    public void Dialogue_Start()
    {
        Dialogue_Manage.Instance.GetEventName("Example");
    }

    //�����ִ� ��ư ����
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

    //�����ִ� �ֻ��� ����
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

    //��� ���������� ��ư count�ʱ�ȭ
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

    //���̷� �̺�Ʈ
    public void Siren_event()
    {
        if(Siren_count == 3)
        {
            bool is_flug = false;
            //�͸��� �Ǵ�
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
        //���� �Ǵ�
        for (int i = 0; i < P_stat.Item_list.Length; i++)
        {
            if (P_stat.Item_list[i] == 12)
            {
                is_coin = true;
            }
        }
        //���� ����Ʈ�� �Ǵ� �߰� ����
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

    //�������� ���۽� �۵��ϴ� �ٽ� �Լ�
    public void Stage_start(int _stage)
    {
        _currentItemIndex = 0;
        switch (_stage)
        {
            //0
            case (int)stage.Prolouge:
                Current_stage = stage.Prolouge;
                Button_off();
                Dialogue_Manage.Instance.Change_back(0);        // ���ٲٱ�
                CSVParsingD.instance.Setcsv(_csvFiles[0]);      // ó�� �����ϴ� ���
                Dialogue_Start();                               // ������
                break;
            //1
            case (int)stage.RedRoom:
                Current_stage = stage.RedRoom;
                _current_buttons = _buttons[0];
                Button_off();
                _current_buttons.SetActive(true);                    // ��ư�ѱ�
                Dialogue_Manage.Instance.Change_back(4);        // ���ٲٱ�
                CSVParsingD.instance.Setcsv(_csvFiles[1]);      // ó�� �����ϴ� ���
                Dialogue_Start();                               // ������
                break;
            //2
            case (int)stage.Endless_Hallway:
                Current_stage = stage.Endless_Hallway;
                Button_off();
                Dialogue_Manage.Instance.Change_back(5);        // ���ٲٱ�
                CSVParsingD.instance.Setcsv(_csvFiles[2]);      // ó�� �����ϴ� ���
                Dialogue_Start();                               // ������
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
                Dialogue_Manage.Instance.Dialouge_off(); //���� ���� ��簡 ���� ������ ������ �г��� ����
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
                P_stat.Coin_num = 15; //ī���� Ĩ ȹ�� (15��)
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

    // deadend ���� (SoftReset��ư���� ����)
    public void SoftReset()
    {
        Siren_count = 0;
        Owner_count = 0;
        Button_reset();
        Remove_Item(_currentItemIndex);
        Stage_start((int)Current_stage);
    }

    // ó������ (HardReset��ư���� ����)
    public void HardReset()
    {
        Siren_count = 0;
        Owner_count = 0;
        Button_reset();
        Remove_Item(P_stat.C_Item_Index);
        P_stat.C_san = P_stat.Max_san;
        Stage_start((int)First_stage);
    }

    // ����
    public void ending()
    {
        Siren_count = 0;
        Owner_count = 0;
        Button_reset();
        SceneManager.LoadScene("Title_Scene");
    }

    //�ֻ��� ����
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

    // ���̷� ī��Ʈ ����(������ ��ư���� ����)
    public void Increase_siren()
    {
        //3�̻��̸� �������� ����
        if (Siren_count < 3)
        {
            Siren_On = true;
        }
        Siren_count++; 
    }

    // ȣ������ ī��Ʈ ����(��ȸ�� ��ư���� ����)
    public void Increase_owner()
    {
        Owner_count++;
    }

    // ������ �߰�
    public void Add_Item(int item)
    {
        P_stat.Item_list[P_stat.C_Item_Index] = item;
        P_stat.C_Item_Index++;
        _currentItemIndex++;
    }

    //������ ����
    public void Remove_Item(int item)
    {
        for(int i = 0; i < item; i++)
        {
            P_stat.C_Item_Index--;
            P_stat.Item_list[P_stat.C_Item_Index] = 0;
        }
    }
}
