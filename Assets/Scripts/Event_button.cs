using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Event_button : MonoBehaviour
{
    [SerializeField]
    private string _filename; // 각 버튼별로 하나하나 이벤트 이름 써줘야 함

    public void Play_Event()
    {
        //이벤트 다이얼로그 로드
        CSVParsingD.instance.Setcsv(Resources.Load<TextAsset>(_filename));
        //다이얼로그 실행
        Dialogue_Manage.Instance.GetEventName("Example");
    }
}
