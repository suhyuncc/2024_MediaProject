using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casino_changer : Event_button
{
    [SerializeField]
    private Player_stat P_stat;

    public override void Play_Event()
    {
        if(Get_count() == 0)
        {
            base.Play_Event();
        }
        else
        {
            if(P_stat.Coin_num >= 100)
            {
                //이벤트 다이얼로그 로드
                CSVParsingD.instance.Setcsv(Get_events(1));
                //다이얼로그 실행
                Dialogue_Manage.Instance.GetEventName("Example");
            }
            else
            {
                //이벤트 다이얼로그 로드
                CSVParsingD.instance.Setcsv(Get_events(2));
                //다이얼로그 실행
                Dialogue_Manage.Instance.GetEventName("Example");
            }
        }
    }
}
