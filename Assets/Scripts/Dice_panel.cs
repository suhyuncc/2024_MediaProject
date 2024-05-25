using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InnerDriveStudios.DiceCreator;

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
    private string _next_Event; // 다음 대사

    public Text R_txt;

    public int resultCount;

    [SerializeField]
    private string _stat;
    private bool _diceDone;
    private int _dice;

    private void OnEnable()
    {
        _diceDone = false;
        _rollButton.SetActive(true);
        _returnButton.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        Print_result();
    }

    void Print_result()
    {
        int resultValue = 0;

        for (int i = 0; i < dies.Length; i++)
        {
            IRollResult result = dies[i].GetRollResult();

            //only show values if we are NOT rolling OR updating every frame
            resultValue +=
                (!dies[i].isRolling) ? int.Parse(result.valuesAsString) : int.Parse("0");
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
                _dice = resultValue;
                _returnButton.SetActive(true);
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
        }
    }

    public void SetEventName(string _eventName)
    {
        _next_Event = _eventName;
    }

    public void Roll_btn()
    {
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
        //계산
        switch (_stat)
        {
            case "san":
                P_stat.C_san -= _dice;
                break;
            case "str":
                break;
            case "luk":
                break;
            case "int":
                break;
            case "dex":
                break;
        }
        //다음 대사 실행
        Dialogue_Manage.Instance.GetEventName(_next_Event.ToString());
    }
}
