using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cafe_Button : Event_button
{
    [SerializeField]
    private GameObject _drink;
    [SerializeField]
    private Dice_panel _dicePanel;

    private bool curse = true;
    public override void Play_Event()
    {
        //음료가 꺼져있다면
        if(!_drink.activeSelf && _dicePanel.is_success && curse)
        {
            //음료Death실행
            CSVParsingD.instance.Setcsv(Get_events(1));
            //다이얼로그 실행
            Dialogue_Manage.Instance.GetEventName("Example");
        }
        else
        {
            //기존 스크립트 진행
            CSVParsingD.instance.Setcsv(Get_events(0));
            //다이얼로그 실행
            Dialogue_Manage.Instance.GetEventName("Example");
        }

        GameManager.Instance.current_btn = this.gameObject;
    }

    public void Dis_curse()
    {
        curse = false;
    }
}
