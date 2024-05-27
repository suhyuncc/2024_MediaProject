using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InnerDriveStudios.DiceCreator;
using System;

public class Dice_panel : MonoBehaviour
{
    [SerializeField]
    private Player_stat P_stat;
    [SerializeField]
    private ARollable[] dies;
    [SerializeField]
    private Sprite[] _icons;
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private GameObject _rollButton;
    [SerializeField]
    private GameObject _returnButton;
    [SerializeField]
    private GameObject _diciButton;
    [SerializeField]
    private string _next_Event; // 다음 대사
    [SerializeField]
    private string[] _next_Events; // 다음 대사 list

    public Text R_txt;
    public Text Result_show;
    public Text Title;

    public int resultCount;

    public bool is_success;
    public bool is_bigsuccess;
    public bool Player_set;
    [SerializeField]
    private string _stat;

    [Header("Bat")]
    [SerializeField]
    private GameObject _bat;
    [SerializeField]
    private Text _battext;
    [SerializeField]
    private Slider _batslider;

    private bool _diceDone;
    private int _diceResult = 0;
    private int _playerSet_count;

    private bool _choiceOdd;
    private bool _choiceEven;
    private bool _resutOdd;
    private bool _resutEven;
    private void OnEnable()
    {
        is_success = false;
        is_bigsuccess = false;
        _choiceOdd = false;
        _choiceEven = false;
        _resutOdd = false;
        _resutEven = false;
        _diceDone = false;

        _diceResult = 0;

        _returnButton.SetActive(false);
        Result_show.gameObject.SetActive(false);

        if(_stat == "dici")
        {
            _rollButton.SetActive(false);
            _diciButton.SetActive(true);
            _bat.SetActive(true);
        }
        else
        {
            _rollButton.SetActive(true);
            _diciButton.SetActive(false);
            _bat.SetActive(false);
        }

        //플레이어 세팅 모드일때 스탯 초기화
        if (Player_set)
        {
            Title.gameObject.SetActive(true);

            _playerSet_count = 0;

            P_stat.P_str = 0;
            P_stat.P_luk = 0;
            P_stat.P_int = 0;
            P_stat.P_dex = 0;

            P_stat.Max_san = 50;
            P_stat.C_san = P_stat.Max_san;

            for(int i = 0; i < P_stat.Item_list.Length; i++)
            {
                P_stat.Item_list[i] = 0;
            }
            P_stat.C_Item_Index = 0;
            P_stat.Coin_num = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Player_set)
        {
            switch (_playerSet_count)
            {
                case 1:
                    _icon.sprite = _icons[1];
                    break;
                case 2:
                    _icon.sprite = _icons[2];
                    break;
                case 3:
                    _icon.sprite = _icons[3];
                    break;
                case 4:
                    _icon.sprite = _icons[4];
                    break;
                default:
                    _icon.sprite = _icons[0];
                    break;
            }
        }

        if(_stat == "dici")
        {
            _battext.text = $"{(int)(P_stat.Coin_num * _batslider.value)}";
        }
        Print_result();
    }

    void Print_result()
    {
        int resultValue = 0;

        for (int i = 0; i < dies.Length; i++)
        {
            if (dies[i].gameObject.activeSelf)
            {
                IRollResult result = dies[i].GetRollResult();

                //only show values if we are NOT rolling OR updating every frame
                resultValue +=
                    (!dies[i].isRolling) ? int.Parse(result.valuesAsString) : int.Parse("0");
            }

            
        }

        for (int i = 0; i < dies.Length; i++)
        {
            if (!dies[i].isRolling)
            {
                resultCount++;
            }
            else
            {
                resultCount = 0;
            }
        }

        if (resultCount == dies.Length)
        {
            
            resultCount = 0;
            if(_diceDone)
            {
                R_txt.text = string.Format("결과 = {0}", resultValue);
                _diceResult = resultValue;

                if (Player_set)
                {
                    if(_playerSet_count == 4)
                    {
                        _returnButton.SetActive(true);
                    }
                    else
                    {
                        _rollButton.SetActive(true);
                    }
                }
                else
                {
                    _returnButton.SetActive(true);
                }
                
                Show_Text();
            }
            else
            {
                R_txt.text = string.Format("결과 = ??");
            }
        }
        else
        {
            
            resultCount = 0;
        }

    }

    //아이콘 이미지 변경
    public void Set_icon(string stat)
    {
        _stat = stat;

        switch (stat)
        {
            case "san":
            case "san_int":
                _icon.sprite = _icons[0];
                break;
            case "str":
                _icon.sprite = _icons[1];
                break;
            case "luk":
                _icon.sprite = _icons[2];
                break;
            case "int":
                _icon.sprite = _icons[3];
                break;
            case "dex":
                _icon.sprite = _icons[4];
                break;
            case "dici":
                _icon.sprite = _icons[0];
                _diciButton.SetActive(true);
                break;
        }
    }

