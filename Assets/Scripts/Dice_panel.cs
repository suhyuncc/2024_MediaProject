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
    private Sprite[] _resultIcons;
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private Image _resultIcon;
    [SerializeField]
    private GameObject _rollButton;
    [SerializeField]
    private GameObject _returnButton;
    [SerializeField]
    private string _next_Event; // 다음 대사
    [SerializeField]
    private string[] _next_Events; // 다음 대사 list
    [SerializeField]
    private Text _verdict;
    [SerializeField]
    private Text _StatTxt;

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

    [Header("MiniMap")]
    [SerializeField]
    private GameObject _miniMap;

    private bool _diceDone;
    private int _diceResult = 0;
    private int _playerSet_count;
    public int _cook_count = 0;

    private void OnEnable()
    {
        is_success = false;
        is_bigsuccess = false;
        _diceDone = false;

        _diceResult = 0;

        _returnButton.SetActive(false);
        Result_show.gameObject.SetActive(false);
        _rollButton.SetActive(true);

        if (_stat == "poker")
        {
            _bat.SetActive(true);
        }
        else
        {
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

            P_stat.Max_san = 90;
            P_stat.C_san = P_stat.Max_san;

            for(int i = 0; i < P_stat.Item_list.Length; i++)
            {
                P_stat.Item_list[i] = -1;
            }
            P_stat.C_Item_Index = 0;
            P_stat.Coin_num = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Player_set)
        {
            _verdict.text = "";
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
        else
        {
            //판정표 출력
            Verdict();
        }

        _battext.text = $"{(int)(P_stat.Coin_num * _batslider.value)}";

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
                R_txt.text = string.Format("{0}", resultValue);
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
                R_txt.text = string.Format("??");
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
        _resultIcon.gameObject.SetActive(true);

        switch (stat)
        {
            case "san":
                _icon.sprite = _icons[0];
                _resultIcon.sprite = _resultIcons[0];
                break;
            case "desan":
                _icon.sprite = _icons[0];
                _resultIcon.gameObject.SetActive(false);
                break;
            case "str":
                _icon.sprite = _icons[1];
                _resultIcon.sprite = _resultIcons[1];
                break;
            case "poker":
            case "luk":
                _icon.sprite = _icons[2];
                _resultIcon.sprite = _resultIcons[2];
                break;
            case "int":
                _icon.sprite = _icons[3];
                _resultIcon.sprite = _resultIcons[3];
                break;
            case "dex":
                _icon.sprite = _icons[4];
                _resultIcon.sprite = _resultIcons[4];
                break;
        }
    }

    private void Verdict()
    {
        switch (_stat)
        {
            case "san":
                _verdict.text = $"대성공 <= {P_stat.C_san / 4} \n" +
                    $"성공 <= {P_stat.C_san} \n" +
                    $"실패 > {P_stat.C_san} \n" +
                    $"대실패 = 98, 99, 100";
                _StatTxt.text = $"{P_stat.C_san}";
                break;

            case "str":
                _verdict.text = $"대성공 <= {P_stat.P_str / 4} \n" +
                    $"성공 <= {P_stat.P_str} \n" +
                    $"실패 > {P_stat.P_str} \n" +
                    $"대실패 = 98, 99, 100";
                _StatTxt.text = $"{P_stat.P_str}";
                break;

            case "poker":
            case "luk":
                _verdict.text = $"대성공 <= {P_stat.P_luk / 4} \n" +
                    $"성공 <= {P_stat.P_luk} \n" +
                    $"실패 > {P_stat.P_luk} \n" +
                    $"대실패 = 98, 99, 100";
                _StatTxt.text = $"{P_stat.P_luk}";
                break;

            case "int":
                _verdict.text = $"대성공 <= {P_stat.P_int / 4} \n" +
                    $"성공 <= {P_stat.P_int} \n" +
                    $"실패 > {P_stat.P_int} \n" +
                    $"대실패 = 98, 99, 100";
                _StatTxt.text = $"{P_stat.P_int}";
                break;

            case "dex":
                _verdict.text = $"대성공 <= {P_stat.P_dex / 4} \n" +
                    $"성공 <= {P_stat.P_dex} \n" +
                    $"실패 > {P_stat.P_dex} \n" +
                    $"대실패 = 98, 99, 100";
                _StatTxt.text = $"{P_stat.P_dex}";
                break;

            case "desan":
                _verdict.text = "";
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
            
            //미니맵 UI켜기
            _miniMap.SetActive(true);
            P_stat.Is_minimap = true;

            //다음 대사 실행
            Dialogue_Manage.Instance.GetEventName(_next_Events[0].ToString());
        }
        else
        {
            //계산
            switch (_stat)
            {
                case "desan":
                    P_stat.C_san -= _diceResult;
                    //다음 대사 실행
                    Dialogue_Manage.Instance.GetEventName(_next_Events[0].ToString());
                    break;

                case "poker":
                    //판돈은 잃고
                    P_stat.Coin_num -= Int32.Parse(_battext.text);

                    //대성공(d1)
                    if (is_bigsuccess)
                    {
                        //4배
                        P_stat.Coin_num += 4 * Int32.Parse(_battext.text);
                        Dialogue_Manage.Instance.GetEventName(_next_Events[0].ToString());
                    }
                    //성공(d2)
                    else if (is_success)
                    {
                        //2배
                        P_stat.Coin_num += 2 * Int32.Parse(_battext.text);
                        Dialogue_Manage.Instance.GetEventName(_next_Events[1].ToString());
                    }
                    //실패(d3)
                    else
                    {
                        Dialogue_Manage.Instance.GetEventName(_next_Events[2].ToString());
                    }
                    break;

                case "san":
                case "str":
                case "luk":
                case "int":
                case "dex":
                    //대성공(d1)
                    if (is_bigsuccess)
                    {
                        //일반 주방일 때
                        if (GameManager.Instance.Current_stage == stage.Cook)
                        {
                            GameManager.Instance.Chef_success_count++;
                        }

                        if (GameManager.Instance.Current_stage == stage.Party_to_Cook)
                        {
                            _cook_count = 0;
                            //탈출
                            GameManager.Instance.Stage_start((int)stage.Escape_Cook);
                        }
                        else
                        {
                            Dialogue_Manage.Instance.GetEventName(_next_Events[0].ToString());
                        }
                    }
                    //성공(d2)
                    else if(is_success)
                    {
                        //일반 주방일 때
                        if (GameManager.Instance.Current_stage == stage.Cook)
                        {
                            GameManager.Instance.Chef_success_count++;
                        }

                        if (GameManager.Instance.Current_stage == stage.Party_to_Cook)
                        {
                            if(_cook_count < 3)
                            {
                                _cook_count++;
                            }
                        }

                        if(_cook_count >= 3)
                        {
                            _cook_count = 0;
                            //탈출
                            GameManager.Instance.Stage_start((int)stage.Escape_Cook);
                        }
                        else
                        {
                            Dialogue_Manage.Instance.GetEventName(_next_Events[1].ToString());
                        }
                        
                    }
                    //실패(d3)
                    else
                    {
                        Dialogue_Manage.Instance.GetEventName(_next_Events[2].ToString());
                    }
                    break;
            }          
        }
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
                case "desan":
                    Result_show.gameObject.SetActive(false);
                    break;
                case "san":
                    if (_diceResult < P_stat.C_san / 4)
                    {
                        is_bigsuccess = true;
                        Result_show.text = "대성공!!";
                    }
                    else if (_diceResult < P_stat.C_san)
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
                case "str":
                    if (_diceResult < P_stat.P_str / 4)
                    {
                        is_bigsuccess = true;
                        Result_show.text = "대성공!!";
                    }
                    else if (_diceResult < P_stat.P_str)
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
                case "poker":
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
                    if (_diceResult < P_stat.P_dex / 4)
                    {
                        is_bigsuccess = true;
                        Result_show.text = "대성공!!";
                    }
                    else if (_diceResult < P_stat.P_dex)
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
            }
        }
        
    }
}
