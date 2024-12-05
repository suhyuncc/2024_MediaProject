using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Event_button : MonoBehaviour
{
    [SerializeField]
    private int _eventCount; //버튼이 몇번 눌렸는지
    [SerializeField]
    private TextAsset[] _events; // 대사를 실행시킬 csv파일들

    private void Update()
    {
        if(_eventCount == _events.Length)
        {
            this.gameObject.SetActive(false);
        }
    }


    public virtual void Play_Event()
    {
        //이벤트 다이얼로그 로드
        CSVParsingD.instance.Setcsv(_events[_eventCount]);
        //다이얼로그 실행
        Dialogue_Manage.Instance.GetEventName("Example");
        _eventCount++;
    }

    // count초기화(deadend, hardreset에 사용)
    public virtual void CountReset()
    {
        _eventCount = 0;
    }

    public virtual void CountFull()
    {
        _eventCount = _events.Length;
    }

    public void Up_count(int count)
    {
        _eventCount += count;
    }

    public TextAsset Get_events(int count)
    {
        return _events[count];
    }

    public int Get_count()
    {
        return _eventCount;
    }

    public void Set_count(int i)
    {
        _eventCount = i;
    }
}
