using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dici : Event_button
{
    public override void Play_Event()
    {
        //�̺�Ʈ ���̾�α� �ε�
        CSVParsingD.instance.Setcsv(Get_events(Get_count()));
        //���̾�α� ����
        Dialogue_Manage.Instance.GetEventName("Example");
    }
}
