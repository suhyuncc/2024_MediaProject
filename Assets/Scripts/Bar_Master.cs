using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar_Master : Event_button
{
    [SerializeField]
    private Dice_panel _dicePanel;
    [SerializeField]
    private GameObject _drink;
    [SerializeField]
    private GameObject _book;

    public override void Play_Event()
    {
        if (!_book.activeSelf)
        {
            if(_dicePanel.is_bigsuccess || _dicePanel.is_success)
            {
                //이벤트 다이얼로그 로드
                CSVParsingD.instance.Setcsv(Get_events(3));
                //다이얼로그 실행
                Dialogue_Manage.Instance.GetEventName("Example");
            }
            else
            {
                //이벤트 다이얼로그 로드
                CSVParsingD.instance.Setcsv(Get_events(4));
                //다이얼로그 실행
                Dialogue_Manage.Instance.GetEventName("Example");
            }
        }
        else
        {
            base.Play_Event();
        }
        
        this.gameObject.SetActive(false);
    }

    public void Drink_btn()
    {
        Up_count(1);
    }

    public void Book_btn()
    {
        Up_count(2);
    }
}
