using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBox : MonoBehaviour
{
    [SerializeField]
    private Dice_panel _dice_Panel;
    [SerializeField]
    private string _checkStat; // 대상 스탯
    [SerializeField]
    private string _diceName; // 주사위 종류
    [SerializeField]
    private string[] _next_Events; // 다음 대사


    public void SetCheckStat(string checkstat)
    {
        _checkStat = checkstat;
    }

    public void SetDiceName(string diceName)
    {
        _diceName = diceName;
    }

    public void SetEventName(string[] _eventName)
    {
        _next_Events = _eventName;
    }

    

    public void Go_dice()
    {
        if (_dice_Panel.Player_set)
        {
            // 다음 대사 넘기기
            _dice_Panel.SetEventName(_next_Events);

            // 화면전환과 동시에 주사위 세팅
            GameManager.Instance.Dice_On(_diceName);
        }
        else if (_checkStat == "poker")
        {
            GameManager.Instance.Play_Casino();

            CardGameManager.Instance.SetEventName(_next_Events[0]);
        }
        else if (_checkStat == "rush")
        {
            GameManager.Instance.Play_Rush();

            R_H_Manager.instance.SetEventName(_next_Events);
        }
        else
        {
            // 아이콘 세팅
            _dice_Panel.Set_icon(_checkStat);

            // 다음 대사 넘기기
            _dice_Panel.SetEventName(_next_Events);

            // 화면전환과 동시에 주사위 세팅
            GameManager.Instance.Dice_On(_diceName);
        }

        for (int i = 0; i < this.transform.parent.childCount; i++)
        {
            //모든 선택창 감추기
            this.transform.parent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
