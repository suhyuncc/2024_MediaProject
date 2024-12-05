using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Event_button : MonoBehaviour
{
    [SerializeField]
    private int _eventCount; //��ư�� ��� ���ȴ���
    [SerializeField]
    private TextAsset[] _events; // ��縦 �����ų csv���ϵ�

    private void Update()
    {
        if(_eventCount == _events.Length)
        {
            this.gameObject.SetActive(false);
        }
    }


    public virtual void Play_Event()
    {
        //�̺�Ʈ ���̾�α� �ε�
        CSVParsingD.instance.Setcsv(_events[_eventCount]);
        //���̾�α� ����
        Dialogue_Manage.Instance.GetEventName("Example");
        _eventCount++;
    }

    // count�ʱ�ȭ(deadend, hardreset�� ���)
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
