using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_btn : Event_button
{
    [SerializeField]
    private Player_stat P_stat;     //플레이어 정보

    public override void Play_Event()
    {
        bool is_fire = false;
        //라이터 보유 판단
        for (int i = 0; i < P_stat.Item_list.Length; i++)
        {
            if (P_stat.Item_list[i] == 8)
            {
                is_fire = true;
            }
        }

        //아이템 판단
        if (is_fire)
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
            CountFull();
        }

    }
}