    public void SetEventName(string[] _eventName)
    {
        _next_Events = _eventName;
    }

    public void Roll_btn()
    {
        if (Player_set)
        {
            _playerSet_count++;
        }

        for (int i = 0; i < dies.Length; i++)
        {
            if (!dies[i].isRolling && dies[i].gameObject.activeSelf)
            {
                dies[i].Roll();
            }
        }
        _diceDone = true;
    }

    public void Dice_check()
    {
        // 플레이어 세팅 모드 해제
        if (Player_set)
        {
            Title.gameObject.SetActive(false);
            Player_set = false;
            _playerSet_count = 0;
            GameManager.Instance.Stage_start(0);
        }
        else
        {
            //계산
            switch (_stat)
            {
                case "san":
                    P_stat.C_san -= _diceResult;
                    //다음 대사 실행
                    Dialogue_Manage.Instance.GetEventName(_next_Events[0].ToString());
                    break;
                case "san_int":
                    P_stat.C_san -= _diceResult;
                    P_stat.P_int -= _diceResult;
                    //다음 대사 실행
                    Dialogue_Manage.Instance.GetEventName(_next_Events[0].ToString());
                    break;
                case "str":
                case "luk":
                case "int":
                case "dex":
                    //대성공(d2)
                    if (is_bigsuccess)
                    {
                        Dialogue_Manage.Instance.GetEventName(_next_Events[1].ToString());
                    }
                    //성공(d1)
                    else if(is_success)
                    {
                        Dialogue_Manage.Instance.GetEventName(_next_Events[0].ToString());
                    }
                    //실패(d3)
                    else
                    {
                        Dialogue_Manage.Instance.GetEventName(_next_Events[2].ToString());
                    }
                    break;
                case "dici":
                    //판돈은 잃고
                    P_stat.Coin_num -= Int32.Parse(_battext.text);

                    if (is_success)
                    {
                        P_stat.Coin_num += 2 * Int32.Parse(_battext.text);
                        Dialogue_Manage.Instance.GetEventName(_next_Events[0].ToString());
                    }
                    else
                    {
                        Dialogue_Manage.Instance.GetEventName(_next_Events[1].ToString());
                    }
                    
                    break;
            }

            
        }
    }

    //홀 버튼
    public void Odd()
    {
        _choiceOdd = true;
    }

    //짝 버튼
    public void Even()
    {
        _choiceEven = true;
    }

    private void Show_Text()
    {
        if (Player_set)
        {
            Result_show.gameObject.SetActive(true);
            switch (_playerSet_count)
            {
                case 1:
                    Result_show.text = $"근력 = {(_diceResult + 6) * 5}";
                    P_stat.P_str = (_diceResult + 6) * 5;
                    break;
                case 2:
                    Result_show.text = $"행운 = {(_diceResult + 6) * 5}";
                    P_stat.P_luk = (_diceResult + 6) * 5;
                    break;
                case 3:
                    Result_show.text = $"지능 = {(_diceResult + 6) * 5}";
                    P_stat.P_int = (_diceResult + 6) * 5;
                    break;
                case 4:
                    Result_show.text = $"민첩 = {(_diceResult + 6) * 5}";
                    P_stat.P_dex = (_diceResult + 6) * 5;
                    break;
                default:
                    break;
            }
        }
        else
        {
            Result_show.gameObject.SetActive(true);
            switch (_stat)
            {
                case "san":
                    Result_show.gameObject.SetActive(false);
                    break;
                case "str":
                    break;
                case "luk":
                    if (_diceResult < P_stat.P_luk / 4)
                    {
                        is_bigsuccess = true;
                        Result_show.text = "대성공!!";
                    }
                    else if (_diceResult < P_stat.P_luk)
                    {
                        is_success = true;
                        Result_show.text = "성공!!";
                    }
                    else
                    {
                        is_success = false;
                        Result_show.text = "실패...";
                    }
                    break;
                case "int":
                    if(_diceResult < P_stat.P_int / 4)
                    {
                        is_bigsuccess = true;
                        Result_show.text = "대성공!!";
                    }
                    else if(_diceResult < P_stat.P_int)
                    {
                        is_success = true;
                        Result_show.text = "성공!!";
                    }
                    else
                    {
                        is_success = false;
                        Result_show.text = "실패...";
                    }
                    break;
                case "dex":
                    break;
                case "dici":
                    

                    //홀,짝 판별
                    if (_diceResult % 2 == 0)
                    {
                        _resutEven = true;
                    }
                    else
                    {
                        _resutOdd = true;
                    }

                    //최종 결과 판별
                    if (_resutOdd && _choiceOdd || _resutEven && _choiceEven)
                    {
                        is_success = true;
                        Result_show.text = "성공!!";
                    }
                    else
                    {
                        is_success = false;
                        Result_show.text = "실패...";
                    }
                    break;
                default:
                    Result_show.gameObject.SetActive(false);
                    break;
            }
        }
        
    }
}
