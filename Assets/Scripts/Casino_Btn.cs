using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casino_Btn : Event_button
{
    public override void Play_Event()
    {
        if (Get_count() == 0)
        {
            base.Play_Event();
        }
        else 
        {
            //이벤트 다이얼로그 로드
            CSVParsingD.instance.Setcsv(Get_events(Get_count()));
            //다이얼로그 실행
            Dialogue_Manage.Instance.GetEventName("Example");
        }
        
    }
}
