using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_Button : Event_button
{
    public override void Play_Event()
    {
        //이벤트 다이얼로그 로드
        CSVParsingD.instance.Setcsv(Get_events(0));
        //다이얼로그 실행
        Dialogue_Manage.Instance.GetEventName("Example");
    }
}
