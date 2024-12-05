using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changer_btn : Event_button
{
    [SerializeField]
    private Player_stat P_stat;     //플레이어 정보

    public override void Play_Event()
    {
        //아이템 판단
        if (P_stat.Coin_num > 100)
        {
            //이벤트 다이얼로그 로드
            CSVParsingD.instance.Setcsv(Get_events(0));
            //다이얼로그 실행
            Dialogue_Manage.Instance.GetEventName("Example");
            CountFull();
        }
        else
        {
            //이벤트 다이얼로그 로드
            CSVParsingD.instance.Setcsv(Get_events(1));
            //다이얼로그 실행
            Dialogue_Manage.Instance.GetEventName("Example");
        }

    }
}
