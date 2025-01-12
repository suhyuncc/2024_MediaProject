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
    Ending,
    Cook,
    EV6F,
    EV4F,
    EV2F,
    EV4F_E,
    EV4F_O,
    EV_D5F_E,
    EV_D5F_O,
    EV_D1F_E,
    EV_D1F_O,
    EV1F_E,
    EV1F_O,
    Party_to_Cook,
    Escape_Cook,
    Bad,
    Real,
    Hidden
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Start Stage Number")]
    public int _startNum;

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
    private GameObject _menual;
    [SerializeField]
    private GameObject _miniMap;
    [SerializeField]
    private GameObject[] _buttons;
    private GameObject _current_buttons;
    [SerializeField]
    private Text _location;

    [Header("EV")]
    [SerializeField]
    private GameObject[] _evBtns;
    public int EV_index;

    [Header("Siren")]
    public int Siren_count;
    public bool Siren_On;
    [SerializeField]
    private TextAsset[] _sirenCSV; //������ ���̷� �̺�Ʈ

    [Header("Owner")]
    public int Owner_count;
    [SerializeField]
    private TextAsset[] _ownerCSV; //��ȸ�� ��� �̺�Ʈ

    [Header("Chef")]
    public int Chef_count;
    public int Chef_success_count;
    [SerializeField]
    private TextAsset[] _chefCSV; //���� �̺�Ʈ

    [Header("Manager")]
    public bool Manager_On;
    public bool Manager_Start;
    public int Manager_count;
    [SerializeField]
    private TextAsset[] _managerCSV; //�Ŵ��� �̺�Ʈ

    [Header("STAGE")]
    public stage Current_stage;
    private stage First_stage;

    [Header("BGM")]
    [SerializeField]
    private AudioSource _bgmPlayer;
    [SerializeField]
    private AudioClip[] _bgmClips;

    [Header("SFX")]
    [SerializeField]
    private AudioSource _sfxPlayer;
    [SerializeField]
    private AudioClip[] _sfxClips;

    private Camera _CurrentCam;//���� ī�޶�

    private int _currentItemIndex; // ���� ȭ�鿡�� ���� �������� ��

    [Header("Toggles")]
    public bool after_Party = false; // ��ȸ�� �湮 ����
    public bool is_ticket = false; // Ƽ�� ����
    public bool casino_first = true; // ī���� ù�湮����
    public bool party_first = true; // ��ȸ�� ù�湮����
    public bool store_first = true; // ����� ù�湮����
    public bool store_after_first = true; // ����� ù�湮����
    public bool after_owner = false; // ����̺�Ʈ ��������
    public bool after_sweet = false; // ����Ʈ�� ��������

    private void Awake()
    {
        //Debug.Log("Awake_test");
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        First_stage = stage.RedRoom;

        _CurrentCam = _mainCam;

        Siren_count = 0;

        if(P_stat.Is_menual)
        {
            _menual.SetActive(true);
        }
        else
        {
            _menual.SetActive(false);
        }

        if (P_stat.Is_minimap)
        {
            _miniMap.SetActive(true);
        }
        else
        {
            _miniMap.SetActive(false);
        }

        //�ֻ��� ������Ʈ �����
        for (int i = 0; i < _dices.Length; i++)
        {
            _dices[i].SetActive(false);
        }

        //�÷��̾� ���� ��ũ���ͺ� ������Ʈ���� �������� ��ȣ ��������
        _startNum = P_stat.Current_stage_num;

        if(_startNum == 0)
        {
            after_Party = false;
            is_ticket = false;
            casino_first = true;
            party_first = true;
            store_first = true;
            store_after_first = true;
            after_owner = false;
            after_sweet = false;

            Reset_Item();
        }

        // �������� ������ ����
        Stage_start(_startNum);
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

    //�����ִ� EV��ư ����
    private void EV_Button_off()
    {
        for (int i = 0; i < _evBtns.Length; i++)
        {
            if (_evBtns[i].activeSelf)
            {
                _evBtns[i].SetActive(false);
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
                if(_buttons[i].transform.GetChild(j).GetComponent<Event_button>() != null)
                {
                    _buttons[i].transform.GetChild(j).GetComponent<Event_button>().CountReset();
                }
                
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

    //��� �̺�Ʈ
    public void Owner_Event()
    {
        Set_BGM(13);
        Owner_count = 0;
        bool is_coin = false;
        //���� ���� �Ǵ�
        for (int i = 0; i < P_stat.Item_list.Length; i++)
        {
            if (P_stat.Item_list[i] == 12)
            {
                is_coin = true;
            }
        }

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

    //�ڴ��� ��� �̺�Ʈ
    public void Late_Owner_Event()
    {
        Set_BGM(13);
        Owner_count = 0;
        bool is_coin = false;
        //���� ���� �Ǵ�
        for (int i = 0; i < P_stat.Item_list.Length; i++)
        {
            if (P_stat.Item_list[i] == 12)
            {
                is_coin = true;
            }
        }

        if (is_coin)
        {
            CSVParsingD.instance.Setcsv(_ownerCSV[2]);
        }
        else
        {
            CSVParsingD.instance.Setcsv(_ownerCSV[3]);
        }

        Dialogue_Start();
    }

    //���� �̺�Ʈ
    public void Chef_Event()
    {
        Chef_count = 0;

        if (Chef_success_count >= 3)
        {
            CSVParsingD.instance.Setcsv(_chefCSV[0]);
        }
        else
        {
            CSVParsingD.instance.Setcsv(_chefCSV[1]);
        }

        Chef_success_count = 0;
        Dialogue_Start();
    }

    //�Ŵ��� �̺�Ʈ
    public void Manager_Event()
    {
        if (Manager_count == 2)
        {
            CSVParsingD.instance.Setcsv(_managerCSV[Manager_count - 1]);
            Manager_count = 0;
            Manager_On = false;
            Dialogue_Start();
        }
        else if (Manager_count == 1)
        {
            CSVParsingD.instance.Setcsv(_managerCSV[Manager_count - 1]);
            Dialogue_Start();
        }
    }


    //�������� ���۽� �۵��ϴ� �ٽ� �Լ�
    public void Stage_start(int _stage)
    {
        _currentItemIndex = 0;

        if (_stage == 0)
        {
            // ���� ����
            _dicePanel.GetComponent<Dice_panel>().Player_set = true;
        }
        else
        {
            // ���� ����
            _dicePanel.GetComponent<Dice_panel>().Player_set = false;
        }

        switch (_stage)
        {
            case -5:    //����� �̺�Ʈ ���н�
                Set_BGM(6);
                Time.timeScale = 1;
                Current_stage = stage.B2;
                _location.text = "B2F �����";
                Button_off();
                Dialogue_Manage.Instance.Change_back(9);
                CSVParsingD.instance.Setcsv(_csvFiles[18]);
                Dialogue_Start();
                break;

            case -4:    //����� �̺�Ʈ ������
                Set_BGM(6);
                Time.timeScale = 1;
                Current_stage = stage.B2;
                _location.text = "B2F �����";
                Button_off();
                Dialogue_Manage.Instance.Change_back(9);
                CSVParsingD.instance.Setcsv(_csvFiles[17]);
                Dialogue_Start();
                break;

            case -3:
                //Debug.Log("����X");
                StopAllCoroutines();
                SceneManager.LoadScene("MiniGameScene");
                break;

            case -2:
                //Debug.Log("������");
                StopAllCoroutines();
                SceneManager.LoadScene("MiniGameScene");
                break;

            case -1:
                //Debug.Log("������");
                StopAllCoroutines();
                SceneManager.LoadScene("MiniGameScene");
                break;
            //0
            case (int)stage.Prolouge:
                Set_BGM(0);
                Current_stage = stage.Prolouge;
                Button_off();
                _location.text = ".....";
                Dialogue_Manage.Instance.Change_back(0);        // ���ٲٱ�
                CSVParsingD.instance.Setcsv(_csvFiles[0]);      // ó�� �����ϴ� ���
                Dialogue_Start();                               // ������
                break;
            //1
            case (int)stage.RedRoom:
                Set_BGM(2);
                Set_SFX(1);
                Current_stage = stage.RedRoom;
                _current_buttons = _buttons[0];
                _location.text = "6F ����";
                Button_off();
                _current_buttons.SetActive(true);                    // ��ư�ѱ�
                Dialogue_Manage.Instance.Change_back(4);        // ���ٲٱ�
                CSVParsingD.instance.Setcsv(_csvFiles[1]);      // ó�� �����ϴ� ���
                Dialogue_Start();                               // ������
                break;
            //2
            case (int)stage.Endless_Hallway:
                Current_stage = stage.Endless_Hallway;
                _current_buttons = _buttons[9];
                _location.text = "6F ����";
                Button_off();
                _current_buttons.SetActive(true);                    // ��ư�ѱ�
                Dialogue_Manage.Instance.Change_back(5);        // ���ٲٱ�
                CSVParsingD.instance.Setcsv(_csvFiles[2]);      // ó�� �����ϴ� ���
                Dialogue_Start();                               // ������
                break;
            //3
            case (int)stage.Cafe:
                Set_BGM(3);
                Current_stage = stage.Cafe;
                _current_buttons = _buttons[1];
                _location.text = "4F ī��";
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(6);        
                CSVParsingD.instance.Setcsv(_csvFiles[3]);      
                Dialogue_Start();
                break;
            //4
            case (int)stage.RedSwim:
                Set_BGM(4);
                Current_stage = stage.RedSwim;
                _current_buttons = _buttons[2];
                _location.text = "2F ������";
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(7);
                CSVParsingD.instance.Setcsv(_csvFiles[4]);
                Dialogue_Start();
                break;
            //5
            case (int)stage.Swim_to_Cafe:
                Set_BGM(3);
                Current_stage = stage.Swim_to_Cafe;
                _current_buttons = _buttons[11];
                _location.text = "4F ī��";
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(6);
                CSVParsingD.instance.Setcsv(_csvFiles[5]);
                Dialogue_Start();
                break;

            //6
            case (int)stage.To_lobby:
                Set_BGM(5);
                Current_stage = stage.To_lobby;
                _location.text = "5F ī����";
                _current_buttons = _buttons[12];
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(15);
                CSVParsingD.instance.Setcsv(_csvFiles[6]);
                Dialogue_Start();
                break;

            //7
            case (int)stage.Dark_Robby:
                Set_BGM(5);
                Current_stage = stage.Dark_Robby;
                _current_buttons = _buttons[3];
                _location.text = "1F �κ�";
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(8);
                CSVParsingD.instance.Setcsv(_csvFiles[7]);
                Dialogue_Start();
                break;

            //8
            case (int)stage.B2:
                Set_BGM(6);
                Current_stage = stage.B2;
                _current_buttons = _buttons[4];
                _location.text = "B2F �����";
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(9);
                CSVParsingD.instance.Setcsv(_csvFiles[8]);
                Dialogue_Start();
                break;

            //9
            case (int)stage.Light_Robby:
                Set_BGM(1);
                Current_stage = stage.Light_Robby;
                _current_buttons = _buttons[5];
                _location.text = "1F �κ�";
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(1);
                Dialogue_Manage.Instance.Dialouge_off(); //���� ���� ��簡 ���� ������ ������ �г��� ����
                break;

            //10
            case (int)stage.Party_before:
                Set_BGM(8);
                Current_stage = stage.Party_before;
                after_Party = true;
                _current_buttons = _buttons[13];
                _location.text = "10F ��ȸ��";
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(15);
                CSVParsingD.instance.Setcsv(_csvFiles[9]);
                Dialogue_Start();
                break;
            
            //11
            case (int)stage.Sweet_room:
                Set_BGM(10);
                Current_stage = stage.Sweet_room;
                _current_buttons = _buttons[15];
                _location.text = "9F ����Ʈ��";
                Button_off();
                _current_buttons.SetActive(true);
                after_sweet = true;
                Dialogue_Manage.Instance.Change_back(10);
                CSVParsingD.instance.Setcsv(_csvFiles[27]);
                Dialogue_Start();
                break;

            //12
            case (int)stage.Casino:
                Set_BGM(7);
                Current_stage = stage.Casino;
                _location.text = "5F ī����";

                if (after_Party)
                {
                    _current_buttons = _buttons[6];
                    Button_off();
                    _current_buttons.SetActive(true);
                    Dialogue_Manage.Instance.Change_back(12);

                    if (casino_first)
                    {
                        casino_first = false;
                        P_stat.Coin_num = 20; //ī���� Ĩ ȹ�� (20��)
                        CSVParsingD.instance.Setcsv(_csvFiles[11]);
                        Dialogue_Start();
                    }
                    else
                    {
                        Dialogue_Manage.Instance.Dialouge_off(); //���� ���� ��簡 ���� ������ ������ �г��� ����
                    }
                    
                }
                else
                {
                    _current_buttons = _buttons[14];
                    Button_off();
                    _current_buttons.SetActive(true);
                    Dialogue_Manage.Instance.Change_back(12);
                    CSVParsingD.instance.Setcsv(_csvFiles[10]);
                    Dialogue_Start();
                }
                
                
                break;

            //13
            case (int)stage.Party:
                Set_BGM(8);
                Current_stage = stage.Party;
                after_Party = true;
                _location.text = "10F ��ȸ��";

                Check_ticket();

                if (is_ticket)
                {
                    _current_buttons = _buttons[7];
                    Button_off();
                    _current_buttons.SetActive(true);
                    Dialogue_Manage.Instance.Change_back(15);
                    if(Owner_count == 4)
                    {
                        Late_Owner_Event();
                    }
                    else
                    {
                        if (party_first)
                        {
                            party_first = false;
                            CSVParsingD.instance.Setcsv(_csvFiles[12]);
                            Dialogue_Start();
                        }
                        else
                        {
                            Dialogue_Manage.Instance.Dialouge_off(); //���� ���� ��簡 ���� ������ ������ �г��� ����
                        }
                    }
                }
                else
                {
                    _current_buttons = _buttons[13];
                    Button_off();
                    _current_buttons.SetActive(true);
                    Dialogue_Manage.Instance.Change_back(13);
                    CSVParsingD.instance.Setcsv(_csvFiles[13]);
                    Dialogue_Start();
                }
                
                
                break;

            //14
            case (int)stage.Store:
                Current_stage = stage.Store;
                _location.text = "B1F �����";
                Dialogue_Manage.Instance.Change_back(14);

                //��� ���Ķ��
                if (after_owner)
                {
                    Set_BGM(13);
                    _current_buttons = _buttons[18];
                    Button_off();
                    _current_buttons.SetActive(true);

                    if (store_after_first)
                    {
                        store_after_first = false;
                        CSVParsingD.instance.Setcsv(_csvFiles[23]);
                        Dialogue_Start();
                    }
                    else
                    {
                        Dialogue_Manage.Instance.Dialouge_off(); //���� ���� ��簡 ���� ������ ������ �г��� ����
                    }
                }
                //�ƴ϶��
                else
                {
                    Set_BGM(12);
                    _current_buttons = _buttons[8];
                    Button_off();
                    _current_buttons.SetActive(true);

                    if (store_first)
                    {
                        store_first = false;
                        CSVParsingD.instance.Setcsv(_csvFiles[22]);
                        Dialogue_Start();
                    }
                    else
                    {
                        Dialogue_Manage.Instance.Dialouge_off(); //���� ���� ��簡 ���� ������ ������ �г��� ����
                    }
                    
                }

                
                break;

            //15
            case (int)stage.Ending:
                Set_BGM(14);
                Current_stage = stage.Ending;
                _location.text = "��??";
                Button_off();
                Dialogue_Manage.Instance.Change_back(15);
                CSVParsingD.instance.Setcsv(_csvFiles[14]);
                Dialogue_Start();
                break;

            //16
            case (int)stage.Cook:
                Set_BGM(9);
                Current_stage = stage.Cook;
                _current_buttons = _buttons[17];
                _location.text = "3F �ֹ�";
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(16);
                CSVParsingD.instance.Setcsv(_csvFiles[21]);
                Dialogue_Start();
                break;

            //17
            case (int)stage.EV6F:
                Current_stage = stage.EV6F;
                _current_buttons = _buttons[10];
                _location.text = "6F ���������� Ȧ";
                Button_off();
                _current_buttons.SetActive(true);
                EV_Button_off();
                EV_index = 0;                               //���������� ��ư �ε��� ����
                Dialogue_Manage.Instance.Change_back(17);
                CSVParsingD.instance.Setcsv(_csvFiles[16]);
                Dialogue_Start();
                break;

            //18
            case (int)stage.EV4F:
                Set_SFX(6);
                EV_index = 1;
                EV_Button_off();
                Dialogue_Manage.Instance.Change_Subback(4);
                CSVParsingD.instance.Setcsv(_csvFiles[15]);
                Dialogue_Start();
                break;

            //19
            case (int)stage.EV2F:
                Set_SFX(6);
                EV_index = 2;
                EV_Button_off();
                Dialogue_Manage.Instance.Change_Subback(4);
                CSVParsingD.instance.Setcsv(_csvFiles[15]);
                Dialogue_Start();
                break;

            //20
            case (int)stage.EV4F_E:
                Set_SFX(6);
                EV_index = 3;
                EV_Button_off();
                Dialogue_Manage.Instance.Change_Subback(4);
                CSVParsingD.instance.Setcsv(_csvFiles[15]);
                Dialogue_Start();
                break;

            //21
            case (int)stage.EV4F_O:
                Set_SFX(6);
                EV_index = 4;
                EV_Button_off();
                Dialogue_Manage.Instance.Change_Subback(4);
                CSVParsingD.instance.Setcsv(_csvFiles[15]);
                Dialogue_Start();
                break;

            //22
            case (int)stage.EV_D5F_E:
                Set_SFX(6);
                EV_index = 5;
                EV_Button_off();
                Dialogue_Manage.Instance.Change_Subback(4);
                CSVParsingD.instance.Setcsv(_csvFiles[15]);
                Dialogue_Start();
                break;

            //23
            case (int)stage.EV_D5F_O:
                Set_SFX(6);
                EV_index = 6;
                EV_Button_off();
                Dialogue_Manage.Instance.Change_Subback(4);
                CSVParsingD.instance.Setcsv(_csvFiles[15]);
                Dialogue_Start();
                break;

            //24
            case (int)stage.EV_D1F_E:
                Set_SFX(6);
                EV_index = 7;
                EV_Button_off();
                Dialogue_Manage.Instance.Change_Subback(4);
                CSVParsingD.instance.Setcsv(_csvFiles[15]);
                Dialogue_Start();
                break;

            //25
            case (int)stage.EV_D1F_O:
                Set_SFX(6);
                EV_index = 8;
                EV_Button_off();
                Dialogue_Manage.Instance.Change_Subback(4);
                CSVParsingD.instance.Setcsv(_csvFiles[15]);
                Dialogue_Start();
                break;

            //26
            case (int)stage.EV1F_E:
                Set_SFX(6);
                EV_index = 9;
                EV_Button_off();
                Dialogue_Manage.Instance.Change_Subback(4);
                CSVParsingD.instance.Setcsv(_csvFiles[15]);
                Dialogue_Start();
                break;

            //27
            case (int)stage.EV1F_O:
                Set_SFX(6);
                EV_index = 10;
                EV_Button_off();
                Dialogue_Manage.Instance.Change_Subback(4);
                CSVParsingD.instance.Setcsv(_csvFiles[15]);
                Dialogue_Start();
                break;

            //28
            case (int)stage.Party_to_Cook:
                Set_BGM(11);
                Current_stage = stage.Party_to_Cook;
                _location.text = "3F �ֹ�";
                Button_off();
                Dialogue_Manage.Instance.Change_back(16);
                CSVParsingD.instance.Setcsv(_csvFiles[19]);
                Dialogue_Start();
                break;

            //29
            case (int)stage.Escape_Cook:
                Set_BGM(2);
                Current_stage = stage.Cook;
                _location.text = "3F �ֹ�";
                _current_buttons = _buttons[16];
                Button_off();
                _current_buttons.SetActive(true);
                Dialogue_Manage.Instance.Change_back(16);
                CSVParsingD.instance.Setcsv(_csvFiles[20]);
                Dialogue_Start();
                break;

            //30
            case (int)stage.Bad:
                Set_BGM(13);
                Dialogue_Manage.Instance.Change_back(14);
                Current_stage = stage.Bad;
                _location.text = "��??";
                Button_off();
                CSVParsingD.instance.Setcsv(_csvFiles[24]);
                Dialogue_Start();
                break;

            //31
            case (int)stage.Real:
                Set_BGM(13);
                Dialogue_Manage.Instance.Change_back(14);
                Current_stage = stage.Real;
                _location.text = "��??";
                Button_off();
                CSVParsingD.instance.Setcsv(_csvFiles[25]);
                Dialogue_Start();
                break;

            //32
            case (int)stage.Hidden:
                Set_BGM(13);
                Dialogue_Manage.Instance.Change_back(14);
                Current_stage = stage.Hidden;
                _location.text = "��??";
                Button_off();
                CSVParsingD.instance.Setcsv(_csvFiles[26]);
                Dialogue_Start();
                break;
        }
    }

    // deadend ���� (SoftReset��ư���� ����)
    public void SoftReset()
    {
        Siren_count = 0;
        Owner_count = 0;
        Chef_count = 0;
        Chef_success_count = 0;
        Siren_On = false;

        //is_ticket = false;
        casino_first = true;
        party_first = true;
        store_first = true;
        store_after_first = true;
        after_owner = false;
        after_sweet = false;

        Button_reset();
        Remove_Item(_currentItemIndex);
        Stage_start((int)Current_stage);
    }

    // ó������ (HardReset��ư���� ����)
    public void HardReset()
    {
        Siren_count = 0;
        Owner_count = 0;
        Chef_count = 0;
        Chef_success_count = 0;
        Siren_On = false;

        after_Party = false;
        is_ticket = false;
        casino_first = true;
        party_first = true;
        store_first = true;
        store_after_first = true;
        after_owner = false;
        after_sweet = false;

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
        Chef_count = 0;
        Chef_success_count = 0;
        Siren_On = false;

        after_Party = false;
        is_ticket = false;
        casino_first = true;
        party_first = true;
        store_first = true;
        store_after_first = true;
        after_owner = false;
        after_sweet = false;

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

        if (Manager_On)
        {
            Manager_Start = true;
            Manager_count++;
        }
    }

    // ���� ī��Ʈ ����(�ֹ� ��ư���� ����)
    public void Increase_chef()
    {
        Chef_count++;
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
            P_stat.Item_list[P_stat.C_Item_Index] = -1;
        }
    }

    private void Reset_Item()
    {
        for (int i = 0; i < P_stat.Item_list.Length; i++)
        {
            P_stat.Item_list[i] = -1;
        }
    }

    //���������� ��ư �ѱ�
    public void EV_ON(int num)
    {
        _evBtns[num].SetActive(true);
    }

    //�Ŵ��� �ѱ�
    public void Manager_ON()
    {
        Manager_On = true;
    }

    //Ƽ��Ȯ��
    private void Check_ticket()
    {
        is_ticket = false;

        //������ �Ǵ�
        for (int i = 0; i < P_stat.Item_list.Length; i++)
        {
            if(P_stat.Item_list[i] == 14)
            {
                is_ticket = true;
            }
        }
    }

    //������ ������� ���� ��ư�� ����
    public void Last_btn()
    {
        if(after_sweet)
        {
            Stage_start(31);
        }
        else
        {
            Stage_start(30);
        }
    }

    public void Set_BGM(int i)
    {
        _bgmPlayer.Pause();
        _bgmPlayer.clip = _bgmClips[i];
        _bgmPlayer.Play();
    }

    public void Set_SFX(int i)
    {
        _sfxPlayer.Pause();
        _sfxPlayer.clip = null;
        _sfxPlayer.clip = _sfxClips[i];
        _sfxPlayer.Play();
    }

    public void Stop_SFX()
    {
        _sfxPlayer.Stop();
    }

    public void Menual_On()
    {
        _menual.SetActive(true);
        P_stat.Is_menual = true;
    }
}
