using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casino_changer : Event_button
{
    [SerializeField]
    private Player_stat P_stat;

    public override void Play_Event()
    {
        //아이템 판단
        for (int i = 0; i < P_stat.Item_list.Length; i++)
        {
            //티켓 O
            if (P_stat.Item_list[i] == 14)
            {
                //이벤트 다이얼로그 로드
                CSVParsingD.instance.Setcsv(Get_events(2));
                //다이얼로그 실행
                Dialogue_Manage.Instance.GetEventName("Example");
                CountFull();
                return;
            }
        }

        if (P_stat.Coin_num >= 100)
        {
            //이벤트 다이얼로그 로드
            CSVParsingD.instance.Setcsv(Get_events(1));
            //다이얼로그 실행
            Dialogue_Manage.Instance.GetEventName("Example");
        }
        else
        {
            //이벤트 다이얼로그 로드
            CSVParsingD.instance.Setcsv(Get_events(0));
            //다이얼로그 실행
            Dialogue_Manage.Instance.GetEventName("Example");
        }

    }

}
